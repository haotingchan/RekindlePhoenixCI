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
using DataObjects.Dao.Together.SpecificDao;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;

/// <summary>
/// Winni, 2019/03/06 
/// </summary>
namespace PhoenixCI.FormUI.PrefixP {
   /// <summary>
   /// P0020 查詢資料明細
   /// 功能：Retrieve, Print
   /// </summary>
   public partial class WP0020 : FormParent {

      private DP0020 daoP0020;
      private DP0030 daoP0030; //test
      private DP0050 daoP0050; //test
      protected class LookupItem {
         public string ValueMember { get; set; }
         public string DisplayMember { get; set; }
      }

      public WP0020(string programID , string programName) : base(programID , programName) {
         try {
            InitializeComponent();
            daoP0020 = new DP0020();
            daoP0030 = new DP0030(); //test
            daoP0050 = new DP0050(); //test

            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);

            txtStartDate.Text = "%";
            txtEndDate.Text = "%";

            //下拉選單(系統別)
            List<LookupItem> ddlbGrp1 = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "W", DisplayMember = "W：網際網路"},
                                        new LookupItem() { ValueMember = "V", DisplayMember = "V：語音查詢" }};
            Extension.SetDataTable(ddlbType , ddlbGrp1 , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor);

            //下拉選單(審查結果)
            List<LookupItem> ddlbGrp2 = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "S", DisplayMember = "S：審核成功"},
                                        new LookupItem() { ValueMember = "F", DisplayMember = "F：審核失敗"},
                                        new LookupItem() { ValueMember = "A", DisplayMember = "A：全部" }};
            Extension.SetDataTable(ddlbItem , ddlbGrp2 , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor);

            //下拉選單(類別)
            List<LookupItem> ddlbGrp3 = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "I", DisplayMember = "I：依交易人查明細"},
                                        new LookupItem() { ValueMember = "F", DisplayMember = "F：依期貨商合計" }};
            Extension.SetDataTable(ddlbCate , ddlbGrp3 , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor);

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      /// <summary>
      /// 視窗啟動時,設定一些UI元件初始值
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Open() {
         base.Open();
         return ResultStatus.Success;
      }

      /// <summary>
      /// 視窗啟動後
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus AfterOpen() {
         //Db db = GlobalDaoSetting.DB;
         //DbConnection dc = PbFunc.f_get_exec_oth(db.CreateConnection() , "POS"); //中華電信
         //if (dc.State == ConnectionState.Open) {
         //   return ResultStatus.Success;
         //}
         //return ResultStatus.Fail;
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

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      /// <summary>
      /// 按下[讀取/預覽]按鈕時,去資料庫撈資料
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Retrieve() {
         base.Retrieve();
         DataTable dtContent = new DataTable();
         string type = ddlbType.EditValue.AsString();
         string item = ddlbItem.EditValue.AsString();
         string cate = ddlbCate.EditValue.AsString();
         dtContent = daoP0020.ExecuteStoredProcedure(txtStartDate.Text , txtEndDate.Text , type , item , cate);
         DataTable dtContent2 = new DataTable();
         dtContent2 = daoP0030.ExecuteStoredProcedure(txtStartDate.Text , txtEndDate.Text , type , cate);
         DataTable dtContent3 = new DataTable();
         dtContent3 = daoP0050.ExecuteStoredProcedure(txtStartDate.Text , txtEndDate.Text);

         //這段不知用在哪，先照翻
         string isArg = "查詢條件：交易日期：";
         if (txtStartDate.Text == "%" && txtEndDate.Text == "%") {
            isArg += "全部";
         } else {
            isArg += txtStartDate.Text + "-" + txtEndDate.Text;
         }
         isArg += " ,查詢系統別：" + ddlbType.Text + " ,審查結果：" + ddlbItem.Text + " ,查詢類別：" + ddlbCate.Text;

         string searchType = ddlbCate.Text.Substring(0 , 1);
         if (searchType == "F") {
            //grid長的會有點不一樣
         } else { //searchType == "I"

            DataTable dtTmp = dtContent.Clone(); //先將dtContent複製結構給dtTmp
            DataTable dtName = dtContent.DefaultView.ToTable(true, "FCM_NAME"); //依期貨商(FCM_NAME)判別
            //循環分組求和
            for(int w = 0 ; w < dtName.Rows.Count ; w++) {
               DataRow[] rows = dtContent.Select("FCM_NAME='" + dtName.Rows[w][0] + "'");
               DataTable dt = dtTmp.Clone();
               foreach (DataRow row in rows) {
                  dt.Rows.Add(row.ItemArray);
               }
               DataRow dr = dtTmp.NewRow();
               dr[3] = dtName.Rows[w][0].ToString();
               dr[4] = dt.Compute("Count(APPLYDATE)" , "");//compute 是一个函数用于计算，只有两个参数
               dtTmp.Rows.Add(dr);

            }


            gcMain.DataSource = dtTmp;
            gcMain.Visible = true;
            gcMain.Focus();
         }

         return ResultStatus.Success;
      }

   }
}