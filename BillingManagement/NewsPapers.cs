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
    public partial class NewsPapers : Form
    {
        int NewspaperId = 0;
        int NewsPaperCostId = 0;
        BillingManagement bm = null;
        DateTime? dtCurrent = null;
        public NewsPapers()
        {
            InitializeComponent();
        }

        private void NewsPapers_Load(object sender, EventArgs e)
        {
            try
            {
                bm = (BillingManagement)this.MdiParent;
                bindNewsPaperGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void bindNewsPaperGrid()
        {
            NewspaperId = 0;
            NewsPaperCostId = 0;

            txtPaperName.Text = "";
            txtSunday.Text = "";
            txtMonday.Text = "";
            txtTuesday.Text = "";
            txtWednesday.Text = "";
            txtThirsday.Text = "";
            txtFriday.Text = "";
            txtSaturday.Text = "";

            dtCurrent = null;
            dtStart.Value = DateTime.Now;
            btnDelete.Visible = false;
            btnSaveUpdate.Text = "Save";
            grdNewspapers.ReadOnly = true;
            using (var context = new BillMgmtEntities())
            {
                List<NewsPaper> ns = context.NewsPapers.ToList();
                List<NewsPaperModel> nsm = new List<NewsPaperModel>();
                foreach (NewsPaper n in ns)
                {
                    NewsPaperModel nsm1 = new NewsPaperModel();
                    nsm1.Id = n.Id;
                    nsm1.PaperName = n.PaperName;
                    nsm1.SundayCost = 0;
                    nsm1.MondayCost = 0;
                    nsm1.TuesdayCost = 0;
                    nsm1.WednesdayCost = 0;
                    nsm1.ThursdayCost = 0;
                    nsm1.FridayCost = 0;
                    nsm1.SaturdayCost = 0;

                    NewsPaperRate npr = n.NewsPaperRates.OrderByDescending(c => c.Startdate).FirstOrDefault();
                    if (npr != null)
                    {
                        nsm1.SundayCost = npr.Sunday == null ? 0 : npr.Sunday.Value;
                        nsm1.MondayCost = npr.Monday == null ? 0 : npr.Monday.Value;
                        nsm1.TuesdayCost = npr.Tuesday == null ? 0 : npr.Tuesday.Value;
                        nsm1.WednesdayCost = npr.Wednesday == null ? 0 : npr.Wednesday.Value;
                        nsm1.ThursdayCost = npr.Thursday == null ? 0 : npr.Thursday.Value;
                        nsm1.FridayCost = npr.Friday == null ? 0 : npr.Friday.Value;
                        nsm1.SaturdayCost = npr.Saturday == null ? 0 : npr.Saturday.Value;

                        nsm1.Applydate = npr.Startdate == null ? DateTime.Now : npr.Startdate.Value;
                        nsm1.NewsPaperCostId = npr.Id;
                    }
                    nsm.Add(nsm1);
                }
                grdNewspapers.DataSource = nsm;
                grdNewspapers.Columns["Id"].Visible = false;
                grdNewspapers.Columns["NewsPaperCostId"].Visible = false;
            }
        }

        private void grdNewspapers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            NewspaperId = (int)grdNewspapers.Rows[e.RowIndex].Cells["Id"].Value;
            txtPaperName.Text = grdNewspapers.Rows[e.RowIndex].Cells["PaperName"].Value.ToString();
            txtSunday.Text = grdNewspapers.Rows[e.RowIndex].Cells["SundayCost"].Value.ToString();
            txtMonday.Text = grdNewspapers.Rows[e.RowIndex].Cells["MondayCost"].Value.ToString();
            txtTuesday.Text = grdNewspapers.Rows[e.RowIndex].Cells["TuesdayCost"].Value.ToString();
            txtWednesday.Text = grdNewspapers.Rows[e.RowIndex].Cells["WednesdayCost"].Value.ToString();
            txtThirsday.Text = grdNewspapers.Rows[e.RowIndex].Cells["ThursdayCost"].Value.ToString();
            txtFriday.Text = grdNewspapers.Rows[e.RowIndex].Cells["FridayCost"].Value.ToString();
            txtSaturday.Text = grdNewspapers.Rows[e.RowIndex].Cells["SaturdayCost"].Value.ToString();

            dtStart.Value = (DateTime)grdNewspapers.Rows[e.RowIndex].Cells["Applydate"].Value;
            dtCurrent = (DateTime)grdNewspapers.Rows[e.RowIndex].Cells["Applydate"].Value;
            NewsPaperCostId = (int)grdNewspapers.Rows[e.RowIndex].Cells["NewsPaperCostId"].Value;
            btnDelete.Visible = true;
            btnSaveUpdate.Text = "Update";
        }


        private void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateNewspaperData()) return;
                NewsPaper newspaperToAdd = null;
                using (var context = new BillMgmtEntities())
                {
                    if (NewspaperId != 0)
                    {
                        newspaperToAdd = (from c in context.NewsPapers
                                          where c.Id == NewspaperId
                                          select c).FirstOrDefault();
                    }
                    else
                    {
                        newspaperToAdd = new NewsPaper();
                        newspaperToAdd.CreatedBy = bm.LoginId;
                        newspaperToAdd.CreatedDate = DateTime.Now;
                    }
                    newspaperToAdd.PaperName = txtPaperName.Text;
                    newspaperToAdd.IsActive = true;
                    newspaperToAdd.UpdatedBy = bm.LoginId;
                    newspaperToAdd.UpdatedDate = DateTime.Now;
                    if (NewspaperId == 0)
                    {
                        context.NewsPapers.Add(newspaperToAdd);
                    }
                    context.SaveChanges();

                    decimal cost = 0;

                    if (dtStart.Value != dtCurrent)
                    {
                        if (NewsPaperCostId == 0)
                        {
                            NewsPaperRate npr = new NewsPaperRate();
                            npr.NewsPaperId = newspaperToAdd.Id;
                            npr.IsActive = true;
                            decimal.TryParse(txtSunday.Text, out cost);
                            npr.Sunday = cost;
                            cost = 0;
                            decimal.TryParse(txtMonday.Text, out cost);
                            npr.Monday = cost;
                            cost = 0;
                            decimal.TryParse(txtTuesday.Text, out cost);
                            npr.Tuesday = cost;
                            cost = 0;
                            decimal.TryParse(txtWednesday.Text, out cost);
                            npr.Wednesday = cost;
                            cost = 0;
                            decimal.TryParse(txtThirsday.Text, out cost);
                            npr.Thursday = cost;
                            cost = 0;
                            decimal.TryParse(txtFriday.Text, out cost);
                            npr.Friday = cost;
                            cost = 0;
                            decimal.TryParse(txtSaturday.Text, out cost);
                            npr.Saturday = cost;

                            npr.Startdate = dtStart.Value;
                            npr.CreatedBy = bm.LoginId;
                            context.NewsPaperRates.Add(npr);
                        }
                        else
                        {
                            var confirm = MessageBox.Show("Are you sure you want to change the cost of paper??", "Confirm Change!!", MessageBoxButtons.YesNo);
                            if (confirm == DialogResult.Yes)
                            {
                                NewsPaperRate npr = null;
                                if (dtCurrent.Value.Date >= dtStart.Value.Date)
                                {
                                    npr = context.NewsPaperRates.Where(c => c.Id == NewsPaperCostId).FirstOrDefault();
                                    decimal.TryParse(txtSunday.Text, out cost);
                                    npr.Sunday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtMonday.Text, out cost);
                                    npr.Monday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtTuesday.Text, out cost);
                                    npr.Tuesday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtWednesday.Text, out cost);
                                    npr.Wednesday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtThirsday.Text, out cost);
                                    npr.Thursday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtFriday.Text, out cost);
                                    npr.Friday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtSaturday.Text, out cost);
                                    npr.Saturday = cost;
                                    npr.Startdate = dtStart.Value;

                                }
                                else
                                {
                                    npr = new NewsPaperRate();
                                    npr.NewsPaperId = newspaperToAdd.Id;
                                    npr.IsActive = true;
                                    decimal.TryParse(txtSunday.Text, out cost);
                                    npr.Sunday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtMonday.Text, out cost);
                                    npr.Monday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtTuesday.Text, out cost);
                                    npr.Tuesday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtWednesday.Text, out cost);
                                    npr.Wednesday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtThirsday.Text, out cost);
                                    npr.Thursday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtFriday.Text, out cost);
                                    npr.Friday = cost;
                                    cost = 0;
                                    decimal.TryParse(txtSaturday.Text, out cost);
                                    npr.Saturday = cost;
                                    npr.Startdate = dtStart.Value;
                                    npr.CreatedBy = bm.LoginId;
                                    context.NewsPaperRates.Add(npr);

                                }
                            }

                        }
                        context.SaveChanges();
                    }


                    bindNewsPaperGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateNewspaperData()
        {
            if (txtPaperName.Text.Trim() == "")
            {
                MessageBox.Show("News paper Name is required.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPaperName.Focus();
                return false;
            }

            if (txtSunday.Text.Trim() == "" || txtMonday.Text.Trim() == "" || txtTuesday.Text.Trim() == "" || txtWednesday.Text.Trim() == "" || txtThirsday.Text.Trim() == "" || txtFriday.Text.Trim() == "" || txtSaturday.Text.Trim() == "")
            {
                MessageBox.Show("Please enter all cost values of week days", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            decimal cost = 0;
            if (!decimal.TryParse(txtSunday.Text, out cost) || !decimal.TryParse(txtMonday.Text, out cost) || !decimal.TryParse(txtTuesday.Text, out cost) || !decimal.TryParse(txtWednesday.Text, out cost) || !decimal.TryParse(txtThirsday.Text, out cost) || !decimal.TryParse(txtFriday.Text, out cost) || !decimal.TryParse(txtSaturday.Text, out cost))
            {
                MessageBox.Show("Cost must be Numeric value.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //if (dtStart.Value < dtCurrent && NewsPaperCostId > 0)
            //{
            //    MessageBox.Show("Please select the date grater then the last date when the new rate applied on this news paper.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    dtStart.Focus();
            //    return false;
            //}


            return true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (NewspaperId == 0) return;
            var confirm = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!!", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (var context = new BillMgmtEntities())
                    {
                        var newsPaperToDelete = (from b in context.NewsPapers
                                                 where b.Id == NewspaperId
                                                 select b).FirstOrDefault();

                        while (newsPaperToDelete.NewsPaperRates.Count() > 0)
                        {
                            var newsp = newsPaperToDelete.NewsPaperRates.FirstOrDefault();
                            context.NewsPaperRates.Remove(newsp);
                        }

                        context.NewsPapers.Remove(newsPaperToDelete);
                        context.SaveChanges();
                        bindNewsPaperGrid();
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
