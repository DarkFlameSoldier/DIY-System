using System;
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

            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                sqlconnection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlconnection;

                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    cmd.CommandText = "UPDATE Users SET Username = @User, Email = @Email, ExperienceLevelId = @ExpId WHERE UserId = @UserId";
                }
                else
                {
                    cmd.CommandText = "UPDATE Users SET Username = @User, Password = @Pass, Email = @Email, ExperienceLevelId = @ExpId WHERE UserId = @UserId";
                    cmd.Parameters.AddWithValue("@Pass", textBox2.Text);
                }

                cmd.Parameters.AddWithValue("@User", textBox1.Text);
                cmd.Parameters.AddWithValue("@Email", textBox3.Text);

                if (comboBox1.SelectedValue != null)
                {
                    cmd.Parameters.AddWithValue("@ExpId", Convert.ToInt32(comboBox1.SelectedValue));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ExpId", DBNull.Value);
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

        private void LoadExistingUserData()
        {
            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = "SELECT Username, Email, ExperienceLevelId FROM Users WHERE UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(query, sqlconnection))
                {
                    cmd.Parameters.AddWithValue("@UserId", _userIdEdit);
                    sqlconnection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["Username"].ToString();
                            textBox3.Text = reader["Email"].ToString();

                            if (reader["ExperienceLevelId"] != DBNull.Value)
                            {
                                comboBox1.SelectedValue = reader["ExperienceLevelId"];
                            }
                        }
                    }
                }
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}

