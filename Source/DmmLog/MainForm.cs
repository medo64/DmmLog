using DmmLogDriver;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DmmLog {
    internal partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            mnu.Renderer = new Helpers.ToolStripBorderlessProfessionalRenderer();

            Medo.Windows.Forms.State.SetupOnLoadAndClose(this);
        }


        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData) {
            switch (keyData) {
                case Keys.Alt | Keys.Menu:
                case Keys.F10:
                    if (mnu.ContainsFocus) {
                        this.Select();
                        //TODO: Select something else
                    } else {
                        mnu.Select();
                        mnu.Items[0].Select();
                    }
                    return true;

                case Keys.Control | Keys.N:
                    mnuNew.PerformClick();
                    return true;


                case Keys.F1:
                    mnuApp.ShowDropDown();
                    return true;

                default: return base.ProcessCmdKey(ref msg, keyData);
            }
        }


        private void Form_Load(object sender, EventArgs e) {
            Devices.Load();

            foreach (var device in Devices.LoadedDevices) {
                AddDeviceMenu(device);
            }
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e) {

        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e) {
            Devices.Save();
        }


        #region Menu

        private void mnuNew_Click(object sender, EventArgs e) {

        }


        private void mnuDeviceAdd_DropDownOpening(object sender, EventArgs e) {
            mnuDeviceAdd.DropDownItems.Clear();
            foreach (var driver in Drivers.LoadedDrivers) {
                var menuItem = new ToolStripMenuItem(driver.Information.DisplayName);
                menuItem.Click += delegate(object senderClick, EventArgs eClick) {
                    switch (driver.Information.Interface) {
                        case DmmDriverInterface.None: {
                                using (var frm = new NewDeviceNoInterfaceForm(driver)) {
                                    if (frm.ShowDialog(this) == DialogResult.OK) {
                                        var device = Devices.Add(driver, frm.SelectedDisplayName, "");
                                        if (device != null) {
                                            AddDeviceMenu(device);
                                        }
                                    }
                                }
                            } break;

                        case DmmDriverInterface.SerialPort: {
                                using (var frm = new NewDeviceSerialInterfaceForm(driver)) {
                                    if (frm.ShowDialog(this) == DialogResult.OK) {
                                        var device = Devices.Add(driver, frm.SelectedDisplayName, frm.SelectedSettings);
                                        if (device != null) {
                                            AddDeviceMenu(device);
                                        }
                                    }
                                }
                            } break;

                        default: throw new NotSupportedException("Unknown interface '" + driver.Information.Interface.ToString() + "'.");
                    }
                };
                mnuDeviceAdd.DropDownItems.Add(menuItem);
            }
        }


        private void mnuAppFeedback_Click(object sender, EventArgs e) {
            Medo.Diagnostics.ErrorReport.ShowDialog(this, null, new Uri("http://jmedved.com/feedback/"));
        }

        private void mnuAppUpgrade_Click(object sender, EventArgs e) {
            Medo.Services.Upgrade.ShowDialog(this, new Uri("http://jmedved.com/upgrade/"));
        }

        private void mnuAppDonate_Click(object sender, EventArgs e) {
            Process.Start("http://www.jmedved.com/donate/");
        }

        private void mnuAppAbout_Click(object sender, EventArgs e) {
            Medo.Windows.Forms.AboutBox.ShowDialog(this, new Uri("http://www.jmedved.com/dmmlog/"));
        }

        #endregion


        #region Helpers

        private void AddDeviceMenu(Device device) {
            var insertIndex = mnu.Items.Count - 1; //default insert location
            for (int i = mnu.Items.IndexOf(mnuDeviceSeparator) + 1; i < mnu.Items.Count; i++) {
                if ((mnu.Items[i].Tag as Device) == null) { //all device items have their instance in Tag
                    insertIndex = i;
                    break;
                }
            }
            mnu.Items.Insert(++insertIndex, device.CreateMenuItem(this, mnu));
        }

        #endregion

    }
}
