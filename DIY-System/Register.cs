using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DIY_System
{
    public partial class Register : Form
    {

        SqlConnection sqlconnection;
        SqlCommand sqlcommand;
        string query;
        SqlDataAdapter sqladapter;
        string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivo\source\repos\DIY-System\DIY-System\DIY.mdf;Integrated Security=True";

        public Register()
        {
            InitializeComponent();
        }

        private void ClearData()
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Regex rule = new Regex(@"^[a-zA-Z0-9]+$");
            if (!rule.IsMatch(e.KeyChar.ToString()) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            Regex rule = new Regex(@"^[a-zA-Z0-9]+$");
            if (!rule.IsMatch(e.KeyChar.ToString()) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private bool checkUserName(object sender, EventArgs e)
        {
            bool exists = false;

            sqlconnection = new SqlConnection(cs);
            sqlconnection.Open();
            query = "Select * FROM Users WHERE Username=@UsName";
            sqlcommand = new SqlCommand(query, sqlconnection);
            sqlcommand.Parameters.AddWithValue("@UsName", textBox1.Text);
            sqladapter = new SqlDataAdapter(sqlcommand);
            DataTable dataTable = new DataTable();
            sqladapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Username already exists!");
                textBox1.Clear();
                textBox1.Focus();
                exists = true;
            }

            sqlconnection.Close();
            return exists;
        }

        private bool checkEmail(object sender, EventArgs e)
        {
            bool exists = false;
            sqlconnection = new SqlConnection(cs);
            sqlconnection.Open();
            query = "Select * FROM Users WHERE Email=@Email";
            sqlcommand = new SqlCommand(query, sqlconnection);
            sqlcommand.Parameters.AddWithValue("@Email", textBox3.Text);
            sqladapter = new SqlDataAdapter(sqlcommand);
            DataTable dataTable = new DataTable();
            sqladapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Email already exists!");
                textBox3.Clear();
                textBox3.Focus();
                exists = true;
            }
            sqlconnection.Close();
            return exists;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {

                if (!string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    Regex emailRule = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");

                    if (!emailRule.IsMatch(textBox3.Text))
                    {
                        MessageBox.Show("Please enter a valid email address.", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        textBox3.Focus();

                        return;
                    }
                }


                if (checkUserName(sender, e) == true)
                {
                    return;
                }

                if (checkEmail(sender, e) == true)
                {
                    return;
                }

                sqlconnection = new SqlConnection(cs);
                sqlconnection.Open();
                query = "Insert INTO Users (Username,Email,ExperienceLevel,RoleId,Password) VALUES(@UsName,@Email,@Ex,@Role,@Pass)";
                sqlcommand = new SqlCommand(query, sqlconnection);
                sqlcommand.Parameters.AddWithValue("@UsName", textBox1.Text);
                sqlcommand.Parameters.AddWithValue("@Email", textBox3.Text);
                sqlcommand.Parameters.AddWithValue("@Pass", textBox4.Text);
                sqlcommand.Parameters.AddWithValue("@Ex", comboBox1.SelectedItem.ToString());
                sqlcommand.Parameters.AddWithValue("@Role", 1);
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
                ClearData();
                MessageBox.Show("Inserted successfully!");

                if (CurrentUser.IsAdmin)
                {
                    this.Close();
                }
                else
                {
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Close();
                }

            }
            else
            {
                MessageBox.Show("Please fill in all fields!");
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Register_FormClosed(object sender, FormClosedEventArgs e)
        { 

        }
    }
}
