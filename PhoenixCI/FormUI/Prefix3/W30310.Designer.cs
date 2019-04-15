namespace PhoenixCI.FormUI.Prefix3
{
   partial class W30310
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
         this.stMsgtxt = new System.Windows.Forms.Label();
         this.gb1_label = new System.Windows.Forms.Label();
         this.gb_1 = new DevExpress.XtraEditors.PanelControl();
         this.emMonth = new PhoenixCI.Widget.TextDateEdit();
         this.st_3 = new System.Windows.Forms.Label();
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
         ((System.ComponentModel.ISupportInitialize)(this.emMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(514, 220);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(514, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl2
         // 
         this.panelControl2.Controls.Add(this.r_frame);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 30);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(514, 220);
         this.panelControl2.TabIndex = 1;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.stMsgtxt);
         this.r_frame.Controls.Add(this.gb1_label);
         this.r_frame.Controls.Add(this.gb_1);
         this.r_frame.Location = new System.Drawing.Point(30, 27);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(456, 167);
         this.r_frame.TabIndex = 1;
         // 
         // st_msg_txt
         // 
         this.stMsgtxt.AutoSize = true;
         this.stMsgtxt.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.stMsgtxt.ForeColor = System.Drawing.Color.Blue;
         this.stMsgtxt.Location = new System.Drawing.Point(24, 135);
         this.stMsgtxt.Name = "st_msg_txt";
         this.stMsgtxt.Size = new System.Drawing.Size(153, 19);
         this.stMsgtxt.TabIndex = 1;
         this.stMsgtxt.Text = "訊息：資料轉出中........";
         this.stMsgtxt.Visible = false;
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
         this.gb_1.Controls.Add(this.emMonth);
         this.gb_1.Controls.Add(this.st_3);
         this.gb_1.Location = new System.Drawing.Point(28, 25);
         this.gb_1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.gb_1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gb_1.Name = "gb_1";
         this.gb_1.Size = new System.Drawing.Size(403, 96);
         this.gb_1.TabIndex = 0;
         // 
         // em_month
         // 
         this.emMonth.DateTimeValue = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
         this.emMonth.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.emMonth.EditValue = "2019/01";
         this.emMonth.Location = new System.Drawing.Point(99, 36);
         this.emMonth.Name = "emMonth";
         this.emMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.emMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.emMonth.Properties.Mask.EditMask = "yyyy/MM";
         this.emMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.emMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.emMonth.Size = new System.Drawing.Size(71, 28);
         this.emMonth.TabIndex = 4;
         // 
         // st_3
         // 
         this.st_3.AutoSize = true;
         this.st_3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.st_3.Location = new System.Drawing.Point(48, 40);
         this.st_3.Name = "st_3";
         this.st_3.Size = new System.Drawing.Size(58, 21);
         this.st_3.TabIndex = 0;
         this.st_3.Text = "月份：";
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
         // W30310
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(514, 250);
         this.Controls.Add(this.panelControl2);
         this.Name = "W30310";
         this.Text = "W30310";
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
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label stMsgtxt;
      private System.Windows.Forms.Label gb1_label;
      private DevExpress.XtraEditors.PanelControl gb_1;
      private System.Windows.Forms.Label st_3;
      private DevExpress.XtraEditors.TextEdit textEdit1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private PhoenixCI.Widget.TextDateEdit  emMonth;
   }
}