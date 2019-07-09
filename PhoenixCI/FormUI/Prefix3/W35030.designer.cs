namespace PhoenixCI.FormUI.Prefix3
{
    partial class W35030
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
         this.ExportShow = new System.Windows.Forms.Label();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.grpxDescription = new System.Windows.Forms.GroupBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.label6 = new System.Windows.Forms.Label();
         this.txt = new DevExpress.XtraEditors.TextEdit();
         this.label7 = new System.Windows.Forms.Label();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.label8 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.grpxDescription.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txt.Properties)).BeginInit();
         this.groupBox2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.ExportShow);
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(1175, 725);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(1175, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(1175, 725);
         this.panelControl1.TabIndex = 0;
         // 
         // ExportShow
         // 
         this.ExportShow.AutoSize = true;
         this.ExportShow.ForeColor = System.Drawing.Color.Blue;
         this.ExportShow.Location = new System.Drawing.Point(26, 352);
         this.ExportShow.MaximumSize = new System.Drawing.Size(360, 120);
         this.ExportShow.Name = "ExportShow";
         this.ExportShow.Size = new System.Drawing.Size(85, 20);
         this.ExportShow.TabIndex = 83;
         this.ExportShow.Text = "開始轉檔...";
         this.ExportShow.Visible = false;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.grpxDescription);
         this.r_frame.Location = new System.Drawing.Point(30, 30);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(438, 316);
         this.r_frame.TabIndex = 84;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.Controls.Add(this.groupBox1);
         this.grpxDescription.Controls.Add(this.groupBox2);
         this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
         this.grpxDescription.Location = new System.Drawing.Point(15, 20);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(407, 276);
         this.grpxDescription.TabIndex = 16;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // groupBox1
         // 
         this.groupBox1.AutoSize = true;
         this.groupBox1.Controls.Add(this.label6);
         this.groupBox1.Controls.Add(this.txt);
         this.groupBox1.Controls.Add(this.label7);
         this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.groupBox1.ForeColor = System.Drawing.Color.Navy;
         this.groupBox1.Location = new System.Drawing.Point(24, 32);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(360, 114);
         this.groupBox1.TabIndex = 6;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "證交所權證";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Font = new System.Drawing.Font("微軟正黑體", 10F);
         this.label6.ForeColor = System.Drawing.Color.Black;
         this.label6.Location = new System.Drawing.Point(131, 71);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(173, 18);
         this.label6.TabIndex = 7;
         this.label6.Text = "(例如：2011Q2代表第2季)";
         // 
         // txt
         // 
         this.txt.Location = new System.Drawing.Point(135, 42);
         this.txt.MenuManager = this.ribbonControl;
         this.txt.Name = "txt";
         this.txt.Size = new System.Drawing.Size(143, 26);
         this.txt.TabIndex = 6;
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.ForeColor = System.Drawing.Color.Black;
         this.label7.Location = new System.Drawing.Point(37, 45);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(102, 21);
         this.label7.TabIndex = 2;
         this.label7.Text = "年度+季別：";
         // 
         // groupBox2
         // 
         this.groupBox2.AutoSize = true;
         this.groupBox2.Controls.Add(this.txtDate);
         this.groupBox2.Controls.Add(this.label8);
         this.groupBox2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.groupBox2.ForeColor = System.Drawing.Color.Navy;
         this.groupBox2.Location = new System.Drawing.Point(24, 152);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(360, 96);
         this.groupBox2.TabIndex = 8;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "本公司上市標的";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(91, 42);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(144, 26);
         this.txtDate.TabIndex = 15;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.ForeColor = System.Drawing.Color.Black;
         this.label8.Location = new System.Drawing.Point(37, 45);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(58, 21);
         this.label8.TabIndex = 2;
         this.label8.Text = "日期：";
         // 
         // W35030
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1175, 755);
         this.Controls.Add(this.panelControl1);
         this.Name = "W35030";
         this.Text = "35030";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txt.Properties)).EndInit();
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
      private System.Windows.Forms.Label ExportShow;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.GroupBox grpxDescription;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Label label6;
      private DevExpress.XtraEditors.TextEdit txt;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.GroupBox groupBox2;
      private BaseGround.Widget.TextDateEdit txtDate;
      private System.Windows.Forms.Label label8;
   }
}