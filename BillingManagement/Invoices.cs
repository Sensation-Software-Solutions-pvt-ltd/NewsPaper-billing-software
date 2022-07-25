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
    public partial class Invoices : Form
    {
        BillingManagement bm { get { return (BillingManagement)this.MdiParent; } }
        public Invoices()
        {
            InitializeComponent();
        }

        private void Invoices_Load(object sender, EventArgs e)
        {
            try
            {
                bingGrdInvoices();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }



        private void bingGrdInvoices()
        {
            try
            {
                using (var context = new BillMgmtEntities())
                {
                    var Invoices = context.Invoices.OrderByDescending(c => c.Id).Select(d => new
                    {
                        NID = d.Id,
                        CustomerName = d.Customer.FirstName + " " + d.Customer.LastName,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate,
                        Amount = d.Amount,
                        Status = d.IsPaid.Value ? "Paid" : "Unpaid"

                    }).ToList();

                    grdInvoices.DataSource = Invoices;
                    grdInvoices.Columns["NID"].Visible = false;

                    if (!grdInvoices.Columns.Contains("checkBoxDelete"))
                    {
                        DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                        checkBoxColumn.HeaderText = "";
                        checkBoxColumn.Width = 30;
                        checkBoxColumn.Name = "checkBoxDelete";
                        grdInvoices.Columns.Add(checkBoxColumn);
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

            foreach (DataGridViewRow row in grdInvoices.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["checkBoxDelete"].Value);
                if (isSelected)
                {
                    selectedForDeleteCount++;
                }
            }

            if (selectedForDeleteCount > 0)
            {
                var confirm = MessageBox.Show("Are you sure you want to delete the selected Items??" + Environment.NewLine + "You need to generate new bills if any one already generated with this rate value.", "Confirm Change!!", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    using (var context = new BillMgmtEntities())
                    {
                        foreach (DataGridViewRow row in grdInvoices.Rows)
                        {
                            bool isSelected = Convert.ToBoolean(row.Cells["checkBoxDelete"].Value);
                            if (isSelected)
                            {
                                int ID = (int)row.Cells["NID"].Value;
                                Invoice inv = context.Invoices.Where(c => c.Id == ID).FirstOrDefault();
                                while (inv.InvoiceDetails.Count() > 0)
                                {
                                    var invd = inv.InvoiceDetails.FirstOrDefault();
                                    context.InvoiceDetails.Remove(invd);
                                }

                                context.Invoices.Remove(inv);
                            }
                        }
                        context.SaveChanges();
                        bingGrdInvoices();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select atleast one item for delete.");
            }

        }
    }
}
