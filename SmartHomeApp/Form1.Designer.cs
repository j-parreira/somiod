namespace SmartHomeApp
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
            this.groupBoxLighting = new System.Windows.Forms.GroupBox();
            this.labelGardenLightSwitch = new System.Windows.Forms.Label();
            this.labelParkingLightSwitch = new System.Windows.Forms.Label();
            this.richTextBoxGardenLight = new System.Windows.Forms.RichTextBox();
            this.labelGardenLight = new System.Windows.Forms.Label();
            this.richTextBoxParkingLight = new System.Windows.Forms.RichTextBox();
            this.labelParkingLight = new System.Windows.Forms.Label();
            this.groupBoxHeating = new System.Windows.Forms.GroupBox();
            this.labelHeaterSwitch = new System.Windows.Forms.Label();
            this.labelAirConditioningSwitch = new System.Windows.Forms.Label();
            this.richTextBoxHeater = new System.Windows.Forms.RichTextBox();
            this.labelHeater = new System.Windows.Forms.Label();
            this.richTextBoxAirConditioning = new System.Windows.Forms.RichTextBox();
            this.labelAirConditioning = new System.Windows.Forms.Label();
            this.groupBoxSecurity = new System.Windows.Forms.GroupBox();
            this.labelAlarmSwitch = new System.Windows.Forms.Label();
            this.labelCameraSwitch = new System.Windows.Forms.Label();
            this.richTextBoxAlarm = new System.Windows.Forms.RichTextBox();
            this.labelAlarm = new System.Windows.Forms.Label();
            this.richTextBoxCamera = new System.Windows.Forms.RichTextBox();
            this.labelCamera = new System.Windows.Forms.Label();
            this.groupBoxLighting.SuspendLayout();
            this.groupBoxHeating.SuspendLayout();
            this.groupBoxSecurity.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLighting
            // 
            this.groupBoxLighting.BackColor = System.Drawing.Color.LightYellow;
            this.groupBoxLighting.Controls.Add(this.labelGardenLightSwitch);
            this.groupBoxLighting.Controls.Add(this.labelParkingLightSwitch);
            this.groupBoxLighting.Controls.Add(this.richTextBoxGardenLight);
            this.groupBoxLighting.Controls.Add(this.labelGardenLight);
            this.groupBoxLighting.Controls.Add(this.richTextBoxParkingLight);
            this.groupBoxLighting.Controls.Add(this.labelParkingLight);
            this.groupBoxLighting.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxLighting.Location = new System.Drawing.Point(12, 12);
            this.groupBoxLighting.Name = "groupBoxLighting";
            this.groupBoxLighting.Size = new System.Drawing.Size(370, 241);
            this.groupBoxLighting.TabIndex = 0;
            this.groupBoxLighting.TabStop = false;
            this.groupBoxLighting.Text = "Lighting Application";
            // 
            // labelGardenLightSwitch
            // 
            this.labelGardenLightSwitch.AutoSize = true;
            this.labelGardenLightSwitch.BackColor = System.Drawing.SystemColors.Window;
            this.labelGardenLightSwitch.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGardenLightSwitch.Location = new System.Drawing.Point(248, 124);
            this.labelGardenLightSwitch.Name = "labelGardenLightSwitch";
            this.labelGardenLightSwitch.Size = new System.Drawing.Size(52, 35);
            this.labelGardenLightSwitch.TabIndex = 6;
            this.labelGardenLightSwitch.Text = "ON";
            // 
            // labelParkingLightSwitch
            // 
            this.labelParkingLightSwitch.AutoSize = true;
            this.labelParkingLightSwitch.BackColor = System.Drawing.SystemColors.Window;
            this.labelParkingLightSwitch.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelParkingLightSwitch.Location = new System.Drawing.Point(73, 124);
            this.labelParkingLightSwitch.Name = "labelParkingLightSwitch";
            this.labelParkingLightSwitch.Size = new System.Drawing.Size(60, 35);
            this.labelParkingLightSwitch.TabIndex = 5;
            this.labelParkingLightSwitch.Text = "OFF";
            // 
            // richTextBoxGardenLight
            // 
            this.richTextBoxGardenLight.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxGardenLight.Location = new System.Drawing.Point(198, 70);
            this.richTextBoxGardenLight.Name = "richTextBoxGardenLight";
            this.richTextBoxGardenLight.Size = new System.Drawing.Size(150, 150);
            this.richTextBoxGardenLight.TabIndex = 4;
            this.richTextBoxGardenLight.Text = "";
            // 
            // labelGardenLight
            // 
            this.labelGardenLight.AutoSize = true;
            this.labelGardenLight.Location = new System.Drawing.Point(194, 43);
            this.labelGardenLight.Name = "labelGardenLight";
            this.labelGardenLight.Size = new System.Drawing.Size(117, 24);
            this.labelGardenLight.TabIndex = 3;
            this.labelGardenLight.Text = "Garden Light";
            // 
            // richTextBoxParkingLight
            // 
            this.richTextBoxParkingLight.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxParkingLight.Location = new System.Drawing.Point(26, 70);
            this.richTextBoxParkingLight.Name = "richTextBoxParkingLight";
            this.richTextBoxParkingLight.Size = new System.Drawing.Size(150, 150);
            this.richTextBoxParkingLight.TabIndex = 2;
            this.richTextBoxParkingLight.Text = "";
            // 
            // labelParkingLight
            // 
            this.labelParkingLight.AutoSize = true;
            this.labelParkingLight.Location = new System.Drawing.Point(22, 43);
            this.labelParkingLight.Name = "labelParkingLight";
            this.labelParkingLight.Size = new System.Drawing.Size(118, 24);
            this.labelParkingLight.TabIndex = 0;
            this.labelParkingLight.Text = "Parking Light";
            // 
            // groupBoxHeating
            // 
            this.groupBoxHeating.BackColor = System.Drawing.Color.Bisque;
            this.groupBoxHeating.Controls.Add(this.labelHeaterSwitch);
            this.groupBoxHeating.Controls.Add(this.labelAirConditioningSwitch);
            this.groupBoxHeating.Controls.Add(this.richTextBoxHeater);
            this.groupBoxHeating.Controls.Add(this.labelHeater);
            this.groupBoxHeating.Controls.Add(this.richTextBoxAirConditioning);
            this.groupBoxHeating.Controls.Add(this.labelAirConditioning);
            this.groupBoxHeating.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxHeating.Location = new System.Drawing.Point(406, 12);
            this.groupBoxHeating.Name = "groupBoxHeating";
            this.groupBoxHeating.Size = new System.Drawing.Size(370, 241);
            this.groupBoxHeating.TabIndex = 7;
            this.groupBoxHeating.TabStop = false;
            this.groupBoxHeating.Text = "Heating Application";
            // 
            // labelHeaterSwitch
            // 
            this.labelHeaterSwitch.AutoSize = true;
            this.labelHeaterSwitch.BackColor = System.Drawing.SystemColors.Window;
            this.labelHeaterSwitch.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeaterSwitch.Location = new System.Drawing.Point(248, 124);
            this.labelHeaterSwitch.Name = "labelHeaterSwitch";
            this.labelHeaterSwitch.Size = new System.Drawing.Size(52, 35);
            this.labelHeaterSwitch.TabIndex = 6;
            this.labelHeaterSwitch.Text = "ON";
            // 
            // labelAirConditioningSwitch
            // 
            this.labelAirConditioningSwitch.AutoSize = true;
            this.labelAirConditioningSwitch.BackColor = System.Drawing.SystemColors.Window;
            this.labelAirConditioningSwitch.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAirConditioningSwitch.Location = new System.Drawing.Point(73, 124);
            this.labelAirConditioningSwitch.Name = "labelAirConditioningSwitch";
            this.labelAirConditioningSwitch.Size = new System.Drawing.Size(60, 35);
            this.labelAirConditioningSwitch.TabIndex = 5;
            this.labelAirConditioningSwitch.Text = "OFF";
            // 
            // richTextBoxHeater
            // 
            this.richTextBoxHeater.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxHeater.Location = new System.Drawing.Point(198, 70);
            this.richTextBoxHeater.Name = "richTextBoxHeater";
            this.richTextBoxHeater.Size = new System.Drawing.Size(150, 150);
            this.richTextBoxHeater.TabIndex = 4;
            this.richTextBoxHeater.Text = "";
            // 
            // labelHeater
            // 
            this.labelHeater.AutoSize = true;
            this.labelHeater.Location = new System.Drawing.Point(194, 43);
            this.labelHeater.Name = "labelHeater";
            this.labelHeater.Size = new System.Drawing.Size(67, 24);
            this.labelHeater.TabIndex = 3;
            this.labelHeater.Text = "Heater";
            // 
            // richTextBoxAirConditioning
            // 
            this.richTextBoxAirConditioning.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxAirConditioning.Location = new System.Drawing.Point(26, 70);
            this.richTextBoxAirConditioning.Name = "richTextBoxAirConditioning";
            this.richTextBoxAirConditioning.Size = new System.Drawing.Size(150, 150);
            this.richTextBoxAirConditioning.TabIndex = 2;
            this.richTextBoxAirConditioning.Text = "";
            // 
            // labelAirConditioning
            // 
            this.labelAirConditioning.AutoSize = true;
            this.labelAirConditioning.Location = new System.Drawing.Point(22, 43);
            this.labelAirConditioning.Name = "labelAirConditioning";
            this.labelAirConditioning.Size = new System.Drawing.Size(147, 24);
            this.labelAirConditioning.TabIndex = 0;
            this.labelAirConditioning.Text = "Air Conditioning";
            // 
            // groupBoxSecurity
            // 
            this.groupBoxSecurity.BackColor = System.Drawing.Color.MistyRose;
            this.groupBoxSecurity.Controls.Add(this.labelAlarmSwitch);
            this.groupBoxSecurity.Controls.Add(this.labelCameraSwitch);
            this.groupBoxSecurity.Controls.Add(this.richTextBoxAlarm);
            this.groupBoxSecurity.Controls.Add(this.labelAlarm);
            this.groupBoxSecurity.Controls.Add(this.richTextBoxCamera);
            this.groupBoxSecurity.Controls.Add(this.labelCamera);
            this.groupBoxSecurity.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSecurity.Location = new System.Drawing.Point(800, 12);
            this.groupBoxSecurity.Name = "groupBoxSecurity";
            this.groupBoxSecurity.Size = new System.Drawing.Size(370, 241);
            this.groupBoxSecurity.TabIndex = 7;
            this.groupBoxSecurity.TabStop = false;
            this.groupBoxSecurity.Text = "Security Application";
            // 
            // labelAlarmSwitch
            // 
            this.labelAlarmSwitch.AutoSize = true;
            this.labelAlarmSwitch.BackColor = System.Drawing.SystemColors.Window;
            this.labelAlarmSwitch.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlarmSwitch.Location = new System.Drawing.Point(248, 124);
            this.labelAlarmSwitch.Name = "labelAlarmSwitch";
            this.labelAlarmSwitch.Size = new System.Drawing.Size(52, 35);
            this.labelAlarmSwitch.TabIndex = 6;
            this.labelAlarmSwitch.Text = "ON";
            // 
            // labelCameraSwitch
            // 
            this.labelCameraSwitch.AutoSize = true;
            this.labelCameraSwitch.BackColor = System.Drawing.SystemColors.Window;
            this.labelCameraSwitch.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCameraSwitch.Location = new System.Drawing.Point(73, 124);
            this.labelCameraSwitch.Name = "labelCameraSwitch";
            this.labelCameraSwitch.Size = new System.Drawing.Size(60, 35);
            this.labelCameraSwitch.TabIndex = 5;
            this.labelCameraSwitch.Text = "OFF";
            // 
            // richTextBoxAlarm
            // 
            this.richTextBoxAlarm.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxAlarm.Location = new System.Drawing.Point(198, 70);
            this.richTextBoxAlarm.Name = "richTextBoxAlarm";
            this.richTextBoxAlarm.Size = new System.Drawing.Size(150, 150);
            this.richTextBoxAlarm.TabIndex = 4;
            this.richTextBoxAlarm.Text = "";
            // 
            // labelAlarm
            // 
            this.labelAlarm.AutoSize = true;
            this.labelAlarm.Location = new System.Drawing.Point(194, 43);
            this.labelAlarm.Name = "labelAlarm";
            this.labelAlarm.Size = new System.Drawing.Size(60, 24);
            this.labelAlarm.TabIndex = 3;
            this.labelAlarm.Text = "Alarm";
            // 
            // richTextBoxCamera
            // 
            this.richTextBoxCamera.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxCamera.Location = new System.Drawing.Point(26, 70);
            this.richTextBoxCamera.Name = "richTextBoxCamera";
            this.richTextBoxCamera.Size = new System.Drawing.Size(150, 150);
            this.richTextBoxCamera.TabIndex = 2;
            this.richTextBoxCamera.Text = "";
            // 
            // labelCamera
            // 
            this.labelCamera.AutoSize = true;
            this.labelCamera.Location = new System.Drawing.Point(22, 43);
            this.labelCamera.Name = "labelCamera";
            this.labelCamera.Size = new System.Drawing.Size(74, 24);
            this.labelCamera.TabIndex = 0;
            this.labelCamera.Text = "Camera";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 263);
            this.Controls.Add(this.groupBoxSecurity);
            this.Controls.Add(this.groupBoxHeating);
            this.Controls.Add(this.groupBoxLighting);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxLighting.ResumeLayout(false);
            this.groupBoxLighting.PerformLayout();
            this.groupBoxHeating.ResumeLayout(false);
            this.groupBoxHeating.PerformLayout();
            this.groupBoxSecurity.ResumeLayout(false);
            this.groupBoxSecurity.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLighting;
        private System.Windows.Forms.Label labelParkingLight;
        private System.Windows.Forms.RichTextBox richTextBoxParkingLight;
        private System.Windows.Forms.RichTextBox richTextBoxGardenLight;
        private System.Windows.Forms.Label labelGardenLight;
        private System.Windows.Forms.Label labelGardenLightSwitch;
        private System.Windows.Forms.Label labelParkingLightSwitch;
        private System.Windows.Forms.GroupBox groupBoxHeating;
        private System.Windows.Forms.Label labelHeaterSwitch;
        private System.Windows.Forms.Label labelAirConditioningSwitch;
        private System.Windows.Forms.RichTextBox richTextBoxHeater;
        private System.Windows.Forms.Label labelHeater;
        private System.Windows.Forms.RichTextBox richTextBoxAirConditioning;
        private System.Windows.Forms.Label labelAirConditioning;
        private System.Windows.Forms.GroupBox groupBoxSecurity;
        private System.Windows.Forms.Label labelAlarmSwitch;
        private System.Windows.Forms.Label labelCameraSwitch;
        private System.Windows.Forms.RichTextBox richTextBoxAlarm;
        private System.Windows.Forms.Label labelAlarm;
        private System.Windows.Forms.RichTextBox richTextBoxCamera;
        private System.Windows.Forms.Label labelCamera;
    }
}

