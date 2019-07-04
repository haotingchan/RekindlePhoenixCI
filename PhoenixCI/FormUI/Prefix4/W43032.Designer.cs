namespace PhoenixCI.FormUI.Prefix4 {
   partial class W43032 {
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
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.labKind = new System.Windows.Forms.Label();
         this.grpRbDate = new System.Windows.Forms.GroupBox();
         this.txtSid = new DevExpress.XtraEditors.TextEdit();
         this.gbItem = new DevExpress.XtraEditors.RadioGroup();
         this.label9 = new System.Windows.Forms.Label();
         this.grpChkModel = new System.Windows.Forms.GroupBox();
         this.chkModel = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         this.grpRbDate.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtSid.Properties)).BeginInit();
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
         this.panParent.Size = new System.Drawing.Size(468, 525);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(468, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 415);
         this.labMsg.MaximumSize = new System.Drawing.Size(378, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.Controls.Add(this.txtDate);
         this.panFilter.Controls.Add(this.labKind);
         this.panFilter.Controls.Add(this.grpRbDate);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Controls.Add(this.grpChkModel);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(378, 390);
         this.panFilter.TabIndex = 76;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12/01";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(90, 42);
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
         this.txtDate.TabIndex = 0;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // labKind
         // 
         this.labKind.AutoSize = true;
         this.labKind.ForeColor = System.Drawing.Color.Black;
         this.labKind.Location = new System.Drawing.Point(37, 45);
         this.labKind.Name = "labKind";
         this.labKind.Size = new System.Drawing.Size(58, 21);
         this.labKind.TabIndex = 90;
         this.labKind.Text = "日期：";
         // 
         // grpRbDate
         // 
         this.grpRbDate.Controls.Add(this.txtSid);
         this.grpRbDate.Controls.Add(this.gbItem);
         this.grpRbDate.Location = new System.Drawing.Point(27, 90);
         this.grpRbDate.Name = "grpRbDate";
         this.grpRbDate.Size = new System.Drawing.Size(325, 200);
         this.grpRbDate.TabIndex = 88;
         this.grpRbDate.TabStop = false;
         this.grpRbDate.Text = "查詢條件";
         // 
         // txtSid
         // 
         this.txtSid.EditValue = "";
         this.txtSid.Location = new System.Drawing.Point(143, 159);
         this.txtSid.MenuManager = this.ribbonControl;
         this.txtSid.Name = "txtSid";
         this.txtSid.Properties.Appearance.Options.UseTextOptions = true;
         this.txtSid.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtSid.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtSid.Size = new System.Drawing.Size(66, 26);
         this.txtSid.TabIndex = 2;
         // 
         // gbItem
         // 
         this.gbItem.EditValue = "rbSid";
         this.gbItem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.gbItem.Location = new System.Drawing.Point(18, 24);
         this.gbItem.Name = "gbItem";
         this.gbItem.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbItem.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.gbItem.Properties.Appearance.Options.UseBackColor = true;
         this.gbItem.Properties.Appearance.Options.UseForeColor = true;
         this.gbItem.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.gbItem.Properties.Columns = 1;
         this.gbItem.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbFut", "股票期貨上市標的證券"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbOpt", "股票選擇權上市標的證券"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbAll", "股票期貨暨選擇權上市標的證券"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbAdj", "本日保證金異動標的證券"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbSid", "選擇單一證券")});
         this.gbItem.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.gbItem.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbItem.Size = new System.Drawing.Size(286, 170);
         this.gbItem.TabIndex = 1;
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
         this.grpChkModel.Location = new System.Drawing.Point(27, 295);
         this.grpChkModel.Name = "grpChkModel";
         this.grpChkModel.Size = new System.Drawing.Size(325, 70);
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
         this.chkModel.ColumnWidth = 100;
         this.chkModel.HotTrackSelectMode = DevExpress.XtraEditors.HotTrackSelectMode.SelectItemOnClick;
         this.chkModel.ItemAutoHeight = true;
         this.chkModel.ItemPadding = new System.Windows.Forms.Padding(5);
         this.chkModel.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkSma", "SMA", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkEwma", "EWMA"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkMaxVol", "MaxVol")});
         this.chkModel.Location = new System.Drawing.Point(20, 26);
         this.chkModel.LookAndFeel.SkinName = "Office 2013";
         this.chkModel.LookAndFeel.UseDefaultLookAndFeel = false;
         this.chkModel.MultiColumn = true;
         this.chkModel.Name = "chkModel";
         this.chkModel.SelectionMode = System.Windows.Forms.SelectionMode.None;
         this.chkModel.ShowFocusRect = false;
         this.chkModel.Size = new System.Drawing.Size(304, 34);
         this.chkModel.TabIndex = 3;
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
         this.r_frame.Size = new System.Drawing.Size(418, 465);
         this.r_frame.TabIndex = 77;
         // 
         // W43032
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(468, 555);
         this.Name = "W43032";
         this.Text = "W43032";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         this.grpRbDate.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.txtSid.Properties)).EndInit();
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
      private DevExpress.XtraEditors.TextEdit txtSid;
      protected DevExpress.XtraEditors.RadioGroup gbItem;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.GroupBox grpChkModel;
      private DevExpress.XtraEditors.CheckedListBoxControl chkModel;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private BaseGround.Widget.TextDateEdit txtDate;
      private System.Windows.Forms.Label labKind;
   }
}