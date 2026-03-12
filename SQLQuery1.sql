CREATE DATABASE QuanLySinhVien;
GO

USE QuanLySinhVien;
GO

-- Bảng Khoa
CREATE TABLE Khoa (
    MaKhoa INT IDENTITY(1,1) PRIMARY KEY,
    TenKhoa NVARCHAR(100) NOT NULL,
    DienThoai VARCHAR(15)
);

-- Bảng Lớp
CREATE TABLE Lop (
    MaLop INT IDENTITY(1,1) PRIMARY KEY,
    TenLop NVARCHAR(50),
    MaKhoa INT,
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

-- Bảng Sinh viên
CREATE TABLE SinhVien (
    MaSV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(200),
    MaLop INT,
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);

-- Bảng Giảng viên
CREATE TABLE GiangVien (
    MaGV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100),
    HocVi NVARCHAR(50),
    MaKhoa INT,
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

-- Bảng Môn học
CREATE TABLE MonHoc (
    MaMon INT IDENTITY(1,1) PRIMARY KEY,
    TenMon NVARCHAR(100),
    SoTinChi INT,
    MaGV INT,
    FOREIGN KEY (MaGV) REFERENCES GiangVien(MaGV)
);

-- Bảng Điểm
CREATE TABLE BangDiem (
    MaSV INT,
    MaMon INT,
    Diem FLOAT,
    PRIMARY KEY (MaSV, MaMon),
    FOREIGN KEY (MaSV) REFERENCES SinhVien(MaSV),
    FOREIGN KEY (MaMon) REFERENCES MonHoc(MaMon)
);

-- Thêm dữ liệu mẫu
INSERT INTO Khoa (TenKhoa, DienThoai) VALUES
(N'Công nghệ thông tin','0123456789'),
(N'Kinh tế','0987654321');

INSERT INTO Lop (TenLop, MaKhoa) VALUES
(N'CNTT01',1),
(N'CNTT02',1),
(N'KT01',2);

INSERT INTO SinhVien (HoTen, NgaySinh, GioiTinh, DiaChi, MaLop) VALUES
(N'Nguyễn Văn A','2003-05-12',N'Nam',N'Hà Nội',1),
(N'Trần Thị B','2003-08-20',N'Nữ',N'Hà Nội',1),
(N'Lê Văn C','2002-11-10',N'Nam',N'Hải Phòng',2);

INSERT INTO GiangVien (HoTen, HocVi, MaKhoa) VALUES
(N'Nguyễn Văn GV1',N'Thạc sĩ',1),
(N'Trần Văn GV2',N'Tiến sĩ',2);

INSERT INTO MonHoc (TenMon, SoTinChi, MaGV) VALUES
(N'Cơ sở dữ liệu',3,1),
(N'Lập trình Java',4,1),
(N'Kinh tế vi mô',3,2);

INSERT INTO BangDiem (MaSV, MaMon, Diem) VALUES
(1,1,8.5),
(1,2,7.5),
(2,1,9.0),
(3,3,8.0);