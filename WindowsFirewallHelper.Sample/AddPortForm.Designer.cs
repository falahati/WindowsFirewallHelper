namespace WindowsFirewallHelper.Sample
{
    partial class AddPortForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.nud_port = new System.Windows.Forms.NumericUpDown();
			this.cb_protocol = new System.Windows.Forms.ComboBox();
			this.btn_ok = new System.Windows.Forms.Button();
			this.btn_cancel = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxRuleName = new System.Windows.Forms.TextBox();
			this.checkedListBoxProfiles = new System.Windows.Forms.CheckedListBox();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nud_port)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Port Number:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 71);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Protocol:";
			// 
			// nud_port
			// 
			this.nud_port.Location = new System.Drawing.Point(103, 42);
			this.nud_port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nud_port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nud_port.Name = "nud_port";
			this.nud_port.Size = new System.Drawing.Size(105, 20);
			this.nud_port.TabIndex = 1;
			this.nud_port.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
			// 
			// cb_protocol
			// 
			this.cb_protocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_protocol.FormattingEnabled = true;
			this.cb_protocol.Location = new System.Drawing.Point(103, 68);
			this.cb_protocol.Name = "cb_protocol";
			this.cb_protocol.Size = new System.Drawing.Size(105, 21);
			this.cb_protocol.TabIndex = 2;
			// 
			// btn_ok
			// 
			this.btn_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_ok.Location = new System.Drawing.Point(374, 224);
			this.btn_ok.Name = "btn_ok";
			this.btn_ok.Size = new System.Drawing.Size(78, 23);
			this.btn_ok.TabIndex = 5;
			this.btn_ok.Text = "Ok";
			this.btn_ok.UseVisualStyleBackColor = true;
			// 
			// btn_cancel
			// 
			this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_cancel.Location = new System.Drawing.Point(290, 224);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new System.Drawing.Size(78, 23);
			this.btn_cancel.TabIndex = 4;
			this.btn_cancel.Text = "Cancel";
			this.btn_cancel.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Rule Name:";
			// 
			// textBoxRuleName
			// 
			this.textBoxRuleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxRuleName.Location = new System.Drawing.Point(103, 10);
			this.textBoxRuleName.Name = "textBoxRuleName";
			this.textBoxRuleName.Size = new System.Drawing.Size(349, 20);
			this.textBoxRuleName.TabIndex = 0;
			// 
			// checkedListBoxProfiles
			// 
			this.checkedListBoxProfiles.FormattingEnabled = true;
			this.checkedListBoxProfiles.Items.AddRange(new object[] {
            "Domain",
            "Private",
            "Public"});
			this.checkedListBoxProfiles.Location = new System.Drawing.Point(103, 105);
			this.checkedListBoxProfiles.Name = "checkedListBoxProfiles";
			this.checkedListBoxProfiles.Size = new System.Drawing.Size(105, 109);
			this.checkedListBoxProfiles.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 105);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Profiles:";
			// 
			// AddPortForm
			// 
			this.AcceptButton = this.btn_ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btn_cancel;
			this.ClientSize = new System.Drawing.Size(464, 256);
			this.ControlBox = false;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.checkedListBoxProfiles);
			this.Controls.Add(this.textBoxRuleName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btn_cancel);
			this.Controls.Add(this.btn_ok);
			this.Controls.Add(this.cb_protocol);
			this.Controls.Add(this.nud_port);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddPortForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Port Rule";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.nud_port)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nud_port;
        private System.Windows.Forms.ComboBox cb_protocol;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxRuleName;
		private System.Windows.Forms.CheckedListBox checkedListBoxProfiles;
		private System.Windows.Forms.Label label4;
	}
}