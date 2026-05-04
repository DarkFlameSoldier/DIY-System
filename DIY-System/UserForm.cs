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
using System.Xml.Linq;

namespace DIY_System
{
    public partial class UserForm : Form
    {

        DataView gridDataSource;
        SqlConnection sqlconnection;
        SqlCommand sqlcommand;
        string query;
        DataTable datatable;
        SqlDataAdapter sqladpter;
        int ID = 0;
        string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivo\source\repos\DIY-System\DIY-System\DIY.mdf;Integrated Security=True";

        private void DisplayData()
        {
            sqlconnection = new SqlConnection(cs);
            query = @"SELECT 
                p.ProjectId, 
                p.Title, 
                p.Description, 
                p.Instructions, 
                p.PhotoPath, 
                u.Username AS[Author],
                c.Name AS[Category]
                FROM Projects p
                INNER JOIN Users u ON p.UserId = u.UserId
                INNER JOIN Categories c ON p.CategoryId = c.CategoryId";
            sqlcommand = new SqlCommand(query, sqlconnection);
            sqladpter = new SqlDataAdapter();
            datatable = new DataTable();

            sqladpter.SelectCommand = sqlcommand;
            sqladpter.Fill(datatable);

            dataGridView1.DataSource = datatable;
        }

        public UserForm()
        {
            InitializeComponent();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            button7.Visible = CurrentUser.IsAdmin;

            DisplayData();
            LoadCategories();
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateProject crp = new CreateProject();
            crp.ShowDialog();
            DisplayData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sqlconnection = new SqlConnection(cs);
            query = @"SELECT 
                p.ProjectId, 
                p.Title, 
                p.Description, 
                p.Instructions, 
                p.PhotoPath, 
                u.Username AS[Author],
                c.Name AS[Category]
                FROM Projects p
                INNER JOIN Users u ON p.UserId = u.UserId
                INNER JOIN Categories c ON p.CategoryId = c.CategoryId
                WHERE p.UserId = @LoggedInUser";
            sqlcommand = new SqlCommand(query, sqlconnection);
            sqlcommand.Parameters.AddWithValue("@LoggedInUser", CurrentUser.UserID);
            sqladpter = new SqlDataAdapter();
            datatable = new DataTable();

            sqladpter.SelectCommand = sqlcommand;
            sqladpter.Fill(datatable);

            dataGridView1.DataSource = datatable;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a project from the list to delete.");
                return;
            }

            int selectedProjectId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProjectId"].Value);

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to permanently delete this project?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection sqlconnection = new SqlConnection(cs))
                {
                    sqlconnection.Open();

                    string deleteMaterialsQuery = "DELETE FROM ProjectMaterials WHERE ProjectId = @ProjId";
                    using (SqlCommand cmdMaterials = new SqlCommand(deleteMaterialsQuery, sqlconnection))
                    {
                        cmdMaterials.Parameters.AddWithValue("@ProjId", selectedProjectId);
                        cmdMaterials.ExecuteNonQuery();
                    }

                    string deleteProjectQuery = "DELETE FROM Projects WHERE ProjectId = @ProjId AND UserId = @UserId";
                    using (SqlCommand cmdProject = new SqlCommand(deleteProjectQuery, sqlconnection))
                    {
                        cmdProject.Parameters.AddWithValue("@ProjId", selectedProjectId);
                        cmdProject.Parameters.AddWithValue("@UserId", CurrentUser.UserID);

                        int rowsAffected = cmdProject.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Project successfully deleted!");
                            DisplayData();
                        }
                        else
                        {
                            MessageBox.Show("Error: You do not have permission to delete this project.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a project to view.");
                return;
            }

            int selectedProjectId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProjectId"].Value);

            ViewProject viewForm = new ViewProject(selectedProjectId);

            viewForm.ShowDialog();
        }

        private void LoadCategories()
        {
            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = "SELECT CategoryId, Name FROM Categories";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlconnection);
                DataTable categoriesTable = new DataTable();
                adapter.Fill(categoriesTable);
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "CategoryId";
                comboBox1.DataSource = categoriesTable;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select a category to search for.");
                return;
            }

            int selectedSearchId = Convert.ToInt32(comboBox1.SelectedValue);

            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = @"SELECT 
                            p.ProjectId, 
                            p.Title, 
                            p.Description, 
                            p.Instructions, 
                            p.PhotoPath, 
                            u.Username AS [Author], 
                            c.Name AS [Category]
                          FROM Projects p
                          INNER JOIN Users u ON p.UserId = u.UserId
                          INNER JOIN Categories c ON p.CategoryId = c.CategoryId
                          WHERE p.CategoryId = @CatId";

                using (SqlCommand sqlcommand = new SqlCommand(query, sqlconnection))
                {
                    sqlcommand.Parameters.AddWithValue("@CatId", selectedSearchId);

                    SqlDataAdapter sqladapter = new SqlDataAdapter(sqlcommand);
                    DataTable datatable = new DataTable();
                    sqladapter.Fill(datatable);

                    if (datatable.Rows.Count == 0)
                    {
                        dataGridView1.DataSource = null;
                        MessageBox.Show("No projects found in this category yet!", "No Results");
                    }
                    else
                    {
                        dataGridView1.DataSource = datatable;
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm();
            adminForm.ShowDialog();
        }
    }
}
