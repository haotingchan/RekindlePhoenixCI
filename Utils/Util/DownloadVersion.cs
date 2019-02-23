using System;
using System.IO;
using Utils.Dao;
using Utils.Entity.Constant;
using Utils.Entity.Table;
using Utils.Entity.XML;
using Utils.FileType;
using Utils.WinForm;

namespace Utils.Util
{
    public class DownloadVersion
    {
        /// <summary>
        /// 資料庫名稱
        /// </summary>
        private string Database { get; set; }

        /// <summary>
        /// 系統類別
        /// </summary>
        private string Type { get; set; }
        
        private Download download = SettingDragons<Setting>.Instance.Setting.Download;

        public DownloadVersion()
        {
            this.Database = download.Database;
            this.Type = download.PBcodeType;
            util = DataUtil.getInstance(this.Database);
            dao = PBCODEDao.getInstance(util);
        }

        private DataUtil util = null;
        private PBCODEDao dao = null;

        /// <summary>
        /// DownloadVersion建構元
        /// </summary>
        /// <param name="database">資料庫名稱(Setting>Database : DBConst)</param>
        /// <param name="type">系統類別(PBCODE_TYPE)</param>
        public DownloadVersion(string database, string type)
        {
            this.Database = database;
            this.Type = type;
            util = DataUtil.getInstance(this.Database);
            dao = PBCODEDao.getInstance(util);
        }

        private downloadForm d = new downloadForm();
        private string directory = AppDomain.CurrentDomain.BaseDirectory;
        private string bkFolder = AppDomain.CurrentDomain.BaseDirectory + "backup\\";

        public bool downloadVersion()
        {
            switch (download.Type)
            {
                case "XML":
                    return downloadVersionByXML();
                case "INI":
                    return downloadVersionByINI();
                default:
                    return false;
            }
        }

        private bool downloadVersionByINI()
        {
            bool result = false;
            d.Show();
            d.Refresh();
            INIFile iniFile = new INIFile(directory + download.FileName);
            try
            {
                var versionList = dao.getVersionList(this.Type);
                foreach (var v in versionList)
                {
                    d.PerformStep();
                    string fileOldVersion = iniFile.ReadKeyValue(DownloadConst.VERSION, v.PBCODE_FILE_NAME);
                    fileOldVersion = string.IsNullOrEmpty(fileOldVersion) ? "0" : fileOldVersion;
                    string filePath = directory + v.PBCODE_FILE_NAME;
                    if (int.Parse(v.PBCODE_VERSION) > int.Parse(fileOldVersion))
                    {
                        updateFile(filePath, v);
                        iniFile.Write(DownloadConst.VERSION, v.PBCODE_FILE_NAME, v.PBCODE_VERSION);
                        result = true;
                    }
                }
            }
            catch 
            {
                throw;
            }
            finally
            {
                iniFile.Write(DownloadConst.VERSION, DownloadConst.DATE, DateTime.Now.ToString("yyyy/MM/dd"));
            }
            return result;
        }

        private bool downloadVersionByXML()
        {
            bool result = false;
            d.Show();
            d.Refresh();
            XMLFile<VersionInfo> xmlFile = new XMLFile<VersionInfo>(directory + download.FileName);
            VersionInfo verInfo = xmlFile.read();
            try
            {
                var versionList = dao.getVersionList(this.Type);
                foreach (var v in versionList)
                {
                    d.PerformStep();
                    var fileInfo = verInfo.FileList.Find(x => x.Name == v.PBCODE_FILE_NAME);
                    if (fileInfo == null)
                    {
                        verInfo.FileList.Add(new Utils.Entity.XML.FileInfo(v.PBCODE_FILE_NAME));
                    }
                    string fileOldVersion = verInfo.FileList.Find(x => x.Name == v.PBCODE_FILE_NAME).Ver;
                    string filePath = directory + v.PBCODE_FILE_NAME;
                    if (int.Parse(v.PBCODE_VERSION) > int.Parse(fileOldVersion))
                    {
                        updateFile(filePath, v);
                        verInfo.FileList.Find(x => x.Name == v.PBCODE_FILE_NAME).Ver = v.PBCODE_VERSION;
                        result = true;
                    }
                }
                d.Hide();
            }
            catch
            {
                throw;
            }
            finally
            {
                verInfo.Date = DateTime.Now.ToString("yyyy/MM/dd");
                xmlFile.write(verInfo);
            }
            return result;
        }

        /// <summary>
        /// 更新檔案
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="v"></param>
        private void updateFile(string filePath, PBCODE v)
        {
            d.setTitle("系統更新中....");
            d.downloadFile(v.PBCODE_FILE_NAME);
            d.Refresh();
            if (File.Exists(filePath))
            {
                Directory.CreateDirectory(bkFolder);
                if (File.Exists(bkFolder + v.PBCODE_FILE_NAME)) {
                    File.Delete(bkFolder + v.PBCODE_FILE_NAME);
                }
                File.Move(filePath, bkFolder + v.PBCODE_FILE_NAME);
            }
            using (FileStream fs = File.Create(filePath))
            {
                fs.Write(v.PBCODE_BIN_CODE, 0, v.PBCODE_BIN_CODE.Length);
            }
        }

        /// <summary>
        /// 清除舊版本檔案
        /// </summary>
        public void removeOldVersion()
        {
            DirectoryInfo di = new DirectoryInfo(bkFolder);
            if (di.Exists)
            {
                di.Delete(true);
            }
        }
    }
}