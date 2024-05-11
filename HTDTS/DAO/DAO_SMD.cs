using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HTDTS.DAO
{
    public class DAO_SMD
    {
        private string KEY = string.Empty;
        public DAO_SMD(string conn_key)
        {
            KEY = conn_key;
        }

        private string GetConnectString()
        {
            return new ConfigCipher().Verify(KEY);
        }

        //EXEC spSMD_SYS_HT_DOWNLOAD @SITE_ID='302100' , @TYPE=0   --盤點機商品主檔---檔名:\DAT\DPPRD.DAT 
        public DataTable Run_sp_SMDDPPRD(string store_id)
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(GetConnectString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.CommandText = "spSMD_SYS_HT_DOWNLOAD";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@SITE_ID", SqlDbType.VarChar).Value = store_id;
                        comm.Parameters.Add("@TYPE", SqlDbType.Int).Value = 0;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                        {
                            adapter.Fill(table);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return table;
        }

        //EXEC spSMD_SYS_HT_DOWNLOAD @SITE_ID='302100' , @TYPE=1   --(生鮮)即時主檔---檔名:\DAT\SMDITEM.DAT
        public DataTable Run_sp_SMDITEM(string store_id)
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(GetConnectString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.CommandText = "spSMD_SYS_HT_DOWNLOAD";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@SITE_ID", SqlDbType.VarChar).Value = store_id;
                        comm.Parameters.Add("@TYPE", SqlDbType.Int).Value = 1;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                        {
                            adapter.Fill(table);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return table;
        }

        //EXEC spSMD_SYS_HT_DOWNLOAD @SITE_ID='302100' , @TYPE=2   --(生鮮)部門檔  ---檔名:\DAT\SMDDPD.DAT
        public DataTable Run_sp_SMDDPD(string store_id)
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(GetConnectString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.CommandText = "spSMD_SYS_HT_DOWNLOAD";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@SITE_ID", SqlDbType.VarChar).Value = store_id;
                        comm.Parameters.Add("@TYPE", SqlDbType.Int).Value = 2;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                        {
                            adapter.Fill(table);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return table;
        }

        //EXEC spSMD_SYS_HT_DOWNLOAD @SITE_ID='302100' , @TYPE=3   --(生鮮)驗收主檔---檔名:\DAT\SMDRCVH.DAT
        public DataTable Run_sp_SMDRCVH(string store_id)
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(GetConnectString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.CommandText = "spSMD_SYS_HT_DOWNLOAD";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@SITE_ID", SqlDbType.VarChar).Value = store_id;
                        comm.Parameters.Add("@TYPE", SqlDbType.Int).Value = 3;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                        {
                            adapter.Fill(table);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return table;
        }

        //EXEC spSMD_SYS_HT_DOWNLOAD @SITE_ID='302100' , @TYPE=4   --(生鮮)驗收明細檔-檔名:\DAT\SMDRCVL.DAT
        public DataTable Run_sp_SMDRCVL(string store_id)
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(GetConnectString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.CommandText = "spSMD_SYS_HT_DOWNLOAD";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@SITE_ID", SqlDbType.VarChar).Value = store_id;
                        comm.Parameters.Add("@TYPE", SqlDbType.Int).Value = 4;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                        {
                            adapter.Fill(table);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return table;
        }
    }
}
