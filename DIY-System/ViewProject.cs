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
    public partial class ViewProject : Form
    {
        private int _projectId;

        public ViewProject(int passedInProjectId)
        {
            InitializeComponent();

            _projectId = passedInProjectId;
        }

        private void ViewProject_Load(object sender, EventArgs e)
        {
            LoadProjectDetails();
        }

        private void LoadProjectDetails()
        {
            string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivo\source\repos\DIY-System\DIY-System\DIY.mdf;Integrated Security=True";

            using (SqlConnection sqlconnection = new SqlConnection(cs))
            {
                sqlconnection.Open();

                string projectQuery = "SELECT Title, Description, Instructions, PhotoPath FROM Projects WHERE ProjectId = @ProjId";
                using (SqlCommand cmd = new SqlCommand(projectQuery, sqlconnection))
                {
                    cmd.Parameters.AddWithValue("@ProjId", _projectId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label1.Text = reader["Title"].ToString();
                            richTextBox2.Text = "DESCRIPTION:\r\n" + reader["Description"].ToString() + "\r\n\r\nINSTRUCTIONS:\r\n" + reader["Instructions"].ToString();

                            string photoPath = reader["PhotoPath"].ToString();
                            if (!string.IsNullOrEmpty(photoPath) && System.IO.File.Exists(photoPath))
                            {
                                pictureBox1.ImageLocation = photoPath;
                                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                        }
                    }
                }

                string materialQuery = @"SELECT m.Name 
                                 FROM Materials m 
                                 INNER JOIN ProjectMaterials pm ON m.MaterialId = pm.MaterialId 
                                 WHERE pm.ProjectId = @ProjId";

                using (SqlCommand cmdMat = new SqlCommand(materialQuery, sqlconnection))
                {
                    cmdMat.Parameters.AddWithValue("@ProjId", _projectId);
                    using (SqlDataReader reader = cmdMat.ExecuteReader())
                    {
                        listBox1.Items.Clear();
                        while (reader.Read())
                        {
                            listBox1.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewProject_Load_1(object sender, EventArgs e)
        {
            LoadProjectDetails();
        }
    }
}
