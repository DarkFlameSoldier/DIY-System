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
    }
}
