namespace PhoenixCI.FormUI.Prefix4 {
    partial class W40021 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
         this.labMsg = new System.Windows.Forms.Label();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.chkTxt = new System.Windows.Forms.CheckBox();
         this.lblDate = new System.Windows.Forms.Label();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.groupAdmin = new System.Windows.Forms.GroupBox();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         this.panFilter.SuspendLayout();
         this.groupAdmin.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.groupAdmin);
         this.panParent.Controls.Add(this.panFilter);
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(601, 372);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(601, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 147);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Location = new System.Drawing.Point(15, 24);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(402, 175);
         this.r_frame.TabIndex = 78;
         // 
         // chkTxt
         // 
         this.chkTxt.AutoSize = true;
         this.chkTxt.Location = new System.Drawing.Point(19, 28);
         this.chkTxt.Name = "chkTxt";
         this.chkTxt.Size = new System.Drawing.Size(92, 24);
         this.chkTxt.TabIndex = 79;
         this.chkTxt.Text = "寫文字檔";
         this.chkTxt.UseVisualStyleBackColor = true;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblDate.ForeColor = System.Drawing.Color.Black;
         this.lblDate.Location = new System.Drawing.Point(37, 59);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(57, 20);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "日期：";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(100, 56);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(100, 26);
         this.txtDate.TabIndex = 77;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Controls.Add(this.txtDate);
         this.panFilter.Controls.Add(this.lblDate);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(34, 39);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(366, 126);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // groupAdmin
         // 
         this.groupAdmin.Controls.Add(this.chkTxt);
         this.groupAdmin.Location = new System.Drawing.Point(423, 72);
         this.groupAdmin.Name = "groupAdmin";
         this.groupAdmin.Size = new System.Drawing.Size(130, 70);
         this.groupAdmin.TabIndex = 80;
         this.groupAdmin.TabStop = false;
         this.groupAdmin.Text = "管理者測試";
         // 
         // W40021
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(601, 402);
         this.Name = "W40021";
         this.Text = "W40021";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         this.groupAdmin.ResumeLayout(false);
         this.groupAdmin.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labMsg;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.GroupBox groupAdmin;
      private System.Windows.Forms.CheckBox chkTxt;
      private System.Windows.Forms.GroupBox panFilter;
      private BaseGround.Widget.TextDateEdit txtDate;
      private System.Windows.Forms.Label lblDate;
   }
}