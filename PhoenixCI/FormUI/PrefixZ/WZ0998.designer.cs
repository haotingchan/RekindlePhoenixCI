namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0998
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
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RPT_TXN_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_TXD_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_SEQ_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_VALUE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_LEVEL_1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_LEVEL_2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_LEVEL_3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_LEVEL_4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_LEVEL_CNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_VALUE_2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_VALUE_3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_VALUE_4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_VALUE_5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblTXN = new System.Windows.Forms.Label();
            this.lblDpt = new System.Windows.Forms.Label();
            this.ddlTxnId = new DevExpress.XtraEditors.LookUpEdit();
            this.txtTxdId = new DevExpress.XtraEditors.TextEdit();
            this.txtSeq = new DevExpress.XtraEditors.TextEdit();
            this.lblSeq = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTxnId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTxdId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeq.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.txtSeq);
            this.panParent.Controls.Add(this.lblSeq);
            this.panParent.Controls.Add(this.txtTxdId);
            this.panParent.Controls.Add(this.ddlTxnId);
            this.panParent.Controls.Add(this.lblDpt);
            this.panParent.Controls.Add(this.lblTXN);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(1180, 538);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1180, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcMain.Location = new System.Drawing.Point(12, 43);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1156, 481);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.ColumnPanelRowHeight = 30;
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.RPT_TXN_ID,
            this.RPT_TXD_ID,
            this.RPT_SEQ_NO,
            this.RPT_VALUE,
            this.RPT_LEVEL_1,
            this.RPT_LEVEL_2,
            this.RPT_LEVEL_3,
            this.RPT_LEVEL_4,
            this.RPT_TYPE,
            this.RPT_LEVEL_CNT,
            this.RPT_VALUE_2,
            this.RPT_VALUE_3,
            this.RPT_VALUE_4,
            this.RPT_VALUE_5});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            // 
            // RPT_TXN_ID
            // 
            this.RPT_TXN_ID.Caption = "作業代號";
            this.RPT_TXN_ID.FieldName = "RPT_TXN_ID";
            this.RPT_TXN_ID.Name = "RPT_TXN_ID";
            this.RPT_TXN_ID.Visible = true;
            this.RPT_TXN_ID.VisibleIndex = 0;
            // 
            // RPT_TXD_ID
            // 
            this.RPT_TXD_ID.Caption = "作業子代號";
            this.RPT_TXD_ID.FieldName = "RPT_TXD_ID";
            this.RPT_TXD_ID.Name = "RPT_TXD_ID";
            this.RPT_TXD_ID.Visible = true;
            this.RPT_TXD_ID.VisibleIndex = 1;
            // 
            // RPT_SEQ_NO
            // 
            this.RPT_SEQ_NO.Caption = "排序";
            this.RPT_SEQ_NO.FieldName = "RPT_SEQ_NO";
            this.RPT_SEQ_NO.Name = "RPT_SEQ_NO";
            this.RPT_SEQ_NO.Visible = true;
            this.RPT_SEQ_NO.VisibleIndex = 2;
            // 
            // RPT_VALUE
            // 
            this.RPT_VALUE.Caption = "判斷值";
            this.RPT_VALUE.FieldName = "RPT_VALUE";
            this.RPT_VALUE.Name = "RPT_VALUE";
            this.RPT_VALUE.Visible = true;
            this.RPT_VALUE.VisibleIndex = 3;
            // 
            // RPT_LEVEL_1
            // 
            this.RPT_LEVEL_1.Caption = "Level 1";
            this.RPT_LEVEL_1.FieldName = "RPT_LEVEL_1";
            this.RPT_LEVEL_1.Name = "RPT_LEVEL_1";
            this.RPT_LEVEL_1.Visible = true;
            this.RPT_LEVEL_1.VisibleIndex = 4;
            // 
            // RPT_LEVEL_2
            // 
            this.RPT_LEVEL_2.Caption = "Level 2";
            this.RPT_LEVEL_2.FieldName = "RPT_LEVEL_2";
            this.RPT_LEVEL_2.Name = "RPT_LEVEL_2";
            this.RPT_LEVEL_2.Visible = true;
            this.RPT_LEVEL_2.VisibleIndex = 5;
            // 
            // RPT_LEVEL_3
            // 
            this.RPT_LEVEL_3.Caption = "Level 3";
            this.RPT_LEVEL_3.FieldName = "RPT_LEVEL_3";
            this.RPT_LEVEL_3.Name = "RPT_LEVEL_3";
            this.RPT_LEVEL_3.Visible = true;
            this.RPT_LEVEL_3.VisibleIndex = 6;
            // 
            // RPT_LEVEL_4
            // 
            this.RPT_LEVEL_4.Caption = "Level 4";
            this.RPT_LEVEL_4.FieldName = "RPT_LEVEL_4";
            this.RPT_LEVEL_4.Name = "RPT_LEVEL_4";
            this.RPT_LEVEL_4.Visible = true;
            this.RPT_LEVEL_4.VisibleIndex = 7;
            // 
            // RPT_TYPE
            // 
            this.RPT_TYPE.Caption = "類型";
            this.RPT_TYPE.FieldName = "RPT_TYPE";
            this.RPT_TYPE.Name = "RPT_TYPE";
            this.RPT_TYPE.Visible = true;
            this.RPT_TYPE.VisibleIndex = 8;
            // 
            // RPT_LEVEL_CNT
            // 
            this.RPT_LEVEL_CNT.Caption = "Level 數";
            this.RPT_LEVEL_CNT.FieldName = "RPT_LEVEL_CNT";
            this.RPT_LEVEL_CNT.Name = "RPT_LEVEL_CNT";
            this.RPT_LEVEL_CNT.Visible = true;
            this.RPT_LEVEL_CNT.VisibleIndex = 9;
            // 
            // RPT_VALUE_2
            // 
            this.RPT_VALUE_2.Caption = "值2";
            this.RPT_VALUE_2.FieldName = "RPT_VALUE_2";
            this.RPT_VALUE_2.Name = "RPT_VALUE_2";
            this.RPT_VALUE_2.Visible = true;
            this.RPT_VALUE_2.VisibleIndex = 10;
            // 
            // RPT_VALUE_3
            // 
            this.RPT_VALUE_3.Caption = "值3";
            this.RPT_VALUE_3.FieldName = "RPT_VALUE_3";
            this.RPT_VALUE_3.Name = "RPT_VALUE_3";
            this.RPT_VALUE_3.Visible = true;
            this.RPT_VALUE_3.VisibleIndex = 11;
            // 
            // RPT_VALUE_4
            // 
            this.RPT_VALUE_4.Caption = "值4";
            this.RPT_VALUE_4.FieldName = "RPT_VALUE_4";
            this.RPT_VALUE_4.Name = "RPT_VALUE_4";
            this.RPT_VALUE_4.Visible = true;
            this.RPT_VALUE_4.VisibleIndex = 12;
            // 
            // RPT_VALUE_5
            // 
            this.RPT_VALUE_5.Caption = "值5";
            this.RPT_VALUE_5.FieldName = "RPT_VALUE_5";
            this.RPT_VALUE_5.Name = "RPT_VALUE_5";
            this.RPT_VALUE_5.Visible = true;
            this.RPT_VALUE_5.VisibleIndex = 13;
            // 
            // lblTXN
            // 
            this.lblTXN.AutoSize = true;
            this.lblTXN.Location = new System.Drawing.Point(8, 12);
            this.lblTXN.Name = "lblTXN";
            this.lblTXN.Size = new System.Drawing.Size(89, 20);
            this.lblTXN.TabIndex = 6;
            this.lblTXN.Text = "作業代號：";
            // 
            // lblDpt
            // 
            this.lblDpt.AutoSize = true;
            this.lblDpt.Location = new System.Drawing.Point(566, 12);
            this.lblDpt.Name = "lblDpt";
            this.lblDpt.Size = new System.Drawing.Size(105, 20);
            this.lblDpt.TabIndex = 9;
            this.lblDpt.Text = "作業子代號：";
            // 
            // ddlTxnId
            // 
            this.ddlTxnId.Location = new System.Drawing.Point(94, 9);
            this.ddlTxnId.MenuManager = this.ribbonControl;
            this.ddlTxnId.Name = "ddlTxnId";
            this.ddlTxnId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlTxnId.Size = new System.Drawing.Size(466, 26);
            this.ddlTxnId.TabIndex = 15;
            // 
            // txtTxdId
            // 
            this.txtTxdId.Location = new System.Drawing.Point(677, 9);
            this.txtTxdId.MenuManager = this.ribbonControl;
            this.txtTxdId.Name = "txtTxdId";
            this.txtTxdId.Size = new System.Drawing.Size(100, 26);
            this.txtTxdId.TabIndex = 16;
            // 
            // txtSeq
            // 
            this.txtSeq.Location = new System.Drawing.Point(878, 9);
            this.txtSeq.MenuManager = this.ribbonControl;
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Size = new System.Drawing.Size(287, 26);
            this.txtSeq.TabIndex = 18;
            // 
            // lblSeq
            // 
            this.lblSeq.AutoSize = true;
            this.lblSeq.Location = new System.Drawing.Point(792, 12);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(89, 20);
            this.lblSeq.TabIndex = 17;
            this.lblSeq.Text = "排序欄位：";
            // 
            // WZ0998
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 568);
            this.Name = "WZ0998";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTxnId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTxdId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeq.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label lblTXN;
        private System.Windows.Forms.Label lblDpt;
        private DevExpress.XtraEditors.LookUpEdit ddlTxnId;
        private DevExpress.XtraEditors.TextEdit txtTxdId;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_TXN_ID;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_TXD_ID;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_SEQ_NO;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_VALUE;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_LEVEL_1;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_LEVEL_2;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_LEVEL_3;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_LEVEL_4;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_LEVEL_CNT;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_VALUE_2;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_VALUE_3;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_VALUE_4;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_VALUE_5;
        private DevExpress.XtraEditors.TextEdit txtSeq;
        private System.Windows.Forms.Label lblSeq;
    }
}