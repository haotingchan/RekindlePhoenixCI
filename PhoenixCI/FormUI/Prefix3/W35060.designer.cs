namespace PhoenixCI.FormUI.Prefix3
{
    partial class W35060
    {
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
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.label1 = new System.Windows.Forms.Label();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.label3 = new System.Windows.Forms.Label();
         this.ddlScCode = new DevExpress.XtraEditors.ComboBoxEdit();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         this.panelControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlScCode.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.panelControl2);
         this.panParent.Size = new System.Drawing.Size(805, 408);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(805, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(805, 408);
         this.panelControl1.TabIndex = 0;
         // 
         // panelControl2
         // 
         this.panelControl2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panelControl2.Appearance.Options.UseBackColor = true;
         this.panelControl2.Controls.Add(this.ddlScCode);
         this.panelControl2.Controls.Add(this.label1);
         this.panelControl2.Controls.Add(this.txtDate);
         this.panelControl2.Controls.Add(this.label3);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl2.Location = new System.Drawing.Point(12, 12);
         this.panelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(781, 77);
         this.panelControl2.TabIndex = 15;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.ForeColor = System.Drawing.Color.Black;
         this.label1.Location = new System.Drawing.Point(243, 29);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(57, 20);
         this.label1.TabIndex = 5;
         this.label1.Text = "資料：";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12/01";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(110, 24);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(100, 28);
         this.txtDate.TabIndex = 3;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.ForeColor = System.Drawing.Color.Black;
         this.label3.Location = new System.Drawing.Point(25, 28);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(89, 20);
         this.label3.TabIndex = 4;
         this.label3.Text = "交易日期：";
         // 
         // ddlScCode
         // 
         this.ddlScCode.Location = new System.Drawing.Point(296, 26);
         this.ddlScCode.MenuManager = this.ribbonControl;
         this.ddlScCode.Name = "ddlScCode";
         this.ddlScCode.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlScCode.Properties.Appearance.Options.UseBackColor = true;
         this.ddlScCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlScCode.Properties.Items.AddRange(new object[] {
            "CNH DEPO RATE (路透代碼CNHF=)",
            "CNH SWAP POINTS (路透代碼CNHF=)",
            "CNH Volatility (路透代碼CNHVOL=)",
            "USD DEPO RATE (路透代碼USDD=)"});
         this.ddlScCode.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlScCode.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlScCode.Size = new System.Drawing.Size(352, 26);
         this.ddlScCode.TabIndex = 87;
         // 
         // W35060
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(805, 438);
         this.Controls.Add(this.panelControl1);
         this.Name = "W35060";
         this.Text = "35010";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         this.panelControl2.ResumeLayout(false);
         this.panelControl2.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlScCode.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private System.Windows.Forms.Label label1;
      private BaseGround.Widget.TextDateEdit txtDate;
      private System.Windows.Forms.Label label3;
      private DevExpress.XtraEditors.ComboBoxEdit ddlScCode;
   }
}