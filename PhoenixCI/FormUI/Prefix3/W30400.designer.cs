namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30400 {
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
         this.label2 = new System.Windows.Forms.Label();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtKindId = new DevExpress.XtraEditors.TextEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.txtMon = new BaseGround.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtKindId.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtMon.Properties)).BeginInit();
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
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label2.ForeColor = System.Drawing.Color.Navy;
         this.label2.Location = new System.Drawing.Point(222, 90);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(161, 20);
         this.label2.TabIndex = 5;
         this.label2.Text = "(%代表全部股票期貨)";
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
         this.r_frame.Size = new System.Drawing.Size(450, 220);
         this.r_frame.TabIndex = 79;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 170);
         this.labMsg.MaximumSize = new System.Drawing.Size(360, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 80;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.Controls.Add(this.txtKindId);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.txtMon);
         this.panFilter.Controls.Add(this.lblDate);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(410, 145);
         this.panFilter.TabIndex = 76;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtKindId
         // 
         this.txtKindId.EditValue = "%";
         this.txtKindId.Location = new System.Drawing.Point(155, 87);
         this.txtKindId.MenuManager = this.ribbonControl;
         this.txtKindId.Name = "txtKindId";
         this.txtKindId.Properties.Appearance.Options.UseTextOptions = true;
         this.txtKindId.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtKindId.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtKindId.Size = new System.Drawing.Size(61, 26);
         this.txtKindId.TabIndex = 12;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label1.ForeColor = System.Drawing.Color.Black;
         this.label1.Location = new System.Drawing.Point(37, 90);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(122, 21);
         this.label1.TabIndex = 80;
         this.label1.Text = "股票期貨代號：";
         // 
         // txtMon
         // 
         this.txtMon.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtMon.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtMon.EditValue = "2018/12";
         this.txtMon.EnterMoveNextControl = true;
         this.txtMon.Location = new System.Drawing.Point(92, 42);
         this.txtMon.MenuManager = this.ribbonControl;
         this.txtMon.Name = "txtMon";
         this.txtMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtMon.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtMon.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtMon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtMon.Properties.Mask.ShowPlaceHolders = false;
         this.txtMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtMon.Size = new System.Drawing.Size(100, 26);
         this.txtMon.TabIndex = 0;
         this.txtMon.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.lblDate.ForeColor = System.Drawing.Color.Black;
         this.lblDate.Location = new System.Drawing.Point(37, 45);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(58, 21);
         this.lblDate.TabIndex = 83;
         this.lblDate.Text = "月份：";
         // 
         // W30400
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(645, 435);
         this.Name = "W30400";
         this.Text = "W30400";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtKindId.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtMon.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
      private System.Windows.Forms.Label label2;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.GroupBox panFilter;
      private BaseGround.Widget.TextDateEdit txtMon;
      private System.Windows.Forms.Label lblDate;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.TextEdit txtKindId;
      private System.Windows.Forms.Label labMsg;
   }
}