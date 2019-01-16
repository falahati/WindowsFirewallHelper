using System;
using System.Windows.Forms;

namespace WindowsFirewallHelper.Sample
{
    internal partial class EditPortForm : Form
    {
        public EditPortForm()
        {
            InitializeComponent();
            cb_protocol.SelectedIndex = 0;
        }

        public FirewallProtocol FirewallProtocol
        {
            get
            {
                switch (cb_protocol.Text?.ToUpper().Trim())
                {
                    case "ALL":
                        return FirewallProtocol.Any;
                    case "TCP":
                        return FirewallProtocol.TCP;
                    case "UDP":
                        return FirewallProtocol.UDP;
                    default:
                        FirewallProtocol protocol;
                        if (FirewallProtocol.TryParse(cb_protocol.Text, out protocol))
                            return protocol;
                        return null;
                }
            }
            set
            {
                if (value == FirewallProtocol.Any)
                    cb_protocol.SelectedItem = "ALL";
                else if (value == FirewallProtocol.TCP)
                    cb_protocol.SelectedItem = "TCP";
                else if (value == FirewallProtocol.UDP)
                    cb_protocol.SelectedItem = "UDP";
                else
                    cb_protocol.SelectedItem = FirewallProtocol.ProtocolNumber.ToString();
            }
        }

        public ushort PortNumber
        {
            get { return (ushort) nud_port.Value; }
            set { nud_port.Value = value; }
        }

        private void FormSubmit(object sender, EventArgs e)
        {
            if (FirewallProtocol == null)
            {
                cb_protocol.SelectedIndex = 0;
                cb_protocol.Focus();
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}