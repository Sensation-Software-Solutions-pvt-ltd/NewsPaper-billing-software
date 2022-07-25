using BillingManagement.Entity;
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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text.Trim() == "")
                {
                    MessageBox.Show("Username is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserName.Focus();

                }

                else if (txtPassword.Text.Trim() == "")
                {
                    MessageBox.Show("Password is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPassword.Focus();
                }

                else
                {
                    using (var context = new BillMgmtEntities())
                    {
                        string username = txtUserName.Text;
                        string password = txtPassword.Text;
                        User u = context.Users.Where(c => c.UserName == username && c.Password == password).FirstOrDefault();
                        if (u != null)
                        {
                            BillingManagement dash = new BillingManagement();
                            dash.LoginId = u.Id;
                            dash.lg = this;
                            dash.Show();
                            this.Hide();

                        }
                        else
                        {
                            MessageBox.Show("Username or Password is not correct.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
