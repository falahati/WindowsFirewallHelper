using System.Windows.Forms;

namespace WindowsFirewallHelper.Net5Sample
{
    public partial class EditRuleForm : Form
    {
        public EditRuleForm(IFirewallRule rule)
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = rule;
        }
    }
}