
namespace SANHUA_MAIN
{
    partial class Form_add_product
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Btn_Add = new System.Windows.Forms.Button();
            this.Btn_Del = new System.Windows.Forms.Button();
            this.Btn_Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(33, 32);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(712, 199);
            this.listBox1.TabIndex = 0;
            // 
            // Btn_Add
            // 
            this.Btn_Add.Location = new System.Drawing.Point(74, 284);
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(75, 23);
            this.Btn_Add.TabIndex = 1;
            this.Btn_Add.Text = "添加";
            this.Btn_Add.UseVisualStyleBackColor = true;
            // 
            // Btn_Del
            // 
            this.Btn_Del.Location = new System.Drawing.Point(255, 284);
            this.Btn_Del.Name = "Btn_Del";
            this.Btn_Del.Size = new System.Drawing.Size(75, 23);
            this.Btn_Del.TabIndex = 2;
            this.Btn_Del.Text = "删除";
            this.Btn_Del.UseVisualStyleBackColor = true;
            // 
            // Btn_Save
            // 
            this.Btn_Save.Location = new System.Drawing.Point(441, 284);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(75, 23);
            this.Btn_Save.TabIndex = 3;
            this.Btn_Save.Text = "保存";
            this.Btn_Save.UseVisualStyleBackColor = true;
            // 
            // Form_add_product
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Btn_Save);
            this.Controls.Add(this.Btn_Del);
            this.Controls.Add(this.Btn_Add);
            this.Controls.Add(this.listBox1);
            this.Name = "Form_add_product";
            this.Text = "Form_add_product";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Btn_Add;
        private System.Windows.Forms.Button Btn_Del;
        private System.Windows.Forms.Button Btn_Save;
    }
}