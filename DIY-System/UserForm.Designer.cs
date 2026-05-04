namespace DIY_System
{
    partial class UserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ProjectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instructionsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photoPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.projectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dIYDataSet = new DIY_System.DIYDataSet();
            this.projectsTableAdapter = new DIY_System.DIYDataSetTableAdapters.ProjectsTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dIYDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProjectId,
            this.titleDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.instructionsDataGridViewTextBoxColumn,
            this.photoPathDataGridViewTextBoxColumn,
            this.UserId,
            this.CategoryId});
            this.dataGridView1.DataSource = this.projectsBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 171);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1139, 352);
            this.dataGridView1.TabIndex = 0;
            // 
            // ProjectId
            // 
            this.ProjectId.DataPropertyName = "ProjectId";
            this.ProjectId.HeaderText = "ProjectId";
            this.ProjectId.MinimumWidth = 6;
            this.ProjectId.Name = "ProjectId";
            this.ProjectId.ReadOnly = true;
            this.ProjectId.Width = 125;
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            this.titleDataGridViewTextBoxColumn.Width = 125;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Width = 125;
            // 
            // instructionsDataGridViewTextBoxColumn
            // 
            this.instructionsDataGridViewTextBoxColumn.DataPropertyName = "Instructions";
            this.instructionsDataGridViewTextBoxColumn.HeaderText = "Instructions";
            this.instructionsDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.instructionsDataGridViewTextBoxColumn.Name = "instructionsDataGridViewTextBoxColumn";
            this.instructionsDataGridViewTextBoxColumn.Width = 125;
            // 
            // photoPathDataGridViewTextBoxColumn
            // 
            this.photoPathDataGridViewTextBoxColumn.DataPropertyName = "PhotoPath";
            this.photoPathDataGridViewTextBoxColumn.HeaderText = "PhotoPath";
            this.photoPathDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.photoPathDataGridViewTextBoxColumn.Name = "photoPathDataGridViewTextBoxColumn";
            this.photoPathDataGridViewTextBoxColumn.Width = 125;
            // 
            // UserId
            // 
            this.UserId.DataPropertyName = "Author";
            this.UserId.HeaderText = "Author";
            this.UserId.MinimumWidth = 6;
            this.UserId.Name = "UserId";
            this.UserId.Width = 125;
            // 
            // CategoryId
            // 
            this.CategoryId.DataPropertyName = "Category";
            this.CategoryId.HeaderText = "Category";
            this.CategoryId.MinimumWidth = 6;
            this.CategoryId.Name = "CategoryId";
            this.CategoryId.Width = 125;
            // 
            // projectsBindingSource
            // 
            this.projectsBindingSource.DataMember = "Projects";
            this.projectsBindingSource.DataSource = this.dIYDataSet;
            // 
            // dIYDataSet
            // 
            this.dIYDataSet.DataSetName = "DIYDataSet";
            this.dIYDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // projectsTableAdapter
            // 
            this.projectsTableAdapter.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(334, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "Create Project";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 34);
            this.button2.TabIndex = 2;
            this.button2.Text = "My Projects";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(173, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(155, 34);
            this.button3.TabIndex = 3;
            this.button3.Text = "All projects";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(495, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(155, 34);
            this.button4.TabIndex = 4;
            this.button4.Text = "Delete Project";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(656, 12);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(155, 34);
            this.button5.TabIndex = 5;
            this.button5.Text = "View Project";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1012, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 6;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(996, 42);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(155, 34);
            this.button6.TabIndex = 7;
            this.button6.Text = "Search Project";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(978, 122);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(155, 34);
            this.button7.TabIndex = 8;
            this.button7.Text = "Admin Menu";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(817, 12);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(155, 34);
            this.button8.TabIndex = 9;
            this.button8.Text = "Edit Project";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 535);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "UserForm";
            this.Text = "UserForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserForm_FormClosed);
            this.Load += new System.EventHandler(this.UserForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dIYDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private DIYDataSet dIYDataSet;
        private System.Windows.Forms.BindingSource projectsBindingSource;
        private DIYDataSetTableAdapters.ProjectsTableAdapter projectsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn instructionsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photoPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryId;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
    }
}