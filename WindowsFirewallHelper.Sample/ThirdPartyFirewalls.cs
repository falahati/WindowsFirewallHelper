using System;
using System.Windows.Forms;

namespace WindowsFirewallHelper.Sample
{
    public partial class ThirdPartyFirewalls : Form
    {
        public ThirdPartyFirewalls()
        {
            InitializeComponent();
        }

        private void ThirdPartyFirewalls_Load(object sender, EventArgs e)
        {
            foreach (FirewallProduct registeredProduct in FirewallManager.RegisteredProducts)
            {
                listBoxFirewalls.Items.Add(registeredProduct.Name);
            }
        }
    }
}
