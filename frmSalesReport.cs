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
    public partial class frmSalesReport : Form
    {
        public frmSalesReport()
        {
            InitializeComponent();
        }

        private DataTable dt;

        public void grid()
        {
            try
            {
                string connection = "datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root";
                string query = "SELECT Barcode, Brand, ItemName,Date,MOP,Seller,Cost,Price from storecustomer WHERE MONTH(Date) = MONTH(CURRENT_DATE()) AND YEAR(Date) = YEAR(CURRENT_DATE()) limit 100000";
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataTable();
                da.Fill(dt);

                dt.Columns.Add("Profit", typeof(decimal));

                foreach (DataRow row in dt.Rows)
                {
                    decimal cost = Convert.ToDecimal(row["Cost"]) ;
                    decimal price = Convert.ToDecimal(row["Price"]) ;
                    row["Cost"] = cost.ToString("#,0");
                    row["Price"] = price.ToString("#,0");
                    decimal profit = price - cost;
                    if (profit % 1 == 0)
                    {
                        row["Profit"] = Math.Truncate(profit);
                    }
                    else
                    {
                        row["Profit"] = Math.Round(profit, 2);
                    }
                }

                dataGridView1.DataSource = dt;
                dataGridView1.Columns["Profit"].DefaultCellStyle.Format = "N2";
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void gridlist()
        {
            try
            {
                string connection = "datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root";
                string query = "SELECT Barcode, Brand, ItemName,Date,MOP,Seller,Cost,Price from storecustomer WHERE MONTH(Date) = MONTH(CURRENT_DATE()) AND YEAR(Date) = YEAR(CURRENT_DATE()) limit 100000";
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataTable();
                da.Fill(dt);

                dt.Columns.Add("Profit", typeof(decimal));

                foreach (DataRow row in dt.Rows)
                {
                    decimal cost = Convert.ToDecimal(row["Cost"]);
                    decimal price = Convert.ToDecimal(row["Price"]);
                    row["Cost"] = cost.ToString("#,0");
                    row["Price"] = price.ToString("#,0");
                    decimal profit = price - cost;
                    if (profit % 1 == 0)
                    {
                        row["Profit"] = Math.Truncate(profit);
                    }
                    else
                    {
                        row["Profit"] = Math.Round(profit, 2);
                    }
                }

                dataGridView1.DataSource = dt;
                dataGridView1.Columns["Profit"].DefaultCellStyle.Format = "N0";
                var liverProfits = dt.AsEnumerable()
                                   .GroupBy(row => row.Field<string>("Seller"))
                                   .Select(group =>
                                       new
                                       {
                                           Seller = group.Key,
                                           TotalProfit = group.Sum(row => row.Field<decimal>("Profit"))
                                       })
                                   .OrderByDescending(item => item.TotalProfit);

                decimal totalProfit = 0m;

                foreach (var liverProfit in liverProfits)
                {
                    listbox1.Items.Add($"{liverProfit.Seller}:  ¥ {liverProfit.TotalProfit.ToString("N2")}");
                    totalProfit += liverProfit.TotalProfit;
                }

                lblProfit.Text = $" ¥ {totalProfit.ToString("N0")}";

 
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

         public void searchdate()
        {
            try
            {
                listbox1.Items.Clear();
                string connection = "datasource=localhost;database=therealmaluho;port=3306;userid=root;password=root";
                string query = "SELECT Barcode, Brand, ItemName,Date,MOP,Seller,Cost,Price FROM storecustomer WHERE Date BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' limit 10000000 ";
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlCommand cmd = new MySqlCommand(query, conn);
               
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dataGridView1.RowTemplate.Height = 100;
                dataGridView1.AllowUserToAddRows = false;
                da.Fill(dt);

                dt.Columns.Add("Profit", typeof(decimal));

                foreach (DataRow row in dt.Rows)
                {
                    decimal cost = Convert.ToDecimal(row["Cost"]) ;
                    decimal price = Convert.ToDecimal(row["Price"]);
                    row["Cost"] = cost.ToString("#,0");
                    row["Price"] = price.ToString("#,0");
                    decimal profit = price - cost;
                    if (profit % 1 == 0)
                    {     
                        row["Profit"] = Math.Truncate(profit);
                    }
                    else
                    {
                        row["Profit"] = Math.Round(profit, 2);
                    }
                }
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["Profit"].DefaultCellStyle.Format = "N0";     
                var liverProfits = dt.AsEnumerable()
                                    .GroupBy(row => row.Field<string>("Seller"))
                                    .Select(group =>
                                        new
                                        {
                                            Seller = group.Key,
                                            TotalProfit = group.Sum(row => row.Field<decimal>("Profit"))
                                        }).OrderByDescending(item => item.TotalProfit);


                decimal totalProfit = 0m;

                foreach (var liverProfit in liverProfits)
                {
                    listbox1.Items.Add($"{liverProfit.Seller}:  ¥ {liverProfit.TotalProfit.ToString("N2")}");
                    totalProfit += liverProfit.TotalProfit;
                }

                lblProfit.Text = $" ¥ {totalProfit.ToString("N0")}";

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Please contact your administrator");
            }
        }
        private void frmSalesReport_Load(object sender, EventArgs e)
        {
            grid();
            gridlist();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            searchdate();
        }

        private void officialInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmInvoice fr=new frmInvoice();
            fr.ShowDialog();
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmCalculator calculator = new frmCalculator();
            calculator.ShowDialog();
        }

        private void receiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Receipt receipt = new Receipt();
            receipt.ShowDialog();
        }

        private void itemSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmItemSearch search = new frmItemSearch();
            search.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
