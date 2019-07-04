namespace PhoenixCI.FormUI.Prefix5 {
    partial class W55050 {
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
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtEndMonth = new BaseGround.Widget.TextDateEdit();
         this.txtStartMonth = new BaseGround.Widget.TextDateEdit();
         this.rgpType = new DevExpress.XtraEditors.RadioGroup();
         this.labFcmNo = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.labDate = new System.Windows.Forms.Label();
         this.cbxFcmStartNo = new DevExpress.XtraEditors.LookUpEdit();
         this.cbxFcmEndNo = new DevExpress.XtraEditors.LookUpEdit();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rgpType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbxFcmStartNo.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbxFcmEndNo.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(810, 320);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(810, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
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
         this.r_frame.Size = new System.Drawing.Size(760, 260);
         this.r_frame.TabIndex = 80;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 210);
         this.labMsg.MaximumSize = new System.Drawing.Size(720, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 81;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.Controls.Add(this.txtEndMonth);
         this.panFilter.Controls.Add(this.txtStartMonth);
         this.panFilter.Controls.Add(this.rgpType);
         this.panFilter.Controls.Add(this.labFcmNo);
         this.panFilter.Controls.Add(this.label4);
         this.panFilter.Controls.Add(this.label5);
         this.panFilter.Controls.Add(this.labDate);
         this.panFilter.Controls.Add(this.cbxFcmStartNo);
         this.panFilter.Controls.Add(this.cbxFcmEndNo);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(720, 185);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtEndMonth
         // 
         this.txtEndMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndMonth.EditValue = "2018/12";
         this.txtEndMonth.EnterMoveNextControl = true;
         this.txtEndMonth.Location = new System.Drawing.Point(260, 37);
         this.txtEndMonth.MenuManager = this.ribbonControl;
         this.txtEndMonth.Name = "txtEndMonth";
         this.txtEndMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndMonth.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtEndMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtEndMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEndMonth.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndMonth.Size = new System.Drawing.Size(100, 26);
         this.txtEndMonth.TabIndex = 1;
         this.txtEndMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartMonth
         // 
         this.txtStartMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartMonth.EditValue = "2018/12";
         this.txtStartMonth.EnterMoveNextControl = true;
         this.txtStartMonth.Location = new System.Drawing.Point(137, 37);
         this.txtStartMonth.MenuManager = this.ribbonControl;
         this.txtStartMonth.Name = "txtStartMonth";
         this.txtStartMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartMonth.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtStartMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtStartMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtStartMonth.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartMonth.Size = new System.Drawing.Size(100, 26);
         this.txtStartMonth.TabIndex = 0;
         this.txtStartMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // rgpType
         // 
         this.rgpType.EditValue = "fcm";
         this.rgpType.Location = new System.Drawing.Point(137, 122);
         this.rgpType.Name = "rgpType";
         this.rgpType.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.rgpType.Properties.Appearance.Options.UseBackColor = true;
         this.rgpType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("fcm", "依期貨商別"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("product", "依商品別")});
         this.rgpType.Properties.LookAndFeel.SkinName = "Office 2013";
         this.rgpType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.rgpType.Size = new System.Drawing.Size(250, 40);
         this.rgpType.TabIndex = 4;
         // 
         // labFcmNo
         // 
         this.labFcmNo.AutoSize = true;
         this.labFcmNo.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.labFcmNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.labFcmNo.Location = new System.Drawing.Point(37, 85);
         this.labFcmNo.Name = "labFcmNo";
         this.labFcmNo.Size = new System.Drawing.Size(73, 20);
         this.labFcmNo.TabIndex = 16;
         this.labFcmNo.Text = "期貨商：";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(393, 85);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(22, 21);
         this.label4.TabIndex = 14;
         this.label4.Text = "~";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(239, 40);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(22, 21);
         this.label5.TabIndex = 11;
         this.label5.Text = "~";
         // 
         // labDate
         // 
         this.labDate.AutoSize = true;
         this.labDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.labDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.labDate.Location = new System.Drawing.Point(37, 40);
         this.labDate.Name = "labDate";
         this.labDate.Size = new System.Drawing.Size(57, 20);
         this.labDate.TabIndex = 2;
         this.labDate.Text = "月份：";
         // 
         // cbxFcmStartNo
         // 
         this.cbxFcmStartNo.Location = new System.Drawing.Point(137, 82);
         this.cbxFcmStartNo.Name = "cbxFcmStartNo";
         this.cbxFcmStartNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbxFcmStartNo.Properties.LookAndFeel.SkinName = "The Bezier";
         this.cbxFcmStartNo.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.cbxFcmStartNo.Size = new System.Drawing.Size(250, 26);
         this.cbxFcmStartNo.TabIndex = 2;
         // 
         // cbxFcmEndNo
         // 
         this.cbxFcmEndNo.Location = new System.Drawing.Point(417, 82);
         this.cbxFcmEndNo.Name = "cbxFcmEndNo";
         this.cbxFcmEndNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbxFcmEndNo.Properties.LookAndFeel.SkinName = "The Bezier";
         this.cbxFcmEndNo.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.cbxFcmEndNo.Size = new System.Drawing.Size(250, 26);
         this.cbxFcmEndNo.TabIndex = 3;
         // 
         // W55050
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(810, 350);
         this.Name = "W55050";
         this.Text = "W55050";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rgpType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbxFcmStartNo.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbxFcmEndNo.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

      #endregion

      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private BaseGround.Widget.TextDateEdit txtEndMonth;
      private BaseGround.Widget.TextDateEdit txtStartMonth;
      private DevExpress.XtraEditors.RadioGroup rgpType;
      private System.Windows.Forms.Label labFcmNo;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label labDate;
      private DevExpress.XtraEditors.LookUpEdit cbxFcmStartNo;
      private DevExpress.XtraEditors.LookUpEdit cbxFcmEndNo;
   }
}