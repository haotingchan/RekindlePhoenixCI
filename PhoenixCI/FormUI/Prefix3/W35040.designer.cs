namespace PhoenixCI.FormUI.Prefix3
{
    partial class W35040
    {
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
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.ddlScCode = new DevExpress.XtraEditors.LookUpEdit();
         this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
         this.txtKind1 = new DevExpress.XtraEditors.TextEdit();
         this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
         this.txtAftEndYM = new BaseGround.Widget.TextDateEdit();
         this.label4 = new System.Windows.Forms.Label();
         this.txtAftStartYM = new BaseGround.Widget.TextDateEdit();
         this.label5 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ddlScCode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtKind1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEndYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftStartYM.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(511, 371);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(511, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(511, 371);
         this.panelControl1.TabIndex = 0;
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
         this.r_frame.Size = new System.Drawing.Size(455, 310);
         this.r_frame.TabIndex = 83;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 260);
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
         this.panFilter.Controls.Add(this.ddlScCode);
         this.panFilter.Controls.Add(this.textEdit1);
         this.panFilter.Controls.Add(this.txtKind1);
         this.panFilter.Controls.Add(this.gbMarket);
         this.panFilter.Controls.Add(this.txtAftEndYM);
         this.panFilter.Controls.Add(this.label4);
         this.panFilter.Controls.Add(this.txtAftStartYM);
         this.panFilter.Controls.Add(this.label5);
         this.panFilter.Controls.Add(this.label7);
         this.panFilter.Controls.Add(this.label6);
         this.panFilter.Controls.Add(this.label8);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(415, 235);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // ddlScCode
         // 
         this.ddlScCode.Location = new System.Drawing.Point(122, 177);
         this.ddlScCode.Name = "ddlScCode";
         this.ddlScCode.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlScCode.Properties.Appearance.Options.UseBackColor = true;
         this.ddlScCode.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlScCode.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlScCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlScCode.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name1", "全部")});
         this.ddlScCode.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlScCode.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlScCode.Properties.NullText = "";
         this.ddlScCode.Properties.PopupSizeable = false;
         this.ddlScCode.Size = new System.Drawing.Size(150, 26);
         this.ddlScCode.TabIndex = 84;
         // 
         // textEdit1
         // 
         this.textEdit1.EditValue = "0000";
         this.textEdit1.Location = new System.Drawing.Point(122, 132);
         this.textEdit1.MenuManager = this.ribbonControl;
         this.textEdit1.Name = "textEdit1";
         this.textEdit1.Size = new System.Drawing.Size(100, 26);
         this.textEdit1.TabIndex = 85;
         // 
         // txtKind1
         // 
         this.txtKind1.EditValue = "9999";
         this.txtKind1.Location = new System.Drawing.Point(270, 132);
         this.txtKind1.MenuManager = this.ribbonControl;
         this.txtKind1.Name = "txtKind1";
         this.txtKind1.Size = new System.Drawing.Size(100, 26);
         this.txtKind1.TabIndex = 84;
         // 
         // gbMarket
         // 
         this.gbMarket.EditValue = "rb_market_0";
         this.gbMarket.Location = new System.Drawing.Point(122, 37);
         this.gbMarket.Name = "gbMarket";
         this.gbMarket.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbMarket.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.gbMarket.Properties.Appearance.Options.UseBackColor = true;
         this.gbMarket.Properties.Appearance.Options.UseForeColor = true;
         this.gbMarket.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.gbMarket.Properties.Columns = 3;
         this.gbMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_0", "標的"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_All", "全部")});
         this.gbMarket.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.gbMarket.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbMarket.Size = new System.Drawing.Size(197, 35);
         this.gbMarket.TabIndex = 5;
         // 
         // txtAftEndYM
         // 
         this.txtAftEndYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftEndYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtAftEndYM.EditValue = "2018/12/01";
         this.txtAftEndYM.EnterMoveNextControl = true;
         this.txtAftEndYM.Location = new System.Drawing.Point(270, 87);
         this.txtAftEndYM.MenuManager = this.ribbonControl;
         this.txtAftEndYM.Name = "txtAftEndYM";
         this.txtAftEndYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftEndYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftEndYM.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAftEndYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtAftEndYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAftEndYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtAftEndYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftEndYM.Size = new System.Drawing.Size(100, 26);
         this.txtAftEndYM.TabIndex = 2;
         this.txtAftEndYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.ForeColor = System.Drawing.Color.Black;
         this.label4.Location = new System.Drawing.Point(37, 180);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(58, 21);
         this.label4.TabIndex = 37;
         this.label4.Text = "面額：";
         // 
         // txtAftStartYM
         // 
         this.txtAftStartYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftStartYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtAftStartYM.EditValue = "2018/12/01";
         this.txtAftStartYM.EnterMoveNextControl = true;
         this.txtAftStartYM.Location = new System.Drawing.Point(122, 87);
         this.txtAftStartYM.MenuManager = this.ribbonControl;
         this.txtAftStartYM.Name = "txtAftStartYM";
         this.txtAftStartYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftStartYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftStartYM.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAftStartYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtAftStartYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAftStartYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtAftStartYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftStartYM.Size = new System.Drawing.Size(100, 26);
         this.txtAftStartYM.TabIndex = 1;
         this.txtAftStartYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(230, 90);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(42, 21);
         this.label5.TabIndex = 6;
         this.label5.Text = "迄：";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.ForeColor = System.Drawing.Color.Black;
         this.label7.Location = new System.Drawing.Point(37, 135);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(90, 21);
         this.label7.TabIndex = 10;
         this.label7.Text = "股票代號：";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(224, 135);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(48, 21);
         this.label6.TabIndex = 10;
         this.label6.Text = "TO：";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.ForeColor = System.Drawing.Color.Black;
         this.label8.Location = new System.Drawing.Point(37, 90);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(90, 21);
         this.label8.TabIndex = 9;
         this.label8.Text = "日  期  起：";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.ForeColor = System.Drawing.Color.Black;
         this.label9.Location = new System.Drawing.Point(37, 45);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(58, 21);
         this.label9.TabIndex = 2;
         this.label9.Text = "商品：";
         // 
         // W35040
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(511, 401);
         this.Controls.Add(this.panelControl1);
         this.Name = "W35040";
         this.Text = "35040";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ddlScCode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtKind1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEndYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftStartYM.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      protected DevExpress.XtraEditors.RadioGroup gbMarket;
      private BaseGround.Widget.TextDateEdit txtAftEndYM;
      private System.Windows.Forms.Label label4;
      private BaseGround.Widget.TextDateEdit txtAftStartYM;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label9;
      private DevExpress.XtraEditors.TextEdit textEdit1;
      private DevExpress.XtraEditors.TextEdit txtKind1;
      private DevExpress.XtraEditors.LookUpEdit ddlScCode;
   }
}