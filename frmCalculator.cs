﻿using System;
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
    public partial class frmCalculator : Form
    {
        public frmCalculator()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        public void clearfields()
        {
            txtBarcode.Clear();
            txtBrand.Clear();
            txtCost.Clear();
            txtPrice.Clear();
            txtTax.Clear();
            txtCardFee.Clear();
            txtProfit.Clear();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtCost.Text != "")
            {
                clearfields();
            }
            else if (txtCost.Lines.Length == 2)
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
                                string columnNameValue = reader.GetString("Brand");
                                string columnNameValue1 = reader.GetString("ItemCostYen");
                                                             
                                txtCost.Text = columnNameValue1;
                                txtBrand.Text = columnNameValue;
                                
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
                            txtBrand.Text = "this item is not yet recorded in the inventory";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.", "ALERT", MessageBoxButtons.OKCancel);
                }
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if(txtPrice.Text!=string.Empty)
            {
                int cursorPosition = txtPrice.SelectionStart;
                string unformattedInput = txtPrice.Text.Replace(",", "");
                if(decimal.TryParse(unformattedInput,out decimal num))
                {
                    string formattedNum = num.ToString("N0");

                    txtPrice.Text = formattedNum;

                    txtPrice.SelectionStart = cursorPosition + (formattedNum.Length - unformattedInput.Length);

                    decimal calcTax = num - (num * 0.1m);
                    decimal calc = calcTax + (calcTax * 0.0101m);
                    decimal calccardfee = calcTax - (calcTax * 0.032m);
                    decimal total = calccardfee;

                    if (decimal.TryParse(txtCost.Text.Replace(",", ""), out decimal originalCost))
                    {
                        decimal finalTotal1 = calcTax * 1.1111m;
                        decimal finalTotal = total - originalCost;
                        txtTax.Text = calcTax.ToString("N0");
                        txtCardFee.Text = calccardfee.ToString("N0");                      
                        txtProfit.Text = finalTotal.ToString("N0");                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid value in txtCost.", "ERROR", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void officialInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmInvoice fi=new frmInvoice();
            fi.ShowDialog();
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void itemSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmItemSearch fi=new frmItemSearch();
            fi.ShowDialog();
        }

        private void receiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Receipt receipt=new Receipt();
            receipt.ShowDialog();

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin fl=new frmLogin();
            fl.ShowDialog();
        }
    }
    }

