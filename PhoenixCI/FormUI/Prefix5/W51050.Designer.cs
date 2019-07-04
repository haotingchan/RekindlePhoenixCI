namespace PhoenixCI.FormUI.Prefix5 {
   partial class W51050 {
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
         this.components = new System.ComponentModel.Container();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.MMFO_MARKET_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MMFO_PARAM_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MMFO_MIN_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MMFO_MIN_PRICETextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.IS_NEWROW = new DevExpress.XtraGrid.Columns.GridColumn();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
         this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.MMFO_MIN_PRICETextEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.gcMain);
         this.panParent.Size = new System.Drawing.Size(673, 455);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(673, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // gvMain
         // 
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.MMFO_MARKET_CODE,
            this.MMFO_PARAM_KEY,
            this.MMFO_MIN_PRICE,
            this.IS_NEWROW});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsCustomization.AllowSort = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
         this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
         // 
         // MMFO_MARKET_CODE
         // 
         this.MMFO_MARKET_CODE.AppearanceCell.BackColor = System.Drawing.Color.White;
         this.MMFO_MARKET_CODE.AppearanceCell.Options.UseBackColor = true;
         this.MMFO_MARKET_CODE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
         this.MMFO_MARKET_CODE.AppearanceHeader.Options.UseBackColor = true;
         this.MMFO_MARKET_CODE.Caption = "交易時段";
         this.MMFO_MARKET_CODE.FieldName = "MMFO_MARKET_CODE";
         this.MMFO_MARKET_CODE.Name = "MMFO_MARKET_CODE";
         this.MMFO_MARKET_CODE.Visible = true;
         this.MMFO_MARKET_CODE.VisibleIndex = 0;
         // 
         // MMFO_PARAM_KEY
         // 
         this.MMFO_PARAM_KEY.AppearanceCell.BackColor = System.Drawing.Color.White;
         this.MMFO_PARAM_KEY.AppearanceCell.Options.UseBackColor = true;
         this.MMFO_PARAM_KEY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
         this.MMFO_PARAM_KEY.AppearanceHeader.Options.UseBackColor = true;
         this.MMFO_PARAM_KEY.Caption = "商品別";
         this.MMFO_PARAM_KEY.FieldName = "MMFO_PARAM_KEY";
         this.MMFO_PARAM_KEY.Name = "MMFO_PARAM_KEY";
         this.MMFO_PARAM_KEY.Visible = true;
         this.MMFO_PARAM_KEY.VisibleIndex = 1;
         // 
         // MMFO_MIN_PRICE
         // 
         this.MMFO_MIN_PRICE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MMFO_MIN_PRICE.AppearanceHeader.Options.UseBackColor = true;
         this.MMFO_MIN_PRICE.Caption = "委託價格限制";
         this.MMFO_MIN_PRICE.ColumnEdit = this.MMFO_MIN_PRICETextEdit1;
         this.MMFO_MIN_PRICE.FieldName = "MMFO_MIN_PRICE";
         this.MMFO_MIN_PRICE.Name = "MMFO_MIN_PRICE";
         this.MMFO_MIN_PRICE.Visible = true;
         this.MMFO_MIN_PRICE.VisibleIndex = 2;
         // 
         // MMFO_MIN_PRICETextEdit1
         // 
         this.MMFO_MIN_PRICETextEdit1.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
         this.MMFO_MIN_PRICETextEdit1.AutoHeight = false;
         this.MMFO_MIN_PRICETextEdit1.ContextImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
         this.MMFO_MIN_PRICETextEdit1.DisplayFormat.FormatString = "##########0.####";
         this.MMFO_MIN_PRICETextEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.MMFO_MIN_PRICETextEdit1.Name = "MMFO_MIN_PRICETextEdit1";
         // 
         // IS_NEWROW
         // 
         this.IS_NEWROW.Caption = "Is_NewRow";
         this.IS_NEWROW.FieldName = "IS_NEWROW";
         this.IS_NEWROW.Name = "IS_NEWROW";
         // 
         // gcMain
         // 
         this.gcMain.Location = new System.Drawing.Point(12, 12);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.MMFO_MIN_PRICETextEdit1});
         this.gcMain.Size = new System.Drawing.Size(649, 431);
         this.gcMain.TabIndex = 0;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // repositoryItemComboBox1
         // 
         this.repositoryItemComboBox1.AutoHeight = false;
         this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
         // 
         // W51050
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(673, 485);
         this.Name = "W51050";
         this.Text = "W51050";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.MMFO_MIN_PRICETextEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private DevExpress.XtraGrid.Columns.GridColumn MMFO_MARKET_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn MMFO_PARAM_KEY;
      private DevExpress.XtraGrid.Columns.GridColumn MMFO_MIN_PRICE;
      private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
      private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
      private DevExpress.XtraGrid.Columns.GridColumn IS_NEWROW;
      private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit MMFO_MIN_PRICETextEdit1;
   }
}