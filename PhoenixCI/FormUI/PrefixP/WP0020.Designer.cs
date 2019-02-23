namespace PhoenixCI.FormUI.PrefixP {
   partial class WP0020 {
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
         this.label1 = new System.Windows.Forms.Label();
         this.lblDpt = new System.Windows.Forms.Label();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.txtFcmNo = new DevExpress.XtraEditors.TextEdit();
         this.txtAccNo = new DevExpress.XtraEditors.TextEdit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFcmNo.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAccNo.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Location = new System.Drawing.Point(0, 85);
         this.panParent.Size = new System.Drawing.Size(763, 406);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(763, 32);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Controls.Add(this.lblDpt);
         this.panelControl1.Controls.Add(this.txtFcmNo);
         this.panelControl1.Controls.Add(this.txtAccNo);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 32);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(763, 53);
         this.panelControl1.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(260, 17);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(89, 20);
         this.label1.TabIndex = 58;
         this.label1.Text = "投資人帳號";
         // 
         // lblDpt
         // 
         this.lblDpt.AutoSize = true;
         this.lblDpt.Location = new System.Drawing.Point(26, 17);
         this.lblDpt.Name = "lblDpt";
         this.lblDpt.Size = new System.Drawing.Size(89, 20);
         this.lblDpt.TabIndex = 54;
         this.lblDpt.Text = "期貨商代號";
         // 
         // panelControl2
         // 
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 85);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(763, 406);
         this.panelControl2.TabIndex = 1;
         // 
         // txtFcmNo
         // 
         this.txtFcmNo.EditValue = "%";
         this.txtFcmNo.Location = new System.Drawing.Point(121, 14);
         this.txtFcmNo.Name = "txtFcmNo";
         this.txtFcmNo.Properties.Mask.EditMask = "yyyy/MM";
         this.txtFcmNo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtFcmNo.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtFcmNo.Size = new System.Drawing.Size(108, 26);
         this.txtFcmNo.TabIndex = 55;
         // 
         // txtAccNo
         // 
         this.txtAccNo.Location = new System.Drawing.Point(355, 14);
         this.txtAccNo.Name = "txtAccNo";
         this.txtAccNo.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.txtAccNo.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.txtAccNo.Size = new System.Drawing.Size(122, 26);
         this.txtAccNo.TabIndex = 62;
         // 
         // WP0020
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(763, 491);
         this.Controls.Add(this.panelControl2);
         this.Controls.Add(this.panelControl1);
         this.Name = "WP0020";
         this.Text = "WP0020";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFcmNo.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtAccNo.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private System.Windows.Forms.Label lblDpt;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.TextEdit txtFcmNo;
      private DevExpress.XtraEditors.TextEdit txtAccNo;
   }
}