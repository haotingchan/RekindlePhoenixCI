namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20112 {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblRange1 = new System.Windows.Forms.Label();
            this.shl1 = new System.Windows.Forms.LinkLabel();
            this.btnPath1 = new System.Windows.Forms.Button();
            this.txtPath1 = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblRange2 = new System.Windows.Forms.Label();
            this.shl2 = new System.Windows.Forms.LinkLabel();
            this.btnPath2 = new System.Windows.Forms.Button();
            this.txtPath2 = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtYear = new PhoenixCI.Widget.TextDateEdit();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.INTWSE1_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.INTWSE1_TRADE_VOLUMN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.INTWSE1_TRADE_AMT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.INTWSE1_TRADE_CNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.INTWSE1_INDEX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.INTWSE1_UP_DOWN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TYPE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProcess = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath1.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath2.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.lblProcess);
            this.panParent.Controls.Add(this.groupBox3);
            this.panParent.Controls.Add(this.groupBox2);
            this.panParent.Controls.Add(this.groupBox1);
            this.panParent.Size = new System.Drawing.Size(1065, 768);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1065, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblRange1);
            this.groupBox1.Controls.Add(this.shl1);
            this.groupBox1.Controls.Add(this.btnPath1);
            this.groupBox1.Controls.Add(this.txtPath1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(38, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(964, 90);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "上櫃資料";
            // 
            // lblRange1
            // 
            this.lblRange1.AutoSize = true;
            this.lblRange1.Location = new System.Drawing.Point(85, 61);
            this.lblRange1.Name = "lblRange1";
            this.lblRange1.Size = new System.Drawing.Size(61, 20);
            this.lblRange1.TabIndex = 5;
            this.lblRange1.Text = "             ";
            // 
            // shl1
            // 
            this.shl1.AutoSize = true;
            this.shl1.Location = new System.Drawing.Point(713, 30);
            this.shl1.Name = "shl1";
            this.shl1.Size = new System.Drawing.Size(89, 20);
            this.shl1.TabIndex = 4;
            this.shl1.TabStop = true;
            this.shl1.Text = "櫃買成交值";
            this.shl1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.shl1_LinkClicked);
            // 
            // btnPath1
            // 
            this.btnPath1.Location = new System.Drawing.Point(677, 29);
            this.btnPath1.Name = "btnPath1";
            this.btnPath1.Size = new System.Drawing.Size(30, 23);
            this.btnPath1.TabIndex = 3;
            this.btnPath1.Text = "...";
            this.btnPath1.UseVisualStyleBackColor = true;
            this.btnPath1.Click += new System.EventHandler(this.btnPath1_Click);
            // 
            // txtPath1
            // 
            this.txtPath1.Location = new System.Drawing.Point(71, 27);
            this.txtPath1.MenuManager = this.ribbonControl;
            this.txtPath1.Name = "txtPath1";
            this.txtPath1.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtPath1.Properties.Appearance.Options.UseBackColor = true;
            this.txtPath1.Size = new System.Drawing.Size(600, 26);
            this.txtPath1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "資料區間：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "路徑：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblRange2);
            this.groupBox2.Controls.Add(this.shl2);
            this.groupBox2.Controls.Add(this.btnPath2);
            this.groupBox2.Controls.Add(this.txtPath2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(38, 185);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(964, 90);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "上市資料";
            // 
            // lblRange2
            // 
            this.lblRange2.AutoSize = true;
            this.lblRange2.Location = new System.Drawing.Point(85, 63);
            this.lblRange2.Name = "lblRange2";
            this.lblRange2.Size = new System.Drawing.Size(61, 20);
            this.lblRange2.TabIndex = 10;
            this.lblRange2.Text = "             ";
            // 
            // shl2
            // 
            this.shl2.AutoSize = true;
            this.shl2.Location = new System.Drawing.Point(713, 32);
            this.shl2.Name = "shl2";
            this.shl2.Size = new System.Drawing.Size(105, 20);
            this.shl2.TabIndex = 9;
            this.shl2.TabStop = true;
            this.shl2.Text = "證交所成交值";
            this.shl2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.shl2_LinkClicked);
            // 
            // btnPath2
            // 
            this.btnPath2.Location = new System.Drawing.Point(677, 31);
            this.btnPath2.Name = "btnPath2";
            this.btnPath2.Size = new System.Drawing.Size(30, 23);
            this.btnPath2.TabIndex = 8;
            this.btnPath2.Text = "...";
            this.btnPath2.UseVisualStyleBackColor = true;
            this.btnPath2.Click += new System.EventHandler(this.btnPath2_Click);
            // 
            // txtPath2
            // 
            this.txtPath2.Location = new System.Drawing.Point(71, 29);
            this.txtPath2.MenuManager = this.ribbonControl;
            this.txtPath2.Name = "txtPath2";
            this.txtPath2.Size = new System.Drawing.Size(600, 26);
            this.txtPath2.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "資料區間：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "路徑：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtYear);
            this.groupBox3.Controls.Add(this.gcMain);
            this.groupBox3.Controls.Add(this.btnSearch);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(38, 310);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(964, 409);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "歷史資料查詢";
            // 
            // txtYear
            // 
            this.txtYear.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtYear.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Year;
            this.txtYear.EditValue = "0001/1/1 上午 12:00:00";
            this.txtYear.Location = new System.Drawing.Point(70, 25);
            this.txtYear.MenuManager = this.ribbonControl;
            this.txtYear.Name = "txtYear";
            this.txtYear.Properties.Appearance.Options.UseTextOptions = true;
            this.txtYear.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtYear.Properties.Mask.EditMask = "0000";
            this.txtYear.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtYear.Properties.Mask.PlaceHolder = '0';
            this.txtYear.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtYear.Properties.MaxLength = 4;
            this.txtYear.Size = new System.Drawing.Size(69, 26);
            this.txtYear.TabIndex = 13;
            this.txtYear.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // gcMain
            // 
            this.gcMain.Location = new System.Drawing.Point(8, 64);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(947, 335);
            this.gcMain.TabIndex = 12;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.INTWSE1_YMD,
            this.INTWSE1_TRADE_VOLUMN,
            this.INTWSE1_TRADE_AMT,
            this.INTWSE1_TRADE_CNT,
            this.INTWSE1_INDEX,
            this.INTWSE1_UP_DOWN,
            this.TYPE_NAME});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsBehavior.ReadOnly = true;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            // 
            // INTWSE1_YMD
            // 
            this.INTWSE1_YMD.AppearanceCell.Options.UseTextOptions = true;
            this.INTWSE1_YMD.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.INTWSE1_YMD.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.INTWSE1_YMD.AppearanceHeader.Options.UseBackColor = true;
            this.INTWSE1_YMD.AppearanceHeader.Options.UseTextOptions = true;
            this.INTWSE1_YMD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.INTWSE1_YMD.Caption = "日期";
            this.INTWSE1_YMD.FieldName = "INTWSE1_YMD";
            this.INTWSE1_YMD.Name = "INTWSE1_YMD";
            this.INTWSE1_YMD.OptionsColumn.FixedWidth = true;
            this.INTWSE1_YMD.OptionsColumn.ReadOnly = true;
            this.INTWSE1_YMD.Visible = true;
            this.INTWSE1_YMD.VisibleIndex = 0;
            this.INTWSE1_YMD.Width = 95;
            // 
            // INTWSE1_TRADE_VOLUMN
            // 
            this.INTWSE1_TRADE_VOLUMN.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.INTWSE1_TRADE_VOLUMN.AppearanceHeader.Options.UseBackColor = true;
            this.INTWSE1_TRADE_VOLUMN.AppearanceHeader.Options.UseTextOptions = true;
            this.INTWSE1_TRADE_VOLUMN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.INTWSE1_TRADE_VOLUMN.Caption = "成交股數";
            this.INTWSE1_TRADE_VOLUMN.FieldName = "INTWSE1_TRADE_VOLUMN";
            this.INTWSE1_TRADE_VOLUMN.Name = "INTWSE1_TRADE_VOLUMN";
            this.INTWSE1_TRADE_VOLUMN.OptionsColumn.FixedWidth = true;
            this.INTWSE1_TRADE_VOLUMN.OptionsColumn.ReadOnly = true;
            this.INTWSE1_TRADE_VOLUMN.Visible = true;
            this.INTWSE1_TRADE_VOLUMN.VisibleIndex = 1;
            this.INTWSE1_TRADE_VOLUMN.Width = 160;
            // 
            // INTWSE1_TRADE_AMT
            // 
            this.INTWSE1_TRADE_AMT.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.INTWSE1_TRADE_AMT.AppearanceHeader.Options.UseBackColor = true;
            this.INTWSE1_TRADE_AMT.AppearanceHeader.Options.UseTextOptions = true;
            this.INTWSE1_TRADE_AMT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.INTWSE1_TRADE_AMT.Caption = "成交金額";
            this.INTWSE1_TRADE_AMT.FieldName = "INTWSE1_TRADE_AMT";
            this.INTWSE1_TRADE_AMT.Name = "INTWSE1_TRADE_AMT";
            this.INTWSE1_TRADE_AMT.OptionsColumn.FixedWidth = true;
            this.INTWSE1_TRADE_AMT.OptionsColumn.ReadOnly = true;
            this.INTWSE1_TRADE_AMT.Visible = true;
            this.INTWSE1_TRADE_AMT.VisibleIndex = 2;
            this.INTWSE1_TRADE_AMT.Width = 160;
            // 
            // INTWSE1_TRADE_CNT
            // 
            this.INTWSE1_TRADE_CNT.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.INTWSE1_TRADE_CNT.AppearanceHeader.Options.UseBackColor = true;
            this.INTWSE1_TRADE_CNT.AppearanceHeader.Options.UseTextOptions = true;
            this.INTWSE1_TRADE_CNT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.INTWSE1_TRADE_CNT.Caption = "成交筆數";
            this.INTWSE1_TRADE_CNT.FieldName = "INTWSE1_TRADE_CNT";
            this.INTWSE1_TRADE_CNT.Name = "INTWSE1_TRADE_CNT";
            this.INTWSE1_TRADE_CNT.OptionsColumn.FixedWidth = true;
            this.INTWSE1_TRADE_CNT.OptionsColumn.ReadOnly = true;
            this.INTWSE1_TRADE_CNT.Visible = true;
            this.INTWSE1_TRADE_CNT.VisibleIndex = 3;
            this.INTWSE1_TRADE_CNT.Width = 130;
            // 
            // INTWSE1_INDEX
            // 
            this.INTWSE1_INDEX.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.INTWSE1_INDEX.AppearanceHeader.Options.UseBackColor = true;
            this.INTWSE1_INDEX.AppearanceHeader.Options.UseTextOptions = true;
            this.INTWSE1_INDEX.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.INTWSE1_INDEX.Caption = "發行量加權股價指數";
            this.INTWSE1_INDEX.FieldName = "INTWSE1_INDEX";
            this.INTWSE1_INDEX.Name = "INTWSE1_INDEX";
            this.INTWSE1_INDEX.OptionsColumn.FixedWidth = true;
            this.INTWSE1_INDEX.OptionsColumn.ReadOnly = true;
            this.INTWSE1_INDEX.Visible = true;
            this.INTWSE1_INDEX.VisibleIndex = 4;
            this.INTWSE1_INDEX.Width = 160;
            // 
            // INTWSE1_UP_DOWN
            // 
            this.INTWSE1_UP_DOWN.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.INTWSE1_UP_DOWN.AppearanceHeader.Options.UseBackColor = true;
            this.INTWSE1_UP_DOWN.AppearanceHeader.Options.UseTextOptions = true;
            this.INTWSE1_UP_DOWN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.INTWSE1_UP_DOWN.Caption = "漲跌點數";
            this.INTWSE1_UP_DOWN.FieldName = "INTWSE1_UP_DOWN";
            this.INTWSE1_UP_DOWN.Name = "INTWSE1_UP_DOWN";
            this.INTWSE1_UP_DOWN.OptionsColumn.FixedWidth = true;
            this.INTWSE1_UP_DOWN.OptionsColumn.ReadOnly = true;
            this.INTWSE1_UP_DOWN.Visible = true;
            this.INTWSE1_UP_DOWN.VisibleIndex = 5;
            // 
            // TYPE_NAME
            // 
            this.TYPE_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.TYPE_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TYPE_NAME.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.TYPE_NAME.AppearanceHeader.Options.UseBackColor = true;
            this.TYPE_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.TYPE_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TYPE_NAME.Caption = "類別";
            this.TYPE_NAME.FieldName = "TYPE_NAME";
            this.TYPE_NAME.Name = "TYPE_NAME";
            this.TYPE_NAME.OptionsColumn.FixedWidth = true;
            this.TYPE_NAME.OptionsColumn.ReadOnly = true;
            this.TYPE_NAME.Visible = true;
            this.TYPE_NAME.VisibleIndex = 6;
            this.TYPE_NAME.Width = 50;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(145, 26);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(55, 25);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查詢";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "年度：";
            // 
            // lblProcess
            // 
            this.lblProcess.AutoSize = true;
            this.lblProcess.ForeColor = System.Drawing.Color.Blue;
            this.lblProcess.Location = new System.Drawing.Point(34, 147);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(169, 20);
            this.lblProcess.TabIndex = 3;
            this.lblProcess.Text = "訊息：資料轉入中........";
            this.lblProcess.Visible = false;
            // 
            // W20112
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 798);
            this.Name = "W20112";
            this.Text = "W20112";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath1.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath2.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel shl1;
        private System.Windows.Forms.Button btnPath1;
        private DevExpress.XtraEditors.TextEdit txtPath1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel shl2;
        private System.Windows.Forms.Button btnPath2;
        private DevExpress.XtraEditors.TextEdit txtPath2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Widget.TextDateEdit txtYear;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label lblRange2;
        private System.Windows.Forms.Label lblRange1;
        private DevExpress.XtraGrid.Columns.GridColumn INTWSE1_YMD;
        private DevExpress.XtraGrid.Columns.GridColumn INTWSE1_TRADE_VOLUMN;
        private DevExpress.XtraGrid.Columns.GridColumn INTWSE1_TRADE_AMT;
        private DevExpress.XtraGrid.Columns.GridColumn INTWSE1_TRADE_CNT;
        private DevExpress.XtraGrid.Columns.GridColumn INTWSE1_INDEX;
        private DevExpress.XtraGrid.Columns.GridColumn INTWSE1_UP_DOWN;
        private DevExpress.XtraGrid.Columns.GridColumn TYPE_NAME;
    }
}