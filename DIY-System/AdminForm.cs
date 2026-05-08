using System;
using System.Collections;
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
    public partial class AdminForm : Form
    {
        SqlConnection sqlconnection;
        SqlCommand sqlcommand;
        string query;
        DataTable datatable;
        SqlDataAdapter sqladpter;
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

        public AdminForm()
        {
            InitializeComponent();
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;

            button11.Visible = false;
            button12.Visible = false;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            DisplayData();
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;

            button11.Visible = false;
            button12.Visible = false;
            textBox1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisplayData();
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            textBox1.Visible = false;
            button11.Visible = false;
            button12.Visible = false;

            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            CreateProject crp = new CreateProject();
            crp.ShowDialog();
            DisplayData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;

            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button11.Visible = false;
            button12.Visible = false;
            textBox1.Visible = false;

            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = @"
            SELECT 
                u.UserId, 
                u.Username, 
                u.Email, 
                u.Password,
                e.LevelName AS [Experience Level]
            FROM Users u
            LEFT JOIN ExperienceLevels e ON u.ExperienceLevelId = e.ExperienceLevelId
            WHERE u.RoleId = 1";

                using (SqlCommand sqlcommand = new SqlCommand(query, sqlconnection))
                {
                    SqlDataAdapter sqladapter = new SqlDataAdapter(sqlcommand);
                    DataTable datatable = new DataTable();

                    sqladapter.Fill(datatable);

                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();

                    if (datatable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = datatable;
                    }
                    else
                    {
                        MessageBox.Show("No normal users found in the database.");
                    }
                }
            }
        }

        

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a project from the list to delete.");
                return;
            }

            int selectedProjectId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProjectId"].Value);

            DialogResult dialogResult = MessageBox.Show("ADMIN ACTION: Are you sure you want to permanently delete this project? This cannot be undone.", "Confirm Admin Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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

                    string deleteProjectQuery = "DELETE FROM Projects WHERE ProjectId = @ProjId";
                    using (SqlCommand cmdProject = new SqlCommand(deleteProjectQuery, sqlconnection))
                    {
                        cmdProject.Parameters.AddWithValue("@ProjId", selectedProjectId);

                        int rowsAffected = cmdProject.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Project successfully deleted by Admin!");
                            DisplayData();
                        }
                        else
                        {
                            MessageBox.Show("Error: Project could not be found. It may have already been deleted.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a project from the list to edit.");
                return;
            }

            int selectedProjectId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProjectId"].Value);

            EditProject editForm = new EditProject(selectedProjectId);
            editForm.ShowDialog();

            button1.PerformClick();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Register rg = new Register();

            rg.ShowDialog();

            button1.PerformClick();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a user from the list to delete.");
                return;
            }

            int selectedUserId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["UserId"].Value);

            if (selectedUserId == CurrentUser.UserID)
            {
                MessageBox.Show("Action Denied: You cannot delete your own admin account!", "Security Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedUserId == 8)
            {
                MessageBox.Show("Action Denied: You cannot delete the system 'deletedUser' account!", "Security Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show(
                "Are you sure you want to permanently delete this user? \n\nAny projects they created will be safely transferred to the 'deletedUser' placeholder account.",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection sqlconnection = new SqlConnection(cs))
                {
                    sqlconnection.Open();

                    string transferQuery = "UPDATE Projects SET UserId = 8 WHERE UserId = @TargetId";
                    using (SqlCommand cmdTransfer = new SqlCommand(transferQuery, sqlconnection))
                    {
                        cmdTransfer.Parameters.AddWithValue("@TargetId", selectedUserId);

                        cmdTransfer.ExecuteNonQuery();
                    }

                    string deleteUserQuery = "DELETE FROM Users WHERE UserId = @TargetId";
                    using (SqlCommand cmdDelete = new SqlCommand(deleteUserQuery, sqlconnection))
                    {
                        cmdDelete.Parameters.AddWithValue("@TargetId", selectedUserId);
                        int rowsAffected = cmdDelete.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User successfully deleted and their projects were reassigned!");

                            button1.PerformClick();
                        }
                        else
                        {
                            MessageBox.Show("Error: User could not be found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a user from the list to edit.");
                return;
            }

            int selectedUserId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["UserId"].Value);

            if (selectedUserId == 8)
            {
                MessageBox.Show("Action Denied: You cannot edit the system 'deletedUser' account!", "Security Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EditUser editForm = new EditUser(selectedUserId);
            editForm.ShowDialog();

            button1.PerformClick();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            button11.Visible = true;
            button12.Visible = true;
            textBox1.Visible = true;

            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;

            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;

            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = @"
            SELECT 
                CategoryId AS [Category Id],
                Name AS [Category Name]
            FROM Categories";

                using (SqlCommand sqlcommand = new SqlCommand(query, sqlconnection))
                {
                    SqlDataAdapter sqladapter = new SqlDataAdapter(sqlcommand);
                    DataTable datatable = new DataTable();

                    sqladapter.Fill(datatable);

                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();

                    if (datatable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = datatable;
                    }
                    else
                    {
                        MessageBox.Show("No normal users found in the database.");
                    }
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string newCategory = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(newCategory))
            {
                MessageBox.Show("Please enter a category name.");
                return;
            }

            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                sqlconnection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Categories WHERE Name = @Name";
                using (SqlCommand cmdCheck = new SqlCommand(checkQuery, sqlconnection))
                {
                    cmdCheck.Parameters.AddWithValue("@Name", newCategory);
                    int exists = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (exists > 0)
                    {
                        MessageBox.Show("This category already exists!");
                        return;
                    }
                }

                string insertQuery = "INSERT INTO Categories (Name) VALUES (@Name)";
                using (SqlCommand cmdInsert = new SqlCommand(insertQuery, sqlconnection))
                {
                    cmdInsert.Parameters.AddWithValue("@Name", newCategory);
                    cmdInsert.ExecuteNonQuery();

                    MessageBox.Show("Category added successfully!");
                    textBox1.Clear();

                    button13.PerformClick();
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a category from the list to delete.");
                return;
            }

            int categoryId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Category Id"].Value);
            string categoryName = dataGridView1.CurrentRow.Cells["Category Name"].Value.ToString();

            DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete the category '{categoryName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection sqlconnection = new SqlConnection(cs))
                {
                    sqlconnection.Open();

                    string checkProjectsQuery = "SELECT COUNT(*) FROM Projects WHERE CategoryId = @CatId";
                    using (SqlCommand cmdCheck = new SqlCommand(checkProjectsQuery, sqlconnection))
                    {
                        cmdCheck.Parameters.AddWithValue("@CatId", categoryId);
                        int projectsCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (projectsCount > 0)
                        {
                            MessageBox.Show($"Cannot delete this category because it is currently used by {projectsCount} project(s). You must reassign or delete those projects first.", "Action Blocked", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string deleteQuery = "DELETE FROM Categories WHERE CategoryId = @CatId";
                    using (SqlCommand cmdDelete = new SqlCommand(deleteQuery, sqlconnection))
                    {
                        cmdDelete.Parameters.AddWithValue("@CatId", categoryId);
                        cmdDelete.ExecuteNonQuery();

                        MessageBox.Show("Category deleted successfully!");

                        button13.PerformClick();
                    }
                }
            }
        }
    }
}
