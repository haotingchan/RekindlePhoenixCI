namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30633 {
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
         this.grpxDescription = new System.Windows.Forms.GroupBox();
         this.txtPrevEndYM = new PhoenixCI.Widget.TextDateEdit();
         this.txtPrevStartYM = new PhoenixCI.Widget.TextDateEdit();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.gb_market = new DevExpress.XtraEditors.RadioGroup();
         this.dw_paramKey = new DevExpress.XtraEditors.LookUpEdit();
         this.labMarket = new System.Windows.Forms.Label();
         this.labParamKey = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.labPrevDate = new System.Windows.Forms.Label();
         this.txtAftEndYM = new PhoenixCI.Widget.TextDateEdit();
         this.txtAftStartYM = new PhoenixCI.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.labAftDate = new System.Windows.Forms.Label();
         this.lblProcessing = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEndYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevStartYM.Properties)).BeginInit();
         this.groupBox2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gb_market.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_paramKey.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEndYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftStartYM.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.lblProcessing);
         this.panParent.Controls.Add(this.grpxDescription);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.Controls.Add(this.txtPrevEndYM);
         this.grpxDescription.Controls.Add(this.txtPrevStartYM);
         this.grpxDescription.Controls.Add(this.groupBox2);
         this.grpxDescription.Controls.Add(this.dw_paramKey);
         this.grpxDescription.Controls.Add(this.labMarket);
         this.grpxDescription.Controls.Add(this.labParamKey);
         this.grpxDescription.Controls.Add(this.label2);
         this.grpxDescription.Controls.Add(this.labPrevDate);
         this.grpxDescription.Controls.Add(this.txtAftEndYM);
         this.grpxDescription.Controls.Add(this.txtAftStartYM);
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.labAftDate);
         this.grpxDescription.Location = new System.Drawing.Point(15, 15);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(427, 196);
         this.grpxDescription.TabIndex = 7;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // txtPrevEndYM
         // 
         this.txtPrevEndYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevEndYM.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtPrevEndYM.EditValue = "2018/12";
         this.txtPrevEndYM.EnterMoveNextControl = true;
         this.txtPrevEndYM.Location = new System.Drawing.Point(254, 98);
         this.txtPrevEndYM.MenuManager = this.ribbonControl;
         this.txtPrevEndYM.Name = "txtPrevEndYM";
         this.txtPrevEndYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevEndYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevEndYM.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtPrevEndYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtPrevEndYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevEndYM.Size = new System.Drawing.Size(100, 26);
         this.txtPrevEndYM.TabIndex = 12;
         this.txtPrevEndYM.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtPrevStartYM
         // 
         this.txtPrevStartYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevStartYM.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtPrevStartYM.EditValue = "2018/12";
         this.txtPrevStartYM.EnterMoveNextControl = true;
         this.txtPrevStartYM.Location = new System.Drawing.Point(117, 98);
         this.txtPrevStartYM.MenuManager = this.ribbonControl;
         this.txtPrevStartYM.Name = "txtPrevStartYM";
         this.txtPrevStartYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevStartYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevStartYM.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtPrevStartYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtPrevStartYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevStartYM.Size = new System.Drawing.Size(100, 26);
         this.txtPrevStartYM.TabIndex = 11;
         this.txtPrevStartYM.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.gb_market);
         this.groupBox2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.groupBox2.Location = new System.Drawing.Point(140, 122);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(214, 46);
         this.groupBox2.TabIndex = 62;
         this.groupBox2.TabStop = false;
         // 
         // gb_market
         // 
         this.gb_market.EditValue = "rb_market_All";
         this.gb_market.Location = new System.Drawing.Point(11, 11);
         this.gb_market.Name = "gb_market";
         this.gb_market.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gb_market.Properties.Appearance.Options.UseBackColor = true;
         this.gb_market.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.gb_market.Properties.Columns = 3;
         this.gb_market.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_All", "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_1", "盤後")});
         this.gb_market.Size = new System.Drawing.Size(205, 35);
         this.gb_market.TabIndex = 0;
         // 
         // dw_paramKey
         // 
         this.dw_paramKey.Location = new System.Drawing.Point(117, 33);
         this.dw_paramKey.Name = "dw_paramKey";
         this.dw_paramKey.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.dw_paramKey.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.dw_paramKey.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dw_paramKey.Properties.NullText = "";
         this.dw_paramKey.Properties.PopupSizeable = false;
         this.dw_paramKey.Size = new System.Drawing.Size(237, 26);
         this.dw_paramKey.TabIndex = 31;
         // 
         // labMarket
         // 
         this.labMarket.AutoSize = true;
         this.labMarket.Location = new System.Drawing.Point(54, 135);
         this.labMarket.Name = "labMarket";
         this.labMarket.Size = new System.Drawing.Size(89, 20);
         this.labMarket.TabIndex = 14;
         this.labMarket.Text = "交易時段：";
         // 
         // labParamKey
         // 
         this.labParamKey.AutoSize = true;
         this.labParamKey.Location = new System.Drawing.Point(54, 36);
         this.labParamKey.Name = "labParamKey";
         this.labParamKey.Size = new System.Drawing.Size(57, 20);
         this.labParamKey.TabIndex = 13;
         this.labParamKey.Text = "商品：";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(223, 98);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(25, 20);
         this.label2.TabIndex = 10;
         this.label2.Text = "～";
         // 
         // labPrevDate
         // 
         this.labPrevDate.AutoSize = true;
         this.labPrevDate.Location = new System.Drawing.Point(54, 101);
         this.labPrevDate.Name = "labPrevDate";
         this.labPrevDate.Size = new System.Drawing.Size(57, 20);
         this.labPrevDate.TabIndex = 9;
         this.labPrevDate.Text = "前期：";
         // 
         // txtAftEndYM
         // 
         this.txtAftEndYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftEndYM.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAftEndYM.EditValue = "2018/12";
         this.txtAftEndYM.EnterMoveNextControl = true;
         this.txtAftEndYM.Location = new System.Drawing.Point(254, 66);
         this.txtAftEndYM.MenuManager = this.ribbonControl;
         this.txtAftEndYM.Name = "txtAftEndYM";
         this.txtAftEndYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftEndYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftEndYM.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAftEndYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtAftEndYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftEndYM.Size = new System.Drawing.Size(100, 26);
         this.txtAftEndYM.TabIndex = 8;
         this.txtAftEndYM.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAftStartYM
         // 
         this.txtAftStartYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftStartYM.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAftStartYM.EditValue = "2018/12";
         this.txtAftStartYM.EnterMoveNextControl = true;
         this.txtAftStartYM.Location = new System.Drawing.Point(117, 66);
         this.txtAftStartYM.MenuManager = this.ribbonControl;
         this.txtAftStartYM.Name = "txtAftStartYM";
         this.txtAftStartYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftStartYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftStartYM.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtAftStartYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtAftStartYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftStartYM.Size = new System.Drawing.Size(100, 26);
         this.txtAftStartYM.TabIndex = 7;
         this.txtAftStartYM.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(223, 66);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(25, 20);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // labAftDate
         // 
         this.labAftDate.AutoSize = true;
         this.labAftDate.Location = new System.Drawing.Point(54, 69);
         this.labAftDate.Name = "labAftDate";
         this.labAftDate.Size = new System.Drawing.Size(57, 20);
         this.labAftDate.TabIndex = 2;
         this.labAftDate.Text = "後期：";
         // 
         // lblProcessing
         // 
         this.lblProcessing.AutoSize = true;
         this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
         this.lblProcessing.Location = new System.Drawing.Point(15, 214);
         this.lblProcessing.Name = "lblProcessing";
         this.lblProcessing.Size = new System.Drawing.Size(85, 20);
         this.lblProcessing.TabIndex = 10;
         this.lblProcessing.Text = "開始轉檔...";
         this.lblProcessing.Visible = false;
         // 
         // W30633
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Name = "W30633";
         this.Text = "W30633";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEndYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevStartYM.Properties)).EndInit();
         this.groupBox2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gb_market.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dw_paramKey.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEndYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftStartYM.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox grpxDescription;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label labAftDate;
      private Widget.TextDateEdit txtAftEndYM;
      private Widget.TextDateEdit txtAftStartYM;
      private System.Windows.Forms.Label lblProcessing;
      private System.Windows.Forms.Label labMarket;
      private System.Windows.Forms.Label labParamKey;
      private Widget.TextDateEdit txtPrevEndYM;
      private Widget.TextDateEdit txtPrevStartYM;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label labPrevDate;
      private DevExpress.XtraEditors.LookUpEdit dw_paramKey;
      protected System.Windows.Forms.GroupBox groupBox2;
      protected DevExpress.XtraEditors.RadioGroup gb_market;
   }
}