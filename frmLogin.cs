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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
        private MySqlConnection con;
        public TextBox TextBox1
        {
            get { return txtPin; }
            set { txtPin = value; }
        }
        public void login()
        {
            try
            {
                con = new MySqlConnection("datasource=localhost;database=maluhotimesheet;port=3306;userid=root;password=root");
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user where Barcode = '" + txtPin.Text + "'", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    this.Hide();
                    Receipt rc = new Receipt();
                    rc.Label1.Text = txtPin.Text;
                    rc.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Log-in failed, Please check your username and password");
                    txtPin.Clear();
                }
                reader.Close();
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}. Please contact your administrator.", "ALERT", MessageBoxButtons.OKCancel);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            login();
        }

        private void cbShowpassword_CheckedChanged(object sender, EventArgs e)
        {
            if(cbShowpassword.Checked)
            {
                txtPin.UseSystemPasswordChar = false;
            }
            else
            {
                txtPin.UseSystemPasswordChar=true;
            }
        }
        private void frmLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                login();
            }
        }
    }
}
