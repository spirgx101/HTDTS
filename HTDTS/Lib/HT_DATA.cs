using Arktech;
using HTDTS.DAO;
using HTDTS.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace HTDTS.Lib
{
    public class HT_DATA
    {
        private readonly string SEND_FOLDER = ConfigurationManager.AppSettings["SEND"].ToString();
        private readonly string RECV_FOLDER = ConfigurationManager.AppSettings["RECV"].ToString();
        private readonly string CHECK_FOLDER = ConfigurationManager.AppSettings["CHECK"].ToString();
        private readonly string WSC_FOLDER = ConfigurationManager.AppSettings["WSC"].ToString();
        private readonly Logger LOGGER = new Logger();


        public HT_DATA(Logger logger)
        {
            LOGGER = logger;
        }

        #region 下傳檔案
        public DataStatus Gen_XSC_XMSDO(WSParmContents wspc, string fcode)
        {
            string file_path = string.Empty;
            string check_path = string.Empty;
            string pc_file_crt_date = string.Empty;
            ITransFile trans_file = null;

            file_path = Path.Combine(SEND_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"XMSDO.DB" : @"XMSDO.SOT");
            check_path = Path.Combine(CHECK_FOLDER, @"XMSDO.DAT");
            trans_file = new HT_XSC_XMSDOFile();

            string ht_file_crt_date = wspc.DOWNLOAD_INFO[fcode].Item2;

            if (File.Exists(file_path))
            {
                pc_file_crt_date = File.GetLastWriteTime(file_path).ToString("yyyyMMdd_HHmmss");

                if (pc_file_crt_date.Substring(0, 8) == DateTime.Now.ToString("yyyyMMdd"))
                {
                    if (string.Compare(pc_file_crt_date, ht_file_crt_date) > 0)
                    {
                        //wspc.FLAG = "5";
                        //wspc.MSG = "傳輸XMSDO.DAT檔案";
                        return DataStatus.NotCreateAndSend;
                    }
                }
            }

            DataTable table = new DataTable();
            DAO_XSC eepdc = new DAO_XSC("XSC");

            string[] xsc_data = wspc.XSC_DATA.Split(',');

            table = eepdc.Run_sp_xsc_XB56(wspc.STR_NO, xsc_data[0], xsc_data[1]);

            trans_file.CreateTransFile(table, check_path);

            CopDll.SetTransType(wspc.TRANS_TYPE);
            if (CopDll.Convert_XMSDO() < 0)
            {
                //wspc.FLAG = "-1";
                //wspc.MSG = "產生XMSDO.DAT錯誤";
                return DataStatus.Error;
            }

            check_path = Path.Combine(CHECK_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"XMSDO.DB" : @"XMSDO.SOT");

            pc_file_crt_date = File.GetLastWriteTime(check_path).ToString("yyyyMMdd_HHmmss");

            if (string.Compare(pc_file_crt_date, ht_file_crt_date) < 0)
            {
                //wspc.FLAG = "5";
                //wspc.MSG = "不傳輸XMSDO.DAT檔案";
                return DataStatus.NotSend;
            }

            //wspc.FLAG = "5";
            //wspc.MSG = "產生、傳輸XMSDO.DAT檔案";
            return DataStatus.CreateAndSend;
        }

        public DataStatus Gen_XSC_DPORD(WSParmContents wspc, string fcode)
        {
            string file_path = string.Empty;
            string check_path = string.Empty;
            string pc_file_crt_date = string.Empty;
            ITransFile trans_file = null;

            file_path = Path.Combine(SEND_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"DPORD.DB" : @"DPORD.SOT");
            check_path = Path.Combine(CHECK_FOLDER, @"DPORD.DAT");
            trans_file = new HT_XSC_DPORDFile();

            string ht_file_crt_date = wspc.DOWNLOAD_INFO[fcode].Item2;

            if (File.Exists(file_path))
            {
                pc_file_crt_date = File.GetLastWriteTime(file_path).ToString("yyyyMMdd_HHmmss");

                if (pc_file_crt_date.Substring(0, 8) == DateTime.Now.ToString("yyyyMMdd"))
                {
                    if (string.Compare(pc_file_crt_date, ht_file_crt_date) > 0)
                    {
                        //wspc.FLAG = "5";
                        //wspc.MSG = "傳輸DPORD.DAT檔案";
                        return DataStatus.NotCreateAndSend;
                    }
                }
            }

            DataTable table = new DataTable();
            DAO_XSC eepdc = new DAO_XSC("XSC");

            table = eepdc.Run_sp_xsc_A11TA(wspc.STR_NO);

            trans_file.CreateTransFile(table, check_path);

            CopDll.SetTransType(wspc.TRANS_TYPE);
            if (CopDll.Convert_DPORD() < 0)
            {
                //wspc.FLAG = "-1";
                //wspc.MSG = "產生DPORD.DAT錯誤";
                return DataStatus.Error;
            }

            check_path = Path.Combine(CHECK_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"DPORD.DB" : @"DPORD.SOT");

            pc_file_crt_date = File.GetLastWriteTime(check_path).ToString("yyyyMMdd_HHmmss");

            if (string.Compare(pc_file_crt_date, ht_file_crt_date) < 0)
            {
                //wspc.FLAG = "5";
                //wspc.MSG = "不傳輸DPORD.DAT檔案";
                return DataStatus.NotSend;
            }

            //wspc.FLAG = "5";
            //wspc.MSG = "產生、傳輸DPORD.DAT檔案";
            return DataStatus.CreateAndSend;
        }

        public DataStatus Gen_CSC_DPPRD(WSParmContents wspc, string fcode)
        {
            string file_path = string.Empty;
            string check_path = string.Empty;
            string pc_file_crt_date = string.Empty;
            ITransFile trans_file = null;

            if (File.Exists(Path.Combine(CHECK_FOLDER, @"DPPRD.DAT"))) File.Delete(Path.Combine(CHECK_FOLDER, @"DPPRD.DAT"));
            if (File.Exists(Path.Combine(CHECK_FOLDER, @"ODPPRD.DAT"))) File.Delete(Path.Combine(CHECK_FOLDER, @"ODPPRD.DAT"));

            if (File.Exists(Path.Combine(SEND_FOLDER, @"DPPRD.DAT"))) File.Delete(Path.Combine(SEND_FOLDER, @"DPPRD.DAT"));
            if (File.Exists(Path.Combine(SEND_FOLDER, @"ODPPRD.DAT"))) File.Delete(Path.Combine(SEND_FOLDER, @"ODPPRD.DAT"));

            file_path = Path.Combine(SEND_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"DPPRD.DB" : @"DPPRD.SOT");
            check_path = Path.Combine(CHECK_FOLDER, @"DPPRD.DAT");
            trans_file = new HT_CSC_DPPRDFile();

            string ht_file_crt_date = wspc.DOWNLOAD_INFO[fcode].Item2;

            if (File.Exists(file_path))
            {
                pc_file_crt_date = File.GetLastWriteTime(file_path).ToString("yyyyMMdd_HHmmss");

                if (pc_file_crt_date.Substring(0, 8) == DateTime.Now.ToString("yyyyMMdd"))
                {
                    if (string.Compare(pc_file_crt_date, ht_file_crt_date) > 0)
                    {
                        //wspc.FLAG = "5";
                        //wspc.MSG = "傳輸DPPRD.DAT、ODPPRD.DAT檔案";
                        return DataStatus.NotCreateAndSend;
                    }
                }
            }

            DataTable table = new DataTable();
            DAO_CSC csc_1o = new DAO_CSC("CSC");

            if (csc_1o.Run_sp_csc_1O_GenData(wspc.STR_NO) < 0) return DataStatus.Error;

            table = csc_1o.Run_sp_csc_1O_dnPrdt(wspc.STR_NO, "1");
            trans_file.CreateTransFile(table, check_path);

            file_path = Path.Combine(SEND_FOLDER, @"ODPPRD.DAT");
            check_path = Path.Combine(CHECK_FOLDER, @"ODPPRD.DAT");
            trans_file = new HT_CSC_ODPPRDFile();

            table = csc_1o.Run_sp_csc_1O_dnPrdt(wspc.STR_NO, "2");
            trans_file.CreateTransFile(table, check_path);

            CopDll.SetTransType(wspc.TRANS_TYPE);
            if (CopDll.Convert_DPPRD() < 0)
            {
                //wspc.FLAG = "-1";
                //wspc.MSG = "產生DPPRD.DAT、ODPPRD.DAT錯誤";
                return DataStatus.Error;
            }

            check_path = Path.Combine(CHECK_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"DPPRD.DB" : @"DPPRD.SOT");

            pc_file_crt_date = File.GetLastWriteTime(check_path).ToString("yyyyMMdd_HHmmss");

            if (string.Compare(pc_file_crt_date, ht_file_crt_date) < 0)
            {
                //wspc.FLAG = "5";
                //wspc.MSG = "不傳輸DPPRD.DAT、ODPPRD.DAT檔案";
                return DataStatus.NotSend;
            }

            //wspc.FLAG = "5";
            //wspc.MSG = "產生、傳輸DPPRD.DAT、ODPPRD.DAT檔案";
            return DataStatus.CreateAndSend;
        }

        public DataStatus Gen_CSC_INSTANT(WSParmContents wspc, string fcode)
        {
            string file_path = string.Empty;
            string check_path = string.Empty;
            string pc_file_crt_date = string.Empty;
            ITransFile trans_file = null;

            file_path = Path.Combine(SEND_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"INSTANT.DB" : @"INSTANT.SOT");
            check_path = Path.Combine(CHECK_FOLDER, @"INSTANT.DAT");
            trans_file = new HT_CSC_INSTANTFile();

            string ht_file_crt_date = wspc.DOWNLOAD_INFO[fcode].Item2;

            if (File.Exists(file_path))
            {
                pc_file_crt_date = File.GetLastWriteTime(file_path).ToString("yyyyMMdd_HHmmss");

                if (pc_file_crt_date.Substring(0, 8) == DateTime.Now.ToString("yyyyMMdd"))
                {
                    if (string.Compare(pc_file_crt_date, ht_file_crt_date) > 0)
                    {
                        //wspc.FLAG = "5";
                        //wspc.MSG = "傳輸INSTANT.DAT檔案";
                        return DataStatus.NotCreateAndSend;
                    }
                }
            }

            DataTable table = new DataTable();
            DAO_CSC csc_1o = new DAO_CSC("CSC");

            table = csc_1o.Run_sp_csc_1O_dnStock(wspc.STR_NO);
            trans_file.CreateTransFile(table, check_path);

            CopDll.SetTransType(wspc.TRANS_TYPE);
            if (CopDll.Convert_INSTANT() < 0)
            {
                //wspc.FLAG = "-1";
                //wspc.MSG = "產生INSTANT.DAT錯誤";
                return DataStatus.Error;
            }

            check_path = Path.Combine(CHECK_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"INSTANT.DB" : @"INSTANT.SOT");
   
            pc_file_crt_date = File.GetLastWriteTime(check_path).ToString("yyyyMMdd_HHmmss");

            if (string.Compare(pc_file_crt_date, ht_file_crt_date) < 0)
            {
                //wspc.FLAG = "5";
                //wspc.MSG = "不傳輸INSTANT.DAT檔案";
                return DataStatus.NotSend;
            }

            //wspc.FLAG = "5";
            //wspc.MSG = "產生、傳輸INSTANT.DAT檔案";
            return DataStatus.CreateAndSend;
        }

        public DataStatus Gen_CSC_PRODBOX(WSParmContents wspc, string fcode)
        {
            string file_path = string.Empty;
            string check_path = string.Empty;
            string pc_file_crt_date = string.Empty;
            ITransFile trans_file = null;

            file_path = Path.Combine(SEND_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"PRODBOX.DB" : @"PRODBOX.SOT");
            check_path = Path.Combine(CHECK_FOLDER, @"PRODBOX.DAT");
            trans_file = new HT_CSC_PRODBOXFile();

            string ht_file_crt_date = wspc.DOWNLOAD_INFO[fcode].Item2;
            pc_file_crt_date = File.GetLastWriteTime(file_path).ToString("yyyyMMdd_HHmmss");

            if (File.Exists(file_path))
            {
                pc_file_crt_date = File.GetLastWriteTime(file_path).ToString("yyyyMMdd_HHmmss");

                if (pc_file_crt_date.Substring(0, 8) == DateTime.Now.ToString("yyyyMMdd"))
                {
                    if (string.Compare(pc_file_crt_date, ht_file_crt_date) > 0)
                    {
                        //wspc.FLAG = "5";
                        //wspc.MSG = "傳輸PRODBOX.DAT檔案";
                        return DataStatus.NotCreateAndSend;
                    }
                }
            }

            DataTable table = new DataTable();
            DAO_CSC csc_1o = new DAO_CSC("CSC");

            table = csc_1o.Run_sp_csc_1O_dnBoxCode(wspc.STR_NO);
            trans_file.CreateTransFile(table, check_path);

            CopDll.SetTransType(wspc.TRANS_TYPE);
            if (CopDll.Convert_PRODBOX() < 0)
            {
                //wspc.FLAG = "-1";
                //wspc.MSG = "產生PRODBOX.DAT錯誤";
                return DataStatus.Error;
            }

            check_path = Path.Combine(CHECK_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"PRODBOX.DB" : @"PRODBOX.SOT");

            pc_file_crt_date = File.GetLastWriteTime(check_path).ToString("yyyyMMdd_HHmmss");

            if (string.Compare(pc_file_crt_date, ht_file_crt_date) < 0)
            {
                //wspc.FLAG = "5";
                //wspc.MSG = "不傳輸PRODBOX.DAT檔案";
                return DataStatus.NotSend;
            }

            //wspc.FLAG = "5";
            //wspc.MSG = "產生、傳輸PRODBOX.DAT檔案";
            return DataStatus.CreateAndSend;
        }

        public DataStatus Gen_SMD_DPPRD(WSParmContents wspc, string fcode)
        {
            string file_path = string.Empty;
            string check_path = string.Empty;
            string pc_file_crt_date = string.Empty;
            ITransFile trans_file = null;

            if (File.Exists(Path.Combine(CHECK_FOLDER, @"DPPRD.DAT"))) File.Delete(Path.Combine(CHECK_FOLDER, @"DPPRD.DAT"));
            if (File.Exists(Path.Combine(CHECK_FOLDER, @"ODPPRD.DAT"))) File.Delete(Path.Combine(CHECK_FOLDER, @"ODPPRD.DAT"));

            if (File.Exists(Path.Combine(SEND_FOLDER, @"DPPRD.DAT"))) File.Delete(Path.Combine(SEND_FOLDER, @"DPPRD.DAT"));
            if (File.Exists(Path.Combine(SEND_FOLDER, @"ODPPRD.DAT"))) File.Delete(Path.Combine(SEND_FOLDER, @"ODPPRD.DAT"));

            file_path = Path.Combine(SEND_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"SMDDPPRD.DB" : @"SMDITEM.SOT");
            check_path = Path.Combine(CHECK_FOLDER, @"SMDITEM.DAT");
           
            trans_file = new HT_SMD_ITEMFile(); 

            string ht_file_crt_date = wspc.DOWNLOAD_INFO[fcode].Item2;
            

            if (File.Exists(file_path))
            {
                pc_file_crt_date = File.GetLastWriteTime(file_path).ToString("yyyyMMdd_HHmmss");

                if (pc_file_crt_date.Substring(0, 8) == DateTime.Now.ToString("yyyyMMdd"))
                {
                    //Console.WriteLine($"PC：{pc_file_crt_date.Substring(0, 8)},NOW：{DateTime.Now.ToString("yyyyMMdd")}");
                    if (string.Compare(pc_file_crt_date, ht_file_crt_date) > 0) //電腦檔案比較新，不產生新檔直接傳輸
                    {
                        //Console.WriteLine($"PC：{pc_file_crt_date},HT：{ht_file_crt_date}");
                        //wspc.FLAG = "5";
                        //wspc.MSG = "傳輸DPPRD.DAT、ODPPRD.DAT、SMDITEM.DAT檔案";
                        return DataStatus.NotCreateAndSend;
                    }
                }
            }

            DataTable table = new DataTable();
            DAO_SMD smd = new DAO_SMD("SMD");

            table = smd.Run_sp_SMDITEM(wspc.STR_NO);
            trans_file.CreateTransFile(table, check_path);


            file_path = Path.Combine(SEND_FOLDER, @"DPPRD.DAT");
            check_path = Path.Combine(CHECK_FOLDER, @"DPPRD.DAT");
            trans_file = new HT_SMD_DPPRDFile();
            table = smd.Run_sp_SMDDPPRD(wspc.STR_NO);
            trans_file.CreateTransFile(table, check_path);


            CopDll.SetTransType(wspc.TRANS_TYPE);
            if (CopDll.Convert_SMDDPPRD() < 0)
            {
                //wspc.FLAG = "-1";
                //wspc.MSG = "產生DPPRD.DAT、ODPPRD.DAT、SMDITEM.DAT錯誤";
                return DataStatus.Error;
            }

            check_path = Path.Combine(CHECK_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"SMDDPPRD.DB" : @"SMDITEM.SOT");

            pc_file_crt_date = File.GetLastWriteTime(check_path).ToString("yyyyMMdd_HHmmss");

            if (string.Compare(pc_file_crt_date, ht_file_crt_date) < 0) //HT檔案比較新，不傳輸檔案
            {
                //wspc.FLAG = "5";
                //wspc.MSG = "不傳輸DPPRD.DAT、ODPPRD.DAT、SMDITEM.DAT檔案";
                return DataStatus.NotSend;
            }

            //wspc.FLAG = "5";
            //wspc.MSG = "產生、傳輸DPPRD.DAT、ODPPRD.DAT、SMDITEM.DAT檔案";
            return DataStatus.CreateAndSend;
        }

        public DataStatus Gen_SMD_RCVHL(WSParmContents wspc, string fcode)
        {
            string file_path = string.Empty;
            string check_path = string.Empty;
            string pc_file_crt_date = string.Empty;
            ITransFile trans_file = null;

            if (File.Exists(Path.Combine(CHECK_FOLDER, @"SMDRCVH.DAT"))) File.Delete(Path.Combine(CHECK_FOLDER, @"SMDRCVH.DAT"));
            if (File.Exists(Path.Combine(CHECK_FOLDER, @"SMDRCVL.DAT"))) File.Delete(Path.Combine(CHECK_FOLDER, @"SMDRCVL.DAT"));

            if (File.Exists(Path.Combine(SEND_FOLDER, @"SMDRCVH.DAT"))) File.Delete(Path.Combine(SEND_FOLDER, @"SMDRCVH.DAT"));
            if (File.Exists(Path.Combine(SEND_FOLDER, @"SMDRCVL.DAT"))) File.Delete(Path.Combine(SEND_FOLDER, @"SMDRCVL.DAT"));

            file_path = Path.Combine(SEND_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"SMDRCVHL.DB" : @"SMDDPD.DAT");
            check_path = Path.Combine(CHECK_FOLDER, @"SMDDPD.DAT");
            trans_file = new HT_SMD_DPDFile();

            string ht_file_crt_date = wspc.DOWNLOAD_INFO[fcode].Item2;

            /*
            if (File.Exists(file_path))
            {
                pc_file_crt_date = File.GetLastWriteTime(file_path).ToString("yyyyMMdd_HHmmss");

                if (pc_file_crt_date.Substring(0, 8) == DateTime.Now.ToString("yyyyMMdd"))
                {
                    //Console.WriteLine($"PC：{pc_file_crt_date.Substring(0, 8)},NOW：{DateTime.Now.ToString("yyyyMMdd")}");
                    if (string.Compare(pc_file_crt_date, ht_file_crt_date) > 0) //電腦檔案比較新，不產生新檔直接傳輸
                    {
                        //Console.WriteLine($"PC：{pc_file_crt_date},HT：{ht_file_crt_date}");
                        //wspc.FLAG = "5";
                        //wspc.MSG = "傳輸SMDDPD.DAT、SMDRCVH.DAT、SMDRCVL.DAT檔案";
                        return DataStatus.NotCreateAndSend;
                    }
                }
            }
            */
            DataTable table = new DataTable();
            DAO_SMD smd = new DAO_SMD("SMD");

            table = smd.Run_sp_SMDDPD(wspc.STR_NO);
            trans_file.CreateTransFile(table, check_path);


            file_path = Path.Combine(SEND_FOLDER, @"SMDRCVH.DAT");
            check_path = Path.Combine(CHECK_FOLDER, @"SMDRCVH.DAT");
            trans_file = new HT_SMD_RCVHFile();
            table = smd.Run_sp_SMDRCVH(wspc.STR_NO);
            trans_file.CreateTransFile(table, check_path);


            file_path = Path.Combine(SEND_FOLDER, @"SMDRCVL.DAT");
            check_path = Path.Combine(CHECK_FOLDER, @"SMDRCVL.DAT");
            trans_file = new HT_SMD_RCVLFile();
            table = smd.Run_sp_SMDRCVL(wspc.STR_NO);
            trans_file.CreateTransFile(table, check_path);


            CopDll.SetTransType(wspc.TRANS_TYPE);
            if (CopDll.Convert_SMDRCVHL() < 0)
            {
                //wspc.FLAG = "-1";
                //wspc.MSG = "產生SMDDPD.DAT、SMDRCVH.DAT、SMDRCVL.DAT錯誤";
                return DataStatus.Error;
            }

            check_path = Path.Combine(CHECK_FOLDER, wspc.TRANS_TYPE.ToUpper() == "PDA" ? @"SMDRCVHL.DB" : @"SMDDPD.DAT");

            pc_file_crt_date = File.GetLastWriteTime(check_path).ToString("yyyyMMdd_HHmmss");

            if (string.Compare(pc_file_crt_date, ht_file_crt_date) < 0)  //HT檔案比較新，不傳輸檔案
            {
                //wspc.FLAG = "5";
                //wspc.MSG = "不傳輸SMDDPD.DAT、SMDRCVH.DAT、SMDRCVL.DAT檔案";
                return DataStatus.NotSend;
            }

            //wspc.FLAG = "5";
            //wspc.MSG = "產生、傳輸SMDDPD.DAT、SMDRCVH.DAT、SMDRCVL.DAT檔案";
            return DataStatus.CreateAndSend;
        }
        #endregion

        #region 上傳檔案
        public void Upload_HT_DATA(WSParmContents wspc)
        {
            bool flag_3E = false; // 2022.08.05 mdho 3E有兩個檔，但是會一次跑完2個檔，判斷是否跑過。

            foreach(string file in wspc.UPLOAD_INFO.Keys)
            {
                switch (file.ToUpper())
                {
                    case "DELIV.DAT":
                    case "BACK.DAT":
                        if (flag_3E == false)
                        {
                            Insert_CSC_DELIV_3E(wspc);
                            Insert_CSC_BACK_3E(wspc);
                            SP_CSC_PDA_3E(wspc);
                            flag_3E = true;
                        }
                        break;
                    case "STOCK.DAT":
                        if (wspc.XSC == "SMD" || wspc.XSC_DATA == "7M")
                            Move_Upload_Data(wspc.MAC, file, wspc);
                        else if (wspc.XSC_DATA == "7C")
                            Insert_CSC_STOCK_7C(wspc);
                        else if (wspc.XSC_DATA == "1G61")
                            Insert_CSC_STOCK_1G61(wspc);
                        break;
                    case "ORDREASON.DAT": //功能廢棄
                        //Insert_ORDREASON(mac_folder);
                        break;
                    case "XMSBK.DAT":
                        string[] xsc_data = wspc.XSC_DATA.Split(',');
                        if (xsc_data.Length == 1)
                        {
                            if (xsc_data[0].Trim() == "XB52") Insert_XSC_XMSBK_XB52(wspc);
                        }
                        else if (xsc_data.Length == 2)
                        {
                            if (xsc_data[0].Trim() == "XB53") Insert_XSC_XMSBK_XB53(wspc);
                        }
                        else if (xsc_data.Length == 3)
                        {
                            if (xsc_data[0].Trim() == "XB6") Insert_XSC_XMSBK_XB6(wspc);
                        }
                        break;
                    case "XMSIN.DAT":
                        //Insert_XMSIN(mac_folder);
                        Insert_XSC_XMSIN_XB51(wspc);
                        break;
                    case "XMSSBK.DAT": //功能廢棄
                        //Insert_XMSSBK(mac_folder);
                        break;
                    case "SMDRCV.DAT":
                        //Insert_SMDRCV(mac_folder);
                        //Move_Upload_Data(wspc.MAC, file, wspc);
                        Insert_SMD_SMDRCV(wspc);
                        break;
                    case "MACID.DAT":
                        break;
                    default:
                        LOGGER.Write(LoggerLevel.ERROR, "[" + file + "] 上傳的檔名錯誤。");
                        Console.WriteLine("[" + file + "] 上傳的檔名錯誤。");
                        break;
                }
            }
        }

        private void Insert_CSC_DELIV_3E(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;

            if (!File.Exists(Path.Combine(RECV_FOLDER, mac, "DELIV.DAT")))
            {
                LOGGER.Write(LoggerLevel.WARNING, "DELIV.DAT 檔案不存在");
                Console.WriteLine("DELIV.DAT 檔案不存在");
                return;
            }

            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac, "DELIV.DAT"), FileMode.Open, FileAccess.Read); 
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 80; //DELIV.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;
            string guid = Guid.NewGuid().ToString().ToUpper();

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("CSC")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[DELIV.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            LOGGER.Write(LoggerLevel.WARNING, logMessage);
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[CSC_PDA_DELIV]
                                                       ([id], [str_no], [ht_id], [lono], [macid], [prdtcode], [ord_no], [pqty], [gqty], [stkdate], 
                                                        [casebar], [casepqty], [caseoqty], [casepcs], [create_datetime], [create_no], [flag])
                                                 VALUES
                                                       (@id, @str_no, @ht_id, @lono, @macid, @prdtcode, @ord_no, @pqty, @gqty, @stkdate,
                                                        @casebar, @casepqty, @caseoqty, @casepcs, getdate(), 'SYS_HTDTS', 0)";

                            comm.Parameters.Add("@id", SqlDbType.VarChar, 36).Value = guid;
                            comm.Parameters.Add("@str_no", SqlDbType.VarChar, 20).Value = str_no;
                            comm.Parameters.Add("@macid", SqlDbType.VarChar, 30).Value = mac;
                            comm.Parameters.Add("@ht_id", SqlDbType.VarChar, 2).Value = record.Substring(0, 2).Trim();
                            comm.Parameters.Add("@prdtcode", SqlDbType.VarChar, 13).Value = record.Substring(2, 13).Trim();
                            comm.Parameters.Add("@lono", SqlDbType.VarChar, 4).Value = record.Substring(15, 4).Trim();
                            comm.Parameters.Add("@ord_no", SqlDbType.VarChar, 8).Value = record.Substring(19, 8).Trim();
                            comm.Parameters.Add("@pqty", SqlDbType.Int).Value = Convert.ToInt32(record.Substring(27, 5).Trim());
                            comm.Parameters.Add("@gqty", SqlDbType.Int).Value = Convert.ToInt32(record.Substring(32, 5).Trim());
                            comm.Parameters.Add("@stkdate", SqlDbType.VarChar, 14).Value = record.Substring(37, 14).Trim();
                            comm.Parameters.Add("@casebar", SqlDbType.VarChar, 14).Value = record.Substring(51, 14).Trim();
                            comm.Parameters.Add("@casepqty", SqlDbType.Int).Value = Convert.ToInt32(record.Substring(65, 5).Trim());
                            comm.Parameters.Add("@caseoqty", SqlDbType.Int).Value = Convert.ToInt32(record.Substring(70, 5).Trim());
                            comm.Parameters.Add("@casepcs", SqlDbType.Int).Value = Convert.ToInt32(record.Substring(75, 5).Trim());

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[DELIV.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    LOGGER.Write(LoggerLevel.INFO, logMessage);
                    Console.WriteLine(logMessage);
                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void Insert_CSC_BACK_3E(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;

            if (!File.Exists(Path.Combine(RECV_FOLDER, mac, "BACK.DAT")))
            {
                LOGGER.Write(LoggerLevel.WARNING, "BACK.DAT 檔案不存在");
                Console.WriteLine("BACK.DAT 檔案不存在");
                return;
            }


            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac, "BACK.DAT"), FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 38; //BACK.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;
            string guid = Guid.NewGuid().ToString().ToUpper();

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("CSC")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[BACK.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            LOGGER.Write(LoggerLevel.WARNING, logMessage);
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[CSC_PDA_BACK]
                                                       ([id], [str_no], [ht_id], [prdtcode], [lono], [macid], [bqty], [stkdate], [create_datetime], [create_no], [flag])
                                                 VALUES
                                                       (@id, @str_no, @ht_id, @prdtcode, @lono, @macid, @bqty, @stkdate, getdate(), 'SYS_HTDTS', 0)";

                            comm.Parameters.Add("@id", SqlDbType.VarChar, 36).Value = guid;
                            comm.Parameters.Add("@str_no", SqlDbType.VarChar, 20).Value = str_no;
                            comm.Parameters.Add("@macid", SqlDbType.VarChar, 30).Value = mac;
                            comm.Parameters.Add("@ht_id", SqlDbType.VarChar, 2).Value = record.Substring(0, 2).Trim();
                            comm.Parameters.Add("@prdtcode", SqlDbType.VarChar, 13).Value = record.Substring(2, 13).Trim();
                            comm.Parameters.Add("@lono", SqlDbType.VarChar, 4).Value = record.Substring(15, 4).Trim();
                            comm.Parameters.Add("@bqty", SqlDbType.Int).Value = Convert.ToInt32(record.Substring(19, 5).Trim());
                            comm.Parameters.Add("@stkdate", SqlDbType.VarChar, 14).Value = record.Substring(24, 14).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[BACK.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    LOGGER.Write(LoggerLevel.INFO, logMessage);
                    Console.WriteLine(logMessage);
                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        private void SP_CSC_PDA_3E(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;
            
            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("CSC")))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "[dbo].[_SP_CSC_PDA_3E]";
                        comm.Parameters.Add("@STR_NO", SqlDbType.VarChar).Value = str_no;
                        comm.Parameters.Add("@MAC", SqlDbType.VarChar).Value = mac;

                        SqlDataReader sqlReader = comm.ExecuteReader(CommandBehavior.SingleRow);

                        if (sqlReader.HasRows)
                        {
                            sqlReader.Read();
                            wspc.FLAG = sqlReader["flag"].ToString();
                            wspc.MSG = sqlReader["msg"].ToString();
                        }

                        sqlReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

            }
        }

        private void Insert_CSC_STOCK_7C(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;

            if (!File.Exists(Path.Combine(RECV_FOLDER, mac, "STOCK.DAT")))
            {
                LOGGER.Write(LoggerLevel.WARNING, "STOCK.DAT 檔案不存在");
                Console.WriteLine("STOCK.DAT 檔案不存在");
                return;
            }


            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, wspc.MAC, "STOCK.DAT"), FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 47; //STOCK.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;
            string guid = Guid.NewGuid().ToString().ToUpper();

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("CSC")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[STOCK.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            LOGGER.Write(LoggerLevel.WARNING, logMessage);
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[CSC_PDA_STOCK_7C]
                                                       ([id], [str_no], [ht_id], [lono], [macid], [prdtcode], [qty], [stkdate], [create_datetime], [create_no], [flag])
                                                VALUES
                                                       (@id, @str_no, @ht_id, @lono, @macid, @prdtcode, @qty, @stkdate, getdate(), 'SYS_HTDTS', 0)";

                            comm.Parameters.Add("@id", SqlDbType.VarChar, 36).Value = guid;
                            comm.Parameters.Add("@str_no", SqlDbType.VarChar, 20).Value = str_no;
                            comm.Parameters.Add("@macid", SqlDbType.VarChar, 30).Value = mac;
                            comm.Parameters.Add("@ht_id", SqlDbType.VarChar).Value = record.Substring(0, 2).Trim();
                            comm.Parameters.Add("@lono", SqlDbType.VarChar).Value = record.Substring(2, 4).Trim();
                            comm.Parameters.Add("@prdtcode", SqlDbType.VarChar).Value = record.Substring(6, 13).Trim();
                            comm.Parameters.Add("@qty", SqlDbType.VarChar).Value = record.Substring(19, 5).Trim();
                            comm.Parameters.Add("@stkdate", SqlDbType.VarChar).Value = record.Substring(24, 14).Trim();
                            //comm.Parameters.Add("@盤點售價", SqlDbType.VarChar).Value = record.Substring(38, 8).Trim();
                            //comm.Parameters.Add("@生鮮註記", SqlDbType.VarChar).Value = record.Substring(46, 1).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[STOCK.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    LOGGER.Write(LoggerLevel.INFO, logMessage);
                    Console.WriteLine(logMessage);

                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "[dbo].[_SP_CSC_PDA_7C]";
                        comm.Parameters.Add("@STR_NO", SqlDbType.VarChar).Value = str_no;
                        comm.Parameters.Add("@MAC", SqlDbType.VarChar).Value = mac;

                        SqlDataReader sqlReader = comm.ExecuteReader(CommandBehavior.SingleRow);

                        if(sqlReader.HasRows)
                        {
                            sqlReader.Read();
                            wspc.FLAG = sqlReader["flag"].ToString();
                            wspc.MSG = sqlReader["msg"].ToString();
                        }

                        sqlReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        private void Insert_CSC_STOCK_1G61(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;
            
            if (!File.Exists(Path.Combine(RECV_FOLDER, mac, "STOCK.DAT")))
            {
                LOGGER.Write(LoggerLevel.WARNING, "STOCK.DAT 檔案不存在");
                Console.WriteLine("STOCK.DAT 檔案不存在");
                return;
            }

            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, wspc.MAC, "STOCK.DAT"), FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 47; //STOCK.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;
            string guid = Guid.NewGuid().ToString().ToUpper();

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("CSC")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[STOCK.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            LOGGER.Write(LoggerLevel.WARNING, logMessage);
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[CSC_PDA_STOCK_1G61]
                                                       ([id], [str_no], [ht_id], [lono], [macid], [prdtcode], [qty], [stkdate], [create_datetime], [create_no], [flag])
                                                 VALUES
                                                       (@id, @str_no, @ht_id, @lono, @macid, @prdtcode, @qty, @stkdate, getdate(), 'SYS_HTDTS', 0)";

                            comm.Parameters.Add("@id", SqlDbType.VarChar, 36).Value = guid;
                            comm.Parameters.Add("@str_no", SqlDbType.VarChar, 20).Value = str_no;
                            comm.Parameters.Add("@macid", SqlDbType.VarChar, 30).Value = mac;
                            comm.Parameters.Add("@ht_id", SqlDbType.VarChar).Value = record.Substring(0, 2).Trim();
                            comm.Parameters.Add("@lono", SqlDbType.VarChar).Value = record.Substring(2, 4).Trim();
                            comm.Parameters.Add("@prdtcode", SqlDbType.VarChar).Value = record.Substring(6, 13).Trim();
                            comm.Parameters.Add("@qty", SqlDbType.VarChar).Value = record.Substring(19, 5).Trim();
                            comm.Parameters.Add("@stkdate", SqlDbType.VarChar).Value = record.Substring(24, 14).Trim();
                            //comm.Parameters.Add("@盤點售價", SqlDbType.VarChar).Value = record.Substring(38, 8).Trim();
                            //comm.Parameters.Add("@生鮮註記", SqlDbType.VarChar).Value = record.Substring(46, 1).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[STOCK.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    LOGGER.Write(LoggerLevel.INFO, logMessage);
                    Console.WriteLine(logMessage);

                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "[dbo].[_SP_CSC_PDA_1G61]";
                        comm.Parameters.Add("@STR_NO", SqlDbType.VarChar).Value = str_no;
                        comm.Parameters.Add("@MAC", SqlDbType.VarChar).Value = mac;

                        SqlDataReader sqlReader = comm.ExecuteReader(CommandBehavior.SingleRow);

                        if (sqlReader.HasRows)
                        {
                            sqlReader.Read();
                            wspc.FLAG = sqlReader["flag"].ToString();
                            wspc.MSG = sqlReader["msg"].ToString();
                        }

                        sqlReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void Insert_XSC_XMSBK_XB52(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;
            
            if (!File.Exists(Path.Combine(RECV_FOLDER, mac, "XMSBK.DAT")))
            {
                LOGGER.Write(LoggerLevel.WARNING, "XMSBK.DAT 檔案不存在");
                Console.WriteLine("XMSBK.DAT 檔案不存在");
                return;
            }

            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac, "XMSBK.DAT"), FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 99; //XMSBK.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;
            string guid = Guid.NewGuid().ToString().ToUpper();

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("XSC")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[XMSBK.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            LOGGER.Write(LoggerLevel.WARNING, logMessage);
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[XMS_PDA_XMSBK_XB52]
                                                       ([id], [str_no], [itf_no], [item_no], [macid], [prdtcode], [plu_no], [csqty], [box_qty], 
                                                        [qty], [QNT_TYPE], [WORK_TYPE], [GOODS_TYPE], [stkdate], [create_datetime], [create_no], [flag])
                                                 VALUES
                                                       (@id, @str_no, @itf_no, @item_no, @macid, @prdtcode, @plu_no, @csqty, @box_qty, 
                                                        @qty, @QNT_TYPE, @WORK_TYPE, @GOODS_TYPE, @stkdate, getdate(), 'SYS_HTDTS', 0); ";

                            comm.Parameters.Add("@id", SqlDbType.VarChar, 36).Value = guid;
                            comm.Parameters.Add("@str_no", SqlDbType.VarChar, 20).Value = str_no;
                            comm.Parameters.Add("@macid", SqlDbType.VarChar, 30).Value = mac;
                            comm.Parameters.Add("@itf_no", SqlDbType.VarChar).Value = record.Substring(0, 14).Trim();
                            comm.Parameters.Add("@item_no", SqlDbType.VarChar).Value = record.Substring(14, 20).Trim();
                            comm.Parameters.Add("@prdtcode", SqlDbType.VarChar).Value = record.Substring(34, 13).Trim();
                            comm.Parameters.Add("@plu_no", SqlDbType.VarChar).Value = record.Substring(47, 13).Trim();
                            comm.Parameters.Add("@csqty", SqlDbType.VarChar).Value = record.Substring(60, 5).Trim();
                            comm.Parameters.Add("@box_qty", SqlDbType.VarChar).Value = record.Substring(65, 5).Trim();
                            comm.Parameters.Add("@qty", SqlDbType.VarChar).Value = record.Substring(70, 5).Trim();
                            comm.Parameters.Add("@QNT_TYPE", SqlDbType.VarChar).Value = record.Substring(75, 1).Trim();
                            comm.Parameters.Add("@WORK_TYPE", SqlDbType.VarChar).Value = record.Substring(76, 1).Trim();
                            comm.Parameters.Add("@GOODS_TYPE", SqlDbType.VarChar).Value = record.Substring(77, 8).Trim();
                            comm.Parameters.Add("@stkdate", SqlDbType.VarChar).Value = record.Substring(85, 14).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[XMSBK.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    LOGGER.Write(LoggerLevel.INFO, logMessage);
                    Console.WriteLine(logMessage);

                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "[dbo].[_SP_XMS_PDA_XB52]";
                        comm.Parameters.Add("@STR_NO", SqlDbType.VarChar).Value = str_no;
                        comm.Parameters.Add("@MAC", SqlDbType.VarChar).Value = mac;

                        SqlDataReader sqlReader = comm.ExecuteReader(CommandBehavior.SingleRow);

                        if (sqlReader.HasRows)
                        {
                            sqlReader.Read();
                            wspc.FLAG = sqlReader["flag"].ToString();
                            wspc.MSG = sqlReader["msg"].ToString();
                        }

                        sqlReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void Insert_XSC_XMSBK_XB53(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;
            string func = string.Empty;
            string trans_strno = string.Empty;

            string[] xsc_data = wspc.XSC_DATA.Trim().Split(',');
            if (xsc_data.Length == 2)
            {
                func = xsc_data[0];
                trans_strno = xsc_data[1];
            }
            else
            {
                LOGGER.Write(LoggerLevel.WARNING, "xsc_data參數錯誤！");
                Console.WriteLine("xsc_data參數錯誤！");
                wspc.FLAG = "-1";
                wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                return;
            }

            if (!File.Exists(Path.Combine(RECV_FOLDER, mac, "XMSBK.DAT")))
            {
                LOGGER.Write(LoggerLevel.WARNING, "XMSBK.DAT 檔案不存在");
                Console.WriteLine("XMSBK.DAT 檔案不存在");
                return;
            }

            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac, "XMSBK.DAT"), FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 99; //XMSBK.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;
            string guid = Guid.NewGuid().ToString().ToUpper();

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("XSC")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[XMSBK.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            LOGGER.Write(LoggerLevel.WARNING, logMessage);
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[XMS_PDA_XMSBK_XB53]
                                                       ([id], [str_no], [itfno], [plastno], [macid], [prdtcode], [plu_no], [csqty], [box_qty], 
                                                        [qty], [QNT_TYPE], [WORK_TYPE], [GOODS_TYPE], [stkdate], [create_datetime], [create_no], [flag])
                                                 VALUES
                                                       (@id, @str_no, @itfno, @plastno, @macid, @prdtcode, @plu_no, @csqty, @box_qty, 
                                                        @qty, @QNT_TYPE, @WORK_TYPE, @GOODS_TYPE, @stkdate, getdate(), 'SYS_HTDTS', 0); ";

                            comm.Parameters.Add("@id", SqlDbType.VarChar, 36).Value = guid;
                            comm.Parameters.Add("@str_no", SqlDbType.VarChar, 20).Value = str_no;
                            comm.Parameters.Add("@macid", SqlDbType.VarChar, 30).Value = mac;
                            comm.Parameters.Add("@itfno", SqlDbType.VarChar).Value = record.Substring(0, 14).Trim();
                            comm.Parameters.Add("@plastno", SqlDbType.VarChar).Value = record.Substring(14, 20).Trim();
                            comm.Parameters.Add("@prdtcode", SqlDbType.VarChar).Value = record.Substring(34, 13).Trim();
                            comm.Parameters.Add("@plu_no", SqlDbType.VarChar).Value = record.Substring(47, 13).Trim();
                            comm.Parameters.Add("@csqty", SqlDbType.VarChar).Value = record.Substring(60, 5).Trim();
                            comm.Parameters.Add("@box_qty", SqlDbType.VarChar).Value = record.Substring(65, 5).Trim();
                            comm.Parameters.Add("@qty", SqlDbType.VarChar).Value = record.Substring(70, 5).Trim();
                            comm.Parameters.Add("@QNT_TYPE", SqlDbType.VarChar).Value = record.Substring(75, 1).Trim();
                            comm.Parameters.Add("@WORK_TYPE", SqlDbType.VarChar).Value = record.Substring(76, 1).Trim();
                            comm.Parameters.Add("@GOODS_TYPE", SqlDbType.VarChar).Value = record.Substring(77, 8).Trim();
                            comm.Parameters.Add("@stkdate", SqlDbType.VarChar).Value = record.Substring(85, 14).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[XMSBK.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    LOGGER.Write(LoggerLevel.INFO, logMessage);
                    Console.WriteLine(logMessage);

                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "[dbo].[_SP_XMS_PDA_XB53]";
                        comm.Parameters.Add("@STR_NO", SqlDbType.VarChar).Value = str_no;
                        comm.Parameters.Add("@TRANS_STRNO", SqlDbType.VarChar).Value = trans_strno;
                        comm.Parameters.Add("@MAC", SqlDbType.VarChar).Value = mac;

                        SqlDataReader sqlReader = comm.ExecuteReader(CommandBehavior.SingleRow);

                        if (sqlReader.HasRows)
                        {
                            sqlReader.Read();
                            wspc.FLAG = sqlReader["flag"].ToString();
                            wspc.MSG = sqlReader["msg"].ToString();
                        }

                        sqlReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void Insert_XSC_XMSBK_XB6(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;
            string func = string.Empty;
            string prj_no = string.Empty;
            string res_no = string.Empty;

            string[] xsc_data = wspc.XSC_DATA.Trim().Split(',');
            if (xsc_data.Length == 3)
            {
                func = xsc_data[0];
                prj_no = xsc_data[1];
                res_no = xsc_data[2];
            }
            else
            {
                LOGGER.Write(LoggerLevel.WARNING, "xsc_data參數錯誤！");
                Console.WriteLine("xsc_data參數錯誤！");
                wspc.FLAG = "-1";
                wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                return;
            }

            if (!File.Exists(Path.Combine(RECV_FOLDER, mac, "XMSBK.DAT")))
            {
                LOGGER.Write(LoggerLevel.WARNING, "XMSBK.DAT 檔案不存在");
                Console.WriteLine("XMSBK.DAT 檔案不存在");
                return;
            }

            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac, "XMSBK.DAT"), FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 99; //XMSBK.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;
            string guid = Guid.NewGuid().ToString().ToUpper();

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("XSC")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[XMSBK.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            LOGGER.Write(LoggerLevel.WARNING, logMessage);
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[XMS_PDA_XMSBK_XB6]
                                                       ([id], [str_no], [itfno], [plastno], [macid], [prdtcode], [plu_no], [csqty], [box_qty], 
                                                        [qty], [QNT_TYPE], [WORK_TYPE], [GOODS_TYPE], [stkdate], [prj_Rtnreason_no], [create_datetime], [create_no], [flag])
                                                 VALUES
                                                       (@id, @str_no, @itfno, @plastno, @macid, @prdtcode, @plu_no, @csqty, @box_qty, 
                                                        @qty, @QNT_TYPE, @WORK_TYPE, @GOODS_TYPE, @stkdate, @prj_Rtnreason_no, getdate(), 'SYS_HTDTS', 0); ";

                            comm.Parameters.Add("@id", SqlDbType.VarChar, 36).Value = guid;
                            comm.Parameters.Add("@str_no", SqlDbType.VarChar, 20).Value = str_no;
                            comm.Parameters.Add("@macid", SqlDbType.VarChar, 30).Value = mac;
                            comm.Parameters.Add("@itfno", SqlDbType.VarChar).Value = record.Substring(0, 14).Trim();
                            comm.Parameters.Add("@plastno", SqlDbType.VarChar).Value = record.Substring(14, 20).Trim();
                            comm.Parameters.Add("@prdtcode", SqlDbType.VarChar).Value = record.Substring(34, 13).Trim();
                            comm.Parameters.Add("@plu_no", SqlDbType.VarChar).Value = record.Substring(47, 13).Trim();
                            comm.Parameters.Add("@csqty", SqlDbType.VarChar).Value = record.Substring(60, 5).Trim();
                            comm.Parameters.Add("@box_qty", SqlDbType.VarChar).Value = record.Substring(65, 5).Trim();
                            comm.Parameters.Add("@qty", SqlDbType.VarChar).Value = record.Substring(70, 5).Trim();
                            comm.Parameters.Add("@QNT_TYPE", SqlDbType.VarChar).Value = record.Substring(75, 1).Trim();
                            comm.Parameters.Add("@WORK_TYPE", SqlDbType.VarChar).Value = record.Substring(76, 1).Trim();
                            comm.Parameters.Add("@GOODS_TYPE", SqlDbType.VarChar).Value = record.Substring(77, 8).Trim();
                            comm.Parameters.Add("@stkdate", SqlDbType.VarChar).Value = record.Substring(85, 14).Trim();
                            comm.Parameters.Add("@prj_Rtnreason_no", SqlDbType.VarChar).Value = res_no;

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[XMSBK.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    LOGGER.Write(LoggerLevel.INFO, logMessage);
                    Console.WriteLine(logMessage);

                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "[dbo].[_SP_XMS_PDA_XB6]";
                        comm.Parameters.Add("@STR_NO", SqlDbType.VarChar).Value = str_no;
                        comm.Parameters.Add("@PRJ_NO", SqlDbType.VarChar).Value = prj_no;
                        comm.Parameters.Add("@RES_NO", SqlDbType.VarChar).Value = res_no;
                        comm.Parameters.Add("@MAC", SqlDbType.VarChar).Value = mac;

                        SqlDataReader sqlReader = comm.ExecuteReader(CommandBehavior.SingleRow);

                        if (sqlReader.HasRows)
                        {
                            sqlReader.Read();
                            wspc.FLAG = sqlReader["flag"].ToString();
                            wspc.MSG = sqlReader["msg"].ToString();
                        }

                        sqlReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void Insert_XSC_XMSIN_XB51(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;

            string[] xsc_data = wspc.XSC_DATA.Trim().Split(',');
            string do_no = string.Empty;
            string site_no = string.Empty;
            if (xsc_data.Length == 2)
            {
                do_no = xsc_data[0];
                site_no = xsc_data[1];
            }
            else
            {
                LOGGER.Write(LoggerLevel.WARNING, "xsc_data參數錯誤！");
                Console.WriteLine("xsc_data參數錯誤！");
                wspc.FLAG = "-1";
                wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                return;
            }

            string guid = Guid.NewGuid().ToString().ToUpper();

            if (!File.Exists(Path.Combine(RECV_FOLDER, mac, "XMSIN.DAT")))
            {
                LOGGER.Write(LoggerLevel.WARNING, "XMSIN.DAT 檔案不存在");
                Console.WriteLine("XMSIN.DAT 檔案不存在");
                return;
            }

            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac, "XMSIN.DAT"), FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 91; //XMSIN.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("XSC")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        record = record.Replace(",", ""); //發現檔案分隔有加逗號(,)
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[XMSIN.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            LOGGER.Write(LoggerLevel.WARNING, logMessage);
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[XMS_PDA_XMSIN]
                                                        ([id], [str_no], [do_no], [site_no], [macid], [itfno], [plastno], [prdtcode],
                                                        [pluno], [csqty], [boxqty], [prdtqty], [stkdate], [flag], [create_datetime], [create_no])
                                                 VALUES
                                                       (@id, @str_no, @do_no, @site_no, @macid, @itfno, @plastno, @prdtcode,
                                                        @pluno, @csqty, @boxqty, @prdtqty, @stkdate, '0', getdate(), 'SYS_HTDTS' );";

                            comm.Parameters.Add("@id", SqlDbType.VarChar, 36).Value = guid;
                            comm.Parameters.Add("@str_no", SqlDbType.VarChar, 20).Value = str_no;
                            comm.Parameters.Add("@do_no", SqlDbType.VarChar, 20).Value = do_no;
                            comm.Parameters.Add("@site_no", SqlDbType.VarChar, 6).Value = site_no;
                            comm.Parameters.Add("@macid", SqlDbType.VarChar, 30).Value = mac;
                            comm.Parameters.Add("@itfno", SqlDbType.VarChar, 14).Value = record.Substring(0, 14).Trim();
                            comm.Parameters.Add("@plastno", SqlDbType.VarChar, 20).Value = record.Substring(14, 20).Trim();
                            comm.Parameters.Add("@prdtcode", SqlDbType.VarChar, 13).Value = record.Substring(34, 13).Trim();
                            comm.Parameters.Add("@pluno", SqlDbType.VarChar, 13).Value = record.Substring(47, 13).Trim();
                            comm.Parameters.Add("@csqty", SqlDbType.VarChar, 5).Value = record.Substring(60, 5).Trim();
                            comm.Parameters.Add("@boxqty", SqlDbType.VarChar, 6).Value = record.Substring(65, 6).Trim();
                            comm.Parameters.Add("@prdtqty", SqlDbType.VarChar, 6).Value = record.Substring(71, 6).Trim();
                            comm.Parameters.Add("@stkdate", SqlDbType.VarChar, 14).Value = record.Substring(77, 14).Trim();
                            
                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[XMSIN.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    LOGGER.Write(LoggerLevel.INFO, logMessage);
                    Console.WriteLine(logMessage);

                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "[dbo].[_SP_XMS_PDA_XB51]";
                        comm.Parameters.Add("@STR_NO", SqlDbType.VarChar).Value = str_no;
                        comm.Parameters.Add("@MAC", SqlDbType.VarChar).Value = mac;

                        SqlDataReader sqlReader = comm.ExecuteReader(CommandBehavior.SingleRow);

                        if (sqlReader.HasRows)
                        {
                            sqlReader.Read();
                            wspc.FLAG = sqlReader["flag"].ToString();
                            wspc.MSG = sqlReader["msg"].ToString();
                        }

                        sqlReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void Insert_SMD_SMDRCV(WSParmContents wspc)
        {
            string str_no = wspc.STR_NO;
            string mac = wspc.MAC;
            string str_hid = string.Empty;

            if (!File.Exists(Path.Combine(RECV_FOLDER, mac, "SMDRCV.DAT")))
            {
                LOGGER.Write(LoggerLevel.WARNING, "SMDRCV.DAT 檔案不存在");
                Console.WriteLine("SMDRCV.DAT 檔案不存在");
                return;
            }

            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac, "SMDRCV.DAT"), FileMode.Open, FileAccess.Read); ;
            //StreamReader reader = new StreamReader(ifs);
            BinaryReader reader = new BinaryReader(ifs, Encoding.Default);
            string record = string.Empty;
            int recordLength = 71; //SMDRCV.DAT每行資料長度
            int recordCount = 0;
            int chunk = 0;
            string logMessage = string.Empty;
            byte[] buffer = new byte[recordLength];

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("SMD")))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = $"SELECT HID FROM [SMD].[dbo].[SMD_ORGANIZATION] WHERE ID = '{str_no}';";

                        str_hid = comm.ExecuteScalar().ToString().Trim();
                    }

                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = $"DELETE [SMD].[dbo].[SMD_SYS_TEMP_HT_RCV] WHERE [STR_HID] = '{str_hid}' AND [HT_ID] = '{wspc.GOTNO}' AND [D_DATE] = CONVERT(VARCHAR(10), GETDATE(), 120); ";

                        comm.ExecuteNonQuery();
                    }

                    //while ((record = reader.ReadLine()) != null)
                    while ((chunk = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        recordCount++;
                        if (buffer[recordLength - 2] != 0x0D && buffer[recordLength - 1] != 0x0A) //減2個字元 0x0D、0x0A
                        {
                            logMessage = "檔案：[SMDRCV.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                        INSERT INTO [dbo].[SMD_SYS_TEMP_HT_RCV]
                                            ([D_DATE], [STR_HID], [SEQ_ID], [HT_ID], [SELLER_ID], [PO_ID], [ITEM_ID], 
                                            [PROMO_PRICE], [PRICE_TYPE], [SPEC], [PRICE], [RCV_QTY], [QTY], [AMOUNT], 
                                            [HT_DATE], [UPD_BY], [UPD_DATE], [FLAG])
                                        VALUES
                                            (@D_DATE, @STR_HID, @SEQ_ID, @HT_ID, @SELLER_ID, @PO_ID, @ITEM_ID, 
                                                @PROMO_PRICE, @PRICE_TYPE, @SPEC, @PRICE, @RCV_QTY, @QTY, @AMOUNT, 
                                                @HT_DATE, @UPD_BY, GETDATE(), 0); ";

                            comm.Parameters.Add("@D_DATE", SqlDbType.SmallDateTime).Value = DateTime.Today;
                            comm.Parameters.Add("@STR_HID", SqlDbType.VarChar).Value = str_hid;
                            comm.Parameters.Add("@SEQ_ID", SqlDbType.Int).Value = recordCount;
                            comm.Parameters.Add("@HT_ID", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 0, 2).Trim();
                            comm.Parameters.Add("@SELLER_ID", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 2, 4).Trim();
                            comm.Parameters.Add("@PO_ID", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 6, 14).Trim();
                            comm.Parameters.Add("@ITEM_ID", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 20, 8).Trim();
                            comm.Parameters.Add("@PROMO_PRICE", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 28, 1).Trim();
                            string str_price_type = Encoding.Default.GetString(buffer, 29, 1).Trim();
                            comm.Parameters.Add("@PRICE_TYPE", SqlDbType.VarChar).Value = str_price_type;
                            comm.Parameters.Add("@SPEC", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 30, 8).Trim();
                            int price = int.Parse(Encoding.Default.GetString(buffer, 38, 4).Trim());
                            comm.Parameters.Add("@PRICE", SqlDbType.Int).Value = price;
                            int rcv_qty = int.Parse(Encoding.Default.GetString(buffer, 42, 5).Trim());
                            comm.Parameters.Add("@RCV_QTY", SqlDbType.Int).Value = rcv_qty;
                            int amount = int.Parse(Encoding.Default.GetString(buffer, 47, 8).Trim());
                            comm.Parameters.Add("@AMOUNT", SqlDbType.Int).Value = amount;
                            decimal qty = 0m;
                            if (str_price_type == "0")
                            {
                                qty = Math.Round((decimal)amount / price, 2);
                            }
                            else
                            {
                                qty = rcv_qty;
                            }

                            comm.Parameters.Add("@QTY", SqlDbType.Decimal).Value = qty;
                            string str_ht_date = Encoding.Default.GetString(buffer, 55, 14).Trim();
                            comm.Parameters.Add("@HT_DATE", SqlDbType.DateTime).Value = DateTime.ParseExact(str_ht_date, "yyyyMMddHHmmss", null);
                            comm.Parameters.Add("@UPD_BY", SqlDbType.VarChar).Value = str_no + "@HT";

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[SMDRCV.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    LOGGER.Write(LoggerLevel.INFO, logMessage);
                    Console.WriteLine(logMessage);

                    wspc.FLAG = "5";
                    wspc.MSG = "共" + recordCount + "筆資料。";

                    
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "[dbo].[spSMD_SYS_HT_UPLOAD_LOG]";
                        comm.Parameters.Add("@STR_HID", SqlDbType.VarChar).Value = str_hid;

                        comm.ExecuteNonQuery();
                        //SqlDataReader sqlReader = comm.ExecuteReader(CommandBehavior.SingleRow);

                        //if (sqlReader.HasRows)
                        //{
                        //    sqlReader.Read();
                        //    wspc.FLAG = sqlReader["flag"].ToString();
                        //    wspc.MSG = sqlReader["msg"].ToString();
                        //    wspc.FLAG = "5";
                        //    wspc.MSG = "共" + recordCount + "筆資料。";
                        //}

                        //sqlReader.Close();
                    }
                    

                }
                catch (Exception ex)
                {
                    LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                    Console.WriteLine(ex.Message);
                    wspc.FLAG = "-1";
                    wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }


        }

        private void Move_Upload_Data(string mac, string file, WSParmContents wspc)
        {
            try
            {
                if (File.Exists(Path.Combine(WSC_FOLDER, file))) File.Delete(Path.Combine(WSC_FOLDER, file));

                File.Copy(Path.Combine(RECV_FOLDER, mac, file), Path.Combine(WSC_FOLDER, file), true);
                wspc.FLAG = "5";
                wspc.MSG = "上傳資料成功。";
            }
            catch (Exception ex)
            {
                LOGGER.Write(LoggerLevel.ERROR, ex.Message);
                Console.WriteLine(ex.Message);
                wspc.FLAG = "-1";
                wspc.MSG = "1.上傳失敗，請聯絡資訊部！";
            }
        }
        #endregion

    }
}


/*
        private void Insert_XMSIN(string mac_folder)
        {

            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac_folder, "XMSIN.DAT"), FileMode.Open, FileAccess.Read); ;
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 109; //XMSIN.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("TEST")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        record = record.Replace(",", ""); //發現檔案分隔有加逗號(,)
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[XMSIN.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[tbHTDTS_XMSIN]
                                                        ([外箱條碼], [物流箱條碼], [貨號], [條碼], [入數],
                                                        [箱數], [總數量], [時間], [Do單號])
                                                    VALUES
                                                        (@外箱條碼, @物流箱條碼, @貨號, @條碼, @入數,
                                                        @箱數, @總數量, @時間, @Do單號)";

                            comm.Parameters.Add("@外箱條碼", SqlDbType.VarChar).Value = record.Substring(0, 14).Trim();
                            comm.Parameters.Add("@物流箱條碼", SqlDbType.VarChar).Value = record.Substring(14, 20).Trim();
                            comm.Parameters.Add("@貨號", SqlDbType.VarChar).Value = record.Substring(34, 13).Trim();
                            comm.Parameters.Add("@條碼", SqlDbType.VarChar).Value = record.Substring(47, 13).Trim();
                            comm.Parameters.Add("@入數", SqlDbType.VarChar).Value = record.Substring(60, 5).Trim();
                            comm.Parameters.Add("@箱數", SqlDbType.VarChar).Value = record.Substring(65, 5).Trim();
                            comm.Parameters.Add("@總數量", SqlDbType.VarChar).Value = record.Substring(70, 5).Trim();
                            comm.Parameters.Add("@時間", SqlDbType.VarChar).Value = record.Substring(75, 14).Trim();
                            comm.Parameters.Add("@Do單號", SqlDbType.VarChar).Value = record.Substring(89, 20).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[XMSIN.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    Console.WriteLine(logMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }


       private void Insert_SMDRCV(string mac_folder)
        {
            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac_folder, "SMDRCV.DAT"), FileMode.Open, FileAccess.Read); ;
            //StreamReader reader = new StreamReader(ifs);
            BinaryReader reader = new BinaryReader(ifs, Encoding.Default);
            string record = string.Empty;
            int recordLength = 71; //XMSSBK.DAT每行資料長度
            int recordCount = 0;
            int chunk = 0;
            string logMessage = string.Empty;
            byte[] buffer = new byte[recordLength];

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("TEST")))
            {
                try
                {
                    conn.Open();

                    //while ((record = reader.ReadLine()) != null)
                    while ((chunk = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        //if (record.Length != (recordLength))
                        if (buffer[recordLength - 2] != 0x0D && buffer[recordLength - 1] != 0x0A) //減2個字元 0x0D、0x0A
                        {
                            logMessage = "檔案：[SMDRCV.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[tbHTDTS_SMDRCV]
                                                    ([機台], [廠編], [單號], [管理碼], [通特賣], [計價方式]
                                                    ,[規格], [單售價], [實收量], [實收額], [作業時間])
                                                VALUES
                                                    (@機台, @廠編, @單號, @管理碼, @通特賣, @計價方式
                                                    ,@規格, @單售價, @實收量, @實收額, @作業時間); ";

                            comm.Parameters.Add("@機台", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 0, 2).Trim();
                            comm.Parameters.Add("@廠編", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 2, 4).Trim();
                            comm.Parameters.Add("@單號", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 6, 14).Trim();
                            comm.Parameters.Add("@管理碼", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 20, 8).Trim();
                            comm.Parameters.Add("@通特賣", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 28, 1).Trim();
                            comm.Parameters.Add("@計價方式", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 29, 1).Trim();
                            comm.Parameters.Add("@規格", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 30, 8).Trim();
                            comm.Parameters.Add("@單售價", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 38, 4).Trim();
                            comm.Parameters.Add("@實收量", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 42, 5).Trim();
                            comm.Parameters.Add("@實收額", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 47, 8).Trim();
                            comm.Parameters.Add("@作業時間", SqlDbType.VarChar).Value = Encoding.Default.GetString(buffer, 55, 14).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[SMDRCV.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    Console.WriteLine(logMessage);
                }
                catch (Exception ex)
                {
                    conn.Close();
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void Insert_ORDREASON(string mac_folder)
        {
            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac_folder, "ORDREASON.DAT"), FileMode.Open, FileAccess.Read); ;
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 52; //ORDREASON.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("TEST")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[ORDREASON.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[tbHTDTS_ORDREASON]
                                                       ([管理碼], [訂單編號], [訂量], [進量], [註記], [原因別], [時間])
                                                 VALUES
                                                       (@管理碼, @訂單編號, @訂量, @進量, @註記, @原因別, @時間)";

                            comm.Parameters.Add("@管理碼", SqlDbType.VarChar).Value = record.Substring(0, 13).Trim();
                            comm.Parameters.Add("@訂單編號", SqlDbType.VarChar).Value = record.Substring(13, 12).Trim();
                            comm.Parameters.Add("@訂量", SqlDbType.VarChar).Value = record.Substring(25, 5).Trim();
                            comm.Parameters.Add("@進量", SqlDbType.VarChar).Value = record.Substring(30, 5).Trim();
                            comm.Parameters.Add("@註記", SqlDbType.VarChar).Value = record.Substring(35, 2).Trim();
                            comm.Parameters.Add("@原因別", SqlDbType.VarChar).Value = record.Substring(37, 1).Trim();
                            comm.Parameters.Add("@時間", SqlDbType.VarChar).Value = record.Substring(38, 14).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[ORDREASON.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    Console.WriteLine(logMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void Insert_XMSSBK(string mac_folder)
        {
            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac_folder, "XMSSBK.DAT"), FileMode.Open, FileAccess.Read); ;
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 99; //XMSSBK.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("TEST")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[XMSSBK.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[tbHTDTS_XMSSBK]
                                                       ([外箱條碼], [號碼], [貨號], [條碼], [入數], [箱數],
                                                        [總數量], [類別], [退貨型態], [銷售金額], [時間])
                                                 VALUES
                                                       (@外箱條碼, @號碼, @貨號, @條碼, @入數, @箱數
                                                       ,@總數量, @類別, @退貨型態, @銷售金額, @時間)";

                            comm.Parameters.Add("@外箱條碼", SqlDbType.VarChar).Value = record.Substring(0, 14).Trim();
                            comm.Parameters.Add("@號碼", SqlDbType.VarChar).Value = record.Substring(14, 20).Trim();
                            comm.Parameters.Add("@貨號", SqlDbType.VarChar).Value = record.Substring(34, 13).Trim();
                            comm.Parameters.Add("@條碼", SqlDbType.VarChar).Value = record.Substring(47, 13).Trim();
                            comm.Parameters.Add("@入數", SqlDbType.VarChar).Value = record.Substring(60, 5).Trim();
                            comm.Parameters.Add("@箱數", SqlDbType.VarChar).Value = record.Substring(65, 5).Trim();
                            comm.Parameters.Add("@總數量", SqlDbType.VarChar).Value = record.Substring(70, 5).Trim();
                            comm.Parameters.Add("@類別", SqlDbType.VarChar).Value = record.Substring(75, 1).Trim();
                            comm.Parameters.Add("@退貨型態", SqlDbType.VarChar).Value = record.Substring(76, 1).Trim();
                            comm.Parameters.Add("@銷售金額", SqlDbType.VarChar).Value = record.Substring(77, 8).Trim();
                            comm.Parameters.Add("@時間", SqlDbType.VarChar).Value = record.Substring(85, 14).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[XMSSBK.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    Console.WriteLine(logMessage);
                }
                catch (Exception ex)
                {
                    conn.Close();
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void Insert_XSC_XMSBK(string mac_folder)
        {
            FileStream ifs = new FileStream(Path.Combine(RECV_FOLDER, mac_folder, "XMSBK.DAT"), FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(ifs);
            string record = string.Empty;
            int recordLength = 99; //XMSBK.DAT每行資料長度
            int recordCount = 0;
            string logMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(new ConfigCipher().Verify("TEST")))
            {
                try
                {
                    conn.Open();

                    while ((record = reader.ReadLine()) != null)
                    {
                        recordCount++;
                        //Console.WriteLine("record：" + record);
                        if (record.Length != (recordLength))
                        {
                            logMessage = "檔案：[XMSBK.DAT]-第[" + recordCount + "]筆資料長度錯誤";
                            Console.WriteLine(logMessage);
                            continue;
                        }
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = @"
                                                INSERT INTO [dbo].[tbHTDTS_XMSBK]
                                                       ([外箱條碼], [物流箱條碼], [貨號], [條碼], [入數], [箱數], 
                                                        [總數量], [類別], [退貨型態], [商品屬性], [時間])
                                                 VALUES
                                                       (@外箱條碼, @物流箱條碼, @貨號, @條碼, @入數, @箱數, 
                                                        @總數量, @類別, @退貨型態, @商品屬性, @時間)";

                            comm.Parameters.Add("@外箱條碼", SqlDbType.VarChar).Value = record.Substring(0, 14).Trim();
                            comm.Parameters.Add("@物流箱條碼", SqlDbType.VarChar).Value = record.Substring(14, 20).Trim();
                            comm.Parameters.Add("@貨號", SqlDbType.VarChar).Value = record.Substring(34, 13).Trim();
                            comm.Parameters.Add("@條碼", SqlDbType.VarChar).Value = record.Substring(47, 13).Trim();
                            comm.Parameters.Add("@入數", SqlDbType.VarChar).Value = record.Substring(60, 5).Trim();
                            comm.Parameters.Add("@箱數", SqlDbType.VarChar).Value = record.Substring(65, 5).Trim();
                            comm.Parameters.Add("@總數量", SqlDbType.VarChar).Value = record.Substring(70, 5).Trim();
                            comm.Parameters.Add("@類別", SqlDbType.VarChar).Value = record.Substring(75, 1).Trim();
                            comm.Parameters.Add("@退貨型態", SqlDbType.VarChar).Value = record.Substring(76, 1).Trim();
                            comm.Parameters.Add("@商品屬性", SqlDbType.VarChar).Value = record.Substring(77, 8).Trim();
                            comm.Parameters.Add("@時間", SqlDbType.VarChar).Value = record.Substring(85, 14).Trim();

                            comm.ExecuteNonQuery();
                        }
                    };
                    logMessage = "檔案：[XMSBK.DAT] - 轉檔完成，共" + recordCount + "筆資料。";
                    Console.WriteLine(logMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

*/
