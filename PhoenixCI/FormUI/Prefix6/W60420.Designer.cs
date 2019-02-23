﻿namespace PhoenixCI.FormUI.Prefix6 {
    partial class W60420 {
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
            this.txtStartDate = new PhoenixCI.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEndDate = new PhoenixCI.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblIndex = new System.Windows.Forms.Label();
            this.dw_index = new DevExpress.XtraEditors.LookUpEdit();
            this.lblCheck = new System.Windows.Forms.Label();
            this.cbx_1 = new System.Windows.Forms.CheckBox();
            this.cbx_5 = new System.Windows.Forms.CheckBox();
            this.cbx_4 = new System.Windows.Forms.CheckBox();
            this.cbx_3 = new System.Windows.Forms.CheckBox();
            this.cbx_2 = new System.Windows.Forms.CheckBox();
            this.grpxTest = new System.Windows.Forms.GroupBox();
            this.sle_cond1 = new System.Windows.Forms.TextBox();
            this.sle_cond2 = new System.Windows.Forms.TextBox();
            this.sle_cond3 = new System.Windows.Forms.TextBox();
            this.sle_cond4_1 = new System.Windows.Forms.TextBox();
            this.sle_cond5_2 = new System.Windows.Forms.TextBox();
            this.sle_cond4_2 = new System.Windows.Forms.TextBox();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dw_index.Properties)).BeginInit();
            this.grpxTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.grpxTest);
            this.panParent.Controls.Add(this.lblProcessing);
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Size = new System.Drawing.Size(919, 554);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(919, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(41, 448);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 13;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.Controls.Add(this.cbx_2);
            this.grpxDescription.Controls.Add(this.cbx_3);
            this.grpxDescription.Controls.Add(this.cbx_4);
            this.grpxDescription.Controls.Add(this.cbx_5);
            this.grpxDescription.Controls.Add(this.cbx_1);
            this.grpxDescription.Controls.Add(this.lblCheck);
            this.grpxDescription.Controls.Add(this.dw_index);
            this.grpxDescription.Controls.Add(this.lblIndex);
            this.grpxDescription.Controls.Add(this.txtStartDate);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.txtEndDate);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Location = new System.Drawing.Point(45, 41);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(582, 404);
            this.grpxDescription.TabIndex = 12;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtStartDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtStartDate.EditValue = "2018/12";
            this.txtStartDate.EnterMoveNextControl = true;
            this.txtStartDate.Location = new System.Drawing.Point(132, 91);
            this.txtStartDate.MenuManager = this.ribbonControl;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Size = new System.Drawing.Size(118, 26);
            this.txtStartDate.TabIndex = 2;
            this.txtStartDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(256, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "～";
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEndDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtEndDate.EditValue = "2018/12";
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(287, 91);
            this.txtEndDate.MenuManager = this.ribbonControl;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Size = new System.Drawing.Size(118, 26);
            this.txtEndDate.TabIndex = 3;
            this.txtEndDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(69, 94);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 20);
            this.lblDate.TabIndex = 12;
            this.lblDate.Text = "日期：";
            // 
            // lblIndex
            // 
            this.lblIndex.AutoSize = true;
            this.lblIndex.BackColor = System.Drawing.Color.Transparent;
            this.lblIndex.Location = new System.Drawing.Point(69, 48);
            this.lblIndex.Name = "lblIndex";
            this.lblIndex.Size = new System.Drawing.Size(57, 20);
            this.lblIndex.TabIndex = 14;
            this.lblIndex.Text = "指數：";
            // 
            // dw_index
            // 
            this.dw_index.Location = new System.Drawing.Point(132, 45);
            this.dw_index.MenuManager = this.ribbonControl;
            this.dw_index.Name = "dw_index";
            this.dw_index.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dw_index.Size = new System.Drawing.Size(178, 26);
            this.dw_index.TabIndex = 1;
            // 
            // lblCheck
            // 
            this.lblCheck.AutoSize = true;
            this.lblCheck.BackColor = System.Drawing.Color.Transparent;
            this.lblCheck.Location = new System.Drawing.Point(69, 155);
            this.lblCheck.Name = "lblCheck";
            this.lblCheck.Size = new System.Drawing.Size(89, 20);
            this.lblCheck.TabIndex = 16;
            this.lblCheck.Text = "檢核標準：";
            // 
            // cbx_1
            // 
            this.cbx_1.AutoSize = true;
            this.cbx_1.Checked = true;
            this.cbx_1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbx_1.Location = new System.Drawing.Point(132, 187);
            this.cbx_1.Name = "cbx_1";
            this.cbx_1.Size = new System.Drawing.Size(152, 24);
            this.cbx_1.TabIndex = 4;
            this.cbx_1.Text = "1. 成份股檔數≧10";
            this.cbx_1.UseVisualStyleBackColor = true;
            // 
            // cbx_5
            // 
            this.cbx_5.AutoSize = true;
            this.cbx_5.Checked = true;
            this.cbx_5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbx_5.Location = new System.Drawing.Point(132, 312);
            this.cbx_5.Name = "cbx_5";
            this.cbx_5.Size = new System.Drawing.Size(404, 64);
            this.cbx_5.TabIndex = 8;
            this.cbx_5.Text = "\r\n5. 最低25%權重之成份股，檔數低於15檔，\r\n    過去半年每日合計成交值之平均值＞5,000萬美元」";
            this.cbx_5.UseVisualStyleBackColor = true;
            // 
            // cbx_4
            // 
            this.cbx_4.AutoSize = true;
            this.cbx_4.Checked = true;
            this.cbx_4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbx_4.Location = new System.Drawing.Point(132, 261);
            this.cbx_4.Name = "cbx_4";
            this.cbx_4.Size = new System.Drawing.Size(404, 64);
            this.cbx_4.TabIndex = 7;
            this.cbx_4.Text = "\r\n4. 最低25%權重之成份股，檔數在15檔(含)以上，\r\n    過去半年每日合計成交值之平均值＞3,000萬美元」";
            this.cbx_4.UseVisualStyleBackColor = true;
            // 
            // cbx_3
            // 
            this.cbx_3.AutoSize = true;
            this.cbx_3.Checked = true;
            this.cbx_3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbx_3.Location = new System.Drawing.Point(132, 247);
            this.cbx_3.Name = "cbx_3";
            this.cbx_3.Size = new System.Drawing.Size(278, 24);
            this.cbx_3.TabIndex = 6;
            this.cbx_3.Text = "3. 權重前五大成份股合計權重≦60%";
            this.cbx_3.UseVisualStyleBackColor = true;
            // 
            // cbx_2
            // 
            this.cbx_2.AutoSize = true;
            this.cbx_2.Checked = true;
            this.cbx_2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbx_2.Location = new System.Drawing.Point(132, 217);
            this.cbx_2.Name = "cbx_2";
            this.cbx_2.Size = new System.Drawing.Size(246, 24);
            this.cbx_2.TabIndex = 5;
            this.cbx_2.Text = "2. 權重最大之成份股權重≦30%";
            this.cbx_2.UseVisualStyleBackColor = true;
            // 
            // grpxTest
            // 
            this.grpxTest.Controls.Add(this.sle_cond4_2);
            this.grpxTest.Controls.Add(this.sle_cond5_2);
            this.grpxTest.Controls.Add(this.sle_cond4_1);
            this.grpxTest.Controls.Add(this.sle_cond3);
            this.grpxTest.Controls.Add(this.sle_cond2);
            this.grpxTest.Controls.Add(this.sle_cond1);
            this.grpxTest.Location = new System.Drawing.Point(665, 186);
            this.grpxTest.Name = "grpxTest";
            this.grpxTest.Size = new System.Drawing.Size(202, 231);
            this.grpxTest.TabIndex = 14;
            this.grpxTest.TabStop = false;
            this.grpxTest.Text = "測試";
            this.grpxTest.Visible = false;
            // 
            // sle_cond1
            // 
            this.sle_cond1.Location = new System.Drawing.Point(31, 28);
            this.sle_cond1.Name = "sle_cond1";
            this.sle_cond1.Size = new System.Drawing.Size(62, 29);
            this.sle_cond1.TabIndex = 0;
            this.sle_cond1.Text = "10";
            // 
            // sle_cond2
            // 
            this.sle_cond2.Location = new System.Drawing.Point(31, 63);
            this.sle_cond2.Name = "sle_cond2";
            this.sle_cond2.Size = new System.Drawing.Size(62, 29);
            this.sle_cond2.TabIndex = 1;
            this.sle_cond2.Text = "0.3";
            // 
            // sle_cond3
            // 
            this.sle_cond3.Location = new System.Drawing.Point(31, 98);
            this.sle_cond3.Name = "sle_cond3";
            this.sle_cond3.Size = new System.Drawing.Size(62, 29);
            this.sle_cond3.TabIndex = 2;
            this.sle_cond3.Text = "0.6";
            // 
            // sle_cond4_1
            // 
            this.sle_cond4_1.Location = new System.Drawing.Point(31, 133);
            this.sle_cond4_1.Name = "sle_cond4_1";
            this.sle_cond4_1.Size = new System.Drawing.Size(62, 29);
            this.sle_cond4_1.TabIndex = 3;
            this.sle_cond4_1.Text = "15";
            // 
            // sle_cond5_2
            // 
            this.sle_cond5_2.Location = new System.Drawing.Point(31, 184);
            this.sle_cond5_2.Name = "sle_cond5_2";
            this.sle_cond5_2.Size = new System.Drawing.Size(79, 29);
            this.sle_cond5_2.TabIndex = 4;
            this.sle_cond5_2.Text = "5000";
            // 
            // sle_cond4_2
            // 
            this.sle_cond4_2.Location = new System.Drawing.Point(99, 133);
            this.sle_cond4_2.Name = "sle_cond4_2";
            this.sle_cond4_2.Size = new System.Drawing.Size(76, 29);
            this.sle_cond4_2.TabIndex = 5;
            this.sle_cond4_2.Text = "3000";
            // 
            // W60420
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 584);
            this.Name = "W60420";
            this.Text = "W60420";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dw_index.Properties)).EndInit();
            this.grpxTest.ResumeLayout(false);
            this.grpxTest.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private Widget.TextDateEdit txtStartDate;
        private System.Windows.Forms.Label label1;
        private Widget.TextDateEdit txtEndDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.CheckBox cbx_2;
        private System.Windows.Forms.CheckBox cbx_3;
        private System.Windows.Forms.CheckBox cbx_4;
        private System.Windows.Forms.CheckBox cbx_5;
        private System.Windows.Forms.CheckBox cbx_1;
        private System.Windows.Forms.Label lblCheck;
        private DevExpress.XtraEditors.LookUpEdit dw_index;
        private System.Windows.Forms.Label lblIndex;
        private System.Windows.Forms.GroupBox grpxTest;
        private System.Windows.Forms.TextBox sle_cond4_2;
        private System.Windows.Forms.TextBox sle_cond5_2;
        private System.Windows.Forms.TextBox sle_cond4_1;
        private System.Windows.Forms.TextBox sle_cond3;
        private System.Windows.Forms.TextBox sle_cond2;
        private System.Windows.Forms.TextBox sle_cond1;
    }
}