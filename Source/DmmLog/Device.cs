using DmmLogDriver;
using Medo.Resources;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DmmLog {
    internal class Device {

        public Device(String displayName, Driver driver, String settings) {
            this.DisplayName = displayName;
            this.Driver = driver;
            this.Settings = settings;
            this.Instance = driver.GetInstance(settings);

            this.Worker.DoWork += Worker_DoWork;
            this.Worker.ProgressChanged += Worker_ProgressChanged;
            this.Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }


        public Driver Driver { get; private set; }
        public String DisplayName { get; private set; }
        public String Settings { get; private set; }

        public DmmDriver Instance { get; private set; }

        private BackgroundWorker Worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };


        public override string ToString() {
            return this.DisplayName;
        }


        #region Control

        public void Connect() {
            if (this.IsConnected == false) {
                this.Worker.RunWorkerAsync();
            }
        }

        public void Disconnect() {
            if (this.IsConnected == true) {
                this.Worker.CancelAsync();
            }
        }


        public bool? IsConnected {
            get {
                if (this.Instance.IsConnected) {
                    return true;
                } else if (this.Worker.IsBusy) {
                    return null; //in process of being connected
                } else {
                    return false;
                }
            }
        }

        private IWin32Window Window;
        private ToolStrip Menu;
        private ToolStripSplitButton MenuItem;

        public ToolStripItem CreateMenuItem(IWin32Window window, ToolStrip menu) {
            Debug.Assert(window != null);
            Debug.Assert(menu != null);

            this.Window = window;
            this.Menu = menu;

            this.MenuItem = new ToolStripSplitButton(this.DisplayName, ManifestResources.GetBitmap("DmmLog.Resources.DmmMenu.png")) {
                DoubleClickEnabled = true,
                ForeColor = SystemColors.GrayText,
                Tag = this,
                ToolTipText = this.Driver.Capabilities.DisplayName
            };


            this.MenuItem.ButtonClick += delegate(object sender, EventArgs e) {
                //TODO:Select device 
                this.MenuItem.ShowDropDown();
            };

            this.MenuItem.ButtonDoubleClick += delegate(object sender, EventArgs e) {
                if (this.IsConnected == false) {
                    this.Connect();
                }
            };

            this.MenuItem.DropDownOpening += delegate(object sender, EventArgs e) {
                this.MenuItem.DropDownItems.Clear();
                FillDeviceDropDown(this.MenuItem.DropDownItems, this);
            };

            return this.MenuItem;
        }

        #endregion


        #region Menu

        private void FillDeviceDropDown(ToolStripItemCollection menu, Device device) {
            if (this.IsConnected == true) {
                var mnuDisconnect = new ToolStripMenuItem("Disconnect");
                mnuDisconnect.Click += delegate(object sender, EventArgs e) {
                    this.Disconnect();
                };
                menu.Add(mnuDisconnect);
            } else if (this.IsConnected == false) {
                var mnuConnect = new ToolStripMenuItem("Connect");
                mnuConnect.Click += delegate(object sender, EventArgs e) {
                    this.Connect();
                };
                menu.Add(mnuConnect);
            } else {
                menu.Add(new ToolStripMenuItem("Connecting...") { Enabled = false });
            }

            menu.Add(new ToolStripSeparator());

            var mnuConfigure = new ToolStripMenuItem("Configure", ManifestResources.GetBitmap("DmmLog.Resources.DmmMenuEdit.png"));
            if (this.IsConnected == false) {
                mnuConfigure.Click += delegate(object sender, EventArgs e) {
                    String newDisplayName = this.DisplayName;
                    String newSettings = this.Settings;
                    switch (this.Driver.Capabilities.Interface) {
                        case DmmDriverInterface.None: {
                                using (var frm = new NewDeviceNoInterfaceForm(this)) {
                                    if (frm.ShowDialog(this.Window) == DialogResult.OK) {
                                        newDisplayName = frm.SelectedDisplayName;
                                        newSettings = "";
                                    }
                                }
                            } break;

                        case DmmDriverInterface.SerialPort: {
                                using (var frm = new NewDeviceSerialInterfaceForm(this)) {
                                    if (frm.ShowDialog(this.Window) == DialogResult.OK) {
                                        newDisplayName = frm.SelectedDisplayName;
                                        newSettings = frm.SelectedSettings;
                                    }
                                }
                            } break;

                        default: throw new NotSupportedException("Unknown interface '" + this.Driver.Capabilities.Interface.ToString() + "'.");
                    }

                    if (!string.Equals(this.DisplayName, newDisplayName, StringComparison.Ordinal)) { //change display name
                        this.DisplayName = newDisplayName;
                        if (this.MenuItem != null) { this.MenuItem.Text = this.DisplayName; }
                    }

                    if (!string.Equals(this.Settings, newSettings, StringComparison.Ordinal)) { //change settings
                        this.Settings = newSettings;
                        this.Instance.Dispose();
                        this.Instance = this.Driver.GetInstance(this.Settings);
                    }
                };
            } else {
                mnuConfigure.Enabled = false;
            }
            menu.Add(mnuConfigure);

            var mnuRemove = new ToolStripMenuItem("Remove", ManifestResources.GetBitmap("DmmLog.Resources.DmmMenuRemove.png"));
            if (this.IsConnected == false) {
                mnuRemove.Click += delegate(object sender, EventArgs e) {
                    if (Medo.MessageBox.ShowQuestion(this.Window, "Do you really want to remove " + this.DisplayName + "?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        Devices.Remove(this);
                        this.Menu.Items.Remove(this.MenuItem);
                    }
                };
            } else {
                mnuRemove.Enabled = false;
            }
            menu.Add(mnuRemove);
        }

        #endregion


        #region Worker

        void Worker_DoWork(object sender, DoWorkEventArgs e) {
            var dmm = this.Instance;

            Debug.WriteLine("I: Connecting to '" + this.DisplayName + "' using '" + this.Driver.Capabilities.DisplayName + "' driver.");
            dmm.Connect();

            if (dmm.IsConnected) {
                while (!this.Worker.CancellationPending) {
                    if (dmm.IsConnected == false) { break; } //TODO: restart?
                    var current = dmm.GetCurrentMeasurement();
                    if (current != null) {
                        Debug.WriteLine("I: Current measurement '" + current.ToString() + "'.");
                        this.CurrentMeasurement = current;
                    } else {
                        Debug.WriteLine("I: Current measurement '-'.");
                    }
                    Devices.RaiseMeasurementUpdate(this);
                    Thread.Sleep(this.Driver.Capabilities.UpdateInterval);

                    Worker.ReportProgress(-1); //new measurement
                }
            }

            dmm.Disconnect();
        }

        void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            this.MenuItem.ForeColor = this.Instance.IsConnected ? SystemColors.ControlText : SystemColors.GrayText;
        }

        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            this.MenuItem.ForeColor = SystemColors.GrayText;
        }

        #endregion


        #region Properties

        private DmmMeasurement _currentMeasurement;
        public DmmMeasurement CurrentMeasurement {
            get {
                var value = this._currentMeasurement;
                return (value != null) && ((DateTime.UtcNow - value.Time).TotalMilliseconds <= this.Driver.Capabilities.UpdateInterval * 1.5) ? value : null;
            }
            private set { this._currentMeasurement = value; }
        }

        #endregion

    }
}
