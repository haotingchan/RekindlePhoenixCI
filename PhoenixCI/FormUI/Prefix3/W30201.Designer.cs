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
         this.grpxDescription = new System.Windows.Forms.GroupBox();
         this.txtToDate = new PhoenixCI.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.txtFromDate = new PhoenixCI.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
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
         this.grpxDescription.Controls.Add(this.txtToDate);
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.txtFromDate);
         this.grpxDescription.Controls.Add(this.lblDate);
         this.grpxDescription.Location = new System.Drawing.Point(31 , 29);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(404 , 119);
         this.grpxDescription.TabIndex = 6;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // txtToDate
         // 
         this.txtToDate.DateTimeValue = new System.DateTime(2018 , 12 , 1 , 0 , 0 , 0 , 0);
         this.txtToDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtToDate.EditValue = "2018/12";
         this.txtToDate.Location = new System.Drawing.Point(237 , 43);
         this.txtToDate.MenuManager = this.ribbonControl;
         this.txtToDate.Name = "txtToDate";
         this.txtToDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtToDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtToDate.Properties.Mask.EditMask = "yyyy/MM";
         this.txtToDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtToDate.Size = new System.Drawing.Size(100 , 26);
         this.txtToDate.TabIndex = 7;
         this.txtToDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(206 , 43);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(25 , 20);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // txtFromDate
         // 
         this.txtFromDate.DateTimeValue = new System.DateTime(2018 , 12 , 1 , 0 , 0 , 0 , 0);
         this.txtFromDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtFromDate.EditValue = "2018/12/12";
         this.txtFromDate.EnterMoveNextControl = true;
         this.txtFromDate.Location = new System.Drawing.Point(100 , 43);
         this.txtFromDate.MenuManager = this.ribbonControl;
         this.txtFromDate.Name = "txtFromDate";
         this.txtFromDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtFromDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtFromDate.Properties.Mask.EditMask = "yyyy/MM";
         this.txtFromDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtFromDate.Size = new System.Drawing.Size(100 , 26);
         this.txtFromDate.TabIndex = 5;
         this.txtFromDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Location = new System.Drawing.Point(37 , 46);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(57 , 20);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "月份：";
         // 
         // W30201
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F , 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836 , 574);
         this.Name = "W30201";
         this.Text = "W30201";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox grpxDescription;
      private Widget.TextDateEdit txtFromDate;
      private System.Windows.Forms.Label lblDate;
      private Widget.TextDateEdit txtToDate;
      private System.Windows.Forms.Label label1;
   }
}