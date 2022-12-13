
namespace SANHUA_MAIN
{
    partial class ScanShield
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.Btn_OK = new System.Windows.Forms.Button();
            this.Btn_CANCEL = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入密码进行修改：";
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(162, 57);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(100, 21);
            this.textBox_password.TabIndex = 1;
            // 
            // Btn_OK
            // 
            this.Btn_OK.Location = new System.Drawing.Point(41, 122);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(75, 23);
            this.Btn_OK.TabIndex = 2;
            this.Btn_OK.Text = "确定";
            this.Btn_OK.UseVisualStyleBackColor = true;
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // Btn_CANCEL
            // 
            this.Btn_CANCEL.Location = new System.Drawing.Point(205, 122);
            this.Btn_CANCEL.Name = "Btn_CANCEL";
            this.Btn_CANCEL.Size = new System.Drawing.Size(75, 23);
            this.Btn_CANCEL.TabIndex = 3;
            this.Btn_CANCEL.Text = "取消";
            this.Btn_CANCEL.UseVisualStyleBackColor = true;
            this.Btn_CANCEL.Click += new System.EventHandler(this.Btn_CANCEL_Click);
            // 
            // ScanShield
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 196);
            this.Controls.Add(this.Btn_CANCEL);
            this.Controls.Add(this.Btn_OK);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.label1);
            this.Name = "ScanShield";
            this.Text = "ScanShield";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Button Btn_OK;
        private System.Windows.Forms.Button Btn_CANCEL;
    }
}