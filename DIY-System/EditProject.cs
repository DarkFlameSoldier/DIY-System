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
    public partial class EditProject : Form
    {
        private int _projectId;
        private string _currentPhotoPath = "";
        string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivo\source\repos\DIY-System\DIY-System\DIY.mdf;Integrated Security=True";

        public EditProject(int projectId)
        {
            InitializeComponent();
            _projectId = projectId;
        }

        private void LoadCategories()
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

        private void LoadExistingProjectData()
        {
            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                sqlconnection.Open();

                string query = "SELECT Title, Description, Instructions, CategoryId, PhotoPath FROM Projects WHERE ProjectId = @ProjId";
                using (SqlCommand cmd = new SqlCommand(query, sqlconnection))
                {
                    cmd.Parameters.AddWithValue("@ProjId", _projectId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtTitle.Text = reader["Title"].ToString();
                            txtDescription.Text = reader["Description"].ToString();
                            txtInstructions.Text = reader["Instructions"].ToString();
                            comboBox1.SelectedValue = reader["CategoryId"];

                            if (reader["PhotoPath"] != DBNull.Value)
                                _currentPhotoPath = reader["PhotoPath"].ToString();
                        }
                    }
                }

                string matQuery = @"SELECT m.Name FROM Materials m 
                                INNER JOIN ProjectMaterials pm ON m.MaterialId = pm.MaterialId 
                                WHERE pm.ProjectId = @ProjId";
                using (SqlCommand cmdMat = new SqlCommand(matQuery, sqlconnection))
                {
                    cmdMat.Parameters.AddWithValue("@ProjId", _projectId);
                    using (SqlDataReader reader = cmdMat.ExecuteReader())
                    {
                        txtMaterials.Clear();
                        while (reader.Read())
                        {
                            txtMaterials.AppendText(reader["Name"].ToString() + Environment.NewLine);
                        }
                    }
                }
            }
        }


        private void EditProject_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadExistingProjectData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                sqlconnection.Open();

                string updateQuery = @"UPDATE Projects 
                               SET Title = @Title, Description = @Desc, Instructions = @Inst, CategoryId = @CatId, PhotoPath = @Photo 
                               WHERE ProjectId = @ProjId";

                using (SqlCommand cmd = new SqlCommand(updateQuery, sqlconnection))
                {
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@Desc", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@Inst", txtInstructions.Text);
                    cmd.Parameters.AddWithValue("@CatId", Convert.ToInt32(comboBox1.SelectedValue));

                    if (string.IsNullOrEmpty(_currentPhotoPath))
                        cmd.Parameters.AddWithValue("@Photo", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Photo", _currentPhotoPath);

                    cmd.Parameters.AddWithValue("@ProjId", _projectId);
                    cmd.ExecuteNonQuery();
                }

                string deleteOldLinks = "DELETE FROM ProjectMaterials WHERE ProjectId = @ProjId";
                using (SqlCommand cmdDelete = new SqlCommand(deleteOldLinks, sqlconnection))
                {
                    cmdDelete.Parameters.AddWithValue("@ProjId", _projectId);
                    cmdDelete.ExecuteNonQuery();
                }

                string insertMaterialQuery = @"
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
                        using (SqlCommand cmdAdd = new SqlCommand(insertMaterialQuery, sqlconnection))
                        {
                            cmdAdd.Parameters.AddWithValue("@MatName", materialName);
                            cmdAdd.Parameters.AddWithValue("@ProjId", _projectId);
                            cmdAdd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Project successfully updated!");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _currentPhotoPath = openFileDialog.FileName;


                label6.Text = _currentPhotoPath;
            }
        }
    }
}
