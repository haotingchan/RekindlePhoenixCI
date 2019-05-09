namespace PhoenixCI.FormUI.Prefix5 {
   partial class W50040 {
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
         this.panFilter = new DevExpress.XtraEditors.PanelControl();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.gbPrice = new DevExpress.XtraEditors.RadioGroup();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.gbItem = new DevExpress.XtraEditors.RadioGroup();
         this.dw_sbrkno = new DevExpress.XtraEditors.LookUpEdit();
         this.dw_prod_kd = new DevExpress.XtraEditors.LookUpEdit();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.txtStartDate = new BaseGround.Widget.TextDateEdit();
         this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.panelGrid = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panFilter)).BeginInit();
         this.panFilter.SuspendLayout();
         this.groupBox2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbPrice.Properties)).BeginInit();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbItem.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_sbrkno.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_prod_kd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelGrid)).BeginInit();
         this.panelGrid.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.panelGrid);
         this.panParent.Controls.Add(this.panFilter);
         this.panParent.Size = new System.Drawing.Size(996, 674);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(996, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Appearance.Options.UseBackColor = true;
         this.panFilter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panFilter.Controls.Add(this.groupBox2);
         this.panFilter.Controls.Add(this.groupBox1);
         this.panFilter.Controls.Add(this.txtEndDate);
         this.panFilter.Controls.Add(this.txtStartDate);
         this.panFilter.Controls.Add(this.gbMarket);
         this.panFilter.Controls.Add(this.label3);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Dock = System.Windows.Forms.DockStyle.Top;
         this.panFilter.Location = new System.Drawing.Point(12, 12);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(972, 155);
         this.panFilter.TabIndex = 78;
         // 
         // groupBox2
         // 
         this.groupBox2.AutoSize = true;
         this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.groupBox2.Controls.Add(this.gbPrice);
         this.groupBox2.Location = new System.Drawing.Point(693, 3);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(161, 135);
         this.groupBox2.TabIndex = 93;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "輸出選擇";
         // 
         // gbPrice
         // 
         this.gbPrice.EditValue = "rb_price_2";
         this.gbPrice.Location = new System.Drawing.Point(19, 34);
         this.gbPrice.Name = "gbPrice";
         this.gbPrice.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbPrice.Properties.Appearance.Options.UseBackColor = true;
         this.gbPrice.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.gbPrice.Properties.Columns = 1;
         this.gbPrice.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_price_0", "最大價差"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_price_1", "最小價差"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_price_2", "平均價差")});
         this.gbPrice.Size = new System.Drawing.Size(115, 73);
         this.gbPrice.TabIndex = 94;
         // 
         // groupBox1
         // 
         this.groupBox1.AutoSize = true;
         this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.groupBox1.Controls.Add(this.gbItem);
         this.groupBox1.Controls.Add(this.dw_sbrkno);
         this.groupBox1.Controls.Add(this.dw_prod_kd);
         this.groupBox1.Location = new System.Drawing.Point(223, 3);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(464, 135);
         this.groupBox1.TabIndex = 8;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "輸入選擇";
         // 
         // gbItem
         // 
         this.gbItem.EditValue = "rb_item_0";
         this.gbItem.Location = new System.Drawing.Point(27, 28);
         this.gbItem.Name = "gbItem";
         this.gbItem.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbItem.Properties.Appearance.Options.UseBackColor = true;
         this.gbItem.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.gbItem.Properties.Columns = 1;
         this.gbItem.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_item_0", "造市者"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_item_1", "商品")});
         this.gbItem.Size = new System.Drawing.Size(78, 79);
         this.gbItem.TabIndex = 9;
         this.gbItem.SelectedIndexChanged += new System.EventHandler(this.gbItem_SelectedIndexChanged);
         // 
         // dw_sbrkno
         // 
         this.dw_sbrkno.Location = new System.Drawing.Point(111, 37);
         this.dw_sbrkno.MenuManager = this.ribbonControl;
         this.dw_sbrkno.Name = "dw_sbrkno";
         this.dw_sbrkno.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dw_sbrkno.Size = new System.Drawing.Size(335, 26);
         this.dw_sbrkno.TabIndex = 79;
         // 
         // dw_prod_kd
         // 
         this.dw_prod_kd.Location = new System.Drawing.Point(111, 72);
         this.dw_prod_kd.MenuManager = this.ribbonControl;
         this.dw_prod_kd.Name = "dw_prod_kd";
         this.dw_prod_kd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dw_prod_kd.Size = new System.Drawing.Size(161, 26);
         this.dw_prod_kd.TabIndex = 92;
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate.EditValue = "2018/12";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(108, 61);
         this.txtEndDate.MenuManager = this.ribbonControl;
         this.txtEndDate.Name = "txtEndDate";
         this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate.TabIndex = 94;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartDate.EditValue = "2018/12";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(108, 26);
         this.txtStartDate.MenuManager = this.ribbonControl;
         this.txtStartDate.Name = "txtStartDate";
         this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartDate.Size = new System.Drawing.Size(100, 26);
         this.txtStartDate.TabIndex = 93;
         this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // gbMarket
         // 
         this.gbMarket.EditValue = "rb_market_0";
         this.gbMarket.Location = new System.Drawing.Point(53, 96);
         this.gbMarket.Name = "gbMarket";
         this.gbMarket.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbMarket.Properties.Appearance.Options.UseBackColor = true;
         this.gbMarket.Properties.Columns = 3;
         this.gbMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_1", "盤後")});
         this.gbMarket.Size = new System.Drawing.Size(155, 35);
         this.gbMarket.TabIndex = 79;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(67, 64);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(41, 20);
         this.label3.TabIndex = 8;
         this.label3.Text = "至：";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(19, 29);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(89, 20);
         this.label2.TabIndex = 7;
         this.label2.Text = "交易時間：";
         // 
         // panelGrid
         // 
         this.panelGrid.Appearance.BackColor = System.Drawing.Color.White;
         this.panelGrid.Appearance.Options.UseBackColor = true;
         this.panelGrid.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelGrid.Controls.Add(this.gcMain);
         this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelGrid.Location = new System.Drawing.Point(12, 167);
         this.panelGrid.Name = "panelGrid";
         this.panelGrid.Size = new System.Drawing.Size(972, 495);
         this.panelGrid.TabIndex = 79;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(0, 0);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.Name = "gcMain";
         this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2});
         this.gcMain.Size = new System.Drawing.Size(972, 495);
         this.gcMain.TabIndex = 2;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         this.gcMain.Visible = false;
         // 
         // gvMain
         // 
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
         // 
         // repositoryItemTextEdit1
         // 
         this.repositoryItemTextEdit1.AutoHeight = false;
         this.repositoryItemTextEdit1.EditFormat.FormatString = "d";
         this.repositoryItemTextEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.repositoryItemTextEdit1.Mask.EditMask = "yyyy/MM/dd HH:mm:ss.fff";
         this.repositoryItemTextEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
         // 
         // repositoryItemTextEdit2
         // 
         this.repositoryItemTextEdit2.AutoHeight = false;
         this.repositoryItemTextEdit2.EditFormat.FormatString = "d";
         this.repositoryItemTextEdit2.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.repositoryItemTextEdit2.Mask.EditMask = "yyyy/MM/dd  交易時間：txtStartTime.Text ~ txtEndTime.Text";
         this.repositoryItemTextEdit2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
         // 
         // W50040
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(996, 704);
         this.Name = "W50040";
         this.Text = "W500400";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panFilter)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         this.groupBox2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gbPrice.Properties)).EndInit();
         this.groupBox1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gbItem.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_sbrkno.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_prod_kd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelGrid)).EndInit();
         this.panelGrid.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.PanelControl panFilter;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      protected DevExpress.XtraEditors.RadioGroup gbMarket;
      private DevExpress.XtraEditors.LookUpEdit dw_sbrkno;
      private DevExpress.XtraEditors.LookUpEdit dw_prod_kd;
      private DevExpress.XtraEditors.PanelControl panelGrid;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
      private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
      private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
      private System.Windows.Forms.GroupBox groupBox1;
      protected DevExpress.XtraEditors.RadioGroup gbItem;
      private System.Windows.Forms.GroupBox groupBox2;
      protected DevExpress.XtraEditors.RadioGroup gbPrice;
   }
}