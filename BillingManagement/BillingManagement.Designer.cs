namespace BillingManagement
{
    partial class BillingManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BillingManagement));
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newsPapersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newPaperAllocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newsPaperRatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualSuplyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invoicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.profitLossToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noSuplyDatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.menuStrip2.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.masterSettingsToolStripMenuItem,
            this.manualEntryToolStripMenuItem,
            this.noSuplyDatesToolStripMenuItem,
            this.logoutToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(837, 24);
            this.menuStrip2.TabIndex = 4;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.homeToolStripMenuItem.Text = "Home";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.homeToolStripMenuItem_Click);
            // 
            // masterSettingsToolStripMenuItem
            // 
            this.masterSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newsPapersToolStripMenuItem,
            this.customersToolStripMenuItem,
            this.newPaperAllocationToolStripMenuItem,
            this.newsPaperRatesToolStripMenuItem});
            this.masterSettingsToolStripMenuItem.Name = "masterSettingsToolStripMenuItem";
            this.masterSettingsToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.masterSettingsToolStripMenuItem.Text = "Master Settings";
            // 
            // newsPapersToolStripMenuItem
            // 
            this.newsPapersToolStripMenuItem.Name = "newsPapersToolStripMenuItem";
            this.newsPapersToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.newsPapersToolStripMenuItem.Text = "News Papers";
            this.newsPapersToolStripMenuItem.Click += new System.EventHandler(this.newsPapersToolStripMenuItem_Click);
            // 
            // customersToolStripMenuItem
            // 
            this.customersToolStripMenuItem.Name = "customersToolStripMenuItem";
            this.customersToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.customersToolStripMenuItem.Text = "Customers";
            this.customersToolStripMenuItem.Click += new System.EventHandler(this.customersToolStripMenuItem_Click);
            // 
            // newPaperAllocationToolStripMenuItem
            // 
            this.newPaperAllocationToolStripMenuItem.Name = "newPaperAllocationToolStripMenuItem";
            this.newPaperAllocationToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.newPaperAllocationToolStripMenuItem.Text = "News Paper Allocation";
            this.newPaperAllocationToolStripMenuItem.Click += new System.EventHandler(this.newPaperAllocationToolStripMenuItem_Click);
            // 
            // newsPaperRatesToolStripMenuItem
            // 
            this.newsPaperRatesToolStripMenuItem.Name = "newsPaperRatesToolStripMenuItem";
            this.newsPaperRatesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.newsPaperRatesToolStripMenuItem.Text = "News Paper Rates";
            this.newsPaperRatesToolStripMenuItem.Click += new System.EventHandler(this.newsPaperRatesToolStripMenuItem_Click);
            // 
            // manualEntryToolStripMenuItem
            // 
            this.manualEntryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualSuplyToolStripMenuItem,
            this.invoicesToolStripMenuItem,
            this.profitLossToolStripMenuItem});
            this.manualEntryToolStripMenuItem.Name = "manualEntryToolStripMenuItem";
            this.manualEntryToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
            this.manualEntryToolStripMenuItem.Text = "Profit and Loss";
            // 
            // manualSuplyToolStripMenuItem
            // 
            this.manualSuplyToolStripMenuItem.Name = "manualSuplyToolStripMenuItem";
            this.manualSuplyToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.manualSuplyToolStripMenuItem.Text = "Manual Supply";
            this.manualSuplyToolStripMenuItem.Click += new System.EventHandler(this.manualSuplyToolStripMenuItem_Click);
            // 
            // invoicesToolStripMenuItem
            // 
            this.invoicesToolStripMenuItem.Name = "invoicesToolStripMenuItem";
            this.invoicesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.invoicesToolStripMenuItem.Text = "Invoices";
            this.invoicesToolStripMenuItem.Click += new System.EventHandler(this.invoicesToolStripMenuItem_Click);
            // 
            // profitLossToolStripMenuItem
            // 
            this.profitLossToolStripMenuItem.Name = "profitLossToolStripMenuItem";
            this.profitLossToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.profitLossToolStripMenuItem.Text = "Profit and Loss";
            // 
            // noSuplyDatesToolStripMenuItem
            // 
            this.noSuplyDatesToolStripMenuItem.Name = "noSuplyDatesToolStripMenuItem";
            this.noSuplyDatesToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.noSuplyDatesToolStripMenuItem.Text = "No Supply Dates";
            this.noSuplyDatesToolStripMenuItem.Click += new System.EventHandler(this.noSuplyDatesToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 489);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(837, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // BillingManagement
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(837, 511);
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip2;
            this.MinimumSize = new System.Drawing.Size(853, 550);
            this.Name = "BillingManagement";
            this.Text = "Billing Management";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BillingManagement_FormClosed);
            this.Load += new System.EventHandler(this.BillingManagement_Load);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem masterSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newsPapersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newPaperAllocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noSuplyDatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newsPaperRatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualSuplyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invoicesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem profitLossToolStripMenuItem;
    }
}



