﻿namespace PhoenixCI.FormUI.Prefix5
{
   partial class W50030
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
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.w500xx = new BaseGround.Widget.ucW500xx();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.documentViewer1 = new DevExpress.XtraPrinting.Preview.DocumentViewer();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         this.panelControl2.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(867, 644);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(867, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.w500xx);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(867, 345);
         this.panelControl1.TabIndex = 0;
         // 
         // w500xx
         // 
         this.w500xx.DataType = null;
         this.w500xx.Dock = System.Windows.Forms.DockStyle.Fill;
         this.w500xx.Ebrkno = null;
         this.w500xx.Edate = null;
         this.w500xx.Filename = null;
         this.w500xx.IsCheck = null;
         this.w500xx.IsOpen = false;
         this.w500xx.Location = new System.Drawing.Point(2, 2);
         this.w500xx.LogText = null;
         this.w500xx.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.w500xx.Name = "w500xx";
         this.w500xx.OCF = null;
         this.w500xx.ProdCategory = null;
         this.w500xx.ProdKindId = null;
         this.w500xx.ProdKindIdSto = null;
         this.w500xx.Sbrkno = null;
         this.w500xx.Sdate = null;
         this.w500xx.Size = new System.Drawing.Size(863, 341);
         this.w500xx.SortType = null;
         this.w500xx.SumSubType = null;
         this.w500xx.SumType = null;
         this.w500xx.TabIndex = 0;
         this.w500xx.TableName = null;
         this.w500xx.TimeNow = null;
         this.w500xx.TxnID = null;
         // 
         // panelControl2
         // 
         this.panelControl2.Controls.Add(this.documentViewer1);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 375);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(867, 299);
         this.panelControl2.TabIndex = 1;
         // 
         // documentViewer1
         // 
         this.documentViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.documentViewer1.IsMetric = true;
         this.documentViewer1.Location = new System.Drawing.Point(2, 2);
         this.documentViewer1.Name = "documentViewer1";
         this.documentViewer1.ShowPageMargins = false;
         this.documentViewer1.Size = new System.Drawing.Size(863, 295);
         this.documentViewer1.TabIndex = 0;
         // 
         // W50030
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(867, 674);
         this.Controls.Add(this.panelControl2);
         this.Controls.Add(this.panelControl1);
         this.Name = "W50030";
         this.Text = "W50030";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         this.panelControl2.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private BaseGround.Widget.ucW500xx w500xx;
        private DevExpress.XtraPrinting.Preview.DocumentViewer documentViewer1;
   }
}