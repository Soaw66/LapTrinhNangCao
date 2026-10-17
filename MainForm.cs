
using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLySinhVien.BUL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien
{
    public class MainForm : Form
    {
        private readonly SinhVienBUL svBUL = new SinhVienBUL();
        private readonly LopBus lopBus = new LopBus();

        private readonly DataGridView dgv = new DataGridView();
        private readonly TextBox txtMaSV = new TextBox();
        private readonly TextBox txtHoTen = new TextBox();
        private readonly ComboBox cboGioiTinh = new ComboBox();
        private readonly DateTimePicker dtpNgaySinh = new DateTimePicker();
        private readonly TextBox txtSDT = new TextBox();
        private readonly ComboBox cboLop = new ComboBox();
        private readonly NumericUpDown numDiem = new NumericUpDown();
        private readonly TextBox txtEmail = new TextBox();

        public MainForm()
        {
            Text = "Quản lý sinh viên";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1050, 700);
            MinimumSize = new Size(900, 600);

            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            TableLayoutPanel root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(10)
            };

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 245));
            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            Controls.Add(root);

            GroupBox group = new GroupBox
            {
                Text = "Thông tin sinh viên",
                Dock = DockStyle.Fill
            };

            root.Controls.Add(group, 0, 0);

            TableLayoutPanel fields = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 4,
                Padding = new Padding(8)
            };

            for (int i = 0; i < 4; i++)
                fields.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 25));

            for (int i = 0; i < 4; i++)
                fields.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 25));

            group.Controls.Add(fields);

            AddField(fields, "Mã sinh viên", txtMaSV, 0, 0);
            AddField(fields, "Họ tên", txtHoTen, 1, 0);

            cboGioiTinh.DropDownStyle =
                ComboBoxStyle.DropDownList;
            cboGioiTinh.Items.AddRange(
                new object[] { "Nam", "Nữ", "Khác" });
            AddField(fields, "Giới tính", cboGioiTinh, 2, 0);

            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.MaxDate = DateTime.Today;
            AddField(fields, "Ngày sinh", dtpNgaySinh, 3, 0);

            AddField(fields, "Số điện thoại", txtSDT, 0, 1);

            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
            AddField(fields, "Lớp học", cboLop, 1, 1);

            numDiem.Minimum = 0;
            numDiem.Maximum = 10;
            numDiem.DecimalPlaces = 2;
            numDiem.Increment = 0.25M;
            AddField(fields, "Điểm", numDiem, 2, 1);

            AddField(fields, "Email", txtEmail, 3, 1);

            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                WrapContents = true
            };

            Button btnThem = new Button
            {
                Text = "Thêm",
                Width = 100,
                Height = 35
            };

            Button btnSua = new Button
            {
                Text = "Cập nhật",
                Width = 100,
                Height = 35
            };

            Button btnXoa = new Button
            {
                Text = "Xóa",
                Width = 100,
                Height = 35
            };

            Button btnMoi = new Button
            {
                Text = "Làm mới",
                Width = 100,
                Height = 35
            };

            buttons.Controls.AddRange(
                new Control[] { btnThem, btnSua, btnXoa, btnMoi });

            fields.Controls.Add(buttons, 0, 2);
            fields.SetColumnSpan(buttons, 4);

            btnThem.Click += (s, e) => AddStudent();
            btnSua.Click += (s, e) => UpdateStudent();
            btnXoa.Click += (s, e) => DeleteStudent();
            btnMoi.Click += (s, e) => ClearFields();

            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgv.SelectionChanged += (s, e) => ShowSelectedStudent();

            root.Controls.Add(dgv, 0, 1);
        }

        private void AddField(
            TableLayoutPanel panel,
            string label,
            Control input,
            int column,
            int row)
        {
            Panel cell = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(4)
            };

            Label lbl = new Label
            {
                Text = label,
                Dock = DockStyle.Top,
                Height = 22
            };

            input.Dock = DockStyle.Top;

            cell.Controls.Add(input);
            cell.Controls.Add(lbl);

            panel.Controls.Add(cell, column, row);
        }

        private void LoadData()
        {
            try
            {
                cboLop.DataSource = null;
                cboLop.DataSource = lopBus.GetAllLopHoc();

                dgv.DataSource = null;
                dgv.DataSource = svBUL.GetAllSinhVien();

                if (dgv.Columns["NgaySinh"] != null)
                    dgv.Columns["NgaySinh"].DefaultCellStyle
                        .Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private SinhVien ReadForm()
        {
            if (cboLop.SelectedValue == null)
                throw new Exception("Vui lòng chọn lớp học.");

            return new SinhVien
            {
                MaSV = txtMaSV.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                GioiTinh = cboGioiTinh.Text,
                NgaySinh = dtpNgaySinh.Value.Date,
                SoDienThoai = txtSDT.Text.Trim(),
                MaLop = cboLop.SelectedValue.ToString(),
                Diem = (double)numDiem.Value,
                Email = txtEmail.Text.Trim()
            };
        }

        private void AddStudent()
        {
            try
            {
                SinhVien sv = ReadForm();
                svBUL.AddSinhVien(sv);

                MessageBox.Show("Thêm sinh viên thành công.");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void UpdateStudent()
        {
            try
            {
                SinhVien sv = ReadForm();
                svBUL.UpdateSinhVien(sv);

                MessageBox.Show("Cập nhật sinh viên thành công.");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void DeleteStudent()
        {
            try
            {
                string maSV = txtMaSV.Text.Trim();

                if (string.IsNullOrWhiteSpace(maSV))
                    throw new Exception(
                        "Hãy chọn sinh viên cần xóa.");

                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa sinh viên " + maSV + "?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                svBUL.DeleteSinhVien(maSV);

                MessageBox.Show("Xóa sinh viên thành công.");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void ShowSelectedStudent()
        {
            if (dgv.CurrentRow == null ||
                dgv.CurrentRow.DataBoundItem is not SinhVien sv)
                return;

            txtMaSV.Text = sv.MaSV;
            txtHoTen.Text = sv.HoTen;
            txtSDT.Text = sv.SoDienThoai;
            txtEmail.Text = sv.Email;
            dtpNgaySinh.Value =
                sv.NgaySinh > DateTime.Today
                    ? DateTime.Today : sv.NgaySinh;

            if (cboGioiTinh.Items.Contains(sv.GioiTinh))
                cboGioiTinh.SelectedItem = sv.GioiTinh;
            else
                cboGioiTinh.SelectedIndex = -1;

            numDiem.Value = (decimal)Math.Clamp(sv.Diem, 0, 10);

            cboLop.SelectedValue = sv.MaLop;

            // Không sửa mã sinh viên khi đang cập nhật.
            txtMaSV.ReadOnly = true;
        }

        private void ClearFields()
        {
            txtMaSV.Clear();
            txtMaSV.ReadOnly = false;
            txtHoTen.Clear();
            txtSDT.Clear();
            txtEmail.Clear();

            cboGioiTinh.SelectedIndex = -1;

            dtpNgaySinh.Value = DateTime.Today;
            numDiem.Value = 0;

            if (cboLop.Items.Count > 0)
                cboLop.SelectedIndex = 0;

            dgv.ClearSelection();
            txtMaSV.Focus();
        }

        private void ShowError(Exception ex)
        {
            // Giữ thông báo của tầng ngoài và lỗi gốc để dễ tìm nguyên nhân.
            Exception root = ex;
            while (root.InnerException != null)
                root = root.InnerException;

            MessageBox.Show(
                ex.Message + "\n\nChi tiết: " + root.Message,
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
