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
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.dwKindId = new DevExpress.XtraEditors.LookUpEdit();
         this.labKind = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.dwFId = new DevExpress.XtraEditors.LookUpEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.grpRbDate = new System.Windows.Forms.GroupBox();
         this.txtDate = new PhoenixCI.Widget.TextDateEdit();
         this.gbItem = new DevExpress.XtraEditors.RadioGroup();
         this.txtStartDate2 = new PhoenixCI.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.txtEndDate2 = new PhoenixCI.Widget.TextDateEdit();
         this.txtStartDate = new PhoenixCI.Widget.TextDateEdit();
         this.label3 = new System.Windows.Forms.Label();
         this.txtEndDate = new PhoenixCI.Widget.TextDateEdit();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwFId.Properties)).BeginInit();
         this.grpRbDate.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbItem.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate2.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate2.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(667, 354);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(667, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(20, 295);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.Controls.Add(this.grpRbDate);
         this.panFilter.Controls.Add(this.dwFId);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.dwKindId);
         this.panFilter.Controls.Add(this.labKind);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(24, 23);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(590, 265);
         this.panFilter.TabIndex = 76;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // dwKindId
         // 
         this.dwKindId.Location = new System.Drawing.Point(169, 62);
         this.dwKindId.Name = "dwKindId";
         this.dwKindId.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.dwKindId.Properties.Appearance.Options.UseBackColor = true;
         this.dwKindId.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.dwKindId.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.dwKindId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dwKindId.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
         this.dwKindId.Properties.NullText = "";
         this.dwKindId.Properties.PopupSizeable = false;
         this.dwKindId.Size = new System.Drawing.Size(159, 26);
         this.dwKindId.TabIndex = 1;
         // 
         // labKind
         // 
         this.labKind.AutoSize = true;
         this.labKind.ForeColor = System.Drawing.Color.Black;
         this.labKind.Location = new System.Drawing.Point(37, 65);
         this.labKind.Name = "labKind";
         this.labKind.Size = new System.Drawing.Size(90, 21);
         this.labKind.TabIndex = 85;
         this.labKind.Text = "契約類別：";
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
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Location = new System.Drawing.Point(15, 15);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(636, 323);
         this.r_frame.TabIndex = 77;
         // 
         // dwFId
         // 
         this.dwFId.Location = new System.Drawing.Point(169, 28);
         this.dwFId.Name = "dwFId";
         this.dwFId.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.dwFId.Properties.Appearance.Options.UseBackColor = true;
         this.dwFId.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.dwFId.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.dwFId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dwFId.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
         this.dwFId.Properties.NullText = "";
         this.dwFId.Properties.PopupSizeable = false;
         this.dwFId.Size = new System.Drawing.Size(403, 26);
         this.dwFId.TabIndex = 0;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.ForeColor = System.Drawing.Color.Black;
         this.label2.Location = new System.Drawing.Point(37, 31);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(126, 21);
         this.label2.TabIndex = 88;
         this.label2.Text = "交易所 + 商品：";
         // 
         // grpRbDate
         // 
         this.grpRbDate.Controls.Add(this.txtStartDate);
         this.grpRbDate.Controls.Add(this.label3);
         this.grpRbDate.Controls.Add(this.txtEndDate);
         this.grpRbDate.Controls.Add(this.txtStartDate2);
         this.grpRbDate.Controls.Add(this.label1);
         this.grpRbDate.Controls.Add(this.txtEndDate2);
         this.grpRbDate.Controls.Add(this.txtDate);
         this.grpRbDate.Controls.Add(this.gbItem);
         this.grpRbDate.Location = new System.Drawing.Point(41, 94);
         this.grpRbDate.Name = "grpRbDate";
         this.grpRbDate.Size = new System.Drawing.Size(531, 148);
         this.grpRbDate.TabIndex = 89;
         this.grpRbDate.TabStop = false;
         this.grpRbDate.Text = "報表類別";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtDate.EditValue = "2018/12";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(324, 29);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(100, 26);
         this.txtDate.TabIndex = 3;
         this.txtDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
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
         this.gbItem.Size = new System.Drawing.Size(513, 122);
         this.gbItem.TabIndex = 2;
         // 
         // txtStartDate2
         // 
         this.txtStartDate2.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate2.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartDate2.EditValue = "2018/12";
         this.txtStartDate2.EnterMoveNextControl = true;
         this.txtStartDate2.Location = new System.Drawing.Point(196, 106);
         this.txtStartDate2.MenuManager = this.ribbonControl;
         this.txtStartDate2.Name = "txtStartDate2";
         this.txtStartDate2.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartDate2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartDate2.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtStartDate2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartDate2.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartDate2.Size = new System.Drawing.Size(100, 26);
         this.txtStartDate2.TabIndex = 6;
         this.txtStartDate2.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
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
         // txtEndDate2
         // 
         this.txtEndDate2.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate2.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndDate2.EditValue = "2018/12";
         this.txtEndDate2.EnterMoveNextControl = true;
         this.txtEndDate2.Location = new System.Drawing.Point(333, 106);
         this.txtEndDate2.MenuManager = this.ribbonControl;
         this.txtEndDate2.Name = "txtEndDate2";
         this.txtEndDate2.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate2.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndDate2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndDate2.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate2.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate2.TabIndex = 7;
         this.txtEndDate2.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartDate.EditValue = "2018/12";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(276, 68);
         this.txtStartDate.MenuManager = this.ribbonControl;
         this.txtStartDate.Name = "txtStartDate";
         this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartDate.Size = new System.Drawing.Size(100, 26);
         this.txtStartDate.TabIndex = 4;
         this.txtStartDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
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
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndDate.EditValue = "2018/12";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(413, 68);
         this.txtEndDate.MenuManager = this.ribbonControl;
         this.txtEndDate.Name = "txtEndDate";
         this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate.TabIndex = 5;
         this.txtEndDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // W49062
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(667, 384);
         this.Name = "W49062";
         this.Text = "W49062";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwFId.Properties)).EndInit();
         this.grpRbDate.ResumeLayout(false);
         this.grpRbDate.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbItem.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate2.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate2.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private DevExpress.XtraEditors.LookUpEdit dwKindId;
      private System.Windows.Forms.Label labKind;
      private System.Windows.Forms.Label label9;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private DevExpress.XtraEditors.LookUpEdit dwFId;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.GroupBox grpRbDate;
      private Widget.TextDateEdit txtStartDate;
      private System.Windows.Forms.Label label3;
      private Widget.TextDateEdit txtEndDate;
      private Widget.TextDateEdit txtStartDate2;
      private System.Windows.Forms.Label label1;
      private Widget.TextDateEdit txtEndDate2;
      private Widget.TextDateEdit txtDate;
      protected DevExpress.XtraEditors.RadioGroup gbItem;
   }
}