using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using WindowsFirewallHelper.FirewallRules;

namespace WindowsFirewallHelper.Sample
{
    internal partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // ReSharper disable once MethodTooLong
        // ReSharper disable once ExcessiveIndentation
        private static void NodeDiscovery(TreeNode node)
        {
            var o = node.Tag;

            if (o == null)
            {
                return;
            }

            node.Nodes.Clear();

            if (o is ICollection<FirewallWASRule> || o is ICollection<IFirewallRule>)
            {
                foreach (var item in ((IEnumerable) o).Cast<IFirewallRule>().OrderBy(rule => rule.FriendlyName))
                {
                    node.Nodes.Add(new TreeNode(item.ToString()) {Tag = item});
                }
            }
            else if (o.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
            {
                foreach (var item in (IEnumerable) o)
                {
                    node.Nodes.Add(new TreeNode(item.ToString()) {Tag = item});
                }
            }
            else if (!o.GetType().IsValueType)
            {
                foreach (var propertyInfo in o.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)) &&
                        propertyInfo.PropertyType != typeof(string))
                    {
                        if (!propertyInfo.GetGetMethod().IsStatic)
                        {
                            var value = propertyInfo.GetValue(o, null);
                            node.Nodes.Add(new TreeNode("[" + propertyInfo.Name + "] ") {Tag = value});
                        }
                    }
                }
            }
        }

        private void AddApplicationRule(object sender, EventArgs e)
        {
            try
            {
                var profileType = (treeView.SelectedNode.Tag as IFirewallProfile)?.Type;

                if (profileType == null)
                {
                    return;
                }

                if (ofd_app.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newAppRule = FirewallManager.Instance.CreateApplicationRule(
                    profileType.Value,
                    "!!TEST!! " + Guid.NewGuid().ToString("B"),
                    FirewallAction.Allow,
                    ofd_app.FileName
                );

                newAppRule.Direction = FirewallDirection.Outbound;

                var editDialog = new EditRuleForm(newAppRule);

                if (editDialog.ShowDialog() == DialogResult.OK)
                {
                    FirewallManager.Instance.Rules.Add(newAppRule);
                    RefreshTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddPortRule(object sender, EventArgs e)
        {
            try
            {
                var profileType = (treeView.SelectedNode.Tag as IFirewallProfile)?.Type;

                if (profileType == null)
                {
                    return;
                }

                var addPortDialog = new AddPortForm(profileType.GetValueOrDefault(), "!!TEST!! " + Guid.NewGuid().ToString("B"));

                if (addPortDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newPortRule = FirewallManager.Instance.CreatePortRule(
                    addPortDialog.Profiles,
                    addPortDialog.RuleName,
                    FirewallAction.Allow,
                    addPortDialog.PortNumber,
                    addPortDialog.FirewallProtocol
                );

                // GMS 2019-08-22 - Commented as the property grid was consistently having error showing the newly created object.
                var editDialog = new EditRuleForm(newPortRule);
                if (editDialog.ShowDialog() == DialogResult.OK)
                {
                    FirewallManager.Instance.Rules.Add(newPortRule);

                    RefreshTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteRule(object sender, EventArgs e)
        {
            try
            {
                if (treeView.SelectedNode.Tag is IFirewallRule rule)
                {
                    FirewallManager.Instance.Rules.Remove(rule);
                    RefreshTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Text =
                $@"{FirewallManager.Instance.Name} ({FirewallManager.Version})";
            RefreshTreeView();
        }

        private void ItemClicked(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                NodeDiscovery(treeView.SelectedNode);
                treeView.SelectedNode.Expand();
            }
        }

        private void ItemSelected(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject =
                !treeView.SelectedNode.Tag.GetType().GetInterfaces().Contains(typeof(IEnumerable))
                    ? treeView.SelectedNode.Tag
                    : null;
            btn_delete.Enabled = treeView.SelectedNode.Tag.GetType().GetInterfaces().Contains(typeof(IFirewallRule));
            btn_port.Enabled = treeView.SelectedNode.Tag is IFirewallProfile;
            btn_app.Enabled = treeView.SelectedNode.Tag is IFirewallProfile;
        }

        private void RefreshTreeView()
        {
            treeView.Nodes.Clear();
            var topLevelNode = new TreeNode(FirewallManager.Instance.Name) {Tag = FirewallManager.Instance};
            treeView.Nodes.Add(topLevelNode);
            NodeDiscovery(topLevelNode);
        }

        private void buttonThirdParty_Click(object sender, EventArgs e)
        {
            using (ThirdPartyFirewalls form = new ThirdPartyFirewalls())
            {
                form.ShowDialog();
            }
        }
    }
}