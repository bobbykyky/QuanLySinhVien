using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace btaplon
{
    public partial class Form3 : Form
    {
        private readonly string _connStr;

        public Form3()
        {
            _connStr = ConfigurationManager
                .ConnectionStrings["btaplon.Properties.Settings.QuanLySinhVienConnectionString"]
                .ConnectionString;

            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadLop();
        }

        private void LoadLop()
        {
            try
            {
                using (var conn = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("SELECT MaLop, TenLop AS [Tên lớp] FROM Lop ORDER BY MaLop", conn))
                {
                    conn.Open();
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    dgvLop.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách lớp.\n" + ex.Message);
            }
        }

        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvLop.CurrentRow == null) return;

            var row = dgvLop.CurrentRow;
            txtMaLop.Text = row.Cells["MaLop"]?.Value?.ToString();
            txtTenLop.Text = row.Cells["Tên lớp"]?.Value?.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var ten = txtTenLop.Text.Trim();
            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Tên lớp không được để trống.");
                return;
            }

            try
            {
                using (var conn = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("INSERT INTO Lop(TenLop) VALUES(@ten)", conn))
                {
                    cmd.Parameters.AddWithValue("@ten", ten);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadLop();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm lớp.\n" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLop.Text.Trim(), out var ma))
            {
                MessageBox.Show("Vui lòng chọn lớp cần sửa.");
                return;
            }

            var ten = txtTenLop.Text.Trim();
            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Tên lớp không được để trống.");
                return;
            }

            try
            {
                using (var conn = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("UPDATE Lop SET TenLop=@ten WHERE MaLop=@ma", conn))
                {
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@ma", ma);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadLop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể sửa lớp.\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLop.Text.Trim(), out var ma))
            {
                MessageBox.Show("Vui lòng chọn lớp cần xóa.");
                return;
            }

            if (MessageBox.Show("Xóa lớp này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                using (var conn = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("DELETE FROM Lop WHERE MaLop=@ma", conn))
                {
                    cmd.Parameters.AddWithValue("@ma", ma);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadLop();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa lớp (có thể lớp đang có sinh viên).\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            ClearInputs();
            LoadLop();
        }

        private void ClearInputs()
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            var kw = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                LoadLop();
                return;
            }

            try
            {
                using (var conn = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand(@"
SELECT MaLop, TenLop AS [Tên lớp]
FROM Lop
WHERE CAST(MaLop AS NVARCHAR(20)) LIKE @kw OR TenLop LIKE @kw
ORDER BY MaLop
", conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + kw + "%");
                    conn.Open();
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    dgvLop.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tìm kiếm lớp.\n" + ex.Message);
            }
        }
    }
}

