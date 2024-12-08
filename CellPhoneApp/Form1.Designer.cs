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
            this.groupBoxMobileApp = new System.Windows.Forms.GroupBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonOff = new System.Windows.Forms.Button();
            this.buttonOn = new System.Windows.Forms.Button();
            this.comboBoxContainer = new System.Windows.Forms.ComboBox();
            this.comboBoxApplication = new System.Windows.Forms.ComboBox();
            this.labelContainer = new System.Windows.Forms.Label();
            this.labelApplication = new System.Windows.Forms.Label();
            this.groupBoxMobileApp.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMobileApp
            // 
            this.groupBoxMobileApp.BackColor = System.Drawing.Color.OldLace;
            this.groupBoxMobileApp.Controls.Add(this.textBoxResult);
            this.groupBoxMobileApp.Controls.Add(this.buttonOff);
            this.groupBoxMobileApp.Controls.Add(this.buttonOn);
            this.groupBoxMobileApp.Controls.Add(this.comboBoxContainer);
            this.groupBoxMobileApp.Controls.Add(this.comboBoxApplication);
            this.groupBoxMobileApp.Controls.Add(this.labelContainer);
            this.groupBoxMobileApp.Controls.Add(this.labelApplication);
            this.groupBoxMobileApp.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMobileApp.Location = new System.Drawing.Point(12, 12);
            this.groupBoxMobileApp.Name = "groupBoxMobileApp";
            this.groupBoxMobileApp.Size = new System.Drawing.Size(407, 463);
            this.groupBoxMobileApp.TabIndex = 8;
            this.groupBoxMobileApp.TabStop = false;
            this.groupBoxMobileApp.Text = "Mobile App Smart Home";
            // 
            // textBoxResult
            // 
            this.textBoxResult.BackColor = System.Drawing.Color.Wheat;
            this.textBoxResult.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxResult.Location = new System.Drawing.Point(26, 409);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.Size = new System.Drawing.Size(355, 28);
            this.textBoxResult.TabIndex = 8;
            this.textBoxResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonOff
            // 
            this.buttonOff.BackColor = System.Drawing.Color.NavajoWhite;
            this.buttonOff.Location = new System.Drawing.Point(26, 322);
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.Size = new System.Drawing.Size(355, 63);
            this.buttonOff.TabIndex = 7;
            this.buttonOff.Text = "OFF";
            this.buttonOff.UseVisualStyleBackColor = false;
            this.buttonOff.Click += new System.EventHandler(this.buttonOff_Click);
            // 
            // buttonOn
            // 
            this.buttonOn.BackColor = System.Drawing.Color.NavajoWhite;
            this.buttonOn.Location = new System.Drawing.Point(26, 236);
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.Size = new System.Drawing.Size(355, 63);
            this.buttonOn.TabIndex = 6;
            this.buttonOn.Text = "ON";
            this.buttonOn.UseVisualStyleBackColor = false;
            this.buttonOn.Click += new System.EventHandler(this.buttonOn_Click);
            // 
            // comboBoxContainer
            // 
            this.comboBoxContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxContainer.FormattingEnabled = true;
            this.comboBoxContainer.Location = new System.Drawing.Point(26, 156);
            this.comboBoxContainer.Name = "comboBoxContainer";
            this.comboBoxContainer.Size = new System.Drawing.Size(355, 32);
            this.comboBoxContainer.TabIndex = 5;
            // 
            // comboBoxApplication
            // 
            this.comboBoxApplication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApplication.FormattingEnabled = true;
            this.comboBoxApplication.Location = new System.Drawing.Point(26, 70);
            this.comboBoxApplication.Name = "comboBoxApplication";
            this.comboBoxApplication.Size = new System.Drawing.Size(355, 32);
            this.comboBoxApplication.TabIndex = 4;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 487);
            this.Controls.Add(this.groupBoxMobileApp);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxMobileApp.ResumeLayout(false);
            this.groupBoxMobileApp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMobileApp;
        private System.Windows.Forms.Label labelContainer;
        private System.Windows.Forms.Label labelApplication;
        private System.Windows.Forms.ComboBox comboBoxApplication;
        private System.Windows.Forms.ComboBox comboBoxContainer;
        private System.Windows.Forms.Button buttonOn;
        private System.Windows.Forms.Button buttonOff;
        private System.Windows.Forms.TextBox textBoxResult;
    }
}

