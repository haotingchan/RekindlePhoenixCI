﻿namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30017 {
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
         this.label4 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.txtSDate = new BaseGround.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.cbxAfter = new System.Windows.Forms.CheckBox();
         this.panelControl = new DevExpress.XtraEditors.PanelControl();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
         this.panelControl.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.panelControl);
         this.panParent.Controls.Add(this.cbxAfter);
         this.panParent.Size = new System.Drawing.Size(780, 420);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(780, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // lblProcessing
         // 
         this.lblProcessing.AutoSize = true;
         this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
         this.lblProcessing.Location = new System.Drawing.Point(15, 235);
         this.lblProcessing.MaximumSize = new System.Drawing.Size(535, 120);
         this.lblProcessing.Name = "lblProcessing";
         this.lblProcessing.Size = new System.Drawing.Size(85, 20);
         this.lblProcessing.TabIndex = 20;
         this.lblProcessing.Text = "開始轉檔...";
         this.lblProcessing.Visible = false;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.grpxDescription.Controls.Add(this.label4);
         this.grpxDescription.Controls.Add(this.label3);
         this.grpxDescription.Controls.Add(this.label2);
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.txtSDate);
         this.grpxDescription.Controls.Add(this.lblDate);
         this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
         this.grpxDescription.Location = new System.Drawing.Point(20, 15);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(535, 210);
         this.grpxDescription.TabIndex = 19;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.label4.ForeColor = System.Drawing.Color.Red;
         this.label4.Location = new System.Drawing.Point(220, 144);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(58, 21);
         this.label4.TabIndex = 11;
         this.label4.Text = "前年度";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label3.Location = new System.Drawing.Point(55, 144);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(337, 20);
         this.label3.TabIndex = 10;
         this.label3.Text = "輸入當年度預估量，及 前年度 現貨平均成交值";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label2.Location = new System.Drawing.Point(37, 121);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(467, 20);
         this.label2.TabIndex = 9;
         this.label2.Text = "(2)跨年度時請先「20430－各商品年度預估日均量維護(交易部)」";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label1.Location = new System.Drawing.Point(37, 92);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(393, 20);
         this.label1.TabIndex = 8;
         this.label1.Text = "(1)請先完成「20110－每日各商品現貨指數資料輸入」";
         // 
         // txtSDate
         // 
         this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtSDate.EditValue = "2018/12/ 01";
         this.txtSDate.EnterMoveNextControl = true;
         this.txtSDate.Location = new System.Drawing.Point(91, 42);
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
         this.txtSDate.TabIndex = 7;
         this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.ForeColor = System.Drawing.Color.Black;
         this.lblDate.Location = new System.Drawing.Point(37, 45);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(58, 21);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "日期：";
         // 
         // cbxAfter
         // 
         this.cbxAfter.AutoSize = true;
         this.cbxAfter.Location = new System.Drawing.Point(623, 87);
         this.cbxAfter.Name = "cbxAfter";
         this.cbxAfter.Size = new System.Drawing.Size(140, 24);
         this.cbxAfter.TabIndex = 21;
         this.cbxAfter.Text = "批次作業結束後";
         this.cbxAfter.UseVisualStyleBackColor = true;
         // 
         // panelControl
         // 
         this.panelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panelControl.Appearance.Options.UseBackColor = true;
         this.panelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelControl.Controls.Add(this.lblProcessing);
         this.panelControl.Controls.Add(this.grpxDescription);
         this.panelControl.Location = new System.Drawing.Point(30, 30);
         this.panelControl.Margin = new System.Windows.Forms.Padding(15);
         this.panelControl.Name = "panelControl";
         this.panelControl.Size = new System.Drawing.Size(575, 285);
         this.panelControl.TabIndex = 22;
         // 
         // W30017
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(780, 450);
         this.Name = "W30017";
         this.Text = "W30017";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
         this.panelControl.ResumeLayout(false);
         this.panelControl.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private BaseGround.Widget.TextDateEdit txtSDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.CheckBox cbxAfter;
        private DevExpress.XtraEditors.PanelControl panelControl;
    }
}