
namespace SANHUA_MAIN
{
    partial class Form_login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_login));
            this.label1 = new System.Windows.Forms.Label();
            this.tb_user = new System.Windows.Forms.TextBox();
            this.btn_login = new System.Windows.Forms.Button();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton_remember = new System.Windows.Forms.RadioButton();
            this.tb_staff = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 71);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "用 户：";
            // 
            // tb_user
            // 
            this.tb_user.Location = new System.Drawing.Point(201, 65);
            this.tb_user.Margin = new System.Windows.Forms.Padding(4);
            this.tb_user.Name = "tb_user";
            this.tb_user.Size = new System.Drawing.Size(132, 25);
            this.tb_user.TabIndex = 2;
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(201, 225);
            this.btn_login.Margin = new System.Windows.Forms.Padding(4);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(100, 29);
            this.btn_login.TabIndex = 1;
            this.btn_login.Text = "登录";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(201, 161);
            this.tb_password.Margin = new System.Windows.Forms.Padding(4);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(132, 25);
            this.tb_password.TabIndex = 4;
            this.tb_password.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 168);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "密 码：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(512, 39);
            this.panel1.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(47, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(389, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "压差法气密性检测 ---浙江先机智能科技";
            // 
            // radioButton_remember
            // 
            this.radioButton_remember.AutoSize = true;
            this.radioButton_remember.Location = new System.Drawing.Point(140, 196);
            this.radioButton_remember.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_remember.Name = "radioButton_remember";
            this.radioButton_remember.Size = new System.Drawing.Size(88, 19);
            this.radioButton_remember.TabIndex = 5;
            this.radioButton_remember.TabStop = true;
            this.radioButton_remember.Text = "记住登录";
            this.radioButton_remember.UseVisualStyleBackColor = true;
            // 
            // tb_staff
            // 
            this.tb_staff.Location = new System.Drawing.Point(201, 112);
            this.tb_staff.Margin = new System.Windows.Forms.Padding(4);
            this.tb_staff.Name = "tb_staff";
            this.tb_staff.Size = new System.Drawing.Size(132, 25);
            this.tb_staff.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(135, 119);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "员工号：";
            // 
            // Form_login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(512, 291);
            this.Controls.Add(this.tb_staff);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.radioButton_remember);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.tb_user);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_login";
            this.Text = "压差法气密性检测 ---浙江先机智能科技欢迎页";
            this.Load += new System.EventHandler(this.Form_login_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_login_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_user;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton_remember;
        private System.Windows.Forms.TextBox tb_staff;
        private System.Windows.Forms.Label label5;
    }
}