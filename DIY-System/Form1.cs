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

        private int VerifyLogIn()
        {
            int fetchedUserId = 0;

            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = "SELECT UserId FROM Users WHERE Username=@Username AND Password=@Password";

                using (SqlCommand sqlcommand = new SqlCommand(query, sqlconnection))
                {
                    sqlcommand.Parameters.AddWithValue("@Username", txtUsername.Text);
                    sqlcommand.Parameters.AddWithValue("@Password", txtPassword.Text);

                    sqlconnection.Open();

                    object result = sqlcommand.ExecuteScalar();

                    if (result != null)
                    {
                        fetchedUserId = Convert.ToInt32(result);
                    }
                }
            }

            return fetchedUserId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int loggedInUserId = VerifyLogIn();

            if (loggedInUserId > 0)
            { 
                CurrentUser.UserID = loggedInUserId;

                MessageBox.Show("Login successful!");

                UserForm uf = new UserForm();
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
