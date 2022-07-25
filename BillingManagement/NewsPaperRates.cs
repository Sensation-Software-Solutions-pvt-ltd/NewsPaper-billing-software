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
    public partial class NewsPaperRates : Form
    {
        BillingManagement bm = null;
        public NewsPaperRates()
        {
            InitializeComponent();
        }

        private void NewsPaperRates_Load(object sender, EventArgs e)
        {

            BillingManagement bm = null;
            try
            {
                bm = (BillingManagement)this.MdiParent;
                using (var context = new BillMgmtEntities())
                {
                    List<NewsPaper> ns = context.NewsPapers.ToList();
                    cmbNewsPapers.DataSource = ns;
                    cmbNewsPapers.DisplayMember = "PaperName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void cmbNewsPapers_SelectedIndexChanged(object sender, EventArgs e)
        {
            bingGrdNewsPaperRates();
        }

        private void bingGrdNewsPaperRates()
        {
            NewsPaper selectedNewsPaper = cmbNewsPapers.SelectedItem as NewsPaper;
            try
            {
                using (var context = new BillMgmtEntities())
                {
                    var NewsPaperRatenpr = context.NewsPaperRates.Where(c => c.NewsPaperId == selectedNewsPaper.Id).OrderByDescending(c => c.Startdate).Select(d => new
                    {
                        NID = d.Id,
                        StartDate = d.Startdate,
                        SundayCost = d.Sunday,
                        MondayCost = d.Monday,
                        TuesdayCost = d.Tuesday,
                        WednesdayCost = d.Wednesday,
                        ThursdayCost = d.Thursday,
                        FridayCost = d.Friday,
                        SaturdayCost = d.Saturday

                    }).ToList();

                    grdNewsPaperRates.DataSource = NewsPaperRatenpr;
                    grdNewsPaperRates.Columns["NID"].Visible = false;

                    if (!grdNewsPaperRates.Columns.Contains("checkBoxDelete"))
                    {
                        DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                        checkBoxColumn.HeaderText = "";
                        checkBoxColumn.Width = 30;
                        checkBoxColumn.Name = "checkBoxDelete";
                        grdNewsPaperRates.Columns.Add(checkBoxColumn);
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

            foreach (DataGridViewRow row in grdNewsPaperRates.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["checkBoxDelete"].Value);
                if (isSelected)
                {
                    selectedForDeleteCount++;
                }
            }

            if (selectedForDeleteCount == grdNewsPaperRates.Rows.Count)
            {
                MessageBox.Show("We can't delete all. We need to keep at least one.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (selectedForDeleteCount > 0)
            {
                var confirm = MessageBox.Show("Are you sure you want to delete the selected Items??" + Environment.NewLine + "You need to generate new bills if any one already generated with this rate value.", "Confirm Change!!", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    using (var context = new BillMgmtEntities())
                    {
                        foreach (DataGridViewRow row in grdNewsPaperRates.Rows)
                        {
                            bool isSelected = Convert.ToBoolean(row.Cells["checkBoxDelete"].Value);
                            if (isSelected)
                            {
                                int ID = (int)row.Cells["NID"].Value;
                                NewsPaperRate npr = context.NewsPaperRates.Where(c => c.Id == ID).FirstOrDefault();
                                context.NewsPaperRates.Remove(npr);
                            }
                        }
                        context.SaveChanges();
                        bingGrdNewsPaperRates();
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
