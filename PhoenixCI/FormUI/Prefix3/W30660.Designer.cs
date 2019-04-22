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
         this.grpxDescription = new System.Windows.Forms.GroupBox();
         this.chkDetail = new System.Windows.Forms.CheckBox();
         this.txtAllEymd = new BaseGround.Widget.TextDateEdit();
         this.txtAllSymd = new BaseGround.Widget.TextDateEdit();
         this.label4 = new System.Windows.Forms.Label();
         this.labDateAll = new System.Windows.Forms.Label();
         this.txtAftEymd = new BaseGround.Widget.TextDateEdit();
         this.txtAftSymd = new BaseGround.Widget.TextDateEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.labDate2 = new System.Windows.Forms.Label();
         this.txtPrevEymd = new BaseGround.Widget.TextDateEdit();
         this.txtPrevSymd = new BaseGround.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.labDate1 = new System.Windows.Forms.Label();
         this.lblProcessing = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllEymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllSymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftSymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevSymd.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.lblProcessing);
         this.panParent.Controls.Add(this.grpxDescription);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.Controls.Add(this.chkDetail);
         this.grpxDescription.Controls.Add(this.txtAllEymd);
         this.grpxDescription.Controls.Add(this.txtAllSymd);
         this.grpxDescription.Controls.Add(this.label4);
         this.grpxDescription.Controls.Add(this.labDateAll);
         this.grpxDescription.Controls.Add(this.txtAftEymd);
         this.grpxDescription.Controls.Add(this.txtAftSymd);
         this.grpxDescription.Controls.Add(this.label2);
         this.grpxDescription.Controls.Add(this.labDate2);
         this.grpxDescription.Controls.Add(this.txtPrevEymd);
         this.grpxDescription.Controls.Add(this.txtPrevSymd);
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.labDate1);
         this.grpxDescription.Location = new System.Drawing.Point(34, 39);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(543, 177);
         this.grpxDescription.TabIndex = 7;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
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
         // txtAllEymd
         // 
         this.txtAllEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAllEymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAllEymd.EditValue = "2018/12";
         this.txtAllEymd.EnterMoveNextControl = true;
         this.txtAllEymd.Location = new System.Drawing.Point(237, 121);
         this.txtAllEymd.MenuManager = this.ribbonControl;
         this.txtAllEymd.Name = "txtAllEymd";
         this.txtAllEymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAllEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAllEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAllEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtAllEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAllEymd.Size = new System.Drawing.Size(100, 26);
         this.txtAllEymd.TabIndex = 16;
         this.txtAllEymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAllSymd
         // 
         this.txtAllSymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAllSymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAllSymd.EditValue = "2018/12";
         this.txtAllSymd.EnterMoveNextControl = true;
         this.txtAllSymd.Location = new System.Drawing.Point(100, 121);
         this.txtAllSymd.MenuManager = this.ribbonControl;
         this.txtAllSymd.Name = "txtAllSymd";
         this.txtAllSymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAllSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAllSymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAllSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtAllSymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAllSymd.Size = new System.Drawing.Size(100, 26);
         this.txtAllSymd.TabIndex = 15;
         this.txtAllSymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
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
         // txtAftEymd
         // 
         this.txtAftEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftEymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAftEymd.EditValue = "2018/12";
         this.txtAftEymd.EnterMoveNextControl = true;
         this.txtAftEymd.Location = new System.Drawing.Point(237, 82);
         this.txtAftEymd.MenuManager = this.ribbonControl;
         this.txtAftEymd.Name = "txtAftEymd";
         this.txtAftEymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAftEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtAftEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftEymd.Size = new System.Drawing.Size(100, 26);
         this.txtAftEymd.TabIndex = 12;
         this.txtAftEymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAftSymd
         // 
         this.txtAftSymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftSymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAftSymd.EditValue = "2018/12";
         this.txtAftSymd.EnterMoveNextControl = true;
         this.txtAftSymd.Location = new System.Drawing.Point(100, 82);
         this.txtAftSymd.MenuManager = this.ribbonControl;
         this.txtAftSymd.Name = "txtAftSymd";
         this.txtAftSymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftSymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAftSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtAftSymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftSymd.Size = new System.Drawing.Size(100, 26);
         this.txtAftSymd.TabIndex = 11;
         this.txtAftSymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
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
         // txtPrevEymd
         // 
         this.txtPrevEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevEymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtPrevEymd.EditValue = "2018/12";
         this.txtPrevEymd.EnterMoveNextControl = true;
         this.txtPrevEymd.Location = new System.Drawing.Point(237, 43);
         this.txtPrevEymd.MenuManager = this.ribbonControl;
         this.txtPrevEymd.Name = "txtPrevEymd";
         this.txtPrevEymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtPrevEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtPrevEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevEymd.Size = new System.Drawing.Size(100, 26);
         this.txtPrevEymd.TabIndex = 8;
         this.txtPrevEymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtPrevSymd
         // 
         this.txtPrevSymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevSymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtPrevSymd.EditValue = "2018/12";
         this.txtPrevSymd.EnterMoveNextControl = true;
         this.txtPrevSymd.Location = new System.Drawing.Point(100, 43);
         this.txtPrevSymd.MenuManager = this.ribbonControl;
         this.txtPrevSymd.Name = "txtPrevSymd";
         this.txtPrevSymd.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevSymd.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtPrevSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtPrevSymd.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevSymd.Size = new System.Drawing.Size(100, 26);
         this.txtPrevSymd.TabIndex = 7;
         this.txtPrevSymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
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
         // lblProcessing
         // 
         this.lblProcessing.AutoSize = true;
         this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
         this.lblProcessing.Location = new System.Drawing.Point(30, 219);
         this.lblProcessing.Name = "lblProcessing";
         this.lblProcessing.Size = new System.Drawing.Size(85, 20);
         this.lblProcessing.TabIndex = 10;
         this.lblProcessing.Text = "開始轉檔...";
         this.lblProcessing.Visible = false;
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
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllEymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAllSymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftSymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevSymd.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labDate1;
        private BaseGround.Widget.TextDateEdit txtPrevEymd;
        private BaseGround.Widget.TextDateEdit txtPrevSymd;
        private System.Windows.Forms.Label lblProcessing;
      private BaseGround.Widget.TextDateEdit txtAllEymd;
      private BaseGround.Widget.TextDateEdit txtAllSymd;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label labDateAll;
      private BaseGround.Widget.TextDateEdit txtAftEymd;
      private BaseGround.Widget.TextDateEdit txtAftSymd;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label labDate2;
      private System.Windows.Forms.CheckBox chkDetail;
   }
}