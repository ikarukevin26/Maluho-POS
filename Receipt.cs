﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace POS
{
    public partial class Receipt : Form
    {
        public Receipt()
        {
            InitializeComponent();
            lblDate.Text = DateTime.Now.ToString("yyyy-dd-MM hh:mm");
            InitializeDataGridView();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void InitializeDataGridView()
        {
        }

        public Label Label1
        {
            get { return lblPin; }
            set { lblPin = value; }
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
            txtFinalCostTax.Clear();
            txtQuantity.Clear();
            txtID.Clear();
            txtCardfee.Clear();
            pictureBox3.Visible = false;
        }
        private void label17_Click(object sender, EventArgs e)
        {
        }
        public void employee()
        {
            try
            {
                MySqlConnection con = new MySqlConnection("datasource=localhost;database=maluhotimesheet;port=3306;userid=root;password=root");
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Select FirstName from user where Barcode='" + lblPin.Text + "'", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    lblEmployeeName.Text = reader.GetString("Firstname");
                }
                else
                {

                }
                con.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.", "ALERT", MessageBoxButtons.OKCancel);
            }
        }
        private void Receipt_Load(object sender, EventArgs e)
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.HeaderText = "Select";
            chk.Name = "chk";
            dataGridView1.Columns.Add(chk);
            dataGridView1.Columns.Add("Brand", "Brand");
            dataGridView1.Columns.Add("ItemName", "ItemName");
            dataGridView1.Columns.Add("Barcode", "Barcode");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("Cost", "Cost");
            dataGridView1.Columns.Add("Tax", "Tax");
            dataGridView1.Columns.Add("Price", "Price");
            dataGridView1.Columns.Add("Inventoryid", "Inventoryid");

            employee();

           
        }
        public void updateitem()
        {
            int quantity = 1;
            int txtquantity = int.Parse(txtQuantity.Text);
            int result = txtquantity - quantity;

            try
            {
                using (MySqlConnection con = new MySqlConnection("datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root"))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE inventory SET Quantity = @Quantity WHERE Inventoryid = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@Quantity", result);
                        cmd.Parameters.AddWithValue("@id", txtID.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {                        
                        }
                        else
                        {
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.", "ALERT", MessageBoxButtons.OKCancel);
            }
        }
      public void addquantity()
        {
            int quantity = 1;
            int txtquantity=int.Parse(txtQuantity.Text);
            int result = txtquantity + quantity;
            try
            {
                using (MySqlConnection con = new MySqlConnection("datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root"))
                {
                    con.Open();
                    using(MySqlCommand cmd=new MySqlCommand("UPDATE inventory SET Quantity = @Quantity WHERE Inventoryid = @id", con))
                    {
                        cmd.Parameters.AddWithValue("Quantity",result);
                        cmd.Parameters.AddWithValue("@id",txtID.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {

                        }
                        else
                        {

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.","ALERT", MessageBoxButtons.OKCancel);
            }
        }
        public void buyitem()
        {
            string priceStr = txtCost.Text.Replace(",", "");
            string input = txtFinalCost.Text.Replace(",", "");
            int cost = int.Parse(priceStr);
            int finalcost = int.Parse(input);
            string status = "paid";
            string customer = "store";
            try
            {
                MySqlConnection con = new MySqlConnection("datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root");
                con.Open();
                MySqlCommand cmd = new MySqlCommand("insert into storecustomer(Barcode,Apparel,Brand,ItemName,Cost,Price,Status,Customer) values ('" + txtBarcode.Text + "','" + txtApparel.Text + "','" + txtBrand.Text + "','" + txtItemName.Text + "','" + cost + "','" + finalcost + "','"+status+"','"+customer+"')", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MessageBox.Show($"Item added");
                    }
                }
                con.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.", "ALERT", MessageBoxButtons.OKCancel);
            }
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
                                string columnNameValue15 = reader.GetString("Quantity");
                                int columnNameValue16 = (int)reader["Inventoryid"];
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
                                txtQuantity.Text = columnNameValue15;
                                txtID.Text = columnNameValue16.ToString();
                                string input1 = txtCost.Text;
                                long num1;

                                if (txtQuantity.Text == "")
                                {
                                    
                                }
                                else
                                {
                                    string quantity=txtQuantity.Text;
                                    int quantity2 = Convert.ToInt32(quantity);
                                    if(quantity2<=0)
                                    {
                                      DialogResult result = MessageBox.Show($"This item is mined or out of inventory. Do you want to  add it again in  inventory", "ALERT", MessageBoxButtons.YesNo);
                                        if(result == DialogResult.Yes)
                                        {
                                            ;
                                            MySqlConnection con1 = new MySqlConnection("datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root");
                                            con1.Open();
                                            MySqlCommand cmd1 = new MySqlCommand("update inventory set Quantity ='" + 1 + "'  where Barcode='" + barcodeValue + "'", con1);
                                            MySqlDataReader reader1 = cmd1.ExecuteReader();
                                            if (reader1.HasRows)
                                            {
                                                while (reader1.Read())
                                                {

                                                }
                                            }
                                                con1.Close();
                                            reader1.Close();
                                        }
                                        clearfields();
                                    }

                                }
                              

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
                    MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.", "ALERT", MessageBoxButtons.OKCancel);
                }
            }
        }
        private decimal overallTotal = 0m;
        private decimal totaltax = 0m;
        private decimal finaltotal = 0m;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtFinalCost.Text == "")
            {
                MessageBox.Show("Please input final cost of the item.", "ALERT", MessageBoxButtons.OK);
                txtFinalCost.Focus();
            }
            else if (txtBarcode.Text == "")
            {
                MessageBox.Show("Please search for the item in barcode area","ALERT",MessageBoxButtons.OK);
                txtBarcode.Focus();
            }
            else if(txtApparel.Text=="")
            {
                MessageBox.Show("Please input Apparel", "ALERT", MessageBoxButtons.OK);
                txtApparel .Focus();
            }
            else if (txtItemName.Text=="")
            {
                MessageBox.Show("Please input the Item Name", "ALERT", MessageBoxButtons.OK);
                txtItemName.Focus();
            }
            else if (txtBrand.Text=="")
            {
                MessageBox.Show("Please input the Brand", "ALERT", MessageBoxButtons.OK);
                txtBrand .Focus();
            }
            else
            {
                updateitem();
                buyitem();
                

                string tax = "10%";
                if (txtApparel.Text != "")
                {
                    if (txtColor.Text == "")
                    {
                        MessageBox.Show("You can't add an item.");
                    }
                    else
                    {
                        dataGridView1.Rows.Add(
                             false,
                            txtBrand.Text,
                            txtItemName.Text,
                            txtBarcode.Text,
                            txtQuantity.Text,
                           txtFinalCostTax.Text,
                            tax,
                            txtFinalCost.Text,
                            txtID.Text,
                            Guid.NewGuid().ToString()
                        );
                        decimal cost;
                        if (decimal.TryParse(txtFinalCostTax.Text, out cost))
                        {
                            txtFinalCostTax.Text=cost.ToString("n0");
                            decimal itemTax = cost * 0.10m;
                            decimal total = cost + itemTax;
                            string itemDetails = $"{txtBarcode.Text}{Environment.NewLine}{txtBrand.Text} {Environment.NewLine}{txtItemName.Text,-22} ¥{txtFinalCostTax.Text,10}{Environment.NewLine}***************************************{Environment.NewLine}";
                            lblitems.Text += itemDetails;
                            overallTotal += cost;
                            totaltax = overallTotal * 0.10m;
                            finaltotal = overallTotal + totaltax;
                        }
                    }
                }              
                clearfields();
                lblSubtotal.Location = new Point(lblSubtotal.Location.X, lblitems.Location.Y + lblitems.Height + 10);
                lblSubtotal.Text = $"Subtotal: ¥ {overallTotal.ToString("n0")}{Environment.NewLine}Tax:        ¥ {totaltax.ToString("n0")}{Environment.NewLine}";
                lblTotal.Location = new Point(lblTotal.Location.X, lblSubtotal.Location.Y + lblSubtotal.Height + 5); 
                lblTotal.Text = $"Total:    ¥ {finaltotal.ToString("n0")}";
                lblTotal.Font = new Font(lblTotal.Font, FontStyle.Bold); 
            }
        }
        private string ShortenItemName(string itemName)
        {
            const int maxLength = 7;
            if (itemName.Length > maxLength)
            {
                return itemName.Substring(0, maxLength) + "..."; 
            }
            return itemName;
        }     
        private void btnBuy_Click(object sender, EventArgs e)
        {     if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Please add items before buying");
            }
            else
            {
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.Landscape = false;
                pd.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 315, 787);
                pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                pd.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = pd;
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    pd.Print();
                }
               
            }
            dataGridView1.Rows.Clear();
            UpdateReceiptDetails();

        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(panel1.Width, panel1.Height);
            panel1.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, panel1.Width, panel1.Height));
            float scaleFactorX = (float)e.PageBounds.Width / bm.Width;
            float scaleFactorY = (float)e.PageBounds.Height / bm.Height;
            float scaleFactor = Math.Min(scaleFactorX, scaleFactorY);
            int scaledWidth = (int)(bm.Width * scaleFactor);
            int scaledHeight = (int)(bm.Height * scaleFactor);
            int offsetX = (e.PageBounds.Width - scaledWidth) / 2;
            int offsetY = 0; 
            e.Graphics.DrawImage(bm, new System.Drawing.Rectangle(offsetX, offsetY, scaledWidth, scaledHeight));
        }
        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            if (txtBarcode.Text == "")
            {
                checkBox1.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        public void removeitem(string barcode)
        {
            try
            {
                MySqlConnection con = new MySqlConnection("datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root");
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"DELETE FROM storecustomer WHERE Barcode=@barcode", con);
                cmd.Parameters.AddWithValue("@barcode", barcode);
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                if (rowsAffected > 0)
                {
                    MessageBox.Show($"Item with barcode {barcode} removed");
                }
                else
                {
                    MessageBox.Show($"No item found with barcode {barcode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.", "ALERT", MessageBoxButtons.OKCancel);
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("There is no item to remove.");
            }
            else
            {
                bool checkboxChecked = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["chk"].Value))
                    {
                        checkboxChecked = true;
                        break;
                    }
                }
                if (!checkboxChecked)
                {
                    MessageBox.Show("No checkbox is selected.");
                }
                else
                {
                    for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                    {
                        DataGridViewRow row = dataGridView1.Rows[i];
                        if (Convert.ToBoolean(row.Cells["chk"].Value))
                        {
                            string barcode = row.Cells["Barcode"].Value.ToString();
                            removeitem(barcode);
                            dataGridView1.Rows.RemoveAt(i);
                        }
                    }
                    addquantity();
                    UpdateReceiptDetails();
                    clearfields();
                }
            }
        }
        private void UpdateReceiptDetails()
        {
            lblitems.Text = "";
            overallTotal = 0;
            totaltax = 0;
            finaltotal = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[5].Value != null && decimal.TryParse(row.Cells[5].Value.ToString(), out decimal cost))
                {
                    decimal itemTax = cost * 0.10m;
                    decimal total = cost + itemTax;

                    string itemDetails = $"{row.Cells[3].Value}{Environment.NewLine}" +
                                         $"{row.Cells[1].Value}{Environment.NewLine}" +
                                         $"{row.Cells[2].Value,-22} ¥ {cost:n0}{Environment.NewLine}" +
                                         $"**************************************{Environment.NewLine}{Environment.NewLine}";
                    lblitems.Text += itemDetails;

                    overallTotal += cost;
                }
            }
            totaltax = overallTotal * 0.10m;
            finaltotal = overallTotal + totaltax;
            lblSubtotal.Location = new Point(lblSubtotal.Location.X, lblitems.Location.Y + lblitems.Height + 10); 
            lblSubtotal.Text = $"Subtotal: ¥ {overallTotal.ToString("n0")}{Environment.NewLine}Tax:        ¥ {totaltax.ToString("n0")}{Environment.NewLine}";
            lblTotal.Location = new Point(lblTotal.Location.X, lblSubtotal.Location.Y + lblSubtotal.Height + 5); 
            lblTotal.Text = $"Total:    ¥ {finaltotal.ToString("n0")}";
            lblTotal.Font = new Font(lblTotal.Font, FontStyle.Bold); 
        }
        private void txtCost_TextChanged(object sender, EventArgs e)
        {
        }
        private void txtFinalCost_TextChanged(object sender, EventArgs e)
        {
            if (txtFinalCost.Text != string.Empty)
            {
                int cursorPosition = txtFinalCost.SelectionStart;
                string unformattedInput = txtFinalCost.Text.Replace(",", "");
                if (decimal.TryParse(unformattedInput, out decimal num))
                {
                    string formattedNum = num.ToString("N0");
                    txtFinalCost.Text = formattedNum;
                    txtFinalCost.SelectionStart = cursorPosition + (formattedNum.Length - unformattedInput.Length);
                    decimal calcTax = num - (num * 0.1m); 
                    decimal calc = calcTax + (calcTax * 0.010101m); 
                    decimal calccardfee = calcTax - (calcTax * 0.032m); 
                    decimal total = calccardfee;
 
                    if (decimal.TryParse(txtCost.Text.Replace(",", ""), out decimal originalCost))
                    {
                        decimal finalTotal1 = calcTax * 1.1111m; 
                        decimal finalTotal = total - originalCost;
                   
                        txtFinalCostTax.Text = calc.ToString("N2"); 
                        txtCardfee.Text = finalTotal.ToString("N0"); 

                        if (finalTotal < 0)
                        {
                            pictureBox3.Visible = true; // Show the picture box
                        }
                        else
                        {
                            pictureBox3.Visible = false; // Hide the picture box if not negative
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid value in txtCost.", "ERROR", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    txtFinalCost.Text = "";
                    txtFinalCostTax.Text = "";
                    txtCardfee.Text = "";
                }
            }
        }
        private void lblEmployee_Click(object sender, EventArgs e)
        {

        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                this.Hide();
                frmLogin fl = new frmLogin();
                fl.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please cancel your transaction before moving to other page.");
            }        
        }

        private void officialInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                this.Hide();
                frmInvoice fi = new frmInvoice();
                fi.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please cancel your transaction before moving to other page.");
            }

        }

        private void itemSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                this.Hide();
                frmItemSearch fl = new frmItemSearch();
                fl.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please cancel your transaction before moving to other page.");
            }
         
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)
            {
                DataGridViewRow row= dataGridView1.Rows[e.RowIndex];
                txtID.Text = row.Cells["Inventoryid"].Value.ToString();
            }
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string idValue = txtID.Text;
                MySqlConnection con = new MySqlConnection("datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root");
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from inventory where Inventoryid ='" + idValue + "'", con);
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
                        string columnNameValue15 = reader.GetString("Quantity");
                        int columnNameValue16 = (int)reader["Inventoryid"];
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
                        txtQuantity.Text = columnNameValue15;
                        txtID.Text = columnNameValue16.ToString();
                        string input1 = txtCost.Text;
                        long num1;
                        if (long.TryParse(input1, out num1))
                        {
                            string formattedNum1 = num1.ToString("n0");
                            txtCost.Text = formattedNum1;
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.","ALERT",MessageBoxButtons.OKCancel);
            }
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                this.Hide();
                frmCalculator fc=new frmCalculator();
                fc.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please cancel your transaction before moving to other page.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
                if (checkBox1.Checked == true)
                {
                    txtSale.Visible = true;
                    percentPicture.Visible = true;

                }
                else
                {
                    txtSale.Visible = false;
                    percentPicture.Visible = false;
                }
            
        }

        private void txtSale_TextChanged(object sender, EventArgs e)
        {
            string stringsale=txtSale.Text;
            double sale=Convert.ToDouble(stringsale);
            string stringcost = txtCost.Text;
            double cost=Convert.ToDouble(stringcost);
            double percent = sale / 100;
            double total = cost-(cost * percent) ;
            txtFinalCost.Text = total.ToString();
        }

        private void salesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSalesReport fs=new frmSalesReport();
            fs.ShowDialog();       
        }
    }
}
