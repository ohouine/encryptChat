namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSend = new Button();
            rtbMessage = new RichTextBox();
            txtIp = new TextBox();
            lblIp = new Label();
            lblMessage = new Label();
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.Location = new Point(362, 101);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(75, 23);
            btnSend.TabIndex = 0;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += button1_Click;
            // 
            // rtbMessage
            // 
            rtbMessage.Location = new Point(135, 52);
            rtbMessage.Name = "rtbMessage";
            rtbMessage.Size = new Size(100, 96);
            rtbMessage.TabIndex = 1;
            rtbMessage.Text = "";
            // 
            // txtIp
            // 
            txtIp.Location = new Point(348, 52);
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(100, 23);
            txtIp.TabIndex = 2;
            // 
            // lblIp
            // 
            lblIp.AutoSize = true;
            lblIp.Location = new Point(348, 28);
            lblIp.Name = "lblIp";
            lblIp.Size = new Size(68, 15);
            lblIp.TabIndex = 3;
            lblIp.Text = "Addresse Ip";
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(139, 25);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(53, 15);
            lblMessage.TabIndex = 4;
            lblMessage.Text = "Message";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblMessage);
            Controls.Add(lblIp);
            Controls.Add(txtIp);
            Controls.Add(rtbMessage);
            Controls.Add(btnSend);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private RichTextBox rtbMessage;
        private TextBox txtIp;
        private Label lblIp;
        private Label lblMessage;
    }
}