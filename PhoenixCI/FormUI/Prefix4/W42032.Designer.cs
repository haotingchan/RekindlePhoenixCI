namespace PhoenixCI.FormUI.Prefix4 {
    partial class W42032 {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSID = new System.Windows.Forms.TextBox();
            this.rdgCondition = new DevExpress.XtraEditors.RadioGroup();
            this.txtSDate = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdgCondition.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.lblProcessing);
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Size = new System.Drawing.Size(912, 630);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(912, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(47, 397);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 24;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.grpxDescription.Controls.Add(this.groupBox1);
            this.grpxDescription.Controls.Add(this.txtSDate);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
            this.grpxDescription.Location = new System.Drawing.Point(51, 45);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(404, 349);
            this.grpxDescription.TabIndex = 23;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "交易量資料轉出Excel 檔";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSID);
            this.groupBox1.Controls.Add(this.rdgCondition);
            this.groupBox1.Location = new System.Drawing.Point(28, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(346, 228);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查詢條件";
            // 
            // txtSID
            // 
            this.txtSID.Location = new System.Drawing.Point(152, 168);
            this.txtSID.Name = "txtSID";
            this.txtSID.Size = new System.Drawing.Size(100, 29);
            this.txtSID.TabIndex = 25;
            // 
            // rdgCondition
            // 
            this.rdgCondition.EditValue = "F";
            this.rdgCondition.Location = new System.Drawing.Point(24, 38);
            this.rdgCondition.MenuManager = this.ribbonControl;
            this.rdgCondition.Name = "rdgCondition";
            this.rdgCondition.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.rdgCondition.Properties.Appearance.Options.UseBackColor = true;
            this.rdgCondition.Properties.Columns = 1;
            this.rdgCondition.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.rdgCondition.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("F", "股票期貨上市標的證券"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("O", "股票選擇權上市標的證券"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ALL", "股票期貨暨選擇權上市標的證券"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ADJ", "本日保證金級距異動標的證券"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("SID", "選擇單一證券")});
            this.rdgCondition.Size = new System.Drawing.Size(295, 168);
            this.rdgCondition.TabIndex = 1;
            // 
            // txtSDate
            // 
            this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtSDate.EditValue = "2018/12/01";
            this.txtSDate.EnterMoveNextControl = true;
            this.txtSDate.Location = new System.Drawing.Point(87, 42);
            this.txtSDate.MenuManager = this.ribbonControl;
            this.txtSDate.Name = "txtSDate";
            this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSDate.Size = new System.Drawing.Size(100, 26);
            this.txtSDate.TabIndex = 2;
            this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.lblDate.Location = new System.Drawing.Point(24, 45);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(58, 21);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "日期：";
            // 
            // W42032
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 660);
            this.Name = "W42032";
            this.Text = "W42032";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdgCondition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.RadioGroup rdgCondition;
        private BaseGround.Widget.TextDateEdit txtSDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox txtSID;
    }
}