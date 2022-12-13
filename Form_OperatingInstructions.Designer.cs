
namespace SANHUA_MAIN
{
    partial class Form_OperatingInstructions
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
            this.richTextBox_Instructions = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox_Instructions
            // 
            this.richTextBox_Instructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Instructions.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_Instructions.Name = "richTextBox_Instructions";
            this.richTextBox_Instructions.Size = new System.Drawing.Size(1035, 682);
            this.richTextBox_Instructions.TabIndex = 0;
            this.richTextBox_Instructions.Text = "";
            // 
            // Form_OperatingInstructions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 682);
            this.Controls.Add(this.richTextBox_Instructions);
            this.Name = "Form_OperatingInstructions";
            this.Text = "Form_OperatingInstructions";
            this.Load += new System.EventHandler(this.Form_OperatingInstructions_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_Instructions;
    }
}