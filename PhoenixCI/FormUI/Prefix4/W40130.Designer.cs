namespace PhoenixCI.FormUI.Prefix4 {
   partial class W40130 {
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
         this.label1 = new System.Windows.Forms.Label();
         this.dwKindId = new DevExpress.XtraEditors.LookUpEdit();
         this.labKind = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(550, 221);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(550, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(20, 159);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.Controls.Add(this.txtDate);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.dwKindId);
         this.panFilter.Controls.Add(this.labKind);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(24, 23);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(471, 129);
         this.panFilter.TabIndex = 76;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.label1.ForeColor = System.Drawing.Color.Black;
         this.label1.Location = new System.Drawing.Point(36, 39);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(58, 21);
         this.label1.TabIndex = 83;
         this.label1.Text = "日期：";
         // 
         // dwKindId
         // 
         this.dwKindId.Location = new System.Drawing.Point(91, 74);
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
         this.dwKindId.Size = new System.Drawing.Size(274, 26);
         this.dwKindId.TabIndex = 1;
         // 
         // labKind
         // 
         this.labKind.AutoSize = true;
         this.labKind.ForeColor = System.Drawing.Color.Black;
         this.labKind.Location = new System.Drawing.Point(37, 77);
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
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Location = new System.Drawing.Point(15, 15);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(519, 188);
         this.r_frame.TabIndex = 77;
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(91, 36);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(100, 26);
         this.txtDate.TabIndex = 87;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // W40130
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(550, 251);
         this.Name = "W40130";
         this.Text = "W40130";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
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
      private System.Windows.Forms.Label label1;
      private BaseGround.Widget.TextDateEdit txtDate;
   }
}