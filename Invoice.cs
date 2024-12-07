using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace POS
{
    public partial class frmInvoice : Form
    {
        public frmInvoice()
        {
            InitializeComponent();
            btnAdd.Click += new EventHandler(this.btnAdd_Click);
            InitializeDataGridView();
            lblDate.Text = DateTime.Now.ToString("yyyy-dd-MM");
            btnRemove.Click += new EventHandler(this.btnRemove_Click);
        }

        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Add("Brand", "摘要");
            dataGridView1.Columns.Add("ItemName", "摘要");
            dataGridView1.Columns.Add("Barcode", "品番");
            dataGridView1.Columns.Add("Quantity", "数量");
            dataGridView1.Columns.Add("Cost", "単価");
            dataGridView1.Columns.Add("Tax", "税率");
            dataGridView1.Columns.Add("Amount", "金額");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void clearfields()
        {
            txtBarcode.Clear();
            txtApparel.Clear();
            txtBrand.Clear();
            txtItemName.Clear();
            txtSize.Clear();
            txtColor.Clear();
            txtCost.Clear();
            txtFinalCost.Clear();
            txtRank.Clear();
            txtMaterial.Clear();
            txtHardware.Clear();
            txtStamp.Clear();
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtApparel.Text != "")
            {
                clearfields();
            }
            else if (txtApparel.Lines.Length == 2)
            {

                MessageBox.Show("Item is out of stock or Item is not yet in the inventory", "WARNING", MessageBoxButtons.OKCancel);

            }
            else
            {
                try
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        string barcodeValue = txtBarcode.Text;
                        MySqlConnection con = new MySqlConnection("datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root");
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("select * from inventory where Barcode='" + barcodeValue + "'", con);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                string columnNameValue = reader.GetString("Apparel");
                                string columnNameValue1 = reader.GetString("Brand");
                                string columnNameValue2 = reader.GetString("ItemName");
                                string columnNameValue3 = reader.GetString("Size");
                                string columnNameValue4 = reader.GetString("Itemcost");
                                string columnNameValue8 = reader.GetString("Itemrank");
                                string columnNameValue9 = reader.GetString("Material");
                                string columnNameValue10 = reader.GetString("Hardware");
                                string columnNameValue11 = reader.GetString("Stamp");
                                string columnNameValue12 = reader.GetString("ItemcostYen");
                                string columnNameValue13 = reader.GetString("Color");
                                string columnNameValue14 = reader.GetString("Itemcost");
                                
                                //get all the value of the string
                                txtApparel.Text = columnNameValue;
                                txtBrand.Text = columnNameValue1;
                                txtItemName.Text = columnNameValue2;
                                txtSize.Text = columnNameValue3;
                                txtRank.Text = columnNameValue8;
                                txtMaterial.Text = columnNameValue9;
                                txtHardware.Text = columnNameValue10;
                                txtStamp.Text = columnNameValue11;
                                txtCost.Text = columnNameValue12;
                                txtColor.Text = columnNameValue13;
                          
                                string input1 = txtCost.Text;
                                long num1;
                                if (long.TryParse(input1, out num1))
                                {

                                    string formattedNum1 = num1.ToString("n0");
                                    txtCost.Text = formattedNum1;
                                }
                            }
                        }
                        else
                        {
                            txtApparel.Text = "this item is not yet recorded in the inventory";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.");
                }
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string quantity = "1";
            string tax = "10%";
            if (txtApparel.Text != "")
            {
                if (txtColor.Text == "")
                {
                    MessageBox.Show("You can't add an item.");
                }
                else
                {
                    // Adding the selected item details to the DataGridView
                    dataGridView1.Rows.Add(
                        txtBrand.Text,
                        txtItemName.Text,
                        txtBarcode.Text,
                        quantity,
                        txtCost.Text,
                        tax,
                        txtCost.Text
                    );
                }
            }
            else
            {

            }
            CalculateTotalCost();
            CalculateTax();
            CalculateFinalTotal();
            clearfields();
        }
        private void CalculateTotalCost()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Cost"].Value != null)
                {
                    if (decimal.TryParse(row.Cells["Cost"].Value.ToString(), out decimal cost))
                    {
                        total += cost;
                    }
                }
            }
            txtTotal.Text = total.ToString("n0");
            lblTotal.Text = total.ToString("n0");
        }
        public void CalculateTax()
        {
            decimal tax = 0;
            decimal total;
            if (decimal.TryParse(txtTotal.Text, out total))
            {
                tax = total * .1m;

            }
            txtTax.Text = tax.ToString("n0");
            lblTax.Text = tax.ToString("n0");
        }
        public void CalculateFinalTotal()
        {
            decimal finalTotal = 0;
            decimal total;
            decimal totalTax;
            if (decimal.TryParse(txtTotal.Text, out total) && decimal.TryParse(txtTax.Text, out totalTax))
            {
                finalTotal = total + totalTax;

            }
            txtFinalTotal.Text = finalTotal.ToString("n0");
            txtFinalTotal2.Text = finalTotal.ToString("n0") + "  円  (税込)";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void btnBuy_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.Landscape = false;
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1170); // Set paper size to A4 (in hundredths of an inch)
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); // Set margins to 0
            pd.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Create a bitmap of the GroupBox content
            Bitmap bm = new Bitmap(panel1.Width, panel1.Height);
            panel1.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, panel1.Width, panel1.Height));

            // Calculate the scaling factor to fit the GroupBox content to the A4 page
            float scaleFactorX = (float)e.PageBounds.Width / bm.Width;
            float scaleFactorY = (float)e.PageBounds.Height / bm.Height;
            float scaleFactor = Math.Min(scaleFactorX, scaleFactorY);

            // Calculate the scaled width and height
            int scaledWidth = (int)(bm.Width * scaleFactor);
            int scaledHeight = (int)(bm.Height * scaleFactor);

            // Draw the scaled image centered on the page
            int offsetX = (e.PageBounds.Width - scaledWidth) / 2;
            int offsetY = (e.PageBounds.Height - scaledHeight) / 2;
            e.Graphics.DrawImage(bm, new System.Drawing.Rectangle(offsetX, offsetY, scaledWidth, scaledHeight));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string barcodeToRemove = txtBarcode.Text.Trim(); // Get the barcode to remove from the textbox

            bool found = false;

            // Iterate through DataGridView rows to find and remove the row with matching barcode
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Barcode"].Value != null && row.Cells["Barcode"].Value.ToString().Trim().Equals(barcodeToRemove, StringComparison.OrdinalIgnoreCase))
                {
                    dataGridView1.Rows.Remove(row); // Remove the row
                    found = true;
                    break; // Exit the loop after removing the first matching row
                }
            }

            if (found)
            {
                CalculateTotalCost(); // Recalculate total cost after removal
                CalculateTax(); // Recalculate tax after removal
                CalculateFinalTotal(); // Recalculate final total after removal
            }
            else
            {
                CalculateTotalCost(); // Recalculate total cost after removal
                CalculateTax(); // Recalculate tax after removal
                CalculateFinalTotal(); // Recalculate final total after removal
                MessageBox.Show("Item removed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void officialInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Receipt rc=new Receipt();
            rc.ShowDialog();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin fl=new frmLogin();
            fl.ShowDialog();
        }

        private void itemSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmItemSearch fl=new frmItemSearch();
            fl.ShowDialog();
        }

        private void txtApparel_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBrand_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtItemName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSize_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCost_TextChanged(object sender, EventArgs e)
        {

        }

        private void salesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSalesReport fl=new frmSalesReport();
            fl.ShowDialog();
        }
    }
}
