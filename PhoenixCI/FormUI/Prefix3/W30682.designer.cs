namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30682 {
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
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.gbType = new DevExpress.XtraEditors.RadioGroup();
         this.label9 = new System.Windows.Forms.Label();
         this.gbReport = new DevExpress.XtraEditors.RadioGroup();
         this.txtStartDate = new BaseGround.Widget.TextDateEdit();
         this.label6 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.label14 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbReport.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(480, 325);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(480, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
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
         this.r_frame.Size = new System.Drawing.Size(430, 265);
         this.r_frame.TabIndex = 92;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 215);
         this.labMsg.MaximumSize = new System.Drawing.Size(390, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 82;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.gbType);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Controls.Add(this.gbReport);
         this.panFilter.Controls.Add(this.txtStartDate);
         this.panFilter.Controls.Add(this.label6);
         this.panFilter.Controls.Add(this.label7);
         this.panFilter.Controls.Add(this.txtEndDate);
         this.panFilter.Controls.Add(this.label14);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(390, 190);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // gbType
         // 
         this.gbType.EditValue = "rbHistory";
         this.gbType.Location = new System.Drawing.Point(122, 82);
         this.gbType.Name = "gbType";
         this.gbType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbType.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.gbType.Properties.Appearance.Options.UseBackColor = true;
         this.gbType.Properties.Appearance.Options.UseForeColor = true;
         this.gbType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.gbType.Properties.Columns = 3;
         this.gbType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbHistory", "歷史"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbInstant", "瞬時")});
         this.gbType.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.gbType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbType.Size = new System.Drawing.Size(170, 35);
         this.gbType.TabIndex = 2;
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.ForeColor = System.Drawing.Color.Black;
         this.label9.Location = new System.Drawing.Point(37, 90);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(90, 21);
         this.label9.TabIndex = 102;
         this.label9.Text = "資料種類：";
         // 
         // gbReport
         // 
         this.gbReport.EditValue = "rbStatistics";
         this.gbReport.Location = new System.Drawing.Point(122, 127);
         this.gbReport.Name = "gbReport";
         this.gbReport.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbReport.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.gbReport.Properties.Appearance.Options.UseBackColor = true;
         this.gbReport.Properties.Appearance.Options.UseForeColor = true;
         this.gbReport.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.gbReport.Properties.Columns = 3;
         this.gbReport.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbStatistics", "統計"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbDetail", "明細")});
         this.gbReport.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.gbReport.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbReport.Size = new System.Drawing.Size(170, 35);
         this.gbReport.TabIndex = 3;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartDate.EditValue = "2018/12/01";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(121, 42);
         this.txtStartDate.MenuManager = this.ribbonControl;
         this.txtStartDate.Name = "txtStartDate";
         this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtStartDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtStartDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartDate.Size = new System.Drawing.Size(100, 26);
         this.txtStartDate.TabIndex = 0;
         this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.ForeColor = System.Drawing.Color.Black;
         this.label6.Location = new System.Drawing.Point(37, 45);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(58, 21);
         this.label6.TabIndex = 8;
         this.label6.Text = "日期：";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.ForeColor = System.Drawing.Color.Black;
         this.label7.Location = new System.Drawing.Point(37, 135);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(90, 21);
         this.label7.TabIndex = 7;
         this.label7.Text = "報表種類：";
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate.EditValue = "2018/12/01";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(249, 42);
         this.txtEndDate.MenuManager = this.ribbonControl;
         this.txtEndDate.Name = "txtEndDate";
         this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEndDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate.TabIndex = 1;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(223, 45);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(26, 21);
         this.label14.TabIndex = 6;
         this.label14.Text = "～";
         // 
         // W30682
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(480, 355);
         this.Name = "W30682";
         this.Text = "W30682";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbReport.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

      #endregion

      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      protected DevExpress.XtraEditors.RadioGroup gbType;
      private System.Windows.Forms.Label label9;
      protected DevExpress.XtraEditors.RadioGroup gbReport;
      private BaseGround.Widget.TextDateEdit txtStartDate;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label7;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private System.Windows.Forms.Label label14;
   }
}