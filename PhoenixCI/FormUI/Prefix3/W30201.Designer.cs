namespace PhoenixCI.FormUI.Prefix3 {

   partial class W30201 {
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
         this.txtEndMon = new BaseGround.Widget.TextDateEdit();
         this.txtStartMon = new BaseGround.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.lblDate = new System.Windows.Forms.Label();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMon.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMon.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(590, 365);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(590, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.txtEndMon);
         this.panFilter.Controls.Add(this.txtStartMon);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.lblDate);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(360, 100);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtEndMon
         // 
         this.txtEndMon.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndMon.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndMon.EditValue = "2018/12";
         this.txtEndMon.EnterMoveNextControl = true;
         this.txtEndMon.Location = new System.Drawing.Point(220, 42);
         this.txtEndMon.MenuManager = this.ribbonControl;
         this.txtEndMon.Name = "txtEndMon";
         this.txtEndMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndMon.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtEndMon.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtEndMon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEndMon.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndMon.Size = new System.Drawing.Size(100, 26);
         this.txtEndMon.TabIndex = 1;
         this.txtEndMon.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartMon
         // 
         this.txtStartMon.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartMon.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartMon.EditValue = "2018/12";
         this.txtStartMon.EnterMoveNextControl = true;
         this.txtStartMon.Location = new System.Drawing.Point(92, 42);
         this.txtStartMon.MenuManager = this.ribbonControl;
         this.txtStartMon.Name = "txtStartMon";
         this.txtStartMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartMon.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtStartMon.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtStartMon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtStartMon.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartMon.Size = new System.Drawing.Size(100, 26);
         this.txtStartMon.TabIndex = 0;
         this.txtStartMon.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(194, 45);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(26, 21);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.ForeColor = System.Drawing.Color.Black;
         this.lblDate.Location = new System.Drawing.Point(37, 45);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(58, 21);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "月份：";
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
         this.r_frame.Size = new System.Drawing.Size(400, 175);
         this.r_frame.TabIndex = 79;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 125);
         this.labMsg.MaximumSize = new System.Drawing.Size(360, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 80;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // W30201
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(590, 395);
         this.Name = "W30201";
         this.Text = "W30201";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMon.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMon.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label lblDate;
      private System.Windows.Forms.Label label1;
      private BaseGround.Widget.TextDateEdit txtEndMon;
      private BaseGround.Widget.TextDateEdit txtStartMon;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
   }
}