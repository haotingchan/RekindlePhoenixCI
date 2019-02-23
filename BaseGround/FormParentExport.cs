using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraBars;
using Log;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BaseGround
{
   /// <summary>
   /// 實作PB的form_parent_export 初始化順序為init-->Activated-->Load
   /// </summary>
   public partial class FormParentExport : DevExpress.XtraBars.Ribbon.RibbonForm
   {
      #region 全域變數
      private string _DefaultFileNamePath;
      private string _ReportID;
      private string _ReportTitle;
      private string _ReportClass;
      private DataTable ids_1;
      private object _PrintableComponent;

      protected BarButtonItem _ToolBtnInsert;
      protected BarButtonItem _ToolBtnSave;
      protected BarButtonItem _ToolBtnDel;
      protected BarButtonItem _ToolBtnRetrieve;
      protected BarButtonItem _ToolBtnRun;
      protected BarButtonItem _ToolBtnImport;
      protected BarButtonItem _ToolBtnExport;
      protected BarButtonItem _ToolBtnPrintAll;

      string is_save_file;//準備輸出excel的完整路徑

      private UserProgInfo _UserProgInfo;

      /// <summary>
      /// 紀錄著目前登錄的使用者和目前使用程式的代碼等資訊
      /// </summary>
      public UserProgInfo UserProgInfo {
         get {
            if (_UserProgInfo == null) {
               _UserProgInfo = new UserProgInfo();
               _UserProgInfo.UserID = GlobalInfo.USER_ID;
               _UserProgInfo.TxnID = gs_txn_id;
            }

            return _UserProgInfo;
         }
      }

      /// <summary>
      /// 將顯示元件(像是Grid)設定用來列印或匯出用
      /// </summary>
      public object PrintableComponent {
         get {
            return _PrintableComponent;
         }

         set {
            _PrintableComponent = value;
         }
      }
      #endregion

      #region 下面是PB定義的全域變數
      const string gs_sys = "CI";
      const string gs_t_result = "處理結果";     //Information!
      const string gs_t_warning = "警告訊息";        //Exclamation!
      const string gs_t_err = "錯誤訊息";    //gs_t_err,MessageBoxButtons.OK,MessageBoxIcon.Stop
      const string gs_t_question = "請選擇";            //Question!
      const string gs_m_ok = "處理完成";
      const string gs_m_no_data = "無此筆資料!";
      const string gs_m_field_err = "欄位資料輸入錯誤!";
      const string gs_m_no_auth = "無此權限，執行此交易";
      const string gs_m_not_allow_exec = "時點不允許執行此交易,視窗即將關閉.";

      const int gi_w_height = 2650;
      const int gi_w_width = 4630;
      const int gi_sub_dw_height = 0;
      const int gi_sub_dw_width = 0;

      /*******************************
      使用者
      *******************************/
      string gs_user_id, gs_dpt_id, gs_user_name, gs_dpt_name;

      /*******************************
      執行作業
      *******************************/
      //現正執行之作業代號
      string gs_txn_id;
      string gs_txn_name;
      const int gi_txn_id_pos = 2;
      const int gi_txn_id_len = 5;
      const int gi_txn_name_pos = 9;
      const int gi_txn_name_len = 40;

      /*******************************
      Path 
      *******************************/
      //AP路徑(application path)
      string gs_ap_path;
      string gs_bmp_path;
      string gs_bmp_folder;
      string gs_work_path;//本磁碟機
      string gs_Excel_path;//Excel路徑
      string gs_SaveReport_path;//報表儲存路徑
      string gs_bcp_path;//載入Data程式位置
      string gs_batch_path;//Run Batch的路徑

      /*******************************
      Enviroment 
      *******************************/
      string gs_screen_type;
      DateTime gdt_ocf_date;
      #endregion


      public FormParentExport()
      {

      }

      /// <summary>
      /// 設定
      /// </summary>
      /// <param name="program_id"></param>
      /// <param name="program_name"></param>
      public FormParentExport(string program_id, string program_name)
      {
         InitializeComponent();

         PaintFormBorder();

         gs_txn_id = program_id;
         gs_txn_name = program_name;
         _ReportClass = "R" + gs_txn_id;

         _DefaultFileNamePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, "CI_" + gs_txn_id + "─" + gs_txn_name + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
         _ReportID = GlobalInfo.SYSTEM_ALIAS + gs_txn_id;
         _ReportTitle = gs_txn_id + "─" + gs_txn_name + GlobalInfo.REPORT_TITLE_MEMO;

         if (MdiParent != null) {
            _ToolBtnInsert = ((FormMain)MdiParent).toolStripButtonInsert;
            _ToolBtnDel = ((FormMain)MdiParent).toolStripButtonDelete;
            _ToolBtnSave = ((FormMain)MdiParent).toolStripButtonSave;
            _ToolBtnRetrieve = ((FormMain)MdiParent).toolStripButtonRetrieve;
            _ToolBtnRun = ((FormMain)MdiParent).toolStripButtonRun;
            _ToolBtnImport = ((FormMain)MdiParent).toolStripButtonImport;
            _ToolBtnExport = ((FormMain)MdiParent).toolStripButtonExport;
            _ToolBtnPrintAll = ((FormMain)MdiParent).toolStripButtonPrintAll;
         }

      }

      /// <summary>
      /// 等於PB的Activate,主要設定要開啟那些功能
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void FormParentExport_Activated(object sender, EventArgs e)
      {

         SingletonLogger.Instance.Info(GlobalInfo.USER_ID, gs_txn_id, "OPEN", " ");

         SetAllToolBtnDisable(); //PbFunc.f_menu_file_disable_all();
         _ToolBtnExport.Enabled = true;//m_frame.m_file.m_export.enabled = true;

      }

      /// <summary>
      /// 等於PB的Open
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void FormParentExport_Load(object sender, EventArgs e)
      {
         this.Text = "[" + gs_txn_id + "] " + gs_txn_name;

         if (!BeforeOpen()) {
            Close();
            return;
         }

         Open();

         AfterOpen();
      }

      /// <summary>
      /// 在open之前檢查
      /// </summary>
      /// <returns>正常回傳true,如果發生錯誤請回傳false</returns>
      protected virtual bool BeforeOpen()
      {
         if (!PbFunc.f_chk_run_timing(gs_txn_id)) {
            MessageBox.Show("今日盤後轉檔作業還未完畢!");
            return false;
         }
         return true;
      }

      /// <summary>
      /// PB在這邊設定dataWindow的連線
      /// </summary>
      protected virtual void Open()
      {
         //Input Condition
         em_date.Text = PbFunc.f_ocf_date(0);
         em_month.Text = em_date.Text.Substring(0, 6);
         DateTime.TryParse(em_date.Text, out gdt_ocf_date);

         st_msg_txt.Visible = false;
      }

      /// <summary>
      /// PB在這邊設定dataWindow的連線
      /// </summary>
      protected virtual void AfterOpen()
      {
         //nothing
      }




      #region Retrieve相關事件

      /// <summary>
      /// 從母視窗的Retrieve按鈕觸發此事件
      /// </summary>
      public virtual void ProcessRetrieve()
      {
         if (!BeforeRetrieve()) return;

         Retrieve();

         AfterRetrieve();
      }

      /// <summary>
      /// 在Retrieve之前檢查
      /// </summary>
      /// <returns>正常回傳true,如果發生錯誤請回傳false</returns>
      protected virtual bool BeforeRetrieve()
      {
         //nothing
         return true;
      }

      protected virtual void Retrieve()
      {
         //nothing
      }

      protected virtual void AfterRetrieve()
      {
         //nothing
      }
      #endregion

      #region Export相關事件

      /// <summary>
      /// 從母視窗的Export按鈕觸發此事件
      /// </summary>
      public virtual void ProcessExport()
      {
         string startTime = DateTime.Now.ToString("HH:mm:ss");

         if (!BeforeExport()) return;

         Export();

         AfterExport(startTime);
      }

      /// <summary>
      /// 在Export之前檢查
      /// </summary>
      /// <returns>正常回傳true,如果發生錯誤請回傳false</returns>
      protected virtual bool BeforeExport()
      {
         //條件值檢核

         DateTime.TryParse(em_date.Text, out DateTime inputDate);
         if (inputDate == null) {
            MessageBox.Show("日期輸入錯誤!");
            em_date.Focus();
            return false;
         }

         DateTime.TryParse(em_month.Text + "/01", out DateTime inputMonth);
         if (inputMonth == null) {
            MessageBox.Show("月份輸入錯誤!");
            em_month.Focus();
            return false;
         }

         //TODO:請點選儲存檔案之目錄
         //檔名	= 報表型態(起-迄).xls
         is_save_file = wf_GetFileSaveName(em_month.Text + "_MMONTH.txt");
         if (is_save_file == "") {
            return false;
         }


         //show msg
         st_msg_txt.Visible = true;
         st_msg_txt.Text = "開始轉檔...";
         //SetPointer(HourGlass!);//TODO:ken,設定滑鼠指標為漏斗(忙碌)

         return true;
      }

      protected virtual void Export()
      {
         //nothing
      }

      protected virtual void AfterExport(string startTime)
      {
         //TODO:檢查輸出筆數為0則不轉出
         //if (ids_1.rowcount() == 0) {
         //    messagebox(gs_t_err, "轉出筆數為０!", StopSign!);
         //    return;
         //}

         //TODO: export to excel
         //ids_1.SaveAs(is_save_file, Excel!, ib_title);

         //Write LOGF
         if (PbFunc.f_write_logf(gs_txn_id, "E", "轉出檔案:" + is_save_file) < 0) {
            return;
         }

         string finishTime = DateTime.Now.ToString("HH:mm:ss");
         MessageDisplay.Info("轉檔完成!", gs_t_result + " " + startTime + " ～ " + finishTime);
      }
      #endregion

      #region Print相關事件

      /// <summary>
      /// 從母視窗的Retrieve按鈕觸發此事件
      /// </summary>
      public virtual void ProcessPrint()
      {
         if (!BeforePrint()) return;

         Print();

         AfterPrint();
      }

      /// <summary>
      /// 在Print之前檢查
      /// </summary>
      /// <returns>正常回傳true,如果發生錯誤請回傳false</returns>
      protected virtual bool BeforePrint()
      {
         //nothing
         return true;
      }

      /// <summary>
      /// 列印
      /// </summary>
      protected virtual void Print()
      {
         ReportHelper reportHelper = new ReportHelper(PrintableComponent, _ReportID, _ReportTitle);
         reportHelper.FilePath = _DefaultFileNamePath;
         reportHelper.FileType = FileType.PDF;
         reportHelper.IsPrintedFromPrintButton = true;
         reportHelper.Print();
      }

      protected virtual void AfterPrint()
      {
         MessageDisplay.Info(MessageDisplay.MSG_PRINT);
      }
      #endregion



      private void FormParentExport_SizeChanged(object sender, EventArgs e)
      {
         if (MdiParent != null) {
            if (this.WindowState == FormWindowState.Maximized) {
               ((FormMain)MdiParent).standaloneBarDockControlMdi.Visible = true;
            }
            else {
               ((FormMain)MdiParent).standaloneBarDockControlMdi.Visible = false;
            }
         }
      }

      protected virtual void SetAllToolBtnDisable()
      {
         _ToolBtnInsert.Enabled = false;
         _ToolBtnDel.Enabled = false;
         _ToolBtnSave.Enabled = false;
         _ToolBtnRetrieve.Enabled = false;
         _ToolBtnRun.Enabled = false;
         _ToolBtnImport.Enabled = false;
         _ToolBtnExport.Enabled = false;
         _ToolBtnPrintAll.Enabled = false;
      }

      protected virtual void PaintFormBorder()
      {
         SkinElement element = SkinManager.GetSkinElement(SkinProductId.Ribbon, DevExpress.LookAndFeel.UserLookAndFeel.Default, "FormCaptionNoRibbon");
         Image image = element.Image.GetImages().Images[1];
         int counter = element.Image.ImageCount;
         Bitmap bmp = new Bitmap(image.Width, image.Height * 2);

         using (Graphics graphics = Graphics.FromImage(bmp)) {
            int y = 0;
            while (counter-- > 0) {
               graphics.DrawImage(image, new Rectangle(0, y, image.Width, image.Height));
               graphics.DrawRectangle(Pens.DarkSlateGray, 0, y, image.Width - 1, image.Height);
               y += image.Height;
            }
         }
         element.SetActualImage(bmp, true);
         LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
      }

      /// <summary>
      /// Copy Excel Template File to report path
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="fileType"></param>
      /// <returns>Destination File Path</returns>
      public virtual string CopyExcelTemplateFile(string fileName, FileType fileType)
      {
         string originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH, fileName + "." + fileType.ToString().ToLower());

         string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
             fileName + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + "." + fileType.ToString().ToLower());

         File.Copy(originalFilePath, destinationFilePath, true);

         return destinationFilePath;
      }

      /// <summary>
      /// 彈出選擇存檔的系統視窗
      /// </summary>
      /// <param name="as_filename">檔名</param>
      /// <returns></returns>
      public virtual string wf_GetFileSaveName(string as_filename)
      {

         if (!Directory.Exists(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH)) {
            Directory.CreateDirectory(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH);
         }

         SaveFileDialog dialog = new SaveFileDialog();
         dialog.Title = "請點選儲存檔案之目錄";
         dialog.InitialDirectory = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH;
         dialog.FileName = as_filename;
         dialog.Filter = "Excel(*.*)|*.xls;*.xlsx";
         if (dialog.ShowDialog() == DialogResult.OK) {
            return dialog.FileName;
         }
         return "";
      }


   }
}