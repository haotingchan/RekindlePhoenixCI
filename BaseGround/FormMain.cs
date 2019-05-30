﻿using ActionService.DbDirect;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using Log;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseGround {
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm {
        public FormMain() {
            InitializeComponent();
            ServiceCommon serviceCommon = new ServiceCommon();
            TXN daoTXN = new TXN();
            DataTable dt = new DataTable();
            if (GlobalInfo.USER_ID == GlobalDaoSetting.GetConnectionInfo.ConnectionName) {
                dt = Task.Run(() => daoTXN.ListData()).Result;
            }
            else {
                //dt = Task.Run(() => serviceCommon.ListTxnByUser(GlobalInfo.USER_ID)).Result;
                dt = Task.Run(() => daoTXN.ListData()).Result;
            }

            AccordionControlElement item = null;
            string txnId = "";
            string txnName = "";
            string txnExtend = "";
            string txnParent = "";
            //全部作業
            foreach (var group in accordionMenu.Elements) {
                string groupTag = group.Tag.ToString().ToUpper();


                var result = from x in dt.AsEnumerable()
                             where x.Field<String>("TXN_ID").Substring(0, 1) == groupTag ||
                                   (x.Field<String>("TXN_ID").Substring(0, 2) == "ND" &&
                                   x.Field<String>("TXN_ID").Substring(2, 1) == groupTag)
                             orderby x.Field<Int16>("TXN_SEQ_NO")
                             select x;

                //各系列作業
                foreach (DataRow row in result) {

                    txnId = row["TXN_ID"].ToString().Trim();
                    txnName = row["TXN_NAME"].ToString().Trim();
                    txnExtend = row["TXN_EXTEND"].ToString().Trim();
                    txnParent = row["TXN_PARENT_ID"].ToString().Trim();
                    //一階項目
                    if (txnId.Substring(0, 2) != "ND" && txnParent == groupTag) {
                        item = new AccordionControlElement() {
                            Text = string.Format("{0} {1}", txnId, txnName),
                            Tag = new ItemData() { TXN_ID = txnId, TXN_NAME = txnName },
                            Style = ElementStyle.Item
                        };
                    }
                    //二階項目先跳過
                    else if (txnParent != groupTag) continue;
                    //一階節點
                    else {
                        item = new AccordionControlElement() {
                            Text = string.Format("{0}", txnName),
                            Tag = new ItemData() { TXN_ID = txnId, TXN_NAME = txnName },
                            Style = ElementStyle.Group
                        };
                        if (txnExtend == "Y") item.Expanded = true;
                        //放入子項目(二階)
                        foreach (DataRow dr in result) {
                            string parent = dr["TXN_PARENT_ID"].ToString().Trim();
                            string Id = dr["TXN_ID"].ToString().Trim();
                            string name = dr["TXN_NAME"].ToString().Trim();
                            string level = dr["TXN_LEVEL"].ToString().Trim();
                            AccordionControlElement subItem = null;
                            if (Id.Substring(0, 2) == "ND") {
                                subItem = new AccordionControlElement() {
                                    Text = string.Format("{0}", name),
                                    Tag = new ItemData() { TXN_ID = Id, TXN_NAME = name },
                                    Style = ElementStyle.Group
                                };
                            }
                            else {
                                subItem = new AccordionControlElement() {
                                    Text = string.Format("{0} {1}", Id, name),
                                    Tag = new ItemData() { TXN_ID = Id, TXN_NAME = name },
                                    Style = ElementStyle.Item
                                };
                            }
                            if (parent == txnId) item.Elements.Add(subItem);
                        }
                    }
                    group.Elements.Add(item);
                }//foreach (DataRow row in result)
                if (group.Elements.Count == 0) {
                    group.Visible = false;
                }

                //放入父階層
                //foreach (DataRow row in result) {
                //    txnParent = row["TXN_PARENT_ID"].ToString().Trim();
                //    txnName = row["TXN_NAME"].ToString().Trim();
                //    if (txnParent.Length>1&&txnParent.Substring(0, 2) == "ND") {
                //        item = new AccordionControlElement() {
                //            Text = string.Format("{0} {1}", txnId, txnName),
                //            Tag = new ItemData() { TXN_ID = txnId, TXN_NAME = txnName },
                //            Style = ElementStyle.Item
                //        };
                //        group.Elements[txnParent].AccordionControl.Elements.Add(item);
                //    }
                //}
            }//foreach (var group in accordionMenu.Elements)

            toolStripStatusLabelServerName.Text = GlobalDaoSetting.GetConnectionInfo.ConnectionName;
            toolStripStatusLabelDBName.Text = GlobalDaoSetting.Database;
            toolStripStatusLabelUserName.Text = GlobalInfo.USER_NAME;
            toolStripStatusLabelVersionNum.Text = new AP().GetMaxVersion();

            switch (SystemStatus.SystemType) {
                case SystemType.CI:
                    this.Text = "交易資訊統計管理系統";
                    break;

                default:
                    break;
            }
        }

        public FormMain(string txnID, string txnName) : this() {
            OpenForm(txnID, txnName).CloseBox = false;
            scSearch.Visible = false;
            sidePanelMenu.Visible = false;
        }

        /// <summary>
        /// 開啟或關閉MdiChild時會觸發此事件
        /// </summary>
        private void FormMain_MdiChildActivate(object sender, EventArgs e) {
            // 當ActiveMdiChild為null時，代表已經都沒有MdiChild了
            if (this.ActiveMdiChild == null) {
                toolStripButtonSave.Enabled = false;
                toolStripButtonInsert.Enabled = false;
                toolStripButtonDelete.Enabled = false;
                toolStripButtonRetrieve.Enabled = false;
                toolStripButtonRun.Enabled = false;
                toolStripButtonImport.Enabled = false;
                toolStripButtonExport.Enabled = false;
                toolStripButtonPrintAll.Enabled = false;
            }
        }

        private void ToolStripButtonSave_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ((FormParent)ActiveMdiChild).ProcessSaveFlow();
        }

        private void ToolStripButtonInsert_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ((FormParent)ActiveMdiChild).ProcessInsert();
        }

        private void ToolStripButtonDelete_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ((FormParent)ActiveMdiChild).ProcessDelete();
        }

        private void ToolStripButtonRetrieve_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ((FormParent)ActiveMdiChild).ProcessRetrieve();
        }

        private void toolStripButtonRun_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ((FormParent)ActiveMdiChild).ProcessRun();
        }

        private void toolStripButtonImport_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ((FormParent)ActiveMdiChild).ProcessImport();
        }

        private void toolStripButtonExport_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ((FormParent)ActiveMdiChild).ProcessExport();
        }

        private void ToolStripButtonPrintAll_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            FormParent activeMdiChildFormParent = ((FormParent)ActiveMdiChild);
            activeMdiChildFormParent.ProcessPrintAll(activeMdiChildFormParent.PrintOrExportSetting());
        }

        private void ToolStripButtonQuit_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            // 當ActiveMdiChild為null時，代表已經都沒有MdiChild了
            if (this.ActiveMdiChild == null) {
                Dispose();
                Application.Exit();
            }
            else {
                FormParent activeMdiChildFormParent = ((FormParent)ActiveMdiChild);
                activeMdiChildFormParent.AutoValidate = AutoValidate.Disable;
                activeMdiChildFormParent.CausesValidation = false;
                activeMdiChildFormParent.Close();
            }
        }

        public FormParent OpenForm(string txn_id, string txn_name) {
            var dllIndividual = Assembly.LoadFile(Application.ExecutablePath);
            string typeFormat = "{0}.FormUI.Prefix{1}.W{2}";
            Type myType = dllIndividual.GetType(string.Format(typeFormat, Path.GetFileNameWithoutExtension(Application.ExecutablePath), txn_id.Substring(0, 1), txn_id));

            if (myType == null) {
                MessageDisplay.Error("無此程式");
                accordionMenu.Focus();
                accordionMenu.KeyNavHelperEx.SelectedElement = accordionMenu.SelectedElement;
                return null;
            }

            object myObj = Activator.CreateInstance(myType, txn_id, txn_name);

            FormParent formInstance = (FormParent)myObj;

            //int width = SystemInformation.PrimaryMonitorSize.Width;
            //if (width <= 1600) {
            //    formInstance.WindowState = FormWindowState.Maximized;
            //}

            if (formInstance.BeforeOpen() == ResultStatus.Success) {
                formInstance.MdiParent = this;
                formInstance.FormClosed += new FormClosedEventHandler(Child_FormClosed);
                formInstance.Icon = (Icon)Icon.Clone();
                formInstance.BackColor = Color.FromArgb(192, 220, 192);
                formInstance.StartPosition = FormStartPosition.Manual;
                formInstance.WindowState = FormWindowState.Maximized;

                formInstance.Show();
            }

            return formInstance;
        }

        private void Child_FormClosed(object sender, FormClosedEventArgs e) {
            if (this.MdiChildren.Length == 1) {
                this.BeginInvoke(new MethodInvoker(() => { accordionMenu.Focus(); accordionMenu.KeyNavHelperEx.SelectedElement = accordionMenu.SelectedElement; }));
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e) {
            SingletonLogger.Instance.Info(GlobalInfo.USER_ID, "Logoff", "FormClosed", " ");
            Application.Exit();
        }

        private void SearchControl1_KeyDown(object sender, KeyEventArgs e) {
            string txnID = "";

            if (e.KeyCode == Keys.Enter) {
                txnID = ((SearchControl)sender).EditValue.ToString();

                if (string.IsNullOrEmpty(txnID)) {
                    MessageDisplay.Error("請輸入文字 !");
                    return;
                }

                var itemList = accordionMenu.Elements.Where(x => x.Tag.ToString().ToUpper() == txnID.Substring(0, 1).ToUpper()).FirstOrDefault();

                if (itemList == null) {
                    MessageDisplay.Error("無此程式");
                    return;
                }

                var item = itemList.Elements.Where(x => ((ItemData)x.Tag).TXN_ID.ToUpper() == txnID.ToUpper()).FirstOrDefault();

                if (item == null) {
                    MessageDisplay.Error("無此程式");
                    return;
                }
                else {
                    accordionMenu.SelectedElement = item;
                    accordionMenu.KeyNavHelperEx.SelectedElement = item;
                    accordionMenu.ExpandElement(item);
                    accordionMenu.MakeElementVisible(accordionMenu.SelectedElement);

                    ItemData itemData = ((ItemData)item.Tag);
                    OpenForm(itemData.TXN_ID, itemData.TXN_NAME);
                }
            }
        }

        private void AccordionMenu_ElementClick(object sender, ElementClickEventArgs e) {
            ((AccordionControl)sender).SelectedElement = null;
            ((AccordionControl)sender).SelectedElement = e.Element;

            accordionMenu.KeyNavHelperEx.SelectedElement = e.Element;
        }

        private void AccordionMenu_DoubleClick(object sender, EventArgs e) {
            var element = ((AccordionControl)sender).SelectedElement;

            if (element != null && element.Style == ElementStyle.Item) {
                ItemData itemData = ((ItemData)element.Tag);
                OpenForm(itemData.TXN_ID, itemData.TXN_NAME);
            }
        }

        private void AccordionMenu_KeyDown(object sender, KeyEventArgs e) {
            KeyboardNavigationHelper helper = accordionMenu.KeyNavHelperEx;
            var element = ((AccordionControl)sender).SelectedElement;

            if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && helper.SelectedElement.Style != ElementStyle.Group) {
                accordionMenu.SelectedElement = helper.SelectedElement;
                helper.SelectedElement = accordionMenu.SelectedElement;
            }

            if (e.KeyCode == Keys.Enter && element != null && element.Style == ElementStyle.Item) {
                ItemData itemData = ((ItemData)element.Tag);
                OpenForm(itemData.TXN_ID, itemData.TXN_NAME);
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                Application.Exit();
            }
        }

        protected override void OnClosed(EventArgs e) {
            Dispose();
            Close();
        }
    }

    public class AccordionControlEx : AccordionControl {
        public KeyboardNavigationHelper KeyNavHelperEx {
            get { return this.KeyNavHelper; }
        }
    }

    public class ItemData {
        public string TXN_ID { get; set; }
        public string TXN_NAME { get; set; }
    }
}