namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30414 {
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
         this.txtEndMon = new BaseGround.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.txtStartMon = new BaseGround.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.labMsg = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMon.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMon.Properties)).BeginInit();
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
         this.panFilter.Controls.Add(this.txtEndMon);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.txtStartMon);
         this.panFilter.Controls.Add(this.lblDate);
         this.panFilter.Location = new System.Drawing.Point(34, 39);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(404, 119);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtEndMon
         // 
         this.txtEndMon.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndMon.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndMon.EditValue = "2018/12";
         this.txtEndMon.EnterMoveNextControl = true;
         this.txtEndMon.Location = new System.Drawing.Point(224, 43);
         this.txtEndMon.MenuManager = this.ribbonControl;
         this.txtEndMon.Name = "txtEndMon";
         this.txtEndMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndMon.Properties.Mask.EditMask = "yyyy/MM";
         this.txtEndMon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndMon.Size = new System.Drawing.Size(100, 26);
         this.txtEndMon.TabIndex = 4;
         this.txtEndMon.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(197, 46);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(21, 20);
         this.label1.TabIndex = 3;
         this.label1.Text = "~";
         // 
         // txtStartMon
         // 
         this.txtStartMon.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartMon.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartMon.EditValue = "2018/12";
         this.txtStartMon.EnterMoveNextControl = true;
         this.txtStartMon.Location = new System.Drawing.Point(91, 43);
         this.txtStartMon.MenuManager = this.ribbonControl;
         this.txtStartMon.Name = "txtStartMon";
         this.txtStartMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartMon.Properties.Mask.EditMask = "yyyy/MM";
         this.txtStartMon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartMon.Size = new System.Drawing.Size(100, 26);
         this.txtStartMon.TabIndex = 0;
         this.txtStartMon.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Location = new System.Drawing.Point(37, 46);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(57, 20);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "月份：";
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(30, 161);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(69, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "轉檔中...";
         this.labMsg.Visible = false;
         // 
         // W30414
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Name = "W30414";
         this.Text = "W30414";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMon.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMon.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox panFilter;
        private System.Windows.Forms.Label lblDate;
        private BaseGround.Widget.TextDateEdit txtStartMon;
        private System.Windows.Forms.Label labMsg;
      private BaseGround.Widget.TextDateEdit txtEndMon;
      private System.Windows.Forms.Label label1;
   }
}