using BaseGround;
using Common;
using DataObjects.Dao.Together;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoenixCI.Shared
{
   public static class ReportExportFunc
   {
      #region 全域變數
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

      static string gs_user_id, gs_dpt_id, gs_user_name, gs_dpt_name;//使用者

      /*******************************
      執行作業
      *******************************/
      //現正執行之作業代號
      static string gs_txn_id;
      static string gs_txn_name;
      const int gi_txn_id_pos = 2;
      const int gi_txn_id_len = 5;
      const int gi_txn_name_pos = 9;
      const int gi_txn_name_len = 40;


      static string gs_ap_path;//AP路徑(application path)
      static string gs_bmp_path;
      static string gs_bmp_folder;
      static string gs_work_path;//本磁碟機
      static string gs_excel_path { get { return GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH; } }//Excel路徑
      static string gs_savereport_path { get { return GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH; } }//報表儲存路徑
      static string gs_bcp_path;//載入Data程式位置
      static string gs_batch_path;//Run Batch的路徑

      static string gs_screen_type;//Enviroment 
      static DateTime gdt_ocf_date;//Enviroment 
      #endregion

      public static string wf_GetFileSaveName(string as_filename)
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
      /// <summary>
      /// 搜尋指定的副檔名並設定儲存路徑
      /// </summary>
      /// <param name="is_txn_id"></param>
      /// <param name="as_filename"></param>
      /// <returns></returns>
      public static string wf_copyfile(string is_txn_id, string as_filename)
      {
         string ls_excel_name, ls_excel_ext, ls_excel_path, ls_sub_path, ls_rename;
         ls_excel_path = gs_excel_path;
         ls_excel_name = as_filename;
         ls_excel_ext = ".xls";
         //讀取ci.RPTX設定檔
         DataTable dtRPTX = new RPTX().ListByTxn(is_txn_id, as_filename);
         ls_sub_path = dtRPTX.Rows[0]["RPTX_SUBPATH"].AsString();
         ls_excel_ext = dtRPTX.Rows[0]["RPTX_FILENAME_EXT"].AsString();
         ls_rename = dtRPTX.Rows[0]["RPTX_RENAME"].AsString();
         if (ls_sub_path != ".") {
            ls_excel_path = ls_excel_path + ls_sub_path;
         }
         if (!File.Exists(Path.Combine(ls_excel_path, ls_excel_name+ls_excel_ext))) {
            MessageDisplay.Error("無此檔案「" + Path.Combine(ls_excel_path, ls_excel_name + ls_excel_ext) + "」!");
         }

         string originalFilePath = Path.Combine(gs_excel_path, as_filename + ls_excel_ext);

         string destinationFilePath = Path.Combine(gs_savereport_path,as_filename + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + ls_excel_ext);

         File.Copy(originalFilePath, destinationFilePath, true);
         if (!File.Exists(destinationFilePath)) {
            MessageDisplay.Error("複製「" + ls_excel_path + ls_excel_name + ls_excel_ext + "」到「" + destinationFilePath + "」檔案錯誤!");
         }
         return destinationFilePath;
      }

   }
}
