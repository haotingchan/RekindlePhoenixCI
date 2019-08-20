﻿namespace PhoenixCI.FormUI.Prefix3
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
         this.stMsgTxt = new System.Windows.Forms.Label();
         this.emMonth = new BaseGround.Widget.TextDateEdit();
         this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.lblDate = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         this.panelControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.emMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.panFilter.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(590, 365);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(590, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl2
         // 
         this.panelControl2.Controls.Add(this.r_frame);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 30);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(590, 365);
         this.panelControl2.TabIndex = 1;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Controls.Add(this.stMsgTxt);
         this.r_frame.Location = new System.Drawing.Point(30, 30);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(370, 200);
         this.r_frame.TabIndex = 1;
         // 
         // stMsgTxt
         // 
         this.stMsgTxt.AutoSize = true;
         this.stMsgTxt.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.stMsgTxt.ForeColor = System.Drawing.Color.Blue;
         this.stMsgTxt.Location = new System.Drawing.Point(15, 150);
         this.stMsgTxt.MaximumSize = new System.Drawing.Size(330, 120);
         this.stMsgTxt.Name = "stMsgTxt";
         this.stMsgTxt.Size = new System.Drawing.Size(153, 19);
         this.stMsgTxt.TabIndex = 1;
         this.stMsgTxt.Text = "訊息：資料轉出中........";
         this.stMsgTxt.Visible = false;
         // 
         // emMonth
         // 
         this.emMonth.DateTimeValue = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
         this.emMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.emMonth.EditValue = "2019/01";
         this.emMonth.Location = new System.Drawing.Point(110, 54);
         this.emMonth.Name = "emMonth";
         this.emMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.emMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.emMonth.Properties.EditFormat.FormatString = "yyyyMM";
         this.emMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.emMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.emMonth.Properties.Mask.ShowPlaceHolders = false;
         this.emMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.emMonth.Size = new System.Drawing.Size(100, 26);
         this.emMonth.TabIndex = 4;
         this.emMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
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
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.emMonth);
         this.panFilter.Controls.Add(this.lblDate);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(330, 130);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.ForeColor = System.Drawing.Color.Black;
         this.lblDate.Location = new System.Drawing.Point(55, 57);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(58, 21);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "月份：";
         // 
         // W30310
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(590, 395);
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
         ((System.ComponentModel.ISupportInitialize)(this.emMonth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label stMsgTxt;
      private DevExpress.XtraEditors.TextEdit textEdit1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private BaseGround.Widget.TextDateEdit  emMonth;
      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label lblDate;
   }
}