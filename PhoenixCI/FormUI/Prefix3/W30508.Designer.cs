namespace PhoenixCI.FormUI.Prefix3
{
   partial class W30508
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
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.stMsgTxt = new System.Windows.Forms.Label();
         this.gb1_label = new System.Windows.Forms.Label();
         this.gb_1 = new DevExpress.XtraEditors.PanelControl();
         this.st_date = new DevExpress.XtraLayout.LayoutControl();
         this.emStartDate = new BaseGround.Widget.TextDateEdit();
         this.emEndDate = new BaseGround.Widget.TextDateEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.stDate = new System.Windows.Forms.Label();
         this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         this.panelControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gb_1)).BeginInit();
         this.gb_1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.st_date)).BeginInit();
         this.st_date.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.emStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(532, 232);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(532, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl2
         // 
         this.panelControl2.Controls.Add(this.r_frame);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 30);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(532, 232);
         this.panelControl2.TabIndex = 1;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.stMsgTxt);
         this.r_frame.Controls.Add(this.gb1_label);
         this.r_frame.Controls.Add(this.gb_1);
         this.r_frame.Location = new System.Drawing.Point(30, 27);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(461, 172);
         this.r_frame.TabIndex = 1;
         // 
         // stMsgTxt
         // 
         this.stMsgTxt.AutoSize = true;
         this.stMsgTxt.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.stMsgTxt.ForeColor = System.Drawing.Color.Blue;
         this.stMsgTxt.Location = new System.Drawing.Point(34, 138);
         this.stMsgTxt.Name = "stMsgTxt";
         this.stMsgTxt.Size = new System.Drawing.Size(153, 19);
         this.stMsgTxt.TabIndex = 1;
         this.stMsgTxt.Text = "訊息：資料轉出中........";
         this.stMsgTxt.Visible = false;
         // 
         // gb1_label
         // 
         this.gb1_label.AutoSize = true;
         this.gb1_label.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.gb1_label.ForeColor = System.Drawing.Color.Navy;
         this.gb1_label.Location = new System.Drawing.Point(34, 14);
         this.gb1_label.Name = "gb1_label";
         this.gb1_label.Size = new System.Drawing.Size(122, 21);
         this.gb1_label.TabIndex = 0;
         this.gb1_label.Text = "請輸入交易日期";
         // 
         // gb_1
         // 
         this.gb_1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.gb_1.Appearance.Options.UseBackColor = true;
         this.gb_1.Appearance.Options.UseTextOptions = true;
         this.gb_1.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
         this.gb_1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gb_1.Controls.Add(this.st_date);
         this.gb_1.Controls.Add(this.stDate);
         this.gb_1.Location = new System.Drawing.Point(28, 25);
         this.gb_1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.gb_1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gb_1.Name = "gb_1";
         this.gb_1.Size = new System.Drawing.Size(406, 90);
         this.gb_1.TabIndex = 0;
         // 
         // st_date
         // 
         this.st_date.Controls.Add(this.emStartDate);
         this.st_date.Controls.Add(this.emEndDate);
         this.st_date.Location = new System.Drawing.Point(109, 32);
         this.st_date.Name = "st_date";
         this.st_date.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(666, 57, 650, 400);
         this.st_date.Root = this.Root;
         this.st_date.Size = new System.Drawing.Size(229, 33);
         this.st_date.TabIndex = 4;
         this.st_date.Text = "layoutControl1";
         // 
         // emStartDate
         // 
         this.emStartDate.DateTimeValue = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
         this.emStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.emStartDate.EditValue = "2019/01/01";
         this.emStartDate.Location = new System.Drawing.Point(2, 2);
         this.emStartDate.Name = "emStartDate";
         this.emStartDate.Properties.Appearance.Options.UseTextOptions = true;
         this.emStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.emStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.emStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.emStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.emStartDate.Size = new System.Drawing.Size(102, 28);
         this.emStartDate.StyleController = this.st_date;
         this.emStartDate.TabIndex = 4;
         // 
         // emEndDate
         // 
         this.emEndDate.DateTimeValue = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
         this.emEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.emEndDate.EditValue = "2019/01/01";
         this.emEndDate.Location = new System.Drawing.Point(125, 2);
         this.emEndDate.Name = "emEndDate";
         this.emEndDate.Properties.Appearance.Options.UseTextOptions = true;
         this.emEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.emEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.emEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.emEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.emEndDate.Size = new System.Drawing.Size(102, 28);
         this.emEndDate.StyleController = this.st_date;
         this.emEndDate.TabIndex = 5;
         // 
         // Root
         // 
         this.Root.AppearanceGroup.BackColor = System.Drawing.Color.Transparent;
         this.Root.AppearanceGroup.Options.UseBackColor = true;
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem4});
         this.Root.Name = "Root";
         this.Root.OptionsItemText.TextToControlDistance = 5;
         this.Root.Size = new System.Drawing.Size(229, 33);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.emStartDate;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
         this.layoutControlItem2.Size = new System.Drawing.Size(106, 33);
         this.layoutControlItem2.Text = "layoutControlItem1";
         this.layoutControlItem2.TextLocation = DevExpress.Utils.Locations.Left;
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.emEndDate;
         this.layoutControlItem4.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem4.Location = new System.Drawing.Point(106, 0);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
         this.layoutControlItem4.Size = new System.Drawing.Size(123, 33);
         this.layoutControlItem4.Text = "~";
         this.layoutControlItem4.TextLocation = DevExpress.Utils.Locations.Left;
         this.layoutControlItem4.TextSize = new System.Drawing.Size(12, 20);
         // 
         // stDate
         // 
         this.stDate.AutoSize = true;
         this.stDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.stDate.Location = new System.Drawing.Point(49, 38);
         this.stDate.Name = "stDate";
         this.stDate.Size = new System.Drawing.Size(58, 21);
         this.stDate.TabIndex = 1;
         this.stDate.Text = "日期：";
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
         // W30508
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(532, 262);
         this.Controls.Add(this.panelControl2);
         this.Name = "W30508";
         this.Text = "W30508";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         this.panelControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gb_1)).EndInit();
         this.gb_1.ResumeLayout(false);
         this.gb_1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.st_date)).EndInit();
         this.st_date.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.emStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label stMsgTxt;
      private System.Windows.Forms.Label gb1_label;
      private DevExpress.XtraEditors.PanelControl gb_1;
      private DevExpress.XtraEditors.TextEdit textEdit1;
      private DevExpress.XtraLayout.LayoutControl st_date;
      private BaseGround.Widget.TextDateEdit emStartDate;
      private BaseGround.Widget.TextDateEdit emEndDate;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private System.Windows.Forms.Label stDate;
   }
}