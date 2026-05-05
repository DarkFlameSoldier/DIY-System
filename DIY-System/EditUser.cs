using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DIY_System
{
    public partial class EditUser : Form
    {

        string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivo\source\repos\DIY-System\DIY-System\DIY.mdf;Integrated Security=True";
        private int _userIdEdit;
        public EditUser(int userid)
        {
            InitializeComponent();
            _userIdEdit = userid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                sqlconnection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlconnection;

                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    cmd.CommandText = "UPDATE Users SET Username = @User, Email = @Email, ExperienceLevel = @Exp WHERE UserId = @UserId";
                }
                else
                {
                    cmd.CommandText = "UPDATE Users SET Username = @User, Password = @Pass, Email = @Email, ExperienceLevel = @Exp WHERE UserId = @UserId";
                    cmd.Parameters.AddWithValue("@Pass", textBox2.Text);
                }

                cmd.Parameters.AddWithValue("@User", textBox1.Text);
                cmd.Parameters.AddWithValue("@Email", textBox3.Text);

                if (comboBox1.SelectedItem != null)
                {
                    cmd.Parameters.AddWithValue("@Exp", comboBox1.SelectedItem.ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Exp", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@UserId", _userIdEdit);

                cmd.ExecuteNonQuery();

                MessageBox.Show("User profile successfully updated!");
                this.Close();
            }
        }

        private void EditUser_Load(object sender, EventArgs e)
        {
            LoadExperienceLevels();
            LoadExistingUserData();
        }

        private void LoadExperienceLevels()
        {
            comboBox1.Items.Add("New");
            comboBox1.Items.Add("Advanced");
            comboBox1.Items.Add("Experienced");
        }

        private void LoadExistingUserData()
        {
            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = "SELECT Username, Email, ExperienceLevel, Password FROM Users WHERE UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(query, sqlconnection))
                {
                    cmd.Parameters.AddWithValue("@UserId", _userIdEdit);
                    sqlconnection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["Username"].ToString();
                            textBox2.Text = reader["Password"].ToString();
                            textBox3.Text = reader["Email"].ToString();

                            comboBox1.SelectedItem = reader["ExperienceLevel"].ToString();
                        }
                    }
                }
            }
        }
    }
}

