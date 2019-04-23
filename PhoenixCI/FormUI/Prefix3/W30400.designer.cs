﻿namespace PhoenixCI.FormUI.Prefix3 {
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
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.label2 = new System.Windows.Forms.Label();
         this.txtKindId = new DevExpress.XtraEditors.TextEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.lblDate = new System.Windows.Forms.Label();
         this.labMsg = new System.Windows.Forms.Label();
         this.txtMon = new BaseGround.Widget.TextDateEdit();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtKindId.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtMon.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.labMsg);
         this.panParent.Controls.Add(this.panFilter);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.txtMon);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.txtKindId);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.lblDate);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(34, 39);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(404, 163);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label2.ForeColor = System.Drawing.Color.Navy;
         this.label2.Location = new System.Drawing.Point(220, 85);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(161, 20);
         this.label2.TabIndex = 5;
         this.label2.Text = "(%代表全部股票期貨)";
         // 
         // txtKindId
         // 
         this.txtKindId.EditValue = "%";
         this.txtKindId.Location = new System.Drawing.Point(153, 82);
         this.txtKindId.MenuManager = this.ribbonControl;
         this.txtKindId.Name = "txtKindId";
         this.txtKindId.Properties.Appearance.Options.UseTextOptions = true;
         this.txtKindId.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtKindId.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtKindId.Size = new System.Drawing.Size(61, 26);
         this.txtKindId.TabIndex = 4;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label1.ForeColor = System.Drawing.Color.Black;
         this.label1.Location = new System.Drawing.Point(37, 85);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(122, 21);
         this.label1.TabIndex = 3;
         this.label1.Text = "股票期貨代號：";
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblDate.ForeColor = System.Drawing.Color.Black;
         this.lblDate.Location = new System.Drawing.Point(37, 46);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(58, 21);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "月份：";
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(30, 205);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(69, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "轉檔中...";
         this.labMsg.Visible = false;
         // 
         // txtMon
         // 
         this.txtMon.DateTimeValue = new System.DateTime(((long)(0)));
         this.txtMon.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtMon.EditValue = "2018/12";
         this.txtMon.EnterMoveNextControl = true;
         this.txtMon.Location = new System.Drawing.Point(96, 43);
         this.txtMon.MenuManager = this.ribbonControl;
         this.txtMon.Name = "txtMon";
         this.txtMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtMon.Properties.Mask.EditMask = "yyyy/MM";
         this.txtMon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtMon.Properties.Mask.ShowPlaceHolders = false;
         this.txtMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtMon.Size = new System.Drawing.Size(100, 26);
         this.txtMon.TabIndex = 11;
         this.txtMon.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // W30400
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Name = "W30400";
         this.Text = "W30400";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtKindId.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtMon.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox panFilter;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.Label label2;
      private DevExpress.XtraEditors.TextEdit txtKindId;
      private System.Windows.Forms.Label label1;
      private BaseGround.Widget.TextDateEdit txtMon;
   }
}