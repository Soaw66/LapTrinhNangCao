
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    public class SinhVienDAL
    {
        private readonly DataHelper db = new DataHelper();

        private SinhVien MapSinhVien(DataRow row)
        {
            return new SinhVien
            {
                MaSV = row["MaSV"].ToString(),
                HoTen = row["HoTen"].ToString(),
                GioiTinh = row["GioiTinh"] == DBNull.Value
                    ? "" : row["GioiTinh"].ToString(),
                NgaySinh = row["NgaySinh"] == DBNull.Value
                    ? DateTime.Today
                    : Convert.ToDateTime(row["NgaySinh"]),
                SoDienThoai = row["SoDienThoai"] == DBNull.Value
                    ? "" : row["SoDienThoai"].ToString(),
                MaLop = row["MaLop"].ToString(),
                Diem = row["Diem"] == DBNull.Value
                    ? 0 : Convert.ToDouble(row["Diem"]),
                Email = row["Email"] == DBNull.Value
                    ? "" : row["Email"].ToString()
            };
        }

        private SqlParameter[] GetParameters(SinhVien sv)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@MaSV", sv.MaSV),
                new SqlParameter("@MaLop", sv.MaLop),
                new SqlParameter("@HoTen", sv.HoTen),
                new SqlParameter("@GioiTinh",
                    string.IsNullOrWhiteSpace(sv.GioiTinh)
                        ? DBNull.Value : sv.GioiTinh),
                new SqlParameter("@NgaySinh", sv.NgaySinh.Date),
                new SqlParameter("@Diem", sv.Diem),
                new SqlParameter("@SoDienThoai",
                    string.IsNullOrWhiteSpace(sv.SoDienThoai)
                        ? DBNull.Value : sv.SoDienThoai),
                new SqlParameter("@Email",
                    string.IsNullOrWhiteSpace(sv.Email)
                        ? DBNull.Value : sv.Email)
            };
        }

        public List<SinhVien> GetAllSinhVien()
        {
            try
            {
                DataTable dt = db.GetDataTable(
                    "SELECT * FROM SinhVien ORDER BY MaSV");

                List<SinhVien> list = new List<SinhVien>();

                foreach (DataRow row in dt.Rows)
                    list.Add(MapSinhVien(row));

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL: Lỗi lấy danh sách sinh viên.", ex);
            }
        }

        public SinhVien GetSinhVienById(string maSV)
        {
            try
            {
                DataTable dt = db.GetDataTable(
                    "SELECT * FROM SinhVien WHERE MaSV = @MaSV",
                    new SqlParameter("@MaSV", maSV));

                return dt.Rows.Count == 0
                    ? null
                    : MapSinhVien(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                throw new Exception("DAL: Lỗi tìm sinh viên.", ex);
            }
        }

        public void AddSinhVien(SinhVien sv)
        {
            try
            {
                string sql = @"
                    INSERT INTO SinhVien
                    (MaSV, MaLop, HoTen, GioiTinh, NgaySinh,
                     Diem, SoDienThoai, Email)
                    VALUES
                    (@MaSV, @MaLop, @HoTen, @GioiTinh, @NgaySinh,
                     @Diem, @SoDienThoai, @Email)";

                db.ExecuteNonQuery(sql, GetParameters(sv));
            }
            catch (Exception ex)
            {
                throw new Exception("DAL: Lỗi thêm sinh viên.", ex);
            }
        }

        public void UpdateSinhVien(SinhVien sv)
        {
            try
            {
                string sql = @"
                    UPDATE SinhVien
                    SET MaLop = @MaLop,
                        HoTen = @HoTen,
                        GioiTinh = @GioiTinh,
                        NgaySinh = @NgaySinh,
                        Diem = @Diem,
                        SoDienThoai = @SoDienThoai,
                        Email = @Email
                    WHERE MaSV = @MaSV";

                int rows = db.ExecuteNonQuery(sql, GetParameters(sv));

                if (rows == 0)
                    throw new Exception(
                        "Không tìm thấy sinh viên cần sửa.");
            }
            catch (Exception ex)
            {
                throw new Exception("DAL: Lỗi cập nhật sinh viên.", ex);
            }
        }

        public void DeleteSinhVien(string maSV)
        {
            try
            {
                int rows = db.ExecuteNonQuery(
                    "DELETE FROM SinhVien WHERE MaSV = @MaSV",
                    new SqlParameter("@MaSV", maSV));

                if (rows == 0)
                    throw new Exception(
                        "Không tìm thấy sinh viên cần xóa.");
            }
            catch (Exception ex)
            {
                throw new Exception("DAL: Lỗi xóa sinh viên.", ex);
            }
        }
    }
}
