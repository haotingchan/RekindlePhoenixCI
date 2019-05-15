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
            this.lblNote = new System.Windows.Forms.Label();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.grpxPasswordSetting = new System.Windows.Forms.GroupBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnChange = new System.Windows.Forms.Button();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.txtConfirmPassword = new DevExpress.XtraEditors.TextEdit();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.grpxUserInfo = new System.Windows.Forms.GroupBox();
            this.lblUserIdDesc = new System.Windows.Forms.Label();
            this.lblUserNameDesc = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.lblOrgPassword = new System.Windows.Forms.Label();
            this.txtOrgPassword = new DevExpress.XtraEditors.TextEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            this.grpxPasswordSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmPassword.Properties)).BeginInit();
            this.grpxUserInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrgPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.panelControl);
            this.panParent.Controls.Add(this.lblNote);
            this.panParent.Size = new System.Drawing.Size(439, 435);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(439, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(12, 381);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(332, 40);
            this.lblNote.TabIndex = 16;
            this.lblNote.Text = "註：(1)密碼長度必須8個字元以上\r\n　　(2)必須包含英文大寫、英文小寫以及數字";
            // 
            // panelControl
            // 
            this.panelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panelControl.Appearance.Options.UseBackColor = true;
            this.panelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl.Controls.Add(this.grpxPasswordSetting);
            this.panelControl.Controls.Add(this.grpxUserInfo);
            this.panelControl.Location = new System.Drawing.Point(16, 18);
            this.panelControl.Margin = new System.Windows.Forms.Padding(15);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(407, 348);
            this.panelControl.TabIndex = 19;
            // 
            // grpxPasswordSetting
            // 
            this.grpxPasswordSetting.Controls.Add(this.lblPassword);
            this.grpxPasswordSetting.Controls.Add(this.btnChange);
            this.grpxPasswordSetting.Controls.Add(this.txtPassword);
            this.grpxPasswordSetting.Controls.Add(this.txtConfirmPassword);
            this.grpxPasswordSetting.Controls.Add(this.lblConfirmPassword);
            this.grpxPasswordSetting.Location = new System.Drawing.Point(25, 170);
            this.grpxPasswordSetting.Name = "grpxPasswordSetting";
            this.grpxPasswordSetting.Size = new System.Drawing.Size(355, 147);
            this.grpxPasswordSetting.TabIndex = 20;
            this.grpxPasswordSetting.TabStop = false;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblPassword.Location = new System.Drawing.Point(24, 28);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(122, 21);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "變更後新密碼：";
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
            // txtPassword
            // 
            this.txtPassword.EditValue = "";
            this.txtPassword.Location = new System.Drawing.Point(151, 25);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.MaxLength = 10;
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(190, 26);
            this.txtPassword.TabIndex = 9;
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.EditValue = "";
            this.txtConfirmPassword.Location = new System.Drawing.Point(151, 67);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Properties.MaxLength = 10;
            this.txtConfirmPassword.Properties.UseSystemPasswordChar = true;
            this.txtConfirmPassword.Size = new System.Drawing.Size(190, 26);
            this.txtConfirmPassword.TabIndex = 10;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblConfirmPassword.Location = new System.Drawing.Point(8, 70);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(138, 21);
            this.lblConfirmPassword.TabIndex = 12;
            this.lblConfirmPassword.Text = "再次確認新密碼：";
            // 
            // grpxUserInfo
            // 
            this.grpxUserInfo.Controls.Add(this.lblUserIdDesc);
            this.grpxUserInfo.Controls.Add(this.lblUserNameDesc);
            this.grpxUserInfo.Controls.Add(this.lblUserName);
            this.grpxUserInfo.Controls.Add(this.lblUserId);
            this.grpxUserInfo.Controls.Add(this.lblOrgPassword);
            this.grpxUserInfo.Controls.Add(this.txtOrgPassword);
            this.grpxUserInfo.Location = new System.Drawing.Point(25, 15);
            this.grpxUserInfo.Name = "grpxUserInfo";
            this.grpxUserInfo.Size = new System.Drawing.Size(355, 154);
            this.grpxUserInfo.TabIndex = 19;
            this.grpxUserInfo.TabStop = false;
            // 
            // lblUserIdDesc
            // 
            this.lblUserIdDesc.AutoSize = true;
            this.lblUserIdDesc.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblUserIdDesc.Location = new System.Drawing.Point(40, 25);
            this.lblUserIdDesc.Name = "lblUserIdDesc";
            this.lblUserIdDesc.Size = new System.Drawing.Size(106, 21);
            this.lblUserIdDesc.TabIndex = 3;
            this.lblUserIdDesc.Text = "使用者代號：";
            // 
            // lblUserNameDesc
            // 
            this.lblUserNameDesc.AutoSize = true;
            this.lblUserNameDesc.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblUserNameDesc.Location = new System.Drawing.Point(40, 67);
            this.lblUserNameDesc.Name = "lblUserNameDesc";
            this.lblUserNameDesc.Size = new System.Drawing.Size(106, 21);
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
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(151, 25);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(0, 20);
            this.lblUserId.TabIndex = 9;
            // 
            // lblOrgPassword
            // 
            this.lblOrgPassword.AutoSize = true;
            this.lblOrgPassword.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblOrgPassword.Location = new System.Drawing.Point(40, 109);
            this.lblOrgPassword.Name = "lblOrgPassword";
            this.lblOrgPassword.Size = new System.Drawing.Size(106, 21);
            this.lblOrgPassword.TabIndex = 0;
            this.lblOrgPassword.Text = "原始舊密碼：";
            // 
            // txtOrgPassword
            // 
            this.txtOrgPassword.EditValue = "";
            this.txtOrgPassword.Location = new System.Drawing.Point(151, 106);
            this.txtOrgPassword.Name = "txtOrgPassword";
            this.txtOrgPassword.Properties.MaxLength = 10;
            this.txtOrgPassword.Properties.UseSystemPasswordChar = true;
            this.txtOrgPassword.Size = new System.Drawing.Size(190, 26);
            this.txtOrgPassword.TabIndex = 8;
            // 
            // WZ0990
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 465);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WZ0990";
            this.Text = "WZ0990";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.grpxPasswordSetting.ResumeLayout(false);
            this.grpxPasswordSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmPassword.Properties)).EndInit();
            this.grpxUserInfo.ResumeLayout(false);
            this.grpxUserInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrgPassword.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblNote;
        private DevExpress.XtraEditors.PanelControl panelControl;
        private System.Windows.Forms.GroupBox grpxPasswordSetting;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnChange;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.TextEdit txtConfirmPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.GroupBox grpxUserInfo;
        private System.Windows.Forms.Label lblUserIdDesc;
        private System.Windows.Forms.Label lblUserNameDesc;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.Label lblOrgPassword;
        private DevExpress.XtraEditors.TextEdit txtOrgPassword;
    }
}