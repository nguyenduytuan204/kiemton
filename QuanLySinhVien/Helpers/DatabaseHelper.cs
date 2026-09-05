using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLySinhVien.Helpers
{
    public static class DatabaseHelper
    {
        // Chuỗi kết nối SQL Server (Tùy chỉnh theo máy người dùng)
        public static string ConnectionString = @"Server=localhost;Database=QuanLySinhVienDB;Trusted_Connection=True;TrustServerCertificate=True;";

        /// <summary>
        /// Mở kết nối đến cơ sở dữ liệu
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Thực thi truy vấn SELECT và trả về DataTable (Dùng Parameter chống SQL Injection)
        /// </summary>
        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Thực thi truy vấn INSERT, UPDATE, DELETE và trả về số dòng bị tác động
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Thực thi truy vấn trả về 1 giá trị duy nhất (Ví dụ: COUNT, SUM)
        /// </summary>
        public static object? ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            object? result = null;
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    result = command.ExecuteScalar();
                }
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra kết nối CSDL
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
