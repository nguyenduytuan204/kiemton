using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyDatSan
{
    /// <summary>
    /// Lớp tiện ích dùng chung để kết nối và thao tác với CSDL QUANLYDATSAN.
    /// </summary>
    public static class DBConnection
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["QuanLyDatSanDB"].ConnectionString; }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>Thực thi 1 câu lệnh SELECT (text) và trả về DataTable.</summary>
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        /// <summary>Gọi 1 Stored Procedure trả về dữ liệu (SELECT) và trả về DataTable.</summary>
        public static DataTable ExecuteStoredProcedure(string procName, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(procName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        /// <summary>Gọi 1 Stored Procedure không trả dữ liệu (INSERT/UPDATE/DELETE).</summary>
        public static int ExecuteNonQueryProcedure(string procName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(procName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
