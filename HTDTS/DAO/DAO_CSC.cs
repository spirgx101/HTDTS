using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace HTDTS.DAO
{
    public class DAO_CSC
    {
        private string KEY = string.Empty;
        public DAO_CSC(string conn_key)
        {
            KEY = conn_key;
        }

        private string GetConnectString()
        {
            //return ConfigurationManager.AppSettings["CSC"].ToString();
            return new ConfigCipher().Verify(KEY);
        }

        public int Run_sp_csc_1O_GenData(string store_id, bool pscript = false)
        {
            int ret_code = 0;
            string ret_msg = string.Empty;
            using (SqlConnection conn = new SqlConnection(GetConnectString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = "_sp_csc_1O_GenData";
                        comm.CommandTimeout = 0;

                        comm.Parameters.Add("@StoreID", SqlDbType.VarChar).Value = store_id;
                        comm.Parameters.Add("@retcode", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        comm.Parameters.Add("@PScript", SqlDbType.Bit).Value = pscript;

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if ((reader.HasRows) && (!reader[0].Equals(DBNull.Value)))
                                    {
                                        ret_code = reader.GetInt16(0);
                                        ret_msg = reader.GetString(1);                                        
                                    }
                                }
                            }                           
                        }                 
                    }                  
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
                return ret_code;
            }
        }

        public DataTable Run_sp_csc_1O_dnPrdt(string store_id, string dtype, bool pscript = false)
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
                        comm.CommandText = "_sp_csc_1O_dnPrdt";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@StoreID", SqlDbType.VarChar).Value = store_id;
                        comm.Parameters.Add("@DType", SqlDbType.VarChar).Value = dtype;
                        comm.Parameters.Add("@PScript", SqlDbType.Bit).Value = pscript;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                        {
                            adapter.Fill(table);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return table;
        }

        public DataTable Run_sp_csc_1O_dnStock(string store_id, bool pscript = false)
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
                        comm.CommandText = "_sp_csc_1O_dnStock";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@StoreID", SqlDbType.VarChar).Value = store_id;
                        comm.Parameters.Add("@PScript", SqlDbType.Bit).Value = pscript;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                        {
                            adapter.Fill(table);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return table;
        }

        public DataTable Run_sp_csc_1O_dnBoxCode(string store_id, bool pscript = false)
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
                        comm.CommandText = "_sp_csc_1O_dnBoxCode";
                        //comm.CommandTimeout = 0;

                        comm.Parameters.Add("@StoreID", SqlDbType.VarChar).Value = store_id;
                        comm.Parameters.Add("@PScript", SqlDbType.Bit).Value = pscript;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                        {
                            adapter.Fill(table);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
