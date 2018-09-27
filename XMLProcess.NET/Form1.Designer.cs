namespace XMLProcess.NET
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.file_open = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_insert = new System.Windows.Forms.Button();
            this.btn_DeleteAll = new System.Windows.Forms.Button();
            this.txt_data1 = new System.Windows.Forms.TextBox();
            this.txt_data3 = new System.Windows.Forms.TextBox();
            this.btn_DeleteCheck = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_data4 = new System.Windows.Forms.TextBox();
            this.txt_data2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_path = new System.Windows.Forms.TextBox();
            this.btn_update = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_newXmlFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_fileName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_insertPath = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_insertType = new System.Windows.Forms.TextBox();
            this.txt_insertEN = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_insetCH = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.txt_insetCH);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txt_insertPath);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txt_insertType);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txt_insertEN);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(913, 444);
            this.panel1.TabIndex = 0;
            // 
            // file_open
            // 
            this.file_open.Location = new System.Drawing.Point(6, 20);
            this.file_open.Name = "file_open";
            this.file_open.Size = new System.Drawing.Size(76, 23);
            this.file_open.TabIndex = 2;
            this.file_open.Text = "打开文件";
            this.file_open.UseVisualStyleBackColor = true;
            this.file_open.Click += new System.EventHandler(this.file_open_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(362, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 421);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据区域";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_insert);
            this.panel2.Controls.Add(this.btn_DeleteAll);
            this.panel2.Controls.Add(this.txt_data1);
            this.panel2.Controls.Add(this.txt_data3);
            this.panel2.Controls.Add(this.btn_DeleteCheck);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txt_data4);
            this.panel2.Controls.Add(this.txt_data2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txt_path);
            this.panel2.Controls.Add(this.btn_update);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(6, 326);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(524, 89);
            this.panel2.TabIndex = 3;
            // 
            // btn_insert
            // 
            this.btn_insert.Location = new System.Drawing.Point(332, 32);
            this.btn_insert.Name = "btn_insert";
            this.btn_insert.Size = new System.Drawing.Size(75, 23);
            this.btn_insert.TabIndex = 4;
            this.btn_insert.Text = "添加";
            this.btn_insert.UseVisualStyleBackColor = true;
            this.btn_insert.Click += new System.EventHandler(this.btn_insert_Click);
            // 
            // btn_DeleteAll
            // 
            this.btn_DeleteAll.Location = new System.Drawing.Point(436, 4);
            this.btn_DeleteAll.Name = "btn_DeleteAll";
            this.btn_DeleteAll.Size = new System.Drawing.Size(75, 23);
            this.btn_DeleteAll.TabIndex = 4;
            this.btn_DeleteAll.Text = "删除所有";
            this.btn_DeleteAll.UseVisualStyleBackColor = true;
            this.btn_DeleteAll.Click += new System.EventHandler(this.btn_DeleteAll_Click);
            // 
            // txt_data1
            // 
            this.txt_data1.Location = new System.Drawing.Point(49, 4);
            this.txt_data1.Name = "txt_data1";
            this.txt_data1.ReadOnly = true;
            this.txt_data1.Size = new System.Drawing.Size(100, 21);
            this.txt_data1.TabIndex = 8;
            // 
            // txt_data3
            // 
            this.txt_data3.Location = new System.Drawing.Point(49, 34);
            this.txt_data3.Name = "txt_data3";
            this.txt_data3.Size = new System.Drawing.Size(100, 21);
            this.txt_data3.TabIndex = 9;
            // 
            // btn_DeleteCheck
            // 
            this.btn_DeleteCheck.Location = new System.Drawing.Point(332, 4);
            this.btn_DeleteCheck.Name = "btn_DeleteCheck";
            this.btn_DeleteCheck.Size = new System.Drawing.Size(75, 23);
            this.btn_DeleteCheck.TabIndex = 4;
            this.btn_DeleteCheck.Text = "删除选中";
            this.btn_DeleteCheck.UseVisualStyleBackColor = true;
            this.btn_DeleteCheck.Click += new System.EventHandler(this.btn_DeleteCheck_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "编号";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "中文名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "协议类型";
            // 
            // txt_data4
            // 
            this.txt_data4.Location = new System.Drawing.Point(215, 34);
            this.txt_data4.Name = "txt_data4";
            this.txt_data4.Size = new System.Drawing.Size(100, 21);
            this.txt_data4.TabIndex = 10;
            // 
            // txt_data2
            // 
            this.txt_data2.Location = new System.Drawing.Point(215, 4);
            this.txt_data2.Name = "txt_data2";
            this.txt_data2.Size = new System.Drawing.Size(100, 21);
            this.txt_data2.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(161, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "英文名";
            // 
            // txt_path
            // 
            this.txt_path.Location = new System.Drawing.Point(49, 65);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(462, 21);
            this.txt_path.TabIndex = 2;
            // 
            // btn_update
            // 
            this.btn_update.Location = new System.Drawing.Point(436, 32);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(75, 23);
            this.btn_update.TabIndex = 3;
            this.btn_update.Text = "修改";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "路径";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(524, 300);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            // 
            // btn_newXmlFile
            // 
            this.btn_newXmlFile.Location = new System.Drawing.Point(107, 20);
            this.btn_newXmlFile.Name = "btn_newXmlFile";
            this.btn_newXmlFile.Size = new System.Drawing.Size(75, 23);
            this.btn_newXmlFile.TabIndex = 3;
            this.btn_newXmlFile.Text = "新建";
            this.btn_newXmlFile.UseVisualStyleBackColor = true;
            this.btn_newXmlFile.Click += new System.EventHandler(this.btn_newXmlFile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_fileName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.file_open);
            this.groupBox2.Controls.Add(this.btn_newXmlFile);
            this.groupBox2.Location = new System.Drawing.Point(16, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(340, 56);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文件操作";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(188, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "文件名";
            // 
            // txt_fileName
            // 
            this.txt_fileName.Location = new System.Drawing.Point(234, 22);
            this.txt_fileName.Name = "txt_fileName";
            this.txt_fileName.Size = new System.Drawing.Size(100, 21);
            this.txt_fileName.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "路径";
            // 
            // txt_insertPath
            // 
            this.txt_insertPath.Location = new System.Drawing.Point(72, 147);
            this.txt_insertPath.Name = "txt_insertPath";
            this.txt_insertPath.Size = new System.Drawing.Size(266, 21);
            this.txt_insertPath.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(184, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "英文名";
            // 
            // txt_insertType
            // 
            this.txt_insertType.Location = new System.Drawing.Point(72, 89);
            this.txt_insertType.Name = "txt_insertType";
            this.txt_insertType.Size = new System.Drawing.Size(100, 21);
            this.txt_insertType.TabIndex = 11;
            // 
            // txt_insertEN
            // 
            this.txt_insertEN.Location = new System.Drawing.Point(238, 120);
            this.txt_insertEN.Name = "txt_insertEN";
            this.txt_insertEN.Size = new System.Drawing.Size(100, 21);
            this.txt_insertEN.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 6;
            this.label9.Text = "协议类型";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(25, 123);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "中文名";
            // 
            // txt_insetCH
            // 
            this.txt_insetCH.Location = new System.Drawing.Point(72, 120);
            this.txt_insetCH.Name = "txt_insetCH";
            this.txt_insetCH.Size = new System.Drawing.Size(100, 21);
            this.txt_insetCH.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 459);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_DeleteAll;
        private System.Windows.Forms.Button btn_DeleteCheck;
        private System.Windows.Forms.Button btn_insert;
        private System.Windows.Forms.Button file_open;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txt_data1;
        private System.Windows.Forms.TextBox txt_data3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_data4;
        private System.Windows.Forms.TextBox txt_data2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_path;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_fileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_newXmlFile;
        private System.Windows.Forms.TextBox txt_insetCH;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_insertPath;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_insertType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_insertEN;
    }
}

