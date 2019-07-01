namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30055 {
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
         this.gcMsg = new DevExpress.XtraGrid.GridControl();
         this.gvMsg = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.SheetName = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SheetSubTitle = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SubMsg = new DevExpress.XtraGrid.Columns.GridColumn();
         this.Msg = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panel1 = new System.Windows.Forms.Panel();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtSDate = new BaseGround.Widget.TextDateEdit();
         this.cbxNews = new System.Windows.Forms.CheckBox();
         this.cbxTJF = new System.Windows.Forms.CheckBox();
         this.ddlType = new DevExpress.XtraEditors.LookUpEdit();
         this.labType = new System.Windows.Forms.Label();
         this.labDate = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMsg)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMsg)).BeginInit();
         this.panel1.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlType.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.panel1);
         this.panParent.Controls.Add(this.gcMsg);
         this.panParent.Size = new System.Drawing.Size(858, 730);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(858, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // gcMsg
         // 
         this.gcMsg.Location = new System.Drawing.Point(12, 218);
         this.gcMsg.MainView = this.gvMsg;
         this.gcMsg.MenuManager = this.ribbonControl;
         this.gcMsg.Name = "gcMsg";
         this.gcMsg.Size = new System.Drawing.Size(831, 500);
         this.gcMsg.TabIndex = 4;
         this.gcMsg.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMsg});
         // 
         // gvMsg
         // 
         this.gvMsg.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.gvMsg.Appearance.Empty.Options.UseBackColor = true;
         this.gvMsg.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.gvMsg.Appearance.Row.Options.UseBackColor = true;
         this.gvMsg.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.gvMsg.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.SheetName,
            this.SheetSubTitle,
            this.SubMsg,
            this.Msg});
         this.gvMsg.GridControl = this.gcMsg;
         this.gvMsg.Name = "gvMsg";
         // 
         // SheetName
         // 
         this.SheetName.Caption = "Sheet";
         this.SheetName.FieldName = "SheetName";
         this.SheetName.Name = "SheetName";
         this.SheetName.Visible = true;
         this.SheetName.VisibleIndex = 0;
         this.SheetName.Width = 89;
         // 
         // SheetSubTitle
         // 
         this.SheetSubTitle.Caption = "小項";
         this.SheetSubTitle.FieldName = "SheetSubTitle";
         this.SheetSubTitle.Name = "SheetSubTitle";
         this.SheetSubTitle.Visible = true;
         this.SheetSubTitle.VisibleIndex = 1;
         this.SheetSubTitle.Width = 400;
         // 
         // SubMsg
         // 
         this.SubMsg.Caption = " ";
         this.SubMsg.FieldName = "SubMsg";
         this.SubMsg.Name = "SubMsg";
         this.SubMsg.Visible = true;
         this.SubMsg.VisibleIndex = 2;
         this.SubMsg.Width = 54;
         // 
         // Msg
         // 
         this.Msg.Caption = "結果";
         this.Msg.FieldName = "Msg";
         this.Msg.Name = "Msg";
         this.Msg.Visible = true;
         this.Msg.VisibleIndex = 3;
         this.Msg.Width = 100;
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.labMsg);
         this.panel1.Controls.Add(this.panFilter);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(12, 12);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(834, 200);
         this.panel1.TabIndex = 26;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(30, 172);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(184, 16);
         this.labMsg.TabIndex = 27;
         this.labMsg.Text = "訊息：資料轉出中........";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Controls.Add(this.txtSDate);
         this.panFilter.Controls.Add(this.cbxNews);
         this.panFilter.Controls.Add(this.cbxTJF);
         this.panFilter.Controls.Add(this.ddlType);
         this.panFilter.Controls.Add(this.labType);
         this.panFilter.Controls.Add(this.labDate);
         this.panFilter.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(3, 3);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(377, 166);
         this.panFilter.TabIndex = 26;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtSDate
         // 
         this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtSDate.EditValue = "2018/12/01";
         this.txtSDate.EnterMoveNextControl = true;
         this.txtSDate.Location = new System.Drawing.Point(89, 28);
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
         this.txtSDate.TabIndex = 0;
         this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // cbxNews
         // 
         this.cbxNews.AutoSize = true;
         this.cbxNews.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.cbxNews.ForeColor = System.Drawing.Color.Black;
         this.cbxNews.Location = new System.Drawing.Point(30, 144);
         this.cbxNews.Name = "cbxNews";
         this.cbxNews.Size = new System.Drawing.Size(127, 20);
         this.cbxNews.TabIndex = 3;
         this.cbxNews.Text = "Email寄送報社";
         this.cbxNews.UseVisualStyleBackColor = true;
         // 
         // cbxTJF
         // 
         this.cbxTJF.AutoSize = true;
         this.cbxTJF.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.cbxTJF.ForeColor = System.Drawing.Color.Black;
         this.cbxTJF.Location = new System.Drawing.Point(30, 114);
         this.cbxTJF.Name = "cbxTJF";
         this.cbxTJF.Size = new System.Drawing.Size(175, 20);
         this.cbxTJF.TabIndex = 2;
         this.cbxTJF.Text = "Email寄送日本交易所";
         this.cbxTJF.UseVisualStyleBackColor = true;
         // 
         // ddlType
         // 
         this.ddlType.Location = new System.Drawing.Point(89, 72);
         this.ddlType.MenuManager = this.ribbonControl;
         this.ddlType.Name = "ddlType";
         this.ddlType.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
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
         this.labType.Location = new System.Drawing.Point(26, 75);
         this.labType.Name = "labType";
         this.labType.Size = new System.Drawing.Size(59, 16);
         this.labType.TabIndex = 14;
         this.labType.Text = "盤別：";
         // 
         // labDate
         // 
         this.labDate.AutoSize = true;
         this.labDate.ForeColor = System.Drawing.Color.Black;
         this.labDate.Location = new System.Drawing.Point(26, 34);
         this.labDate.Name = "labDate";
         this.labDate.Size = new System.Drawing.Size(59, 16);
         this.labDate.TabIndex = 12;
         this.labDate.Text = "日期：";
         // 
         // W30055
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(858, 760);
         this.Name = "W30055";
         this.Text = "W30055";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMsg)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMsg)).EndInit();
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlType.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gcMsg;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMsg;
        private DevExpress.XtraGrid.Columns.GridColumn SheetName;
        private DevExpress.XtraGrid.Columns.GridColumn SheetSubTitle;
        private DevExpress.XtraGrid.Columns.GridColumn SubMsg;
        private DevExpress.XtraGrid.Columns.GridColumn Msg;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.CheckBox cbxNews;
      private System.Windows.Forms.CheckBox cbxTJF;
      private DevExpress.XtraEditors.LookUpEdit ddlType;
      private System.Windows.Forms.Label labType;
      private System.Windows.Forms.Label labDate;
      private BaseGround.Widget.TextDateEdit txtSDate;
   }
}