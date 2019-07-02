namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30610 {
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
         this.label2 = new System.Windows.Forms.Label();
         this.gbStatistics = new DevExpress.XtraEditors.RadioGroup();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.txtStartDate = new BaseGround.Widget.TextDateEdit();
         this.txtEndMonth = new BaseGround.Widget.TextDateEdit();
         this.txtStartMonth = new BaseGround.Widget.TextDateEdit();
         this.label3 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbStatistics.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(550, 290);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(550, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(278, 89);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(26, 21);
         this.label2.TabIndex = 12;
         this.label2.Text = "～";
         // 
         // gbStatistics
         // 
         this.gbStatistics.EditValue = "rbMon";
         this.gbStatistics.Location = new System.Drawing.Point(37, 28);
         this.gbStatistics.Name = "gbStatistics";
         this.gbStatistics.Properties.AllowFocused = false;
         this.gbStatistics.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbStatistics.Properties.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.gbStatistics.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.gbStatistics.Properties.Appearance.Options.UseBackColor = true;
         this.gbStatistics.Properties.Appearance.Options.UseFont = true;
         this.gbStatistics.Properties.Appearance.Options.UseForeColor = true;
         this.gbStatistics.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.gbStatistics.Properties.Columns = 1;
         this.gbStatistics.Properties.EnableFocusRect = true;
         this.gbStatistics.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMon", "月統計 – 月份："),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbDay", "日明細 – 日期：")});
         this.gbStatistics.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.gbStatistics.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbStatistics.Size = new System.Drawing.Size(133, 99);
         this.gbStatistics.TabIndex = 0;
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
         this.r_frame.Size = new System.Drawing.Size(500, 230);
         this.r_frame.TabIndex = 80;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 180);
         this.labMsg.MaximumSize = new System.Drawing.Size(460, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 81;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Controls.Add(this.txtEndDate);
         this.panFilter.Controls.Add(this.txtStartDate);
         this.panFilter.Controls.Add(this.txtEndMonth);
         this.panFilter.Controls.Add(this.txtStartMonth);
         this.panFilter.Controls.Add(this.label3);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.gbStatistics);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(460, 155);
         this.panFilter.TabIndex = 80;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate.EditValue = "2018/12/01";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(304, 86);
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
         this.txtEndDate.TabIndex = 4;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartDate.EditValue = "2018/12/01";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(176, 86);
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
         this.txtStartDate.TabIndex = 3;
         this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtEndMonth
         // 
         this.txtEndMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndMonth.EditValue = "2018/12";
         this.txtEndMonth.EnterMoveNextControl = true;
         this.txtEndMonth.Location = new System.Drawing.Point(304, 39);
         this.txtEndMonth.MenuManager = this.ribbonControl;
         this.txtEndMonth.Name = "txtEndMonth";
         this.txtEndMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndMonth.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtEndMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtEndMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEndMonth.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndMonth.Size = new System.Drawing.Size(100, 26);
         this.txtEndMonth.TabIndex = 2;
         this.txtEndMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartMonth
         // 
         this.txtStartMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartMonth.EditValue = "2018/12";
         this.txtStartMonth.EnterMoveNextControl = true;
         this.txtStartMonth.Location = new System.Drawing.Point(176, 39);
         this.txtStartMonth.MenuManager = this.ribbonControl;
         this.txtStartMonth.Name = "txtStartMonth";
         this.txtStartMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartMonth.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtStartMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtStartMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtStartMonth.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartMonth.Size = new System.Drawing.Size(100, 26);
         this.txtStartMonth.TabIndex = 1;
         this.txtStartMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(278, 42);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(26, 21);
         this.label3.TabIndex = 79;
         this.label3.Text = "～";
         // 
         // W30610
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(550, 320);
         this.Name = "W30610";
         this.Text = "W30610";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbStatistics.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
      private System.Windows.Forms.Label label2;
      protected DevExpress.XtraEditors.RadioGroup gbStatistics;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.GroupBox panFilter;
      private BaseGround.Widget.TextDateEdit txtEndMonth;
      private BaseGround.Widget.TextDateEdit txtStartMonth;
      private System.Windows.Forms.Label label3;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
      private System.Windows.Forms.Label labMsg;
   }
}