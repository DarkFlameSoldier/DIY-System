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

                if (comboBox1.SelectedValue == null)
                {
                    MessageBox.Show("Please select an Experience Level.");
                    return;
                }

                using (SqlConnection sqlconnection = new SqlConnection(cs))
                {
                    sqlconnection.Open();

                    string checkUser = "SELECT COUNT(*) FROM Users WHERE Username = @User";
                    using (SqlCommand cmdCheck = new SqlCommand(checkUser, sqlconnection))
                    {
                        cmdCheck.Parameters.AddWithValue("@User", textBox1.Text);
                        int userExists = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (userExists > 0)
                        {
                            MessageBox.Show("That username is already taken. Please choose another.");
                            return;
                        }
                    }

                    string insertQuery = @"INSERT INTO Users (Username, Password, RoleId, Email, ExperienceLevelId) 
                               VALUES (@User, @Pass, 1, @Email, @ExpId)";

                    using (SqlCommand cmdInsert = new SqlCommand(insertQuery, sqlconnection))
                    {
                        cmdInsert.Parameters.AddWithValue("@User", textBox1.Text);
                        cmdInsert.Parameters.AddWithValue("@Pass", textBox4.Text);
                        cmdInsert.Parameters.AddWithValue("@Email", textBox3.Text);
                        cmdInsert.Parameters.AddWithValue("@ExpId", Convert.ToInt32(comboBox1.SelectedValue));

                        cmdInsert.ExecuteNonQuery();
                    }

                    MessageBox.Show("Account successfully created!");

                    if (CurrentUser.IsAdmin)
                    {
                        this.Close();
                    }
                    else
                    {
                        Form1 login = new Form1();
                        login.Show();
                        this.Hide();
                    }
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

        private void LoadExperienceLevels()
        {
            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = "SELECT ExperienceLevelId, LevelName FROM ExperienceLevels";
                using (SqlCommand cmd = new SqlCommand(query, sqlconnection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dtLevels = new DataTable();
                    adapter.Fill(dtLevels);

                    comboBox1.DataSource = dtLevels;
                    comboBox1.DisplayMember = "LevelName";
                    comboBox1.ValueMember = "ExperienceLevelId";
                }
            }
        }
        private void Register_Load(object sender, EventArgs e)
        {
            LoadExperienceLevels();
        }
    }
}
