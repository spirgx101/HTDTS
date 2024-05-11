using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HTDTS.DAO
{
    public class DAO_XSC
    {
        private string KEY = string.Empty;

        public DAO_XSC(string conn_key)
        {
            KEY = conn_key;
        }

        private string GetConnectString()
        {
            return new ConfigCipher().Verify(KEY);
        }

        public DataTable Run_sp_xsc_A11TA(string store_id)
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
                        comm.CommandText = "_sp_xsc_A11TA";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@STR_NO", SqlDbType.VarChar, 10).Value = store_id;

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

        public DataTable Run_sp_xsc_XB56(string store_id, string do_no, string site_no)
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
                        comm.CommandText = "_sp_xsc_XB56";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@STR_NO", SqlDbType.VarChar, 10).Value = store_id;
                        comm.Parameters.Add("@DO_NO", SqlDbType.VarChar, 20).Value = do_no;
                        comm.Parameters.Add("@SITE_NO", SqlDbType.VarChar, 6).Value = site_no;

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
