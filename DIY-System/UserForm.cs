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
        string Query;
        DataTable datatable;
        SqlDataAdapter sqladpter;
        int ID = 0;
        string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivo\source\repos\DIY-System\DIY-System\DIY.mdf;Integrated Security=True";

        private void DisplayData()
        {
            sqlconnection = new SqlConnection(cs);
            Query = @"SELECT 
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
            sqlcommand = new SqlCommand(Query, sqlconnection);
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
            DisplayData();
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
            Query = @"SELECT 
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
            sqlcommand = new SqlCommand(Query, sqlconnection);
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
            // 1. Check if they actually clicked on a row in the grid
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a project from the list to delete.");
                return;
            }

            // 2. Extract the ProjectId from the hidden column in the selected row
            // (Make sure "ProjectId" exactly matches the column name from your SQL query!)
            int selectedProjectId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProjectId"].Value);

            // 3. Safety Check: Ask them if they are absolutely sure!
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to permanently delete this project?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection sqlconnection = new SqlConnection(cs))
                {
                    sqlconnection.Open();

                    // ==========================================
                    // STEP 1: Delete the linked materials first!
                    // ==========================================
                    string deleteMaterialsQuery = "DELETE FROM ProjectMaterials WHERE ProjectId = @ProjId";
                    using (SqlCommand cmdMaterials = new SqlCommand(deleteMaterialsQuery, sqlconnection))
                    {
                        cmdMaterials.Parameters.AddWithValue("@ProjId", selectedProjectId);
                        cmdMaterials.ExecuteNonQuery(); // This safely un-links all the materials
                    }

                    // ==========================================
                    // STEP 2: Now delete the actual project!
                    // ==========================================
                    string deleteProjectQuery = "DELETE FROM Projects WHERE ProjectId = @ProjId AND UserId = @UserId";
                    using (SqlCommand cmdProject = new SqlCommand(deleteProjectQuery, sqlconnection))
                    {
                        cmdProject.Parameters.AddWithValue("@ProjId", selectedProjectId);
                        cmdProject.Parameters.AddWithValue("@UserId", CurrentUser.UserID);

                        int rowsAffected = cmdProject.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Project successfully deleted!");
                            DisplayData(); // Refresh the grid
                        }
                        else
                        {
                            MessageBox.Show("Error: You do not have permission to delete this project.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
