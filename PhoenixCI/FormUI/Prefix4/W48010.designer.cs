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
         this.txtSDate = new BaseGround.Widget.TextDateEdit();
         this.gcExport = new DevExpress.XtraGrid.GridControl();
         this.gvExport = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
         this.labMemo = new System.Windows.Forms.Label();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.panFilter = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.ddlKind = new DevExpress.XtraEditors.LookUpEdit();
         this.labKind = new System.Windows.Forms.Label();
         this.ddlSort = new DevExpress.XtraEditors.LookUpEdit();
         this.labSort = new System.Windows.Forms.Label();
         this.ddlData = new DevExpress.XtraEditors.LookUpEdit();
         this.ddlSubType = new DevExpress.XtraEditors.LookUpEdit();
         this.labDate = new System.Windows.Forms.Label();
         this.labSubType = new System.Windows.Forms.Label();
         this.labData = new System.Windows.Forms.Label();
         this.panel1 = new System.Windows.Forms.Panel();
         this.panel2 = new System.Windows.Forms.Panel();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panFilter)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ddlKind.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSort.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlData.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSubType.Properties)).BeginInit();
         this.panel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Dock = System.Windows.Forms.DockStyle.None;
         this.panParent.Location = new System.Drawing.Point(742, 512);
         this.panParent.Size = new System.Drawing.Size(73, 65);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(815, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // txtSDate
         // 
         this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtSDate.EditValue = "2018/12/01";
         this.txtSDate.EnterMoveNextControl = true;
         this.txtSDate.Location = new System.Drawing.Point(110, 17);
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
         // gcExport
         // 
         this.gcExport.Location = new System.Drawing.Point(693, 88);
         this.gcExport.MainView = this.gvExport;
         this.gcExport.MenuManager = this.ribbonControl;
         this.gcExport.Name = "gcExport";
         this.gcExport.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit2});
         this.gcExport.Size = new System.Drawing.Size(231, 148);
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
         this.labMemo.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.labMemo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.labMemo.Location = new System.Drawing.Point(16, 146);
         this.labMemo.Name = "labMemo";
         this.labMemo.Size = new System.Drawing.Size(681, 20);
         this.labMemo.TabIndex = 21;
         this.labMemo.Text = "備    註：已下市契約之最小風險價格係數顯示空白；有效契約之最小風險價格係數不可為空白。";
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(0, 235);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(815, 340);
         this.gcMain.TabIndex = 27;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.Appearance.Row.BorderColor = System.Drawing.Color.Black;
         this.gvMain.Appearance.Row.Options.UseBorderColor = true;
         this.gvMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         // 
         // panFilter
         // 
         this.panFilter.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Appearance.Options.UseBackColor = true;
         this.panFilter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panFilter.Controls.Add(this.gcExport);
         this.panFilter.Controls.Add(this.labMsg);
         this.panFilter.Controls.Add(this.ddlKind);
         this.panFilter.Controls.Add(this.labMemo);
         this.panFilter.Controls.Add(this.labKind);
         this.panFilter.Controls.Add(this.ddlSort);
         this.panFilter.Controls.Add(this.labSort);
         this.panFilter.Controls.Add(this.ddlData);
         this.panFilter.Controls.Add(this.ddlSubType);
         this.panFilter.Controls.Add(this.txtSDate);
         this.panFilter.Controls.Add(this.labDate);
         this.panFilter.Controls.Add(this.labSubType);
         this.panFilter.Controls.Add(this.labData);
         this.panFilter.Dock = System.Windows.Forms.DockStyle.Top;
         this.panFilter.Location = new System.Drawing.Point(0, 0);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(815, 204);
         this.panFilter.TabIndex = 80;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 175);
         this.labMsg.MaximumSize = new System.Drawing.Size(790, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 83;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // ddlKind
         // 
         this.ddlKind.Location = new System.Drawing.Point(615, 17);
         this.ddlKind.Name = "ddlKind";
         this.ddlKind.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlKind.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlKind.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlKind.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlKind.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlKind.Properties.NullText = "";
         this.ddlKind.Properties.PopupSizeable = false;
         this.ddlKind.Size = new System.Drawing.Size(150, 26);
         this.ddlKind.TabIndex = 4;
         // 
         // labKind
         // 
         this.labKind.AutoSize = true;
         this.labKind.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labKind.Location = new System.Drawing.Point(529, 20);
         this.labKind.Name = "labKind";
         this.labKind.Size = new System.Drawing.Size(90, 21);
         this.labKind.TabIndex = 102;
         this.labKind.Text = "契約代號：";
         // 
         // ddlSort
         // 
         this.ddlSort.Location = new System.Drawing.Point(337, 107);
         this.ddlSort.Name = "ddlSort";
         this.ddlSort.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlSort.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlSort.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlSort.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlSort.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlSort.Properties.NullText = "";
         this.ddlSort.Properties.PopupSizeable = false;
         this.ddlSort.Size = new System.Drawing.Size(281, 26);
         this.ddlSort.TabIndex = 3;
         // 
         // labSort
         // 
         this.labSort.AutoSize = true;
         this.labSort.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labSort.Location = new System.Drawing.Point(251, 110);
         this.labSort.Name = "labSort";
         this.labSort.Size = new System.Drawing.Size(90, 21);
         this.labSort.TabIndex = 100;
         this.labSort.Text = "排序方式：";
         // 
         // ddlData
         // 
         this.ddlData.Location = new System.Drawing.Point(337, 62);
         this.ddlData.Name = "ddlData";
         this.ddlData.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlData.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlData.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlData.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlData.Properties.NullText = "";
         this.ddlData.Properties.PopupSizeable = false;
         this.ddlData.Size = new System.Drawing.Size(150, 26);
         this.ddlData.TabIndex = 2;
         // 
         // ddlSubType
         // 
         this.ddlSubType.Location = new System.Drawing.Point(337, 17);
         this.ddlSubType.Name = "ddlSubType";
         this.ddlSubType.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlSubType.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlSubType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlSubType.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlSubType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlSubType.Properties.NullText = "";
         this.ddlSubType.Properties.PopupSizeable = false;
         this.ddlSubType.Size = new System.Drawing.Size(150, 26);
         this.ddlSubType.TabIndex = 1;
         // 
         // labDate
         // 
         this.labDate.AutoSize = true;
         this.labDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labDate.Location = new System.Drawing.Point(15, 20);
         this.labDate.Name = "labDate";
         this.labDate.Size = new System.Drawing.Size(90, 21);
         this.labDate.TabIndex = 63;
         this.labDate.Text = "查詢日期：";
         // 
         // labSubType
         // 
         this.labSubType.AutoSize = true;
         this.labSubType.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labSubType.Location = new System.Drawing.Point(251, 20);
         this.labSubType.Name = "labSubType";
         this.labSubType.Size = new System.Drawing.Size(90, 21);
         this.labSubType.TabIndex = 73;
         this.labSubType.Text = "契約類別：";
         // 
         // labData
         // 
         this.labData.AutoSize = true;
         this.labData.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labData.Location = new System.Drawing.Point(251, 65);
         this.labData.Name = "labData";
         this.labData.Size = new System.Drawing.Size(90, 21);
         this.labData.TabIndex = 74;
         this.labData.Text = "資料內容：";
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.panFilter);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 30);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(815, 205);
         this.panel1.TabIndex = 81;
         // 
         // panel2
         // 
         this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel2.Location = new System.Drawing.Point(0, 30);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(815, 545);
         this.panel2.TabIndex = 82;
         // 
         // W48010
         // 
         this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.Appearance.Options.UseBackColor = true;
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(815, 575);
         this.Controls.Add(this.gcMain);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.panel2);
         this.Name = "W48010";
         this.Text = "W48010";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panel2, 0);
         this.Controls.SetChildIndex(this.panel1, 0);
         this.Controls.SetChildIndex(this.gcMain, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panFilter)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ddlKind.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSort.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlData.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSubType.Properties)).EndInit();
         this.panel1.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label labMemo;
        private DevExpress.XtraGrid.GridControl gcExport;
        private DevExpress.XtraGrid.Views.Grid.GridView gvExport;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
      private BaseGround.Widget.TextDateEdit txtSDate;
      private DevExpress.XtraEditors.PanelControl panFilter;
      private System.Windows.Forms.Label labDate;
      private System.Windows.Forms.Label labSubType;
      private System.Windows.Forms.Label labData;
      private DevExpress.XtraEditors.LookUpEdit ddlKind;
      private System.Windows.Forms.Label labKind;
      private DevExpress.XtraEditors.LookUpEdit ddlSort;
      private System.Windows.Forms.Label labSort;
      private DevExpress.XtraEditors.LookUpEdit ddlData;
      private DevExpress.XtraEditors.LookUpEdit ddlSubType;
      private System.Windows.Forms.Label labMsg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}