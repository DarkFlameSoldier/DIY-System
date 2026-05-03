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
    public partial class CreateProject : Form
    {
        SqlConnection sqlconnection;
        SqlCommand sqlcommand;
        string query;
        SqlDataAdapter sqladapter;
        string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivo\source\repos\DIY-System\DIY-System\DIY.mdf;Integrated Security=True";

        string selectedPhotoPath = "";
        public CreateProject()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedPhotoPath = openFileDialog.FileName;


                label6.Text = selectedPhotoPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtInstructions.Text))
            {
                MessageBox.Show("Please fill in at least the Title and Instructions.");
                return;
            }

            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                sqlconnection.Open();

                string projectQuery = @"INSERT INTO Projects (Title, Description, Instructions, PhotoPath, CategoryId, UserId) 
                                VALUES (@Title, @Desc, @Inst, @Photo, @CatId, @UserId);
                                SELECT SCOPE_IDENTITY();";

                int newProjectId = 0;

                using (SqlCommand cmdProject = new SqlCommand(projectQuery, sqlconnection))
                {
                    cmdProject.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmdProject.Parameters.AddWithValue("@Desc", txtDescription.Text);
                    cmdProject.Parameters.AddWithValue("@Inst", txtInstructions.Text);

                    if (string.IsNullOrEmpty(selectedPhotoPath))
                        cmdProject.Parameters.AddWithValue("@Photo", DBNull.Value);
                    else
                        cmdProject.Parameters.AddWithValue("@Photo", selectedPhotoPath);

                    cmdProject.Parameters.AddWithValue("@CatId", Convert.ToInt32(comboBox1.SelectedValue));
                    cmdProject.Parameters.AddWithValue("@UserId", CurrentUser.UserID);

                    newProjectId = Convert.ToInt32(cmdProject.ExecuteScalar());
                }

                string materialQuery = @"
                                        IF NOT EXISTS (SELECT 1 FROM Materials WHERE Name = @MatName)
                                        BEGIN
                                        INSERT INTO Materials (Name) VALUES (@MatName);
                                        END
                                        DECLARE @CurrentMatId INT;
                                        SELECT @CurrentMatId = MaterialId FROM Materials WHERE Name = @MatName;
                                        INSERT INTO ProjectMaterials (ProjectId, MaterialId) VALUES (@ProjId, @CurrentMatId);";

                foreach (string line in txtMaterials.Lines)
                {
                    string materialName = line.Trim();

                    if (!string.IsNullOrWhiteSpace(materialName))
                    {
                        using (SqlCommand cmdMaterial = new SqlCommand(materialQuery, sqlconnection))
                        {
                            cmdMaterial.Parameters.AddWithValue("@MatName", materialName);
                            cmdMaterial.Parameters.AddWithValue("@ProjId", newProjectId);
                            cmdMaterial.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Project successfully created!");
                this.Close();
            }
        }

        private void CreateProject_Load(object sender, EventArgs e)
        {
            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                string query = "SELECT CategoryId, Name FROM Categories";

                using (SqlCommand sqlcommand = new SqlCommand(query, sqlconnection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataTable dtCategories = new DataTable();
                    adapter.Fill(dtCategories);

                    comboBox1.DataSource = dtCategories;

                    comboBox1.DisplayMember = "Name";

                    comboBox1.ValueMember = "CategoryId";
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
