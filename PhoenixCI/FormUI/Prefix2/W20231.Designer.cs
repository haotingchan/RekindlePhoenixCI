namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20231 {
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
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDate = new BaseGround.Widget.TextDateEdit();
            this.txtProdDate = new BaseGround.Widget.TextDateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.PLS4_SID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLS4_KIND_ID2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLS4_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLS4_FUT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLS4_OPT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLS4_PDK_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLS4_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLS4_W_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLS4_STATUS_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLS4_PID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.APDK_KIND_GRP2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COMPUTE_1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProdDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Size = new System.Drawing.Size(1069, 693);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1069, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnAdd);
            this.panelControl1.Controls.Add(this.btnCopy);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.txtProdDate);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.txtDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1069, 110);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcMain);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 140);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1069, 583);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(2, 2);
            this.gcMain.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.gcMain.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1065, 579);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.COMPUTE_1,
            this.PLS4_YMD,
            this.PLS4_SID,
            this.PLS4_KIND_ID2,
            this.PLS4_FUT,
            this.PLS4_OPT,
            this.PLS4_STATUS_CODE,
            this.PLS4_PDK_YMD,
            this.PLS4_PID,
            this.PLS4_W_TIME,
            this.PLS4_W_USER_ID,
            this.APDK_KIND_GRP2,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ShowGroupPanel = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(43, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 21);
            this.label2.TabIndex = 22;
            this.label2.Text = "計算日期：";
            // 
            // txtDate
            // 
            this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtDate.EditValue = "2018/12";
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(138, 28);
            this.txtDate.MenuManager = this.ribbonControl;
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDate.Size = new System.Drawing.Size(105, 26);
            this.txtDate.TabIndex = 21;
            this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtProdDate
            // 
            this.txtProdDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtProdDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtProdDate.EditValue = "2018/12";
            this.txtProdDate.EnterMoveNextControl = true;
            this.txtProdDate.Location = new System.Drawing.Point(622, 28);
            this.txtProdDate.MenuManager = this.ribbonControl;
            this.txtProdDate.Name = "txtProdDate";
            this.txtProdDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtProdDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtProdDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtProdDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtProdDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtProdDate.Size = new System.Drawing.Size(105, 26);
            this.txtProdDate.TabIndex = 23;
            this.txtProdDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(375, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(241, 21);
            this.label3.TabIndex = 24;
            this.label3.Text = "比對期貨/選擇權商品基準日期：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(43, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(202, 21);
            this.label5.TabIndex = 25;
            this.label5.Text = "新上市股票期貨，需先執行";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(618, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 21);
            this.label1.TabIndex = 26;
            this.label1.Text = "(轉入商品時一定要輸入)";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(733, 26);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(118, 31);
            this.btnCopy.TabIndex = 27;
            this.btnCopy.Text = "複製全部個股";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(251, 62);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(118, 31);
            this.btnAdd.TabIndex = 29;
            this.btnAdd.Text = "新增個股契約";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // PLS4_SID
            // 
            this.PLS4_SID.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
            this.PLS4_SID.AppearanceHeader.Options.UseBackColor = true;
            this.PLS4_SID.AppearanceHeader.Options.UseTextOptions = true;
            this.PLS4_SID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PLS4_SID.Caption = "股票代號";
            this.PLS4_SID.FieldName = "PLS4_SID";
            this.PLS4_SID.Name = "PLS4_SID";
            this.PLS4_SID.Visible = true;
            this.PLS4_SID.VisibleIndex = 2;
            // 
            // PLS4_KIND_ID2
            // 
            this.PLS4_KIND_ID2.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
            this.PLS4_KIND_ID2.AppearanceHeader.Options.UseBackColor = true;
            this.PLS4_KIND_ID2.AppearanceHeader.Options.UseTextOptions = true;
            this.PLS4_KIND_ID2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PLS4_KIND_ID2.Caption = "個股商品2碼";
            this.PLS4_KIND_ID2.FieldName = "PLS4_KIND_ID2";
            this.PLS4_KIND_ID2.Name = "PLS4_KIND_ID2";
            this.PLS4_KIND_ID2.Visible = true;
            this.PLS4_KIND_ID2.VisibleIndex = 3;
            // 
            // PLS4_YMD
            // 
            this.PLS4_YMD.AppearanceHeader.BackColor = System.Drawing.Color.Transparent;
            this.PLS4_YMD.AppearanceHeader.Options.UseBackColor = true;
            this.PLS4_YMD.AppearanceHeader.Options.UseTextOptions = true;
            this.PLS4_YMD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PLS4_YMD.Caption = "計算日期";
            this.PLS4_YMD.FieldName = "PLS4_YMD";
            this.PLS4_YMD.Name = "PLS4_YMD";
            this.PLS4_YMD.Visible = true;
            this.PLS4_YMD.VisibleIndex = 1;
            // 
            // PLS4_FUT
            // 
            this.PLS4_FUT.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
            this.PLS4_FUT.AppearanceHeader.Options.UseBackColor = true;
            this.PLS4_FUT.AppearanceHeader.Options.UseTextOptions = true;
            this.PLS4_FUT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PLS4_FUT.Caption = "期貨";
            this.PLS4_FUT.FieldName = "PLS4_FUT";
            this.PLS4_FUT.Name = "PLS4_FUT";
            this.PLS4_FUT.Visible = true;
            this.PLS4_FUT.VisibleIndex = 4;
            // 
            // PLS4_OPT
            // 
            this.PLS4_OPT.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
            this.PLS4_OPT.AppearanceHeader.Options.UseBackColor = true;
            this.PLS4_OPT.AppearanceHeader.Options.UseTextOptions = true;
            this.PLS4_OPT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PLS4_OPT.Caption = "選擇權";
            this.PLS4_OPT.FieldName = "PLS4_OPT";
            this.PLS4_OPT.Name = "PLS4_OPT";
            this.PLS4_OPT.Visible = true;
            this.PLS4_OPT.VisibleIndex = 5;
            // 
            // PLS4_PDK_YMD
            // 
            this.PLS4_PDK_YMD.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
            this.PLS4_PDK_YMD.AppearanceHeader.Options.UseBackColor = true;
            this.PLS4_PDK_YMD.AppearanceHeader.Options.UseTextOptions = true;
            this.PLS4_PDK_YMD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PLS4_PDK_YMD.Caption = "商品比對日期";
            this.PLS4_PDK_YMD.FieldName = "PLS4_PDK_YMD";
            this.PLS4_PDK_YMD.Name = "PLS4_PDK_YMD";
            this.PLS4_PDK_YMD.Visible = true;
            this.PLS4_PDK_YMD.VisibleIndex = 8;
            // 
            // PLS4_W_TIME
            // 
            this.PLS4_W_TIME.Caption = "PLS4_W_TIME";
            this.PLS4_W_TIME.FieldName = "PLS4_W_TIME";
            this.PLS4_W_TIME.Name = "PLS4_W_TIME";
            // 
            // PLS4_W_USER_ID
            // 
            this.PLS4_W_USER_ID.Caption = "PLS4_W_USER_ID";
            this.PLS4_W_USER_ID.FieldName = "PLS4_W_USER_ID";
            this.PLS4_W_USER_ID.Name = "PLS4_W_USER_ID";
            // 
            // PLS4_STATUS_CODE
            // 
            this.PLS4_STATUS_CODE.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
            this.PLS4_STATUS_CODE.AppearanceHeader.Options.UseBackColor = true;
            this.PLS4_STATUS_CODE.AppearanceHeader.Options.UseTextOptions = true;
            this.PLS4_STATUS_CODE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PLS4_STATUS_CODE.Caption = "商品狀態";
            this.PLS4_STATUS_CODE.FieldName = "PLS4_STATUS_CODE";
            this.PLS4_STATUS_CODE.Name = "PLS4_STATUS_CODE";
            this.PLS4_STATUS_CODE.Visible = true;
            this.PLS4_STATUS_CODE.VisibleIndex = 6;
            // 
            // PLS4_PID
            // 
            this.PLS4_PID.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
            this.PLS4_PID.AppearanceHeader.Options.UseBackColor = true;
            this.PLS4_PID.AppearanceHeader.Options.UseTextOptions = true;
            this.PLS4_PID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PLS4_PID.Caption = "上市/上櫃";
            this.PLS4_PID.FieldName = "PLS4_PID";
            this.PLS4_PID.Name = "PLS4_PID";
            this.PLS4_PID.Visible = true;
            this.PLS4_PID.VisibleIndex = 7;
            // 
            // APDK_KIND_GRP2
            // 
            this.APDK_KIND_GRP2.Caption = "APDK_KIND_GRP2";
            this.APDK_KIND_GRP2.FieldName = "APDK_KIND_GRP2";
            this.APDK_KIND_GRP2.Name = "APDK_KIND_GRP2";
            // 
            // COMPUTE_1
            // 
            this.COMPUTE_1.AppearanceHeader.BackColor = System.Drawing.Color.Transparent;
            this.COMPUTE_1.AppearanceHeader.Options.UseBackColor = true;
            this.COMPUTE_1.AppearanceHeader.Options.UseTextOptions = true;
            this.COMPUTE_1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.COMPUTE_1.Caption = "流水號";
            this.COMPUTE_1.FieldName = "COMPUTE_1";
            this.COMPUTE_1.Name = "COMPUTE_1";
            this.COMPUTE_1.Visible = true;
            this.COMPUTE_1.VisibleIndex = 0;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // W20231
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 723);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W20231";
            this.Text = "W20231";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProdDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label label2;
        private BaseGround.Widget.TextDateEdit txtDate;
        private BaseGround.Widget.TextDateEdit txtProdDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnAdd;
        private DevExpress.XtraGrid.Columns.GridColumn COMPUTE_1;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_YMD;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_SID;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_KIND_ID2;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_FUT;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_OPT;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_STATUS_CODE;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_PDK_YMD;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_PID;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_W_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_W_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn APDK_KIND_GRP2;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
    }
}