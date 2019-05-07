﻿namespace PhoenixCI.FormUI.Prefix2 {
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
         this.btnAdd = new System.Windows.Forms.Button();
         this.btnCopy = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.emDate = new BaseGround.Widget.TextDateEdit();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.PLS4_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
         this.PLS4_SID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.PLS4_KIND_ID2 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.PLS4_FUT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.PLS4_OPT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.PLS4_STATUS_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.PLS4_PID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.PLS4_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.PLS4_W_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.APDK_KIND_GRP2 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
         this.emProdDate = new DevExpress.XtraEditors.TextEdit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.emDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         this.panelControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emProdDate.Properties)).BeginInit();
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
         this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panelControl1.Appearance.Options.UseBackColor = true;
         this.panelControl1.Controls.Add(this.btnAdd);
         this.panelControl1.Controls.Add(this.btnCopy);
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Controls.Add(this.label5);
         this.panelControl1.Controls.Add(this.label3);
         this.panelControl1.Controls.Add(this.label2);
         this.panelControl1.Controls.Add(this.emDate);
         this.panelControl1.Controls.Add(this.emProdDate);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(1069, 110);
         this.panelControl1.TabIndex = 0;
         // 
         // btnAdd
         // 
         this.btnAdd.Location = new System.Drawing.Point(251, 62);
         this.btnAdd.Name = "btnAdd";
         this.btnAdd.Size = new System.Drawing.Size(118, 31);
         this.btnAdd.TabIndex = 29;
         this.btnAdd.Text = "新增個股契約";
         this.btnAdd.UseVisualStyleBackColor = true;
         this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
         // 
         // btnCopy
         // 
         this.btnCopy.Location = new System.Drawing.Point(733, 26);
         this.btnCopy.Name = "btnCopy";
         this.btnCopy.Size = new System.Drawing.Size(118, 31);
         this.btnCopy.TabIndex = 27;
         this.btnCopy.Text = "複製全部個股";
         this.btnCopy.UseVisualStyleBackColor = true;
         this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
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
         // emDate
         // 
         this.emDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.emDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.emDate.EditValue = "2018/12/01";
         this.emDate.EnterMoveNextControl = true;
         this.emDate.Location = new System.Drawing.Point(138, 28);
         this.emDate.MenuManager = this.ribbonControl;
         this.emDate.Name = "emDate";
         this.emDate.Properties.Appearance.Options.UseTextOptions = true;
         this.emDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.emDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.emDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.emDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.emDate.Properties.Mask.ShowPlaceHolders = false;
         this.emDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.emDate.Size = new System.Drawing.Size(105, 28);
         this.emDate.TabIndex = 21;
         this.emDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // panelControl2
         // 
         this.panelControl2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.panelControl2.Appearance.Options.UseBackColor = true;
         this.panelControl2.Controls.Add(this.gcMain);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 140);
         this.panelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(1069, 583);
         this.panelControl2.TabIndex = 1;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.gcMain.EmbeddedNavigator.Appearance.BorderColor = System.Drawing.Color.Black;
         this.gcMain.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
         this.gcMain.EmbeddedNavigator.Appearance.Options.UseBorderColor = true;
         this.gcMain.Location = new System.Drawing.Point(3, 3);
         this.gcMain.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.gcMain.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(1063, 577);
         this.gcMain.TabIndex = 0;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.Appearance.HorzLine.BackColor = System.Drawing.Color.Black;
         this.gvMain.Appearance.HorzLine.Options.UseBackColor = true;
         this.gvMain.Appearance.VertLine.BackColor = System.Drawing.Color.Black;
         this.gvMain.Appearance.VertLine.Options.UseBackColor = true;
         this.gvMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PLS4_YMD,
            this.PLS4_SID,
            this.PLS4_KIND_ID2,
            this.PLS4_FUT,
            this.PLS4_OPT,
            this.PLS4_STATUS_CODE,
            this.PLS4_PID,
            this.PLS4_W_TIME,
            this.PLS4_W_USER_ID,
            this.APDK_KIND_GRP2,
            this.Is_NewRow});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         this.gvMain.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
         this.gvMain.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.True;
         // 
         // PLS4_YMD
         // 
         this.PLS4_YMD.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.PLS4_YMD.AppearanceCell.BorderColor = System.Drawing.Color.Black;
         this.PLS4_YMD.AppearanceCell.Options.UseBackColor = true;
         this.PLS4_YMD.AppearanceCell.Options.UseBorderColor = true;
         this.PLS4_YMD.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.PLS4_YMD.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
         this.PLS4_YMD.AppearanceHeader.Options.UseBackColor = true;
         this.PLS4_YMD.AppearanceHeader.Options.UseForeColor = true;
         this.PLS4_YMD.AppearanceHeader.Options.UseTextOptions = true;
         this.PLS4_YMD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.PLS4_YMD.Caption = "計算日期";
         this.PLS4_YMD.FieldName = "PLS4_YMD";
         this.PLS4_YMD.Name = "PLS4_YMD";
         this.PLS4_YMD.OptionsColumn.AllowEdit = false;
         this.PLS4_YMD.Visible = true;
         this.PLS4_YMD.VisibleIndex = 0;
         this.PLS4_YMD.Width = 91;
         // 
         // PLS4_SID
         // 
         this.PLS4_SID.AppearanceCell.BackColor = System.Drawing.Color.Silver;
         this.PLS4_SID.AppearanceCell.Options.UseBackColor = true;
         this.PLS4_SID.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.PLS4_SID.AppearanceHeader.Options.UseBackColor = true;
         this.PLS4_SID.AppearanceHeader.Options.UseTextOptions = true;
         this.PLS4_SID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.PLS4_SID.Caption = "股票代號";
         this.PLS4_SID.FieldName = "PLS4_SID";
         this.PLS4_SID.Name = "PLS4_SID";
         this.PLS4_SID.OptionsColumn.AllowEdit = false;
         this.PLS4_SID.Visible = true;
         this.PLS4_SID.VisibleIndex = 1;
         this.PLS4_SID.Width = 91;
         // 
         // PLS4_KIND_ID2
         // 
         this.PLS4_KIND_ID2.AppearanceCell.BackColor = System.Drawing.Color.Silver;
         this.PLS4_KIND_ID2.AppearanceCell.Options.UseBackColor = true;
         this.PLS4_KIND_ID2.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
         this.PLS4_KIND_ID2.AppearanceHeader.Options.UseBackColor = true;
         this.PLS4_KIND_ID2.AppearanceHeader.Options.UseTextOptions = true;
         this.PLS4_KIND_ID2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.PLS4_KIND_ID2.Caption = "個股商品2碼";
         this.PLS4_KIND_ID2.FieldName = "PLS4_KIND_ID2";
         this.PLS4_KIND_ID2.Name = "PLS4_KIND_ID2";
         this.PLS4_KIND_ID2.Visible = true;
         this.PLS4_KIND_ID2.VisibleIndex = 2;
         this.PLS4_KIND_ID2.Width = 103;
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
         this.PLS4_FUT.VisibleIndex = 3;
         this.PLS4_FUT.Width = 73;
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
         this.PLS4_OPT.VisibleIndex = 4;
         this.PLS4_OPT.Width = 73;
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
         this.PLS4_STATUS_CODE.VisibleIndex = 5;
         this.PLS4_STATUS_CODE.Width = 106;
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
         this.PLS4_PID.VisibleIndex = 6;
         this.PLS4_PID.Width = 85;
         // 
         // PLS4_W_TIME
         // 
         this.PLS4_W_TIME.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.PLS4_W_TIME.AppearanceCell.Options.UseBackColor = true;
         this.PLS4_W_TIME.Caption = "PLS4_W_TIME";
         this.PLS4_W_TIME.FieldName = "PLS4_W_TIME";
         this.PLS4_W_TIME.Name = "PLS4_W_TIME";
         // 
         // PLS4_W_USER_ID
         // 
         this.PLS4_W_USER_ID.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.PLS4_W_USER_ID.AppearanceCell.Options.UseBackColor = true;
         this.PLS4_W_USER_ID.Caption = "PLS4_W_USER_ID";
         this.PLS4_W_USER_ID.FieldName = "PLS4_W_USER_ID";
         this.PLS4_W_USER_ID.Name = "PLS4_W_USER_ID";
         // 
         // APDK_KIND_GRP2
         // 
         this.APDK_KIND_GRP2.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.APDK_KIND_GRP2.AppearanceCell.Options.UseBackColor = true;
         this.APDK_KIND_GRP2.Caption = "APDK_KIND_GRP2";
         this.APDK_KIND_GRP2.FieldName = "APDK_KIND_GRP2";
         this.APDK_KIND_GRP2.Name = "APDK_KIND_GRP2";
         // 
         // Is_NewRow
         // 
         this.Is_NewRow.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.Is_NewRow.AppearanceCell.Options.UseBackColor = true;
         this.Is_NewRow.Caption = "Is_NewRow";
         this.Is_NewRow.FieldName = "Is_NewRow";
         this.Is_NewRow.Name = "Is_NewRow";
         // 
         // emProdDate
         // 
         this.emProdDate.EditValue = "0000/00/00";
         this.emProdDate.Location = new System.Drawing.Point(622, 28);
         this.emProdDate.MenuManager = this.ribbonControl;
         this.emProdDate.Name = "emProdDate";
         this.emProdDate.Properties.Appearance.Options.UseTextOptions = true;
         this.emProdDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.emProdDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.emProdDate.Properties.Mask.EditMask = "\\d\\d\\d\\d/\\d\\d/\\d\\d";
         this.emProdDate.Properties.Mask.IgnoreMaskBlank = false;
         this.emProdDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
         this.emProdDate.Properties.Mask.PlaceHolder = '0';
         this.emProdDate.Properties.Mask.ShowPlaceHolders = false;
         this.emProdDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.emProdDate.Size = new System.Drawing.Size(105, 28);
         this.emProdDate.TabIndex = 23;
         // 
         // W20231
         // 
         this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.Appearance.Options.UseBackColor = true;
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
         ((System.ComponentModel.ISupportInitialize)(this.emDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         this.panelControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emProdDate.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label label2;
        private BaseGround.Widget.TextDateEdit emDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnAdd;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_YMD;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_SID;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_KIND_ID2;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_FUT;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_OPT;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_STATUS_CODE;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_PID;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_W_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn PLS4_W_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn APDK_KIND_GRP2;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
      private DevExpress.XtraEditors.TextEdit emProdDate;
   }
}