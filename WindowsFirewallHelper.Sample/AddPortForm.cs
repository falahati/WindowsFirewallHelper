using System.Windows.Forms;

namespace WindowsFirewallHelper.Sample
{
    internal partial class AddPortForm : Form
    {
        public AddPortForm()
        {
            InitializeComponent();

            cb_protocol.Items.Add(FirewallProtocol.TCP);
            cb_protocol.Items.Add(FirewallProtocol.UDP);
            cb_protocol.Items.Add(FirewallProtocol.Any);
            cb_protocol.SelectedIndex = 0;
        }

        public FirewallProtocol FirewallProtocol
        {
            get => cb_protocol.SelectedItem as FirewallProtocol;
        }

        public ushort PortNumber
        {
            get => (ushort) nud_port.Value;
        }
    }
}