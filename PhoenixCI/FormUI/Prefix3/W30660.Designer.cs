﻿namespace PhoenixCI.FormUI.Prefix3 {
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
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtAllEymd = new BaseGround.Widget.TextDateEdit();
         this.txtAllSymd = new BaseGround.Widget.TextDateEdit();
         this.txtAftEymd = new BaseGround.Widget.TextDateEdit();
         this.txtPrevEymd = new BaseGround.Widget.TextDateEdit();
         this.txtAftSymd = new BaseGround.Widget.TextDateEdit();
         this.txtPrevSymd = new BaseGround.Widget.TextDateEdit();
         this.chkDetail = new System.Windows.Forms.CheckBox();
         this.label4 = new System.Windows.Forms.Label();
         this.labDateAll = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.labDate2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.labDate1 = new System.Windows.Forms.Label();
         this.labMsg = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllEymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllSymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftSymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevSymd.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.labMsg);
         this.panParent.Controls.Add(this.panFilter);
         this.panParent.Size = new System.Drawing.Size(836, 544);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(836, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.txtAllEymd);
         this.panFilter.Controls.Add(this.txtAllSymd);
         this.panFilter.Controls.Add(this.txtAftEymd);
         this.panFilter.Controls.Add(this.txtPrevEymd);
         this.panFilter.Controls.Add(this.txtAftSymd);
         this.panFilter.Controls.Add(this.txtPrevSymd);
         this.panFilter.Controls.Add(this.chkDetail);
         this.panFilter.Controls.Add(this.label4);
         this.panFilter.Controls.Add(this.labDateAll);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.labDate2);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.labDate1);
         this.panFilter.Location = new System.Drawing.Point(34, 39);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(543, 177);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtAllEymd
         // 
         this.txtAllEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAllEymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtAllEymd.EditValue = "2018/12";
         this.txtAllEymd.EnterMoveNextControl = true;
         this.txtAllEymd.Location = new System.Drawing.Point(237, 121);
         this.txtAllEymd.MenuManager = this.ribbonControl;
         this.txtAllEymd.Name = "txtAllEymd";
         this.txtAllEymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAllEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAllEymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAllEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAllEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
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
         this.txtAllSymd.EditValue = "2018/12";
         this.txtAllSymd.EnterMoveNextControl = true;
         this.txtAllSymd.Location = new System.Drawing.Point(100, 121);
         this.txtAllSymd.MenuManager = this.ribbonControl;
         this.txtAllSymd.Name = "txtAllSymd";
         this.txtAllSymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAllSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAllSymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAllSymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAllSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
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
         this.txtAftEymd.EditValue = "2018/12";
         this.txtAftEymd.EnterMoveNextControl = true;
         this.txtAftEymd.Location = new System.Drawing.Point(237, 82);
         this.txtAftEymd.MenuManager = this.ribbonControl;
         this.txtAftEymd.Name = "txtAftEymd";
         this.txtAftEymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftEymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAftEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAftEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
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
         this.txtPrevEymd.EditValue = "2018/12";
         this.txtPrevEymd.EnterMoveNextControl = true;
         this.txtPrevEymd.Location = new System.Drawing.Point(237, 43);
         this.txtPrevEymd.MenuManager = this.ribbonControl;
         this.txtPrevEymd.Name = "txtPrevEymd";
         this.txtPrevEymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevEymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtPrevEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtPrevEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
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
         this.txtAftSymd.EditValue = "2018/12";
         this.txtAftSymd.EnterMoveNextControl = true;
         this.txtAftSymd.Location = new System.Drawing.Point(100, 82);
         this.txtAftSymd.MenuManager = this.ribbonControl;
         this.txtAftSymd.Name = "txtAftSymd";
         this.txtAftSymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftSymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtAftSymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAftSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
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
         this.txtPrevSymd.EditValue = "2018/12";
         this.txtPrevSymd.EnterMoveNextControl = true;
         this.txtPrevSymd.Location = new System.Drawing.Point(100, 43);
         this.txtPrevSymd.MenuManager = this.ribbonControl;
         this.txtPrevSymd.Name = "txtPrevSymd";
         this.txtPrevSymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevSymd.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtPrevSymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtPrevSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtPrevSymd.Properties.Mask.ShowPlaceHolders = false;
         this.txtPrevSymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevSymd.Size = new System.Drawing.Size(100, 26);
         this.txtPrevSymd.TabIndex = 76;
         this.txtPrevSymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // chkDetail
         // 
         this.chkDetail.AutoSize = true;
         this.chkDetail.Location = new System.Drawing.Point(356, 123);
         this.chkDetail.Name = "chkDetail";
         this.chkDetail.Size = new System.Drawing.Size(156, 24);
         this.chkDetail.TabIndex = 17;
         this.chkDetail.Text = "產出每日明細資料";
         this.chkDetail.UseVisualStyleBackColor = true;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(206, 121);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(25, 20);
         this.label4.TabIndex = 14;
         this.label4.Text = "～";
         // 
         // labDateAll
         // 
         this.labDateAll.AutoSize = true;
         this.labDateAll.Location = new System.Drawing.Point(37, 124);
         this.labDateAll.Name = "labDateAll";
         this.labDateAll.Size = new System.Drawing.Size(57, 20);
         this.labDateAll.TabIndex = 13;
         this.labDateAll.Text = "全期：";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(206, 82);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(25, 20);
         this.label2.TabIndex = 10;
         this.label2.Text = "～";
         // 
         // labDate2
         // 
         this.labDate2.AutoSize = true;
         this.labDate2.Location = new System.Drawing.Point(37, 85);
         this.labDate2.Name = "labDate2";
         this.labDate2.Size = new System.Drawing.Size(57, 20);
         this.labDate2.TabIndex = 9;
         this.labDate2.Text = "本週：";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(206, 43);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(25, 20);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // labDate1
         // 
         this.labDate1.AutoSize = true;
         this.labDate1.Location = new System.Drawing.Point(37, 46);
         this.labDate1.Name = "labDate1";
         this.labDate1.Size = new System.Drawing.Size(57, 20);
         this.labDate1.TabIndex = 2;
         this.labDate1.Text = "上週：";
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(30, 219);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // W30660
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Name = "W30660";
         this.Text = "W30660";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllEymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllSymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftSymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevSymd.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox panFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labDate1;
        private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label labDateAll;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label labDate2;
      private System.Windows.Forms.CheckBox chkDetail;
      private BaseGround.Widget.TextDateEdit txtAllEymd;
      private BaseGround.Widget.TextDateEdit txtAllSymd;
      private BaseGround.Widget.TextDateEdit txtAftEymd;
      private BaseGround.Widget.TextDateEdit txtPrevEymd;
      private BaseGround.Widget.TextDateEdit txtAftSymd;
      private BaseGround.Widget.TextDateEdit txtPrevSymd;
   }
}