
using System;
using System.Collections.Generic;
using QuanLySinhVien.Data.DAL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.BUL
{
    public class SinhVienBUL
    {
        private readonly SinhVienDAL svd = new SinhVienDAL();

        public List<SinhVien> GetAllSinhVien()
        {
            try
            {
                return svd.GetAllSinhVien();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "BLL: Lỗi lấy danh sách sinh viên.", ex);
            }
        }

        public SinhVien GetSinhVienById(string maSV)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maSV))
                    throw new Exception("Mã sinh viên không hợp lệ.");

                return svd.GetSinhVienById(maSV);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL: Lỗi tìm sinh viên.", ex);
            }
        }

        public void AddSinhVien(SinhVien sv)
        {
            try
            {
                if (sv == null)
                    throw new ArgumentNullException(nameof(sv));

                List<string> errors = sv.IsInValid();

                if (errors.Count > 0)
                    throw new Exception(string.Join("\n", errors));

                if (svd.GetSinhVienById(sv.MaSV) != null)
                    throw new Exception("Mã sinh viên đã tồn tại.");

                svd.AddSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL: Lỗi thêm sinh viên.", ex);
            }
        }

        public void UpdateSinhVien(SinhVien sv)
        {
            try
            {
                if (sv == null)
                    throw new ArgumentNullException(nameof(sv));

                List<string> errors = sv.IsInValid();

                if (errors.Count > 0)
                    throw new Exception(string.Join("\n", errors));

                if (svd.GetSinhVienById(sv.MaSV) == null)
                    throw new Exception("Sinh viên không tồn tại.");

                svd.UpdateSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL: Lỗi sửa sinh viên.", ex);
            }
        }

        public void DeleteSinhVien(string maSV)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maSV))
                    throw new Exception("Mã sinh viên không hợp lệ.");

                svd.DeleteSinhVien(maSV);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL: Lỗi xóa sinh viên.", ex);
            }
        }
    }
}
