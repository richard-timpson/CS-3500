using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace NetworkController
{

    public class Networking
    {

        /// <summary>
        /// This class holds all the necessary state to represent a socket connection
        /// Note that all of its fields are public because we are using it like a "struct"
        /// It is a simple collection of fields
        /// </summary>
        public class SocketState
        {
            public Socket theSocket;

            public delegate void ErrorHandler(string message);

            public delegate void callMe(SocketState ss);

            public int ID { get; private set; }

            public callMe _call;
            public ErrorHandler handleError;
            // This is the buffer where we will receive data from the socket
            public byte[] messageBuffer = new byte[4000];

            // This is a larger (growable) buffer, in case a single receive does not contain the full message.
            public StringBuilder sb = new StringBuilder();

            public bool Connected { get; set; }

            /// <summary>
            /// Constructor for the socketState
            /// </summary>
            /// <param name="s"></param>
            /// <param name="id"></param>
            /// <param name="callMeCallBack"></param>
            public SocketState(Socket s, callMe callMeCallBack, int _id)
            {
                theSocket = s;
                _call = callMeCallBack;
                ID = _id;
            }
        }
        public class ListenerState
        {
            public TcpListener listener;
            public SocketState.callMe _call;
            public int ID { get; set; }
            public ListenerState(SocketState.callMe callMeCallBack, TcpListener _listener, int _id)
            {
                listener = _listener;
                _call = callMeCallBack;
                ID = _id;
            }
        }


        public class NetworkController
        {
            public delegate void DisconnectErrorHandler(SocketState ss, string message);
            public static event DisconnectErrorHandler DisconnectError;
            public const int DEFAULT_PORT = 11000;


            /// <summary>
            /// Creates a Socket object for the given host string
            /// </summary>
            /// <param name="hostName">The host name or IP address</param>
            /// <param name="socket">The created Socket</param>
            /// <param name="ipAddress">The created IPAddress</param>
            public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
            {
                ipAddress = IPAddress.None;
                socket = null;
                try
                {
                    // Establish the remote endpoint for the socket.
                    IPHostEntry ipHostInfo;

                    // Determine if the server address is a URL or an IP
                    try
                    {
                        ipHostInfo = Dns.GetHostEntry(hostName);
                        bool foundIPV4 = false;
                        foreach (IPAddress addr in ipHostInfo.AddressList)
                            if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                            {
                                foundIPV4 = true;
                                ipAddress = addr;
                                break;
                            }
                        // Didn't find any IPV4 addresses
                        if (!foundIPV4)
                        {
                            System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                            throw new ArgumentException("Invalid IP");
                        }
                    }
                    catch (Exception)
                    {
                        // see if host name is actually an ipaddress, i.e., 155.99.123.456
                        System.Diagnostics.Debug.WriteLine("using IP");
                        ipAddress = IPAddress.Parse(hostName);
                    }

                    // Create a TCP/IP socket.
                    socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                    // Disable Nagle's algorithm - can speed things up for tiny messages, 
                    // such as for a game
                    socket.NoDelay = true;

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                    //DisconnectError(ss, e.Message);
                    throw e;
                }
            }
            /// <summary>
            /// The function that triggers the event loop for client connections
            /// </summary>
            /// <param name="callMe"></param>
            /// <param name="ID"></param>
            public static void ServerAwaitingClientLoop(SocketState.callMe callMe, int ID)
            {
                int port = 11000;
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                ListenerState ls = new ListenerState(callMe, listener, ID);
                listener.BeginAcceptSocket(AcceptNewClient, ls);

            }
            /// <summary>
            /// The function that continues the event loop for each new client connection
            /// </summary>
            /// <param name="ar"></param>
            public static void AcceptNewClient(IAsyncResult ar)
            {
                ListenerState ls = (ListenerState)ar.AsyncState;
                Socket socket = ls.listener.EndAcceptSocket(ar);
                SocketState ss = new SocketState(socket, ls._call, ls.ID);
                ss._call(ss);
                ls.ID++;
                ls.listener.BeginAcceptSocket(AcceptNewClient, ls);
            }

            /// <summary>
            /// Function used to connect view to server. 
            /// </summary>
            /// <param name="hostName"></param>
            /// <param name="cb"></param>
            /// <returns></returns>
            public static Socket ConnectToServer(string hostName, SocketState.callMe cb)
            {
                System.Diagnostics.Debug.WriteLine("connecting  to " + hostName);

                // Create a TCP/IP socket.
                Socket socket;
                IPAddress ipAddress;

                NetworkController.MakeSocket(hostName, out socket, out ipAddress);

                SocketState ss = new SocketState(socket, cb, -1);

                socket.BeginConnect(ipAddress, NetworkController.DEFAULT_PORT, ConnectedCallback, ss);


                return socket;

            }

            /// <summary>
            /// This function is "called" by the operating system when the remote site acknowledges the connect request
            /// </summary>
            /// <param name="ar"></param>
            private static void ConnectedCallback(IAsyncResult ar)
            {
                SocketState ss = (SocketState)ar.AsyncState;

                // Complete the connection.
                try
                {
                    ss.theSocket.EndConnect(ar);
                    ss._call(ss);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            }

            /// <summary>
            /// This function is "called" by the operating system when data arrives on the socket
            /// </summary>
            /// <param name="ar"></param>
            private static void ReceiveCallback(IAsyncResult ar)
            {
                SocketState ss = (SocketState)ar.AsyncState;
                ss.Connected = true;
                int bytesRead = 0;
                try
                {
                    bytesRead = ss.theSocket.EndReceive(ar);
                    // If the socket is still open
                    if (bytesRead > 0)
                    {
                        string theMessage = Encoding.UTF8.GetString(ss.messageBuffer, 0, bytesRead);
                        // Append the received data to the growable buffer.
                        // It may be an incomplete message, so we need to start building it up piece by piece
                        ss.sb.Append(theMessage);
                        ss._call(ss);

                    }
                    else
                    {
                        DisconnectError(ss, "Stopped getting data from server");
                        ss.theSocket.Close();
                    }
                }
                catch (Exception e)
                {
                    // set the connection to false to
                    ss.Connected = false;

                    // trigger a disconnect event that passes in a socket state. Calling it here because this seems to be where
                    // exceptions are thrown after a disconnect. 
                    DisconnectError(ss, e.Message);

                }

            }
            /// <summary>
            /// A callback invoked when a send operation completes
            /// Move this function to a standalone networking library. 
            /// </summary>
            /// <param name="ar"></param>
            private static void SendCallback(IAsyncResult ar)
            {
                Socket s = (Socket)ar.AsyncState;
                try
                {
                    s.EndSend(ar);
                }
                catch (SocketException E)
                {
                    System.Diagnostics.Debug.WriteLine(E.Message);
                    Console.WriteLine(E.Message);
                }
            }

            /// <summary>
            /// Sends messages to server.
            /// </summary>
            /// <param name="message"></param>
            /// <param name="ss"></param>
            public static void Send(string message, Networking.SocketState ss)
            {

                // Append a newline, since that is our protocol's terminating character for a message.
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                try
                {
                    ss.theSocket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, ss.theSocket);
                }
                catch (SocketException E)
                {
                    System.Diagnostics.Debug.WriteLine(E.Message);
                }
            }

            /// <summary>
            /// Calls BeginReceive to get data from the server.
            /// </summary>
            /// <param name="ss"></param>
            public static void GetData(Networking.SocketState ss)
            {
                // not wrapping this in a try catch because for some reason the exceptions always throw in the receive callback. 
                ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length, SocketFlags.None, ReceiveCallback, ss);
            }
        }
    }
}
