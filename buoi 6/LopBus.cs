
using System;
using System.Collections.Generic;
using QuanLySinhVien.Data.DAL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.BUL
{
    public class LopBus
    {
        private readonly LopDAL lopDAL = new LopDAL();

        public List<LopHoc> GetAllLopHoc()
        {
            try
            {
                return lopDAL.GetAllLopHoc();
            }
            catch (Exception ex)
            {
                throw new Exception("BLL: Lỗi lấy danh sách lớp.", ex);
            }
        }

        public void AddLopHoc(LopHoc lh)
        {
            try
            {
                if (lh == null)
                    throw new ArgumentNullException(nameof(lh));

                if (string.IsNullOrWhiteSpace(lh.MaLop))
                    throw new Exception("Mã lớp không được để trống.");

                if (string.IsNullOrWhiteSpace(lh.TenLop))
                    throw new Exception("Tên lớp không được để trống.");

                if (lopDAL.GetLopHocByMaLop(lh.MaLop) != null)
                    throw new Exception("Mã lớp đã tồn tại.");

                lopDAL.AddLopHoc(lh);
            }
            catch (Exception ex)
            {
                throw new Exception("BLL: Lỗi thêm lớp học.", ex);
            }
        }
    }
}
