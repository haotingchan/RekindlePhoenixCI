namespace PhoenixCI.FormUI.Prefix5 {
    partial class W55060 {
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
            this.txtToMonth = new BaseGround.Widget.TextDateEdit();
            this.txtFromMonth = new BaseGround.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtToMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromMonth.Properties)).BeginInit();
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
            this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.grpxDescription.Controls.Add(this.txtToMonth);
            this.grpxDescription.Controls.Add(this.txtFromMonth);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
            this.grpxDescription.Location = new System.Drawing.Point(34, 39);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(404, 119);
            this.grpxDescription.TabIndex = 7;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // txtToMonth
            // 
            this.txtToMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtToMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtToMonth.EditValue = "2018/12";
            this.txtToMonth.EnterMoveNextControl = true;
            this.txtToMonth.Location = new System.Drawing.Point(237, 43);
            this.txtToMonth.MenuManager = this.ribbonControl;
            this.txtToMonth.Name = "txtToMonth";
            this.txtToMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtToMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtToMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtToMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtToMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToMonth.Size = new System.Drawing.Size(100, 26);
            this.txtToMonth.TabIndex = 8;
            this.txtToMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtFromMonth
            // 
            this.txtFromMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtFromMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtFromMonth.EditValue = "2018/12";
            this.txtFromMonth.EnterMoveNextControl = true;
            this.txtFromMonth.Location = new System.Drawing.Point(100, 43);
            this.txtFromMonth.MenuManager = this.ribbonControl;
            this.txtFromMonth.Name = "txtFromMonth";
            this.txtFromMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtFromMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtFromMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtFromMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtFromMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromMonth.Size = new System.Drawing.Size(100, 26);
            this.txtFromMonth.TabIndex = 7;
            this.txtFromMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label1.Location = new System.Drawing.Point(206, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "～";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.lblDate.Location = new System.Drawing.Point(37, 46);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(58, 21);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "月份：";
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(30, 161);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 10;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // W55060
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 574);
            this.Name = "W55060";
            this.Text = "W55060";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtToMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromMonth.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDate;
        private BaseGround.Widget.TextDateEdit txtToMonth;
        private BaseGround.Widget.TextDateEdit txtFromMonth;
        private System.Windows.Forms.Label lblProcessing;
    }
}