namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30680 {
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
         this.chkGroup = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtEndYMD = new BaseGround.Widget.TextDateEdit();
         this.txtStartYMD = new BaseGround.Widget.TextDateEdit();
         this.label14 = new System.Windows.Forms.Label();
         this.label15 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(590, 375);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(590, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // chkGroup
         // 
         this.chkGroup.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.chkGroup.Appearance.Options.UseBackColor = true;
         this.chkGroup.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.chkGroup.ItemPadding = new System.Windows.Forms.Padding(5);
         this.chkGroup.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkNewVixs", "新VIX統計檔", System.Windows.Forms.CheckState.Checked, 0),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkNewVix", "新VIX明細檔", 1),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkNewVols", "新VIX平均值", 2),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkNewVold", "新VIX平均值明細檔", 3),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkOldVixs", "舊VIX統計檔", 4),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkOldVix", "舊VIX明細檔", 5)});
         this.chkGroup.Location = new System.Drawing.Point(92, 90);
         this.chkGroup.LookAndFeel.SkinName = "Office 2013";
         this.chkGroup.LookAndFeel.UseDefaultLookAndFeel = false;
         this.chkGroup.MultiColumn = true;
         this.chkGroup.Name = "chkGroup";
         this.chkGroup.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.chkGroup.Size = new System.Drawing.Size(394, 121);
         this.chkGroup.TabIndex = 4;
         this.chkGroup.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.chkGroup_DrawItem);
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
         this.r_frame.Size = new System.Drawing.Size(540, 315);
         this.r_frame.TabIndex = 84;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 265);
         this.labMsg.MaximumSize = new System.Drawing.Size(500, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 82;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.txtEndYMD);
         this.panFilter.Controls.Add(this.txtStartYMD);
         this.panFilter.Controls.Add(this.chkGroup);
         this.panFilter.Controls.Add(this.label14);
         this.panFilter.Controls.Add(this.label15);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(500, 240);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtEndYMD
         // 
         this.txtEndYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndYMD.EditValue = "2018/12/01";
         this.txtEndYMD.EnterMoveNextControl = true;
         this.txtEndYMD.Location = new System.Drawing.Point(220, 42);
         this.txtEndYMD.MenuManager = this.ribbonControl;
         this.txtEndYMD.Name = "txtEndYMD";
         this.txtEndYMD.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndYMD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndYMD.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndYMD.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtEndYMD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEndYMD.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndYMD.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndYMD.Size = new System.Drawing.Size(100, 26);
         this.txtEndYMD.TabIndex = 4;
         this.txtEndYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartYMD
         // 
         this.txtStartYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartYMD.EditValue = "2018/12/01";
         this.txtStartYMD.EnterMoveNextControl = true;
         this.txtStartYMD.Location = new System.Drawing.Point(92, 42);
         this.txtStartYMD.MenuManager = this.ribbonControl;
         this.txtStartYMD.Name = "txtStartYMD";
         this.txtStartYMD.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartYMD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartYMD.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtStartYMD.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtStartYMD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtStartYMD.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartYMD.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartYMD.Size = new System.Drawing.Size(100, 26);
         this.txtStartYMD.TabIndex = 0;
         this.txtStartYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(194, 45);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(26, 21);
         this.label14.TabIndex = 6;
         this.label14.Text = "～";
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.ForeColor = System.Drawing.Color.Black;
         this.label15.Location = new System.Drawing.Point(37, 45);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(58, 21);
         this.label15.TabIndex = 2;
         this.label15.Text = "日期：";
         // 
         // W30680
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(590, 405);
         this.Name = "W30680";
         this.Text = "W30680";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.CheckedListBoxControl chkGroup;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private BaseGround.Widget.TextDateEdit txtEndYMD;
      private BaseGround.Widget.TextDateEdit txtStartYMD;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.Label label15;
   }
}