
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLySinhVien.Data.DAL
{
    public class DataHelper
    {
        private readonly string connectionString =
            @"Server=LAPTOP-6SN65NCS\HUECTMDB;Database=QLSV;Integrated Security=True;TrustServerCertificate=True";

        public DataTable GetDataTable(
            string sql, params SqlParameter[] parameters)
        {
            try
            {
                using SqlConnection con =
                    new SqlConnection(connectionString);

                using SqlCommand cmd = new SqlCommand(sql, con);

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using SqlDataAdapter adapter =
                    new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi đọc dữ liệu SQL Server.", ex);
            }
        }

        public int ExecuteNonQuery(
            string sql, params SqlParameter[] parameters)
        {
            try
            {
                using SqlConnection con =
                    new SqlConnection(connectionString);

                using SqlCommand cmd = new SqlCommand(sql, con);

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                con.Open();

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thực hiện câu lệnh SQL.", ex);
            }
        }
    }
}
