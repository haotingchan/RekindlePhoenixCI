using BusinessObjects;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Data;

namespace ActionService.Interfaces
{
    public interface IServiceCommon : IService
    {
        /// <summary>
        /// 判斷在JSW這個TABLE裡面有沒有權限
        /// </summary>
        bool HasJswPermission(string programID, string jswType);

        /// <summary>
        /// 判斷在JSW這個TABLE裡面有沒有權限
        /// </summary>
        bool HasJswPermission(string programID);

        /// <summary>
        /// 抓OCF這個TABLE存到物件裡
        /// </summary>
        BO_OCF GetOCF();

        /// <summary>
        /// 取得交易天數
        /// </summary>
        /// <returns></returns>
        int getAOCFDates(string symd,string eymd);

        /// <summary>
        /// 某個使用者有哪些程式的權限
        /// </summary>
        DataTable ListTxnByUser(string userID);

        /// <summary>
        /// 將某個程式代碼的JSW為Y的列都抓出來
        /// </summary>
        DataTable ListAllowJswPermission(string programID);

        /// <summary>
        /// 傳入一個DataTable，如果該列已存在DB會下UPDATE語法，如果不存在會下INSERT語法
        /// </summary>
        /// <param name="inputDT">傳入的資料</param>
        /// <param name="tableName">TABLE的名字</param>
        /// <param name="insertColumnList">要新增的欄位</param>
        /// <param name="updateColumnList">要更新的欄位</param>
        /// <param name="updateOrDeleteKeysColumnList">KEY的欄位</param>
        ResultData SaveForAll(DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList, PokeBall pokeBall);

        /// <summary>
        /// 傳入一個DataTable，如果該列已存在DB會下UPDATE語法，如果不存在會下INSERT語法
        /// </summary>
        /// <param name="connInfo">哪個DB的連線</param>
        /// <param name="inputDT">傳入的資料</param>
        /// <param name="tableName">TABLE的名字</param>
        /// <param name="insertColumnList">要新增的欄位</param>
        /// <param name="updateColumnList">要更新的欄位</param>
        /// <param name="updateOrDeleteKeysColumnList">KEY的欄位</param>
        ResultData SaveForAll(ConnectionInfo connInfo, DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList, PokeBall pokeBall);

        /// <summary>
        /// 根據DataTable的改變，會下不同的SQL去異動DB，並傳回各異動的資料集
        /// </summary>
        /// <param name="inputDT">傳入的資料</param>
        /// <param name="tableName">TABLE的名字</param>
        /// <param name="insertColumnList">要新增的欄位</param>
        /// <param name="updateColumnList">要更新的欄位</param>
        /// <param name="updateOrDeleteKeysColumnList">KEY的欄位</param>
        ResultData SaveForChanged(DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList, PokeBall pokeBall);

        /// <summary>
        /// 多個動作包成一個Transaction用
        /// </summary>
        /// <param name="inputFunc">傳入一個Function，所有的動作都放在裡面</param>
        ResultData MultiActionTransaction(Func<PokeBall, ResultStatus> inputFunc);

        /// <summary>
        /// 執行StoredProcedure用
        /// </summary>
        /// <param name="spName">SP的名字</param>
        /// <param name="dbParams">傳入的參數集合</param>
        /// <param name="hasReturnParameter">有沒有要接SP的Return值回來</param>
        ResultData ExecuteStoredProcedure(string spName, List<DbParameterEx> dbParams, bool hasReturnParameter);

        ResultData ExecuteStoredProcedure(ConnectionInfo connInfo, string spName, List<DbParameterEx> dbParams, bool hasReturnParameter);

        ResultData ExecuteInfoWorkFlow(string workFlowName, UserProgInfo userProgInfo);

        /// <summary>
        /// 使用者代號及姓名資料
        /// </summary>
        DataTable ListDataForUserIDAndUserName();

        /// <summary>
        /// 部門資料
        /// </summary>
        DataTable ListDPT();

        /// <summary>
        /// 報表
        /// </summary>
        DataTable ListRPT(string RPT_TXD_ID);

        /// <summary>
        /// 作業代號及名稱
        /// </summary>
        DataTable ListDataForTxnIdAndName();

        /// <summary>
        /// StoredProcedure的參數
        /// </summary>
        DataTable ListTXFPARM(string TXFPARM_SERVER, string TXFPARM_DB, string TXFPARM_TXN_ID, string TXFPARM_TID);
    }
}