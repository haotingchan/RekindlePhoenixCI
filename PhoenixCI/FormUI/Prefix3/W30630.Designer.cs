namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30630 {
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
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtPrevEndYM = new BaseGround.Widget.TextDateEdit();
         this.txtPrevStartYM = new BaseGround.Widget.TextDateEdit();
         this.txtAftEndYM = new BaseGround.Widget.TextDateEdit();
         this.txtAftStartYM = new BaseGround.Widget.TextDateEdit();
         this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
         this.dwParamKey = new DevExpress.XtraEditors.LookUpEdit();
         this.labMarket = new System.Windows.Forms.Label();
         this.labParamKey = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.labPrevDate = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.labAftDate = new System.Windows.Forms.Label();
         this.labMsg = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEndYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevStartYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEndYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftStartYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dwParamKey.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.labMsg);
         this.panParent.Controls.Add(this.panFilter);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.txtPrevEndYM);
         this.panFilter.Controls.Add(this.txtPrevStartYM);
         this.panFilter.Controls.Add(this.txtAftEndYM);
         this.panFilter.Controls.Add(this.txtAftStartYM);
         this.panFilter.Controls.Add(this.gbMarket);
         this.panFilter.Controls.Add(this.dwParamKey);
         this.panFilter.Controls.Add(this.labMarket);
         this.panFilter.Controls.Add(this.labParamKey);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.labPrevDate);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.labAftDate);
         this.panFilter.Location = new System.Drawing.Point(15, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(427, 196);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtPrevEndYM
         // 
         this.txtPrevEndYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevEndYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtPrevEndYM.EditValue = "2018/12";
         this.txtPrevEndYM.EnterMoveNextControl = true;
         this.txtPrevEndYM.Location = new System.Drawing.Point(254, 98);
         this.txtPrevEndYM.MenuManager = this.ribbonControl;
         this.txtPrevEndYM.Name = "txtPrevEndYM";
         this.txtPrevEndYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevEndYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevEndYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtPrevEndYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtPrevEndYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtPrevEndYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevEndYM.Size = new System.Drawing.Size(100, 26);
         this.txtPrevEndYM.TabIndex = 36;
         this.txtPrevEndYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtPrevStartYM
         // 
         this.txtPrevStartYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevStartYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtPrevStartYM.EditValue = "2018/12";
         this.txtPrevStartYM.EnterMoveNextControl = true;
         this.txtPrevStartYM.Location = new System.Drawing.Point(117, 98);
         this.txtPrevStartYM.MenuManager = this.ribbonControl;
         this.txtPrevStartYM.Name = "txtPrevStartYM";
         this.txtPrevStartYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevStartYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevStartYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtPrevStartYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtPrevStartYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtPrevStartYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevStartYM.Size = new System.Drawing.Size(100, 26);
         this.txtPrevStartYM.TabIndex = 35;
         this.txtPrevStartYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAftEndYM
         // 
         this.txtAftEndYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftEndYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAftEndYM.EditValue = "2018/12";
         this.txtAftEndYM.EnterMoveNextControl = true;
         this.txtAftEndYM.Location = new System.Drawing.Point(254, 66);
         this.txtAftEndYM.MenuManager = this.ribbonControl;
         this.txtAftEndYM.Name = "txtAftEndYM";
         this.txtAftEndYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftEndYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftEndYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtAftEndYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAftEndYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtAftEndYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftEndYM.Size = new System.Drawing.Size(100, 26);
         this.txtAftEndYM.TabIndex = 34;
         this.txtAftEndYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAftStartYM
         // 
         this.txtAftStartYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftStartYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAftStartYM.EditValue = "2018/12";
         this.txtAftStartYM.EnterMoveNextControl = true;
         this.txtAftStartYM.Location = new System.Drawing.Point(117, 66);
         this.txtAftStartYM.MenuManager = this.ribbonControl;
         this.txtAftStartYM.Name = "txtAftStartYM";
         this.txtAftStartYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftStartYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftStartYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtAftStartYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAftStartYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtAftStartYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftStartYM.Size = new System.Drawing.Size(100, 26);
         this.txtAftStartYM.TabIndex = 33;
         this.txtAftStartYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // gbMarket
         // 
         this.gbMarket.EditValue = "rb_market_All";
         this.gbMarket.Location = new System.Drawing.Point(141, 130);
         this.gbMarket.Name = "gbMarket";
         this.gbMarket.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbMarket.Properties.Appearance.Options.UseBackColor = true;
         this.gbMarket.Properties.Columns = 3;
         this.gbMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_All", "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_1", "盤後")});
         this.gbMarket.Size = new System.Drawing.Size(213, 35);
         this.gbMarket.TabIndex = 32;
         // 
         // dwParamKey
         // 
         this.dwParamKey.Location = new System.Drawing.Point(117, 33);
         this.dwParamKey.Name = "dwParamKey";
         this.dwParamKey.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.dwParamKey.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.dwParamKey.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dwParamKey.Properties.NullText = "";
         this.dwParamKey.Properties.PopupSizeable = false;
         this.dwParamKey.Size = new System.Drawing.Size(237, 26);
         this.dwParamKey.TabIndex = 31;
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
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 214);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // W30630
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Name = "W30630";
         this.Text = "W30630";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEndYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevStartYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEndYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftStartYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dwParamKey.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label labAftDate;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.Label labMarket;
      private System.Windows.Forms.Label labParamKey;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label labPrevDate;
      private DevExpress.XtraEditors.LookUpEdit dwParamKey;
      protected DevExpress.XtraEditors.RadioGroup gbMarket;
      private BaseGround.Widget.TextDateEdit txtPrevEndYM;
      private BaseGround.Widget.TextDateEdit txtPrevStartYM;
      private BaseGround.Widget.TextDateEdit txtAftEndYM;
      private BaseGround.Widget.TextDateEdit txtAftStartYM;
   }
}