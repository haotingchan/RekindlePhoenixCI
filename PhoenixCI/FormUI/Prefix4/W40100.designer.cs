namespace PhoenixCI.FormUI.Prefix4
{
    partial class W40100
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
         this.grpxDescription = new System.Windows.Forms.GroupBox();
         this.label1 = new System.Windows.Forms.Label();
         this.ddlAdjType = new DevExpress.XtraEditors.LookUpEdit();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.ExportShow = new System.Windows.Forms.Label();
         this.panel1 = new System.Windows.Forms.Panel();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ddlAdjType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         this.panel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.panel1);
         this.panParent.Size = new System.Drawing.Size(713, 499);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(713, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(713, 499);
         this.panelControl1.TabIndex = 0;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.ddlAdjType);
         this.grpxDescription.Controls.Add(this.txtDate);
         this.grpxDescription.Controls.Add(this.lblDate);
         this.grpxDescription.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
         this.grpxDescription.Location = new System.Drawing.Point(14, 21);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(404, 164);
         this.grpxDescription.TabIndex = 13;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.label1.Location = new System.Drawing.Point(37, 91);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(138, 21);
         this.label1.TabIndex = 16;
         this.label1.Text = "保證金調整狀況：";
         // 
         // ddlAdjType
         // 
         this.ddlAdjType.Location = new System.Drawing.Point(180, 88);
         this.ddlAdjType.Name = "ddlAdjType";
         this.ddlAdjType.Properties.Appearance.BackColor = System.Drawing.Color.White;
         this.ddlAdjType.Properties.Appearance.Options.UseBackColor = true;
         this.ddlAdjType.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlAdjType.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlAdjType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlAdjType.Properties.DropDownRows = 5;
         this.ddlAdjType.Properties.NullText = "";
         this.ddlAdjType.Properties.PopupSizeable = false;
         this.ddlAdjType.Size = new System.Drawing.Size(144, 26);
         this.ddlAdjType.TabIndex = 7;
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12/01";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(128, 42);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(144, 26);
         this.txtDate.TabIndex = 15;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.lblDate.Location = new System.Drawing.Point(37, 46);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(58, 21);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "日期：";
         // 
         // ExportShow
         // 
         this.ExportShow.AutoSize = true;
         this.ExportShow.ForeColor = System.Drawing.Color.Navy;
         this.ExportShow.Location = new System.Drawing.Point(10, 188);
         this.ExportShow.Name = "ExportShow";
         this.ExportShow.Size = new System.Drawing.Size(54, 20);
         this.ExportShow.TabIndex = 14;
         this.ExportShow.Text = "label1";
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panel1.Controls.Add(this.grpxDescription);
         this.panel1.Controls.Add(this.ExportShow);
         this.panel1.Location = new System.Drawing.Point(30, 30);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(433, 224);
         this.panel1.TabIndex = 15;
         // 
         // W40100
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(713, 529);
         this.Controls.Add(this.panelControl1);
         this.Name = "W40100";
         this.Text = "40100";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ddlAdjType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label ExportShow;
      private DevExpress.XtraEditors.LookUpEdit ddlAdjType;
      private System.Windows.Forms.Label label1;
      public BaseGround.Widget.TextDateEdit txtDate;
      private System.Windows.Forms.Panel panel1;
   }
}