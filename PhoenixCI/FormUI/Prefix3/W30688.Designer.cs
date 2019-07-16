namespace PhoenixCI.FormUI.Prefix3
{
   partial class W30688
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
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
      private void InitializeComponent()
      {
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.r_frame = new DevExpress.XtraEditors.PanelControl();
            this.stMsgTxt = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoGroup = new DevExpress.XtraEditors.RadioGroup();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEndDate = new BaseGround.Widget.TextDateEdit();
            this.txtStartDate = new BaseGround.Widget.TextDateEdit();
            this.st_3 = new System.Windows.Forms.Label();
            this.grpxDescription = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
            this.r_frame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdoGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            this.grpxDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Size = new System.Drawing.Size(695, 284);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(695, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.r_frame);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 30);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(695, 284);
            this.panelControl2.TabIndex = 1;
            // 
            // r_frame
            // 
            this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.r_frame.Appearance.Options.UseBackColor = true;
            this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.r_frame.Controls.Add(this.grpxDescription);
            this.r_frame.Controls.Add(this.stMsgTxt);
            this.r_frame.Location = new System.Drawing.Point(30, 27);
            this.r_frame.Name = "r_frame";
            this.r_frame.Size = new System.Drawing.Size(600, 207);
            this.r_frame.TabIndex = 1;
            // 
            // stMsgTxt
            // 
            this.stMsgTxt.AutoSize = true;
            this.stMsgTxt.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.stMsgTxt.ForeColor = System.Drawing.Color.Blue;
            this.stMsgTxt.Location = new System.Drawing.Point(24, 180);
            this.stMsgTxt.Name = "stMsgTxt";
            this.stMsgTxt.Size = new System.Drawing.Size(153, 19);
            this.stMsgTxt.TabIndex = 1;
            this.stMsgTxt.Text = "訊息：資料轉出中........";
            this.stMsgTxt.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(32, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 21);
            this.label1.TabIndex = 86;
            this.label1.Text = "商品：";
            // 
            // rdoGroup
            // 
            this.rdoGroup.EditValue = "%";
            this.rdoGroup.Location = new System.Drawing.Point(96, 93);
            this.rdoGroup.Name = "rdoGroup";
            this.rdoGroup.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoGroup.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.rdoGroup.Properties.Appearance.Options.UseBackColor = true;
            this.rdoGroup.Properties.Appearance.Options.UseForeColor = true;
            this.rdoGroup.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoGroup.Properties.Columns = 4;
            this.rdoGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("TWSE", "臺指選擇權"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("TWSEELEC", "電子選擇權"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("TWSEBKI", "金融選擇權"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("%", "全部")});
            this.rdoGroup.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
            this.rdoGroup.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.rdoGroup.Size = new System.Drawing.Size(423, 35);
            this.rdoGroup.TabIndex = 85;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 21);
            this.label3.TabIndex = 84;
            this.label3.Text = "～";
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate.EditValue = "2018/12/01";
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(224, 44);
            this.txtEndDate.MenuManager = this.ribbonControl;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEndDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtEndDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEndDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Size = new System.Drawing.Size(100, 28);
            this.txtEndDate.TabIndex = 83;
            this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate.EditValue = "2018/12/01";
            this.txtStartDate.EnterMoveNextControl = true;
            this.txtStartDate.Location = new System.Drawing.Point(96, 44);
            this.txtStartDate.MenuManager = this.ribbonControl;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStartDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtStartDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtStartDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Size = new System.Drawing.Size(100, 28);
            this.txtStartDate.TabIndex = 82;
            this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // st_3
            // 
            this.st_3.AutoSize = true;
            this.st_3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.st_3.ForeColor = System.Drawing.Color.Black;
            this.st_3.Location = new System.Drawing.Point(32, 48);
            this.st_3.Name = "st_3";
            this.st_3.Size = new System.Drawing.Size(58, 21);
            this.st_3.TabIndex = 0;
            this.st_3.Text = "日期：";
            // 
            // grpxDescription
            // 
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.st_3);
            this.grpxDescription.Controls.Add(this.rdoGroup);
            this.grpxDescription.Controls.Add(this.txtStartDate);
            this.grpxDescription.Controls.Add(this.label3);
            this.grpxDescription.Controls.Add(this.txtEndDate);
            this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
            this.grpxDescription.Location = new System.Drawing.Point(28, 24);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(541, 151);
            this.grpxDescription.TabIndex = 6;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // W30688
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 314);
            this.Controls.Add(this.panelControl2);
            this.Name = "W30688";
            this.Text = "W30688";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
            this.r_frame.ResumeLayout(false);
            this.r_frame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdoGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label stMsgTxt;
      private System.Windows.Forms.Label st_3;
        private System.Windows.Forms.Label label3;
        private BaseGround.Widget.TextDateEdit txtEndDate;
        private BaseGround.Widget.TextDateEdit txtStartDate;
        private System.Windows.Forms.Label label1;
        protected DevExpress.XtraEditors.RadioGroup rdoGroup;
        private System.Windows.Forms.GroupBox grpxDescription;
    }
}