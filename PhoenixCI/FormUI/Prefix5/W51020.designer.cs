namespace PhoenixCI.FormUI.Prefix5
{
    partial class W51020
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(W51020));
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.MMFT_MARKET_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.MMFT_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMFT_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.MMFT_BEGIN_MTH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.MMFT_MTH_CNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMFT_IN_CNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMFT_OUT_CNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMFT_AVG_CNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMFT_PROD_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.MMFT_REF_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMFT_CP_KIND = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMFT_END_S = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMFT_END_E = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Size = new System.Drawing.Size(1379, 706);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1379, 32);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panel1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 552);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1379, 186);
            this.panelControl2.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(461, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(221, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "(TXO應須計算月成績, 故為99)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(949, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(389, 180);
            this.label4.TabIndex = 14;
            this.label4.Text = "例：RHO近月開始全部月份納入，\r\n       但其中每日4個月份需符合維持時間(25010)規定，\r\n        另序列條件為價內1價外4，其中取8\r\n　" +
    "　(1)近月份起始月份 = 1\r\n　　(2)符合月份個數 = 99 (全部)\r\n　　(3)計算月份數 = 4 \r\n　　(4)價內個數 = 1\r\n　　(5)價外個" +
    "數 = 4\r\n　　(6)計算序列數 = 8";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(949, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(413, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "註：計算序列數 = 依價內個數&價外個數，全部取幾個序列";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(26, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(429, 80);
            this.label2.TabIndex = 12;
            this.label2.Text = "註：近月份起始月份 = 1代表最近月,2最表次近月...\r\n       符合月份個數 = 99代表全部\r\n　　到期日(起),到期日(迄)  = 正負數代表到期日" +
    "加減營業日,\r\n       判斷交易日符合到期日條件才計算\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(507, 180);
            this.label1.TabIndex = 11;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gcMain);
            this.panelControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panelControl1.Location = new System.Drawing.Point(0, 32);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1379, 520);
            this.panelControl1.TabIndex = 0;
            // 
            // gcMain
            // 
            this.gcMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(2, 2);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemTextEdit3,
            this.repositoryItemTextEdit4});
            this.gcMain.Size = new System.Drawing.Size(1375, 516);
            this.gcMain.TabIndex = 6;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.MMFT_MARKET_CODE,
            this.MMFT_ID,
            this.MMFT_KIND_ID,
            this.MMFT_BEGIN_MTH,
            this.MMFT_MTH_CNT,
            this.MMFT_IN_CNT,
            this.MMFT_OUT_CNT,
            this.MMFT_AVG_CNT,
            this.MMFT_PROD_TYPE,
            this.MMFT_REF_KIND_ID,
            this.MMFT_CP_KIND,
            this.MMFT_END_S,
            this.MMFT_END_E,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsCustomization.AllowSort = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // 
            // MMFT_MARKET_CODE
            // 
            this.MMFT_MARKET_CODE.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.MMFT_MARKET_CODE.AppearanceCell.Options.UseBackColor = true;
            this.MMFT_MARKET_CODE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MMFT_MARKET_CODE.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_MARKET_CODE.Caption = "交易時段";
            this.MMFT_MARKET_CODE.ColumnEdit = this.repositoryItemTextEdit2;
            this.MMFT_MARKET_CODE.FieldName = "MMFT_MARKET_CODE";
            this.MMFT_MARKET_CODE.MinWidth = 30;
            this.MMFT_MARKET_CODE.Name = "MMFT_MARKET_CODE";
            this.MMFT_MARKET_CODE.Visible = true;
            this.MMFT_MARKET_CODE.VisibleIndex = 0;
            this.MMFT_MARKET_CODE.Width = 128;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.MaxLength = 10;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // MMFT_ID
            // 
            this.MMFT_ID.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.MMFT_ID.AppearanceCell.Options.UseBackColor = true;
            this.MMFT_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MMFT_ID.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_ID.Caption = "設定代號";
            this.MMFT_ID.ColumnEdit = this.repositoryItemTextEdit2;
            this.MMFT_ID.FieldName = "MMFT_ID";
            this.MMFT_ID.MinWidth = 30;
            this.MMFT_ID.Name = "MMFT_ID";
            this.MMFT_ID.Visible = true;
            this.MMFT_ID.VisibleIndex = 1;
            this.MMFT_ID.Width = 127;
            // 
            // MMFT_KIND_ID
            // 
            this.MMFT_KIND_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMFT_KIND_ID.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_KIND_ID.Caption = "商品";
            this.MMFT_KIND_ID.ColumnEdit = this.repositoryItemTextEdit3;
            this.MMFT_KIND_ID.FieldName = "MMFT_KIND_ID";
            this.MMFT_KIND_ID.MinWidth = 30;
            this.MMFT_KIND_ID.Name = "MMFT_KIND_ID";
            this.MMFT_KIND_ID.Visible = true;
            this.MMFT_KIND_ID.VisibleIndex = 2;
            this.MMFT_KIND_ID.Width = 127;
            // 
            // repositoryItemTextEdit3
            // 
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.MaxLength = 7;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // MMFT_BEGIN_MTH
            // 
            this.MMFT_BEGIN_MTH.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMFT_BEGIN_MTH.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_BEGIN_MTH.Caption = "近月份起始月份";
            this.MMFT_BEGIN_MTH.ColumnEdit = this.repositoryItemTextEdit1;
            this.MMFT_BEGIN_MTH.FieldName = "MMFT_BEGIN_MTH";
            this.MMFT_BEGIN_MTH.MinWidth = 30;
            this.MMFT_BEGIN_MTH.Name = "MMFT_BEGIN_MTH";
            this.MMFT_BEGIN_MTH.Visible = true;
            this.MMFT_BEGIN_MTH.VisibleIndex = 3;
            this.MMFT_BEGIN_MTH.Width = 182;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit1.MaxLength = 6;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // MMFT_MTH_CNT
            // 
            this.MMFT_MTH_CNT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMFT_MTH_CNT.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_MTH_CNT.Caption = "符合月份個數";
            this.MMFT_MTH_CNT.ColumnEdit = this.repositoryItemTextEdit1;
            this.MMFT_MTH_CNT.FieldName = "MMFT_MTH_CNT";
            this.MMFT_MTH_CNT.MinWidth = 30;
            this.MMFT_MTH_CNT.Name = "MMFT_MTH_CNT";
            this.MMFT_MTH_CNT.Visible = true;
            this.MMFT_MTH_CNT.VisibleIndex = 4;
            this.MMFT_MTH_CNT.Width = 158;
            // 
            // MMFT_IN_CNT
            // 
            this.MMFT_IN_CNT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMFT_IN_CNT.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_IN_CNT.Caption = "價內個數";
            this.MMFT_IN_CNT.ColumnEdit = this.repositoryItemTextEdit1;
            this.MMFT_IN_CNT.FieldName = "MMFT_IN_CNT";
            this.MMFT_IN_CNT.MinWidth = 30;
            this.MMFT_IN_CNT.Name = "MMFT_IN_CNT";
            this.MMFT_IN_CNT.Visible = true;
            this.MMFT_IN_CNT.VisibleIndex = 5;
            this.MMFT_IN_CNT.Width = 119;
            // 
            // MMFT_OUT_CNT
            // 
            this.MMFT_OUT_CNT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMFT_OUT_CNT.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_OUT_CNT.Caption = "價外個數";
            this.MMFT_OUT_CNT.ColumnEdit = this.repositoryItemTextEdit1;
            this.MMFT_OUT_CNT.FieldName = "MMFT_OUT_CNT";
            this.MMFT_OUT_CNT.MinWidth = 30;
            this.MMFT_OUT_CNT.Name = "MMFT_OUT_CNT";
            this.MMFT_OUT_CNT.Visible = true;
            this.MMFT_OUT_CNT.VisibleIndex = 6;
            this.MMFT_OUT_CNT.Width = 119;
            // 
            // MMFT_AVG_CNT
            // 
            this.MMFT_AVG_CNT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMFT_AVG_CNT.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_AVG_CNT.Caption = "計算序列數";
            this.MMFT_AVG_CNT.ColumnEdit = this.repositoryItemTextEdit1;
            this.MMFT_AVG_CNT.FieldName = "MMFT_AVG_CNT";
            this.MMFT_AVG_CNT.MinWidth = 30;
            this.MMFT_AVG_CNT.Name = "MMFT_AVG_CNT";
            this.MMFT_AVG_CNT.Visible = true;
            this.MMFT_AVG_CNT.VisibleIndex = 7;
            this.MMFT_AVG_CNT.Width = 134;
            // 
            // MMFT_PROD_TYPE
            // 
            this.MMFT_PROD_TYPE.AppearanceCell.Options.UseTextOptions = true;
            this.MMFT_PROD_TYPE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MMFT_PROD_TYPE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMFT_PROD_TYPE.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_PROD_TYPE.Caption = "商品別";
            this.MMFT_PROD_TYPE.ColumnEdit = this.repositoryItemTextEdit4;
            this.MMFT_PROD_TYPE.FieldName = "MMFT_PROD_TYPE";
            this.MMFT_PROD_TYPE.MinWidth = 30;
            this.MMFT_PROD_TYPE.Name = "MMFT_PROD_TYPE";
            this.MMFT_PROD_TYPE.Visible = true;
            this.MMFT_PROD_TYPE.VisibleIndex = 8;
            this.MMFT_PROD_TYPE.Width = 119;
            // 
            // repositoryItemTextEdit4
            // 
            this.repositoryItemTextEdit4.AutoHeight = false;
            this.repositoryItemTextEdit4.MaxLength = 1;
            this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
            // 
            // MMFT_REF_KIND_ID
            // 
            this.MMFT_REF_KIND_ID.AppearanceCell.Options.UseTextOptions = true;
            this.MMFT_REF_KIND_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MMFT_REF_KIND_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMFT_REF_KIND_ID.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_REF_KIND_ID.Caption = "標的商品";
            this.MMFT_REF_KIND_ID.ColumnEdit = this.repositoryItemTextEdit3;
            this.MMFT_REF_KIND_ID.FieldName = "MMFT_REF_KIND_ID";
            this.MMFT_REF_KIND_ID.MinWidth = 30;
            this.MMFT_REF_KIND_ID.Name = "MMFT_REF_KIND_ID";
            this.MMFT_REF_KIND_ID.Visible = true;
            this.MMFT_REF_KIND_ID.VisibleIndex = 9;
            this.MMFT_REF_KIND_ID.Width = 119;
            // 
            // MMFT_CP_KIND
            // 
            this.MMFT_CP_KIND.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMFT_CP_KIND.AppearanceHeader.Options.UseBackColor = true;
            this.MMFT_CP_KIND.Caption = "價平月份";
            this.MMFT_CP_KIND.FieldName = "MMFT_CP_KIND";
            this.MMFT_CP_KIND.MinWidth = 30;
            this.MMFT_CP_KIND.Name = "MMFT_CP_KIND";
            this.MMFT_CP_KIND.Visible = true;
            this.MMFT_CP_KIND.VisibleIndex = 10;
            this.MMFT_CP_KIND.Width = 125;
            // 
            // MMFT_END_S
            // 
            this.MMFT_END_S.Caption = "gridColumn1";
            this.MMFT_END_S.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.MMFT_END_S.FieldName = "MMFT_END_S";
            this.MMFT_END_S.MinWidth = 30;
            this.MMFT_END_S.Name = "MMFT_END_S";
            this.MMFT_END_S.Width = 112;
            // 
            // MMFT_END_E
            // 
            this.MMFT_END_E.Caption = "gridColumn1";
            this.MMFT_END_E.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.MMFT_END_E.FieldName = "MMFT_END_E";
            this.MMFT_END_E.MinWidth = 30;
            this.MMFT_END_E.Name = "MMFT_END_E";
            this.MMFT_END_E.Width = 112;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.MinWidth = 30;
            this.Is_NewRow.Name = "Is_NewRow";
            this.Is_NewRow.Width = 112;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1375, 182);
            this.panel1.TabIndex = 16;
            // 
            // W51020
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1379, 738);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W51020";
            this.Text = "W51020";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_MARKET_CODE;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_ID;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_KIND_ID;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_BEGIN_MTH;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_MTH_CNT;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_IN_CNT;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_OUT_CNT;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_AVG_CNT;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_PROD_TYPE;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_REF_KIND_ID;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_CP_KIND;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_END_S;
        private DevExpress.XtraGrid.Columns.GridColumn MMFT_END_E;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
        private System.Windows.Forms.Panel panel1;
    }
}