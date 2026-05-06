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

        private bool VerifyLogIn()
        {
            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = "SELECT UserId, RoleId FROM Users WHERE Username=@Username AND Password=@Password";

                using (SqlCommand sqlcommand = new SqlCommand(query, sqlconnection))
                {
                    sqlcommand.Parameters.AddWithValue("@Username", txtUsername.Text);
                    sqlcommand.Parameters.AddWithValue("@Password", txtPassword.Text);

                    sqlconnection.Open();

                    using (SqlDataReader reader = sqlcommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CurrentUser.UserID = Convert.ToInt32(reader["UserId"]);
                            CurrentUser.RoleID = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 1;

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (VerifyLogIn())
            {
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
