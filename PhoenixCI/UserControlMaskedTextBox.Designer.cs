namespace PhoenixCI
{
    partial class UserControlMaskedTextBox
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.MaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.SuspendLayout();
            // 
            // MaskedTextBox
            // 
            this.MaskedTextBox.Font = new System.Drawing.Font("微軟正黑體", 12.18462F);
            this.MaskedTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MaskedTextBox.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.MaskedTextBox.Location = new System.Drawing.Point(4, 4);
            this.MaskedTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.MaskedTextBox.Mask = "0000/00/00";
            this.MaskedTextBox.Name = "MaskedTextBox";
            this.MaskedTextBox.PromptChar = ' ';
            this.MaskedTextBox.Size = new System.Drawing.Size(115, 37);
            this.MaskedTextBox.TabIndex = 0;
            // 
            // UserControlMaskedTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MaskedTextBox);
            this.Name = "UserControlMaskedTextBox";
            this.Size = new System.Drawing.Size(161, 47);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox MaskedTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
