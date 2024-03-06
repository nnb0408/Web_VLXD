using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VatLieuXayDung.Models;
using VatLieuXayDung.Common;
using System.Configuration;

namespace VatLieuXayDung.Controllers
{
    public class GioHangController : Controller
    {
        QLVLXDDataContext db = new QLVLXDDataContext();

        //
        // GET: /GioHang/

        public ActionResult LayGioHang(int? page, int? index, int? flag, int? ptgiam, string magiamgia)
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            var gg = Session["giamgia"] as string;
            if (string.IsNullOrEmpty(gg))
                Session["giamgia"] = 0;

            if (!index.HasValue)
            {
                index = 0;
                Session["magiamgia"] = "Không có mã giảm giá";
            }

            if (!flag.HasValue)
                flag = 0;

            if (!ptgiam.HasValue)
                ptgiam = 0;

            if (string.IsNullOrEmpty(magiamgia))
                magiamgia = "Không có mã giảm giá";

            var cartQuery = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user);

            Session["favorite"] = db.SanPham_YeuThiches.Where(t => t.KhachHang.TenKH == user).Count();
            Session["cart"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Count();
            Session["tongtien"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Sum(t => t.SanPham.GiaBan * t.SoLuong).ToString();

            var tongtien = Session["tongtien"] as string;
            if (string.IsNullOrEmpty(tongtien))
                Session["tongtien"] = 0;
            if (!string.IsNullOrEmpty(tongtien))
                Session["tongtien"] = Convert.ToInt32(tongtien);

            var tt = Convert.ToInt32(Session["tongtien"]);
            Session["tiencoc"] = (int)(tt * 0.4);
            if (flag == 0)
            {
                var giamgia = (int)(tt * (ptgiam / 100.0));
                Session["giamgia"] = giamgia;
                Session["tongcong"] = tt - giamgia;
                //ggall = giamgia;
                //ttall = tt - giamgia;
            }
            else
            {
                Session["tongcong"] = tt;
                //ttall = tt;
            }

            //Session["a"] = ggall;
            //Session["b"] = ttall;

            int itemsPerPage = 4;
            int pageNumber = (page ?? 1);

            var cart = cartQuery.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            var km = db.KhuyenMais.OrderByDescending(t => t.PhanTramGiam).Take(db.KhuyenMais.Count() - 1).ToList();

            if (cart.Count == 0)
            {
                Session["thongbao"] = "Không có gì trong giỏ hàng";
                return RedirectToAction("GioHangRong", "GioHang");
            }

            ViewBag.TotalPages = Math.Ceiling((double)cartQuery.Count() / itemsPerPage);
            ViewBag.CurrentPage = pageNumber;

            GioHang gioHang = new GioHang
            {
                SanPham = cart,
                KhuyenMai = km,
                Index = index,
                Flag = flag,
                Magiamgia = magiamgia
            };
            return View(gioHang);
        }

        //[HttpPost]
        //public ActionResult LayGioHang(int soluong, string coupon_code)
        //{
        //    var user = Session["user"] as string;
        //    if (string.IsNullOrEmpty(user))
        //        Session["user"] = string.Empty;
        //    Session["favorite"] = db.SanPham_YeuThiches.Where(t => t.KhachHang.TenKH == user).Count();
        //    Session["cart"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Count();
        //    Session["tongtien"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Sum(t => t.SanPham.GiaBan).ToString();


        //    var cart = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).ToList();
        //    if (cart.Count == 0)
        //        return RedirectToAction("GioHangRong", "GioHang");
        //    return View(cart);
        //}

        public ActionResult GioHangRong()
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            //var gg = Session["giamgia"] as string;
            //if (string.IsNullOrEmpty(gg))
            //    Session["giamgia"] = 0;

            var cartQuery = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user);

            Session["favorite"] = db.SanPham_YeuThiches.Where(t => t.KhachHang.TenKH == user).Count();
            Session["cart"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Count();
            Session["tongtien"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Sum(t => t.SanPham.GiaBan * t.SoLuong).ToString();
            Session["cxn"] = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Chờ xác nhận").Count();
            Session["clh"] = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Chờ lấy hàng").Count();
            Session["dgh"] = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Đang giao hàng").Count();

            var tongtien = Session["tongtien"] as string;
            if (string.IsNullOrEmpty(tongtien))
                Session["tongtien"] = 0;

            //var tt = Convert.ToInt32(Session["tongtien"]);
            //Session["tiencoc"] = (int)(tt * 0.4);
            //var giamgia = Convert.ToInt32(Session["giamgia"]);
            //Session["tongcong"] = tt - giamgia;
            return View();
        }

        public ActionResult ThemSanPhamVaoGio(SanPham_TrongGioHang spgh, int masp)
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                return RedirectToAction("DangNhap", "NguoiDung");

            var sp = db.SanPham_TrongGioHangs.SingleOrDefault(n => n.KhachHang.TenKH == user && n.MaSP == masp);
            var kh = db.KhachHangs.SingleOrDefault(k => k.TenKH.Equals(user));
            if (sp == null)
            {
                spgh.MaKH = kh.MaKH;
                spgh.MaSP = masp;
                spgh.SoLuong = 1;
                db.SanPham_TrongGioHangs.InsertOnSubmit(spgh);
                ViewBag.TB = "Đã thêm sản phẩm vào giỏ hàng!";
            }
            else
            {
                sp.SoLuong += 1;
                ViewBag.TB = "Đã cập nhật số lượng sản phẩm vào giỏ hàng!";
            }
            db.SubmitChanges();

            return RedirectToAction("LayGioHang", "GioHang");
        }

        public ActionResult XoaSanPhamTrongGio(int masp)
        {
            var user = Session["user"] as string;

            SanPham_TrongGioHang sp = db.SanPham_TrongGioHangs.Where(n => n.KhachHang.TenKH == user).Where(m => m.MaSP == masp).Single();
            var favorite = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).ToList();
            if (sp != null)
            {
                db.SanPham_TrongGioHangs.DeleteOnSubmit(sp);
                db.SubmitChanges();
                ViewBag.TB = "Đã xóa sản phẩm khỏi giỏ hàng!";
                return RedirectToAction("LayGioHang", "GioHang");
            }
            else
                ViewBag.TB = "Xóa thất bại!";
            if (favorite.Count == 0)
                return RedirectToAction("GioHangRong", "GioHang");
            return RedirectToAction("LayGioHang", "GioHang");
        }

        public ActionResult CapNhatGioHang(SanPham_TrongGioHang spgh, int masp, string soluong)
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                return RedirectToAction("DangNhap", "NguoiDung");

            var sp = db.SanPham_TrongGioHangs.SingleOrDefault(n => n.KhachHang.TenKH == user && n.MaSP == masp);
            var kh = db.KhachHangs.SingleOrDefault(k => k.TenKH.Equals(user));
            if (sp == null)
            {
                spgh.MaKH = kh.MaKH;
                spgh.MaSP = masp;
                spgh.SoLuong = Convert.ToInt32(soluong);
                db.SanPham_TrongGioHangs.InsertOnSubmit(spgh);
                ViewBag.TB = "Đã thêm sản phẩm vào giỏ hàng!";
            }
            else
            {
                sp.SoLuong += Convert.ToInt32(soluong);
                ViewBag.TB = "Đã cập nhật số lượng sản phẩm vào giỏ hàng!";
            }
            db.SubmitChanges();
            return RedirectToAction("LayGioHang", "GioHang");
        }

        public ActionResult CapNhatGioHangSo2(int masp, string soluong)
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                return RedirectToAction("DangNhap", "NguoiDung");

            //var soluong = Request.Form["soluong_" + masp];
            var sp = db.SanPham_TrongGioHangs.SingleOrDefault(n => n.KhachHang.TenKH == user && n.MaSP == masp);

            if (sp != null)
            {
                sp.SoLuong = Convert.ToInt32(soluong);
                ViewBag.TB = "Đã cập nhật số lượng sản phẩm vào giỏ hàng!";
            }
            db.SubmitChanges();
            return RedirectToAction("LayGioHang", "GioHang");
        }

        public ActionResult GiamGia(string coupon_code)
        {
            string[] ma = coupon_code.Split(' ');
            int flagg = 0;
            int indexx = 0;
            int? phantramgiam = 0;
            var giamgia = db.KhuyenMais.SingleOrDefault(t => t.MaKhuyenMai.Equals(ma[0]));

            if (giamgia != null)
            {
                Session["coupon"] = "Bạn đang áp dụng mã giảm giá " + giamgia.MaKhuyenMai + " giảm " + giamgia.PhanTramGiam + "%. Hạn sử dụng từ " + giamgia.NgayApDung + " - " + giamgia.NgayHetHan;
                phantramgiam = giamgia.PhanTramGiam;
                if (giamgia.NgayHetHan < DateTime.Now.Date)
                    flagg = 1;
            }
            else
            {
                Session["coupon"] = "Lỗi áp dụng mã khuyến mãi";
                flagg = 1;
            }

            if (coupon_code.Equals("Không có mã giảm giá"))
            {
                indexx = 0;
                phantramgiam = 0;
            }
            else
                indexx = 1;
            return RedirectToAction("LayGioHang", "GioHang", new { index = indexx, flag = flagg, ptgiam = phantramgiam, magiamgia = giamgia.MaKhuyenMai });
        }

        public ActionResult DatHang(string coupon, string coupon_code)
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                return RedirectToAction("DangNhap", "NguoiDung");

            var tongtien = Session["tongtien"] + "";
            if (string.IsNullOrEmpty(tongtien))
            {
                Session["tongtien"] = 0;
                Session["thongbao"] = "Không có gì trong giỏ hàng";
                return RedirectToAction("GioHangRong", "GioHang");
            }

            var gg = Session["giamgia"] as string;
            if (string.IsNullOrEmpty(gg))
                Session["giamgia"] = 0;

            if (string.IsNullOrEmpty(coupon_code))
                coupon_code = "Không có mã giảm giá";

            //Session["tongtien"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Sum(t => t.SanPham.GiaBan * t.SoLuong).ToString();
            //var s = Session["tongtien"] as string;
            //if (!string.IsNullOrEmpty(s))
            //    Session["tongtien"] = Convert.ToInt32(s);

            //if (alltong == 0)
            //{
            //    Session["tongtien"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Sum(t => t.SanPham.GiaBan * t.SoLuong).ToString();
            //    alltong = Convert.ToInt32(Session["tongtien"]);
            //}

            var tt = Convert.ToInt32(Session["tongtien"]);
            Session["tiencoc"] = (int)(tt * 0.4);
            Session["giamgia"] = Convert.ToInt32(coupon);
            var giamgia = Convert.ToInt32(coupon);
            Session["tongcong"] = tt - giamgia;

            var listsanpham = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).ToList();
            var kh = db.KhachHangs.SingleOrDefault(k => k.TenKH.Equals(user));

            User_DatHang user_DatHang = new User_DatHang
            {
                SanPham = listsanpham,
                KhachHang = kh,
                Magiamgia = coupon_code,
                Giamgia = giamgia
            };
            return View(user_DatHang);
        }

        public ActionResult XacNhanDatHang(HoaDon hd, List<ChiTietHoaDon> cthd, string ten, string diachi, string sdt, string email, string ghichu, string coupon_code, int coupon)
        {
            cthd = new List<ChiTietHoaDon>();
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            var gg = Session["giamgia"] as string;
            if (string.IsNullOrEmpty(gg))
                Session["giamgia"] = 0;

            //var magiamgia = Session["magiamgia"] as string;
            //if (string.IsNullOrEmpty(gg))
            //{
            //    Session["magiamgia"] = "Không có mã giảm giá";
            //    magiamgia = Session["magiamgia"] as string;
            //}

            var tt = Convert.ToInt32(Session["tongtien"]);
            Session["tongcong"] = tt - coupon;
            Session["tiencoc"] = (int)(tt * 0.4);
            var tiencoc = Convert.ToInt32(Session["tiencoc"]);
            var tc = Convert.ToInt32(Session["tongcong"]);

            var kh = db.KhachHangs.SingleOrDefault(k => k.TenKH.Equals(user));
            var hoadon = db.HoaDons.Where(t => t.KhachHang.TenKH == user).ToList();
            var listsanpham = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).ToList();

            var sohoadon = 1;

            if (hoadon.Count == 0)
            {
                hd.MaKH = kh.MaKH;
                hd.NgayLapHD = DateTime.Now;
                hd.SoHoaDon = 1;
                hd.TenNguoiNhan = ten;
                hd.DiaChi = diachi;
                hd.SDT = sdt;
                hd.Email = email;
                hd.ThanhTien = tc;
                hd.TienCoc = tiencoc;
                hd.MaKhuyenMai = coupon_code;
                hd.GhiChu = ghichu;
                hd.TrangThai = false;
                hd.TrangThaiDonHang = "Chờ xác nhận";
                db.HoaDons.InsertOnSubmit(hd);
                db.SubmitChanges();
            }
            else
            {
                var n = hoadon.Count;
                for (int i = 0; i < n; i++)
                {
                    if(hoadon[i].SoHoaDon == null)
                    {
                        hoadon[i].SoHoaDon = 1;
                        db.SubmitChanges();
                    }
                    else if (hoadon[i].SoHoaDon == sohoadon)
                    {
                        sohoadon = hoadon[i].SoHoaDon.Value + 1;
                    }
                    sohoadon = hoadon[i].SoHoaDon.Value + 1;
                }
                hd.MaKH = kh.MaKH;
                hd.NgayLapHD = DateTime.Now;
                hd.SoHoaDon = sohoadon;
                hd.TenNguoiNhan = ten;
                hd.DiaChi = diachi;
                hd.SDT = sdt;
                hd.Email = email;
                hd.ThanhTien = tc;
                hd.TienCoc = tiencoc;
                hd.MaKhuyenMai = coupon_code;
                hd.GhiChu = ghichu;
                hd.TrangThai = false;
                hd.TrangThaiDonHang = "Chờ xác nhận";
                db.HoaDons.InsertOnSubmit(hd);
                db.SubmitChanges();
            }
            var hoadon2 = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.SoHoaDon == sohoadon).SingleOrDefault();
            var sp = listsanpham.Count;
            for (int i = 0; i < sp; i++)
            {
                ChiTietHoaDon chitiethoadon = new ChiTietHoaDon();
                chitiethoadon.MaHD = hoadon2.MaHD;
                chitiethoadon.MaSP = listsanpham[i].MaSP;
                chitiethoadon.SoLuong = listsanpham[i].SoLuong;
                chitiethoadon.GiaBan = listsanpham[i].SanPham.GiaBan;
                chitiethoadon.ThanhTien = listsanpham[i].SoLuong * listsanpham[i].SanPham.GiaBan;
                cthd.Add(chitiethoadon);
            }
            db.ChiTietHoaDons.InsertAllOnSubmit(cthd);

            var strSanPham = "";
            foreach (var item in listsanpham)
            {
                strSanPham += "<tr>";
                strSanPham += "<td class = 'tdten'> " + item.SanPham.TenSP + "</td>";
                strSanPham += "<td class = 'tdsl'> " + item.SoLuong + "</td>";
                strSanPham += "<td class = 'tdsl'> " + string.Format("{0:N0}", (item.SoLuong * item.SanPham.GiaBan)) + "<span> ₫</span> </td>";
                //strSanPham += "<td>" + item.SoLuong + "</td>";
                //strSanPham += "<td>" + string.Format("{0:N0}", (item.SoLuong * item.SanPham.GiaBan)) + "</td> <span>₫</span>";
                strSanPham += "</tr>";
            }
            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2.html"));
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", kh.TenKH);
            contentCustomer = contentCustomer.Replace("{{MaDon}}", hoadon2.MaHD.ToString());
            contentCustomer = contentCustomer.Replace("{{NgayDat}}", hoadon2.NgayLapHD.ToString());
            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", Mail.FormatTien(tt));
            contentCustomer = contentCustomer.Replace("{{GiamGia}}", Mail.FormatTien(coupon));
            contentCustomer = contentCustomer.Replace("{{TienCoc}}", Mail.FormatTien(tiencoc));
            contentCustomer = contentCustomer.Replace("{{TongTien}}", Mail.FormatTien(tc));
            contentCustomer = contentCustomer.Replace("{{TenNguoiNhan}}", ten.ToString());
            contentCustomer = contentCustomer.Replace("{{DiaChiNhan}}", diachi.ToString());
            contentCustomer = contentCustomer.Replace("{{SoNguoiNhan}}", sdt.ToString());
            contentCustomer = contentCustomer.Replace("{{EmailNguoiNhan}}", email.ToString());
            contentCustomer = contentCustomer.Replace("{{GhiChu}}", ghichu.ToString());
            Mail.SendMail("Pynbatu", "Đơn hàng #" + hoadon2.MaHD.ToString(), contentCustomer, email);

            string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send1.html"));
            contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", kh.TenKH);
            contentAdmin = contentAdmin.Replace("{{MaDon}}", hoadon2.MaHD.ToString());
            contentAdmin = contentAdmin.Replace("{{NgayDat}}", hoadon2.NgayLapHD.ToString());
            contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
            contentAdmin = contentAdmin.Replace("{{ThanhTien}}", Mail.FormatTien(tt));
            contentAdmin = contentAdmin.Replace("{{GiamGia}}", Mail.FormatTien(coupon));
            contentAdmin = contentAdmin.Replace("{{TienCoc}}", Mail.FormatTien(tiencoc));
            contentAdmin = contentAdmin.Replace("{{TongTien}}", Mail.FormatTien(tc));
            contentAdmin = contentAdmin.Replace("{{DiaChi}}", kh.DiaChi.ToString());
            contentAdmin = contentAdmin.Replace("{{SoDienThoai}}", kh.SDT.ToString());
            contentAdmin = contentAdmin.Replace("{{Email}}", kh.Email.ToString());
            contentAdmin = contentAdmin.Replace("{{TenNguoiNhan}}", ten.ToString());
            contentAdmin = contentAdmin.Replace("{{DiaChiNhan}}", diachi.ToString());
            contentAdmin = contentAdmin.Replace("{{SoNguoiNhan}}", sdt.ToString());
            contentAdmin = contentAdmin.Replace("{{EmailNguoiNhan}}", email.ToString());
            contentAdmin = contentAdmin.Replace("{{GhiChu}}", ghichu.ToString());
            Mail.SendMail("Pynbatu", "Đơn hàng #" + hoadon2.MaHD.ToString(), contentAdmin, ConfigurationManager.AppSettings["AdminEmail4"]);

            db.SanPham_TrongGioHangs.DeleteAllOnSubmit(listsanpham);
            db.SubmitChanges();
            Session["thongbao"] = "Bạn đã đặt hàng thành công, giỏ hàng rỗng";
            return RedirectToAction("GioHangRong", "GioHang");
        }
    }
}
