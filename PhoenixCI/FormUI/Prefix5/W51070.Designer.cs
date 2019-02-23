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
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.label3 = new System.Windows.Forms.Label();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.gb_system = new DevExpress.XtraEditors.RadioGroup();
         this.gb1 = new System.Windows.Forms.GroupBox();
         this.gb_market = new DevExpress.XtraEditors.RadioGroup();
         this.lblDate = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.OP_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SLT_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SLT_MIN = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SLT_MAX = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SLT_SPREAD = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SLT_SPREAD_LONG = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SLT_SPREAD_MULTI = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SLT_SPREAD_MAX = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SLT_VALID_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SLT_PRICE_FLUC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gb_system.Properties)).BeginInit();
         this.gb1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gb_market.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.gcMain);
         this.panParent.Location = new System.Drawing.Point(0 , 84);
         this.panParent.Size = new System.Drawing.Size(759 , 336);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(759 , 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.label3);
         this.panelControl1.Controls.Add(this.groupBox1);
         this.panelControl1.Controls.Add(this.gb1);
         this.panelControl1.Controls.Add(this.lblDate);
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Controls.Add(this.label2);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0 , 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(759 , 54);
         this.panelControl1.TabIndex = 0;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("微軟正黑體" , 12F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ((byte)(136)));
         this.label3.Location = new System.Drawing.Point(500 , 18);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(89 , 20);
         this.label3.TabIndex = 15;
         this.label3.Text = "交易日期：";
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.gb_system);
         this.groupBox1.Font = new System.Drawing.Font("微軟正黑體" , 9.75F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ((byte)(136)));
         this.groupBox1.Location = new System.Drawing.Point(318 , 3);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(165 , 40);
         this.groupBox1.TabIndex = 13;
         this.groupBox1.TabStop = false;
         // 
         // gb_system
         // 
         this.gb_system.EditValue = "rb_system_0";
         this.gb_system.Location = new System.Drawing.Point(5 , 13);
         this.gb_system.Name = "gb_system";
         this.gb_system.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gb_system.Properties.Appearance.Options.UseBackColor = true;
         this.gb_system.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.gb_system.Properties.Columns = 2;
         this.gb_system.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_system_0", "期貨"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_system_1", "選擇權")});
         this.gb_system.Properties.EditValueChanged += new System.EventHandler(this.gb_system_Properties_EditValueChanged);
         this.gb_system.Size = new System.Drawing.Size(155 , 22);
         this.gb_system.TabIndex = 0;
         // 
         // gb1
         // 
         this.gb1.Controls.Add(this.gb_market);
         this.gb1.Font = new System.Drawing.Font("微軟正黑體" , 9.75F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ((byte)(136)));
         this.gb1.Location = new System.Drawing.Point(61 , 3);
         this.gb1.Name = "gb1";
         this.gb1.Size = new System.Drawing.Size(165 , 40);
         this.gb1.TabIndex = 11;
         this.gb1.TabStop = false;
         // 
         // gb_market
         // 
         this.gb_market.EditValue = "rb_market_0";
         this.gb_market.Location = new System.Drawing.Point(6 , 10);
         this.gb_market.Name = "gb_market";
         this.gb_market.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gb_market.Properties.Appearance.Options.UseBackColor = true;
         this.gb_market.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.gb_market.Properties.Columns = 2;
         this.gb_market.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_1", "夜盤")});
         this.gb_market.Properties.EditValueChanged += new System.EventHandler(this.gb_market_Properties_EditValueChanged);
         this.gb_market.Size = new System.Drawing.Size(155 , 30);
         this.gb_market.TabIndex = 0;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Font = new System.Drawing.Font("微軟正黑體" , 12F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ((byte)(136)));
         this.lblDate.Location = new System.Drawing.Point(584 , 18);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(105 , 20);
         this.lblDate.TabIndex = 16;
         this.lblDate.Text = "yyyy/MM/dd";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("微軟正黑體" , 12F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ((byte)(136)));
         this.label1.Location = new System.Drawing.Point(5 , 18);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(57 , 20);
         this.label1.TabIndex = 12;
         this.label1.Text = "盤別：";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("微軟正黑體" , 12F , System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point , ((byte)(136)));
         this.label2.Location = new System.Drawing.Point(246 , 18);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(73 , 20);
         this.label2.TabIndex = 14;
         this.label2.Text = "系統別：";
         // 
         // panelControl2
         // 
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0 , 84);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(759 , 336);
         this.panelControl2.TabIndex = 1;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(12 , 12);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(735 , 312);
         this.gcMain.TabIndex = 12;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.OP_TYPE,
            this.SLT_KIND_ID,
            this.SLT_MIN,
            this.SLT_MAX,
            this.SLT_SPREAD,
            this.SLT_SPREAD_LONG,
            this.SLT_SPREAD_MULTI,
            this.SLT_SPREAD_MAX,
            this.SLT_VALID_QNTY,
            this.SLT_PRICE_FLUC});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ShowGroupPanel = false;
         // 
         // OP_TYPE
         // 
         this.OP_TYPE.AppearanceCell.ForeColor = System.Drawing.Color.Red;
         this.OP_TYPE.AppearanceCell.Options.UseForeColor = true;
         this.OP_TYPE.FieldName = "OP_TYPE";
         this.OP_TYPE.Name = "OP_TYPE";
         this.OP_TYPE.OptionsColumn.AllowEdit = false;
         this.OP_TYPE.OptionsColumn.ShowCaption = false;
         this.OP_TYPE.Visible = true;
         this.OP_TYPE.VisibleIndex = 0;
         this.OP_TYPE.Width = 30;
         // 
         // SLT_KIND_ID
         // 
         this.SLT_KIND_ID.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))) , ((int)(((byte)(224)))) , ((int)(((byte)(224)))));
         this.SLT_KIND_ID.AppearanceCell.Options.UseBackColor = true;
         this.SLT_KIND_ID.Caption = "商品";
         this.SLT_KIND_ID.FieldName = "SLT_KIND_ID";
         this.SLT_KIND_ID.Name = "SLT_KIND_ID";
         this.SLT_KIND_ID.OptionsColumn.AllowEdit = false;
         this.SLT_KIND_ID.Visible = true;
         this.SLT_KIND_ID.VisibleIndex = 1;
         this.SLT_KIND_ID.Width = 79;
         // 
         // SLT_MIN
         // 
         this.SLT_MIN.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))) , ((int)(((byte)(224)))) , ((int)(((byte)(224)))));
         this.SLT_MIN.AppearanceCell.Options.UseBackColor = true;
         this.SLT_MIN.Caption = "最小值";
         this.SLT_MIN.FieldName = "SLT_MIN";
         this.SLT_MIN.Name = "SLT_MIN";
         this.SLT_MIN.OptionsColumn.AllowEdit = false;
         this.SLT_MIN.Visible = true;
         this.SLT_MIN.VisibleIndex = 2;
         this.SLT_MIN.Width = 79;
         // 
         // SLT_MAX
         // 
         this.SLT_MAX.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))) , ((int)(((byte)(224)))) , ((int)(((byte)(224)))));
         this.SLT_MAX.AppearanceCell.Options.UseBackColor = true;
         this.SLT_MAX.Caption = "最大值";
         this.SLT_MAX.FieldName = "SLT_MAX";
         this.SLT_MAX.Name = "SLT_MAX";
         this.SLT_MAX.OptionsColumn.AllowEdit = false;
         this.SLT_MAX.Visible = true;
         this.SLT_MAX.VisibleIndex = 3;
         this.SLT_MAX.Width = 79;
         // 
         // SLT_SPREAD
         // 
         this.SLT_SPREAD.Caption = "價差";
         this.SLT_SPREAD.FieldName = "SLT_SPREAD";
         this.SLT_SPREAD.Name = "SLT_SPREAD";
         this.SLT_SPREAD.Visible = true;
         this.SLT_SPREAD.VisibleIndex = 4;
         this.SLT_SPREAD.Width = 79;
         // 
         // SLT_SPREAD_LONG
         // 
         this.SLT_SPREAD_LONG.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))) , ((int)(((byte)(224)))) , ((int)(((byte)(224)))));
         this.SLT_SPREAD_LONG.AppearanceCell.Options.UseBackColor = true;
         this.SLT_SPREAD_LONG.Caption = "月份順序";
         this.SLT_SPREAD_LONG.FieldName = "SLT_SPREAD_LONG";
         this.SLT_SPREAD_LONG.Name = "SLT_SPREAD_LONG";
         this.SLT_SPREAD_LONG.OptionsColumn.AllowEdit = false;
         this.SLT_SPREAD_LONG.Visible = true;
         this.SLT_SPREAD_LONG.VisibleIndex = 5;
         this.SLT_SPREAD_LONG.Width = 79;
         // 
         // SLT_SPREAD_MULTI
         // 
         this.SLT_SPREAD_MULTI.Caption = "報價價差放寬倍數";
         this.SLT_SPREAD_MULTI.FieldName = "SLT_SPREAD_MULTI";
         this.SLT_SPREAD_MULTI.Name = "SLT_SPREAD_MULTI";
         this.SLT_SPREAD_MULTI.Visible = true;
         this.SLT_SPREAD_MULTI.VisibleIndex = 6;
         this.SLT_SPREAD_MULTI.Width = 79;
         // 
         // SLT_SPREAD_MAX
         // 
         this.SLT_SPREAD_MAX.Caption = "價差Max";
         this.SLT_SPREAD_MAX.FieldName = "SLT_SPREAD_MAX";
         this.SLT_SPREAD_MAX.Name = "SLT_SPREAD_MAX";
         this.SLT_SPREAD_MAX.Visible = true;
         this.SLT_SPREAD_MAX.VisibleIndex = 7;
         this.SLT_SPREAD_MAX.Width = 79;
         // 
         // SLT_VALID_QNTY
         // 
         this.SLT_VALID_QNTY.Caption = "最低報價口數";
         this.SLT_VALID_QNTY.FieldName = "SLT_VALID_QNTY";
         this.SLT_VALID_QNTY.Name = "SLT_VALID_QNTY";
         this.SLT_VALID_QNTY.Visible = true;
         this.SLT_VALID_QNTY.VisibleIndex = 8;
         this.SLT_VALID_QNTY.Width = 79;
         // 
         // SLT_PRICE_FLUC
         // 
         this.SLT_PRICE_FLUC.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))) , ((int)(((byte)(224)))) , ((int)(((byte)(224)))));
         this.SLT_PRICE_FLUC.AppearanceCell.Options.UseBackColor = true;
         this.SLT_PRICE_FLUC.Caption = "價差數值型態";
         this.SLT_PRICE_FLUC.FieldName = "SLT_PRICE_FLUC";
         this.SLT_PRICE_FLUC.Name = "SLT_PRICE_FLUC";
         this.SLT_PRICE_FLUC.OptionsColumn.AllowEdit = false;
         this.SLT_PRICE_FLUC.Visible = true;
         this.SLT_PRICE_FLUC.VisibleIndex = 9;
         this.SLT_PRICE_FLUC.Width = 93;
         // 
         // XtraForm1
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F , 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(759 , 420);
         this.Controls.Add(this.panelControl2);
         this.Controls.Add(this.panelControl1);
         this.Name = "W51070";
         this.Text = "W51070";
         this.Controls.SetChildIndex(this.ribbonControl , 0);
         this.Controls.SetChildIndex(this.panelControl1 , 0);
         this.Controls.SetChildIndex(this.panelControl2 , 0);
         this.Controls.SetChildIndex(this.panParent , 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gb_system.Properties)).EndInit();
         this.gb1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gb_market.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private System.Windows.Forms.Label label3;
      protected System.Windows.Forms.GroupBox groupBox1;
      protected DevExpress.XtraEditors.RadioGroup gb_system;
      protected System.Windows.Forms.GroupBox gb1;
      protected DevExpress.XtraEditors.RadioGroup gb_market;
      private System.Windows.Forms.Label lblDate;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private DevExpress.XtraGrid.Columns.GridColumn OP_TYPE;
      private DevExpress.XtraGrid.Columns.GridColumn SLT_KIND_ID;
      private DevExpress.XtraGrid.Columns.GridColumn SLT_MIN;
      private DevExpress.XtraGrid.Columns.GridColumn SLT_MAX;
      private DevExpress.XtraGrid.Columns.GridColumn SLT_SPREAD;
      private DevExpress.XtraGrid.Columns.GridColumn SLT_SPREAD_LONG;
      private DevExpress.XtraGrid.Columns.GridColumn SLT_SPREAD_MULTI;
      private DevExpress.XtraGrid.Columns.GridColumn SLT_SPREAD_MAX;
      private DevExpress.XtraGrid.Columns.GridColumn SLT_VALID_QNTY;
      private DevExpress.XtraGrid.Columns.GridColumn SLT_PRICE_FLUC;
   }
}