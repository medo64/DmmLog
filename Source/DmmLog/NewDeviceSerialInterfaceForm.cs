using DmmLogDriver;
using Medo;
using System;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Windows.Forms;

namespace DmmLog {
    internal partial class NewDeviceSerialInterfaceForm : Form {

        private NewDeviceSerialInterfaceForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;


            foreach (var portName in SerialPort.GetPortNames()) {
                cmbPortName.Items.Add(new TagItem<string, string>(portName, portName));
            }

            foreach (var baudRate in new int[] { 4800, 9600, 19200, 38400, 57600, 115200 }) {
                cmbBaudRate.Items.Add(new TagItem<int, string>(baudRate, baudRate.ToString(CultureInfo.CurrentCulture)));
            }

            foreach (var parity in new Parity[] { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space }) {
                cmbParity.Items.Add(new TagItem<Parity, string>(parity, parity.ToString()));
            }

            cmbDataBits.Items.Add(new TagItem<int, string>(7, 7.ToString(CultureInfo.CurrentCulture)));
            cmbDataBits.Items.Add(new TagItem<int, string>(8, 8.ToString(CultureInfo.CurrentCulture)));

            cmbStopBits.Items.Add(new TagItem<StopBits, string>(StopBits.None, 0.ToString(CultureInfo.CurrentCulture)));
            cmbStopBits.Items.Add(new TagItem<StopBits, string>(StopBits.One, 1.ToString(CultureInfo.CurrentCulture)));
            cmbStopBits.Items.Add(new TagItem<StopBits, string>(StopBits.Two, 2.ToString(CultureInfo.CurrentCulture)));
        }

        public NewDeviceSerialInterfaceForm(Driver driver) //new device
            : this() {
            this.Text = "New " + driver.Capabilities.DisplayName + " device";

            cmbPortName.SelectedIndex = 0;
            cmbBaudRate.SelectedIndex = 1; //9600
            cmbParity.SelectedIndex = 0;   //N
            cmbDataBits.SelectedIndex = 1; //8
            cmbStopBits.SelectedIndex = 1; //1
        }

        public NewDeviceSerialInterfaceForm(Device device) //edit device
            : this() {
            this.Text = "Settings for " + device.DisplayName;

            txtDisplayName.Text = device.DisplayName;

            var settings = DmmSerialPortSettings.Parse(device.Settings);

            foreach (TagItem<string, string> item in cmbPortName.Items) {
                if (item.Key.Equals(settings.PortName)) {
                    cmbPortName.SelectedItem = item;
                    break;
                }
            }
            if (cmbPortName.SelectedIndex == -1) { cmbPortName.SelectedIndex = cmbPortName.Items.Add(new TagItem<string, string>(settings.PortName, "(" + settings.PortName + ")")); }

            foreach (TagItem<int, string> item in cmbBaudRate.Items) {
                if (item.Key.Equals(settings.BaudRate)) {
                    cmbBaudRate.SelectedItem = item;
                    break;
                }
            }
            if (cmbBaudRate.SelectedIndex == -1) { cmbBaudRate.SelectedIndex = cmbBaudRate.Items.Add(new TagItem<int, string>(settings.BaudRate, "(" + settings.BaudRate.ToString(CultureInfo.CurrentCulture) + ")")); }

            foreach (TagItem<Parity, string> item in cmbParity.Items) {
                if (item.Key.Equals(settings.Parity)) {
                    cmbParity.SelectedItem = item;
                    break;
                }
            }

            foreach (TagItem<int, string> item in cmbDataBits.Items) {
                if (item.Key.Equals(settings.DataBits)) {
                    cmbDataBits.SelectedItem = item;
                    break;
                }
            }

            foreach (TagItem<StopBits, string> item in cmbStopBits.Items) {
                if (item.Key.Equals(settings.StopBits)) {
                    cmbStopBits.SelectedItem = item;
                    break;
                }
            }
        }


        private void Form_Load(object sender, System.EventArgs e) {
            btnOk.Enabled = TryParse();
        }


        private void cmbDriver_SelectedIndexChanged(object sender, System.EventArgs e) {
            cmbSerialPortSettings_Common_SelectedIndexChanged(sender, e);
        }

        private void cmbSerialPortSettings_Common_SelectedIndexChanged(object sender, System.EventArgs e) {
            btnOk.Enabled = TryParse();
        }


        public String SelectedDisplayName { get; private set; }
        public String SelectedSettings { get; private set; }


        private void btnOk_Click(object sender, System.EventArgs e) {
            string displayName;
            string portName;
            int baudRate;
            Parity parity;
            int dataBits;
            StopBits stopBits;

            if (TryParse(out displayName, out portName, out baudRate, out parity, out dataBits, out stopBits)) {
                this.SelectedDisplayName = displayName;
                this.SelectedSettings = (new DmmSerialPortSettings(portName, baudRate, parity, dataBits, stopBits)).ToString();
            }
        }


        private bool TryParse() {
            string displayName;
            string portName;
            int baudRate;
            Parity parity;
            int dataBits;
            StopBits stopBits;
            return TryParse(out displayName, out portName, out baudRate, out parity, out dataBits, out stopBits);
        }

        private bool TryParse(out string displayName, out string portName, out int baudRate, out Parity parity, out int dataBits, out StopBits stopBits) {
            var hasErrors = false;

            displayName = txtDisplayName.Text.Trim();
            if (displayName.Length == 0) {
                displayName = default(string);
                hasErrors = true;
            }

            if (cmbPortName.SelectedIndex >= 0) {
                portName = ((TagItem<string, string>)cmbPortName.SelectedItem).Key;
            } else {
                portName = default(string);
                hasErrors = true;
            }

            baudRate = default(int);
            if (cmbBaudRate.SelectedIndex >= 0) {
                baudRate = ((TagItem<int, string>)cmbBaudRate.SelectedItem).Key;
            } else {
                hasErrors = true;
            }

            parity = default(Parity);
            if (cmbParity.SelectedIndex >= 0) {
                parity = ((TagItem<Parity, string>)cmbParity.SelectedItem).Key;
            } else {
                hasErrors = true;
            }

            dataBits = default(int);
            if (cmbDataBits.SelectedIndex >= 0) {
                dataBits = ((TagItem<int, string>)cmbDataBits.SelectedItem).Key;
            } else {
                hasErrors = true;
            }

            stopBits = default(StopBits);
            if (cmbStopBits.SelectedIndex >= 0) {
                stopBits = ((TagItem<StopBits, string>)cmbStopBits.SelectedItem).Key;
            } else {
                hasErrors = true;
            }

            return !hasErrors;
        }

    }
}
