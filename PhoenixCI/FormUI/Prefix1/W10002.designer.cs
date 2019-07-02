namespace PhoenixCI.FormUI.Prefix1
{
    partial class W10002
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
            if (disposing && (components != null))
            {
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
            this.txtPrevOcfDate = new BaseGround.Widget.TextDateEdit();
            this.lblPrevOcfDate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtOcfDate.Properties)).BeginInit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrevOcfDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOcfDate
            // 
            this.txtOcfDate.Location = new System.Drawing.Point(126, 42);
            this.txtOcfDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtOcfDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtOcfDate.Properties.EditFormat.FormatString = "yyyy/MM/dd";
            this.txtOcfDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtOcfDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtOcfDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtOcfDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblOcfDate
            // 
            this.lblOcfDate.Location = new System.Drawing.Point(31, 45);
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.txtPrevOcfDate);
            this.panParent.Controls.Add(this.lblPrevOcfDate);
            this.panParent.Controls.SetChildIndex(this.lblOcfDate, 0);
            this.panParent.Controls.SetChildIndex(this.txtOcfDate, 0);
            this.panParent.Controls.SetChildIndex(this.lblPrevOcfDate, 0);
            this.panParent.Controls.SetChildIndex(this.txtPrevOcfDate, 0);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // txtPrevOcfDate
            // 
            this.txtPrevOcfDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtPrevOcfDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtPrevOcfDate.EditValue = "0001/1/1 上午 12:00:00";
            this.txtPrevOcfDate.Location = new System.Drawing.Point(126, 9);
            this.txtPrevOcfDate.MenuManager = this.ribbonControl;
            this.txtPrevOcfDate.Name = "txtPrevOcfDate";
            this.txtPrevOcfDate.Properties.EditFormat.FormatString = "yyyy/MM/dd";
            this.txtPrevOcfDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtPrevOcfDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtPrevOcfDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtPrevOcfDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPrevOcfDate.Size = new System.Drawing.Size(100, 26);
            this.txtPrevOcfDate.TabIndex = 15;
            this.txtPrevOcfDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // lblPrevOcfDate
            // 
            this.lblPrevOcfDate.AutoSize = true;
            this.lblPrevOcfDate.Location = new System.Drawing.Point(15, 12);
            this.lblPrevOcfDate.Name = "lblPrevOcfDate";
            this.lblPrevOcfDate.Size = new System.Drawing.Size(105, 20);
            this.lblPrevOcfDate.TabIndex = 14;
            this.lblPrevOcfDate.Text = "前一交易日：";
            // 
            // W10002
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 761);
            this.Name = "W10002";
            ((System.ComponentModel.ISupportInitialize)(this.txtOcfDate.Properties)).EndInit();
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrevOcfDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BaseGround.Widget.TextDateEdit txtPrevOcfDate;
        private System.Windows.Forms.Label lblPrevOcfDate;
    }
}