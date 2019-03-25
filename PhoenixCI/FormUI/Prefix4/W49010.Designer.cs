namespace PhoenixCI.FormUI.Prefix4
{
   partial class W49010
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
         this.label1 = new System.Windows.Forms.Label();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.BackColor = System.Drawing.Color.MintCream;
         this.panParent.Size = new System.Drawing.Size(806, 500);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(806, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panelControl1.Appearance.Options.UseBackColor = true;
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(806, 65);
         this.panelControl1.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.ForeColor = System.Drawing.Color.Red;
         this.label1.Location = new System.Drawing.Point(25, 25);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(697, 20);
         this.label1.TabIndex = 0;
         this.label1.Text = "備    註：已下市契約之最小風險價格係數一律為空白；有效契約之最小風險價格係數不可為空白。";
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(0, 95);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(806, 435);
         this.gcMain.TabIndex = 1;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         // 
         // W49010
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(806, 530);
         this.Controls.Add(this.gcMain);
         this.Controls.Add(this.panelControl1);
         this.Name = "W49010";
         this.Text = "W49010";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.gcMain, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private System.Windows.Forms.Label label1;
   }
}