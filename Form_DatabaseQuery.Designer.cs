
namespace SANHUA_MAIN
{
    partial class Form_DatabaseQuery
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView_DatasInfo = new System.Windows.Forms.DataGridView();
            this.Btn_1_Hundred = new System.Windows.Forms.Button();
            this.Btn_2_Hundred = new System.Windows.Forms.Button();
            this.Btn_Query = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DatasInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView_DatasInfo);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1013, 462);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据信息显示";
            // 
            // dataGridView_DatasInfo
            // 
            this.dataGridView_DatasInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DatasInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_DatasInfo.Location = new System.Drawing.Point(3, 21);
            this.dataGridView_DatasInfo.Name = "dataGridView_DatasInfo";
            this.dataGridView_DatasInfo.RowHeadersWidth = 51;
            this.dataGridView_DatasInfo.RowTemplate.Height = 27;
            this.dataGridView_DatasInfo.Size = new System.Drawing.Size(1007, 438);
            this.dataGridView_DatasInfo.TabIndex = 0;
            // 
            // Btn_1_Hundred
            // 
            this.Btn_1_Hundred.Location = new System.Drawing.Point(47, 502);
            this.Btn_1_Hundred.Name = "Btn_1_Hundred";
            this.Btn_1_Hundred.Size = new System.Drawing.Size(96, 35);
            this.Btn_1_Hundred.TabIndex = 1;
            this.Btn_1_Hundred.Text = "100条显示";
            this.Btn_1_Hundred.UseVisualStyleBackColor = true;
            this.Btn_1_Hundred.Click += new System.EventHandler(this.Btn_1_Hundred_Click);
            // 
            // Btn_2_Hundred
            // 
            this.Btn_2_Hundred.Location = new System.Drawing.Point(164, 502);
            this.Btn_2_Hundred.Name = "Btn_2_Hundred";
            this.Btn_2_Hundred.Size = new System.Drawing.Size(104, 35);
            this.Btn_2_Hundred.TabIndex = 2;
            this.Btn_2_Hundred.Text = "200条显示";
            this.Btn_2_Hundred.UseVisualStyleBackColor = true;
            this.Btn_2_Hundred.Click += new System.EventHandler(this.Btn_2_Hundred_Click);
            // 
            // Btn_Query
            // 
            this.Btn_Query.Location = new System.Drawing.Point(881, 502);
            this.Btn_Query.Name = "Btn_Query";
            this.Btn_Query.Size = new System.Drawing.Size(87, 35);
            this.Btn_Query.TabIndex = 3;
            this.Btn_Query.Text = "查询";
            this.Btn_Query.UseVisualStyleBackColor = true;
            this.Btn_Query.Click += new System.EventHandler(this.Btn_Query_Click);
            // 
            // Form_DatabaseQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 600);
            this.Controls.Add(this.Btn_Query);
            this.Controls.Add(this.Btn_2_Hundred);
            this.Controls.Add(this.Btn_1_Hundred);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_DatabaseQuery";
            this.Text = "DatabaseQuery";
            this.Load += new System.EventHandler(this.Form_DatabaseQuery_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DatasInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView_DatasInfo;
        private System.Windows.Forms.Button Btn_1_Hundred;
        private System.Windows.Forms.Button Btn_2_Hundred;
        private System.Windows.Forms.Button Btn_Query;
    }
}