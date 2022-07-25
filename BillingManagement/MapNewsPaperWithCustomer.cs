using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BillingManagement.Entity;
using BillingManagement.Models;

namespace BillingManagement
{
    public partial class MapNewsPaperWithCustomer : Form
    {
        BillingManagement bm = null;
        int NPAID = 0;

        public MapNewsPaperWithCustomer()
        {
            InitializeComponent();
        }

        private void NewsPapers_Load(object sender, EventArgs e)
        {
            try
            {
                bm = (BillingManagement)this.MdiParent;
                using (var context = new BillMgmtEntities())
                {
                    List<NewsPaper> ns = context.NewsPapers.ToList();
                    cbNewsPaper.DataSource = ns;
                    cbNewsPaper.DisplayMember = "PaperName";

                    List<CustomerDropdown> cs = context.Customers.Select(c => new CustomerDropdown() { CustomerId = c.Id, CustomerName = c.FirstName + " " + c.LastName + "(" + c.AdharNumber + ")" }).ToList();
                    CustomerDropdown csnew = new CustomerDropdown();
                    csnew.CustomerId = 0;
                    csnew.CustomerName = "Select Customer";
                    cs.Insert(0, csnew);
                    cbCustomers.SelectedIndexChanged -= new System.EventHandler(this.cbCustomers_SelectedIndexChanged);
                    cbCustomers.DataSource = cs;
                    cbCustomers.DisplayMember = "CustomerName";
                    bingGrdNewsPaperRates(0);
                    cbCustomers.SelectedIndexChanged += new System.EventHandler(this.cbCustomers_SelectedIndexChanged);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private void bingGrdNewsPaperRates(int CustomerId)
        {
            try
            {
                NPAID = 0;               
                btnCancel.Visible = false;
                btnSaveUpdate.Text = "Save";
                chkifEnd.Checked = false;
                dtEnd.Enabled = false;
                dtStart.Value = DateTime.Now;
                dtEnd.Value = DateTime.Now;
                cbCustomers.Enabled = true;
                cbNewsPaper.Enabled = true;

                using (var context = new BillMgmtEntities())
                {
                    if (CustomerId == 0)
                    {
                        var NSA = context.NewsPaperAllocations.Select(d => new { NPAID = d.Id, CustomerName = d.Customer.FirstName + " " + d.Customer.LastName, d.Customer.AdharNumber, d.NewsPaper.PaperName, d.StartDate, d.EndDate }).ToList();
                        grdNewsPapers.DataSource = NSA;
                    }
                    else
                    {
                        var NSA = context.NewsPaperAllocations.Where(c => c.CustomerId == CustomerId).Select(d => new { NPAID = d.Id, CustomerName = d.Customer.FirstName + " " + d.Customer.LastName, d.Customer.AdharNumber, d.NewsPaper.PaperName, d.StartDate, d.EndDate }).ToList();
                        grdNewsPapers.DataSource = NSA;
                    }


                    grdNewsPapers.Columns["NPAID"].Visible = false;

                    if (!grdNewsPapers.Columns.Contains("checkBoxDelete"))
                    {
                        DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                        checkBoxColumn.HeaderText = "";
                        checkBoxColumn.Width = 30;
                        checkBoxColumn.Name = "checkBoxDelete";
                        grdNewsPapers.Columns.Add(checkBoxColumn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedForDeleteCount = 0;

            foreach (DataGridViewRow row in grdNewsPapers.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["checkBoxDelete"].Value);
                if (isSelected)
                {
                    selectedForDeleteCount++;
                }
            }
            if (selectedForDeleteCount > 0)
            {
                var confirm = MessageBox.Show("Are you sure you want to delete the selected Items??" + Environment.NewLine + "By deleting may need to generate bill again.", "Confirm Change!!", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    using (var context = new BillMgmtEntities())
                    {
                        foreach (DataGridViewRow row in grdNewsPapers.Rows)
                        {
                            bool isSelected = Convert.ToBoolean(row.Cells["checkBoxDelete"].Value);
                            if (isSelected)
                            {
                                int ID = (int)row.Cells["NPAID"].Value;
                                NewsPaperAllocation npr = context.NewsPaperAllocations.Where(c => c.Id == ID).FirstOrDefault();
                                context.NewsPaperAllocations.Remove(npr);
                            }
                        }

                        context.SaveChanges();
                        CustomerDropdown customerdd = cbCustomers.SelectedItem as CustomerDropdown;
                        bingGrdNewsPaperRates(customerdd.CustomerId);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select atleast one item for delete.");
            }
        }

        private void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateNewspaperData()) return;
                using (var context = new BillMgmtEntities())
                {
                    CustomerDropdown customerdd = cbCustomers.SelectedItem as CustomerDropdown;
                    NewsPaper NewsPaperdd = cbNewsPaper.SelectedItem as NewsPaper;
                    if (NPAID == 0)
                    {
                        NewsPaperAllocation npa = new NewsPaperAllocation();
                        npa.CustomerId = customerdd.CustomerId;
                        npa.NewspaperId = NewsPaperdd.Id;
                        npa.StartDate = dtStart.Value.Date;
                        npa.NumberOfCopies = Convert.ToInt32(txtCopies.Value);
                        if (chkifEnd.Checked)
                            npa.EndDate = dtEnd.Value.Date;
                        else
                            npa.EndDate = null;

                        npa.CreatedBy = bm.LoginId;
                        context.NewsPaperAllocations.Add(npa);
                    }
                    else
                    {
                        NewsPaperAllocation npa = context.NewsPaperAllocations.Where(c => c.Id == NPAID).FirstOrDefault();
                        npa.StartDate = dtStart.Value.Date;
                        npa.NumberOfCopies = Convert.ToInt32(txtCopies.Value);
                        if (chkifEnd.Checked)
                            npa.EndDate = dtEnd.Value.Date;
                        else
                            npa.EndDate = null;

                        npa.CreatedBy = bm.LoginId;

                    }
                    context.SaveChanges();
                    bingGrdNewsPaperRates(customerdd.CustomerId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateNewspaperData()
        {
            if (cbCustomers.SelectedIndex == 0)
            {
                MessageBox.Show("Please select Customer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbCustomers.Focus();
                return false;
            }
            CustomerDropdown customerdd = cbCustomers.SelectedItem as CustomerDropdown;
            NewsPaper NewsPaperdd = cbNewsPaper.SelectedItem as NewsPaper;

            List<NewsPaperAllocation> nprs = new List<NewsPaperAllocation>();
            using (var context = new BillMgmtEntities())
            {
                nprs = context.NewsPaperAllocations.Where(c => c.CustomerId == customerdd.CustomerId && c.NewspaperId == NewsPaperdd.Id).ToList();
            }

            if (nprs.Exists(c => c.StartDate > dtStart.Value && c.StartDate < dtEnd.Value && c.Id != NPAID))
            {
                MessageBox.Show("There is another record conflicting with same. Please change the start or end date.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtStart.Focus();
                return false;
            }

            if (nprs.Exists(c => c.EndDate > dtStart.Value && c.EndDate < dtEnd.Value && c.Id != NPAID))
            {
                MessageBox.Show("There is another record conflicting with same. Please change the start or end date.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtStart.Focus();
                return false;
            }

            if (nprs.Exists(c => c.StartDate <= dtStart.Value && c.EndDate >= dtStart.Value && c.Id != NPAID))
            {
                MessageBox.Show("There is another record conflicting with same. Please change the start or end date.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtStart.Focus();
                return false;
            }

            if (nprs.Exists(c => c.StartDate >= dtStart.Value && c.EndDate == null && !chkifEnd.Checked && c.Id != NPAID))
            {
                MessageBox.Show("There is another record conflicting with same. Please change the start or end date.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtStart.Focus();
                return false;
            }

            if (nprs.Exists(c => c.StartDate <= dtEnd.Value && c.EndDate >= dtEnd.Value && chkifEnd.Checked && c.Id != NPAID))
            {
                MessageBox.Show("There is another record conflicting with same. Please change the start or end date.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtEnd.Focus();
                return false;
            }

            return true;
        }

        private void cbCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerDropdown customerdd = cbCustomers.SelectedItem as CustomerDropdown;           
            bingGrdNewsPaperRates(customerdd.CustomerId);
        }

        private void grdNewsPapers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            NPAID = (int)grdNewsPapers.Rows[e.RowIndex].Cells["NPAID"].Value;

            using (var context = new BillMgmtEntities())
            {
                NewsPaperAllocation npr = context.NewsPaperAllocations.Where(c => c.Id == NPAID).FirstOrDefault();
                int ind = 0; 
                foreach(var item in cbCustomers.Items)
                {                    
                    CustomerDropdown cd = item as CustomerDropdown;
                    if (cd.CustomerId == npr.CustomerId)
                        break;
                    ind++;
                }

                cbCustomers.SelectedIndexChanged -= new System.EventHandler(this.cbCustomers_SelectedIndexChanged);
                cbCustomers.SelectedIndex = ind;
                cbCustomers.SelectedIndexChanged += new System.EventHandler(this.cbCustomers_SelectedIndexChanged);

                int ind1 = 0;
                foreach (var item in cbNewsPaper.Items)
                {                   
                    NewsPaper np = item as NewsPaper;
                    if (np.Id == npr.NewspaperId)
                        break;
                    ind1++;
                }

                cbNewsPaper.SelectedIndex = ind1;
                txtCopies.Value = npr.NumberOfCopies.Value;
                dtStart.Value = npr.StartDate.Value;

                if (npr.EndDate != null)
                {
                    chkifEnd.Checked = true;
                    dtEnd.Value = npr.EndDate.Value;
                    dtEnd.Enabled = true;
                }
            }
            cbCustomers.Enabled = false;
            cbNewsPaper.Enabled = false;
            btnCancel.Visible = true;
            btnSaveUpdate.Text = "Update";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NPAID = 0;
            cbCustomers.Enabled = true;
            cbNewsPaper.Enabled = true;
            cbCustomers.SelectedIndex = 0;
            cbNewsPaper.SelectedIndex = 0;
            btnCancel.Visible = false;
            btnSaveUpdate.Text = "Save";
            chkifEnd.Checked = false;
            dtEnd.Enabled = false;
            dtStart.Value = DateTime.Now;
            dtEnd.Value = DateTime.Now;
        }

        private void chkifEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkifEnd.Checked)
                dtEnd.Enabled = true;
            else
                dtEnd.Enabled = false;
        }
    }
}
