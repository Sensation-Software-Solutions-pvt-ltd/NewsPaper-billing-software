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
using System.Data.Entity;

namespace BillingManagement
{
    public partial class BulkSupply : Form
    {
        BillingManagement bm { get { return (BillingManagement)this.MdiParent; } }
        public BulkSupply()
        {
            InitializeComponent();
        }

        private void BulkSupply_Load(object sender, EventArgs e)
        {  
            try
            {
                using (var context = new BillMgmtEntities())
                {
                    List<NewsPaperDropdown> ns = context.NewsPapers.Select(d=> new NewsPaperDropdown() { Id=d.Id, PaperName=d.PaperName}).ToList();
                    NewsPaperDropdown ns0 = new NewsPaperDropdown();
                    ns0.Id = 0;
                    ns0.PaperName = "Select News Paper";
                    ns.Insert(0, ns0);
                    cmbNewsPapers.DataSource = ns;
                    cmbNewsPapers.DisplayMember = "PaperName";                   
                }
                dtSupplyDate.Value = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void cmbNewsPapers_SelectedIndexChanged(object sender, EventArgs e)
        {
            bingGrdBulkSupply();
        }
        private void dtSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            bingGrdBulkSupply();
        }

        private void bingGrdBulkSupply()
        {
            NewsPaperDropdown selectedNewsPaper = cmbNewsPapers.SelectedItem as NewsPaperDropdown;
            DateTime dt = dtSupplyDate.Value.Date;
            if(selectedNewsPaper.Id==0)
            {
                dt = DateTime.Now.Date;
                numCopies.Value = 0;
            }

            try
            {
                using (var context = new BillMgmtEntities())
                {
                    if (selectedNewsPaper.Id == 0)
                    {
                        var NewsPaperRatenpr = context.ManualBulkSupplies.OrderByDescending(c => c.SupplyDate).Select(d => new
                        {
                            NID = d.Id,
                            NewsPaperName = d.NewsPaper.PaperName,
                            NumberOfCopies = d.NumberOfCopies,
                            SupplyDate = d.SupplyDate

                        }).ToList();
                        grdBulkSupply.DataSource = NewsPaperRatenpr;
                    }
                    else
                    {
                        var NewsPaperRatenpr = context.ManualBulkSupplies.Where(c => c.NewsPaperId == selectedNewsPaper.Id && DbFunctions.TruncateTime(c.SupplyDate) == dt).OrderByDescending(c => c.SupplyDate).Select(d => new
                        {
                            NID = d.Id,
                            NewsPaperName = d.NewsPaper.PaperName,
                            NumberOfCopies = d.NumberOfCopies,
                            SupplyDate = d.SupplyDate

                        }).ToList();
                        grdBulkSupply.DataSource = NewsPaperRatenpr;
                    }

                    grdBulkSupply.Columns["NID"].Visible = false;

                    if (!grdBulkSupply.Columns.Contains("checkBoxDelete"))
                    {
                        DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                        checkBoxColumn.HeaderText = "";
                        checkBoxColumn.Width = 30;
                        checkBoxColumn.Name = "checkBoxDelete";
                        grdBulkSupply.Columns.Add(checkBoxColumn);
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

            foreach (DataGridViewRow row in grdBulkSupply.Rows)
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
                        foreach (DataGridViewRow row in grdBulkSupply.Rows)
                        {
                            bool isSelected = Convert.ToBoolean(row.Cells["checkBoxDelete"].Value);
                            if (isSelected)
                            {
                                int ID = (int)row.Cells["NID"].Value;
                                ManualBulkSupply npr = context.ManualBulkSupplies.Where(c => c.Id == ID).FirstOrDefault();
                                context.ManualBulkSupplies.Remove(npr);
                            }
                        }
                        context.SaveChanges();
                        bingGrdBulkSupply();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select atleast one item for delete.");
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NewsPaperDropdown selectedNewsPaper = cmbNewsPapers.SelectedItem as NewsPaperDropdown;
            DateTime dt = dtSupplyDate.Value.Date;
            int numberofcopies = (int)numCopies.Value;

            if (selectedNewsPaper.Id == 0)
            {
                MessageBox.Show("Please select News Paper", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbNewsPapers.Focus();
                return ;
            }

            if (numCopies.Value <= 0)
            {
                MessageBox.Show("Please enter number of copies more then 0;", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                numCopies.Focus();
                return;
            }


            using (var context = new BillMgmtEntities())
            {
                ManualBulkSupply mbs = context.ManualBulkSupplies.Where(c => c.NewsPaperId == selectedNewsPaper.Id && DbFunctions.TruncateTime(c.SupplyDate) == dt).FirstOrDefault();
                if(mbs != null)
                {
                    mbs.NumberOfCopies = numberofcopies + mbs.NumberOfCopies;
                }
                else
                {
                    mbs = new ManualBulkSupply();
                    mbs.NewsPaperId = selectedNewsPaper.Id;
                    mbs.NumberOfCopies = numberofcopies;
                    mbs.SupplyDate = dt;
                    context.ManualBulkSupplies.Add(mbs);
                }
                context.SaveChanges();
                bingGrdBulkSupply();
            }
        }

       
    }
}
