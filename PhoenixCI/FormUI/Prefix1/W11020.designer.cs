﻿namespace PhoenixCI.FormUI.Prefix1
{
    partial class W11020
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
            ((System.ComponentModel.ISupportInitialize)(this.txtOcfDate.Properties)).BeginInit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOcfDate
            // 
            this.txtOcfDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtOcfDate.Location = new System.Drawing.Point(78, 37);
            this.txtOcfDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtOcfDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtOcfDate.Properties.EditFormat.FormatString = "yyyyMM";
            this.txtOcfDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
            this.txtOcfDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtOcfDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtOcfDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtOcfDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblOcfDate
            // 
            this.lblOcfDate.Location = new System.Drawing.Point(15, 40);
            this.lblOcfDate.Size = new System.Drawing.Size(57, 20);
            this.lblOcfDate.Text = "年月：";
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // W11020
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 761);
            this.Name = "W11020";
            ((System.ComponentModel.ISupportInitialize)(this.txtOcfDate.Properties)).EndInit();
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}