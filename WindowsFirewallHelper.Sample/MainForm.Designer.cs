namespace WindowsFirewallHelper.Sample
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.treeView = new System.Windows.Forms.TreeView();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.btn_app = new System.Windows.Forms.Button();
			this.btn_port = new System.Windows.Forms.Button();
			this.btn_delete = new System.Windows.Forms.Button();
			this.ofd_app = new System.Windows.Forms.OpenFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonThirdParty = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView.Location = new System.Drawing.Point(12, 35);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(669, 658);
			this.treeView.TabIndex = 0;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ItemSelected);
			this.treeView.DoubleClick += new System.EventHandler(this.ItemClicked);
			// 
			// propertyGrid
			// 
			this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid.Location = new System.Drawing.Point(687, 35);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
			this.propertyGrid.Size = new System.Drawing.Size(369, 590);
			this.propertyGrid.TabIndex = 1;
			this.propertyGrid.ToolbarVisible = false;
			// 
			// btn_app
			// 
			this.btn_app.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_app.Location = new System.Drawing.Point(953, 670);
			this.btn_app.Name = "btn_app";
			this.btn_app.Size = new System.Drawing.Size(103, 23);
			this.btn_app.TabIndex = 2;
			this.btn_app.Text = "Create App Rule";
			this.btn_app.UseVisualStyleBackColor = true;
			this.btn_app.Click += new System.EventHandler(this.AddApplicationRule);
			// 
			// btn_port
			// 
			this.btn_port.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_port.Location = new System.Drawing.Point(844, 670);
			this.btn_port.Name = "btn_port";
			this.btn_port.Size = new System.Drawing.Size(103, 23);
			this.btn_port.TabIndex = 3;
			this.btn_port.Text = "Create Port Rule";
			this.btn_port.UseVisualStyleBackColor = true;
			this.btn_port.Click += new System.EventHandler(this.AddPortRule);
			// 
			// btn_delete
			// 
			this.btn_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_delete.Location = new System.Drawing.Point(687, 670);
			this.btn_delete.Name = "btn_delete";
			this.btn_delete.Size = new System.Drawing.Size(103, 23);
			this.btn_delete.TabIndex = 4;
			this.btn_delete.Text = "Delete Rule";
			this.btn_delete.UseVisualStyleBackColor = true;
			this.btn_delete.Click += new System.EventHandler(this.DeleteRule);
			// 
			// ofd_app
			// 
			this.ofd_app.Filter = "Executable Files|*.exe";
			this.ofd_app.Title = "Select an executable file";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(687, 632);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(366, 32);
			this.label1.TabIndex = 5;
			this.label1.Text = "Select a profile to create a simple rule or click a rule to delete. Use the prope" +
    "rty panel to modify each rule.";
			// 
			// buttonThirdParty
			// 
			this.buttonThirdParty.Location = new System.Drawing.Point(13, 4);
			this.buttonThirdParty.Name = "buttonThirdParty";
			this.buttonThirdParty.Size = new System.Drawing.Size(127, 23);
			this.buttonThirdParty.TabIndex = 6;
			this.buttonThirdParty.Text = "Third Party Firewalls";
			this.buttonThirdParty.UseVisualStyleBackColor = true;
			this.buttonThirdParty.Click += new System.EventHandler(this.buttonThirdParty_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1068, 705);
			this.Controls.Add(this.buttonThirdParty);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btn_delete);
			this.Controls.Add(this.btn_port);
			this.Controls.Add(this.btn_app);
			this.Controls.Add(this.propertyGrid);
			this.Controls.Add(this.treeView);
			this.Name = "MainForm";
			this.Text = "Simple Firewall Configurator";
			this.Load += new System.EventHandler(this.FormLoad);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button btn_app;
        private System.Windows.Forms.Button btn_port;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.OpenFileDialog ofd_app;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonThirdParty;
	}
}

