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
         this.txtPrevEndYM = new BaseGround.Widget.TextDateEdit();
         this.txtPrevStartYM = new BaseGround.Widget.TextDateEdit();
         this.txtAftEndYM = new BaseGround.Widget.TextDateEdit();
         this.txtAftStartYM = new BaseGround.Widget.TextDateEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.dwParamKey = new DevExpress.XtraEditors.LookUpEdit();
         this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
         this.label4 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEndYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevStartYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEndYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftStartYM.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwParamKey.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(450, 370);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(450, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // txtPrevEndYM
         // 
         this.txtPrevEndYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevEndYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtPrevEndYM.EditValue = "2018/12";
         this.txtPrevEndYM.EnterMoveNextControl = true;
         this.txtPrevEndYM.Location = new System.Drawing.Point(220, 132);
         this.txtPrevEndYM.MenuManager = this.ribbonControl;
         this.txtPrevEndYM.Name = "txtPrevEndYM";
         this.txtPrevEndYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevEndYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevEndYM.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtPrevEndYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtPrevEndYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtPrevEndYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtPrevEndYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevEndYM.Size = new System.Drawing.Size(100, 26);
         this.txtPrevEndYM.TabIndex = 4;
         this.txtPrevEndYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtPrevStartYM
         // 
         this.txtPrevStartYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtPrevStartYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtPrevStartYM.EditValue = "2018/12";
         this.txtPrevStartYM.EnterMoveNextControl = true;
         this.txtPrevStartYM.Location = new System.Drawing.Point(92, 132);
         this.txtPrevStartYM.MenuManager = this.ribbonControl;
         this.txtPrevStartYM.Name = "txtPrevStartYM";
         this.txtPrevStartYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtPrevStartYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtPrevStartYM.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtPrevStartYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtPrevStartYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtPrevStartYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtPrevStartYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtPrevStartYM.Size = new System.Drawing.Size(100, 26);
         this.txtPrevStartYM.TabIndex = 3;
         this.txtPrevStartYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAftEndYM
         // 
         this.txtAftEndYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftEndYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAftEndYM.EditValue = "2018/12";
         this.txtAftEndYM.EnterMoveNextControl = true;
         this.txtAftEndYM.Location = new System.Drawing.Point(220, 87);
         this.txtAftEndYM.MenuManager = this.ribbonControl;
         this.txtAftEndYM.Name = "txtAftEndYM";
         this.txtAftEndYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftEndYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftEndYM.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtAftEndYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtAftEndYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAftEndYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtAftEndYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftEndYM.Size = new System.Drawing.Size(100, 26);
         this.txtAftEndYM.TabIndex = 2;
         this.txtAftEndYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtAftStartYM
         // 
         this.txtAftStartYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtAftStartYM.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtAftStartYM.EditValue = "2018/12";
         this.txtAftStartYM.EnterMoveNextControl = true;
         this.txtAftStartYM.Location = new System.Drawing.Point(92, 87);
         this.txtAftStartYM.MenuManager = this.ribbonControl;
         this.txtAftStartYM.Name = "txtAftStartYM";
         this.txtAftStartYM.Properties.Appearance.Options.UseTextOptions = true;
         this.txtAftStartYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtAftStartYM.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtAftStartYM.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtAftStartYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtAftStartYM.Properties.Mask.ShowPlaceHolders = false;
         this.txtAftStartYM.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtAftStartYM.Size = new System.Drawing.Size(100, 26);
         this.txtAftStartYM.TabIndex = 1;
         this.txtAftStartYM.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(194, 135);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(26, 21);
         this.label2.TabIndex = 10;
         this.label2.Text = "～";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(194, 90);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(26, 21);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Location = new System.Drawing.Point(30, 30);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(400, 310);
         this.r_frame.TabIndex = 81;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 260);
         this.labMsg.MaximumSize = new System.Drawing.Size(360, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 80;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.dwParamKey);
         this.panFilter.Controls.Add(this.gbMarket);
         this.panFilter.Controls.Add(this.label4);
         this.panFilter.Controls.Add(this.txtPrevEndYM);
         this.panFilter.Controls.Add(this.txtAftEndYM);
         this.panFilter.Controls.Add(this.txtPrevStartYM);
         this.panFilter.Controls.Add(this.txtAftStartYM);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.label7);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.label6);
         this.panFilter.Controls.Add(this.label5);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(360, 235);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // dwParamKey
         // 
         this.dwParamKey.Location = new System.Drawing.Point(92, 42);
         this.dwParamKey.Name = "dwParamKey";
         this.dwParamKey.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.dwParamKey.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.dwParamKey.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dwParamKey.Properties.LookAndFeel.SkinName = "The Bezier";
         this.dwParamKey.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.dwParamKey.Properties.NullText = "";
         this.dwParamKey.Properties.PopupSizeable = false;
         this.dwParamKey.Size = new System.Drawing.Size(228, 26);
         this.dwParamKey.TabIndex = 0;
         // 
         // gbMarket
         // 
         this.gbMarket.EditValue = "rb_market_All";
         this.gbMarket.Location = new System.Drawing.Point(123, 172);
         this.gbMarket.Name = "gbMarket";
         this.gbMarket.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbMarket.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.gbMarket.Properties.Appearance.Options.UseBackColor = true;
         this.gbMarket.Properties.Appearance.Options.UseForeColor = true;
         this.gbMarket.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.gbMarket.Properties.Columns = 3;
         this.gbMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_All", "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_1", "盤後")});
         this.gbMarket.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.gbMarket.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbMarket.Size = new System.Drawing.Size(197, 35);
         this.gbMarket.TabIndex = 5;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.ForeColor = System.Drawing.Color.Black;
         this.label4.Location = new System.Drawing.Point(37, 180);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(90, 21);
         this.label4.TabIndex = 37;
         this.label4.Text = "交易時段：";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.ForeColor = System.Drawing.Color.Black;
         this.label7.Location = new System.Drawing.Point(37, 135);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(58, 21);
         this.label7.TabIndex = 10;
         this.label7.Text = "前期：";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.ForeColor = System.Drawing.Color.Black;
         this.label6.Location = new System.Drawing.Point(37, 90);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(58, 21);
         this.label6.TabIndex = 9;
         this.label6.Text = "後期：";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.ForeColor = System.Drawing.Color.Black;
         this.label5.Location = new System.Drawing.Point(37, 45);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(58, 21);
         this.label5.TabIndex = 2;
         this.label5.Text = "商品：";
         // 
         // W30630
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(450, 400);
         this.Name = "W30630";
         this.Text = "W30630";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevEndYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtPrevStartYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftEndYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAftStartYM.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dwParamKey.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private BaseGround.Widget.TextDateEdit txtPrevEndYM;
      private BaseGround.Widget.TextDateEdit txtPrevStartYM;
      private BaseGround.Widget.TextDateEdit txtAftEndYM;
      private BaseGround.Widget.TextDateEdit txtAftStartYM;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label6;
      protected DevExpress.XtraEditors.RadioGroup gbMarket;
      private DevExpress.XtraEditors.LookUpEdit dwParamKey;
   }
}