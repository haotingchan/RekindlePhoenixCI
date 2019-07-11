namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30418 {
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
            this.dwKindId = new DevExpress.XtraEditors.LookUpEdit();
            this.gbPC = new DevExpress.XtraEditors.RadioGroup();
            this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEndDate = new BaseGround.Widget.TextDateEdit();
            this.txtStartDate = new BaseGround.Widget.TextDateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
            this.r_frame.SuspendLayout();
            this.panFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbPC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.r_frame);
            this.panParent.Size = new System.Drawing.Size(645, 405);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(645, 30);
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
            this.r_frame.Size = new System.Drawing.Size(520, 283);
            this.r_frame.TabIndex = 81;
            // 
            // labMsg
            // 
            this.labMsg.AutoSize = true;
            this.labMsg.ForeColor = System.Drawing.Color.Blue;
            this.labMsg.Location = new System.Drawing.Point(15, 250);
            this.labMsg.MaximumSize = new System.Drawing.Size(360, 120);
            this.labMsg.Name = "labMsg";
            this.labMsg.Size = new System.Drawing.Size(85, 20);
            this.labMsg.TabIndex = 80;
            this.labMsg.Text = "開始轉檔...";
            this.labMsg.Visible = false;
            // 
            // panFilter
            // 
            this.panFilter.AutoSize = true;
            this.panFilter.Controls.Add(this.dwKindId);
            this.panFilter.Controls.Add(this.gbPC);
            this.panFilter.Controls.Add(this.gbMarket);
            this.panFilter.Controls.Add(this.label5);
            this.panFilter.Controls.Add(this.label2);
            this.panFilter.Controls.Add(this.label1);
            this.panFilter.Controls.Add(this.txtEndDate);
            this.panFilter.Controls.Add(this.txtStartDate);
            this.panFilter.Controls.Add(this.label3);
            this.panFilter.Controls.Add(this.label4);
            this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.panFilter.ForeColor = System.Drawing.Color.Navy;
            this.panFilter.Location = new System.Drawing.Point(20, 15);
            this.panFilter.Name = "panFilter";
            this.panFilter.Size = new System.Drawing.Size(478, 220);
            this.panFilter.TabIndex = 6;
            this.panFilter.TabStop = false;
            this.panFilter.Text = "請輸入交易日期";
            // 
            // dwKindId
            // 
            this.dwKindId.Location = new System.Drawing.Point(109, 116);
            this.dwKindId.Name = "dwKindId";
            this.dwKindId.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dwKindId.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.dwKindId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dwKindId.Properties.LookAndFeel.SkinName = "The Bezier";
            this.dwKindId.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.dwKindId.Properties.NullText = "";
            this.dwKindId.Properties.PopupSizeable = false;
            this.dwKindId.Size = new System.Drawing.Size(274, 26);
            this.dwKindId.TabIndex = 4;
            this.dwKindId.EditValueChanged += new System.EventHandler(this.dwKindId_EditValueChanged);
            // 
            // gbPC
            // 
            this.gbPC.EditValue = "N";
            this.gbPC.Location = new System.Drawing.Point(109, 149);
            this.gbPC.MenuManager = this.ribbonControl;
            this.gbPC.Name = "gbPC";
            this.gbPC.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.gbPC.Properties.Appearance.Options.UseBackColor = true;
            this.gbPC.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gbPC.Properties.Columns = 2;
            this.gbPC.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("N", "不分計"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Y", "分計")});
            this.gbPC.Size = new System.Drawing.Size(195, 43);
            this.gbPC.TabIndex = 5;
            // 
            // gbMarket
            // 
            this.gbMarket.Location = new System.Drawing.Point(109, 72);
            this.gbMarket.MenuManager = this.ribbonControl;
            this.gbMarket.Name = "gbMarket";
            this.gbMarket.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.gbMarket.Properties.Appearance.Options.UseBackColor = true;
            this.gbMarket.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gbMarket.Properties.Columns = 2;
            this.gbMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbTFXM", "現貨市場三大法人"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbFut", "期貨市場三大法人")});
            this.gbMarket.Size = new System.Drawing.Size(342, 43);
            this.gbMarket.TabIndex = 3;
            this.gbMarket.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(6, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 21);
            this.label5.TabIndex = 9;
            this.label5.Text = "買賣權分計：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(22, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "商品類別：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(54, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 21);
            this.label1.TabIndex = 7;
            this.label1.Text = "市場：";
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate.EditValue = "2018/12/01";
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(237, 42);
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
            this.txtEndDate.TabIndex = 2;
            this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate.EditValue = "2018/12/01";
            this.txtStartDate.EnterMoveNextControl = true;
            this.txtStartDate.Location = new System.Drawing.Point(109, 42);
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
            this.txtStartDate.TabIndex = 1;
            this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(211, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "～";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(54, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 21);
            this.label4.TabIndex = 2;
            this.label4.Text = "日期：";
            // 
            // W30418
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 435);
            this.Name = "W30418";
            this.Text = "W30418";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
            this.r_frame.ResumeLayout(false);
            this.r_frame.PerformLayout();
            this.panFilter.ResumeLayout(false);
            this.panFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwKindId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbPC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      #endregion

      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.RadioGroup gbPC;
        private DevExpress.XtraEditors.RadioGroup gbMarket;
        private DevExpress.XtraEditors.LookUpEdit dwKindId;
    }
}