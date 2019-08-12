namespace PhoenixCI.FormUI.Prefix4 {
    partial class W48020 {
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
         this.gcExport = new DevExpress.XtraGrid.GridControl();
         this.gvExport = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
         this.txtEDate = new DevExpress.XtraEditors.TextEdit();
         this.txtSDate = new DevExpress.XtraEditors.TextEdit();
         this.labDateUnit = new System.Windows.Forms.Label();
         this.labDateBetween = new System.Windows.Forms.Label();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.panFilter = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.ddlKind = new DevExpress.XtraEditors.LookUpEdit();
         this.labMemo = new System.Windows.Forms.Label();
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
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panFilter)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ddlKind.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSort.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlData.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlSubType.Properties)).BeginInit();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Dock = System.Windows.Forms.DockStyle.None;
         this.panParent.Location = new System.Drawing.Point(724, 483);
         this.panParent.Size = new System.Drawing.Size(91, 89);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(815, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // gcExport
         // 
         this.gcExport.Location = new System.Drawing.Point(722, 41);
         this.gcExport.MainView = this.gvExport;
         this.gcExport.MenuManager = this.ribbonControl;
         this.gcExport.Name = "gcExport";
         this.gcExport.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit2});
         this.gcExport.Size = new System.Drawing.Size(221, 154);
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
         // txtEDate
         // 
         this.txtEDate.EditValue = ((short)(2018));
         this.txtEDate.Location = new System.Drawing.Point(216, 62);
         this.txtEDate.MenuManager = this.ribbonControl;
         this.txtEDate.Name = "txtEDate";
         this.txtEDate.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.txtEDate.Properties.Appearance.Options.UseForeColor = true;
         this.txtEDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.txtEDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.txtEDate.Properties.Mask.EditMask = "d";
         this.txtEDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.txtEDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEDate.Properties.MaxLength = 4;
         this.txtEDate.Size = new System.Drawing.Size(63, 26);
         this.txtEDate.TabIndex = 2;
         // 
         // txtSDate
         // 
         this.txtSDate.EditValue = ((short)(2018));
         this.txtSDate.Location = new System.Drawing.Point(101, 62);
         this.txtSDate.MenuManager = this.ribbonControl;
         this.txtSDate.Name = "txtSDate";
         this.txtSDate.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.txtSDate.Properties.Appearance.Options.UseForeColor = true;
         this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtSDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.txtSDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.txtSDate.Properties.Mask.EditMask = "d";
         this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtSDate.Properties.MaxLength = 4;
         this.txtSDate.Size = new System.Drawing.Size(63, 26);
         this.txtSDate.TabIndex = 1;
         // 
         // labDateUnit
         // 
         this.labDateUnit.AutoSize = true;
         this.labDateUnit.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labDateUnit.ForeColor = System.Drawing.Color.Black;
         this.labDateUnit.Location = new System.Drawing.Point(280, 65);
         this.labDateUnit.Name = "labDateUnit";
         this.labDateUnit.Size = new System.Drawing.Size(26, 21);
         this.labDateUnit.TabIndex = 31;
         this.labDateUnit.Text = "年";
         // 
         // labDateBetween
         // 
         this.labDateBetween.AutoSize = true;
         this.labDateBetween.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labDateBetween.ForeColor = System.Drawing.Color.Black;
         this.labDateBetween.Location = new System.Drawing.Point(165, 65);
         this.labDateBetween.Name = "labDateBetween";
         this.labDateBetween.Size = new System.Drawing.Size(50, 21);
         this.labDateBetween.TabIndex = 30;
         this.labDateBetween.Text = "年  至";
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.EmbeddedNavigator.Appearance.Options.UseTextOptions = true;
         this.gcMain.Location = new System.Drawing.Point(0, 0);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(815, 338);
         this.gcMain.TabIndex = 27;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.Appearance.Row.BorderColor = System.Drawing.Color.Black;
         this.gvMain.Appearance.Row.Options.UseBorderColor = true;
         this.gvMain.Appearance.Row.Options.UseTextOptions = true;
         this.gvMain.AppearancePrint.Row.Options.UseTextOptions = true;
         this.gvMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsPrint.AutoWidth = false;
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         // 
         // panFilter
         // 
         this.panFilter.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Appearance.Options.UseBackColor = true;
         this.panFilter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panFilter.Controls.Add(this.gcExport);
         this.panFilter.Controls.Add(this.labDateUnit);
         this.panFilter.Controls.Add(this.txtEDate);
         this.panFilter.Controls.Add(this.labMsg);
         this.panFilter.Controls.Add(this.txtSDate);
         this.panFilter.Controls.Add(this.ddlKind);
         this.panFilter.Controls.Add(this.labMemo);
         this.panFilter.Controls.Add(this.labKind);
         this.panFilter.Controls.Add(this.ddlSort);
         this.panFilter.Controls.Add(this.labSort);
         this.panFilter.Controls.Add(this.ddlData);
         this.panFilter.Controls.Add(this.ddlSubType);
         this.panFilter.Controls.Add(this.labDate);
         this.panFilter.Controls.Add(this.labSubType);
         this.panFilter.Controls.Add(this.labDateBetween);
         this.panFilter.Controls.Add(this.labData);
         this.panFilter.Dock = System.Windows.Forms.DockStyle.Top;
         this.panFilter.Location = new System.Drawing.Point(0, 0);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(815, 204);
         this.panFilter.TabIndex = 81;
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
         this.ddlKind.Location = new System.Drawing.Point(435, 17);
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
         // labKind
         // 
         this.labKind.AutoSize = true;
         this.labKind.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labKind.Location = new System.Drawing.Point(349, 20);
         this.labKind.Name = "labKind";
         this.labKind.Size = new System.Drawing.Size(90, 21);
         this.labKind.TabIndex = 102;
         this.labKind.Text = "契約代號：";
         // 
         // ddlSort
         // 
         this.ddlSort.Location = new System.Drawing.Point(435, 62);
         this.ddlSort.Name = "ddlSort";
         this.ddlSort.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlSort.Properties.Appearance.Options.UseBackColor = true;
         this.ddlSort.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlSort.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlSort.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlSort.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlSort.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlSort.Properties.NullText = "";
         this.ddlSort.Properties.PopupSizeable = false;
         this.ddlSort.Size = new System.Drawing.Size(281, 26);
         this.ddlSort.TabIndex = 5;
         // 
         // labSort
         // 
         this.labSort.AutoSize = true;
         this.labSort.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labSort.Location = new System.Drawing.Point(349, 65);
         this.labSort.Name = "labSort";
         this.labSort.Size = new System.Drawing.Size(90, 21);
         this.labSort.TabIndex = 100;
         this.labSort.Text = "排序方式：";
         // 
         // ddlData
         // 
         this.ddlData.Location = new System.Drawing.Point(101, 107);
         this.ddlData.Name = "ddlData";
         this.ddlData.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlData.Properties.Appearance.Options.UseBackColor = true;
         this.ddlData.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlData.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlData.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlData.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlData.Properties.NullText = "";
         this.ddlData.Properties.PopupSizeable = false;
         this.ddlData.Size = new System.Drawing.Size(150, 26);
         this.ddlData.TabIndex = 3;
         // 
         // ddlSubType
         // 
         this.ddlSubType.Location = new System.Drawing.Point(101, 17);
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
         this.ddlSubType.TabIndex = 0;
         // 
         // labDate
         // 
         this.labDate.AutoSize = true;
         this.labDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labDate.Location = new System.Drawing.Point(15, 65);
         this.labDate.Name = "labDate";
         this.labDate.Size = new System.Drawing.Size(90, 21);
         this.labDate.TabIndex = 63;
         this.labDate.Text = "查詢期間：";
         // 
         // labSubType
         // 
         this.labSubType.AutoSize = true;
         this.labSubType.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labSubType.Location = new System.Drawing.Point(15, 20);
         this.labSubType.Name = "labSubType";
         this.labSubType.Size = new System.Drawing.Size(90, 21);
         this.labSubType.TabIndex = 73;
         this.labSubType.Text = "契約類別：";
         // 
         // labData
         // 
         this.labData.AutoSize = true;
         this.labData.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labData.Location = new System.Drawing.Point(15, 110);
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
         this.panel1.Size = new System.Drawing.Size(815, 207);
         this.panel1.TabIndex = 82;
         // 
         // panel2
         // 
         this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.panel2.Controls.Add(this.gcMain);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel2.Location = new System.Drawing.Point(0, 237);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(815, 338);
         this.panel2.TabIndex = 83;
         // 
         // W48020
         // 
         this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.Appearance.Options.UseBackColor = true;
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(815, 575);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Name = "W48020";
         this.Text = "W48020";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panel1, 0);
         this.Controls.SetChildIndex(this.panel2, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
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
         this.panel2.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.GridControl gcExport;
        private DevExpress.XtraGrid.Views.Grid.GridView gvExport;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private System.Windows.Forms.Label labDateUnit;
        private System.Windows.Forms.Label labDateBetween;
        private DevExpress.XtraEditors.TextEdit txtEDate;
        private DevExpress.XtraEditors.TextEdit txtSDate;
      private DevExpress.XtraEditors.PanelControl panFilter;
      private System.Windows.Forms.Label labMsg;
      private DevExpress.XtraEditors.LookUpEdit ddlKind;
      private System.Windows.Forms.Label labMemo;
      private System.Windows.Forms.Label labKind;
      private DevExpress.XtraEditors.LookUpEdit ddlSort;
      private System.Windows.Forms.Label labSort;
      private DevExpress.XtraEditors.LookUpEdit ddlData;
      private DevExpress.XtraEditors.LookUpEdit ddlSubType;
      private System.Windows.Forms.Label labDate;
      private System.Windows.Forms.Label labSubType;
      private System.Windows.Forms.Label labData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}