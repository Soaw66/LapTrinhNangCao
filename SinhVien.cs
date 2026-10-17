
using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Data.Entity
{
    public class SinhVien
    {
        public string MaSV { get; set; } = "";
        public string HoTen { get; set; } = "";
        public string GioiTinh { get; set; } = "";
        public DateTime NgaySinh { get; set; } = DateTime.Today;
        public string SoDienThoai { get; set; } = "";
        public string MaLop { get; set; } = "";
        public double Diem { get; set; }
        public string Email { get; set; } = "";

        public List<string> IsInValid()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(MaSV))
                errors.Add("Mã sinh viên không được để trống.");

            if (string.IsNullOrWhiteSpace(HoTen))
                errors.Add("Họ tên không được để trống.");

            if (string.IsNullOrWhiteSpace(MaLop))
                errors.Add("Vui lòng chọn lớp.");

            if (NgaySinh.Date > DateTime.Today)
                errors.Add("Ngày sinh không được ở tương lai.");

            if (double.IsNaN(Diem) ||
                double.IsInfinity(Diem) ||
                Diem < 0 || Diem > 10)
                errors.Add("Điểm phải nằm trong khoảng từ 0 đến 10.");

            return errors;
        }
    }
}
