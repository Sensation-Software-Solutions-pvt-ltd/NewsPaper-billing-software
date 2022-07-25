using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BillingManagement
{
    public partial class BillingManagement : Form
    {
        public int LoginId = 0;
        private string formtitle = "BIlling Management";
        Home ds = null;
        Customers Cs = null;
        NewsPapers ns = null;
        NewsPaperRates nsr = null;
        MapNewsPaperWithCustomer mnpwc = null;
        Invoices inv = null;
        BulkSupply bs = null;
        public Login lg = null;

        public BillingManagement()
        {
            InitializeComponent();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void CloseAllMDIChildreen(Form form)
        {
            ds = form != ds ? null : ds;
            Cs = form != Cs ? null : Cs;
            ns = form != ns ? null : ns;
            nsr = form != nsr ? null : nsr;
            mnpwc = form != mnpwc ? null : mnpwc;
            inv = form != inv ? null : inv;
            bs = form != bs ? null : bs;

            foreach (Form frm in this.MdiChildren)
            {
                if (frm != form)
                {
                    frm.Dispose();
                    return;
                }
            }
        }

        private void BillingManagement_Load(object sender, EventArgs e)
        {
            ds = new Home();
            ds.MdiParent = this;
            ds.Dock = DockStyle.Fill;
            ds.Show();
            this.Text = ds.Text + " - " + formtitle;
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChildreen(ds);
            if (ds == null)
            {
                ds = new Home();
                ds.MdiParent = this;
                ds.Dock = DockStyle.Fill;
                ds.Show();
                this.Text = ds.Text + " - " + formtitle;
                this.LayoutMdi(MdiLayout.Cascade);
            }
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChildreen(Cs);
            if (Cs == null)
            {
                Cs = new Customers();
                Cs.MdiParent = this;
                Cs.Dock = DockStyle.Fill;
                Cs.Show();
                this.Text = Cs.Text + " - " + formtitle;
                this.LayoutMdi(MdiLayout.Cascade);
            }

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lg.Show();
            this.Close();
            this.Dispose();
        }

        private void BillingManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            lg.Show();
        }

        private void newsPapersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChildreen(ns);
            if (ns == null)
            {
                ns = new NewsPapers();
                ns.MdiParent = this;
                ns.Dock = DockStyle.Fill;
                ns.Show();
                this.Text = ns.Text + " - " + formtitle;
                this.LayoutMdi(MdiLayout.Cascade);
            }
        }

        private void newsPaperRatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChildreen(nsr);
            if (nsr == null)
            {
                nsr = new NewsPaperRates();
                nsr.MdiParent = this;
                nsr.Dock = DockStyle.Fill;
                nsr.Show();
                this.Text = nsr.Text + " - " + formtitle;
                this.LayoutMdi(MdiLayout.Cascade);
            }
        }

        private void newPaperAllocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChildreen(mnpwc);
            if (mnpwc == null)
            {
                mnpwc = new MapNewsPaperWithCustomer();
                mnpwc.MdiParent = this;
                mnpwc.Dock = DockStyle.Fill;
                mnpwc.Show();
                this.Text = mnpwc.Text + " - " + formtitle;
                this.LayoutMdi(MdiLayout.Cascade);
            }
        }

        private void noSuplyDatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NoSupplyDates nsd = new NoSupplyDates();
            nsd.ShowDialog();
        }

        private void invoicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChildreen(mnpwc);
            if (inv == null)
            {
                inv = new Invoices();
                inv.MdiParent = this;
                inv.Dock = DockStyle.Fill;
                inv.Show();
                this.Text = inv.Text + " - " + formtitle;
                this.LayoutMdi(MdiLayout.Cascade);
            }
        }

        private void manualSuplyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChildreen(mnpwc);
            if (bs == null)
            {
                bs = new BulkSupply();
                bs.MdiParent = this;
                bs.Dock = DockStyle.Fill;
                bs.Show();
                this.Text = bs.Text + " - " + formtitle;
                this.LayoutMdi(MdiLayout.Cascade);
            }
        }
    }
}
