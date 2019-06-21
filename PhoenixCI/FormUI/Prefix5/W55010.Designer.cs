namespace PhoenixCI.FormUI.Prefix5 {
   partial class W55010 {
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
         this.lblDpt = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
         this.textEdit1 = new DevExpress.XtraEditors.DateEdit();
         this.textEdit2 = new DevExpress.XtraEditors.DateEdit();
         this.rb_repo = new DevExpress.XtraEditors.RadioGroup();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.lab_Msg = new System.Windows.Forms.Label();
         this.txtToDate = new BaseGround.Widget.TextDateEdit();
         this.txtFromDate = new BaseGround.Widget.TextDateEdit();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.label5 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties.CalendarTimeProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties.CalendarTimeProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rb_repo.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Controls.Add(this.lab_Msg);
         this.panParent.Size = new System.Drawing.Size(510, 280);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(510, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // lblDpt
         // 
         this.lblDpt.AutoSize = true;
         this.lblDpt.Location = new System.Drawing.Point(131, 101);
         this.lblDpt.Name = "lblDpt";
         this.lblDpt.Size = new System.Drawing.Size(41, 20);
         this.lblDpt.TabIndex = 12;
         this.lblDpt.Text = "月份";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(272, 101);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(21, 20);
         this.label2.TabIndex = 17;
         this.label2.Text = "~";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(107, 143);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(73, 20);
         this.label1.TabIndex = 19;
         this.label1.Text = "產出統計";
         // 
         // radioGroup1
         // 
         this.radioGroup1.Location = new System.Drawing.Point(166, 133);
         this.radioGroup1.Name = "radioGroup1";
         this.radioGroup1.Properties.Columns = 2;
         this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(0)), "單月報表"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(1)), "多月明細檔")});
         this.radioGroup1.Size = new System.Drawing.Size(223, 34);
         this.radioGroup1.TabIndex = 20;
         // 
         // textEdit1
         // 
         this.textEdit1.EditValue = null;
         this.textEdit1.Location = new System.Drawing.Point(166, 92);
         this.textEdit1.Name = "textEdit1";
         this.textEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.textEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.textEdit1.Properties.DisplayFormat.FormatString = "";
         this.textEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.textEdit1.Properties.EditFormat.FormatString = "";
         this.textEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.textEdit1.Properties.Mask.EditMask = "";
         this.textEdit1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
         this.textEdit1.Size = new System.Drawing.Size(100, 26);
         this.textEdit1.TabIndex = 16;
         // 
         // textEdit2
         // 
         this.textEdit2.EditValue = null;
         this.textEdit2.Location = new System.Drawing.Point(289, 92);
         this.textEdit2.Name = "textEdit2";
         this.textEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.textEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.textEdit2.Properties.DisplayFormat.FormatString = "";
         this.textEdit2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.textEdit2.Properties.EditFormat.FormatString = "";
         this.textEdit2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.textEdit2.Properties.Mask.EditMask = "";
         this.textEdit2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
         this.textEdit2.Size = new System.Drawing.Size(100, 26);
         this.textEdit2.TabIndex = 18;
         // 
         // rb_repo
         // 
         this.rb_repo.EditValue = ((short)(0));
         this.rb_repo.Location = new System.Drawing.Point(124, 82);
         this.rb_repo.Name = "rb_repo";
         this.rb_repo.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.rb_repo.Properties.Appearance.Options.UseBackColor = true;
         this.rb_repo.Properties.Columns = 2;
         this.rb_repo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(0)), "單月報表"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(1)), "多月明細檔")});
         this.rb_repo.Properties.LookAndFeel.SkinName = "Office 2013";
         this.rb_repo.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.rb_repo.Size = new System.Drawing.Size(233, 34);
         this.rb_repo.TabIndex = 26;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.ForeColor = System.Drawing.Color.Black;
         this.label3.Location = new System.Drawing.Point(37, 90);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(90, 21);
         this.label3.TabIndex = 25;
         this.label3.Text = "產出統計：";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(198, 45);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(22, 21);
         this.label4.TabIndex = 23;
         this.label4.Text = "~";
         // 
         // lab_Msg
         // 
         this.lab_Msg.AutoSize = true;
         this.lab_Msg.Location = new System.Drawing.Point(242, 218);
         this.lab_Msg.Name = "lab_Msg";
         this.lab_Msg.Size = new System.Drawing.Size(0, 20);
         this.lab_Msg.TabIndex = 27;
         // 
         // txtToDate
         // 
         this.txtToDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtToDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtToDate.EditValue = "2018/12";
         this.txtToDate.EnterMoveNextControl = true;
         this.txtToDate.Location = new System.Drawing.Point(225, 42);
         this.txtToDate.MenuManager = this.ribbonControl;
         this.txtToDate.Name = "txtToDate";
         this.txtToDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtToDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtToDate.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtToDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtToDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtToDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtToDate.Size = new System.Drawing.Size(100, 26);
         this.txtToDate.TabIndex = 30;
         this.txtToDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtFromDate
         // 
         this.txtFromDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtFromDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtFromDate.EditValue = "2018/12";
         this.txtFromDate.EnterMoveNextControl = true;
         this.txtFromDate.Location = new System.Drawing.Point(92, 42);
         this.txtFromDate.MenuManager = this.ribbonControl;
         this.txtFromDate.Name = "txtFromDate";
         this.txtFromDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtFromDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtFromDate.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtFromDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtFromDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtFromDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtFromDate.Size = new System.Drawing.Size(100, 26);
         this.txtFromDate.TabIndex = 29;
         this.txtFromDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Location = new System.Drawing.Point(30, 30);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(460, 220);
         this.r_frame.TabIndex = 84;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(16, 170);
         this.labMsg.MaximumSize = new System.Drawing.Size(420, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 80;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.rb_repo);
         this.panFilter.Controls.Add(this.txtToDate);
         this.panFilter.Controls.Add(this.txtFromDate);
         this.panFilter.Controls.Add(this.label3);
         this.panFilter.Controls.Add(this.label5);
         this.panFilter.Controls.Add(this.label4);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(420, 145);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.ForeColor = System.Drawing.Color.Black;
         this.label5.Location = new System.Drawing.Point(37, 45);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(58, 21);
         this.label5.TabIndex = 2;
         this.label5.Text = "月份：";
         // 
         // W55010
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(510, 310);
         this.Controls.Add(this.radioGroup1);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.lblDpt);
         this.Controls.Add(this.textEdit1);
         this.Controls.Add(this.textEdit2);
         this.Name = "W55010";
         this.Text = "W55010";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.textEdit2, 0);
         this.Controls.SetChildIndex(this.textEdit1, 0);
         this.Controls.SetChildIndex(this.lblDpt, 0);
         this.Controls.SetChildIndex(this.label2, 0);
         this.Controls.SetChildIndex(this.label1, 0);
         this.Controls.SetChildIndex(this.radioGroup1, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rb_repo.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Label lblDpt;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.RadioGroup radioGroup1;
      private DevExpress.XtraEditors.DateEdit textEdit1;
      private DevExpress.XtraEditors.DateEdit textEdit2;
      private DevExpress.XtraEditors.RadioGroup rb_repo;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label lab_Msg;
      private BaseGround.Widget.TextDateEdit txtToDate;
      private BaseGround.Widget.TextDateEdit txtFromDate;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label label5;
   }
}