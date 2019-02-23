namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0990
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
            this.lblUserIdDesc = new System.Windows.Forms.Label();
            this.lblOrgPassword = new System.Windows.Forms.Label();
            this.btnChange = new System.Windows.Forms.Button();
            this.txtOrgPassword = new DevExpress.XtraEditors.TextEdit();
            this.lblUserId = new System.Windows.Forms.Label();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new DevExpress.XtraEditors.TextEdit();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.lblUserNameDesc = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.grpxUserInfo = new System.Windows.Forms.GroupBox();
            this.grpxPasswordSetting = new System.Windows.Forms.GroupBox();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrgPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmPassword.Properties)).BeginInit();
            this.grpxUserInfo.SuspendLayout();
            this.grpxPasswordSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelParent
            // 
            this.panParent.Controls.Add(this.grpxPasswordSetting);
            this.panParent.Controls.Add(this.grpxUserInfo);
            this.panParent.Controls.Add(this.lblNote);
            this.panParent.Location = new System.Drawing.Point(0, 32);
            this.panParent.Size = new System.Drawing.Size(394, 382);
            // 
            // lblUserIdDesc
            // 
            this.lblUserIdDesc.AutoSize = true;
            this.lblUserIdDesc.Location = new System.Drawing.Point(40, 25);
            this.lblUserIdDesc.Name = "lblUserIdDesc";
            this.lblUserIdDesc.Size = new System.Drawing.Size(105, 20);
            this.lblUserIdDesc.TabIndex = 3;
            this.lblUserIdDesc.Text = "使用者代號：";
            // 
            // lblOrgPassword
            // 
            this.lblOrgPassword.AutoSize = true;
            this.lblOrgPassword.Location = new System.Drawing.Point(40, 109);
            this.lblOrgPassword.Name = "lblOrgPassword";
            this.lblOrgPassword.Size = new System.Drawing.Size(105, 20);
            this.lblOrgPassword.TabIndex = 0;
            this.lblOrgPassword.Text = "原始舊密碼：";
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(266, 103);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 30);
            this.btnChange.TabIndex = 11;
            this.btnChange.Text = "變更";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // textEditOrgPassword
            // 
            this.txtOrgPassword.EditValue = "";
            this.txtOrgPassword.Location = new System.Drawing.Point(151, 106);
            this.txtOrgPassword.Name = "textEditOrgPassword";
            this.txtOrgPassword.Properties.MaxLength = 10;
            this.txtOrgPassword.Properties.UseSystemPasswordChar = true;
            this.txtOrgPassword.Size = new System.Drawing.Size(190, 26);
            this.txtOrgPassword.TabIndex = 8;
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(151, 25);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(0, 20);
            this.lblUserId.TabIndex = 9;
            // 
            // textEditPassword
            // 
            this.txtPassword.EditValue = "";
            this.txtPassword.Location = new System.Drawing.Point(151, 25);
            this.txtPassword.Name = "textEditPassword";
            this.txtPassword.Properties.MaxLength = 10;
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(190, 26);
            this.txtPassword.TabIndex = 9;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(24, 28);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(121, 20);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "變更後新密碼：";
            // 
            // textEditConfirmPassword
            // 
            this.txtConfirmPassword.EditValue = "";
            this.txtConfirmPassword.Location = new System.Drawing.Point(151, 67);
            this.txtConfirmPassword.Name = "textEditConfirmPassword";
            this.txtConfirmPassword.Properties.MaxLength = 10;
            this.txtConfirmPassword.Properties.UseSystemPasswordChar = true;
            this.txtConfirmPassword.Size = new System.Drawing.Size(190, 26);
            this.txtConfirmPassword.TabIndex = 10;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Location = new System.Drawing.Point(8, 70);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(137, 20);
            this.lblConfirmPassword.TabIndex = 12;
            this.lblConfirmPassword.Text = "再次確認新密碼：";
            // 
            // lblUserNameDesc
            // 
            this.lblUserNameDesc.AutoSize = true;
            this.lblUserNameDesc.Location = new System.Drawing.Point(40, 67);
            this.lblUserNameDesc.Name = "lblUserNameDesc";
            this.lblUserNameDesc.Size = new System.Drawing.Size(105, 20);
            this.lblUserNameDesc.TabIndex = 14;
            this.lblUserNameDesc.Text = "使用者名稱：";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(151, 67);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(0, 20);
            this.lblUserName.TabIndex = 15;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(12, 328);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(332, 40);
            this.lblNote.TabIndex = 16;
            this.lblNote.Text = "註：(1)密碼長度必須8個字元以上\r\n　　(2)必須包含英文大寫、英文小寫以及數字";
            // 
            // groupBoxUserInfo
            // 
            this.grpxUserInfo.Controls.Add(this.lblUserIdDesc);
            this.grpxUserInfo.Controls.Add(this.lblUserNameDesc);
            this.grpxUserInfo.Controls.Add(this.lblUserName);
            this.grpxUserInfo.Controls.Add(this.lblUserId);
            this.grpxUserInfo.Controls.Add(this.lblOrgPassword);
            this.grpxUserInfo.Controls.Add(this.txtOrgPassword);
            this.grpxUserInfo.Location = new System.Drawing.Point(16, 6);
            this.grpxUserInfo.Name = "groupBoxUserInfo";
            this.grpxUserInfo.Size = new System.Drawing.Size(355, 154);
            this.grpxUserInfo.TabIndex = 17;
            this.grpxUserInfo.TabStop = false;
            // 
            // groupBoxPasswordSetting
            // 
            this.grpxPasswordSetting.Controls.Add(this.lblPassword);
            this.grpxPasswordSetting.Controls.Add(this.btnChange);
            this.grpxPasswordSetting.Controls.Add(this.txtPassword);
            this.grpxPasswordSetting.Controls.Add(this.txtConfirmPassword);
            this.grpxPasswordSetting.Controls.Add(this.lblConfirmPassword);
            this.grpxPasswordSetting.Location = new System.Drawing.Point(16, 166);
            this.grpxPasswordSetting.Name = "groupBoxPasswordSetting";
            this.grpxPasswordSetting.Size = new System.Drawing.Size(355, 147);
            this.grpxPasswordSetting.TabIndex = 18;
            this.grpxPasswordSetting.TabStop = false;
            // 
            // WZ0990
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 414);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WZ0990";
            this.Text = "WZ0990";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrgPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmPassword.Properties)).EndInit();
            this.grpxUserInfo.ResumeLayout(false);
            this.grpxUserInfo.PerformLayout();
            this.grpxPasswordSetting.ResumeLayout(false);
            this.grpxPasswordSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUserIdDesc;
        private System.Windows.Forms.Label lblOrgPassword;
        private System.Windows.Forms.Button btnChange;
        private DevExpress.XtraEditors.TextEdit txtOrgPassword;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserNameDesc;
        private DevExpress.XtraEditors.TextEdit txtConfirmPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.GroupBox grpxUserInfo;
        private System.Windows.Forms.GroupBox grpxPasswordSetting;
    }
}