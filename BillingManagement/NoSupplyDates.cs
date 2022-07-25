using BillingManagement.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BillingManagement
{
    public partial class NoSupplyDates : Form
    {
        public NoSupplyDates()
        {
            InitializeComponent();
        }

        private void NoSupplyDates_Load(object sender, EventArgs e)
        {
            bindList();
        }

        private void bindList()
        {
            chklistNoSupplyDates.Items.Clear();
            using (var context = new BillMgmtEntities())
            {
                List<NoSupplyDate> nsd = context.NoSupplyDates.ToList();
                foreach (var item in nsd)
                {
                    chklistNoSupplyDates.Items.Add(item.NoSupplyDate1.Value.Date);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new BillMgmtEntities())
                {
                    NoSupplyDate nsd = context.NoSupplyDates.Where(c => DbFunctions.TruncateTime(c.NoSupplyDate1) == dtNoSupplyDate.Value.Date).FirstOrDefault();
                    if (nsd != null)
                    {
                        MessageBox.Show("Date already exist.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtNoSupplyDate.Focus();
                        return;
                    }
                    NoSupplyDate nsdnew = new NoSupplyDate();
                    nsdnew.EntryDate = DateTime.Now;
                    nsdnew.NoSupplyDate1 = dtNoSupplyDate.Value;
                    context.NoSupplyDates.Add(nsdnew);
                    context.SaveChanges();
                    bindList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (chklistNoSupplyDates.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select atleast one item to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            var confirm = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!!", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (var context = new BillMgmtEntities())
                    {
                        foreach (var item in chklistNoSupplyDates.CheckedItems)
                        {
                            DateTime dt = Convert.ToDateTime(item);
                            NoSupplyDate nsd = context.NoSupplyDates.Where(c => DbFunctions.TruncateTime(c.NoSupplyDate1) == dt.Date).FirstOrDefault();
                            if (nsd != null)
                            {
                                context.NoSupplyDates.Remove(nsd);
                            }
                        }
                        context.SaveChanges();
                        bindList();
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
