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

        public AddPortForm(
            FirewallProfiles currentProfile,
            string defaultRuleName)
            : this()
        {
            textBoxRuleName.Text = defaultRuleName;

            for (int index = 0; index < checkedListBoxProfiles.Items.Count; index++)
            {
                if (checkedListBoxProfiles.Items[index].ToString() == currentProfile.ToString())
                {
                    checkedListBoxProfiles.SetItemChecked(index, true);
                }
            }
        }
        public FirewallProtocol FirewallProtocol
        {
            get => cb_protocol.SelectedItem as FirewallProtocol;
        }

        public ushort PortNumber
        {
            get => (ushort) nud_port.Value;
        }

        public string RuleName
        {
            get => textBoxRuleName.Text;
        }

        public FirewallProfiles Profiles
        {
            get
            {
                FirewallProfiles profiles = (FirewallProfiles) 0;

                foreach (object checkedItem in checkedListBoxProfiles.CheckedItems)
                {
                    switch (checkedItem.ToString())
                    {
                        case "Domain":
                            profiles |= FirewallProfiles.Domain;
                            break;
                        case "Private":
                            profiles |= FirewallProfiles.Private;
                            break;
                        case "Public":
                            profiles |= FirewallProfiles.Public;
                            break;
                    }
                }

                return profiles;
            }
        }
    }
}