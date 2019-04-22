namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30395 {
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
         this.txtStartMonth = new BaseGround.Widget.TextDateEdit();
         this.label6 = new System.Windows.Forms.Label();
         this.labMsg = new System.Windows.Forms.Label();
         this.labMonth = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.labMsg);
         this.panParent.Controls.Add(this.panFilter);
         this.panParent.Size = new System.Drawing.Size(648, 304);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(648, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Controls.Add(this.labMonth);
         this.panFilter.Controls.Add(this.txtStartMonth);
         this.panFilter.Controls.Add(this.label6);
         this.panFilter.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(15, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(367, 109);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtStartMonth
         // 
         this.txtStartMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartMonth.EditValue = "2018/12";
         this.txtStartMonth.EnterMoveNextControl = true;
         this.txtStartMonth.Location = new System.Drawing.Point(103, 26);
         this.txtStartMonth.MenuManager = this.ribbonControl;
         this.txtStartMonth.Name = "txtStartMonth";
         this.txtStartMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartMonth.Properties.Mask.EditMask = "yyyy/MM";
         this.txtStartMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartMonth.Size = new System.Drawing.Size(100, 26);
         this.txtStartMonth.TabIndex = 0;
         this.txtStartMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.ForeColor = System.Drawing.Color.MidnightBlue;
         this.label6.Location = new System.Drawing.Point(136, 308);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(0, 16);
         this.label6.TabIndex = 73;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 127);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(169, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "訊息：資料轉出中........";
         this.labMsg.Visible = false;
         // 
         // labMonth
         // 
         this.labMonth.AutoSize = true;
         this.labMonth.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labMonth.Location = new System.Drawing.Point(39, 29);
         this.labMonth.Name = "labMonth";
         this.labMonth.Size = new System.Drawing.Size(58, 21);
         this.labMonth.TabIndex = 74;
         this.labMonth.Text = "月份：";
         // 
         // W30395
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(648, 334);
         this.Name = "W30395";
         this.Text = "W30395";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.Label label6;
        private BaseGround.Widget.TextDateEdit txtStartMonth;
      private System.Windows.Forms.Label labMonth;
   }
}