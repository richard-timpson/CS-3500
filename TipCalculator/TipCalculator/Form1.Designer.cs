namespace TipCalculator
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.computeButton = new System.Windows.Forms.Button();
      this.billLabel = new System.Windows.Forms.Label();
      this.billField = new System.Windows.Forms.TextBox();
      this.tipField = new System.Windows.Forms.TextBox();
      this.tipPercentLabel = new System.Windows.Forms.Label();
      this.tipPercentField = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // computeButton
      // 
      this.computeButton.Enabled = false;
      this.computeButton.Location = new System.Drawing.Point(48, 256);
      this.computeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.computeButton.Name = "computeButton";
      this.computeButton.Size = new System.Drawing.Size(114, 36);
      this.computeButton.TabIndex = 0;
      this.computeButton.Text = "Compute Tip";
      this.computeButton.UseVisualStyleBackColor = true;
      this.computeButton.Click += new System.EventHandler(this.computeButton_Click);
      // 
      // billLabel
      // 
      this.billLabel.AutoSize = true;
      this.billLabel.Location = new System.Drawing.Point(48, 97);
      this.billLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.billLabel.Name = "billLabel";
      this.billLabel.Size = new System.Drawing.Size(111, 20);
      this.billLabel.TabIndex = 1;
      this.billLabel.Text = "Enter Total Bill";
      // 
      // billField
      // 
      this.billField.Location = new System.Drawing.Point(250, 91);
      this.billField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.billField.Name = "billField";
      this.billField.Size = new System.Drawing.Size(124, 26);
      this.billField.TabIndex = 2;
      this.billField.TextChanged += new System.EventHandler(this.billField_TextChanged);
      // 
      // tipField
      // 
      this.tipField.Location = new System.Drawing.Point(250, 267);
      this.tipField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.tipField.Name = "tipField";
      this.tipField.ReadOnly = true;
      this.tipField.Size = new System.Drawing.Size(124, 26);
      this.tipField.TabIndex = 3;
      // 
      // tipPercentLabel
      // 
      this.tipPercentLabel.AutoSize = true;
      this.tipPercentLabel.Location = new System.Drawing.Point(52, 165);
      this.tipPercentLabel.Name = "tipPercentLabel";
      this.tipPercentLabel.Size = new System.Drawing.Size(44, 20);
      this.tipPercentLabel.TabIndex = 4;
      this.tipPercentLabel.Text = "Tip%";
      // 
      // tipPercentField
      // 
      this.tipPercentField.Location = new System.Drawing.Point(250, 158);
      this.tipPercentField.Name = "tipPercentField";
      this.tipPercentField.Size = new System.Drawing.Size(124, 26);
      this.tipPercentField.TabIndex = 5;
      this.tipPercentField.Text = "15";
      this.tipPercentField.TextChanged += new System.EventHandler(this.tipPercentField_TextChanged);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(475, 343);
      this.Controls.Add(this.tipPercentField);
      this.Controls.Add(this.tipPercentLabel);
      this.Controls.Add(this.tipField);
      this.Controls.Add(this.billField);
      this.Controls.Add(this.billLabel);
      this.Controls.Add(this.computeButton);
      this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button computeButton;
    private System.Windows.Forms.Label billLabel;
    private System.Windows.Forms.TextBox billField;
    private System.Windows.Forms.TextBox tipField;
    private System.Windows.Forms.Label tipPercentLabel;
    private System.Windows.Forms.TextBox tipPercentField;
  }
}

