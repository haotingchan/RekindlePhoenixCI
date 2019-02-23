namespace PhoenixCI.FormUI.PrefixS
{
    partial class WS0071
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WS0071));
            this.label1 = new System.Windows.Forms.Label();
            this.labSpanDesc = new System.Windows.Forms.Label();
            this.txtStartDate = new PhoenixCI.Widget.TextDateEdit();
            this.labDate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtEndDate = new PhoenixCI.Widget.TextDateEdit();
            this.lbl1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.SPAN_PARAM_MODULE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_PARAM_CLASS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_PARAM_CC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_PARAM_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_PARAM_VALUE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_PARAM_EXPIRY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.SPAN_PARAM_VOL_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_PARAM_VOL_VALUE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.SPAN_PARAM_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_PARAM_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IS_NEWROW = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.panel3);
            this.panParent.Controls.Add(this.panel1);
            this.panParent.Size = new System.Drawing.Size(1376, 613);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1376, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(367, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(961, 140);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // labSpanDesc
            // 
            this.labSpanDesc.AutoSize = true;
            this.labSpanDesc.Location = new System.Drawing.Point(7, 136);
            this.labSpanDesc.Name = "labSpanDesc";
            this.labSpanDesc.Size = new System.Drawing.Size(188, 20);
            this.labSpanDesc.TabIndex = 19;
            this.labSpanDesc.Text = "(2)損益計算價格變化設定";
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtStartDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate.EditValue = "0001/1/1 上午 12:00:00";
            this.txtStartDate.Location = new System.Drawing.Point(125, 13);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.DisplayFormat.FormatString = "yyyyMMdd";
            this.txtStartDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtStartDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtStartDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Size = new System.Drawing.Size(100, 26);
            this.txtStartDate.TabIndex = 15;
            this.txtStartDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            this.txtStartDate.Leave += new System.EventHandler(this.txtStartDate_Leave);
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.Location = new System.Drawing.Point(7, 16);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(112, 20);
            this.labDate.TabIndex = 16;
            this.labDate.Text = "(1) 日期區間：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labSpanDesc);
            this.panel1.Controls.Add(this.txtEndDate);
            this.panel1.Controls.Add(this.txtStartDate);
            this.panel1.Controls.Add(this.lbl1);
            this.panel1.Controls.Add(this.labDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1352, 161);
            this.panel1.TabIndex = 1;
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtEndDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate.EditValue = "0001/1/1 上午 12:00:00";
            this.txtEndDate.Location = new System.Drawing.Point(252, 13);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.DisplayFormat.FormatString = "yyyyMMdd";
            this.txtEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Size = new System.Drawing.Size(100, 26);
            this.txtEndDate.TabIndex = 18;
            this.txtEndDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            this.txtEndDate.Leave += new System.EventHandler(this.txtEndDate_Leave);
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(231, 16);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(21, 20);
            this.lbl1.TabIndex = 17;
            this.lbl1.Text = "~";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(12, 173);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1352, 428);
            this.panel3.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gcMain);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 64);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1352, 364);
            this.panel4.TabIndex = 2;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(0, 0);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemLookUpEdit1,
            this.repositoryItemTextEdit1});
            this.gcMain.Size = new System.Drawing.Size(1352, 364);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.SPAN_PARAM_MODULE,
            this.SPAN_PARAM_CLASS,
            this.SPAN_PARAM_CC,
            this.SPAN_PARAM_TYPE,
            this.SPAN_PARAM_VALUE,
            this.SPAN_PARAM_EXPIRY,
            this.SPAN_PARAM_VOL_TYPE,
            this.SPAN_PARAM_VOL_VALUE,
            this.SPAN_PARAM_USER_ID,
            this.SPAN_PARAM_W_TIME,
            this.IS_NEWROW});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // 
            // SPAN_PARAM_MODULE
            // 
            this.SPAN_PARAM_MODULE.FieldName = "SPAN_PARAM_MODULE";
            this.SPAN_PARAM_MODULE.Name = "SPAN_PARAM_MODULE";
            // 
            // SPAN_PARAM_CLASS
            // 
            this.SPAN_PARAM_CLASS.Caption = "類別";
            this.SPAN_PARAM_CLASS.FieldName = "SPAN_PARAM_CLASS";
            this.SPAN_PARAM_CLASS.Name = "SPAN_PARAM_CLASS";
            this.SPAN_PARAM_CLASS.Visible = true;
            this.SPAN_PARAM_CLASS.VisibleIndex = 0;
            // 
            // SPAN_PARAM_CC
            // 
            this.SPAN_PARAM_CC.Caption = "商品組合";
            this.SPAN_PARAM_CC.FieldName = "SPAN_PARAM_CC";
            this.SPAN_PARAM_CC.Name = "SPAN_PARAM_CC";
            this.SPAN_PARAM_CC.Visible = true;
            this.SPAN_PARAM_CC.VisibleIndex = 1;
            // 
            // SPAN_PARAM_TYPE
            // 
            this.SPAN_PARAM_TYPE.Caption = "設定方式";
            this.SPAN_PARAM_TYPE.FieldName = "SPAN_PARAM_TYPE";
            this.SPAN_PARAM_TYPE.Name = "SPAN_PARAM_TYPE";
            this.SPAN_PARAM_TYPE.Visible = true;
            this.SPAN_PARAM_TYPE.VisibleIndex = 2;
            // 
            // SPAN_PARAM_VALUE
            // 
            this.SPAN_PARAM_VALUE.Caption = "設定值";
            this.SPAN_PARAM_VALUE.FieldName = "SPAN_PARAM_VALUE";
            this.SPAN_PARAM_VALUE.Name = "SPAN_PARAM_VALUE";
            this.SPAN_PARAM_VALUE.Visible = true;
            this.SPAN_PARAM_VALUE.VisibleIndex = 3;
            // 
            // SPAN_PARAM_EXPIRY
            // 
            this.SPAN_PARAM_EXPIRY.Caption = "到期商品設定";
            this.SPAN_PARAM_EXPIRY.ColumnEdit = this.repositoryItemCheckEdit1;
            this.SPAN_PARAM_EXPIRY.FieldName = "SPAN_PARAM_EXPIRY";
            this.SPAN_PARAM_EXPIRY.Name = "SPAN_PARAM_EXPIRY";
            this.SPAN_PARAM_EXPIRY.Visible = true;
            this.SPAN_PARAM_EXPIRY.VisibleIndex = 4;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "是";
            this.repositoryItemCheckEdit1.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.ValueChecked = "1";
            this.repositoryItemCheckEdit1.ValueUnchecked = "0";
            // 
            // SPAN_PARAM_VOL_TYPE
            // 
            this.SPAN_PARAM_VOL_TYPE.Caption = "波動度設定";
            this.SPAN_PARAM_VOL_TYPE.FieldName = "SPAN_PARAM_VOL_TYPE";
            this.SPAN_PARAM_VOL_TYPE.Name = "SPAN_PARAM_VOL_TYPE";
            this.SPAN_PARAM_VOL_TYPE.Visible = true;
            this.SPAN_PARAM_VOL_TYPE.VisibleIndex = 5;
            // 
            // SPAN_PARAM_VOL_VALUE
            // 
            this.SPAN_PARAM_VOL_VALUE.Caption = "設定值(%)";
            this.SPAN_PARAM_VOL_VALUE.ColumnEdit = this.repositoryItemTextEdit1;
            this.SPAN_PARAM_VOL_VALUE.FieldName = "SPAN_PARAM_VOL_VALUE";
            this.SPAN_PARAM_VOL_VALUE.Name = "SPAN_PARAM_VOL_VALUE";
            this.SPAN_PARAM_VOL_VALUE.Visible = true;
            this.SPAN_PARAM_VOL_VALUE.VisibleIndex = 6;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit1.MaxLength = 3;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // SPAN_PARAM_USER_ID
            // 
            this.SPAN_PARAM_USER_ID.Caption = "SPAN_PARAM_USER_ID";
            this.SPAN_PARAM_USER_ID.FieldName = "SPAN_PARAM_USER_ID";
            this.SPAN_PARAM_USER_ID.Name = "SPAN_PARAM_USER_ID";
            // 
            // SPAN_PARAM_W_TIME
            // 
            this.SPAN_PARAM_W_TIME.Caption = "SPAN_PARAM_W_TIME";
            this.SPAN_PARAM_W_TIME.FieldName = "SPAN_PARAM_W_TIME";
            this.SPAN_PARAM_W_TIME.Name = "SPAN_PARAM_W_TIME";
            // 
            // IS_NEWROW
            // 
            this.IS_NEWROW.Caption = "IS_NEWROW";
            this.IS_NEWROW.FieldName = "IS_NEWROW";
            this.IS_NEWROW.Name = "IS_NEWROW";
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1352, 64);
            this.panel2.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(8, 21);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(111, 37);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "清除資料";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // WS0071
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1376, 643);
            this.Name = "WS0071";
            this.Text = "WS0071";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labSpanDesc;
        private Widget.TextDateEdit txtEndDate;
        private Widget.TextDateEdit txtStartDate;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label labDate;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_MODULE;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_CLASS;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_CC;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_VALUE;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_EXPIRY;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_VOL_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_VOL_VALUE;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_PARAM_W_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn IS_NEWROW;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
    }
}