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
            public delegate void callMe(SocketState ss);
            public SocketState.callMe _call;
            public int ID { get;  set; }
            public ListenerState(SocketState.callMe callMeCallBack, TcpListener _listener, int _id)
            {
                listener = _listener;
                _call = callMeCallBack;
                ID = _id;
            }
        }


        public class NetworkController
        {
            public delegate void ErrorHandler(string message);
            public static event ErrorHandler Error;
            public const int DEFAULT_PORT = 11000;
            private static int clientCounter = 0;


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
                    Error(e.Message);
                    throw e;
                }
            }
            public static void ServerAwaitingClientLoop(SocketState.callMe callMe, int ID)
            {
                int port = 11000;
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                ListenerState ls = new ListenerState(callMe, listener, ID);
                listener.BeginAcceptSocket(AcceptNewClient, ls);

            }
            public static void AcceptNewClient(IAsyncResult ar)
            {
                ListenerState ls = (ListenerState)ar.AsyncState;
               
                Socket socket = ls.listener.EndAcceptSocket(ar);
                SocketState ss = new SocketState(socket, ls._call, ls.ID);
                ss._call(ss);
                ls.ID++;
                ls.listener.BeginAcceptSocket(AcceptNewClient, ls);
            }

            // TODO: Move all networking code to this class. Left as an exercise.
            // Networking code should be completely general-purpose, and useable by any other application.
            // It should contain no references to a specific project.
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
            /// Move this function to a standalone networking library.
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
                    Error(e.Message);
                }

            }

            /// <summary>
            /// This function is "called" by the operating system when data arrives on the socket
            /// Move this function to a standalone networking library. 
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
                        Error("Stopped getting data from server");
                        ss.theSocket.Close();
                    }
                }
                catch (Exception e)
                {
                    ss.Connected = false;
                    ss.theSocket.Close();
                    //Error(e.Message); 
                    throw e;
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
                s.EndSend(ar);
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
                ss.theSocket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, ss.theSocket);
            }

            /// <summary>
            /// Calls BeginReceive to get data from the server.
            /// </summary>
            /// <param name="ss"></param>
            public static void GetData(Networking.SocketState ss)
            {
                ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length, SocketFlags.None, ReceiveCallback, ss);
            }
        }
    }
}
