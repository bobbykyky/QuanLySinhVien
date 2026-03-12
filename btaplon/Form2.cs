using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace btaplon
{
    public partial class Form2 : Form
    {
        private readonly string _username;
        private readonly string _connStr;

        public Form2(string username)
        {
            _username = string.IsNullOrWhiteSpace(username) ? "User" : username.Trim();
            _connStr = ConfigurationManager
                .ConnectionStrings["btaplon.Properties.Settings.QuanLySinhVienConnectionString"]
                .ConnectionString;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Text = $"Quản lý sinh viên - Xin chào {_username}";

            cboGioiTinh.Items.Clear();
            cboGioiTinh.Items.Add("Nam");
            cboGioiTinh.Items.Add("Nữ");

            LoadLop();
            LoadSinhVien();
        }

        private void LoadLop()
        {
            try
            {
                using (var conn = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("SELECT MaLop, TenLop FROM Lop ORDER BY TenLop", conn))
                {
                    conn.Open();
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    cboLop.DataSource = dt;
                    cboLop.DisplayMember = "TenLop";
                    cboLop.ValueMember = "MaLop";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách lớp.\n" + ex.Message);
            }
        }

        private void LoadSinhVien()
        {
            try
            {
                using (var conn = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand(@"
SELECT sv.MaSV, sv.HoTen AS [Họ và tên], sv.NgaySinh AS [Ngày sinh], sv.GioiTinh AS [Giới tính],
       sv.DiaChi AS [Địa chỉ], sv.MaLop AS [Mã lớp], l.TenLop AS [Tên lớp]
FROM SinhVien sv
INNER JOIN Lop l ON sv.MaLop = l.MaLop
ORDER BY sv.MaSV
", conn))
                {
                    conn.Open();
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    dgvSinhVien.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sinh viên.\n" + ex.Message);
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvSinhVien.CurrentRow == null) return;

            var row = dgvSinhVien.CurrentRow;
            txtMaSV.Text = row.Cells["MaSV"]?.Value?.ToString();
            txtHoTen.Text = row.Cells["Họ và tên"]?.Value?.ToString();
            txtDiaChi.Text = row.Cells["Địa chỉ"]?.Value?.ToString();
            cboGioiTinh.Text = row.Cells["Giới tính"]?.Value?.ToString();

            if (DateTime.TryParse(row.Cells["Ngày sinh"]?.Value?.ToString(), out var ns))
                dtpNgaySinh.Value = ns;

            // Chọn lớp theo mã lớp
            var maLopStr = row.Cells["Mã lớp"]?.Value?.ToString();
            if (int.TryParse(maLopStr, out var maLop))
                cboLop.SelectedValue = maLop;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // TODO: xử lý thêm sinh viên
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // TODO: xử lý sửa sinh viên
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // TODO: xử lý xóa sinh viên
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSV.Clear();
            txtHoTen.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            cboGioiTinh.SelectedIndex = -1;
            if (cboLop.Items.Count > 0) cboLop.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Today;

            LoadSinhVien();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            var kw = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                LoadSinhVien();
                return;
            }

            try
            {
                using (var conn = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand(@"
SELECT sv.MaSV, sv.HoTen AS [Họ và tên], sv.NgaySinh AS [Ngày sinh], sv.GioiTinh AS [Giới tính],
       sv.DiaChi AS [Địa chỉ], sv.MaLop AS [Mã lớp], l.TenLop AS [Tên lớp]
FROM SinhVien sv
INNER JOIN Lop l ON sv.MaLop = l.MaLop
WHERE sv.MaSV LIKE @kw OR sv.HoTen LIKE @kw OR l.TenLop LIKE @kw
ORDER BY sv.MaSV
", conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + kw + "%");
                    conn.Open();
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    dgvSinhVien.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tìm kiếm.\n" + ex.Message);
            }
        }

        private void menuLop_Click(object sender, EventArgs e)
        {
            using (var f = new Form3())
            {
                f.ShowDialog();
            }

            // Sau khi quản lý lớp xong, nạp lại combobox lớp
            LoadLop();
        }
    }
}

