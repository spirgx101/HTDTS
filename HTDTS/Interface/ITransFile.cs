using System.Data;
using System.IO;
using HTDTS.Lib;
using System;

namespace HTDTS.Interface
{
    interface ITransFile
    {
        bool CreateTransFile(DataTable file_data, string file_path);
    }

    #region HT Create Trans-File
    public class HT_XSC_XMSDOFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            writer.Write(dr["do_no"].ToString().PadRight(20, ' '));
                            writer.Write(dr["cag_no"].ToString().PadRight(20, ' '));
                            writer.Write(dr["type"].ToString());
                            writer.Write(dr["plu_no"].ToString().PadRight(14, ' '));
                            writer.Write(dr["entsqty"].ToString().PadRight(3, ' '));
                            writer.Write(dr["boxqty"].ToString().PadRight(3, ' '));
                            writer.Write(dr["outqty"].ToString().PadRight(6, ' '));
                            writer.Write(dr["AAREAId"].ToString().PadRight(5, ' '));
                            writer.Write("\n");
                        }
                    }
                }

                is_success = true;
            }
            catch
            {
                is_success = false;
            }

            return is_success;
        }
    }

    public class HT_CSC_DPPRDFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows) writer.WriteLine(dr["flatString"].ToString());
                    }
                }
                is_success = true;
            }
            catch
            {
                is_success = false;
            }

            return is_success;
        }
    }

    public class HT_CSC_ODPPRDFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows) writer.WriteLine(dr["flatString"].ToString());
                    }
                }
                is_success = true;
            }
            catch
            {
                is_success = false;
            }

            return is_success;
        }
    }

    public class HT_CSC_INSTANTFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows) writer.WriteLine(dr["flatString"].ToString());
                    }
                }
                is_success = true;
            }
            catch
            {
                is_success = false;
            }

            return is_success;
        }
    }

    public class HT_CSC_PRODBOXFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows) writer.WriteLine(dr["flatString"].ToString());
                    }
                }
                is_success = true;
            }
            catch
            {
                is_success = false;
            }

            return is_success;
        }
    }

    public class HT_XSC_DPORDFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            writer.Write(dr["ntprdtcode"].ToString().PadRight(13, ' '));
                            writer.Write(dr["ntid"].ToString().PadRight(12, ' '));
                            writer.Write(dr["ntqty"].ToString().PadLeft(5, ' '));
                            writer.Write(dr["remark"].ToString().PadRight(2, ' '));
                            writer.Write(dr["cause"].ToString());
                            writer.Write("\n");
                        }
                    }
                }
            
                is_success = true;
            }
            catch
            {
                is_success = false;                
            }

            return is_success;
        }
    }

    public class HT_SMD_DPPRDFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            string flatString = string.Empty;

                            flatString += dr["D1"].ToString().Trim().Alignment(13);
                            flatString += dr["D2"].ToString().Trim().Alignment(13);
                            flatString += dr["D3"].ToString().Trim().Alignment(13);
                            flatString += dr["D4"].ToString().Trim().Alignment(8);
                            flatString += dr["D5"].ToString().Trim().Alignment(2);
                            flatString += dr["D6"].ToString().Trim().Alignment(6);
                            flatString += dr["D7"].ToString().Trim().Alignment(2);
                            flatString += dr["D8"].ToString().Trim().Alignment(1);
                            flatString += dr["D9"].ToString().Trim().Alignment(4);
                            flatString += dr["D10"].ToString().Trim().Alignment(8);
                            flatString += dr["D11"].ToString().Trim().Alignment(5);
                            flatString += dr["D12"].ToString().Trim().Alignment(1);
                            flatString += dr["D13"].ToString().Trim().Alignment(1);

                            /*
                            flatString += dr["D1"].ToString();
                            flatString += dr["D2"].ToString();
                            flatString += dr["D3"].ToString();
                            flatString += dr["D4"].ToString();
                            flatString += dr["D5"].ToString();
                            flatString += dr["D6"].ToString();
                            flatString += dr["D7"].ToString();
                            flatString += dr["D8"].ToString();
                            flatString += dr["D9"].ToString();
                            flatString += dr["D10"].ToString();
                            flatString += dr["D11"].ToString();
                            flatString += dr["D12"].ToString();
                            flatString += dr["D13"].ToString();
                            */
                            writer.WriteLine(flatString);

                        }
                    }
                }
                is_success = true;
            }
            catch
            {
                is_success = false;
            }

            return is_success;
        }
    }

    public class HT_SMD_ITEMFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            string flatString = string.Empty;



                            flatString += dr["D1"].ToString().Trim().Alignment(13);
                            flatString += dr["D2"].ToString().Trim().Alignment(8);
                            flatString += dr["D3"].ToString().Trim().Alignment(6);
                            flatString += dr["D4"].ToString().Trim().Alignment(16);
                            flatString += dr["D5"].ToString().Trim().Alignment(1);
                            flatString += dr["D6"].ToString().Trim().Alignment(1);
                            flatString += dr["D7"].ToString().Trim().Alignment(8);
                            flatString += dr["D8"].ToString().Trim().Alignment(4);
                            flatString += dr["D9"].ToString().Trim().Alignment(4);
                            flatString += dr["D10"].ToString().Trim().Alignment(4);
                            flatString += dr["D11"].ToString().Trim().Alignment(4);
                            flatString += dr["D12"].ToString().Trim().Alignment(3);
                            flatString += dr["D13"].ToString().Trim().Alignment(3);
                            flatString += dr["D14"].ToString().Trim().Alignment(3);
                            flatString += dr["D15"].ToString().Trim().Alignment(3);

                            /*
                            flatString += dr["D1"].ToString();
                                                       flatString += dr["D2"].ToString();
                                                       flatString += dr["D3"].ToString();
                                                       flatString += dr["D4"].ToString();
                                                       flatString += dr["D5"].ToString();
                                                       flatString += dr["D6"].ToString();
                                                       flatString += dr["D7"].ToString();
                                                       flatString += dr["D8"].ToString();
                                                       flatString += dr["D9"].ToString();
                                                       flatString += dr["D10"].ToString();
                                                       flatString += dr["D11"].ToString();
                                                       flatString += dr["D12"].ToString();
                                                       flatString += dr["D13"].ToString();
                                                       flatString += dr["D14"].ToString();
                            */
                            writer.WriteLine(flatString);
                        }
                    }
                }
                is_success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                is_success = false;
            }

            return is_success;
        }
    }

    public class HT_SMD_DPDFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            string flatString = string.Empty;

                            flatString += dr["D1"].ToString().Trim().Alignment(1);
                            flatString += dr["D2"].ToString().Trim().Alignment(8);

                            writer.WriteLine(flatString);
                        }
                    }
                }
                is_success = true;
            }
            catch
            {
                is_success = false;
            }

            return is_success;
        }
    }

    public class HT_SMD_RCVHFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            string flatString = string.Empty;

                            flatString += dr["D1"].ToString().Trim().Alignment(4);
                            flatString += dr["D2"].ToString().Trim().Alignment(6);
                            flatString += dr["D3"].ToString().Trim().Alignment(1);
                            flatString += dr["D4"].ToString().Trim().Alignment(14);
                            flatString += dr["D5"].ToString().Trim().Alignment(4);
                            flatString += dr["D6"].ToString().Trim().Alignment(4);
                            flatString += dr["D7"].ToString().Trim().Alignment(4);
                            flatString += dr["D8"].ToString().Trim().Alignment(4);

                            /*
                            flatString += dr["D1"].ToString();
                            flatString += dr["D2"].ToString();
                            flatString += dr["D3"].ToString();
                            flatString += dr["D4"].ToString();
                            flatString += dr["D5"].ToString();
                            flatString += dr["D6"].ToString();
                            flatString += dr["D7"].ToString();
                            flatString += dr["D8"].ToString();
                            */
                            writer.WriteLine(flatString);
                        }
                    }
                }
                is_success = true;
            }
            catch
            {
                is_success = false;
            }

            return is_success;
        }
    }

    public class HT_SMD_RCVLFile : ITransFile
    {
        public bool CreateTransFile(DataTable table, string file_path)
        {
            bool is_success = false;

            try
            {
                using (FileStream file = new FileStream(file_path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.GetEncoding(950)))
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            string flatString = string.Empty;

                            flatString += dr["D1"].ToString().Trim().Alignment(14);
                            flatString += dr["D2"].ToString().Trim().Alignment(13);
                            flatString += dr["D3"].ToString().Trim().Alignment(8);
                            flatString += dr["D4"].ToString().Trim().Alignment(4);
                            flatString += dr["D5"].ToString().Trim().Alignment(4);

                            /*
                            flatString += dr["D1"].ToString();
                            flatString += dr["D2"].ToString();
                            flatString += dr["D3"].ToString();
                            flatString += dr["D4"].ToString();
                            flatString += dr["D5"].ToString();
                            */
                            writer.WriteLine(flatString);
                        }
                    }
                }
                is_success = true;
            }
            catch
            {
                is_success = false;
            }

            return is_success;
        }
    }

    #endregion

    /*
        #region PDA Create Trans_File
        public class PDADPPRDFile : ITransFile
        {
            public bool CreateTransFile(DataTable table, string file_path)
            {
                bool is_success = false;

                try
                {
                    if (File.Exists(file_path)) File.Delete(file_path);

                    using (IDbConnection cn = new SQLiteConnection(@"data source=" + file_path))
                    {
                        cn.Open();

                        cn.Execute(@"
                            CREATE TABLE DPPRD (
                                vcitocod  char(13),
                                prdtcode  char(13),
                                plu_no    char(13),
                                dockcode  char(8),
                                vcitqty   char(2),
                                prdtslpr  char(8),
                                prdtcisd  char(1),
                                batno     char(4),
                                indc      char(8),
                                vcittype  char(5),
                                onpack    char(1),
                                freshfood char(1)
                            );
                        ");


                        using (var tran = cn.BeginTransaction())
                        {
                            foreach (DataRow dr in table.Rows)
                            {   
                                string cmd = @"
                                    INSERT INTO DPPRD VALUES (@vcitocod, @prdtcode, @plu_no, @dockcode, 
                                        @vcitqty, @prdtslpr, @prdtcisd, @batno, @indc, @vcittype, @onpack, @freshfood) ";
                                DPPRD dpprd = new DPPRD(dr["flatString"].ToString().Substring(0, 13),
                                                            dr["flatString"].ToString().Substring(13, 13),
                                                            dr["flatString"].ToString().Substring(26, 13),
                                                            dr["flatString"].ToString().Substring(39, 8),
                                                            dr["flatString"].ToString().Substring(47, 2),
                                                            dr["flatString"].ToString().Substring(49, 8),
                                                            dr["flatString"].ToString().Substring(57, 1),
                                                            dr["flatString"].ToString().Substring(58, 4),
                                                            dr["flatString"].ToString().Substring(62, 8),
                                                            dr["flatString"].ToString().Substring(70, 5),
                                                            dr["flatString"].ToString().Substring(75, 1),
                                                            dr["flatString"].ToString().Substring(76, 1));

                                cn.Execute(cmd, dpprd);     
                            }

                            tran.Commit();
                        }
                    }

                    is_success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    is_success = false;
                }

                return is_success;
            }
        }

        public class PDAODPPRDFile : ITransFile
        {
            public bool CreateTransFile(DataTable table, string file_path)
            {
                bool is_success = false;

                try
                {
                    if (File.Exists(file_path)) File.Delete(file_path);

                    using (IDbConnection cn = new SQLiteConnection(@"data source=" + file_path))
                    {
                        cn.Open();

                        cn.Execute(@"
                            CREATE TABLE ODPPRD (
                                vcitocod  char(13),
                                prdtcode  char(13),
                                plu_no    char(13),
                                space     char(1),
                                indc1     char(1),
                                indc2     char(1),
                                sup_no    char(5),
                                vcitqty   char(2),
                                prdtslpr  char(8),
                                prdtcisd  char(1),
                                prdtmlqy  char(7),
                                prdtmiqy  char(7)
                            );
                        ");


                        using (var tran = cn.BeginTransaction())
                        {
                            foreach (DataRow dr in table.Rows)
                            {                      
                                string cmd = @"
                                    INSERT INTO ODPPRD VALUES (@vcitocod, @prdtcode, @plu_no, @space, @indc1, @indc2, 
                                        @sup_no, @vcitqty, @prdtslpr, @prdtcisd, @prdtmlqy, @prdtmiqy)  ";
                                ODPPRD odpprd = new ODPPRD(dr["flatString"].ToString().Substring(0, 13),
                                                            dr["flatString"].ToString().Substring(13, 13),
                                                            dr["flatString"].ToString().Substring(26, 13),
                                                            dr["flatString"].ToString().Substring(39, 1),
                                                            dr["flatString"].ToString().Substring(40, 1),
                                                            dr["flatString"].ToString().Substring(41, 1),
                                                            dr["flatString"].ToString().Substring(42, 5),
                                                            dr["flatString"].ToString().Substring(47, 2),
                                                            dr["flatString"].ToString().Substring(49, 8),
                                                            dr["flatString"].ToString().Substring(57, 1),
                                                            dr["flatString"].ToString().Substring(58, 7),
                                                            dr["flatString"].ToString().Substring(65, 7));                
                                cn.Execute(cmd, odpprd);    
                            }

                            tran.Commit();
                        }
                    }

                    is_success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    is_success = false;
                }

                return is_success;
            }
        }



        public class PDAINSTANTFile : ITransFile
        {
            public bool CreateTransFile(DataTable table, string file_path)
            {
                bool is_success = false;

                try
                {
                    if (File.Exists(file_path)) File.Delete(file_path);

                    using (IDbConnection cn = new SQLiteConnection(@"data source=" + file_path))
                    {
                        cn.Open();

                        cn.Execute(@"
                            CREATE TABLE INSTANT (
                                prdtcode  char(13),
                                prdtmpqy  char(5),
                                prdtmlqy  char(7),
                                prdtmiqy  char(7)
                            );
                        ");

                        using (var tran = cn.BeginTransaction())
                        {
                            foreach (DataRow dr in table.Rows)
                            {
                                string cmd = @"
                                    INSERT INTO INSTANT VALUES (@prdtcode, @prdtmpqy, @prdtmlqy, @prdtmiqy) ";
                                INSTANT instant = new INSTANT(dr["flatString"].ToString().Substring(0, 13),
                                                                dr["flatString"].ToString().Substring(13, 5),
                                                                dr["flatString"].ToString().Substring(18, 7),
                                                                dr["flatString"].ToString().Substring(25, 7));

                                cn.Execute(cmd, instant);
                            }

                            tran.Commit();
                        }
                    }

                    is_success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    is_success = false;
                }

                return is_success;
            }
        }




        public class PDAPRODBOXFile : ITransFile
        {
            public bool CreateTransFile(DataTable table, string file_path)
            {
                bool is_success = false;

                try
                {
                    if (File.Exists(file_path)) File.Delete(file_path);

                    using (IDbConnection cn = new SQLiteConnection(@"data source=" + file_path))
                    {
                        cn.Open();

                        cn.Execute(@"
                             CREATE TABLE PRODBOX (
                                goo_no    char(13),
                                plu_no    char(13),
                                itf       char(14),
                                cs_qty    char(5),
                                remark1   char(1),
                                remark2   char(1)
                            );
                        ");

                        using (var tran = cn.BeginTransaction())
                        {
                            foreach (DataRow dr in table.Rows)
                            {
                                var cmd = @"
                                    INSERT INTO PRODBOX VALUES (@goo_no, @plu_no, @itf, @cs_qty, @remark1, @remark2) ";
                                PRODBOX prodbox = new PRODBOX(dr["flatString"].ToString().Substring(0, 13),
                                                              dr["flatString"].ToString().Substring(13, 13),
                                                              dr["flatString"].ToString().Substring(26, 14),
                                                              dr["flatString"].ToString().Substring(40, 5),
                                                              dr["flatString"].ToString().Substring(45, 1),
                                                              dr["flatString"].ToString().Substring(46, 1));

                                cn.Execute(cmd, prodbox);
                            }

                            tran.Commit();
                        }
                    }

                    is_success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    is_success = false;
                }

                return is_success;
            }
        }
        #endregion
    */
}
