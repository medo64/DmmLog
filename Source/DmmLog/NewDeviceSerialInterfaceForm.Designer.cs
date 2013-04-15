namespace DmmLog {
    partial class NewDeviceSerialInterfaceForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.grpSerialPortSettings = new System.Windows.Forms.GroupBox();
            this.lblStopBits = new System.Windows.Forms.Label();
            this.cmbStopBits = new System.Windows.Forms.ComboBox();
            this.lblDataBits = new System.Windows.Forms.Label();
            this.cmbDataBits = new System.Windows.Forms.ComboBox();
            this.lblParity = new System.Windows.Forms.Label();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.lblPortName = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.cmbPortName = new System.Windows.Forms.ComboBox();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.grpSerialPortSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(215, 235);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(134, 235);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grpSerialPortSettings
            // 
            this.grpSerialPortSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSerialPortSettings.Controls.Add(this.lblStopBits);
            this.grpSerialPortSettings.Controls.Add(this.cmbStopBits);
            this.grpSerialPortSettings.Controls.Add(this.lblDataBits);
            this.grpSerialPortSettings.Controls.Add(this.cmbDataBits);
            this.grpSerialPortSettings.Controls.Add(this.lblParity);
            this.grpSerialPortSettings.Controls.Add(this.cmbParity);
            this.grpSerialPortSettings.Controls.Add(this.lblBaudRate);
            this.grpSerialPortSettings.Controls.Add(this.lblPortName);
            this.grpSerialPortSettings.Controls.Add(this.cmbBaudRate);
            this.grpSerialPortSettings.Controls.Add(this.cmbPortName);
            this.grpSerialPortSettings.Location = new System.Drawing.Point(12, 46);
            this.grpSerialPortSettings.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.grpSerialPortSettings.Name = "grpSerialPortSettings";
            this.grpSerialPortSettings.Padding = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.grpSerialPortSettings.Size = new System.Drawing.Size(278, 177);
            this.grpSerialPortSettings.TabIndex = 4;
            this.grpSerialPortSettings.TabStop = false;
            this.grpSerialPortSettings.Text = "Serial port";
            // 
            // lblStopBits
            // 
            this.lblStopBits.AutoSize = true;
            this.lblStopBits.Location = new System.Drawing.Point(6, 150);
            this.lblStopBits.Name = "lblStopBits";
            this.lblStopBits.Size = new System.Drawing.Size(67, 17);
            this.lblStopBits.TabIndex = 8;
            this.lblStopBits.Text = "Stop bits:";
            // 
            // cmbStopBits
            // 
            this.cmbStopBits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStopBits.FormattingEnabled = true;
            this.cmbStopBits.Location = new System.Drawing.Point(122, 147);
            this.cmbStopBits.Name = "cmbStopBits";
            this.cmbStopBits.Size = new System.Drawing.Size(150, 24);
            this.cmbStopBits.TabIndex = 9;
            this.cmbStopBits.SelectedIndexChanged += new System.EventHandler(this.cmbSerialPortSettings_Common_SelectedIndexChanged);
            // 
            // lblDataBits
            // 
            this.lblDataBits.AutoSize = true;
            this.lblDataBits.Location = new System.Drawing.Point(6, 120);
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.Size = new System.Drawing.Size(68, 17);
            this.lblDataBits.TabIndex = 6;
            this.lblDataBits.Text = "Data bits:";
            // 
            // cmbDataBits
            // 
            this.cmbDataBits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataBits.FormattingEnabled = true;
            this.cmbDataBits.Location = new System.Drawing.Point(122, 117);
            this.cmbDataBits.Name = "cmbDataBits";
            this.cmbDataBits.Size = new System.Drawing.Size(150, 24);
            this.cmbDataBits.TabIndex = 7;
            this.cmbDataBits.SelectedIndexChanged += new System.EventHandler(this.cmbSerialPortSettings_Common_SelectedIndexChanged);
            // 
            // lblParity
            // 
            this.lblParity.AutoSize = true;
            this.lblParity.Location = new System.Drawing.Point(6, 90);
            this.lblParity.Name = "lblParity";
            this.lblParity.Size = new System.Drawing.Size(44, 17);
            this.lblParity.TabIndex = 4;
            this.lblParity.Text = "Parity";
            // 
            // cmbParity
            // 
            this.cmbParity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParity.FormattingEnabled = true;
            this.cmbParity.Location = new System.Drawing.Point(122, 87);
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.Size = new System.Drawing.Size(150, 24);
            this.cmbParity.TabIndex = 5;
            this.cmbParity.SelectedIndexChanged += new System.EventHandler(this.cmbSerialPortSettings_Common_SelectedIndexChanged);
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Location = new System.Drawing.Point(6, 60);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(74, 17);
            this.lblBaudRate.TabIndex = 2;
            this.lblBaudRate.Text = "Baud rate:";
            // 
            // lblPortName
            // 
            this.lblPortName.AutoSize = true;
            this.lblPortName.Location = new System.Drawing.Point(6, 30);
            this.lblPortName.Name = "lblPortName";
            this.lblPortName.Size = new System.Drawing.Size(77, 17);
            this.lblPortName.TabIndex = 0;
            this.lblPortName.Text = "Port name:";
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Location = new System.Drawing.Point(122, 57);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(150, 24);
            this.cmbBaudRate.TabIndex = 3;
            this.cmbBaudRate.SelectedIndexChanged += new System.EventHandler(this.cmbSerialPortSettings_Common_SelectedIndexChanged);
            // 
            // cmbPortName
            // 
            this.cmbPortName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Location = new System.Drawing.Point(122, 27);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(150, 24);
            this.cmbPortName.TabIndex = 1;
            this.cmbPortName.SelectedIndexChanged += new System.EventHandler(this.cmbSerialPortSettings_Common_SelectedIndexChanged);
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(90, 12);
            this.txtDisplayName.MaxLength = 32;
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(200, 22);
            this.txtDisplayName.TabIndex = 1;
            this.txtDisplayName.TextChanged += new System.EventHandler(this.cmbSerialPortSettings_Common_SelectedIndexChanged);
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(12, 15);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(49, 17);
            this.lblDisplayName.TabIndex = 0;
            this.lblDisplayName.Text = "Name:";
            // 
            // NewDeviceSerialForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(302, 270);
            this.Controls.Add(this.lblDisplayName);
            this.Controls.Add(this.txtDisplayName);
            this.Controls.Add(this.grpSerialPortSettings);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewDeviceSerialForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device settings";
            this.Load += new System.EventHandler(this.Form_Load);
            this.grpSerialPortSettings.ResumeLayout(false);
            this.grpSerialPortSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox grpSerialPortSettings;
        private System.Windows.Forms.Label lblStopBits;
        private System.Windows.Forms.ComboBox cmbStopBits;
        private System.Windows.Forms.Label lblDataBits;
        private System.Windows.Forms.ComboBox cmbDataBits;
        private System.Windows.Forms.Label lblParity;
        private System.Windows.Forms.ComboBox cmbParity;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.Label lblPortName;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.ComboBox cmbPortName;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.TextBox txtDisplayName;
    }
}