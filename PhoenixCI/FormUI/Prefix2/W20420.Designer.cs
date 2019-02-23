namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20420 {
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.dw_txn_id = new DevExpress.XtraEditors.LookUpEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.IDFG_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IDFG_ACC_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dw_txn_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.panelControl2);
            this.panParent.Location = new System.Drawing.Point(0, 116);
            this.panParent.Size = new System.Drawing.Size(899, 513);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(899, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.dw_txn_id);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(899, 86);
            this.panelControl1.TabIndex = 0;
            // 
            // dw_txn_id
            // 
            this.dw_txn_id.Location = new System.Drawing.Point(87, 42);
            this.dw_txn_id.MenuManager = this.ribbonControl;
            this.dw_txn_id.Name = "dw_txn_id";
            this.dw_txn_id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dw_txn_id.Size = new System.Drawing.Size(285, 26);
            this.dw_txn_id.TabIndex = 3;
            this.dw_txn_id.EditValueChanged += new System.EventHandler(this.dw_txn_id_EditValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(560, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(294, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "（若有調整，請再執行當月28510作業）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "報表別：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(546, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "調整身份碼將影響 [28510－每月買賣比重資料轉入(結6250)]作業之統計轉檔";
            // 
            // panelControl2
            // 
            this.panelControl2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
            this.panelControl2.Appearance.Options.UseBackColor = true;
            this.panelControl2.Controls.Add(this.gcMain);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(12, 12);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(875, 489);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(2, 2);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(871, 485);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
            this.gvMain.Appearance.Empty.Options.UseBackColor = true;
            this.gvMain.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Maroon;
            this.gvMain.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.IDFG_TYPE,
            this.IDFG_ACC_CODE,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // IDFG_TYPE
            // 
            this.IDFG_TYPE.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
            this.IDFG_TYPE.AppearanceHeader.ForeColor = System.Drawing.Color.Maroon;
            this.IDFG_TYPE.AppearanceHeader.Options.UseBackColor = true;
            this.IDFG_TYPE.AppearanceHeader.Options.UseForeColor = true;
            this.IDFG_TYPE.AppearanceHeader.Options.UseTextOptions = true;
            this.IDFG_TYPE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.IDFG_TYPE.Caption = "身份碼歸屬";
            this.IDFG_TYPE.FieldName = "IDFG_TYPE";
            this.IDFG_TYPE.Name = "IDFG_TYPE";
            this.IDFG_TYPE.Visible = true;
            this.IDFG_TYPE.VisibleIndex = 0;
            this.IDFG_TYPE.Width = 200;
            // 
            // IDFG_ACC_CODE
            // 
            this.IDFG_ACC_CODE.AppearanceCell.Options.UseTextOptions = true;
            this.IDFG_ACC_CODE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.IDFG_ACC_CODE.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
            this.IDFG_ACC_CODE.AppearanceHeader.ForeColor = System.Drawing.Color.Maroon;
            this.IDFG_ACC_CODE.AppearanceHeader.Options.UseBackColor = true;
            this.IDFG_ACC_CODE.AppearanceHeader.Options.UseForeColor = true;
            this.IDFG_ACC_CODE.AppearanceHeader.Options.UseTextOptions = true;
            this.IDFG_ACC_CODE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.IDFG_ACC_CODE.Caption = "身份碼";
            this.IDFG_ACC_CODE.FieldName = "IDFG_ACC_CODE";
            this.IDFG_ACC_CODE.Name = "IDFG_ACC_CODE";
            this.IDFG_ACC_CODE.Visible = true;
            this.IDFG_ACC_CODE.VisibleIndex = 1;
            this.IDFG_ACC_CODE.Width = 100;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // W20420
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 629);
            this.Controls.Add(this.panelControl1);
            this.Name = "W20420";
            this.Text = "W20420";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dw_txn_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraEditors.LookUpEdit dw_txn_id;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.Columns.GridColumn IDFG_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn IDFG_ACC_CODE;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
    }
}