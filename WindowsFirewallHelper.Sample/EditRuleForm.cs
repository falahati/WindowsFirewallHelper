using System.Windows.Forms;

namespace WindowsFirewallHelper.Sample
{
    public partial class EditRuleForm : Form
    {
        public EditRuleForm(IRule rule)
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = rule;
        }
    }
}