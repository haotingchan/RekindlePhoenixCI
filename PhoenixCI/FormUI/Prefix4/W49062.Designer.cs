namespace PhoenixCI.FormUI.Prefix4 {
   partial class W49062 {
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
            this.grpRbDate = new System.Windows.Forms.GroupBox();
            this.txtEndDate2 = new BaseGround.Widget.TextDateEdit();
            this.txtStartDate2 = new BaseGround.Widget.TextDateEdit();
            this.txtEndDate = new BaseGround.Widget.TextDateEdit();
            this.txtStartDate = new BaseGround.Widget.TextDateEdit();
            this.txtDate = new BaseGround.Widget.TextDateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbItem = new DevExpress.XtraEditors.RadioGroup();
            this.label2 = new System.Windows.Forms.Label();
            this.labKind = new System.Windows.Forms.Label();
            this.r_frame = new DevExpress.XtraEditors.PanelControl();
            this.dwKindId = new DevExpress.XtraEditors.LookUpEdit();
            this.dwFId = new DevExpress.XtraEditors.LookUpEdit();
            this.lblNote = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpRbDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbItem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
            this.r_frame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwFId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.r_frame);
            this.panParent.Size = new System.Drawing.Size(640, 451);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(640, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // labMsg
            // 
            this.labMsg.AutoSize = true;
            this.labMsg.ForeColor = System.Drawing.Color.Blue;
            this.labMsg.Location = new System.Drawing.Point(25, 343);
            this.labMsg.MaximumSize = new System.Drawing.Size(530, 120);
            this.labMsg.Name = "labMsg";
            this.labMsg.Size = new System.Drawing.Size(85, 20);
            this.labMsg.TabIndex = 10;
            this.labMsg.Text = "開始轉檔...";
            this.labMsg.Visible = false;
            // 
            // grpRbDate
            // 
            this.grpRbDate.Controls.Add(this.txtEndDate2);
            this.grpRbDate.Controls.Add(this.txtStartDate2);
            this.grpRbDate.Controls.Add(this.txtEndDate);
            this.grpRbDate.Controls.Add(this.txtStartDate);
            this.grpRbDate.Controls.Add(this.txtDate);
            this.grpRbDate.Controls.Add(this.label3);
            this.grpRbDate.Controls.Add(this.label1);
            this.grpRbDate.Controls.Add(this.gbItem);
            this.grpRbDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.grpRbDate.ForeColor = System.Drawing.Color.Navy;
            this.grpRbDate.Location = new System.Drawing.Point(30, 115);
            this.grpRbDate.Name = "grpRbDate";
            this.grpRbDate.Size = new System.Drawing.Size(530, 150);
            this.grpRbDate.TabIndex = 89;
            this.grpRbDate.TabStop = false;
            this.grpRbDate.Text = "報表類別";
            // 
            // txtEndDate2
            // 
            this.txtEndDate2.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEndDate2.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate2.EditValue = "2018/12/01";
            this.txtEndDate2.EnterMoveNextControl = true;
            this.txtEndDate2.Location = new System.Drawing.Point(334, 106);
            this.txtEndDate2.MenuManager = this.ribbonControl;
            this.txtEndDate2.Name = "txtEndDate2";
            this.txtEndDate2.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEndDate2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEndDate2.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtEndDate2.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtEndDate2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEndDate2.Properties.Mask.ShowPlaceHolders = false;
            this.txtEndDate2.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate2.Size = new System.Drawing.Size(100, 26);
            this.txtEndDate2.TabIndex = 7;
            this.txtEndDate2.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtStartDate2
            // 
            this.txtStartDate2.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtStartDate2.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate2.EditValue = "2018/12/01";
            this.txtStartDate2.EnterMoveNextControl = true;
            this.txtStartDate2.Location = new System.Drawing.Point(196, 106);
            this.txtStartDate2.MenuManager = this.ribbonControl;
            this.txtStartDate2.Name = "txtStartDate2";
            this.txtStartDate2.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStartDate2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStartDate2.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtStartDate2.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtStartDate2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtStartDate2.Properties.Mask.ShowPlaceHolders = false;
            this.txtStartDate2.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate2.Size = new System.Drawing.Size(100, 26);
            this.txtStartDate2.TabIndex = 6;
            this.txtStartDate2.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate.EditValue = "2018/12/01";
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(414, 68);
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
            this.txtEndDate.TabIndex = 5;
            this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate.EditValue = "2018/12/01";
            this.txtStartDate.EnterMoveNextControl = true;
            this.txtStartDate.Location = new System.Drawing.Point(276, 68);
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
            this.txtStartDate.TabIndex = 4;
            this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtDate
            // 
            this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtDate.EditValue = "2018/12/01";
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(324, 31);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(382, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 21);
            this.label3.TabIndex = 83;
            this.label3.Text = "～";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(302, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 21);
            this.label1.TabIndex = 80;
            this.label1.Text = "～";
            // 
            // gbItem
            // 
            this.gbItem.EditValue = "rbNewDate";
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
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbNewDate", "「保證金調整」最新及次新生效日資料："),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbOldDate", "「保證金調整」歷史生效日資料："),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbHistory", "「保證金」歷史資料：")});
            this.gbItem.Properties.LookAndFeel.SkinName = "Office 2013 Light Gray";
            this.gbItem.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gbItem.Size = new System.Drawing.Size(513, 122);
            this.gbItem.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(25, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 21);
            this.label2.TabIndex = 88;
            this.label2.Text = "交易所 + 商品：";
            // 
            // labKind
            // 
            this.labKind.AutoSize = true;
            this.labKind.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.labKind.ForeColor = System.Drawing.Color.Black;
            this.labKind.Location = new System.Drawing.Point(25, 75);
            this.labKind.Name = "labKind";
            this.labKind.Size = new System.Drawing.Size(90, 21);
            this.labKind.TabIndex = 85;
            this.labKind.Text = "契約類別：";
            // 
            // r_frame
            // 
            this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.r_frame.Appearance.Options.UseBackColor = true;
            this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.r_frame.Controls.Add(this.lblNote);
            this.r_frame.Controls.Add(this.dwKindId);
            this.r_frame.Controls.Add(this.dwFId);
            this.r_frame.Controls.Add(this.grpRbDate);
            this.r_frame.Controls.Add(this.labMsg);
            this.r_frame.Controls.Add(this.label2);
            this.r_frame.Controls.Add(this.labKind);
            this.r_frame.Location = new System.Drawing.Point(30, 30);
            this.r_frame.Name = "r_frame";
            this.r_frame.Size = new System.Drawing.Size(590, 385);
            this.r_frame.TabIndex = 77;
            // 
            // dwKindId
            // 
            this.dwKindId.Location = new System.Drawing.Point(157, 72);
            this.dwKindId.Name = "dwKindId";
            this.dwKindId.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dwKindId.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.dwKindId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dwKindId.Properties.LookAndFeel.SkinName = "The Bezier";
            this.dwKindId.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.dwKindId.Properties.NullText = "";
            this.dwKindId.Properties.PopupSizeable = false;
            this.dwKindId.Size = new System.Drawing.Size(159, 26);
            this.dwKindId.TabIndex = 1;
            // 
            // dwFId
            // 
            this.dwFId.Location = new System.Drawing.Point(157, 27);
            this.dwFId.Name = "dwFId";
            this.dwFId.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dwFId.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.dwFId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dwFId.Properties.LookAndFeel.SkinName = "The Bezier";
            this.dwFId.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.dwFId.Properties.NullText = "";
            this.dwFId.Properties.PopupSizeable = false;
            this.dwFId.Size = new System.Drawing.Size(403, 26);
            this.dwFId.TabIndex = 0;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.ForeColor = System.Drawing.Color.Red;
            this.lblNote.Location = new System.Drawing.Point(26, 278);
            this.lblNote.MaximumSize = new System.Drawing.Size(550, 120);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(538, 40);
            this.lblNote.TabIndex = 90;
            this.lblNote.Text = "【註：工作表[Summary-最新契約價值]國外商品價格採前一日16:00最新成交價】";
            this.lblNote.Visible = false;
            // 
            // W49062
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 481);
            this.Name = "W49062";
            this.Text = "W49062";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpRbDate.ResumeLayout(false);
            this.grpRbDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbItem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
            this.r_frame.ResumeLayout(false);
            this.r_frame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwFId.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.Label labKind;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.GroupBox grpRbDate;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label1;
      protected DevExpress.XtraEditors.RadioGroup gbItem;
      private BaseGround.Widget.TextDateEdit txtEndDate2;
      private BaseGround.Widget.TextDateEdit txtStartDate2;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
      private BaseGround.Widget.TextDateEdit txtDate;
      private DevExpress.XtraEditors.LookUpEdit dwKindId;
      private DevExpress.XtraEditors.LookUpEdit dwFId;
        private System.Windows.Forms.Label lblNote;
    }
}