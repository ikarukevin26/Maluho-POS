using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace POS
{
    public partial class frmItemSearch : Form
    {
        public frmItemSearch()
        {
            InitializeComponent();
        }
        private DataTable dt;
        public void datagrid()
        {
            try
            {
                string connection = "datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root";
                string query = "Select Barcode,Brand,ItemName,Apparel,ItemCostYen,Quantity,Color,Size,Itemrank,Material,Hardware,Stamp from Inventory where Quantity='1'";
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                dt= new DataTable();
                da.Fill(dt);      
                dataGridView1.DataSource = dt;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["ItemCostYen"].Value != null)
                    {
                        if (decimal.TryParse(row.Cells["ItemCostYen"].Value.ToString(), out decimal costYen))
                        {
                            row.Cells["ItemCostYen"].Value = costYen.ToString("N0");
                        }
                    }
                }
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}. Please contact your administrator.","ALERT",MessageBoxButtons.OKCancel);
            }
        }
        public void itemnamesearch()
        {
            try
            {
                string connection = "datasource=192.168.1.34; database=therealmaluho;port=3306;userid=dba;password=Welcome@12345";
                string query = "Select Barcode,Brand,ItemName,Apparel,ItemCostYen,Quantity,Color,Size,Itemrank,Material,Hardware,Stamp from Inventory where ItemName like '" + txtItemName.Text + "%' and Quantity='1'";
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["ItemCostYen"].Value != null)
                    {
                        if (decimal.TryParse(row.Cells["ItemCostYen"].Value.ToString(), out decimal costYen))
                        {
                            row.Cells["ItemCostYen"].Value = costYen.ToString("N0");
                        }
                    }
                }
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.","ALERT",MessageBoxButtons.OKCancel);
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (txtBrand.Text!="")
            itemnamesearch();
        }

        private void frmItemSearch_Load(object sender, EventArgs e)
        {
            datagrid();
        }

        private void receiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Receipt rc=new Receipt();
            rc.ShowDialog();
        }
        private void officialInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmInvoice officialInvoice = new frmInvoice();
            officialInvoice.ShowDialog();
        }
        public void barcode()
        {
            if (txtBarcode.Text != "")
            {
                try
                {
                    string connection = "datasource=192.168.1.34; database=therealmaluho;port=3306;userid=dba;password=Welcome@12345";
                    string query = "Select Barcode,Brand,ItemName,Apparel,ItemCostYen,Quantity,Color,Size,Itemrank,Material,Hardware,Stamp from Inventory where Barcode like '" + txtBarcode.Text + "%' and Quantity='1'";
                    MySqlConnection conn = new MySqlConnection(connection);
                    MySqlCommand cmd= new MySqlCommand(query, conn);
                    MySqlDataAdapter da=new MySqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt= new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    foreach (DataGridViewRow row in dataGridView1.Rows )
                    {
                        if(row.Cells["ItemCostYen"].Value != null)
                        {
                            if (decimal.TryParse(row.Cells["ItemCostYen"].Value.ToString(), out decimal costYen))
                            {
                                row.Cells["ItemCostYen"].Value= costYen.ToString("N0");
                            }
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.", "ALERT", MessageBoxButtons.OKCancel);
                }
            }
        }
        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            barcode();
        }
        public void brandsearch()
        {
            if (txtBrand.Text != "")
            {
                try
                {
                    string connection = "datasource=192.168.1.34; database=therealmaluho;port=3306;userid=dba;password=Welcome@12345";
                    string query = "Select Barcode,Brand,ItemName,Apparel,ItemCostYen,Quantity,Color,Size,Itemrank,Material,Hardware,Stamp from Inventory where Brand like '"+txtBrand.Text+"%' and Quantity='1' ";
                    MySqlConnection conn= new MySqlConnection(connection);
                    MySqlCommand cmd= new MySqlCommand(query, conn);
                    MySqlDataAdapter da=new MySqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["ItemCostYen"].Value != null)
                        {
                            if (decimal.TryParse(row.Cells["ItemCostYen"].Value.ToString(), out decimal costYen))
                            {
                                row.Cells["ItemCostYen"].Value = costYen.ToString("N0");
                            }
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}. Please contact your administrator.", "ALERT", MessageBoxButtons.OKCancel);
                }
            }
        }
        private void txtBrand_TextChanged(object sender, EventArgs e)
        {
            brandsearch();
        }

        private void salesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSalesReport fs=new frmSalesReport();
            fs.ShowDialog();
        }
    }
}
