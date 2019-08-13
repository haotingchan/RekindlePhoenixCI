namespace PhoenixCI.FormUI.Prefix4 {
   partial class W40030 {
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(W40030));
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.grpxDescription = new System.Windows.Forms.GroupBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.MarketTimes = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.txtDate1 = new BaseGround.Widget.TextDateEdit();
         this.ETCSelect = new DevExpress.XtraEditors.LookUpEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.txtDate5 = new BaseGround.Widget.TextDateEdit();
         this.txtDate7 = new BaseGround.Widget.TextDateEdit();
         this.ExportShow = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.ddlAdjType = new DevExpress.XtraEditors.LookUpEdit();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.panel1 = new System.Windows.Forms.Panel();
         this.label3 = new System.Windows.Forms.Label();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.grpxDescription.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.MarketTimes)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ETCSelect.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate5.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate7.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlAdjType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.pictureBox1);
         this.panParent.Controls.Add(this.label3);
         this.panParent.Controls.Add(this.panel1);
         this.panParent.Size = new System.Drawing.Size(801, 630);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(801, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(801, 630);
         this.panelControl1.TabIndex = 0;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.grpxDescription.Controls.Add(this.groupBox1);
         this.grpxDescription.Controls.Add(this.ExportShow);
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.ddlAdjType);
         this.grpxDescription.Controls.Add(this.txtDate);
         this.grpxDescription.Controls.Add(this.lblDate);
         this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
         this.grpxDescription.Location = new System.Drawing.Point(17, 22);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(569, 328);
         this.grpxDescription.TabIndex = 13;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.MarketTimes);
         this.groupBox1.Controls.Add(this.txtDate1);
         this.groupBox1.Controls.Add(this.ETCSelect);
         this.groupBox1.Controls.Add(this.label2);
         this.groupBox1.Controls.Add(this.txtDate5);
         this.groupBox1.Controls.Add(this.txtDate7);
         this.groupBox1.Location = new System.Drawing.Point(41, 113);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(343, 161);
         this.groupBox1.TabIndex = 15;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "商品交易時段：";
         // 
         // MarketTimes
         // 
         this.MarketTimes.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.MarketTimes.Appearance.Options.UseBackColor = true;
         this.MarketTimes.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.MarketTimes.CheckOnClick = true;
         this.MarketTimes.HorzScrollStep = 3;
         this.MarketTimes.ItemHeight = 28;
         this.MarketTimes.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(1, "Group1(13:45)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(5, "Group2(16:15)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(7, "Group3(18:15)")});
         this.MarketTimes.Location = new System.Drawing.Point(35, 30);
         this.MarketTimes.LookAndFeel.SkinName = "Office 2013";
         this.MarketTimes.LookAndFeel.UseDefaultLookAndFeel = false;
         this.MarketTimes.Name = "MarketTimes";
         this.MarketTimes.SelectionMode = System.Windows.Forms.SelectionMode.None;
         this.MarketTimes.Size = new System.Drawing.Size(143, 86);
         this.MarketTimes.TabIndex = 25;
         // 
         // txtDate1
         // 
         this.txtDate1.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate1.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate1.EditValue = "2018/12/01";
         this.txtDate1.EnterMoveNextControl = true;
         this.txtDate1.Location = new System.Drawing.Point(182, 29);
         this.txtDate1.MenuManager = this.ribbonControl;
         this.txtDate1.Name = "txtDate1";
         this.txtDate1.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate1.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate1.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate1.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate1.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate1.Size = new System.Drawing.Size(144, 26);
         this.txtDate1.TabIndex = 24;
         this.txtDate1.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // ETCSelect
         // 
         this.ETCSelect.Location = new System.Drawing.Point(182, 124);
         this.ETCSelect.Name = "ETCSelect";
         this.ETCSelect.Properties.Appearance.BackColor = System.Drawing.Color.White;
         this.ETCSelect.Properties.Appearance.Options.UseBackColor = true;
         this.ETCSelect.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ETCSelect.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ETCSelect.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ETCSelect.Properties.DropDownRows = 5;
         this.ETCSelect.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ETCSelect.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ETCSelect.Properties.NullText = "";
         this.ETCSelect.Properties.PopupSizeable = false;
         this.ETCSelect.Size = new System.Drawing.Size(144, 26);
         this.ETCSelect.TabIndex = 17;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label2.ForeColor = System.Drawing.Color.Black;
         this.label2.Location = new System.Drawing.Point(28, 127);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(106, 21);
         this.label2.TabIndex = 18;
         this.label2.Text = "含ETC VSR：";
         // 
         // txtDate5
         // 
         this.txtDate5.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate5.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate5.EditValue = "2018/12/01";
         this.txtDate5.EnterMoveNextControl = true;
         this.txtDate5.Location = new System.Drawing.Point(182, 58);
         this.txtDate5.MenuManager = this.ribbonControl;
         this.txtDate5.Name = "txtDate5";
         this.txtDate5.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate5.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate5.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate5.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate5.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate5.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate5.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate5.Size = new System.Drawing.Size(144, 26);
         this.txtDate5.TabIndex = 22;
         this.txtDate5.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtDate7
         // 
         this.txtDate7.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate7.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate7.EditValue = "2018/12/01";
         this.txtDate7.EnterMoveNextControl = true;
         this.txtDate7.Location = new System.Drawing.Point(182, 88);
         this.txtDate7.MenuManager = this.ribbonControl;
         this.txtDate7.Name = "txtDate7";
         this.txtDate7.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate7.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate7.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate7.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate7.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate7.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate7.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate7.Size = new System.Drawing.Size(144, 26);
         this.txtDate7.TabIndex = 20;
         this.txtDate7.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // ExportShow
         // 
         this.ExportShow.AutoSize = true;
         this.ExportShow.ForeColor = System.Drawing.Color.Navy;
         this.ExportShow.Location = new System.Drawing.Point(37, 282);
         this.ExportShow.Name = "ExportShow";
         this.ExportShow.Size = new System.Drawing.Size(56, 21);
         this.ExportShow.TabIndex = 14;
         this.ExportShow.Text = "label1";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label1.ForeColor = System.Drawing.Color.Black;
         this.label1.Location = new System.Drawing.Point(37, 49);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(138, 21);
         this.label1.TabIndex = 16;
         this.label1.Text = "保證金調整狀況：";
         // 
         // ddlAdjType
         // 
         this.ddlAdjType.Location = new System.Drawing.Point(180, 46);
         this.ddlAdjType.Name = "ddlAdjType";
         this.ddlAdjType.Properties.Appearance.BackColor = System.Drawing.Color.White;
         this.ddlAdjType.Properties.Appearance.Options.UseBackColor = true;
         this.ddlAdjType.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlAdjType.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlAdjType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlAdjType.Properties.DropDownRows = 5;
         this.ddlAdjType.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlAdjType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlAdjType.Properties.NullText = "";
         this.ddlAdjType.Properties.PopupSizeable = false;
         this.ddlAdjType.Size = new System.Drawing.Size(144, 26);
         this.ddlAdjType.TabIndex = 7;
         this.ddlAdjType.EditValueChanged += new System.EventHandler(this.ddlAdjType_EditValueChanged);
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12/01";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(180, 81);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(144, 26);
         this.txtDate.TabIndex = 15;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblDate.ForeColor = System.Drawing.Color.Black;
         this.lblDate.Location = new System.Drawing.Point(37, 84);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(58, 21);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "日期：";
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panel1.Controls.Add(this.grpxDescription);
         this.panel1.Location = new System.Drawing.Point(30, 30);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(601, 360);
         this.panel1.TabIndex = 14;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(84)))), ((int)(((byte)(150)))));
         this.label3.Location = new System.Drawing.Point(30, 393);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(89, 20);
         this.label3.TabIndex = 15;
         this.label3.Text = "操作說明：";
         // 
         // pictureBox1
         // 
         this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
         this.pictureBox1.Location = new System.Drawing.Point(30, 417);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(601, 151);
         this.pictureBox1.TabIndex = 16;
         this.pictureBox1.TabStop = false;
         // 
         // W40030
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(801, 660);
         this.Controls.Add(this.panelControl1);
         this.Name = "W40030";
         this.Text = "40030";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.MarketTimes)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ETCSelect.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate5.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate7.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlAdjType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private System.Windows.Forms.GroupBox grpxDescription;
      private System.Windows.Forms.Label lblDate;
      private System.Windows.Forms.Label ExportShow;
      private DevExpress.XtraEditors.LookUpEdit ddlAdjType;
      private System.Windows.Forms.Label label1;
      public BaseGround.Widget.TextDateEdit txtDate;
      private System.Windows.Forms.Label label2;
      private DevExpress.XtraEditors.LookUpEdit ETCSelect;
      public BaseGround.Widget.TextDateEdit txtDate1;
      public BaseGround.Widget.TextDateEdit txtDate5;
      public BaseGround.Widget.TextDateEdit txtDate7;
      private System.Windows.Forms.GroupBox groupBox1;
      private DevExpress.XtraEditors.CheckedListBoxControl MarketTimes;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.PictureBox pictureBox1;
      private System.Windows.Forms.Label label3;
   }
}