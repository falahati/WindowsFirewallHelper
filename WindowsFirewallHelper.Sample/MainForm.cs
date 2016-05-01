using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFirewallHelper.Sample
{
    internal partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Text =
                $"{FirewallManager.Instance.Name} ({FirewallManager.Version})";
            RefreshTreeView();
        }

        private void RefreshTreeView()
        {
            treeView.Nodes.Clear();
            var topLevelNode = new TreeNode(FirewallManager.Instance.Name) {Tag = FirewallManager.Instance};
            treeView.Nodes.Add(topLevelNode);
            NodeDiscovery(topLevelNode);
        }

        private void NodeDiscovery(TreeNode node)
        {
            var o = node.Tag;
            if (o == null)
            {
                return;
            }
            node.Nodes.Clear();
            if (o.GetType().GetInterfaces().Contains(typeof (IEnumerable)))
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
                    if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof (IEnumerable)) &&
                        propertyInfo.PropertyType != typeof (string))
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

        private void ItemSelected(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject =
                !treeView.SelectedNode.Tag.GetType().GetInterfaces().Contains(typeof (IEnumerable))
                    ? treeView.SelectedNode.Tag
                    : null;
            btn_delete.Enabled = treeView.SelectedNode.Tag.GetType().GetInterfaces().Contains(typeof (IRule));
            btn_port.Enabled = treeView.SelectedNode.Tag is ActiveCollection<IRule>;
            btn_app.Enabled = treeView.SelectedNode.Tag is ActiveCollection<IRule>;
        }

        private void ItemClicked(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                NodeDiscovery(treeView.SelectedNode);
            }
        }

        private void AddPortRule(object sender, EventArgs e)
        {
            try
            {
                var portDialog = new EditPortForm();
                if (portDialog.ShowDialog() == DialogResult.OK)
                {
                    var rule = FirewallManager.Instance.CreatePortRule(
                        FirewallManager.Instance.GetProfile().Type,
                        $"Port {portDialog.PortNumber} - Protocol #{portDialog.FirewallProtocol}",
                        FirewallAction.Allow, portDialog.PortNumber, portDialog.FirewallProtocol);
                    FirewallManager.Instance.Rules.Add(rule);
                    RefreshTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddApplicationRule(object sender, EventArgs e)
        {
            try
            {
                if (ofd_app.ShowDialog() == DialogResult.OK)
                {
                    var rule = FirewallManager.Instance.CreateApplicationRule(
                        FirewallManager.Instance.GetProfile().Type, ofd_app.SafeFileName, FirewallAction.Allow,
                        ofd_app.FileName);
                    rule.Direction = FirewallDirection.Outbound;
                    FirewallManager.Instance.Rules.Add(rule);
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
                var rule = treeView.SelectedNode.Tag as IRule;
                if (rule != null)
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
    }
}