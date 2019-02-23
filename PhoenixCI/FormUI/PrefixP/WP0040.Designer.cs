namespace PhoenixCI.FormUI.PrefixP {
   partial class WP0040 {
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
         this.label1 = new System.Windows.Forms.Label();
         this.lblDpt = new System.Windows.Forms.Label();
         this.txtFcmNo = new DevExpress.XtraEditors.TextEdit();
         this.txtAccNo = new DevExpress.XtraEditors.TextEdit();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.FCM_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.FCM_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SEQ_ACC_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SYS_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.W_STATUS = new DevExpress.XtraGrid.Columns.GridColumn();
         this.APPLY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.LOCK_CNT = new DevExpress.XtraGrid.Columns.GridColumn();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtFcmNo.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAccNo.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         this.panelControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(774, 469);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(774, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Controls.Add(this.lblDpt);
         this.panelControl1.Controls.Add(this.txtFcmNo);
         this.panelControl1.Controls.Add(this.txtAccNo);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(774, 53);
         this.panelControl1.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(260, 17);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(89, 20);
         this.label1.TabIndex = 58;
         this.label1.Text = "投資人帳號";
         // 
         // lblDpt
         // 
         this.lblDpt.AutoSize = true;
         this.lblDpt.Location = new System.Drawing.Point(26, 17);
         this.lblDpt.Name = "lblDpt";
         this.lblDpt.Size = new System.Drawing.Size(89, 20);
         this.lblDpt.TabIndex = 54;
         this.lblDpt.Text = "期貨商代號";
         // 
         // txtFcmNo
         // 
         this.txtFcmNo.Location = new System.Drawing.Point(121, 14);
         this.txtFcmNo.Name = "txtFcmNo";
         this.txtFcmNo.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtFcmNo.Size = new System.Drawing.Size(108, 26);
         this.txtFcmNo.TabIndex = 55;
         // 
         // txtAccNo
         // 
         this.txtAccNo.Location = new System.Drawing.Point(355, 14);
         this.txtAccNo.Name = "txtAccNo";
         this.txtAccNo.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.txtAccNo.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.txtAccNo.Size = new System.Drawing.Size(122, 26);
         this.txtAccNo.TabIndex = 62;
         // 
         // panelControl2
         // 
         this.panelControl2.Controls.Add(this.gcMain);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 83);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(774, 416);
         this.panelControl2.TabIndex = 1;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(2, 2);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(770, 412);
         this.gcMain.TabIndex = 49;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         this.gcMain.Visible = false;
         // 
         // gvMain
         // 
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.FCM_NAME,
            this.FCM_NO,
            this.SEQ_ACC_NO,
            this.SYS_TYPE,
            this.W_STATUS,
            this.APPLY_DATE,
            this.LOCK_CNT});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ShowGroupPanel = false;
         // 
         // FCM_NAME
         // 
         this.FCM_NAME.Caption = "期貨商";
         this.FCM_NAME.FieldName = "FCM_NAME";
         this.FCM_NAME.Name = "FCM_NAME";
         this.FCM_NAME.Visible = true;
         this.FCM_NAME.VisibleIndex = 0;
         // 
         // FCM_NO
         // 
         this.FCM_NO.Caption = "期貨商代號";
         this.FCM_NO.FieldName = "FCM_NO";
         this.FCM_NO.Name = "FCM_NO";
         this.FCM_NO.Visible = true;
         this.FCM_NO.VisibleIndex = 1;
         // 
         // SEQ_ACC_NO
         // 
         this.SEQ_ACC_NO.Caption = "流水帳號";
         this.SEQ_ACC_NO.FieldName = "SEQ_ACC_NO";
         this.SEQ_ACC_NO.Name = "SEQ_ACC_NO";
         this.SEQ_ACC_NO.Visible = true;
         this.SEQ_ACC_NO.VisibleIndex = 2;
         // 
         // SYS_TYPE
         // 
         this.SYS_TYPE.Caption = "系統別";
         this.SYS_TYPE.FieldName = "SYS_TYPE";
         this.SYS_TYPE.Name = "SYS_TYPE";
         this.SYS_TYPE.Visible = true;
         this.SYS_TYPE.VisibleIndex = 3;
         // 
         // W_STATUS
         // 
         this.W_STATUS.Caption = "狀態";
         this.W_STATUS.FieldName = "W_STATUS";
         this.W_STATUS.Name = "W_STATUS";
         this.W_STATUS.Visible = true;
         this.W_STATUS.VisibleIndex = 4;
         // 
         // APPLY_DATE
         // 
         this.APPLY_DATE.Caption = "申請日期";
         this.APPLY_DATE.FieldName = "APPLY_DATE";
         this.APPLY_DATE.Name = "APPLY_DATE";
         this.APPLY_DATE.Visible = true;
         this.APPLY_DATE.VisibleIndex = 5;
         // 
         // LOCK_CNT
         // 
         this.LOCK_CNT.Caption = "被鎖住次數";
         this.LOCK_CNT.FieldName = "LOCK_CNT";
         this.LOCK_CNT.Name = "LOCK_CNT";
         this.LOCK_CNT.Visible = true;
         this.LOCK_CNT.VisibleIndex = 6;
         // 
         // WP0040
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(774, 499);
         this.Controls.Add(this.panelControl2);
         this.Controls.Add(this.panelControl1);
         this.Name = "WP0040";
         this.Text = "WP0040";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtFcmNo.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAccNo.Properties)).EndInit();
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
      private System.Windows.Forms.Label lblDpt;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.TextEdit txtFcmNo;
      private DevExpress.XtraEditors.TextEdit txtAccNo;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private DevExpress.XtraGrid.Columns.GridColumn FCM_NAME;
      private DevExpress.XtraGrid.Columns.GridColumn FCM_NO;
      private DevExpress.XtraGrid.Columns.GridColumn SEQ_ACC_NO;
      private DevExpress.XtraGrid.Columns.GridColumn SYS_TYPE;
      private DevExpress.XtraGrid.Columns.GridColumn W_STATUS;
      private DevExpress.XtraGrid.Columns.GridColumn APPLY_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn LOCK_CNT;
   }
}