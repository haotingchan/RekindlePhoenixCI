
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao;
using DataObjects.Dao.Together;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace ActionService.DbDirect {
    public class ServiceCommon {
        private OCF daoOCF;
        private AOCF daoAOCF;
        private TXN daoTXN;
        private TXFPARM daoTXFPARM;
        private JSW daoJSW;
        private DataGate daoDataGate;
        private UPF daoUPF;
        private DPT daoDPT;
        private RPT daoRPT;
        private TXFP daoTXFP;

        public ServiceCommon() {
            daoOCF = new OCF();
            daoAOCF = new AOCF();
            daoTXN = new TXN();
            daoTXFPARM = new TXFPARM();
            daoJSW = new JSW();
            daoDataGate = new DataGate();
            daoUPF = new UPF();
            daoDPT = new DPT();
            daoRPT = new RPT();
            daoTXFP = new TXFP();
        }
        /// <summary>
        /// 判斷在JSW這個TABLE裡面有沒有權限
        /// </summary>
        public bool HasJswPermission(string programID, string jswType) {
            return daoJSW.HasJswPermission(programID, jswType);
        }
        /// <summary>
        /// 判斷在JSW這個TABLE裡面有沒有權限
        /// </summary>
        public bool HasJswPermission(string programID) {
            return daoJSW.HasJswPermission(programID);
        }

        public DataTable ListTxnByUser(string userID) {
            return daoTXN.ListTxnByUser(userID);
        }

        public DataTable ListAllowJswPermission(string programID) {
            return daoJSW.ListAllowJswPermission(programID);
        }
        /// <summary>
        /// 傳入一個DataTable，如果該列已存在DB會下UPDATE語法，如果不存在會下INSERT語法
        /// </summary>
        /// <param name="inputDT">傳入的資料</param>
        /// <param name="tableName">TABLE的名字</param>
        /// <param name="insertColumnList">要新增的欄位</param>
        /// <param name="updateColumnList">要更新的欄位</param>
        /// <param name="updateOrDeleteKeysColumnList">KEY的欄位</param>
        public ResultData SaveForAll(DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList, PokeBall pokeBall) {
            return daoDataGate.SaveForAll(inputDT, tableName, insertColumnList, updateColumnList, updateOrDeleteKeysColumnList);
        }
        /// <summary>
        /// 傳入一個DataTable，如果該列已存在DB會下UPDATE語法，如果不存在會下INSERT語法
        /// </summary>
        /// <param name="connInfo">哪個DB的連線</param>
        /// <param name="inputDT">傳入的資料</param>
        /// <param name="tableName">TABLE的名字</param>
        /// <param name="insertColumnList">要新增的欄位</param>
        /// <param name="updateColumnList">要更新的欄位</param>
        /// <param name="updateOrDeleteKeysColumnList">KEY的欄位</param>
        public ResultData SaveForAll(ConnectionInfo connInfo, DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList, PokeBall pokeBall) {
            DataGate dataGate = new DataGate(connInfo);
            return dataGate.SaveForAll(inputDT, tableName, insertColumnList, updateColumnList, updateOrDeleteKeysColumnList);
        }
        /// <summary>
        /// 根據DataTable的改變，會下不同的SQL去異動DB，並傳回各異動的資料集
        /// </summary>
        /// <param name="inputDT">傳入的資料</param>
        /// <param name="tableName">TABLE的名字</param>
        /// <param name="insertColumnList">要新增的欄位</param>
        /// <param name="updateColumnList">要更新的欄位</param>
        /// <param name="updateOrDeleteKeysColumnList">KEY的欄位</param>
        public ResultData SaveForChanged(DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList, PokeBall pokeBall) {
            return daoDataGate.SaveForChanged(inputDT, tableName, insertColumnList, updateColumnList, updateOrDeleteKeysColumnList, pokeBall);
        }
        /// <summary>
        /// 根據DataTable的改變，會下不同的SQL去異動DB，並傳回各異動的資料集
        /// </summary>
        /// <param name="inputDT">傳入的資料</param>
        /// <param name="tableName">TABLE的名字</param>
        /// <param name="insertColumnList">要新增的欄位</param>
        /// <param name="updateColumnList">要更新的欄位</param>
        /// <param name="updateOrDeleteKeysColumnList">KEY的欄位</param>
        public ResultData SaveForChanged(DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList) {
            return daoDataGate.SaveForChanged(inputDT, tableName, insertColumnList, updateColumnList, updateOrDeleteKeysColumnList);
        }

        /// <summary>
        /// 多個動作包成一個Transaction用
        /// </summary>
        /// <param name="inputFunc">傳入一個Function，所有的動作都放在裡面</param>
        public ResultData MultiActionTransaction(Func<PokeBall, ResultStatus> inputFunc) {
            return daoDataGate.MultiActionTransaction(inputFunc);
        }
        /// <summary>
        /// 抓OCF這個TABLE存到物件裡
        /// </summary>
        public BO_OCF GetOCF() {
            return daoOCF.GetOCF();
        }

        public int getAOCFDates(string symd, string eymd) {
            return daoAOCF.GetAOCFDates(symd, eymd);
        }

        public ResultData ExecuteStoredProcedure(string sql, List<DbParameterEx> dbParmsEx, bool hasReturnParameter) {
            return daoDataGate.ExecuteStoredProcedure(sql, dbParmsEx, hasReturnParameter);
        }

        public ResultData ExecuteStoredProcedure(ConnectionInfo connInfo, string sql, List<DbParameterEx> dbParmsEx, bool hasReturnParameter) {
            daoDataGate = new DataGate(connInfo);
            return daoDataGate.ExecuteStoredProcedure(sql, dbParmsEx, hasReturnParameter);
        }

        public ResultData ExecuteInfoWorkFlow(string workFlowName, UserProgInfo userProgInfo, string folder, string service, string apName, string bkFileName) {
            string key = "infa";
            int seq = string.IsNullOrEmpty(service) ? 1 : 2;

            DataTable dt = daoTXFP.ListDataByKeyAndSeq(key, seq);
            ResultData result = new ResultData();

            string workingDirectory = dt.Rows[0]["ls_exec_file"].AsString();
            string domainFile = dt.Rows[0]["ls_domains_file"].AsString();
            string domain = dt.Rows[0]["ls_domain"].AsString();
            service = seq == 1 ? dt.Rows[0]["ls_server"].AsString() : service;
            string user = dt.Rows[0]["ls_str1"].AsString();
            string pwd = dt.Rows[0]["ls_str2"].AsString();
            string language = seq == 1 ? "" : "\nSET INFA_LANGUAGE=en";

            string command = 
$@"SET RunUsr={user}
SET RunPasswd={pwd}{language}
SET INFA_DOMAINS_FILE={domainFile}
{workingDirectory} startworkflow -service {service} -domain {domain} -uv RunUsr -pv RunPasswd -folder {folder} -wait {workFlowName} > {bkFileName}.log
echo return status = %errorlevel% >{bkFileName}.err
exit /b %errorlevel%
";
            string batFile = $"{bkFileName}.bat";
            System.IO.File.WriteAllText(batFile, command);

            Process process = new Process();
            process.StartInfo.FileName = batFile;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            int code;
            code = process.ExitCode;

            string codeDesc = getInfaCodeDesc(code);

            bool isError = false;


            if (process.ExitCode != 0)
            {
                isError = true;
            }

            if (isError)
            {
                SystemSounds.Beep.Play();
                result.returnString = $"請通知「{apName}」 Informatica 作業執行失敗!\n請查詢 {bkFileName}.err 錯誤訊息說明\nService：{service}\nFolder：{folder}\nWorkFlow：{workFlowName}\nCode Description：{code} = {codeDesc}";
                result.Status = ResultStatus.Fail;
            }
            else
            {
                File.Delete(batFile);
                result.Status = ResultStatus.Success;
            }

            return result;
        }

        public string getInfaCodeDesc(int code) {
            string desc = "";
            switch (code) {
                case 0:
                    desc = "Workflow ran successfully";
                    break;
                case 1:
                    desc = "Cannot connect to Power Center server";
                    break;
                case 2:
                    desc = "Workflow or folder does not exist";
                    break;
                case 3:
                    desc = "An error occurred in starting or running the workflow";
                    break;
                case 4:
                    desc = "Usage error";
                    break;
                case 5:
                    desc = "Internal pmcmd error";
                    break;
                case 7:
                    desc = "Invalid Username Password";
                    break;
                case 8:
                    desc = "You do not have permission to perform this task";
                    break;
                case 9:
                    desc = "Connection timed out";
                    break;
                case 13:
                    desc = "Username environment variable not defined";
                    break;
                case 14:
                    desc = "Password environment variable not defined";
                    break;
                case 15:
                    desc = "Username environment variable missing";
                    break;
                case 16:
                    desc = "Password environment variable missing";
                    break;
                case 17:
                    desc = "Parameter file doesnot exist";
                    break;
                case 18:
                    desc = "Initial value missing from parameter file";
                    break;
                case 20:
                    desc = "Repository error occurred. Pls check repository server and database are running";
                    break;
                case 21:
                    desc = "PowerCenter server shutting down";
                    break;
                case 22:
                    desc = "Workflow not unique. Please enter folder name";
                    break;
                case 23:
                    desc = "No data available";
                    break;
                case 24:
                    desc = "Out of memory";
                    break;
                case 25:
                    desc = "Command cancelled";
                    break;
            }
            return desc;
        }


        public DataTable ListDataForUserIDAndUserName() {
            return daoUPF.ListDataForUserIDAndUserName();
        }

        public DataTable ListDPT() {
            return daoDPT.ListData();
        }

        public DataTable ListRPT(string RPT_TXD_ID) {
            return daoRPT.ListData(RPT_TXD_ID);
        }

        public DataTable ListDataForTxnIdAndName() {
            return daoTXN.ListDataForTxnIdAndName();
        }

        public DataTable ListTXFPARM(string TXFPARM_SERVER, string TXFPARM_DB, string TXFPARM_TXN_ID, string TXFPARM_TID) {
            return daoTXFPARM.List(TXFPARM_SERVER, TXFPARM_DB, TXFPARM_TXN_ID, TXFPARM_TID);
        }
    }
}