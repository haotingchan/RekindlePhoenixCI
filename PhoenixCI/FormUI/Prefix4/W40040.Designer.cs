﻿namespace PhoenixCI.FormUI.Prefix4
{
   partial class W40040
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
         this.gb = new DevExpress.XtraEditors.PanelControl();
         this.oswGrpLookItem = new DevExpress.XtraEditors.LookUpEdit();
         this.stOswGrp = new System.Windows.Forms.Label();
         this.stDate = new System.Windows.Forms.Label();
         this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emDate = new PhoenixCI.Widget.TextDateEdit();
         this.gp1Label = new System.Windows.Forms.Label();
         this.gp2Label = new System.Windows.Forms.Label();
         this.gp3Label = new System.Windows.Forms.Label();
         this.allLabel = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         this.panelControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gb)).BeginInit();
         this.gb.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.oswGrpLookItem.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emDate.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(813, 403);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(813, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl2
         // 
         this.panelControl2.Controls.Add(this.r_frame);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 30);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(813, 403);
         this.panelControl2.TabIndex = 1;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.allLabel);
         this.r_frame.Controls.Add(this.gp3Label);
         this.r_frame.Controls.Add(this.gp2Label);
         this.r_frame.Controls.Add(this.gp1Label);
         this.r_frame.Controls.Add(this.stMsgTxt);
         this.r_frame.Controls.Add(this.gb1_label);
         this.r_frame.Controls.Add(this.gb);
         this.r_frame.Location = new System.Drawing.Point(30, 27);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(747, 342);
         this.r_frame.TabIndex = 1;
         // 
         // stMsgTxt
         // 
         this.stMsgTxt.AutoSize = true;
         this.stMsgTxt.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.stMsgTxt.ForeColor = System.Drawing.Color.Blue;
         this.stMsgTxt.Location = new System.Drawing.Point(24, 305);
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
         // gb
         // 
         this.gb.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.gb.Appearance.Options.UseBackColor = true;
         this.gb.Appearance.Options.UseTextOptions = true;
         this.gb.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
         this.gb.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gb.Controls.Add(this.oswGrpLookItem);
         this.gb.Controls.Add(this.emDate);
         this.gb.Controls.Add(this.stOswGrp);
         this.gb.Controls.Add(this.stDate);
         this.gb.Location = new System.Drawing.Point(28, 25);
         this.gb.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.gb.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gb.Name = "gb";
         this.gb.Size = new System.Drawing.Size(647, 135);
         this.gb.TabIndex = 0;
         // 
         // oswGrpLookItem
         // 
         this.oswGrpLookItem.Location = new System.Drawing.Point(160, 73);
         this.oswGrpLookItem.MenuManager = this.ribbonControl;
         this.oswGrpLookItem.Name = "oswGrpLookItem";
         this.oswGrpLookItem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.oswGrpLookItem.Size = new System.Drawing.Size(161, 28);
         this.oswGrpLookItem.TabIndex = 6;
         // 
         // stOswGrp
         // 
         this.stOswGrp.AutoSize = true;
         this.stOswGrp.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.stOswGrp.Location = new System.Drawing.Point(45, 77);
         this.stOswGrp.Name = "stOswGrp";
         this.stOswGrp.Size = new System.Drawing.Size(122, 21);
         this.stOswGrp.TabIndex = 1;
         this.stOswGrp.Text = "商品交易時段：";
         // 
         // stDate
         // 
         this.stDate.AutoSize = true;
         this.stDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.stDate.Location = new System.Drawing.Point(45, 37);
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
         // emDate
         // 
         this.emDate.DateTimeValue = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
         this.emDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
         this.emDate.EditValue = "2019/01/01";
         this.emDate.Location = new System.Drawing.Point(98, 34);
         this.emDate.Name = "emDate";
         this.emDate.Properties.Appearance.Options.UseTextOptions = true;
         this.emDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.emDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.emDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.emDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.emDate.Size = new System.Drawing.Size(95, 28);
         this.emDate.TabIndex = 4;
         this.emDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
         // 
         // gp1Label
         // 
         this.gp1Label.AutoSize = true;
         this.gp1Label.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.gp1Label.ForeColor = System.Drawing.Color.Red;
         this.gp1Label.Location = new System.Drawing.Point(34, 180);
         this.gp1Label.Name = "gp1Label";
         this.gp1Label.Size = new System.Drawing.Size(520, 21);
         this.gp1Label.TabIndex = 2;
         this.gp1Label.Text = "執行本作業，選取商品交易時段為Group1時，請先執行40010、40020";
         // 
         // gp2Label
         // 
         this.gp2Label.AutoSize = true;
         this.gp2Label.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.gp2Label.ForeColor = System.Drawing.Color.Red;
         this.gp2Label.Location = new System.Drawing.Point(34, 209);
         this.gp2Label.Name = "gp2Label";
         this.gp2Label.Size = new System.Drawing.Size(520, 21);
         this.gp2Label.TabIndex = 3;
         this.gp2Label.Text = "執行本作業，選取商品交易時段為Group2時，請先執行40012、40022";
         // 
         // gp3Label
         // 
         this.gp3Label.AutoSize = true;
         this.gp3Label.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.gp3Label.ForeColor = System.Drawing.Color.Red;
         this.gp3Label.Location = new System.Drawing.Point(34, 238);
         this.gp3Label.Name = "gp3Label";
         this.gp3Label.Size = new System.Drawing.Size(454, 21);
         this.gp3Label.TabIndex = 3;
         this.gp3Label.Text = "執行本作業，選取商品交易時段為Group3時，請先執行40013";
         // 
         // allLabel
         // 
         this.allLabel.AutoSize = true;
         this.allLabel.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.allLabel.ForeColor = System.Drawing.Color.Red;
         this.allLabel.Location = new System.Drawing.Point(34, 267);
         this.allLabel.Name = "allLabel";
         this.allLabel.Size = new System.Drawing.Size(680, 21);
         this.allLabel.TabIndex = 4;
         this.allLabel.Text = "執行本作業，選取商品交易時段為All時，請先執行40010、40012、40013、40020及40022";
         // 
         // W40040
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(813, 433);
         this.Controls.Add(this.panelControl2);
         this.Name = "W40040";
         this.Text = "W40040";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         this.panelControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gb)).EndInit();
         this.gb.ResumeLayout(false);
         this.gb.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.oswGrpLookItem.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emDate.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label stMsgTxt;
      private System.Windows.Forms.Label gb1_label;
      private DevExpress.XtraEditors.PanelControl gb;
      private DevExpress.XtraEditors.TextEdit textEdit1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private System.Windows.Forms.Label stDate;
      private System.Windows.Forms.Label stOswGrp;
      private DevExpress.XtraEditors.LookUpEdit oswGrpLookItem;
      private Widget.TextDateEdit emDate;
      private System.Windows.Forms.Label allLabel;
      private System.Windows.Forms.Label gp3Label;
      private System.Windows.Forms.Label gp2Label;
      private System.Windows.Forms.Label gp1Label;
   }
}