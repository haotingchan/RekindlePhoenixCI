namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30660 {
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
         this.txtAllEymd = new BaseGround.Widget.TextDateEdit();
         this.txtAllSymd = new BaseGround.Widget.TextDateEdit();
         this.txtAftEymd = new BaseGround.Widget.TextDateEdit();
         this.txtPrevEymd = new BaseGround.Widget.TextDateEdit();
         this.txtAftSymd = new BaseGround.Widget.TextDateEdit();
         this.txtPrevSymd = new BaseGround.Widget.TextDateEdit();
         this.chkDetail = new System.Windows.Forms.CheckBox();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.label10 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllEymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllSymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftSymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevSymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(625, 325);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(625, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // txtAllEymd
         // 
         this.txtAllEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAllEymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtAllEymd.EditValue = "2018/12/01";
         this.txtAllEymd.EnterMoveNextControl = true;
         this.txtAllEymd.Location = new System.Drawing.Point(220, 132);
         this.txtAllEymd.MenuManager = this.ribbonControl;
         this.txtAllEymd.Name = "txtAllEymd";
         this.txtAllEymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAllEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAllEymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAllEymd.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtAllEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAllEymd.Properties.Mask.ShowPlaceHolders = false;
         this.txtAllEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAllEymd.Size = new System.Drawing.Size(100, 26);
         this.txtAllEymd.TabIndex = 81;
         this.txtAllEymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAllSymd
         // 
         this.txtAllSymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAllSymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtAllSymd.EditValue = "2018/12/01";
         this.txtAllSymd.EnterMoveNextControl = true;
         this.txtAllSymd.Location = new System.Drawing.Point(92, 132);
         this.txtAllSymd.MenuManager = this.ribbonControl;
         this.txtAllSymd.Name = "txtAllSymd";
         this.txtAllSymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAllSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAllSymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAllSymd.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtAllSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAllSymd.Properties.Mask.ShowPlaceHolders = false;
         this.txtAllSymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAllSymd.Size = new System.Drawing.Size(100, 26);
         this.txtAllSymd.TabIndex = 80;
         this.txtAllSymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAftEymd
         // 
         this.txtAftEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftEymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtAftEymd.EditValue = "2018/12/01";
         this.txtAftEymd.EnterMoveNextControl = true;
         this.txtAftEymd.Location = new System.Drawing.Point(220, 87);
         this.txtAftEymd.MenuManager = this.ribbonControl;
         this.txtAftEymd.Name = "txtAftEymd";
         this.txtAftEymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftEymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAftEymd.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtAftEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAftEymd.Properties.Mask.ShowPlaceHolders = false;
         this.txtAftEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftEymd.Size = new System.Drawing.Size(100, 26);
         this.txtAftEymd.TabIndex = 79;
         this.txtAftEymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtPrevEymd
         // 
         this.txtPrevEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevEymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtPrevEymd.EditValue = "2018/12/01";
         this.txtPrevEymd.EnterMoveNextControl = true;
         this.txtPrevEymd.Location = new System.Drawing.Point(220, 42);
         this.txtPrevEymd.MenuManager = this.ribbonControl;
         this.txtPrevEymd.Name = "txtPrevEymd";
         this.txtPrevEymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevEymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtPrevEymd.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtPrevEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtPrevEymd.Properties.Mask.ShowPlaceHolders = false;
         this.txtPrevEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevEymd.Size = new System.Drawing.Size(100, 26);
         this.txtPrevEymd.TabIndex = 77;
         this.txtPrevEymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAftSymd
         // 
         this.txtAftSymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftSymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtAftSymd.EditValue = "2018/12/01";
         this.txtAftSymd.EnterMoveNextControl = true;
         this.txtAftSymd.Location = new System.Drawing.Point(92, 87);
         this.txtAftSymd.MenuManager = this.ribbonControl;
         this.txtAftSymd.Name = "txtAftSymd";
         this.txtAftSymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftSymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAftSymd.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtAftSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAftSymd.Properties.Mask.ShowPlaceHolders = false;
         this.txtAftSymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftSymd.Size = new System.Drawing.Size(100, 26);
         this.txtAftSymd.TabIndex = 78;
         this.txtAftSymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtPrevSymd
         // 
         this.txtPrevSymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevSymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtPrevSymd.EditValue = "2018/12/01";
         this.txtPrevSymd.EnterMoveNextControl = true;
         this.txtPrevSymd.Location = new System.Drawing.Point(92, 42);
         this.txtPrevSymd.MenuManager = this.ribbonControl;
         this.txtPrevSymd.Name = "txtPrevSymd";
         this.txtPrevSymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevSymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtPrevSymd.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtPrevSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtPrevSymd.Properties.Mask.ShowPlaceHolders = false;
         this.txtPrevSymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevSymd.Size = new System.Drawing.Size(100, 26);
         this.txtPrevSymd.TabIndex = 76;
         this.txtPrevSymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // chkDetail
         // 
         this.chkDetail.AutoSize = true;
         this.chkDetail.ForeColor = System.Drawing.Color.Black;
         this.chkDetail.Location = new System.Drawing.Point(339, 134);
         this.chkDetail.Name = "chkDetail";
         this.chkDetail.Size = new System.Drawing.Size(157, 25);
         this.chkDetail.TabIndex = 17;
         this.chkDetail.Text = "產出每日明細資料";
         this.chkDetail.UseVisualStyleBackColor = true;
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
         this.r_frame.Size = new System.Drawing.Size(575, 265);
         this.r_frame.TabIndex = 81;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 215);
         this.labMsg.MaximumSize = new System.Drawing.Size(535, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 80;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.chkDetail);
         this.panFilter.Controls.Add(this.txtPrevEymd);
         this.panFilter.Controls.Add(this.label10);
         this.panFilter.Controls.Add(this.txtPrevSymd);
         this.panFilter.Controls.Add(this.txtAllEymd);
         this.panFilter.Controls.Add(this.txtAftSymd);
         this.panFilter.Controls.Add(this.txtAllSymd);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Controls.Add(this.label8);
         this.panFilter.Controls.Add(this.label7);
         this.panFilter.Controls.Add(this.txtAftEymd);
         this.panFilter.Controls.Add(this.label5);
         this.panFilter.Controls.Add(this.label6);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(535, 190);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(194, 135);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(26, 21);
         this.label10.TabIndex = 82;
         this.label10.Text = "～";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.ForeColor = System.Drawing.Color.Black;
         this.label9.Location = new System.Drawing.Point(37, 135);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(58, 21);
         this.label9.TabIndex = 81;
         this.label9.Text = "本週：";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(194, 90);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(26, 21);
         this.label8.TabIndex = 80;
         this.label8.Text = "～";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.ForeColor = System.Drawing.Color.Black;
         this.label7.Location = new System.Drawing.Point(37, 90);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(58, 21);
         this.label7.TabIndex = 9;
         this.label7.Text = "本週：";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(194, 45);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(26, 21);
         this.label5.TabIndex = 6;
         this.label5.Text = "～";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.ForeColor = System.Drawing.Color.Black;
         this.label6.Location = new System.Drawing.Point(37, 45);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(58, 21);
         this.label6.TabIndex = 2;
         this.label6.Text = "上週：";
         // 
         // W30660
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(625, 355);
         this.Name = "W30660";
         this.Text = "W30660";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllEymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllSymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftSymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevSymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
      private System.Windows.Forms.CheckBox chkDetail;
      private BaseGround.Widget.TextDateEdit txtAllEymd;
      private BaseGround.Widget.TextDateEdit txtAllSymd;
      private BaseGround.Widget.TextDateEdit txtAftEymd;
      private BaseGround.Widget.TextDateEdit txtPrevEymd;
      private BaseGround.Widget.TextDateEdit txtAftSymd;
      private BaseGround.Widget.TextDateEdit txtPrevSymd;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label6;
   }
}