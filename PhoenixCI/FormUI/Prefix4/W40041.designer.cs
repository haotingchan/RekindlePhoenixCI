﻿namespace PhoenixCI.FormUI.Prefix4 {
    partial class W40041 {
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
         this.label4 = new System.Windows.Forms.Label();
         this.ModelTypies = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
         this.oswGrpLookItem = new DevExpress.XtraEditors.LookUpEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.prodLookItem = new DevExpress.XtraEditors.LookUpEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.ExportShow = new System.Windows.Forms.Label();
         this.panel1 = new System.Windows.Forms.Panel();
         this.label3 = new System.Windows.Forms.Label();
         this.reCountBtn = new DevExpress.XtraEditors.SimpleButton();
         this.panel2 = new System.Windows.Forms.Panel();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ModelTypies)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.oswGrpLookItem.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.prodLookItem.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.panel2);
         this.panParent.Controls.Add(this.panel1);
         this.panParent.Size = new System.Drawing.Size(871, 591);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(871, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(871, 591);
         this.panelControl1.TabIndex = 0;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.grpxDescription.Controls.Add(this.label4);
         this.grpxDescription.Controls.Add(this.ModelTypies);
         this.grpxDescription.Controls.Add(this.radioGroup1);
         this.grpxDescription.Controls.Add(this.oswGrpLookItem);
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.prodLookItem);
         this.grpxDescription.Controls.Add(this.label2);
         this.grpxDescription.Controls.Add(this.txtDate);
         this.grpxDescription.Controls.Add(this.lblDate);
         this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
         this.grpxDescription.Location = new System.Drawing.Point(9, 3);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(544, 325);
         this.grpxDescription.TabIndex = 13;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.ForeColor = System.Drawing.Color.Black;
         this.label4.Location = new System.Drawing.Point(37, 162);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(62, 21);
         this.label4.TabIndex = 74;
         this.label4.Text = "模型 ：";
         // 
         // ModelTypies
         // 
         this.ModelTypies.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.ModelTypies.Appearance.Options.UseBackColor = true;
         this.ModelTypies.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.ModelTypies.CheckOnClick = true;
         this.ModelTypies.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("S", "SMA", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("s", "FUT-SMA", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("M", "MAXVol"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("m", "FUT-MAXVol"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("E", "EWMA"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("e", "FUT-EWMA")});
         this.ModelTypies.Location = new System.Drawing.Point(41, 188);
         this.ModelTypies.LookAndFeel.SkinName = "Office 2013";
         this.ModelTypies.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ModelTypies.MultiColumn = true;
         this.ModelTypies.Name = "ModelTypies";
         this.ModelTypies.SelectionMode = System.Windows.Forms.SelectionMode.None;
         this.ModelTypies.Size = new System.Drawing.Size(410, 57);
         this.ModelTypies.TabIndex = 17;
         // 
         // radioGroup1
         // 
         this.radioGroup1.Location = new System.Drawing.Point(40, 256);
         this.radioGroup1.MenuManager = this.ribbonControl;
         this.radioGroup1.Name = "radioGroup1";
         this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
         this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.radioGroup1.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
         this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ALL", "全選"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Cancel", "全取消"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Index", "全選指數類"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ETF", "全選ETF")});
         this.radioGroup1.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.radioGroup1.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.radioGroup1.Properties.EditValueChanged += new System.EventHandler(this.radioGroup1_Properties_EditValueChanged);
         this.radioGroup1.Size = new System.Drawing.Size(411, 37);
         this.radioGroup1.TabIndex = 73;
         // 
         // oswGrpLookItem
         // 
         this.oswGrpLookItem.Location = new System.Drawing.Point(160, 128);
         this.oswGrpLookItem.Name = "oswGrpLookItem";
         this.oswGrpLookItem.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.oswGrpLookItem.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.oswGrpLookItem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.oswGrpLookItem.Properties.LookAndFeel.SkinName = "The Bezier";
         this.oswGrpLookItem.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.oswGrpLookItem.Properties.NullText = "";
         this.oswGrpLookItem.Properties.PopupSizeable = false;
         this.oswGrpLookItem.Size = new System.Drawing.Size(150, 26);
         this.oswGrpLookItem.TabIndex = 72;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.ForeColor = System.Drawing.Color.Black;
         this.label1.Location = new System.Drawing.Point(37, 131);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(126, 21);
         this.label1.TabIndex = 71;
         this.label1.Text = "商品交易時段 ：";
         // 
         // prodLookItem
         // 
         this.prodLookItem.Location = new System.Drawing.Point(160, 85);
         this.prodLookItem.Name = "prodLookItem";
         this.prodLookItem.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.prodLookItem.Properties.Appearance.Options.UseBackColor = true;
         this.prodLookItem.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.prodLookItem.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.prodLookItem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.prodLookItem.Properties.LookAndFeel.SkinName = "The Bezier";
         this.prodLookItem.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.prodLookItem.Properties.NullText = "";
         this.prodLookItem.Properties.PopupSizeable = false;
         this.prodLookItem.Size = new System.Drawing.Size(291, 26);
         this.prodLookItem.TabIndex = 70;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.ForeColor = System.Drawing.Color.Black;
         this.label2.Location = new System.Drawing.Point(37, 88);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(58, 21);
         this.label2.TabIndex = 69;
         this.label2.Text = "商品：";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12/01";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(160, 43);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(150, 26);
         this.txtDate.TabIndex = 15;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.ForeColor = System.Drawing.Color.Black;
         this.lblDate.Location = new System.Drawing.Point(37, 46);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(58, 21);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "日期：";
         // 
         // ExportShow
         // 
         this.ExportShow.AutoSize = true;
         this.ExportShow.Location = new System.Drawing.Point(5, 331);
         this.ExportShow.Name = "ExportShow";
         this.ExportShow.Size = new System.Drawing.Size(54, 20);
         this.ExportShow.TabIndex = 14;
         this.ExportShow.Text = "label1";
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.label3);
         this.panel1.Controls.Add(this.reCountBtn);
         this.panel1.Controls.Add(this.grpxDescription);
         this.panel1.Controls.Add(this.ExportShow);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(12, 12);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(847, 354);
         this.panel1.TabIndex = 15;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(503, 331);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(341, 20);
         this.label3.TabIndex = 16;
         this.label3.Text = "註: 上次調整公告日 = 上次調整日之前一交易日";
         // 
         // reCountBtn
         // 
         this.reCountBtn.Location = new System.Drawing.Point(559, 272);
         this.reCountBtn.Name = "reCountBtn";
         this.reCountBtn.Size = new System.Drawing.Size(135, 42);
         this.reCountBtn.TabIndex = 15;
         this.reCountBtn.Text = "重新計算";
         this.reCountBtn.Visible = false;
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.gcMain);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel2.Location = new System.Drawing.Point(12, 366);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(847, 213);
         this.panel2.TabIndex = 16;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(0, 0);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1});
         this.gcMain.Size = new System.Drawing.Size(847, 213);
         this.gcMain.TabIndex = 0;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         // 
         // repositoryItemButtonEdit1
         // 
         this.repositoryItemButtonEdit1.AutoHeight = false;
         this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
         // 
         // W40041
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(871, 621);
         this.Controls.Add(this.panelControl1);
         this.Name = "W40041";
         this.Text = "40041";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ModelTypies)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.oswGrpLookItem.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.prodLookItem.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label ExportShow;
        private BaseGround.Widget.TextDateEdit txtDate;
      private System.Windows.Forms.Panel panel2;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private System.Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.RadioGroup radioGroup1;
      private DevExpress.XtraEditors.LookUpEdit oswGrpLookItem;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.LookUpEdit prodLookItem;
      private System.Windows.Forms.Label label2;
      private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
      private DevExpress.XtraEditors.SimpleButton reCountBtn;
      private System.Windows.Forms.Label label3;
      private DevExpress.XtraEditors.CheckedListBoxControl ModelTypies;
      private System.Windows.Forms.Label label4;
   }
}