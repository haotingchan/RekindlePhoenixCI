namespace PhoenixCI.FormUI.Prefix5 {
   partial class W50060 {
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
         this.label9 = new System.Windows.Forms.Label();
         this.txtEndTime = new DevExpress.XtraEditors.TextEdit();
         this.txtStartTime = new DevExpress.XtraEditors.TextEdit();
         this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.txtStartDate = new BaseGround.Widget.TextDateEdit();
         this.sle_1 = new DevExpress.XtraEditors.TextEdit();
         this.dw_prod_kd = new DevExpress.XtraEditors.LookUpEdit();
         this.dw_sbrkno = new DevExpress.XtraEditors.LookUpEdit();
         this.ddlb_1 = new DevExpress.XtraEditors.ComboBoxEdit();
         this.sle_2 = new DevExpress.XtraEditors.TextEdit();
         this.label8 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.lblDpt = new System.Windows.Forms.Label();
         this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.AMMD_BRK_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.BRK_ABBR_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_ACC_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_SETTLE_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_PC_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_STRIKE_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_BUY_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_SELL_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_B_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_S_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AMMD_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panFilter = new DevExpress.XtraEditors.PanelControl();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndTime.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartTime.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sle_1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_prod_kd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_sbrkno.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlb_1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sle_2.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panFilter)).BeginInit();
         this.panFilter.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.gcMain);
         this.panParent.Controls.Add(this.panFilter);
         this.panParent.Size = new System.Drawing.Size(1042, 655);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(1042, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(554, 65);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(89, 20);
         this.label9.TabIndex = 97;
         this.label9.Text = "買  賣  權：";
         // 
         // txtEndTime
         // 
         this.txtEndTime.EditValue = "08:45";
         this.txtEndTime.Location = new System.Drawing.Point(216, 62);
         this.txtEndTime.MenuManager = this.ribbonControl;
         this.txtEndTime.Name = "txtEndTime";
         this.txtEndTime.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndTime.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndTime.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndTime.Properties.Mask.EditMask = "HH:mm";
         this.txtEndTime.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndTime.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndTime.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndTime.Size = new System.Drawing.Size(68, 26);
         this.txtEndTime.TabIndex = 96;
         // 
         // txtStartTime
         // 
         this.txtStartTime.EditValue = "08:45";
         this.txtStartTime.Location = new System.Drawing.Point(216, 17);
         this.txtStartTime.MenuManager = this.ribbonControl;
         this.txtStartTime.Name = "txtStartTime";
         this.txtStartTime.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartTime.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartTime.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtStartTime.Properties.Mask.EditMask = "HH:mm";
         this.txtStartTime.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartTime.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartTime.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartTime.Size = new System.Drawing.Size(68, 26);
         this.txtStartTime.TabIndex = 86;
         // 
         // gbMarket
         // 
         this.gbMarket.EditValue = "rbMarket0";
         this.gbMarket.Location = new System.Drawing.Point(110, 102);
         this.gbMarket.Name = "gbMarket";
         this.gbMarket.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbMarket.Properties.Appearance.Options.UseBackColor = true;
         this.gbMarket.Properties.Columns = 2;
         this.gbMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarket0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarket1", "盤後")});
         this.gbMarket.Properties.LookAndFeel.SkinName = "Office 2013";
         this.gbMarket.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbMarket.Size = new System.Drawing.Size(174, 35);
         this.gbMarket.TabIndex = 0;
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate.EditValue = "2018/12";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(110, 62);
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
         this.txtEndDate.TabIndex = 95;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartDate.EditValue = "2018/12";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(110, 17);
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
         this.txtStartDate.TabIndex = 94;
         this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // sle_1
         // 
         this.sle_1.Location = new System.Drawing.Point(649, 107);
         this.sle_1.MenuManager = this.ribbonControl;
         this.sle_1.Name = "sle_1";
         this.sle_1.Properties.Mask.EditMask = "n";
         this.sle_1.Size = new System.Drawing.Size(117, 26);
         this.sle_1.TabIndex = 81;
         // 
         // dw_prod_kd
         // 
         this.dw_prod_kd.Location = new System.Drawing.Point(401, 62);
         this.dw_prod_kd.MenuManager = this.ribbonControl;
         this.dw_prod_kd.Name = "dw_prod_kd";
         this.dw_prod_kd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dw_prod_kd.Properties.NullText = "";
         this.dw_prod_kd.Properties.PopupSizeable = false;
         this.dw_prod_kd.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
         this.dw_prod_kd.Size = new System.Drawing.Size(117, 26);
         this.dw_prod_kd.TabIndex = 78;
         // 
         // dw_sbrkno
         // 
         this.dw_sbrkno.Location = new System.Drawing.Point(401, 17);
         this.dw_sbrkno.MenuManager = this.ribbonControl;
         this.dw_sbrkno.Name = "dw_sbrkno";
         this.dw_sbrkno.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.dw_sbrkno.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.dw_sbrkno.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dw_sbrkno.Properties.NullText = "";
         this.dw_sbrkno.Properties.PopupSizeable = false;
         this.dw_sbrkno.Size = new System.Drawing.Size(365, 26);
         this.dw_sbrkno.TabIndex = 79;
         // 
         // ddlb_1
         // 
         this.ddlb_1.Location = new System.Drawing.Point(649, 62);
         this.ddlb_1.MenuManager = this.ribbonControl;
         this.ddlb_1.Name = "ddlb_1";
         this.ddlb_1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlb_1.Properties.Items.AddRange(new object[] {
            " ",
            "買權",
            "賣權"});
         this.ddlb_1.Size = new System.Drawing.Size(117, 26);
         this.ddlb_1.TabIndex = 77;
         // 
         // sle_2
         // 
         this.sle_2.Location = new System.Drawing.Point(897, 62);
         this.sle_2.MenuManager = this.ribbonControl;
         this.sle_2.Name = "sle_2";
         this.sle_2.Properties.Mask.EditMask = "n";
         this.sle_2.Size = new System.Drawing.Size(100, 26);
         this.sle_2.TabIndex = 76;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(772, 110);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(81, 20);
         this.label8.TabIndex = 75;
         this.label8.Text = "(yyyymm)";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(322, 65);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(57, 20);
         this.label7.TabIndex = 74;
         this.label7.Text = "商品：";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(322, 20);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(73, 20);
         this.label6.TabIndex = 73;
         this.label6.Text = "造市者：";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(802, 65);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(89, 20);
         this.label5.TabIndex = 72;
         this.label5.Text = "履約價格：";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(554, 110);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(89, 20);
         this.label3.TabIndex = 70;
         this.label3.Text = "契約月份：";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(15, 110);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(89, 20);
         this.label2.TabIndex = 69;
         this.label2.Text = "交易時段：";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(15, 65);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(41, 20);
         this.label1.TabIndex = 64;
         this.label1.Text = "至：";
         // 
         // lblDpt
         // 
         this.lblDpt.AutoSize = true;
         this.lblDpt.Location = new System.Drawing.Point(15, 20);
         this.lblDpt.Name = "lblDpt";
         this.lblDpt.Size = new System.Drawing.Size(89, 20);
         this.lblDpt.TabIndex = 63;
         this.lblDpt.Text = "交易時間：";
         // 
         // panelControl3
         // 
         this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl3.Location = new System.Drawing.Point(0, 30);
         this.panelControl3.Name = "panelControl3";
         this.panelControl3.Size = new System.Drawing.Size(1042, 655);
         this.panelControl3.TabIndex = 2;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(12, 162);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(1018, 481);
         this.gcMain.TabIndex = 1;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         this.gcMain.Visible = false;
         // 
         // gvMain
         // 
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.AMMD_BRK_NO,
            this.BRK_ABBR_NAME,
            this.AMMD_ACC_NO,
            this.AMMD_KIND_ID,
            this.AMMD_SETTLE_DATE,
            this.AMMD_PC_CODE,
            this.AMMD_STRIKE_PRICE,
            this.AMMD_BUY_PRICE,
            this.AMMD_SELL_PRICE,
            this.AMMD_B_QNTY,
            this.AMMD_S_QNTY,
            this.AMMD_W_TIME,
            this.AMMD_DATE});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
         this.gvMain.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvMain_CustomColumnDisplayText);
         // 
         // AMMD_BRK_NO
         // 
         this.AMMD_BRK_NO.AppearanceCell.Options.UseTextOptions = true;
         this.AMMD_BRK_NO.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_BRK_NO.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_BRK_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_BRK_NO.Caption = "造市者代號";
         this.AMMD_BRK_NO.FieldName = "AMMD_BRK_NO";
         this.AMMD_BRK_NO.Name = "AMMD_BRK_NO";
         this.AMMD_BRK_NO.Visible = true;
         this.AMMD_BRK_NO.VisibleIndex = 0;
         // 
         // BRK_ABBR_NAME
         // 
         this.BRK_ABBR_NAME.AppearanceHeader.Options.UseTextOptions = true;
         this.BRK_ABBR_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.BRK_ABBR_NAME.Caption = "造市者名稱";
         this.BRK_ABBR_NAME.FieldName = "BRK_ABBR_NAME";
         this.BRK_ABBR_NAME.Name = "BRK_ABBR_NAME";
         this.BRK_ABBR_NAME.Visible = true;
         this.BRK_ABBR_NAME.VisibleIndex = 1;
         // 
         // AMMD_ACC_NO
         // 
         this.AMMD_ACC_NO.AppearanceCell.Options.UseTextOptions = true;
         this.AMMD_ACC_NO.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_ACC_NO.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_ACC_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_ACC_NO.Caption = "帳號";
         this.AMMD_ACC_NO.FieldName = "AMMD_ACC_NO";
         this.AMMD_ACC_NO.Name = "AMMD_ACC_NO";
         this.AMMD_ACC_NO.Visible = true;
         this.AMMD_ACC_NO.VisibleIndex = 2;
         // 
         // AMMD_KIND_ID
         // 
         this.AMMD_KIND_ID.AppearanceCell.Options.UseTextOptions = true;
         this.AMMD_KIND_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_KIND_ID.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_KIND_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_KIND_ID.Caption = "商品";
         this.AMMD_KIND_ID.FieldName = "AMMD_KIND_ID";
         this.AMMD_KIND_ID.Name = "AMMD_KIND_ID";
         this.AMMD_KIND_ID.Visible = true;
         this.AMMD_KIND_ID.VisibleIndex = 3;
         // 
         // AMMD_SETTLE_DATE
         // 
         this.AMMD_SETTLE_DATE.AppearanceCell.Options.UseTextOptions = true;
         this.AMMD_SETTLE_DATE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_SETTLE_DATE.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_SETTLE_DATE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_SETTLE_DATE.Caption = "契約月份";
         this.AMMD_SETTLE_DATE.FieldName = "AMMD_SETTLE_DATE";
         this.AMMD_SETTLE_DATE.Name = "AMMD_SETTLE_DATE";
         this.AMMD_SETTLE_DATE.Visible = true;
         this.AMMD_SETTLE_DATE.VisibleIndex = 4;
         // 
         // AMMD_PC_CODE
         // 
         this.AMMD_PC_CODE.AppearanceCell.Options.UseTextOptions = true;
         this.AMMD_PC_CODE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_PC_CODE.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_PC_CODE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_PC_CODE.Caption = "買賣權";
         this.AMMD_PC_CODE.FieldName = "AMMD_PC_CODE";
         this.AMMD_PC_CODE.Name = "AMMD_PC_CODE";
         this.AMMD_PC_CODE.Visible = true;
         this.AMMD_PC_CODE.VisibleIndex = 5;
         // 
         // AMMD_STRIKE_PRICE
         // 
         this.AMMD_STRIKE_PRICE.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_STRIKE_PRICE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_STRIKE_PRICE.Caption = "履約價格";
         this.AMMD_STRIKE_PRICE.FieldName = "AMMD_STRIKE_PRICE";
         this.AMMD_STRIKE_PRICE.Name = "AMMD_STRIKE_PRICE";
         this.AMMD_STRIKE_PRICE.Visible = true;
         this.AMMD_STRIKE_PRICE.VisibleIndex = 6;
         // 
         // AMMD_BUY_PRICE
         // 
         this.AMMD_BUY_PRICE.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_BUY_PRICE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_BUY_PRICE.Caption = "買價價格";
         this.AMMD_BUY_PRICE.FieldName = "AMMD_BUY_PRICE";
         this.AMMD_BUY_PRICE.Name = "AMMD_BUY_PRICE";
         this.AMMD_BUY_PRICE.Visible = true;
         this.AMMD_BUY_PRICE.VisibleIndex = 7;
         // 
         // AMMD_SELL_PRICE
         // 
         this.AMMD_SELL_PRICE.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_SELL_PRICE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_SELL_PRICE.Caption = "賣價價格";
         this.AMMD_SELL_PRICE.FieldName = "AMMD_SELL_PRICE";
         this.AMMD_SELL_PRICE.Name = "AMMD_SELL_PRICE";
         this.AMMD_SELL_PRICE.Visible = true;
         this.AMMD_SELL_PRICE.VisibleIndex = 8;
         // 
         // AMMD_B_QNTY
         // 
         this.AMMD_B_QNTY.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_B_QNTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_B_QNTY.Caption = "委買數量";
         this.AMMD_B_QNTY.FieldName = "AMMD_B_QNTY";
         this.AMMD_B_QNTY.Name = "AMMD_B_QNTY";
         this.AMMD_B_QNTY.Visible = true;
         this.AMMD_B_QNTY.VisibleIndex = 9;
         // 
         // AMMD_S_QNTY
         // 
         this.AMMD_S_QNTY.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_S_QNTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_S_QNTY.Caption = "委賣數量";
         this.AMMD_S_QNTY.FieldName = "AMMD_S_QNTY";
         this.AMMD_S_QNTY.Name = "AMMD_S_QNTY";
         this.AMMD_S_QNTY.Visible = true;
         this.AMMD_S_QNTY.VisibleIndex = 10;
         // 
         // AMMD_W_TIME
         // 
         this.AMMD_W_TIME.AppearanceCell.Options.UseTextOptions = true;
         this.AMMD_W_TIME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_W_TIME.AppearanceHeader.Options.UseTextOptions = true;
         this.AMMD_W_TIME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.AMMD_W_TIME.Caption = "委託時間";
         this.AMMD_W_TIME.FieldName = "AMMD_W_TIME";
         this.AMMD_W_TIME.Name = "AMMD_W_TIME";
         this.AMMD_W_TIME.Visible = true;
         this.AMMD_W_TIME.VisibleIndex = 11;
         // 
         // AMMD_DATE
         // 
         this.AMMD_DATE.Caption = "日期";
         this.AMMD_DATE.FieldName = "AMMD_DATE";
         this.AMMD_DATE.Name = "AMMD_DATE";
         // 
         // panFilter
         // 
         this.panFilter.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Appearance.Options.UseBackColor = true;
         this.panFilter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Controls.Add(this.lblDpt);
         this.panFilter.Controls.Add(this.txtEndTime);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.txtStartTime);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.gbMarket);
         this.panFilter.Controls.Add(this.label3);
         this.panFilter.Controls.Add(this.txtEndDate);
         this.panFilter.Controls.Add(this.label5);
         this.panFilter.Controls.Add(this.txtStartDate);
         this.panFilter.Controls.Add(this.label6);
         this.panFilter.Controls.Add(this.sle_1);
         this.panFilter.Controls.Add(this.label7);
         this.panFilter.Controls.Add(this.dw_prod_kd);
         this.panFilter.Controls.Add(this.label8);
         this.panFilter.Controls.Add(this.dw_sbrkno);
         this.panFilter.Controls.Add(this.sle_2);
         this.panFilter.Controls.Add(this.ddlb_1);
         this.panFilter.Dock = System.Windows.Forms.DockStyle.Top;
         this.panFilter.Location = new System.Drawing.Point(12, 12);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(1018, 150);
         this.panFilter.TabIndex = 79;
         // 
         // W50060
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1042, 685);
         this.Controls.Add(this.panelControl3);
         this.Name = "W50060";
         this.Text = "XtraForm1";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl3, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndTime.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartTime.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sle_1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_prod_kd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_sbrkno.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlb_1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sle_2.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panFilter)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.PanelControl panelControl3;
      private DevExpress.XtraEditors.TextEdit sle_1;
      private DevExpress.XtraEditors.LookUpEdit dw_prod_kd;
      private DevExpress.XtraEditors.LookUpEdit dw_sbrkno;
      protected DevExpress.XtraEditors.RadioGroup gbMarket;
      private DevExpress.XtraEditors.ComboBoxEdit ddlb_1;
      private DevExpress.XtraEditors.TextEdit sle_2;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label lblDpt;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_BRK_NO;
      private DevExpress.XtraGrid.Columns.GridColumn BRK_ABBR_NAME;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_ACC_NO;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_KIND_ID;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_SETTLE_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_PC_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_STRIKE_PRICE;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_BUY_PRICE;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_SELL_PRICE;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_B_QNTY;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_S_QNTY;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_W_TIME;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
      private DevExpress.XtraEditors.TextEdit txtEndTime;
      private DevExpress.XtraEditors.TextEdit txtStartTime;
      private System.Windows.Forms.Label label9;
      private DevExpress.XtraGrid.Columns.GridColumn AMMD_DATE;
      private DevExpress.XtraEditors.PanelControl panFilter;
   }
}