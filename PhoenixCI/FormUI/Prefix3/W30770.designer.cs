namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30770 {
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
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtEndMonth = new PhoenixCI.Widget.TextDateEdit();
         this.txtStartMonth = new PhoenixCI.Widget.TextDateEdit();
         this.panNight = new System.Windows.Forms.GroupBox();
         this.rdoGroup = new DevExpress.XtraEditors.RadioGroup();
         this.txtEndDate = new PhoenixCI.Widget.TextDateEdit();
         this.txtStartDate = new PhoenixCI.Widget.TextDateEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.chkGroup = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.ddlOswGrp = new DevExpress.XtraEditors.LookUpEdit();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).BeginInit();
         this.panNight.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rdoGroup.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlOswGrp.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.ddlOswGrp);
         this.panParent.Controls.Add(this.labMsg);
         this.panParent.Controls.Add(this.panFilter);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Controls.Add(this.txtEndMonth);
         this.panFilter.Controls.Add(this.txtStartMonth);
         this.panFilter.Controls.Add(this.panNight);
         this.panFilter.Controls.Add(this.txtEndDate);
         this.panFilter.Controls.Add(this.txtStartDate);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.label6);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.chkGroup);
         this.panFilter.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(15, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(367, 195);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtEndMonth
         // 
         this.txtEndMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndMonth.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndMonth.EditValue = "2018/12";
         this.txtEndMonth.EnterMoveNextControl = true;
         this.txtEndMonth.Location = new System.Drawing.Point(240, 26);
         this.txtEndMonth.MenuManager = this.ribbonControl;
         this.txtEndMonth.Name = "txtEndMonth";
         this.txtEndMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndMonth.Properties.Mask.EditMask = "yyyy/MM";
         this.txtEndMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndMonth.Size = new System.Drawing.Size(100, 26);
         this.txtEndMonth.TabIndex = 1;
         this.txtEndMonth.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartMonth
         // 
         this.txtStartMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartMonth.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartMonth.EditValue = "2018/12";
         this.txtStartMonth.EnterMoveNextControl = true;
         this.txtStartMonth.Location = new System.Drawing.Point(103, 26);
         this.txtStartMonth.MenuManager = this.ribbonControl;
         this.txtStartMonth.Name = "txtStartMonth";
         this.txtStartMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartMonth.Properties.Mask.EditMask = "yyyy/MM";
         this.txtStartMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartMonth.Size = new System.Drawing.Size(100, 26);
         this.txtStartMonth.TabIndex = 0;
         this.txtStartMonth.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // panNight
         // 
         this.panNight.Controls.Add(this.rdoGroup);
         this.panNight.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panNight.ForeColor = System.Drawing.Color.Navy;
         this.panNight.Location = new System.Drawing.Point(23, 113);
         this.panNight.Name = "panNight";
         this.panNight.Size = new System.Drawing.Size(317, 66);
         this.panNight.TabIndex = 77;
         this.panNight.TabStop = false;
         this.panNight.Text = "資料";
         // 
         // rdoGroup
         // 
         this.rdoGroup.EditValue = "Future";
         this.rdoGroup.Location = new System.Drawing.Point(33, 22);
         this.rdoGroup.Name = "rdoGroup";
         this.rdoGroup.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.rdoGroup.Properties.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.rdoGroup.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.rdoGroup.Properties.Appearance.Options.UseBackColor = true;
         this.rdoGroup.Properties.Appearance.Options.UseFont = true;
         this.rdoGroup.Properties.Appearance.Options.UseForeColor = true;
         this.rdoGroup.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.rdoGroup.Properties.Columns = 3;
         this.rdoGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Future", "期貨"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Option", "選擇權"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("%", "全部")});
         this.rdoGroup.Size = new System.Drawing.Size(248, 35);
         this.rdoGroup.TabIndex = 4;
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndDate.EditValue = "2018/12";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(240, 69);
         this.txtEndDate.MenuManager = this.ribbonControl;
         this.txtEndDate.Name = "txtEndDate";
         this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate.TabIndex = 3;
         this.txtEndDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartDate.EditValue = "2018/12";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(103, 69);
         this.txtStartDate.MenuManager = this.ribbonControl;
         this.txtStartDate.Name = "txtStartDate";
         this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartDate.Size = new System.Drawing.Size(100, 26);
         this.txtStartDate.TabIndex = 2;
         this.txtStartDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(209, 75);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(25, 16);
         this.label2.TabIndex = 76;
         this.label2.Text = "～";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.ForeColor = System.Drawing.Color.MidnightBlue;
         this.label6.Location = new System.Drawing.Point(136, 308);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(0, 16);
         this.label6.TabIndex = 73;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(209, 31);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(25, 16);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // chkGroup
         // 
         this.chkGroup.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.chkGroup.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.chkGroup.Appearance.Options.UseBackColor = true;
         this.chkGroup.Appearance.Options.UseFont = true;
         this.chkGroup.AppearanceSelected.BackColor = System.Drawing.Color.White;
         this.chkGroup.AppearanceSelected.BackColor2 = System.Drawing.Color.White;
         this.chkGroup.AppearanceSelected.Options.UseBackColor = true;
         this.chkGroup.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.chkGroup.HorzScrollStep = 9;
         this.chkGroup.ItemPadding = new System.Windows.Forms.Padding(11);
         this.chkGroup.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("Month", "月份：", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("Day", "日期：")});
         this.chkGroup.Location = new System.Drawing.Point(29, 19);
         this.chkGroup.Name = "chkGroup";
         this.chkGroup.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.chkGroup.Size = new System.Drawing.Size(74, 88);
         this.chkGroup.TabIndex = 5;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 213);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(169, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "訊息：資料轉出中........";
         this.labMsg.Visible = false;
         // 
         // ddlOswGrp
         // 
         this.ddlOswGrp.Location = new System.Drawing.Point(388, 43);
         this.ddlOswGrp.Name = "ddlOswGrp";
         this.ddlOswGrp.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.ddlOswGrp.Properties.Appearance.Options.UseBackColor = true;
         this.ddlOswGrp.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlOswGrp.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlOswGrp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlOswGrp.Properties.DropDownRows = 5;
         this.ddlOswGrp.Properties.NullText = "";
         this.ddlOswGrp.Properties.PopupSizeable = false;
         this.ddlOswGrp.Size = new System.Drawing.Size(122, 26);
         this.ddlOswGrp.TabIndex = 6;
         this.ddlOswGrp.Visible = false;
         // 
         // W30770
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Name = "W30770";
         this.Text = "W30770";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).EndInit();
         this.panNight.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.rdoGroup.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlOswGrp.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.Label label6;
      private DevExpress.XtraEditors.CheckedListBoxControl chkGroup;
        private Widget.TextDateEdit txtEndDate;
        private Widget.TextDateEdit txtStartDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox panNight;
        protected DevExpress.XtraEditors.RadioGroup rdoGroup;
        private Widget.TextDateEdit txtEndMonth;
        private Widget.TextDateEdit txtStartMonth;
        private DevExpress.XtraEditors.LookUpEdit ddlOswGrp;
    }
}