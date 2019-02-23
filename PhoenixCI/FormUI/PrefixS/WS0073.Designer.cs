namespace PhoenixCI.FormUI.PrefixS
{
    partial class WS0073
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.labSpanDesc = new System.Windows.Forms.Label();
            this.txtEndDate = new PhoenixCI.Widget.TextDateEdit();
            this.txtStartDate = new PhoenixCI.Widget.TextDateEdit();
            this.lbl1 = new System.Windows.Forms.Label();
            this.labDate = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.SPAN_MARGIN_SPN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_MARGIN_SPN_PATH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_MARGIN_POS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_MARGIN_RATIO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_MARGIN_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPAN_MARGIN_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.panel2);
            this.panParent.Controls.Add(this.panel1);
            this.panParent.Size = new System.Drawing.Size(1032, 655);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1032, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labSpanDesc);
            this.panel1.Controls.Add(this.txtEndDate);
            this.panel1.Controls.Add(this.txtStartDate);
            this.panel1.Controls.Add(this.lbl1);
            this.panel1.Controls.Add(this.labDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 148);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(367, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(613, 100);
            this.label1.TabIndex = 1;
            this.label1.Text = "備註：\r\n1.  如選擇路徑為D:\\SPAN_TEST\\UNZIP，則以現行之資料計算。\r\n2.  S0073功能係以指定之 SPAN參數檔案及部位檔案產生SPA" +
    "N保證金計算結果資料，\r\n並儲存於D:\\SPAN_TEST\\Margin。\r\n3.  本作業每執行一日之資料約為5分鐘，敬請耐心等候。\r\n";
            // 
            // labSpanDesc
            // 
            this.labSpanDesc.AutoSize = true;
            this.labSpanDesc.Location = new System.Drawing.Point(7, 125);
            this.labSpanDesc.Name = "labSpanDesc";
            this.labSpanDesc.Size = new System.Drawing.Size(272, 20);
            this.labSpanDesc.TabIndex = 19;
            this.labSpanDesc.Text = "(2) 指定保證金計算之參數檔部位設定";
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtEndDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate.EditValue = "0001/1/1 上午 12:00:00";
            this.txtEndDate.Location = new System.Drawing.Point(252, 13);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.DisplayFormat.FormatString = "yyyyMMdd";
            this.txtEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Size = new System.Drawing.Size(100, 26);
            this.txtEndDate.TabIndex = 18;
            this.txtEndDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtStartDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate.EditValue = "0001/1/1 上午 12:00:00";
            this.txtStartDate.Location = new System.Drawing.Point(125, 13);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.DisplayFormat.FormatString = "yyyyMMdd";
            this.txtStartDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtStartDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtStartDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Size = new System.Drawing.Size(100, 26);
            this.txtStartDate.TabIndex = 15;
            this.txtStartDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(231, 16);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(21, 20);
            this.lbl1.TabIndex = 17;
            this.lbl1.Text = "~";
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.Location = new System.Drawing.Point(7, 16);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(112, 20);
            this.labDate.TabIndex = 16;
            this.labDate.Text = "(1) 日期區間：";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gcMain);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(12, 160);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 483);
            this.panel2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(0, 0);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1008, 483);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.SPAN_MARGIN_SPN,
            this.SPAN_MARGIN_SPN_PATH,
            this.SPAN_MARGIN_POS,
            this.SPAN_MARGIN_RATIO,
            this.SPAN_MARGIN_USER_ID,
            this.SPAN_MARGIN_W_TIME});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            // 
            // SPAN_MARGIN_SPN
            // 
            this.SPAN_MARGIN_SPN.Caption = "參數設定";
            this.SPAN_MARGIN_SPN.FieldName = "SPAN_MARGIN_SPN";
            this.SPAN_MARGIN_SPN.Name = "SPAN_MARGIN_SPN";
            this.SPAN_MARGIN_SPN.Visible = true;
            this.SPAN_MARGIN_SPN.VisibleIndex = 0;
            // 
            // SPAN_MARGIN_SPN_PATH
            // 
            this.SPAN_MARGIN_SPN_PATH.Caption = "參數檔案路徑";
            this.SPAN_MARGIN_SPN_PATH.FieldName = "SPAN_MARGIN_SPN_PATH";
            this.SPAN_MARGIN_SPN_PATH.Name = "SPAN_MARGIN_SPN_PATH";
            this.SPAN_MARGIN_SPN_PATH.Visible = true;
            this.SPAN_MARGIN_SPN_PATH.VisibleIndex = 1;
            // 
            // SPAN_MARGIN_POS
            // 
            this.SPAN_MARGIN_POS.Caption = "部位檔案";
            this.SPAN_MARGIN_POS.FieldName = "SPAN_MARGIN_POS";
            this.SPAN_MARGIN_POS.Name = "SPAN_MARGIN_POS";
            this.SPAN_MARGIN_POS.Visible = true;
            this.SPAN_MARGIN_POS.VisibleIndex = 2;
            // 
            // SPAN_MARGIN_RATIO
            // 
            this.SPAN_MARGIN_RATIO.Caption = "結構比";
            this.SPAN_MARGIN_RATIO.FieldName = "SPAN_MARGIN_RATIO";
            this.SPAN_MARGIN_RATIO.Name = "SPAN_MARGIN_RATIO";
            this.SPAN_MARGIN_RATIO.Visible = true;
            this.SPAN_MARGIN_RATIO.VisibleIndex = 3;
            // 
            // SPAN_MARGIN_USER_ID
            // 
            this.SPAN_MARGIN_USER_ID.Caption = "SPAN_MARGIN_USER_ID";
            this.SPAN_MARGIN_USER_ID.FieldName = "SPAN_MARGIN_USER_ID";
            this.SPAN_MARGIN_USER_ID.Name = "SPAN_MARGIN_USER_ID";
            // 
            // SPAN_MARGIN_W_TIME
            // 
            this.SPAN_MARGIN_W_TIME.Caption = "SPAN_MARGIN_W_TIME";
            this.SPAN_MARGIN_W_TIME.FieldName = "SPAN_MARGIN_W_TIME";
            this.SPAN_MARGIN_W_TIME.Name = "SPAN_MARGIN_W_TIME";
            // 
            // WS0073
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 685);
            this.Name = "WS0073";
            this.Text = "WS0073";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Widget.TextDateEdit txtEndDate;
        private Widget.TextDateEdit txtStartDate;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label labDate;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labSpanDesc;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_MARGIN_SPN_PATH;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_MARGIN_SPN;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_MARGIN_POS;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_MARGIN_RATIO;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_MARGIN_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn SPAN_MARGIN_W_TIME;
    }
}