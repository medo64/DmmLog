using System;
using System.Drawing;
using System.Windows.Forms;

namespace DmmLog {
    internal partial class NewDeviceNoInterfaceForm : Form {

        private NewDeviceNoInterfaceForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
        }

         public NewDeviceNoInterfaceForm(Driver driver) //new device
            : this() {
            this.Text = "New " + driver.Capabilities.DisplayName + " device";
        }

         public NewDeviceNoInterfaceForm(Device device) //edit device
            : this() {
            this.Text = "Settings for " + device.DisplayName;

            txtDisplayName.Text = device.DisplayName;
        }


         private void Form_Load(object sender, System.EventArgs e) {
             btnOk.Enabled = TryParse();
         }


         private void txtDisplayName_TextChanged(object sender, System.EventArgs e) {
             btnOk.Enabled = TryParse();
         }


         public String SelectedDisplayName { get; private set; }


         private void btnOk_Click(object sender, System.EventArgs e) {
             string displayName;
             if (TryParse(out displayName)) {
                 this.SelectedDisplayName = displayName;
             }
         }


         private bool TryParse() {
             string displayName;
             return TryParse(out displayName);
         }

         private bool TryParse(out string displayName) {
             var hasErrors = false;

             displayName = txtDisplayName.Text.Trim();
             if (displayName.Length == 0) {
                 displayName = default(string);
                 hasErrors = true;
             }

             return !hasErrors;
         }

    }
}
