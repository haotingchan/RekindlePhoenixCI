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
         this.grpxDescription = new System.Windows.Forms.GroupBox();
         this.txtEndYMD = new BaseGround.Widget.TextDateEdit();
         this.txtStartYMD = new BaseGround.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.labAftDate = new System.Windows.Forms.Label();
         this.chkGroup = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.labMsg);
         this.panParent.Controls.Add(this.grpxDescription);
         this.panParent.Size = new System.Drawing.Size(665, 464);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(665, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.Controls.Add(this.txtEndYMD);
         this.grpxDescription.Controls.Add(this.txtStartYMD);
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.labAftDate);
         this.grpxDescription.Controls.Add(this.chkGroup);
         this.grpxDescription.Location = new System.Drawing.Point(15, 15);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(602, 242);
         this.grpxDescription.TabIndex = 7;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // txtEndYMD
         // 
         this.txtEndYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndYMD.EditValue = "2018/12";
         this.txtEndYMD.EnterMoveNextControl = true;
         this.txtEndYMD.Location = new System.Drawing.Point(241, 36);
         this.txtEndYMD.MenuManager = this.ribbonControl;
         this.txtEndYMD.Name = "txtEndYMD";
         this.txtEndYMD.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndYMD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndYMD.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndYMD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndYMD.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndYMD.Size = new System.Drawing.Size(100, 26);
         this.txtEndYMD.TabIndex = 1;
         this.txtEndYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartYMD
         // 
         this.txtStartYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartYMD.EditValue = "2018/12";
         this.txtStartYMD.EnterMoveNextControl = true;
         this.txtStartYMD.Location = new System.Drawing.Point(104, 36);
         this.txtStartYMD.MenuManager = this.ribbonControl;
         this.txtStartYMD.Name = "txtStartYMD";
         this.txtStartYMD.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartYMD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartYMD.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtStartYMD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartYMD.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartYMD.Size = new System.Drawing.Size(100, 26);
         this.txtStartYMD.TabIndex = 0;
         this.txtStartYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(210, 36);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(25, 20);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // labAftDate
         // 
         this.labAftDate.AutoSize = true;
         this.labAftDate.Location = new System.Drawing.Point(41, 39);
         this.labAftDate.Name = "labAftDate";
         this.labAftDate.Size = new System.Drawing.Size(57, 20);
         this.labAftDate.TabIndex = 2;
         this.labAftDate.Text = "日期：";
         // 
         // chkGroup
         // 
         this.chkGroup.AppearanceSelected.BackColor = System.Drawing.Color.White;
         this.chkGroup.AppearanceSelected.BackColor2 = System.Drawing.Color.White;
         this.chkGroup.AppearanceSelected.Options.UseBackColor = true;
         this.chkGroup.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.chkGroup.ItemPadding = new System.Windows.Forms.Padding(5);
         this.chkGroup.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkNewVixs", "新VIX統計檔", System.Windows.Forms.CheckState.Checked, 0),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkNewVix", "新VIX明細檔", 1),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkNewVols", "新VIX平均值", 2),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkNewVold", "新VIX平均值明細檔", 3),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkOldVixs", "舊VIX統計檔", 4),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkOldVix", "舊VIX明細檔", 5)});
         this.chkGroup.Location = new System.Drawing.Point(45, 68);
         this.chkGroup.MultiColumn = true;
         this.chkGroup.Name = "chkGroup";
         this.chkGroup.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.chkGroup.Size = new System.Drawing.Size(551, 146);
         this.chkGroup.TabIndex = 4;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 260);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // W30680
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(665, 494);
         this.Name = "W30680";
         this.Text = "W30680";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox grpxDescription;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label labAftDate;
      private BaseGround.Widget.TextDateEdit txtEndYMD;
      private BaseGround.Widget.TextDateEdit txtStartYMD;
      private System.Windows.Forms.Label labMsg;
      private DevExpress.XtraEditors.CheckedListBoxControl chkGroup;
   }
}