create DATABASE QL_VatLieuXayDung;
go
use QL_VatLieuXayDung
go
-- Bảng loại sản phẩm
CREATE TABLE LoaiSanPham (
    MaLoai INT IDENTITY(1,1)  PRIMARY KEY,
    TenLoai NVARCHAR(50) UNIQUE,
    MoTa NVARCHAR(255) ,
)

-- Bảng nhân viên
GO
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1)  PRIMARY KEY,
    TenNV NVARCHAR(255) NOT NULL,
    Phai NVARCHAR(5) NOT NULL,
    NgaySinh DATE,
    DiaChi NVARCHAR(255) NOT NULL,
    SDT NVARCHAR(10) UNIQUE,
    TrangThai NVARCHAR(50) DEFAULT N'Đang làm',
	
	
    
)

-- Bảng sản phẩm
GO
CREATE TABLE SanPham (
    MaSP INT IDENTITY(1,1)  PRIMARY KEY,
    TenSP NVARCHAR(255) UNIQUE ,
    DonViTinh  NVARCHAR(50) NOT NULL,
	SoSao int,
    GiaBan int,
    GiaNhap int,
    TinhTrang  NVARCHAR(50)  ,
    MoTa  NVARCHAR(MAX) NOT NULL,
    ThongTin  NVARCHAR(MAX) NOT NULL,
    ImageUrl  NVARCHAR(255) NOT NULL,
    MaLoai INT,
	TonKho int ,
	LUOTDANHGIA INT DEFAULT 0,
	YeuThich BIT DEFAULT 0,
	FOREIGN KEY (MaLoai) REFERENCES LoaiSanPham(MaLoai),
	
   
)

-- Bảng khách hàng
GO
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1)  PRIMARY KEY,
    TenKH NVARCHAR(255) NOT NULL,
    Phai NVARCHAR(5) NOT NULL,
    NgaySinh DATE,
    DiaChi NVARCHAR(255) NOT NULL,
    SDT NVARCHAR(10) UNIQUE,
    UserName NVARCHAR(50) UNIQUE ,
    MatKhau NVARCHAR(255) ,
    Email NVARCHAR(255) ,
    TrangThai NVARCHAR(50) DEFAULT N'Không khoá',
		
)

GO
create table PhanQuyen(
maquyen varchar(10) primary key,
tenquyen nvarchar(50),
trangthai nvarchar(50),
)

-- Bảng tài khoản nhân viên
GO
CREATE TABLE TaiKhoanNV (
    UserName NVARCHAR(50) PRIMARY KEY,
    MatKhau NVARCHAR(255) NOT NULL,
    MaNV int Unique,
    maquyen varchar(10),
    TrangThai NVARCHAR(50) DEFAULT N'Off' ,
	FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
	foreign key (maquyen) references phanquyen(maquyen),
	
)



-- Bảng hãng sản xuất
GO
CREATE TABLE HangSanXuat 
(
    MaHSX INT IDENTITY(1,1)  PRIMARY KEY,
    TenHSX NVARCHAR(255) UNIQUE,
    DiaChi NVARCHAR(255),
    SDT NVARCHAR(10) UNIQUE
)
--Khuyến mãi 
GO
CREATE TABLE KhuyenMai (
    MaKhuyenMai NVarChar(50),
    PhanTramGiam INT,
    NgayApDung date,
	NgayHetHan date , 
	
	PRIMARY KEY (MaKhuyenMai),

)

-- Bảng hoá đơn
GO
CREATE TABLE HoaDon (
    MaHD INT IDENTITY(1,1)  PRIMARY KEY,
    MaKH INT,
    NgayLapHD DATETIME,
	SoHoaDon int,
	TenNguoiNhan NVARCHAR(255) NOT NULL,  
	DiaChi NVARCHAR(255) NOT NULL, 
    SDT NVARCHAR(10),
    Email NVARCHAR(255) ,
    ThanhTien INT,
	TienCoc int,
	MaKhuyenMai NVarChar(50) ,
	GhiChu NVARCHAR(255),
    TrangThai bit,
	TrangThaiDonHang NVARCHAR(255),
	FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
	FOREIGN KEY (MaKhuyenMai) REFERENCES KhuyenMai(MaKhuyenMai),
)


-- Bảng chi tiết hoá đơn
GO
CREATE TABLE ChiTietHoaDon (
    MaHD INT,
    MaSP INT,
    SoLuong INT,
	GiaBan INT , 
	ThanhTien INT,
	PRIMARY KEY (MaHD, MaSP),
	FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
	FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
    
)

-- Bảng chi tiết xác nhận hoá đơn
GO
CREATE TABLE XacNhanHoaDon (
    MaHD INT unique,
    MaNV INT UNIQUE,
    NgayXacNhan DATETIME,
	PRIMARY KEY (mahd, Manv),
	FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
	FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
    
)





-- Bảng phiếu đặt
GO
CREATE TABLE PhieuDat (
    MaPhieuDat INT IDENTITY(1,1)  PRIMARY KEY,
    NgayLap DATE,
    TrangThai NVARCHAR(50),
    MaHSX int,
	FOREIGN KEY (MaHSX) REFERENCES HangSanXuat(MaHSX)
)
-- Bảng phiếu nhập
GO
CREATE TABLE PhieuNhap (
    MaPhieuNhap INT IDENTITY(1,1)  PRIMARY KEY,
    MaNV INT,
    NgayNhap DATE,
    MaPhieuDat int,
    TongTien int,
    TrangThai NVARCHAR(50),
	FOREIGN KEY (MaPhieuDat) REFERENCES PhieuDat(MaPhieuDat),
	FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)

    
)
-- Bảng chi tiết phiếu đặt
GO
CREATE TABLE ChiTietPhieuDat (
    MaPhieuDat INT,
    MaSanPham INT,
    SoLuong INT,
	PRIMARY KEY (MaPhieuDat, MaSanPham),
	FOREIGN KEY (MaPhieuDat) REFERENCES PhieuDat(MaPhieuDat),
	FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSP)
    
)

-- Bảng chi tiết phiếu nhập
GO
CREATE TABLE ChiTietPhieuNhap (
    MaPhieuNhap INT,
    MaSP INT,
    SoLuong INT,
	GiaNhap INT , 
	ThanhTien INT,
	PRIMARY KEY (MaPhieuNhap, MaSP),
	FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap),
	FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP),

	 
)


-- Bảng tin tức
GO
CREATE TABLE TinTuc (
    MaTin INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
	ImageUrl NVARCHAR (255),
    NoiDung TEXT,
    NgayDang DATE
    
)

-- Bảng lịch sử hoạt động
GO
CREATE TABLE LichSuHoatDong (
    UserName  NVARCHAR(50),
    ThoiGian DATETIME,
    HoatDong NVARCHAR(255),
    TrangThai  NVARCHAR(50),
	PRIMARY KEY (UserName, ThoiGian),
)

-- Bảng sản phẩm yêu thích
GO
CREATE TABLE SanPham_YeuThich (
    MaSP INT,
    MaKH int
	PRIMARY KEY (MaSP, MaKH),
	FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
	FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)

)
-- Bảng sản phẩm trong gio hang
GO
CREATE TABLE SanPham_TrongGioHang(
    MaSP INT,
    MaKH int,
	SoLuong int,
	PRIMARY KEY (MaSP, MaKH),
	FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
	FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)

)

-- Bảng chi tiết sản phẩm hãng sản xuất
GO
CREATE TABLE ChiTiet_SanPham_HangSanXuat (
    MaSP INT unique,
    MaHSX int
	PRIMARY KEY (MaSP, MaHSX),
	FOREIGN KEY (MaHSX) REFERENCES HangSanXuat(MaHSX),
	FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)

) 
GO
CREATE TABLE LichSuGia (
    MaSP INT ,
	Gia INT,
    NgayCapNhat DATETIME,
	PRIMARY KEY (MaSP, Gia, NgayCapNhat),
	FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)

) 

-- Bảng đánh giá
GO
CREATE TABLE DANHGIA (
    MaKH INT NOT NULL,
    MaSP INT NOT NULL,
	TIEUDE NVARCHAR(MAX),
    NOIDUNG NVARCHAR(MAX),
	SOSAO INT,
	HINHANH VARCHAR(50),
	NGAYDG DATETIME,
	PRIMARY KEY (MaKH, MaSP),
    FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- Trigger để cập nhật số sao của vé xe khi có lượt đánh giá mới
GO
CREATE TRIGGER UpdateRating
ON DANHGIA
AFTER INSERT
AS
BEGIN
    -- Cập nhật số sao của vé xe liên quan đến lượt đánh giá mới
    UPDATE SanPham
    SET SOSAO = ROUND((
        SELECT SUM(DANHGIA.SOSAO) 
        FROM DANHGIA 
        WHERE DANHGIA.MaSP = SanPham.MaSP
    ) / (
        SELECT COUNT(*)
        FROM DANHGIA
        WHERE DANHGIA.MaSP = SanPham.MaSP
    ), 0)
    FROM SanPham
    JOIN inserted ON SanPham.MaSP = inserted.MaSP;
END;

-- Trigger để cập nhật lượt đánh giá của sản phẩm khi có lượt đánh giá mới
GO
CREATE TRIGGER trg_DanhGia_Insert
ON DANHGIA
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật lượt đánh giá trong bảng VEXE
    UPDATE SanPham
    SET LUOTDANHGIA = SanPham.LUOTDANHGIA + I.NumRatings
    FROM SanPham
    JOIN (
        SELECT MaSP, COUNT(*) AS NumRatings
        FROM inserted
        GROUP BY MaSP
    ) AS I ON SanPham.MaSP = I.MaSP;
END;


 -- Trigger để giảm số lượng tồn kho khi có hoá đơn mua hàng
Go
CREATE TRIGGER Trg_GiamTonKho
ON HoaDon
AFTER UPDATE
AS
BEGIN
    -- Cập nhật số lượng tồn kho của sản phẩm khi hoá đơn có trạng thái "Chờ lấy hàng"
    UPDATE sp
    SET sp.TonKho = sp.TonKho - cthd.SoLuong
    FROM SanPham sp
    INNER JOIN ChiTietHoaDon cthd ON sp.MaSP = cthd.MaSP
    INNER JOIN inserted i ON cthd.MaHD = i.MaHD
    WHERE i.TrangThaiDonHang = N'Chờ lấy hàng';
END;

-- Trigger để cập nhật trạng thái sản phẩm khi có hoá đơn mua hàng
GO
CREATE TRIGGER Trg_CapNhatTrangThai
ON ChiTietHoaDon
AFTER INSERT
AS
BEGIN
    -- Cập nhật trạng thái của sản phẩm khi số lượng tồn kho giảm xuống 0
    UPDATE SanPham
    SET TinhTrang = N'Hết hàng'
    WHERE MaSP IN (SELECT i.MaSP FROM inserted i
                   JOIN SanPham sp ON i.MaSP = sp.MaSP
                   WHERE sp.TonKho - i.SoLuong <= 0);
END;

-- Trigger để tăng số lượng tồn kho khi có chi tiết phiếu nhập
GO
CREATE TRIGGER Trg_TangTonKho
ON ChiTietPhieuNhap
AFTER INSERT
AS
BEGIN
    -- Cập nhật số lượng tồn kho của sản phẩm khi có chi tiết phiếu nhập
    UPDATE SanPham
    SET TonKho = TonKho + i.SoLuong
    FROM SanPham
    INNER JOIN inserted i ON SanPham.MaSP = i.MaSP;
END;

-- Trigger để tự động cập nhật lịch sử giá khi có chi tiết phiếu nhập
GO
CREATE TRIGGER Trg_CapNhatLichSuGia
ON ChiTietPhieuNhap
AFTER INSERT
AS
BEGIN
    -- Chèn thông tin lịch sử giá cho các sản phẩm trong chi tiết phiếu nhập
    INSERT INTO LichSuGia (MaSP, Gia, NgayCapNhat)
    SELECT i.MaSP, i.GiaNhap, GETDATE()
    FROM inserted i;
END;


-- Trigger để cập nhật trạng thái của sản phẩm khi có chi tiết phiếu nhập được thêm vào
GO
CREATE TRIGGER Trg_CapNhatTrangThaiSanPham
ON ChiTietPhieuNhap
AFTER INSERT
AS
BEGIN
    -- Cập nhật trạng thái của sản phẩm thành 'Còn hàng' khi có chi tiết phiếu nhập được thêm vào
    UPDATE SanPham
SET TinhTrang = N'Còn hàng'
    FROM SanPham sp
    INNER JOIN inserted i ON sp.MaSP = i.MaSP;
END;

-- Trigger để cập nhật giá nhập của sản phẩm khi có chi tiết phiếu nhập được thêm vào
GO
CREATE TRIGGER Trg_CapNhatGiaNhapSanPham
ON ChiTietPhieuNhap
AFTER INSERT
AS
BEGIN
    -- Cập nhật giá nhập của sản phẩm khi có chi tiết phiếu nhập được thêm vào
    UPDATE SanPham
    SET GiaNhap = i.GiaNhap  -- Giả sử GiaNhap là cột trong ChiTietPhieuNhap
    FROM SanPham sp
    INNER JOIN inserted i ON sp.MaSP = i.MaSP;
END;


SELECT
    HD.NgayLapHD AS Ngay,
    SUM(CT.ThanhTien) AS DoanhThu,
    SUM(CT.ThanhTien - (COALESCE(LS.Gia, LSCuoiCung.Gia) * CT.SoLuong)) AS TienLoi
FROM
    HoaDon HD
JOIN
    ChiTietHoaDon CT ON HD.MaHD = CT.MaHD
OUTER APPLY (
    SELECT TOP 1 Gia
    FROM LichSuGia LS
    WHERE CT.MaSP = LS.MaSP AND HD.NgayLapHD >= LS.NgayCapNhat
    ORDER BY LS.NgayCapNhat DESC
) AS LS
OUTER APPLY (
    SELECT TOP 1 Gia
    FROM LichSuGia LS
    WHERE CT.MaSP = LS.MaSP
    ORDER BY LS.NgayCapNhat DESC
) AS LSCuoiCung
WHERE
    HD.TrangThai = 1 -- Lọc theo hóa đơn có trạng thái "đã thanh toán" (hoặc theo điều kiện của bạn)
GROUP BY
    HD.NgayLapHD;