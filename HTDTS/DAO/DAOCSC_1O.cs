using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace HTDTS.DAO
{
    public class DAOCSC_1O
    {
        private string KEY = string.Empty;
        public DAOCSC_1O(string conn_key)
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
                    conn.Close();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
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
                    conn.Close();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
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
                    conn.Close();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
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
                    conn.Close();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return table;
        }

    }
}
