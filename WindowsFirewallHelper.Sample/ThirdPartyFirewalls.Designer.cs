namespace WindowsFirewallHelper.Sample
{
    partial class ThirdPartyFirewalls
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
            this.listBoxFirewalls = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxFirewalls
            // 
            this.listBoxFirewalls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFirewalls.FormattingEnabled = true;
            this.listBoxFirewalls.Location = new System.Drawing.Point(0, 0);
            this.listBoxFirewalls.Name = "listBoxFirewalls";
            this.listBoxFirewalls.Size = new System.Drawing.Size(396, 475);
            this.listBoxFirewalls.TabIndex = 0;
            // 
            // ThirdPartyFirewalls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 475);
            this.Controls.Add(this.listBoxFirewalls);
            this.Name = "ThirdPartyFirewalls";
            this.Text = "ThirdPartyFirewalls";
            this.Load += new System.EventHandler(this.ThirdPartyFirewalls_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxFirewalls;
    }
}