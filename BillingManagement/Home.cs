using BillingManagement.Entity;
using BillingManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BillingManagement
{
    public partial class Home : Form
    {
        List<InvoiceDetailModel> IDM = new List<InvoiceDetailModel>();
        List<CheckBox> groupOfMonthsCheckBoxes = new List<CheckBox>();
        int customerid1 = 0;
        DateTime startdate1 = DateTime.Now;
        DateTime enddate1 = DateTime.Now;
        int InvoiceType1 = 0;
        int invoiceId = 0;

        public Home()
        {
            InitializeComponent();
        }

        private void Home_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            this.MdiParent.WindowState = FormWindowState.Maximized;
            tblInvoice.Dock = DockStyle.None;
            tblInvoice.Width = 720;
            Bitmap bmp = new Bitmap(tblInvoice.ClientRectangle.Width, tblInvoice.ClientRectangle.Height);
            tblInvoice.DrawToBitmap(bmp, tblInvoice.ClientRectangle);
            e.Graphics.DrawImage(bmp, 30, 20);

            e.Graphics.DrawImage(bmp, 30, bmp.Height + 100);
            tblInvoice.Dock = DockStyle.Fill;
            this.MdiParent.WindowState = FormWindowState.Normal;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if(tblInvoice.Visible == false)
            {
                MessageBox.Show("Please select customer or Click on Generate Invoice", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (invoiceId == 0 && tblInvoice.Visible == true)
            {
                using (var context = new BillMgmtEntities())
                {
                    Invoice INV = new Invoice();
                    INV.CustomerId = customerid1;
                    INV.FromDate = startdate1;
                    INV.ToDate = enddate1;
                    INV.IsPaid = true;
                    INV.Amount = IDM.Sum(c => c.Cost);
                    INV.InvoiceType = InvoiceType1;
                    context.Invoices.Add(INV);
                    context.SaveChanges();

                    lblInvoiceNumber.Text = "INV-" + INV.Id;
                    invoiceId = INV.Id;
                    foreach (var INVD in IDM)
                    {
                        InvoiceDetail ind = new InvoiceDetail();
                        ind.InvoiceId = INV.Id;
                        ind.NewsPaperId = INVD.NewsPaperId;
                        ind.NumberOfCopies = INVD.NumberOfCopies;
                        ind.FromDate = INVD.FromDate;
                        ind.ToDate = INVD.EndDate;
                        ind.Cost = INVD.Cost;
                        context.InvoiceDetails.Add(ind);
                    }
                    context.SaveChanges();
                }
            }

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            PrintDocument Pd = new PrintDocument();
            Pd.PrintPage += Home_PrintPage;
            ppd.Document = Pd;
            ppd.ShowDialog();
            cbCustomer.SelectedIndex = 0;
        }

        private void Home_Load(object sender, EventArgs e)
        {
            groupOfMonthsCheckBoxes.Add(chkJanuary);
            groupOfMonthsCheckBoxes.Add(chkFebruary);
            groupOfMonthsCheckBoxes.Add(chkMarch);
            groupOfMonthsCheckBoxes.Add(chkApril);
            groupOfMonthsCheckBoxes.Add(chkMay);
            groupOfMonthsCheckBoxes.Add(chkJun);
            groupOfMonthsCheckBoxes.Add(chkJuly);
            groupOfMonthsCheckBoxes.Add(chkAugust);
            groupOfMonthsCheckBoxes.Add(chkSeptember);
            groupOfMonthsCheckBoxes.Add(chkOctober);
            groupOfMonthsCheckBoxes.Add(chkNovember);
            groupOfMonthsCheckBoxes.Add(chkDecember);

            cbYears.Items.Clear();
            for(int year= 2005; year <= DateTime.Now.Year; year++ )
            {
                cbYears.Items.Add(year.ToString());
            }
            foreach (var item in cbYears.Items)
            {
                if (Convert.ToInt32(item.ToString()) == DateTime.Now.Year)
                    cbYears.SelectedIndex = cbYears.Items.IndexOf(item);
            }

            dtFrom.Value = DateTime.Now.AddDays(-30);
            dtTo.Value = DateTime.Now;
            using (var context = new BillMgmtEntities())
            {
                List<CustomerDropdown> cs = context.Customers.Select(c => new CustomerDropdown() { CustomerId = c.Id, CustomerName = c.FirstName + " " + c.LastName + "(" + c.AdharNumber + ")" }).ToList();
                CustomerDropdown csnew = new CustomerDropdown();
                csnew.CustomerId = 0;
                csnew.CustomerName = "Select Customer";
                cs.Insert(0, csnew);
                cbCustomer.DataSource = cs;
                cbCustomer.DisplayMember = "CustomerName";
            }
        }

        private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPreview.Visible = true;
            tblInvoice.Visible = false;
            if (!tabMC.Controls.Contains(tbMonthly))
                tabMC.Controls.Add(tbMonthly);
            CustomerDropdown customerdd = cbCustomer.SelectedItem as CustomerDropdown;
            using (var context = new BillMgmtEntities())
            {
                Customer c = context.Customers.Where(d => d.Id == customerdd.CustomerId).FirstOrDefault();

                if (c != null)
                {
                    Invoice In = c.Invoices.Where(d => d.IsPaid == true).OrderByDescending(d => d.ToDate).FirstOrDefault();
                    if (In != null)
                    {
                        tabMC.Controls.Remove(tbMonthly);
                        tblPreviousInvoiceDate.Visible = true;
                        lblPreviousInvoiceNo.Visible = true;
                        lblPreviousInvoiceNo.Text = "Previous Invoice #: " + In.Id.ToString();
                        lblPaiddate.Text = "Last paid upto: " + In.ToDate.Value.ToShortDateString();
                        lblLatpaidAmmount.Text = "Last Amount: " + In.Amount.ToString();
                        tabMC.SelectedTab = tbCustom;
                        dtFrom.Value = In.ToDate.Value.AddDays(1);
                        dtTo.Value = DateTime.Now;
                        if (In.ToDate.Value.ToShortDateString() != DateTime.Now.ToShortDateString())
                            btnGeneratereport.PerformClick();
                        else
                        {
                            MessageBox.Show("Last Invoice (" + In.Id + ") already generated till today of amount " + In.Amount.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        tblPreviousInvoiceDate.Visible = false;
                        lblPreviousInvoiceNo.Visible = false;
                    }


                    lblCustomerName.Text = c.FirstName + " " + c.LastName;
                    lblAdhar.Text = c.AdharNumber;
                    lblPhone.Text = c.Phone;
                }
                else
                {
                    lblCustomerName.Text = "Please select Customer!";
                    lblAdhar.Text = "";
                    lblPhone.Text = "";
                    tblPreviousInvoiceDate.Visible = false;
                    lblPreviousInvoiceNo.Visible = false;
                }
            }
        }

        private bool ValidateData()
        {
            if (cbCustomer.SelectedIndex == 0)
            {
                MessageBox.Show("Please select Customer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbCustomer.Focus();
                return false;
            }

            if (tabMC.SelectedTab == tbMonthly)
            {
                CheckBox cb = groupOfMonthsCheckBoxes.Where(c => c.Checked == true).FirstOrDefault();
                if (cb == null)
                {
                    MessageBox.Show("Please select Month.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            else
            {
                if (dtFrom.Value >= dtTo.Value)
                {
                    MessageBox.Show("Date from must be less then date to.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }

        private void generateInvoice(DateTime startdate, DateTime enddate, int customerid, int InvoiceType)
        {
            try
            {
                customerid1 = customerid;
                startdate1 = startdate;
                enddate1 = enddate;
                InvoiceType1 = InvoiceType;

                using (var context = new BillMgmtEntities())
                {
                    List<NewsPaperAllocation> npa = context.NewsPaperAllocations.Where(c => ((DbFunctions.TruncateTime(c.StartDate) >= startdate.Date && DbFunctions.TruncateTime(c.StartDate) <= enddate.Date) || (DbFunctions.TruncateTime(c.EndDate) >= startdate && DbFunctions.TruncateTime(c.EndDate) <= enddate.Date) || (DbFunctions.TruncateTime(c.StartDate) < startdate.Date && c.EndDate == null)) && c.CustomerId == customerid).OrderBy(d => d.NewspaperId).ToList();

                    IDM = new List<InvoiceDetailModel>();
                    foreach (var item in npa)
                    {
                        InvoiceDetailModel invd = new InvoiceDetailModel();
                        invd.NewsPaperName = item.NewsPaper.PaperName;
                        invd.NewsPaperId = item.NewspaperId.Value;
                        DateTime dateStart = startdate > item.StartDate.Value ? startdate : item.StartDate.Value;
                        DateTime dateEnd = enddate;
                        if (item.EndDate != null)
                        {
                            dateEnd = enddate > item.EndDate.Value ? item.EndDate.Value : enddate;
                        }
                        invd.FromDate = dateStart;
                        invd.EndDate = dateEnd;

                        int NumberOfCopies = 0;
                        decimal cost = 0;
                        while (dateStart <= dateEnd)
                        {
                            NoSupplyDate nsd = context.NoSupplyDates.Where(c => DbFunctions.TruncateTime(c.NoSupplyDate1) == dateStart.Date).FirstOrDefault();
                            if (nsd == null)
                            {
                                NewsPaperRate npr = context.NewsPaperRates.Where(c => (DbFunctions.TruncateTime(c.Startdate) <= dateStart.Date) && c.NewsPaperId == item.NewspaperId).OrderByDescending(d => d.Startdate).FirstOrDefault();
                                NumberOfCopies = NumberOfCopies + (item.NumberOfCopies == null ? 1 : item.NumberOfCopies.Value);
                                if (npr != null)
                                {
                                    if (dateStart.DayOfWeek == DayOfWeek.Sunday) cost = cost + npr.Sunday.Value;
                                    if (dateStart.DayOfWeek == DayOfWeek.Monday) cost = cost + npr.Monday.Value;
                                    if (dateStart.DayOfWeek == DayOfWeek.Tuesday) cost = cost + npr.Tuesday.Value;
                                    if (dateStart.DayOfWeek == DayOfWeek.Wednesday) cost = cost + npr.Wednesday.Value;
                                    if (dateStart.DayOfWeek == DayOfWeek.Thursday) cost = cost + npr.Thursday.Value;
                                    if (dateStart.DayOfWeek == DayOfWeek.Friday) cost = cost + npr.Friday.Value;
                                    if (dateStart.DayOfWeek == DayOfWeek.Saturday) cost = cost + npr.Saturday.Value;
                                }
                            }
                            dateStart = dateStart.AddDays(1);
                        }
                        invd.NumberOfCopies = NumberOfCopies;
                        invd.Cost = cost;
                        IDM.Add(invd);
                    }

                    grdInvoiceItems.DataSource = IDM;
                    grdInvoiceItems.Columns["NewsPaperId"].Visible = false;
                    lblTotal.Text = IDM.Sum(c => c.Cost).ToString("N2");
                    grdInvoiceItems.Enabled = false;
                    invoiceId = 0;
                    lblInvoiceNumber.Text = "Dummy";
                    lblPreview.Visible = false;
                    tblInvoice.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGeneratereport_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;
            CustomerDropdown customerdd = cbCustomer.SelectedItem as CustomerDropdown;
            generateInvoice(dtFrom.Value, dtTo.Value, customerdd.CustomerId, 2);

        }

        private void btnMonthly_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;
            CustomerDropdown customerdd = cbCustomer.SelectedItem as CustomerDropdown;
            CheckBox cb = groupOfMonthsCheckBoxes.Where(c => c.Checked == true).FirstOrDefault();

            DateTime startDate = DateTime.Now;
            int Year = Convert.ToInt32(cbYears.SelectedItem);
            if (cb.Text == "January") startDate = new DateTime(Year, 1, 1);
            if (cb.Text == "February") startDate = new DateTime(Year, 2, 1);
            if (cb.Text == "March") startDate = new DateTime(Year, 3, 1);
            if (cb.Text == "April") startDate = new DateTime(Year, 4, 1);
            if (cb.Text == "May") startDate = new DateTime(Year, 5, 1);
            if (cb.Text == "June") startDate = new DateTime(Year, 6, 1);
            if (cb.Text == "July") startDate = new DateTime(Year, 7, 1);
            if (cb.Text == "August") startDate = new DateTime(Year, 8, 1);
            if (cb.Text == "September") startDate = new DateTime(Year, 9, 1);
            if (cb.Text == "October") startDate = new DateTime(Year, 10, 1);
            if (cb.Text == "November") startDate = new DateTime(Year, 11, 1);
            if (cb.Text == "December") startDate = new DateTime(Year, 12, 1);

            generateInvoice(startDate, startDate.AddMonths(1).AddDays(-1), customerdd.CustomerId, 1); //1 for Monthly Invoice 

        }
        private void checkbox_Click(object sender, EventArgs e)
        {
            foreach (CheckBox cb in groupOfMonthsCheckBoxes)
            {
                if (cb == sender)
                    cb.Checked = true;
                else
                    cb.Checked = false;
            }
        }

        private void cbYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 0;
            foreach (CheckBox cb in groupOfMonthsCheckBoxes)
            {
                cb.Checked = false;
                cb.Enabled = true;
                if (Convert.ToInt32(cbYears.SelectedItem.ToString()) == DateTime.Now.Year)
                {
                    int monthnumber = DateTime.Now.Month;
                    if ((index + 1) >= monthnumber)
                    {
                        cb.Enabled = false;
                    }
                }
                if (Convert.ToInt32(cbYears.SelectedItem.ToString()) > DateTime.Now.Year)
                {
                    cb.Enabled = false;
                }
                index++;
            }
        }

        private void grdInvoiceItems_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            grdInvoiceItems.ClearSelection();
        }
    }
}
