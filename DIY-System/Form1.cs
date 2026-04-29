using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DIY_System
{
    public partial class Form1 : Form
    {
        SqlConnection sqlconnection;
        SqlCommand sqlcommand;
        string query;
        SqlDataAdapter sqladapter;
        string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivo\source\repos\DIY-System\DIY-System\DIY.mdf;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Hide();
        }

        private bool logIn(object sender, EventArgs e)
        {
            sqlconnection = new SqlConnection(cs);
            sqlconnection.Open();
            query = "SELECT * FROM Users WHERE UserName='" + txtUsername.Text + "' AND Password='" + txtPassword.Text + "'";
            sqlcommand = new SqlCommand(query, sqlconnection);
            sqladapter = new SqlDataAdapter(sqlcommand);
            DataTable dataTable = new DataTable();
            sqladapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserForm uf= new UserForm();

            if (logIn(sender, e))
            {
                MessageBox.Show("Login successful!");
                uf.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }
    }
}
