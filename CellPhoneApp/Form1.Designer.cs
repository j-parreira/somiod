namespace CellPhoneApp
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
            this.groupBoxSecurity = new System.Windows.Forms.GroupBox();
            this.labelContainer = new System.Windows.Forms.Label();
            this.labelApplication = new System.Windows.Forms.Label();
            this.comboBoxApplication = new System.Windows.Forms.ComboBox();
            this.comboBoxContainer = new System.Windows.Forms.ComboBox();
            this.buttonOn = new System.Windows.Forms.Button();
            this.buttonOff = new System.Windows.Forms.Button();
            this.groupBoxSecurity.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSecurity
            // 
            this.groupBoxSecurity.BackColor = System.Drawing.Color.OldLace;
            this.groupBoxSecurity.Controls.Add(this.buttonOff);
            this.groupBoxSecurity.Controls.Add(this.buttonOn);
            this.groupBoxSecurity.Controls.Add(this.comboBoxContainer);
            this.groupBoxSecurity.Controls.Add(this.comboBoxApplication);
            this.groupBoxSecurity.Controls.Add(this.labelContainer);
            this.groupBoxSecurity.Controls.Add(this.labelApplication);
            this.groupBoxSecurity.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSecurity.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSecurity.Name = "groupBoxSecurity";
            this.groupBoxSecurity.Size = new System.Drawing.Size(407, 415);
            this.groupBoxSecurity.TabIndex = 8;
            this.groupBoxSecurity.TabStop = false;
            this.groupBoxSecurity.Text = "Smart Home";
            // 
            // labelContainer
            // 
            this.labelContainer.AutoSize = true;
            this.labelContainer.Location = new System.Drawing.Point(22, 129);
            this.labelContainer.Name = "labelContainer";
            this.labelContainer.Size = new System.Drawing.Size(93, 24);
            this.labelContainer.TabIndex = 3;
            this.labelContainer.Text = "Container";
            // 
            // labelApplication
            // 
            this.labelApplication.AutoSize = true;
            this.labelApplication.Location = new System.Drawing.Point(22, 43);
            this.labelApplication.Name = "labelApplication";
            this.labelApplication.Size = new System.Drawing.Size(106, 24);
            this.labelApplication.TabIndex = 0;
            this.labelApplication.Text = "Application";
            // 
            // comboBoxApplication
            // 
            this.comboBoxApplication.FormattingEnabled = true;
            this.comboBoxApplication.Location = new System.Drawing.Point(26, 70);
            this.comboBoxApplication.Name = "comboBoxApplication";
            this.comboBoxApplication.Size = new System.Drawing.Size(355, 32);
            this.comboBoxApplication.TabIndex = 4;
            // 
            // comboBoxContainer
            // 
            this.comboBoxContainer.FormattingEnabled = true;
            this.comboBoxContainer.Location = new System.Drawing.Point(26, 156);
            this.comboBoxContainer.Name = "comboBoxContainer";
            this.comboBoxContainer.Size = new System.Drawing.Size(355, 32);
            this.comboBoxContainer.TabIndex = 5;
            // 
            // buttonOn
            // 
            this.buttonOn.BackColor = System.Drawing.Color.NavajoWhite;
            this.buttonOn.Location = new System.Drawing.Point(26, 242);
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.Size = new System.Drawing.Size(355, 63);
            this.buttonOn.TabIndex = 6;
            this.buttonOn.Text = "ON";
            this.buttonOn.UseVisualStyleBackColor = false;
            this.buttonOn.Click += new System.EventHandler(this.buttonOn_Click);
            // 
            // buttonOff
            // 
            this.buttonOff.BackColor = System.Drawing.Color.NavajoWhite;
            this.buttonOff.Location = new System.Drawing.Point(26, 328);
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.Size = new System.Drawing.Size(355, 63);
            this.buttonOff.TabIndex = 7;
            this.buttonOff.Text = "OFF";
            this.buttonOff.UseVisualStyleBackColor = false;
            this.buttonOff.Click += new System.EventHandler(this.buttonOff_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 437);
            this.Controls.Add(this.groupBoxSecurity);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBoxSecurity.ResumeLayout(false);
            this.groupBoxSecurity.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSecurity;
        private System.Windows.Forms.Label labelContainer;
        private System.Windows.Forms.Label labelApplication;
        private System.Windows.Forms.ComboBox comboBoxApplication;
        private System.Windows.Forms.ComboBox comboBoxContainer;
        private System.Windows.Forms.Button buttonOn;
        private System.Windows.Forms.Button buttonOff;
    }
}

