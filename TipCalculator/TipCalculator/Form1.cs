using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipCalculator
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private double computeTip(double bill, double tipPercent)
    {
      return bill * (tipPercent / 100);
    }

    private void showTip(double tip)
    {
      tipField.Text = tip.ToString();
    }

    private void computeButton_Click(object sender, EventArgs e)
    {
      double tip = computeTip(Double.Parse(billField.Text), Double.Parse(tipPercentField.Text));
      showTip(tip);
    }

    private void billField_TextChanged(object sender, EventArgs e)
    {
      checkInputSanity();
    }

    private void tipPercentField_TextChanged(object sender, EventArgs e)
    {
      checkInputSanity();
    }

    private void checkInputSanity()
    {
      if (!Double.TryParse(billField.Text, out double unused) ||
        !Double.TryParse(tipPercentField.Text, out double unused2))
        computeButton.Enabled = false;
      else
        computeButton.Enabled = true;
    }

  }
}
