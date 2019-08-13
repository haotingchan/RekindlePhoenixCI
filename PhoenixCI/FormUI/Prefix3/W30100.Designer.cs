namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30100 {
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
         this.lblProcessing = new System.Windows.Forms.Label();
         this.grpxDescription = new System.Windows.Forms.GroupBox();
         this.label5 = new System.Windows.Forms.Label();
         this.dwFcmIn = new DevExpress.XtraEditors.LookUpEdit();
         this.dwFcmKs = new DevExpress.XtraEditors.LookUpEdit();
         this.label4 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.rdgMarketCode = new DevExpress.XtraEditors.RadioGroup();
         this.lblDate = new System.Windows.Forms.Label();
         this.panelControl = new DevExpress.XtraEditors.PanelControl();
         this.txtEDate = new BaseGround.Widget.TextDateEdit();
         this.txtSDate = new BaseGround.Widget.TextDateEdit();
         this.label6 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwFcmIn.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dwFcmKs.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rdgMarketCode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
         this.panelControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.panelControl);
         this.panParent.Size = new System.Drawing.Size(955, 645);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(955, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // lblProcessing
         // 
         this.lblProcessing.AutoSize = true;
         this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
         this.lblProcessing.Location = new System.Drawing.Point(11, 282);
         this.lblProcessing.MaximumSize = new System.Drawing.Size(259, 120);
         this.lblProcessing.Name = "lblProcessing";
         this.lblProcessing.Size = new System.Drawing.Size(85, 20);
         this.lblProcessing.TabIndex = 24;
         this.lblProcessing.Text = "開始轉檔...";
         this.lblProcessing.Visible = false;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.grpxDescription.Controls.Add(this.txtEDate);
         this.grpxDescription.Controls.Add(this.label5);
         this.grpxDescription.Controls.Add(this.txtSDate);
         this.grpxDescription.Controls.Add(this.label6);
         this.grpxDescription.Controls.Add(this.dwFcmIn);
         this.grpxDescription.Controls.Add(this.dwFcmKs);
         this.grpxDescription.Controls.Add(this.label4);
         this.grpxDescription.Controls.Add(this.label2);
         this.grpxDescription.Controls.Add(this.label3);
         this.grpxDescription.Controls.Add(this.rdgMarketCode);
         this.grpxDescription.Controls.Add(this.lblDate);
         this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
         this.grpxDescription.Location = new System.Drawing.Point(15, 15);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(417, 259);
         this.grpxDescription.TabIndex = 23;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label5.ForeColor = System.Drawing.Color.Navy;
         this.label5.Location = new System.Drawing.Point(128, 212);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(138, 21);
         this.label5.TabIndex = 20;
         this.label5.Text = "（空白代表全部）";
         // 
         // dwFcmIn
         // 
         this.dwFcmIn.Location = new System.Drawing.Point(138, 182);
         this.dwFcmIn.MenuManager = this.ribbonControl;
         this.dwFcmIn.Name = "dwFcmIn";
         this.dwFcmIn.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dwFcmIn.Size = new System.Drawing.Size(228, 26);
         this.dwFcmIn.TabIndex = 19;
         // 
         // dwFcmKs
         // 
         this.dwFcmKs.Location = new System.Drawing.Point(138, 137);
         this.dwFcmKs.MenuManager = this.ribbonControl;
         this.dwFcmKs.Name = "dwFcmKs";
         this.dwFcmKs.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dwFcmKs.Size = new System.Drawing.Size(228, 26);
         this.dwFcmKs.TabIndex = 18;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label4.Location = new System.Drawing.Point(35, 185);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(106, 21);
         this.label4.TabIndex = 17;
         this.label4.Text = "使用期貨商：";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label2.Location = new System.Drawing.Point(35, 140);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(106, 21);
         this.label2.TabIndex = 16;
         this.label2.Text = "ＫＳ期貨商：";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label3.Location = new System.Drawing.Point(35, 95);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(90, 21);
         this.label3.TabIndex = 15;
         this.label3.Text = "交易時段：";
         // 
         // rdgMarketCode
         // 
         this.rdgMarketCode.EditValue = "%";
         this.rdgMarketCode.Location = new System.Drawing.Point(138, 87);
         this.rdgMarketCode.MenuManager = this.ribbonControl;
         this.rdgMarketCode.Name = "rdgMarketCode";
         this.rdgMarketCode.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.rdgMarketCode.Properties.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.rdgMarketCode.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.rdgMarketCode.Properties.Appearance.Options.UseBackColor = true;
         this.rdgMarketCode.Properties.Appearance.Options.UseFont = true;
         this.rdgMarketCode.Properties.Appearance.Options.UseForeColor = true;
         this.rdgMarketCode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.rdgMarketCode.Properties.Columns = 3;
         this.rdgMarketCode.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("%", "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0%", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1%", "盤後")});
         this.rdgMarketCode.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.rdgMarketCode.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.rdgMarketCode.Size = new System.Drawing.Size(228, 35);
         this.rdgMarketCode.TabIndex = 14;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.lblDate.Location = new System.Drawing.Point(35, 50);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(58, 21);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "日期：";
         // 
         // panelControl
         // 
         this.panelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panelControl.Appearance.Options.UseBackColor = true;
         this.panelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelControl.Controls.Add(this.grpxDescription);
         this.panelControl.Controls.Add(this.lblProcessing);
         this.panelControl.Location = new System.Drawing.Point(30, 30);
         this.panelControl.Margin = new System.Windows.Forms.Padding(15);
         this.panelControl.Name = "panelControl";
         this.panelControl.Size = new System.Drawing.Size(449, 330);
         this.panelControl.TabIndex = 25;
         // 
         // txtEDate
         // 
         this.txtEDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEDate.EditValue = "2018/12/01";
         this.txtEDate.EnterMoveNextControl = true;
         this.txtEDate.Location = new System.Drawing.Point(266, 47);
         this.txtEDate.MenuManager = this.ribbonControl;
         this.txtEDate.Name = "txtEDate";
         this.txtEDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtEDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtEDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEDate.Size = new System.Drawing.Size(100, 26);
         this.txtEDate.TabIndex = 36;
         this.txtEDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtSDate
         // 
         this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtSDate.EditValue = "2018/12/01";
         this.txtSDate.EnterMoveNextControl = true;
         this.txtSDate.Location = new System.Drawing.Point(138, 47);
         this.txtSDate.MenuManager = this.ribbonControl;
         this.txtSDate.Name = "txtSDate";
         this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtSDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtSDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtSDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtSDate.Size = new System.Drawing.Size(100, 26);
         this.txtSDate.TabIndex = 35;
         this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(240, 50);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(26, 21);
         this.label6.TabIndex = 37;
         this.label6.Text = "～";
         // 
         // W30100
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(955, 675);
         this.Name = "W30100";
         this.Text = "W30100";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwFcmIn.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dwFcmKs.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rdgMarketCode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
         this.panelControl.ResumeLayout(false);
         this.panelControl.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.RadioGroup rdgMarketCode;
        private DevExpress.XtraEditors.LookUpEdit dwFcmIn;
        private DevExpress.XtraEditors.LookUpEdit dwFcmKs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.PanelControl panelControl;
      private BaseGround.Widget.TextDateEdit txtEDate;
      private BaseGround.Widget.TextDateEdit txtSDate;
      private System.Windows.Forms.Label label6;
   }
}