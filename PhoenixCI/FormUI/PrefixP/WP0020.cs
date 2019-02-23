using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using Common;
using BusinessObjects.Enums;

//TODO :資料尚未齊全，先做UI畫面

/// <summary>
/// Winni, 2019/01/05 查詢資料明細
/// </summary>
namespace PhoenixCI.FormUI.PrefixP {
   public partial class WP0020 : FormParent {
      public WP0020(string programID , string programName) : base(programID , programName) {
         try {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
         } catch (Exception ex) {
            MessageDisplay.Error(ex.ToString());
         }
      }

      /// <summary>
      /// 視窗啟動時,設定一些UI元件初始值
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Open() {
         base.Open();

//integer li_y
//li_y = 116
//dw_1.move(0 , 244 + li_y)
//if    gs_screen_type = '1' then
////800 * 600
//dw_1.height = 1662 - li_y

//dw_1.width = 3584
//else
////1024 * 768
//dw_1.height = 2315 - li_y

//dw_1.width = 4594
//end   if

         return ResultStatus.Success;
      }

      /// <summary>
      /// 視窗啟動後
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus AfterOpen() {
//its_db = create transaction
////中華電信
//its_db = f_get_exec_oth(its_db , "POS")
//if    not isvalid(its_db) then

//return
//end   if
         return ResultStatus.Success;
      }

      /// <summary>
      /// 設定此功能哪些按鈕可以按
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

         return ResultStatus.Success;
      }

      /// <summary>
      /// 按下[讀取/預覽]按鈕時,去資料庫撈資料
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Retrieve() {
         base.Retrieve();

////Retrieve
//is_arg = "查詢條件：交易日期："
//if    sle_fm.text = '%' and sle_to.text = '%' then
//is_arg = is_arg + '全部'
//else

//is_arg = is_arg + sle_fm.text + "-" + sle_to.text
//end   if
//is_arg = is_arg + " ,查詢系統別：" + mid(trim(ddlb_1.text) , 3 , 90) + " ,審查結果：" + mid(trim(ddlb_2.text) , 3 , 90) + " ,查詢類別：" + mid(trim(ddlb_3.text) , 3 , 90)

//string ls_type
//ls_type = left(ddlb_3.text , 1)
//if    ls_type = 'F' then

//dw_1.dataobject = "d_" + is_txn_id + "_f"
//else
//dw_1.dataobject = "d_" + is_txn_id + "_i"
//end   if
//dw_1.settransobject(its_db)
//dw_1.retrieve(sle_fm.text , sle_to.text , left(ddlb_1.text , 1) , left(ddlb_2.text , 1) , left(ddlb_3.text , 1))

         return ResultStatus.Success;
      }
   }
}