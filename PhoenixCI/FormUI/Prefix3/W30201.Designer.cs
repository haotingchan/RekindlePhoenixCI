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
         this.txtEndMon = new PhoenixCI.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.txtStartMon = new PhoenixCI.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.labMsg = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMon.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMon.Properties)).BeginInit();
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
         this.panFilter.Controls.Add(this.txtEndMon);
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.txtStartMon);
         this.panFilter.Controls.Add(this.lblDate);
         this.panFilter.Location = new System.Drawing.Point(31, 29);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(404, 119);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtEndMon
         // 
         this.txtEndMon.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndMon.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndMon.EditValue = "2018/12";
         this.txtEndMon.Location = new System.Drawing.Point(237, 43);
         this.txtEndMon.MenuManager = this.ribbonControl;
         this.txtEndMon.Name = "txtEndMon";
         this.txtEndMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndMon.Properties.Mask.EditMask = "yyyy/MM";
         this.txtEndMon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndMon.Size = new System.Drawing.Size(100, 26);
         this.txtEndMon.TabIndex = 7;
         this.txtEndMon.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(206, 43);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(25, 20);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // txtStartMon
         // 
         this.txtStartMon.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartMon.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartMon.EditValue = "2018/12/12";
         this.txtStartMon.EnterMoveNextControl = true;
         this.txtStartMon.Location = new System.Drawing.Point(100, 43);
         this.txtStartMon.MenuManager = this.ribbonControl;
         this.txtStartMon.Name = "txtStartMon";
         this.txtStartMon.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartMon.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartMon.Properties.Mask.EditMask = "yyyy/MM";
         this.txtStartMon.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartMon.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartMon.Size = new System.Drawing.Size(100, 26);
         this.txtStartMon.TabIndex = 5;
         this.txtStartMon.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Location = new System.Drawing.Point(37, 46);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(57, 20);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "月份：";
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(27, 151);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(161, 20);
         this.labMsg.TabIndex = 11;
         this.labMsg.Text = "訊息：資料轉出中......";
         this.labMsg.Visible = false;
         // 
         // W30201
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Name = "W30201";
         this.Text = "W30201";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndMon.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMon.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox panFilter;
      private Widget.TextDateEdit txtStartMon;
      private System.Windows.Forms.Label lblDate;
      private Widget.TextDateEdit txtEndMon;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label labMsg;
   }
}