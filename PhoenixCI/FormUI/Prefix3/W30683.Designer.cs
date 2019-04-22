namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30683 {
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
         this.txtSecondMon = new DevExpress.XtraEditors.TextEdit();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.label3 = new System.Windows.Forms.Label();
         this.txtStartDate = new BaseGround.Widget.TextDateEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.lblDate = new System.Windows.Forms.Label();
         this.txtFirstMon = new DevExpress.XtraEditors.TextEdit();
         this.labMsg = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtSecondMon.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFirstMon.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.labMsg);
         this.panParent.Controls.Add(this.panFilter);
         this.panParent.Size = new System.Drawing.Size(628, 409);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(628, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.txtSecondMon);
         this.panFilter.Controls.Add(this.txtEndDate);
         this.panFilter.Controls.Add(this.label3);
         this.panFilter.Controls.Add(this.txtStartDate);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.lblDate);
         this.panFilter.Controls.Add(this.txtFirstMon);
         this.panFilter.Location = new System.Drawing.Point(34, 39);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(409, 169);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtSecondMon
         // 
         this.txtSecondMon.EditValue = "";
         this.txtSecondMon.Location = new System.Drawing.Point(189, 115);
         this.txtSecondMon.MenuManager = this.ribbonControl;
         this.txtSecondMon.Name = "txtSecondMon";
         this.txtSecondMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtSecondMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtSecondMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtSecondMon.Size = new System.Drawing.Size(64, 26);
         this.txtSecondMon.TabIndex = 12;
         this.txtSecondMon.Visible = false;
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndDate.EditValue = "2018/12";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(237, 40);
         this.txtEndDate.MenuManager = this.ribbonControl;
         this.txtEndDate.Name = "txtEndDate";
         this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate.TabIndex = 12;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(257, 86);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(115, 20);
         this.label3.TabIndex = 11;
         this.label3.Text = "(空白代表全部)";
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartDate.EditValue = "2018/12";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(100, 40);
         this.txtStartDate.MenuManager = this.ribbonControl;
         this.txtStartDate.Name = "txtStartDate";
         this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartDate.Size = new System.Drawing.Size(100, 26);
         this.txtStartDate.TabIndex = 11;
         this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(37, 86);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(146, 20);
         this.label2.TabIndex = 9;
         this.label2.Text = "到期月序：第1支腳";
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
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Location = new System.Drawing.Point(37, 46);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(57, 20);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "日期：";
         // 
         // txtFirstMon
         // 
         this.txtFirstMon.EditValue = "";
         this.txtFirstMon.Location = new System.Drawing.Point(189, 83);
         this.txtFirstMon.MenuManager = this.ribbonControl;
         this.txtFirstMon.Name = "txtFirstMon";
         this.txtFirstMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtFirstMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtFirstMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtFirstMon.Size = new System.Drawing.Size(64, 26);
         this.txtFirstMon.TabIndex = 10;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(30, 211);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // W30683
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(628, 439);
         this.Name = "W30683";
         this.Text = "W30683";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtSecondMon.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFirstMon.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox panFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private DevExpress.XtraEditors.TextEdit txtFirstMon;
      private DevExpress.XtraEditors.TextEdit txtSecondMon;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
   }
}