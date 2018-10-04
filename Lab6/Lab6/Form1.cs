using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TipComputeButton_Click(object sender, EventArgs e)
        {
            double InitialValue = double.Parse(InputBox.Text);
            double ComputedValue = InitialValue * .2;
            string ComputedValueString = ComputedValue.ToString();

            OutputBox.Text = ComputedValueString;

            double CheckValue = double.Parse(CheckBox.Text);
            double PercentValue = double.Parse(PercentBox.Text);

            PercentValue = PercentValue * .01;

            ComputedValue = CheckValue * PercentValue;

            OutputBox.Text = ComputedValue.ToString();
        }
    }
}
