namespace PhoenixCI.FormUI.Prefix3
{
   partial class W30720
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
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
      private void InitializeComponent()
      {
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.rFrame = new DevExpress.XtraEditors.PanelControl();
         this.stMsgTxt = new System.Windows.Forms.Label();
         this.gb1Label = new System.Windows.Forms.Label();
         this.gb1 = new DevExpress.XtraEditors.PanelControl();
         this.rgDate = new DevExpress.XtraEditors.RadioGroup();
         this.rgTime = new DevExpress.XtraEditors.RadioGroup();
         this.sleYear = new DevExpress.XtraEditors.TextEdit();
         this.emMonth = new DevExpress.XtraEditors.TextEdit();
         this.st5 = new System.Windows.Forms.Label();
         this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         this.panelControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rFrame)).BeginInit();
         this.rFrame.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gb1)).BeginInit();
         this.gb1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rgDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rgTime.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sleYear.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(605, 327);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(605, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl2
         // 
         this.panelControl2.Controls.Add(this.rFrame);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 30);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(605, 327);
         this.panelControl2.TabIndex = 1;
         // 
         // rFrame
         // 
         this.rFrame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.rFrame.Appearance.Options.UseBackColor = true;
         this.rFrame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.rFrame.Controls.Add(this.stMsgTxt);
         this.rFrame.Controls.Add(this.gb1Label);
         this.rFrame.Controls.Add(this.gb1);
         this.rFrame.Location = new System.Drawing.Point(30, 27);
         this.rFrame.Name = "rFrame";
         this.rFrame.Size = new System.Drawing.Size(461, 270);
         this.rFrame.TabIndex = 9999;
         // 
         // stMsgTxt
         // 
         this.stMsgTxt.AutoSize = true;
         this.stMsgTxt.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.stMsgTxt.ForeColor = System.Drawing.Color.Blue;
         this.stMsgTxt.Location = new System.Drawing.Point(34, 231);
         this.stMsgTxt.Name = "stMsgTxt";
         this.stMsgTxt.Size = new System.Drawing.Size(153, 19);
         this.stMsgTxt.TabIndex = 9999;
         this.stMsgTxt.Text = "訊息：資料轉出中........";
         this.stMsgTxt.Visible = false;
         // 
         // gb1Label
         // 
         this.gb1Label.AutoSize = true;
         this.gb1Label.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.gb1Label.ForeColor = System.Drawing.Color.Navy;
         this.gb1Label.Location = new System.Drawing.Point(34, 14);
         this.gb1Label.Name = "gb1Label";
         this.gb1Label.Size = new System.Drawing.Size(122, 21);
         this.gb1Label.TabIndex = 9999;
         this.gb1Label.Text = "請輸入交易日期";
         // 
         // gb1
         // 
         this.gb1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.gb1.Appearance.Options.UseBackColor = true;
         this.gb1.Controls.Add(this.rgDate);
         this.gb1.Controls.Add(this.rgTime);
         this.gb1.Controls.Add(this.sleYear);
         this.gb1.Controls.Add(this.emMonth);
         this.gb1.Controls.Add(this.st5);
         this.gb1.Location = new System.Drawing.Point(28, 25);
         this.gb1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.gb1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gb1.Name = "gb1";
         this.gb1.Size = new System.Drawing.Size(406, 193);
         this.gb1.TabIndex = 9999;
         // 
         // rgDate
         // 
         this.rgDate.EditValue = "rb_month";
         this.rgDate.Location = new System.Drawing.Point(31, 29);
         this.rgDate.Margin = new System.Windows.Forms.Padding(0);
         this.rgDate.MenuManager = this.ribbonControl;
         this.rgDate.Name = "rgDate";
         this.rgDate.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.rgDate.Properties.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.rgDate.Properties.Appearance.Options.UseBackColor = true;
         this.rgDate.Properties.Appearance.Options.UseFont = true;
         this.rgDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.rgDate.Properties.Columns = 1;
         this.rgDate.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_month", "月報："),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_year", "年報：")});
         this.rgDate.Size = new System.Drawing.Size(70, 92);
         this.rgDate.TabIndex = 1;
         // 
         // rgTime
         // 
         this.rgTime.EditValue = "rb_marketall";
         this.rgTime.Location = new System.Drawing.Point(123, 139);
         this.rgTime.MenuManager = this.ribbonControl;
         this.rgTime.Name = "rgTime";
         this.rgTime.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.rgTime.Properties.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.rgTime.Properties.Appearance.Options.UseBackColor = true;
         this.rgTime.Properties.Appearance.Options.UseFont = true;
         this.rgTime.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.rgTime.Properties.Columns = 3;
         this.rgTime.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_marketall", "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market1", "盤後")});
         this.rgTime.Size = new System.Drawing.Size(203, 33);
         this.rgTime.TabIndex = 3;
         // 
         // sleYear
         // 
         this.sleYear.CausesValidation = false;
         this.sleYear.EditValue = "2019";
         this.sleYear.Enabled = false;
         this.sleYear.Location = new System.Drawing.Point(104, 84);
         this.sleYear.Name = "sleYear";
         this.sleYear.Properties.DisplayFormat.FormatString = "yyyy";
         this.sleYear.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.sleYear.Properties.Mask.EditMask = "\\d\\d\\d\\d";
         this.sleYear.Properties.Mask.IgnoreMaskBlank = false;
         this.sleYear.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
         this.sleYear.Properties.Mask.PlaceHolder = '0';
         this.sleYear.Properties.MaxLength = 4;
         this.sleYear.Size = new System.Drawing.Size(71, 28);
         this.sleYear.TabIndex = 2;
         // 
         // emMonth
         // 
         this.emMonth.CausesValidation = false;
         this.emMonth.EditValue = "2019/01";
         this.emMonth.Location = new System.Drawing.Point(104, 40);
         this.emMonth.Name = "emMonth";
         this.emMonth.Properties.DisplayFormat.FormatString = "yyyy/MM";
         this.emMonth.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.emMonth.Properties.Mask.EditMask = "\\d\\d\\d\\d/\\d\\d";
         this.emMonth.Properties.Mask.IgnoreMaskBlank = false;
         this.emMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
         this.emMonth.Properties.Mask.PlaceHolder = '0';
         this.emMonth.Size = new System.Drawing.Size(75, 28);
         this.emMonth.TabIndex = 0;
         // 
         // st5
         // 
         this.st5.AutoSize = true;
         this.st5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.st5.Location = new System.Drawing.Point(27, 146);
         this.st5.Name = "st5";
         this.st5.Size = new System.Drawing.Size(90, 21);
         this.st5.TabIndex = 9999;
         this.st5.Text = "交易時段：";
         // 
         // textEdit1
         // 
         this.textEdit1.Location = new System.Drawing.Point(5, 5);
         this.textEdit1.MenuManager = this.ribbonControl;
         this.textEdit1.Name = "textEdit1";
         this.textEdit1.Size = new System.Drawing.Size(111, 26);
         this.textEdit1.TabIndex = 4;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(50, 20);
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.AppearanceGroup.BackColor = System.Drawing.Color.Transparent;
         this.layoutControlGroup1.AppearanceGroup.Options.UseBackColor = true;
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
         this.layoutControlGroup1.Size = new System.Drawing.Size(189, 36);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(50, 20);
         // 
         // W30720
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(605, 357);
         this.Controls.Add(this.panelControl2);
         this.Name = "W30720";
         this.Text = "W30720";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         this.panelControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.rFrame)).EndInit();
         this.rFrame.ResumeLayout(false);
         this.rFrame.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gb1)).EndInit();
         this.gb1.ResumeLayout(false);
         this.gb1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rgDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rgTime.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sleYear.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emMonth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private DevExpress.XtraEditors.PanelControl rFrame;
      private System.Windows.Forms.Label stMsgTxt;
      private System.Windows.Forms.Label gb1Label;
      private DevExpress.XtraEditors.PanelControl gb1;
      private DevExpress.XtraEditors.RadioGroup rgTime;
      private System.Windows.Forms.Label st5;
      private DevExpress.XtraEditors.RadioGroup rgDate;
      private DevExpress.XtraEditors.TextEdit textEdit1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraEditors.TextEdit sleYear;
      private DevExpress.XtraEditors.TextEdit emMonth;
   }
}