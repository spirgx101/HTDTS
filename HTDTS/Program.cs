using Arktech;
using HTDTS.Lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace HTDTS
{
    class Program
    {
        private static Mutex RUN_MUTEX;
        private static readonly string SEND_FOLDER = ConfigurationManager.AppSettings["SEND"].ToString();
        private static readonly string RECV_FOLDER = ConfigurationManager.AppSettings["RECV"].ToString();
        private static readonly string CHECK_FOLDER = ConfigurationManager.AppSettings["CHECK"].ToString();
        private static readonly string WSC_FOLDER = ConfigurationManager.AppSettings["WSC"].ToString();
        private static Logger logger = new Logger();

        static void Main(string[] args)
        {
            /* 
            * 2021.09.08 mdho 新增傳輸類別(TRANS_TYPE)：HT、PDA
            * (01) STR_NO：門市代號
            * (02) IP：HT-IP
            * (03) PORT：HT-PORT
            * (04) MAC：HT-MAC
            * (05) FCODE             
            * (06) LAST_DATE 
            * (07) FILE_NAME
            * (08) GOTNO                    
            * (09) XSC
            * (10) XSC_DATA
            * (11) TRANS_TYPE：傳輸類別
            * (12) FLAG：上傳回覆狀態
            * (13) MSG：上傳回覆訊息
           */
            logger.FilePath = Path.Combine(WSC_FOLDER, "HTDTS");
            logger.MacPath = args[3];
            logger.Level = LoggerLevel.INFO;

            if (args.Length != 13)
            {
                logger.Write(LoggerLevel.ERROR, "輸入參數不符。" + args.Length.ToString());
                Console.WriteLine("輸入參數不符。" + args.Length.ToString());
                Environment.Exit(0);
            }

            WSParmContents wspc = new WSParmContents(args);

            //Console.WriteLine( wspc.Get_Parm_String());
            logger.Write(LoggerLevel.INFO, wspc.Get_Args_String());
            
            bool bCreatedNew;
            RUN_MUTEX = new Mutex(false, "mutexHTDTSAccess", out bCreatedNew);  
                    
            RUN_MUTEX.WaitOne();
            GC.Collect();

            if (!Directory.Exists(SEND_FOLDER)) Directory.CreateDirectory(SEND_FOLDER);
            if (!Directory.Exists(RECV_FOLDER)) Directory.CreateDirectory(RECV_FOLDER);
            if (!Directory.Exists(CHECK_FOLDER)) Directory.CreateDirectory(CHECK_FOLDER);

            HT_DATA_Process(wspc);

            RUN_MUTEX.ReleaseMutex();
            RUN_MUTEX.Dispose();

            //if (!(wspc.MAC.ToUpper().Trim() == "AUTO" || wspc.FCODE.Trim() == "4")) Run_Socket_Process(wspc);
            if (wspc.MAC.ToUpper().Trim() != "AUTO") Run_Socket_Process(wspc);
            //Console.ReadKey();
        }

        private static void HT_DATA_Process(WSParmContents wspc)
        {
            List<string> remove_list = new List<string>();
            HT_DATA ht_data = new HT_DATA(logger);

            DataStatus status = 0;
            foreach (string fcode in wspc.FCODE_LIST)
            {
                switch (fcode)
                {
                    case "0": /* 產生 XMSDO.DAT 檔案 */
                        status = ht_data.Gen_XSC_XMSDO(wspc, fcode);
                        if (status == DataStatus.NotSend)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.NotCreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.CreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.Error)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.ERROR, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        break;
                    case "1": /* 產生 DPPRD.DAT、ODPPRD.DAT 檔案 */
                        status = ht_data.Gen_CSC_DPPRD(wspc, fcode);
                        if (status == DataStatus.NotSend)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.NotCreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.CreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.Error)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.ERROR, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        break;
                    case "2": /* 產生 INSTANT.DAT 檔案 */
                        status = ht_data.Gen_CSC_INSTANT(wspc, fcode);
                        if (status == DataStatus.NotSend)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.NotCreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.CreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.Error)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.ERROR, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        break;
                    case "3":
                        break;
                    case "4":
                        ht_data.Upload_HT_DATA(wspc);
                        break;
                    case "5":
                        status = ht_data.Gen_SMD_DPPRD(wspc, fcode);
                        if (status == DataStatus.NotSend)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.NotCreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.CreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.Error)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.ERROR, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        break;
                    case "6":
                        status = ht_data.Gen_SMD_RCVHL(wspc, fcode);
                        if (status == DataStatus.NotSend)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.NotCreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.CreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.Error)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.ERROR, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        break;
                    case "7": /* 產生 BKMOENY.DAT 檔案 */

                        break;
                    case "8": /* 產生 DPORD.DAT 檔案 */
                        status = ht_data.Gen_XSC_DPORD(wspc, fcode);
                        if (status == DataStatus.NotSend)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.NotCreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.CreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.Error)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.ERROR, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        break;
                    case "9": /* 產生 PRODBOX.DAT 檔案 */
                        status = ht_data.Gen_CSC_PRODBOX(wspc, fcode);
                        if (status == DataStatus.NotSend)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.NotCreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.CreateAndSend)
                        {
                            logger.Write(LoggerLevel.INFO, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        else if (status == DataStatus.Error)
                        {
                            remove_list.Add(fcode);
                            logger.Write(LoggerLevel.ERROR, wspc.MSG);
                            Console.WriteLine(wspc.MSG);
                        }
                        break;
                }
            }

            if (wspc.DOWNLOAD_INFO.Count != remove_list.Count)
            {
                foreach (string fcode in remove_list) wspc.DOWNLOAD_INFO.Remove(fcode);
            }
            if (wspc.TRANS_TYPE.ToUpper() == "PDA") CopDll.CreatePdaVersionFile();

            Copy_All_File();

        }

        private static void Copy_All_File()
        {
            try
            {
                string[] fileNames = Directory.GetFiles(CHECK_FOLDER);

                foreach (string file in fileNames)
                    File.Copy(file, Path.Combine(SEND_FOLDER, Path.GetFileName(file)), true);

                foreach (string file in fileNames)
                    File.Delete(file);

                Directory.Delete(CHECK_FOLDER);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Run_Socket_Process(WSParmContents wspc)
        {
            //Console.WriteLine(wspc.Get_Parm_String());
            string exeFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Socket.exe");
            if (File.Exists(exeFile))
            {
                System.Diagnostics.Process socket = new System.Diagnostics.Process();
                socket.StartInfo.FileName = exeFile;
                socket.StartInfo.Arguments = wspc.Get_Parm_String();
                socket.Start();
                socket.WaitForExit();
                socket.Close();
                socket.Dispose();
            }

        }
    }
}
