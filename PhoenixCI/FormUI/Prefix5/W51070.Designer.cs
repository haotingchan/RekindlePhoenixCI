namespace PhoenixCI.FormUI.Prefix5 {
   partial class W51070 {
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
         this.labMsg = new System.Windows.Forms.Label();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.label2 = new System.Windows.Forms.Label();
         this.labTradeDate = new System.Windows.Forms.Label();
         this.gbType = new DevExpress.XtraEditors.RadioGroup();
         this.labSystem = new System.Windows.Forms.Label();
         this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
         this.labMarket = new System.Windows.Forms.Label();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.gcMain);
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(1101, 604);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(1101, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.labMsg.ForeColor = System.Drawing.Color.Red;
         this.labMsg.Location = new System.Drawing.Point(715, 18);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(172, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "備註：(1)確認交易日期";
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.label2);
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Controls.Add(this.labTradeDate);
         this.r_frame.Controls.Add(this.gbType);
         this.r_frame.Controls.Add(this.labSystem);
         this.r_frame.Controls.Add(this.gbMarket);
         this.r_frame.Controls.Add(this.labMarket);
         this.r_frame.Dock = System.Windows.Forms.DockStyle.Top;
         this.r_frame.Location = new System.Drawing.Point(12, 12);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(1077, 70);
         this.r_frame.TabIndex = 78;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.label2.ForeColor = System.Drawing.Color.Red;
         this.label2.Location = new System.Drawing.Point(763, 38);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(268, 20);
         this.label2.TabIndex = 84;
         this.label2.Text = "(2)需於各盤別各商品收盤前修改完成";
         // 
         // labTradeDate
         // 
         this.labTradeDate.AutoSize = true;
         this.labTradeDate.Location = new System.Drawing.Point(504, 26);
         this.labTradeDate.Name = "labTradeDate";
         this.labTradeDate.Size = new System.Drawing.Size(185, 20);
         this.labTradeDate.TabIndex = 83;
         this.labTradeDate.Text = "交易日期：yyyy/mm/dd";
         // 
         // gbType
         // 
         this.gbType.EditValue = "rbTypeFut";
         this.gbType.Location = new System.Drawing.Point(318, 18);
         this.gbType.Name = "gbType";
         this.gbType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbType.Properties.Appearance.Options.UseBackColor = true;
         this.gbType.Properties.Columns = 3;
         this.gbType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbTypeFut", "期貨"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbTypeOpt", "選擇權")});
         this.gbType.Size = new System.Drawing.Size(159, 35);
         this.gbType.TabIndex = 81;
         this.gbType.EditValueChanged += new System.EventHandler(this.gbType_EditValueChanged);
         // 
         // labSystem
         // 
         this.labSystem.AutoSize = true;
         this.labSystem.Location = new System.Drawing.Point(248, 26);
         this.labSystem.Name = "labSystem";
         this.labSystem.Size = new System.Drawing.Size(73, 20);
         this.labSystem.TabIndex = 82;
         this.labSystem.Text = "系統別：";
         // 
         // gbMarket
         // 
         this.gbMarket.EditValue = "rbMarket0";
         this.gbMarket.Location = new System.Drawing.Point(72, 18);
         this.gbMarket.Name = "gbMarket";
         this.gbMarket.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbMarket.Properties.Appearance.Options.UseBackColor = true;
         this.gbMarket.Properties.Columns = 3;
         this.gbMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarket0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarket1", "夜盤")});
         this.gbMarket.Size = new System.Drawing.Size(146, 35);
         this.gbMarket.TabIndex = 79;
         this.gbMarket.EditValueChanged += new System.EventHandler(this.gbMarket_EditValueChanged);
         // 
         // labMarket
         // 
         this.labMarket.AutoSize = true;
         this.labMarket.Location = new System.Drawing.Point(18, 26);
         this.labMarket.Name = "labMarket";
         this.labMarket.Size = new System.Drawing.Size(57, 20);
         this.labMarket.TabIndex = 80;
         this.labMarket.Text = "盤別：";
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(12, 82);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(1077, 510);
         this.gcMain.TabIndex = 79;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
         this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
         this.gvMain.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvMain_CellValueChanged);
         // 
         // W51070
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1101, 634);
         this.Name = "W51070";
         this.Text = "W51070";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Label labMsg;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label labTradeDate;
      protected DevExpress.XtraEditors.RadioGroup gbType;
      private System.Windows.Forms.Label labSystem;
      protected DevExpress.XtraEditors.RadioGroup gbMarket;
      private System.Windows.Forms.Label labMarket;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
   }
}