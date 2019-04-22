namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30053 {
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
            this.panFilter = new System.Windows.Forms.GroupBox();
            this.cbxNews = new System.Windows.Forms.CheckBox();
            this.ddlType = new DevExpress.XtraEditors.LookUpEdit();
            this.labType = new System.Windows.Forms.Label();
            this.txtSDate = new BaseGround.Widget.TextDateEdit();
            this.labDate = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.panFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddlType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.lblProcessing);
            this.panParent.Controls.Add(this.panFilter);
            this.panParent.Size = new System.Drawing.Size(944, 615);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(944, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(59, 271);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 33;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // panFilter
            // 
            this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panFilter.Controls.Add(this.cbxNews);
            this.panFilter.Controls.Add(this.ddlType);
            this.panFilter.Controls.Add(this.labType);
            this.panFilter.Controls.Add(this.txtSDate);
            this.panFilter.Controls.Add(this.labDate);
            this.panFilter.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panFilter.ForeColor = System.Drawing.Color.Navy;
            this.panFilter.Location = new System.Drawing.Point(63, 58);
            this.panFilter.Name = "panFilter";
            this.panFilter.Size = new System.Drawing.Size(387, 210);
            this.panFilter.TabIndex = 32;
            this.panFilter.TabStop = false;
            this.panFilter.Text = "請輸入交易日期";
            // 
            // cbxNews
            // 
            this.cbxNews.AutoSize = true;
            this.cbxNews.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbxNews.ForeColor = System.Drawing.Color.Black;
            this.cbxNews.Location = new System.Drawing.Point(33, 147);
            this.cbxNews.Name = "cbxNews";
            this.cbxNews.Size = new System.Drawing.Size(127, 20);
            this.cbxNews.TabIndex = 15;
            this.cbxNews.Text = "Email寄送報社";
            this.cbxNews.UseVisualStyleBackColor = true;
            // 
            // ddlType
            // 
            this.ddlType.Location = new System.Drawing.Point(93, 99);
            this.ddlType.MenuManager = this.ribbonControl;
            this.ddlType.Name = "ddlType";
            this.ddlType.Properties.Appearance.BackColor = System.Drawing.Color.Silver;
            this.ddlType.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.ddlType.Properties.Appearance.Options.UseBackColor = true;
            this.ddlType.Properties.Appearance.Options.UseForeColor = true;
            this.ddlType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlType.Size = new System.Drawing.Size(150, 26);
            this.ddlType.TabIndex = 1;
            // 
            // labType
            // 
            this.labType.AutoSize = true;
            this.labType.ForeColor = System.Drawing.Color.Black;
            this.labType.Location = new System.Drawing.Point(30, 102);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(59, 16);
            this.labType.TabIndex = 14;
            this.labType.Text = "盤別：";
            // 
            // txtSDate
            // 
            this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtSDate.EditValue = "2018/12";
            this.txtSDate.EnterMoveNextControl = true;
            this.txtSDate.Location = new System.Drawing.Point(93, 58);
            this.txtSDate.MenuManager = this.ribbonControl;
            this.txtSDate.Name = "txtSDate";
            this.txtSDate.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtSDate.Properties.Appearance.Options.UseForeColor = true;
            this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSDate.Size = new System.Drawing.Size(100, 26);
            this.txtSDate.TabIndex = 0;
            this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.ForeColor = System.Drawing.Color.Black;
            this.labDate.Location = new System.Drawing.Point(30, 61);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(59, 16);
            this.labDate.TabIndex = 12;
            this.labDate.Text = "日期：";
            // 
            // W30053
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 645);
            this.Name = "W30053";
            this.Text = "W30053";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.panFilter.ResumeLayout(false);
            this.panFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddlType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox panFilter;
        private System.Windows.Forms.CheckBox cbxNews;
        private DevExpress.XtraEditors.LookUpEdit ddlType;
        private System.Windows.Forms.Label labType;
        private BaseGround.Widget.TextDateEdit txtSDate;
        private System.Windows.Forms.Label labDate;
    }
}