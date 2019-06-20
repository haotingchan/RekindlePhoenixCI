namespace PhoenixCI.FormUI.Prefix4 {
    partial class W40022 {
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
         this.chk_40023_data = new System.Windows.Forms.CheckBox();
         this.groupAdmin = new System.Windows.Forms.GroupBox();
         this.chkTxt = new System.Windows.Forms.CheckBox();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.label3 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.groupAdmin.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Controls.Add(this.groupAdmin);
         this.panParent.Size = new System.Drawing.Size(595, 405);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(595, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // chk_40023_data
         // 
         this.chk_40023_data.AutoSize = true;
         this.chk_40023_data.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.chk_40023_data.ForeColor = System.Drawing.Color.Black;
         this.chk_40023_data.Location = new System.Drawing.Point(45, 90);
         this.chk_40023_data.Name = "chk_40023_data";
         this.chk_40023_data.Size = new System.Drawing.Size(215, 22);
         this.chk_40023_data.TabIndex = 78;
         this.chk_40023_data.Text = "產出開盤參考價 && 收盤價資料";
         this.chk_40023_data.UseVisualStyleBackColor = true;
         this.chk_40023_data.Visible = false;
         // 
         // groupAdmin
         // 
         this.groupAdmin.Controls.Add(this.chkTxt);
         this.groupAdmin.Location = new System.Drawing.Point(446, 45);
         this.groupAdmin.Name = "groupAdmin";
         this.groupAdmin.Size = new System.Drawing.Size(130, 70);
         this.groupAdmin.TabIndex = 80;
         this.groupAdmin.TabStop = false;
         this.groupAdmin.Text = "管理者測試";
         this.groupAdmin.Visible = false;
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
         this.chkTxt.Visible = false;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Location = new System.Drawing.Point(30, 30);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(400, 220);
         this.r_frame.TabIndex = 82;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 170);
         this.labMsg.MaximumSize = new System.Drawing.Size(360, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 80;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.chk_40023_data);
         this.panFilter.Controls.Add(this.txtDate);
         this.panFilter.Controls.Add(this.label3);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(360, 145);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12/01";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(92, 42);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(100, 26);
         this.txtDate.TabIndex = 77;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.ForeColor = System.Drawing.Color.Black;
         this.label3.Location = new System.Drawing.Point(37, 45);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(58, 21);
         this.label3.TabIndex = 2;
         this.label3.Text = "日期：";
         // 
         // W40022
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(595, 435);
         this.Name = "W40022";
         this.Text = "W40022";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.groupAdmin.ResumeLayout(false);
         this.groupAdmin.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
      private System.Windows.Forms.GroupBox groupAdmin;
      private System.Windows.Forms.CheckBox chkTxt;
      private System.Windows.Forms.CheckBox chk_40023_data;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private BaseGround.Widget.TextDateEdit txtDate;
      private System.Windows.Forms.Label label3;
   }
}