namespace PhoenixCI.FormUI.Prefix4 {
    partial class W48010 {
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
         this.txtSDate = new BaseGround.Widget.TextDateEdit();
         this.gcExport = new DevExpress.XtraGrid.GridControl();
         this.gvExport = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
         this.labMemo = new System.Windows.Forms.Label();
         this.labMsg = new System.Windows.Forms.Label();
         this.ddlSort = new DevExpress.XtraEditors.LookUpEdit();
         this.ddlData = new DevExpress.XtraEditors.LookUpEdit();
         this.ddlKind = new DevExpress.XtraEditors.LookUpEdit();
         this.ddlSubType = new DevExpress.XtraEditors.LookUpEdit();
         this.labDate = new System.Windows.Forms.Label();
         this.labKind = new System.Windows.Forms.Label();
         this.labData = new System.Windows.Forms.Label();
         this.labSubType = new System.Windows.Forms.Label();
         this.labSort = new System.Windows.Forms.Label();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSort.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlData.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlKind.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSubType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.gcMain);
         this.panParent.Controls.Add(this.panFilter);
         this.panParent.Size = new System.Drawing.Size(1024, 580);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(1024, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Controls.Add(this.txtSDate);
         this.panFilter.Controls.Add(this.gcExport);
         this.panFilter.Controls.Add(this.labMemo);
         this.panFilter.Controls.Add(this.labMsg);
         this.panFilter.Controls.Add(this.ddlSort);
         this.panFilter.Controls.Add(this.ddlData);
         this.panFilter.Controls.Add(this.ddlKind);
         this.panFilter.Controls.Add(this.ddlSubType);
         this.panFilter.Controls.Add(this.labDate);
         this.panFilter.Controls.Add(this.labKind);
         this.panFilter.Controls.Add(this.labData);
         this.panFilter.Controls.Add(this.labSubType);
         this.panFilter.Controls.Add(this.labSort);
         this.panFilter.Dock = System.Windows.Forms.DockStyle.Top;
         this.panFilter.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(12, 12);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(1000, 195);
         this.panFilter.TabIndex = 25;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入查詢條件";
         // 
         // txtSDate
         // 
         this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtSDate.EditValue = "2018/12/01";
         this.txtSDate.EnterMoveNextControl = true;
         this.txtSDate.Location = new System.Drawing.Point(110, 28);
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
         this.txtSDate.TabIndex = 76;
         this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // gcExport
         // 
         this.gcExport.Location = new System.Drawing.Point(537, 66);
         this.gcExport.MainView = this.gvExport;
         this.gcExport.MenuManager = this.ribbonControl;
         this.gcExport.Name = "gcExport";
         this.gcExport.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit2});
         this.gcExport.Size = new System.Drawing.Size(451, 157);
         this.gcExport.TabIndex = 28;
         this.gcExport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExport});
         this.gcExport.Visible = false;
         // 
         // gvExport
         // 
         this.gvExport.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.gvExport.Appearance.Empty.Options.UseBackColor = true;
         this.gvExport.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Cyan;
         this.gvExport.Appearance.HeaderPanel.Options.UseBackColor = true;
         this.gvExport.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.gvExport.Appearance.Row.BorderColor = System.Drawing.Color.Black;
         this.gvExport.Appearance.Row.Options.UseBackColor = true;
         this.gvExport.Appearance.Row.Options.UseBorderColor = true;
         this.gvExport.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
         this.gvExport.GridControl = this.gcExport;
         this.gvExport.Name = "gvExport";
         this.gvExport.OptionsView.ShowGroupPanel = false;
         // 
         // repositoryItemCheckEdit2
         // 
         this.repositoryItemCheckEdit2.AutoWidth = true;
         this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
         this.repositoryItemCheckEdit2.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
         this.repositoryItemCheckEdit2.ValueChecked = "Y";
         this.repositoryItemCheckEdit2.ValueUnchecked = "N";
         // 
         // labMemo
         // 
         this.labMemo.AutoSize = true;
         this.labMemo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.labMemo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.labMemo.Location = new System.Drawing.Point(26, 135);
         this.labMemo.Name = "labMemo";
         this.labMemo.Size = new System.Drawing.Size(680, 16);
         this.labMemo.TabIndex = 21;
         this.labMemo.Text = "備    註：已下市契約之最小風險價格係數顯示空白；有效契約之最小風險價格係數不可為空白。";
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(26, 166);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(184, 16);
         this.labMsg.TabIndex = 26;
         this.labMsg.Text = "訊息：資料轉出中........";
         this.labMsg.Visible = false;
         // 
         // ddlSort
         // 
         this.ddlSort.Location = new System.Drawing.Point(333, 97);
         this.ddlSort.MenuManager = this.ribbonControl;
         this.ddlSort.Name = "ddlSort";
         this.ddlSort.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.ddlSort.Properties.Appearance.Options.UseForeColor = true;
         this.ddlSort.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlSort.Properties.DropDownRows = 2;
         this.ddlSort.Size = new System.Drawing.Size(281, 26);
         this.ddlSort.TabIndex = 4;
         // 
         // ddlData
         // 
         this.ddlData.Location = new System.Drawing.Point(333, 63);
         this.ddlData.MenuManager = this.ribbonControl;
         this.ddlData.Name = "ddlData";
         this.ddlData.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.ddlData.Properties.Appearance.Options.UseForeColor = true;
         this.ddlData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlData.Properties.DropDownRows = 2;
         this.ddlData.Size = new System.Drawing.Size(150, 26);
         this.ddlData.TabIndex = 3;
         // 
         // ddlKind
         // 
         this.ddlKind.Location = new System.Drawing.Point(590, 28);
         this.ddlKind.MenuManager = this.ribbonControl;
         this.ddlKind.Name = "ddlKind";
         this.ddlKind.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.ddlKind.Properties.Appearance.Options.UseForeColor = true;
         this.ddlKind.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlKind.Properties.DropDownRows = 15;
         this.ddlKind.Size = new System.Drawing.Size(150, 26);
         this.ddlKind.TabIndex = 2;
         // 
         // ddlSubType
         // 
         this.ddlSubType.Location = new System.Drawing.Point(333, 28);
         this.ddlSubType.MenuManager = this.ribbonControl;
         this.ddlSubType.Name = "ddlSubType";
         this.ddlSubType.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.ddlSubType.Properties.Appearance.Options.UseForeColor = true;
         this.ddlSubType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlSubType.Size = new System.Drawing.Size(150, 26);
         this.ddlSubType.TabIndex = 1;
         // 
         // labDate
         // 
         this.labDate.AutoSize = true;
         this.labDate.ForeColor = System.Drawing.Color.Black;
         this.labDate.Location = new System.Drawing.Point(26, 34);
         this.labDate.Name = "labDate";
         this.labDate.Size = new System.Drawing.Size(93, 16);
         this.labDate.TabIndex = 12;
         this.labDate.Text = "查詢日期：";
         // 
         // labKind
         // 
         this.labKind.AutoSize = true;
         this.labKind.ForeColor = System.Drawing.Color.Black;
         this.labKind.Location = new System.Drawing.Point(502, 34);
         this.labKind.Name = "labKind";
         this.labKind.Size = new System.Drawing.Size(93, 16);
         this.labKind.TabIndex = 16;
         this.labKind.Text = "契約代號：";
         // 
         // labData
         // 
         this.labData.AutoSize = true;
         this.labData.ForeColor = System.Drawing.Color.Black;
         this.labData.Location = new System.Drawing.Point(245, 69);
         this.labData.Name = "labData";
         this.labData.Size = new System.Drawing.Size(93, 16);
         this.labData.TabIndex = 18;
         this.labData.Text = "資料內容：";
         // 
         // labSubType
         // 
         this.labSubType.AutoSize = true;
         this.labSubType.ForeColor = System.Drawing.Color.Black;
         this.labSubType.Location = new System.Drawing.Point(245, 34);
         this.labSubType.Name = "labSubType";
         this.labSubType.Size = new System.Drawing.Size(93, 16);
         this.labSubType.TabIndex = 14;
         this.labSubType.Text = "契約類別：";
         // 
         // labSort
         // 
         this.labSort.AutoSize = true;
         this.labSort.ForeColor = System.Drawing.Color.Black;
         this.labSort.Location = new System.Drawing.Point(245, 103);
         this.labSort.Name = "labSort";
         this.labSort.Size = new System.Drawing.Size(93, 16);
         this.labSort.TabIndex = 20;
         this.labSort.Text = "排序方式：";
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(12, 207);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(1000, 361);
         this.gcMain.TabIndex = 27;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.gvMain.Appearance.Empty.Options.UseBackColor = true;
         this.gvMain.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Cyan;
         this.gvMain.Appearance.HeaderPanel.Options.UseBackColor = true;
         this.gvMain.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.gvMain.Appearance.Row.BorderColor = System.Drawing.Color.Black;
         this.gvMain.Appearance.Row.Options.UseBackColor = true;
         this.gvMain.Appearance.Row.Options.UseBorderColor = true;
         this.gvMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsPrint.AutoWidth = false;
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         // 
         // W48010
         // 
         this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.Appearance.Options.UseBackColor = true;
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1024, 610);
         this.Name = "W48010";
         this.Text = "W48010";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSort.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlData.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlKind.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSubType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox panFilter;
        private DevExpress.XtraEditors.LookUpEdit ddlSubType;
        private System.Windows.Forms.Label labSubType;
        private System.Windows.Forms.Label labDate;
        private System.Windows.Forms.Label labMsg;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraEditors.LookUpEdit ddlSort;
        private System.Windows.Forms.Label labSort;
        private DevExpress.XtraEditors.LookUpEdit ddlData;
        private System.Windows.Forms.Label labData;
        private DevExpress.XtraEditors.LookUpEdit ddlKind;
        private System.Windows.Forms.Label labKind;
        private System.Windows.Forms.Label labMemo;
        private DevExpress.XtraGrid.GridControl gcExport;
        private DevExpress.XtraGrid.Views.Grid.GridView gvExport;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
      private BaseGround.Widget.TextDateEdit txtSDate;
   }
}