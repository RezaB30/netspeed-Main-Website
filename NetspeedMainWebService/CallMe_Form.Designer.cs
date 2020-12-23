
namespace NetspeedMainWebService
{
    partial class CallMe_Form
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
            this.CallMeFullName_TextBox = new System.Windows.Forms.TextBox();
            this.CallMePhoneNumber_TextBox = new System.Windows.Forms.TextBox();
            this.SendCallMe_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CallMeFullName_TextBox
            // 
            this.CallMeFullName_TextBox.Location = new System.Drawing.Point(111, 74);
            this.CallMeFullName_TextBox.Name = "CallMeFullName_TextBox";
            this.CallMeFullName_TextBox.Size = new System.Drawing.Size(248, 20);
            this.CallMeFullName_TextBox.TabIndex = 0;
            // 
            // CallMePhoneNumber_TextBox
            // 
            this.CallMePhoneNumber_TextBox.Location = new System.Drawing.Point(111, 162);
            this.CallMePhoneNumber_TextBox.Name = "CallMePhoneNumber_TextBox";
            this.CallMePhoneNumber_TextBox.Size = new System.Drawing.Size(248, 20);
            this.CallMePhoneNumber_TextBox.TabIndex = 1;
            // 
            // SendCallMe_Button
            // 
            this.SendCallMe_Button.Location = new System.Drawing.Point(279, 247);
            this.SendCallMe_Button.Name = "SendCallMe_Button";
            this.SendCallMe_Button.Size = new System.Drawing.Size(80, 23);
            this.SendCallMe_Button.TabIndex = 2;
            this.SendCallMe_Button.Text = "Call Me";
            this.SendCallMe_Button.UseVisualStyleBackColor = true;
            this.SendCallMe_Button.Click += new System.EventHandler(this.SendCallMe_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Location = new System.Drawing.Point(111, 247);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(84, 23);
            this.Cancel_Button.TabIndex = 3;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            // 
            // CallMe_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 450);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.SendCallMe_Button);
            this.Controls.Add(this.CallMePhoneNumber_TextBox);
            this.Controls.Add(this.CallMeFullName_TextBox);
            this.Name = "CallMe_Form";
            this.Text = "Call Me";
            this.Load += new System.EventHandler(this.CallMe_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CallMeFullName_TextBox;
        private System.Windows.Forms.TextBox CallMePhoneNumber_TextBox;
        private System.Windows.Forms.Button SendCallMe_Button;
        private System.Windows.Forms.Button Cancel_Button;
    }
}

