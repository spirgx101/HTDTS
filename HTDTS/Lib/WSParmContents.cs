using System;
using System.Collections.Generic;

namespace HTDTS.Lib
{
    public class WSParmContents
    {
        private string str_no = string.Empty;
        private string ip = string.Empty;
        private string port = string.Empty;
        private string mac = string.Empty;
        private string fcode = string.Empty;
        private List<string> fcode_split = new List<string>();
        private string last_date = string.Empty;
        private List<string> last_date_split = new List<string>();
        private string file_name = string.Empty;
        private List<string> file_name_split = new List<string>();
        private string gotno = string.Empty;
        private string xsc = string.Empty;
        private string xsc_data = string.Empty;
        private string trans_type = string.Empty; //2021.09.08 mdho 新增傳輸類別：HT、PDA
        private string flag = string.Empty; // 2022.07.27 mdho 新增上傳回覆狀態
        private string msg = string.Empty;  // 2022.07.27 mdho 新增上傳回覆訊息
        //private Dictionary<string, string> fcode_info = new Dictionary<string, string>();
        private Dictionary<string, string> upload_info = new Dictionary<string, string>(); //Dictionary<file_name, last_date>
        private Dictionary<string, Tuple<string, string>> download_info = new Dictionary<string, Tuple<string, string>>(); //Dictionary<fcode, <file_name, last_date>>


        public string STR_NO
        {
            get { return str_no; }
            private set { str_no = value; }
        }

        public string IP
        {
            get { return ip; }
            private set { ip = value; }
        }

        public string PORT
        {
            get { return port; }
            private set { port = value; }
        }

        public string MAC
        {
            get { return mac; }
            private set { mac = value; }
        }

        public string FCODE
        {
            get
            {
                return fcode;
            }
            private set { fcode = value; }
        }

        public string LAST_DATE
        {
            get { return last_date; }
            set { last_date = value; }
        }

        public string FILE_NAME
        {
            get { return file_name; }
            set { file_name = value; }
        }

        public string GOTNO
        {
            get { return gotno; }
            set { gotno = value; }
        }

        public string XSC
        {
            get { return xsc; }
            private set { xsc = value; }
        }

        public string XSC_DATA
        {
            get { return xsc_data; }
            private set { xsc_data = value; }
        }

        public string TRANS_TYPE
        {
            get { return trans_type; }
            private set { trans_type = value; }
        }

        public string FLAG
        {
            get { return flag; }
            set { flag = value; }
        }

        public string MSG
        {
            get { return msg; }
            set { msg = value; }
        }

        public List<string> FCODE_LIST
        {
            get
            {
                return fcode_split;
            }
        }

        public Dictionary<string, string> UPLOAD_INFO
        {
            get
            {
                return upload_info;
            }
        }

        public Dictionary<string, Tuple<string, string>> DOWNLOAD_INFO
        {
            get
            {
                return download_info;
            }
            set
            {
                download_info = value;
            }
        }
            



        public WSParmContents(string[] args)
        {
            str_no = args[0];
            ip = args[1];
            port = args[2];
            mac = args[3].ToUpper();
            fcode = args[4];
            fcode_split = new List<string>(args[4].Split(','));
            last_date = args[5];
            last_date_split = new List<string>(args[5].Split(','));
            file_name = args[6].ToUpper();
            file_name_split = new List<string>(args[6].Split(','));
            gotno = args[7];
            xsc = args[8].ToUpper();
            xsc_data = args[9].ToUpper();
            trans_type = args[10].ToUpper();
            flag = args[11];
            msg = args[12];

            if (fcode_split.Count == 1 && fcode == "4")
            {
                for (int i = 0; i < file_name_split.Count; i++)
                {
                    upload_info.Add(file_name_split[i], last_date_split[i]);
                }
            }
            else
            {
                for (int i = 0; i < fcode_split.Count; i++)
                {
                    download_info.Add(fcode_split[i],
                        new Tuple<string, string>(file_name_split[i], last_date_split[i]));
                }
            }
        }

        public string Get_Args_String()
        {
            return $@"STR_NO={STR_NO},IP={IP},PORT={PORT},MAC={MAC},FCODE={FCODE},LAST_DAT={LAST_DATE},FILE_NAME={FILE_NAME},GOTNO={GOTNO},XSC={XSC},XSC_DATA={XSC_DATA},TRANAS_TYPE={TRANS_TYPE},FLAG={FLAG},MSG={MSG}";
        }

        public string Get_Parm_String()
        {
            //return $@"{STR_NO} {IP} {PORT} {MAC} {FCODE} {LAST_DATE} {FILE_NAME} {GOTNO} {XSC} {XSC_DATA} {TRANS_TYPE}";

            
            string str_fcode = string.Empty;
            string str_file_name = string.Empty;
            string str_last_date = string.Empty;

            if (fcode_split.Count == 1 && fcode == "4")
            {
                return $@"{STR_NO} {IP} {PORT} {MAC} {FCODE} {LAST_DATE} {FILE_NAME} {GOTNO} {XSC} {XSC_DATA} {TRANS_TYPE} {FLAG} {MSG}";
            }
            else
            {
                foreach (KeyValuePair<string, Tuple<string, string>> info in DOWNLOAD_INFO)
                {
                    str_fcode = str_fcode + info.Key + ",";
                    str_file_name = str_file_name + info.Value.Item1 + ",";
                    str_last_date = str_last_date + info.Value.Item2 + ",";
                }

                str_fcode = str_fcode.Substring(0, str_fcode.Length - 1);
                str_file_name = str_file_name.Substring(0, str_file_name.Length - 1);
                str_last_date = str_last_date.Substring(0, str_last_date.Length - 1);

                return $@"{STR_NO} {IP} {PORT} {MAC} {str_fcode} {str_last_date} {str_file_name} {GOTNO} {XSC} {XSC_DATA} {TRANS_TYPE} {FLAG} {MSG}";
            }
            
        }



        /*
        public string Get_Socket_Parm_String()
        {
            string FCODE = string.Empty;
            string LAST_DATE = string.Empty;
            string FILE_NAME = string.Empty;

            foreach (KeyValuePair<string, Tuple<string, string>> info in FCODE_INFO)
            {
                FCODE = FCODE + info.Key + ",";
                LAST_DATE = LAST_DATE + info.Value.Item1 + ",";
                FILE_NAME = FILE_NAME + info.Value.Item2 + ",";
            }

            FCODE = FCODE.Substring(0, FCODE.Length - 1);
            LAST_DATE = LAST_DATE.Substring(0, LAST_DATE.Length - 1);
            FILE_NAME = FILE_NAME.Substring(0, FILE_NAME.Length - 1);

            return STR_NO + " " + IP + " " + PORT + " " + MAC + " " + FCODE + " " +
                   LAST_DATE + " " + FILE_NAME + " " + GOTNO + " " + TRANS_TYPE;
        }
        */
    }
}
