namespace PhoenixCI.FormUI.Prefix4 {
   partial class W40170 {
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
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.dwKindId = new DevExpress.XtraEditors.LookUpEdit();
         this.grpRbDate = new System.Windows.Forms.GroupBox();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.txtStartDate = new BaseGround.Widget.TextDateEdit();
         this.label11 = new System.Windows.Forms.Label();
         this.label12 = new System.Windows.Forms.Label();
         this.txtDay = new DevExpress.XtraEditors.TextEdit();
         this.label13 = new System.Windows.Forms.Label();
         this.gbItem = new DevExpress.XtraEditors.RadioGroup();
         this.label8 = new System.Windows.Forms.Label();
         this.labKind = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.grpChkModel = new System.Windows.Forms.GroupBox();
         this.chkModel = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).BeginInit();
         this.grpRbDate.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDay.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbItem.Properties)).BeginInit();
         this.grpChkModel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkModel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(555, 435);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(555, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 325);
         this.labMsg.MaximumSize = new System.Drawing.Size(465, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.Controls.Add(this.dwKindId);
         this.panFilter.Controls.Add(this.grpRbDate);
         this.panFilter.Controls.Add(this.label8);
         this.panFilter.Controls.Add(this.labKind);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Controls.Add(this.grpChkModel);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(465, 300);
         this.panFilter.TabIndex = 76;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // dwKindId
         // 
         this.dwKindId.Location = new System.Drawing.Point(86, 157);
         this.dwKindId.Name = "dwKindId";
         this.dwKindId.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.dwKindId.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.dwKindId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dwKindId.Properties.LookAndFeel.SkinName = "The Bezier";
         this.dwKindId.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.dwKindId.Properties.NullText = "";
         this.dwKindId.Properties.PopupSizeable = false;
         this.dwKindId.Size = new System.Drawing.Size(274, 26);
         this.dwKindId.TabIndex = 5;
         // 
         // grpRbDate
         // 
         this.grpRbDate.Controls.Add(this.txtDate);
         this.grpRbDate.Controls.Add(this.txtEndDate);
         this.grpRbDate.Controls.Add(this.txtStartDate);
         this.grpRbDate.Controls.Add(this.label11);
         this.grpRbDate.Controls.Add(this.label12);
         this.grpRbDate.Controls.Add(this.txtDay);
         this.grpRbDate.Controls.Add(this.label13);
         this.grpRbDate.Controls.Add(this.gbItem);
         this.grpRbDate.Location = new System.Drawing.Point(25, 30);
         this.grpRbDate.Name = "grpRbDate";
         this.grpRbDate.Size = new System.Drawing.Size(415, 115);
         this.grpRbDate.TabIndex = 88;
         this.grpRbDate.TabStop = false;
         this.grpRbDate.Text = "日期";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12/01";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(117, 68);
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
         this.txtDate.TabIndex = 3;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate.EditValue = "2018/12/01";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(247, 31);
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
         this.txtEndDate.TabIndex = 2;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartDate.EditValue = "2018/12/01";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(117, 31);
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
         this.txtStartDate.TabIndex = 1;
         this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Font = new System.Drawing.Font("微軟正黑體", 12F);
         this.label11.ForeColor = System.Drawing.Color.Black;
         this.label11.Location = new System.Drawing.Point(342, 71);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(57, 20);
         this.label11.TabIndex = 82;
         this.label11.Text = "天資料";
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(219, 34);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(26, 21);
         this.label12.TabIndex = 6;
         this.label12.Text = "～";
         // 
         // txtDay
         // 
         this.txtDay.EditValue = "2500";
         this.txtDay.Location = new System.Drawing.Point(270, 68);
         this.txtDay.MenuManager = this.ribbonControl;
         this.txtDay.Name = "txtDay";
         this.txtDay.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDay.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDay.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDay.Size = new System.Drawing.Size(66, 26);
         this.txtDay.TabIndex = 4;
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Font = new System.Drawing.Font("微軟正黑體", 12F);
         this.label13.ForeColor = System.Drawing.Color.Black;
         this.label13.Location = new System.Drawing.Point(223, 71);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(41, 20);
         this.label13.TabIndex = 80;
         this.label13.Text = "，近";
         // 
         // gbItem
         // 
         this.gbItem.EditValue = "rbSdateToEdate";
         this.gbItem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.gbItem.Location = new System.Drawing.Point(12, 20);
         this.gbItem.Name = "gbItem";
         this.gbItem.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbItem.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.gbItem.Properties.Appearance.Options.UseBackColor = true;
         this.gbItem.Properties.Appearance.Options.UseForeColor = true;
         this.gbItem.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.gbItem.Properties.Columns = 1;
         this.gbItem.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbSdateToEdate", "日期起訖："),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbEndDate", "迄止日期：")});
         this.gbItem.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.gbItem.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbItem.Size = new System.Drawing.Size(100, 85);
         this.gbItem.TabIndex = 0;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label8.ForeColor = System.Drawing.Color.Maroon;
         this.label8.Location = new System.Drawing.Point(82, 188);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(365, 19);
         this.label8.TabIndex = 90;
         this.label8.Text = "MSO上市日期2006.03.27，終止上市日期2011.09.22";
         // 
         // labKind
         // 
         this.labKind.AutoSize = true;
         this.labKind.ForeColor = System.Drawing.Color.Black;
         this.labKind.Location = new System.Drawing.Point(33, 160);
         this.labKind.Name = "labKind";
         this.labKind.Size = new System.Drawing.Size(58, 21);
         this.labKind.TabIndex = 85;
         this.labKind.Text = "商品：";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.ForeColor = System.Drawing.Color.MidnightBlue;
         this.label9.Location = new System.Drawing.Point(137, 313);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(0, 21);
         this.label9.TabIndex = 86;
         // 
         // grpChkModel
         // 
         this.grpChkModel.Controls.Add(this.chkModel);
         this.grpChkModel.Location = new System.Drawing.Point(25, 210);
         this.grpChkModel.Name = "grpChkModel";
         this.grpChkModel.Size = new System.Drawing.Size(413, 65);
         this.grpChkModel.TabIndex = 87;
         this.grpChkModel.TabStop = false;
         this.grpChkModel.Text = "模型";
         // 
         // chkModel
         // 
         this.chkModel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.chkModel.Appearance.Options.UseBackColor = true;
         this.chkModel.Appearance.Options.UseTextOptions = true;
         this.chkModel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.chkModel.AppearanceSelected.BackColor = System.Drawing.Color.White;
         this.chkModel.AppearanceSelected.BackColor2 = System.Drawing.Color.White;
         this.chkModel.AppearanceSelected.Options.UseBackColor = true;
         this.chkModel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.chkModel.CheckOnClick = true;
         this.chkModel.ColumnWidth = 130;
         this.chkModel.HotTrackSelectMode = DevExpress.XtraEditors.HotTrackSelectMode.SelectItemOnClick;
         this.chkModel.ItemAutoHeight = true;
         this.chkModel.ItemPadding = new System.Windows.Forms.Padding(5);
         this.chkModel.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkSma", "SMA", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkEwma", "EWMA"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkMaxVol", "MaxVol")});
         this.chkModel.Location = new System.Drawing.Point(18, 23);
         this.chkModel.LookAndFeel.SkinName = "Office 2013";
         this.chkModel.LookAndFeel.UseDefaultLookAndFeel = false;
         this.chkModel.MultiColumn = true;
         this.chkModel.Name = "chkModel";
         this.chkModel.SelectionMode = System.Windows.Forms.SelectionMode.None;
         this.chkModel.ShowFocusRect = false;
         this.chkModel.Size = new System.Drawing.Size(390, 34);
         this.chkModel.TabIndex = 6;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Location = new System.Drawing.Point(30, 30);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(505, 375);
         this.r_frame.TabIndex = 77;
         // 
         // W40170
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(555, 465);
         this.Name = "W40170";
         this.Text = "W40170";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).EndInit();
         this.grpRbDate.ResumeLayout(false);
         this.grpRbDate.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDay.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbItem.Properties)).EndInit();
         this.grpChkModel.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkModel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.GroupBox grpRbDate;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label12;
      private DevExpress.XtraEditors.TextEdit txtDay;
      private System.Windows.Forms.Label label13;
      protected DevExpress.XtraEditors.RadioGroup gbItem;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label labKind;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.GroupBox grpChkModel;
      private DevExpress.XtraEditors.CheckedListBoxControl chkModel;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private BaseGround.Widget.TextDateEdit txtDate;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
      private DevExpress.XtraEditors.LookUpEdit dwKindId;
   }
}