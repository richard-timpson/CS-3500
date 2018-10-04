namespace Lab6
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
            this.TotalBillLabel = new System.Windows.Forms.Label();
            this.TipComputeButton = new System.Windows.Forms.Button();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.OutputBox = new System.Windows.Forms.TextBox();
            this.PercentBox = new System.Windows.Forms.TextBox();
            this.PercentageLabel = new System.Windows.Forms.Label();
            this.CheckLabel = new System.Windows.Forms.Label();
            this.CheckBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TotalBillLabel
            // 
            this.TotalBillLabel.AutoSize = true;
            this.TotalBillLabel.Location = new System.Drawing.Point(114, 91);
            this.TotalBillLabel.Name = "TotalBillLabel";
            this.TotalBillLabel.Size = new System.Drawing.Size(152, 25);
            this.TotalBillLabel.TabIndex = 0;
            this.TotalBillLabel.Text = "Enter Total Bill";
            this.TotalBillLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // TipComputeButton
            // 
            this.TipComputeButton.Location = new System.Drawing.Point(119, 278);
            this.TipComputeButton.Name = "TipComputeButton";
            this.TipComputeButton.Size = new System.Drawing.Size(192, 44);
            this.TipComputeButton.TabIndex = 1;
            this.TipComputeButton.Text = "Compute Tip";
            this.TipComputeButton.UseVisualStyleBackColor = true;
            this.TipComputeButton.Click += new System.EventHandler(this.TipComputeButton_Click);
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(342, 88);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(251, 31);
            this.InputBox.TabIndex = 2;
            // 
            // OutputBox
            // 
            this.OutputBox.Location = new System.Drawing.Point(342, 291);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.Size = new System.Drawing.Size(251, 31);
            this.OutputBox.TabIndex = 3;
            // 
            // PercentBox
            // 
            this.PercentBox.Location = new System.Drawing.Point(342, 146);
            this.PercentBox.Name = "PercentBox";
            this.PercentBox.Size = new System.Drawing.Size(251, 31);
            this.PercentBox.TabIndex = 4;
            // 
            // PercentageLabel
            // 
            this.PercentageLabel.AutoSize = true;
            this.PercentageLabel.Location = new System.Drawing.Point(114, 152);
            this.PercentageLabel.Name = "PercentageLabel";
            this.PercentageLabel.Size = new System.Drawing.Size(122, 25);
            this.PercentageLabel.TabIndex = 5;
            this.PercentageLabel.Text = "Tip Percent";
            // 
            // CheckLabel
            // 
            this.CheckLabel.AutoSize = true;
            this.CheckLabel.Location = new System.Drawing.Point(119, 218);
            this.CheckLabel.Name = "CheckLabel";
            this.CheckLabel.Size = new System.Drawing.Size(127, 25);
            this.CheckLabel.TabIndex = 6;
            this.CheckLabel.Text = "Check Total";
            // 
            // CheckBox
            // 
            this.CheckBox.Location = new System.Drawing.Point(342, 218);
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.Size = new System.Drawing.Size(251, 31);
            this.CheckBox.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CheckBox);
            this.Controls.Add(this.CheckLabel);
            this.Controls.Add(this.PercentageLabel);
            this.Controls.Add(this.PercentBox);
            this.Controls.Add(this.OutputBox);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.TipComputeButton);
            this.Controls.Add(this.TotalBillLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TotalBillLabel;
        private System.Windows.Forms.Button TipComputeButton;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.TextBox OutputBox;
        private System.Windows.Forms.TextBox PercentBox;
        private System.Windows.Forms.Label PercentageLabel;
        private System.Windows.Forms.Label CheckLabel;
        private System.Windows.Forms.TextBox CheckBox;
    }
}

