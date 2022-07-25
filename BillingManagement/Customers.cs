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

namespace BillingManagement
{
    public partial class Customers : Form
    {
        int CustomerId = 0;
        BillingManagement bm = null;
        public Customers()
        {
            InitializeComponent();
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            try
            {
                bm = (BillingManagement)this.MdiParent;
                bindCustomerGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void bindCustomerGrid()
        {
            CustomerId = 0;
            txtFirstName.Text = "";
            txtlastName.Text = "";
            dtBirth.Text = "";
            txtAdhar.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            btnDelete.Visible = false;
            grdCustomers.ReadOnly = true;
            using (var context = new BillMgmtEntities())
            {
                grdCustomers.DataSource = context.Customers.Where(c => c.Isactive == true).ToList();

                grdCustomers.Columns["Isactive"].Visible = false; //hide navigation column
                grdCustomers.Columns["CreatedBy"].Visible = false; //hide navigation column
                grdCustomers.Columns["User"].Visible = false; //hide navigation column
                grdCustomers.Columns["NewsPaperAllocations"].Visible = false; //hide navigation column
                grdCustomers.Columns["Id"].Visible = false; //make the id column read only                
                grdCustomers.Columns["Invoices"].Visible = false; //make the id column read only                
            }

        }

        private void grdCustomers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            CustomerId = (int)grdCustomers.Rows[e.RowIndex].Cells[0].Value;
            txtFirstName.Text = grdCustomers.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtlastName.Text = grdCustomers.Rows[e.RowIndex].Cells[2].Value.ToString();
            dtBirth.Text = grdCustomers.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtAdhar.Text = grdCustomers.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtPhone.Text = grdCustomers.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtEmail.Text = grdCustomers.Rows[e.RowIndex].Cells[6].Value.ToString();
            btnDelete.Visible = true;
        }


        private void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateCustomerData()) return;
                Customer customerToAdd = null;
                using (var context = new BillMgmtEntities())
                {
                    if (CustomerId != 0)
                    {
                        customerToAdd = (from c in context.Customers
                                         where c.Id == CustomerId
                                         select c).FirstOrDefault();
                    }
                    else
                    {
                        customerToAdd = new Customer();
                        customerToAdd.CreatedBy = bm.LoginId;
                    }
                    customerToAdd.FirstName = txtFirstName.Text;
                    customerToAdd.LastName = txtlastName.Text;
                    customerToAdd.DOB = Convert.ToDateTime(dtBirth.Text);
                    customerToAdd.AdharNumber = txtAdhar.Text;
                    customerToAdd.Phone = txtPhone.Text;
                    customerToAdd.Email = txtEmail.Text;
                    customerToAdd.Isactive = true;
                    if (CustomerId == 0)
                    {
                        context.Customers.Add(customerToAdd);
                    }
                    context.SaveChanges();
                    bindCustomerGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateCustomerData()
        {
            if (txtFirstName.Text.Trim() == "")
            {
                MessageBox.Show("First name is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFirstName.Focus();
                return false;
            }

            if (txtlastName.Text.Trim() == "")
            {
                MessageBox.Show("Last name is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtlastName.Focus();
                return false;
            }

            if (txtAdhar.Text.Trim() == "")
            {
                MessageBox.Show("Adhar number is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAdhar.Focus();
                return false;
            }

            if (txtPhone.Text.Trim() == "")
            {
                MessageBox.Show("Phone number is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhone.Focus();
                return false;
            }
            return true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CustomerId == 0) return;
            var confirm = MessageBox.Show("Are you sure to delete this item ??","Confirm Delete!!",MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (var context = new BillMgmtEntities())
                    {
                        var customerToDelete = from b in context.Customers
                                               where b.Id == CustomerId
                                               select b;
                        foreach (var customer in customerToDelete)
                            context.Customers.Remove(customer);
                        context.SaveChanges();
                        bindCustomerGrid();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
