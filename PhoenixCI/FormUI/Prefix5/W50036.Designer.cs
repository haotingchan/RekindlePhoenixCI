namespace PhoenixCI.FormUI.Prefix5
{
   partial class W50036
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
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         DevExpress.XtraPrinting.Caching.MemoryDocumentStorage memoryDocumentStorage1 = new DevExpress.XtraPrinting.Caching.MemoryDocumentStorage();
         this.sidePanel1 = new DevExpress.XtraEditors.SidePanel();
         this.sidePanel3 = new DevExpress.XtraEditors.SidePanel();
         this.documentViewer1 = new DevExpress.XtraPrinting.Preview.DocumentViewer();
         this.sidePanel2 = new DevExpress.XtraEditors.SidePanel();
         this.w500xx = new BaseGround.Widget.ucW500xx();
         this.cachedReportSource1 = new DevExpress.XtraPrinting.Caching.CachedReportSource(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.sidePanel1.SuspendLayout();
         this.sidePanel3.SuspendLayout();
         this.sidePanel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(740, 516);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(740, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // sidePanel1
         // 
         this.sidePanel1.BorderThickness = 0;
         this.sidePanel1.Controls.Add(this.sidePanel3);
         this.sidePanel1.Controls.Add(this.sidePanel2);
         this.sidePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.sidePanel1.Location = new System.Drawing.Point(0, 30);
         this.sidePanel1.Name = "sidePanel1";
         this.sidePanel1.Size = new System.Drawing.Size(740, 516);
         this.sidePanel1.TabIndex = 0;
         this.sidePanel1.Text = "sidePanel1";
         // 
         // sidePanel3
         // 
         this.sidePanel3.BorderThickness = 0;
         this.sidePanel3.Controls.Add(this.documentViewer1);
         this.sidePanel3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.sidePanel3.Location = new System.Drawing.Point(0, 365);
         this.sidePanel3.Name = "sidePanel3";
         this.sidePanel3.Size = new System.Drawing.Size(740, 151);
         this.sidePanel3.TabIndex = 1;
         this.sidePanel3.Text = "sidePanel3";
         // 
         // documentViewer1
         // 
         this.documentViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.documentViewer1.DocumentSource = this.cachedReportSource1;
         this.documentViewer1.IsMetric = true;
         this.documentViewer1.Location = new System.Drawing.Point(0, 0);
         this.documentViewer1.Margin = new System.Windows.Forms.Padding(0);
         this.documentViewer1.Name = "documentViewer1";
         this.documentViewer1.ShowPageMargins = false;
         this.documentViewer1.Size = new System.Drawing.Size(740, 151);
         this.documentViewer1.TabIndex = 0;
         // 
         // sidePanel2
         // 
         this.sidePanel2.BorderThickness = 0;
         this.sidePanel2.Controls.Add(this.w500xx);
         this.sidePanel2.Dock = System.Windows.Forms.DockStyle.Top;
         this.sidePanel2.Location = new System.Drawing.Point(0, 0);
         this.sidePanel2.Name = "sidePanel2";
         this.sidePanel2.Size = new System.Drawing.Size(740, 365);
         this.sidePanel2.TabIndex = 0;
         this.sidePanel2.Text = "sidePanel2";
         // 
         // w500xx
         // 
         this.w500xx.Dock = System.Windows.Forms.DockStyle.Fill;
         this.w500xx.IsOpen = false;
         this.w500xx.IsCheck = null;
         this.w500xx.DataType = null;
         this.w500xx.Ebrkno = null;
         this.w500xx.Edate = null;
         this.w500xx.Filename = null;
         this.w500xx.LogText = null;
         this.w500xx.ProdCategory = null;
         this.w500xx.ProdKindId = null;
         this.w500xx.ProdKindIdSto = null;
         this.w500xx.Sbrkno = null;
         this.w500xx.Sdate = null;
         this.w500xx.SortType = null;
         this.w500xx.SumSubType = null;
         this.w500xx.SumType = null;
         this.w500xx.TableName = null;
         this.w500xx.TimeNow = null;
         this.w500xx.TxnID = null;
         this.w500xx.Location = new System.Drawing.Point(0, 0);
         this.w500xx.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.w500xx.Name = "w500xx";
         this.w500xx.OCF = null;
         this.w500xx.Size = new System.Drawing.Size(740, 365);
         this.w500xx.TabIndex = 0;
         // 
         // cachedReportSource1
         // 
         this.cachedReportSource1.Report = typeof(BaseGround.Report.defReport);
         this.cachedReportSource1.Storage = memoryDocumentStorage1;
         // 
         // W50036
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(740, 546);
         this.Controls.Add(this.sidePanel1);
         this.Name = "W50036";
         this.Text = "W50036";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.sidePanel1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.sidePanel1.ResumeLayout(false);
         this.sidePanel3.ResumeLayout(false);
         this.sidePanel2.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.SidePanel sidePanel1;
      private DevExpress.XtraEditors.SidePanel sidePanel3;
      private DevExpress.XtraEditors.SidePanel sidePanel2;
      private BaseGround.Widget.ucW500xx w500xx;
      private DevExpress.XtraPrinting.Preview.DocumentViewer documentViewer1;
      private DevExpress.XtraPrinting.Caching.CachedReportSource cachedReportSource1;
   }
}