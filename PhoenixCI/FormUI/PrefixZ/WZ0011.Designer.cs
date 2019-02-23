namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0011
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
            this.lblUserId = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnSetting = new System.Windows.Forms.Button();
            this.cbxUserId = new System.Windows.Forms.ComboBox();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelParent
            // 
            this.panParent.Controls.Add(this.txtPassword);
            this.panParent.Controls.Add(this.cbxUserId);
            this.panParent.Controls.Add(this.btnSetting);
            this.panParent.Controls.Add(this.lblUserId);
            this.panParent.Controls.Add(this.lblPassword);
            this.panParent.Size = new System.Drawing.Size(386, 185);
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(44, 38);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(105, 20);
            this.lblUserId.TabIndex = 3;
            this.lblUserId.Text = "使用者代號：";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(12, 85);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(137, 20);
            this.lblPassword.TabIndex = 0;
            this.lblPassword.Text = "起始設定後密碼：";
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(288, 133);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(75, 30);
            this.btnSetting.TabIndex = 7;
            this.btnSetting.Text = "設定";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // cbxUserId
            // 
            this.cbxUserId.BackColor = System.Drawing.Color.White;
            this.cbxUserId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUserId.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxUserId.ForeColor = System.Drawing.Color.Black;
            this.cbxUserId.FormattingEnabled = true;
            this.cbxUserId.Location = new System.Drawing.Point(158, 30);
            this.cbxUserId.Name = "cbxUserId";
            this.cbxUserId.Size = new System.Drawing.Size(205, 28);
            this.cbxUserId.TabIndex = 5;
            this.cbxUserId.SelectedIndexChanged += new System.EventHandler(this.cbxUserId_SelectedIndexChanged);
            // 
            // textEditPassword
            // 
            this.txtPassword.EditValue = "0000000000";
            this.txtPassword.Location = new System.Drawing.Point(158, 85);
            this.txtPassword.Name = "textEditPassword";
            this.txtPassword.Properties.MaxLength = 10;
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(205, 26);
            this.txtPassword.TabIndex = 8;
            // 
            // WZ0011
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 217);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WZ0011";
            this.Text = "WZ0011";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.ComboBox cbxUserId;
        private DevExpress.XtraEditors.TextEdit txtPassword;
    }
}