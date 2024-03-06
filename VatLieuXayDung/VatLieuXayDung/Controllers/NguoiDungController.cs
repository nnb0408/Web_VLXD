using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VatLieuXayDung.Models;

namespace VatLieuXayDung.Controllers
{
    public class NguoiDungController : Controller
    {
        QLVLXDDataContext db = new QLVLXDDataContext();
        //
        // GET: /NguoiDung/

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(KhachHang kh, string fullname, string username, DateTime date, string password, string gender, string repassword, string address, string email, string phone)
        {
            if (password == repassword)
            {
                KhachHang check = db.KhachHangs.SingleOrDefault(n => n.UserName.Equals(username));
                KhachHang sdtkh = db.KhachHangs.SingleOrDefault(n => n.SDT.Equals(phone));
                if (check == null)
                {
                    if(date <= DateTime.Now.Date)
                    {
                        if (sdtkh == null)
                        {
                            kh.TenKH = fullname;
                            kh.Phai = gender;
                            kh.NgaySinh = date;
                            kh.DiaChi = address;
                            kh.SDT = phone;
                            kh.UserName = username;
                            kh.MatKhau = password;
                            kh.Email = email;
                            kh.TrangThai = "Không khóa";
                            db.KhachHangs.InsertOnSubmit(kh);
                            db.SubmitChanges();
                            ViewBag.TB = "Đăng ký thành công!";
                        }
                        else
                            ViewBag.TB = "Số điện thoại đã đã được đăng ký!";
                    }
                    else
                        ViewBag.TB = "Ngày sinh không được lớn hơn ngày hiện tại!";
                }
                else
                    ViewBag.TB = "Tên tài khoản đã có người đăng ký!";
            }
            else
                ViewBag.TB = "Nhập lại mật khẩu không chính xác, vui lòng nhập lại!";
            return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(string username, string password)
        {
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.UserName.Equals(username) && n.MatKhau.Equals(password));
                if (kh != null)
                {
                    if(kh.TrangThai.Equals("Khóa"))
                    {
                        ViewBag.TB = "Tài khoản của bạn đã bị khóa, vui lòng liên hệ 0569512477 để khôi phục!";
                    }
                    else
                    {
                        Session["user"] = kh.TenKH;
                        return RedirectToAction("ShowAllSanPham", "SanPham");
                    }
                }
                else
                {
                    ViewBag.TB = "Tài khoản hoặc mật khẩu không chính xác, vui lòng nhập lại!";
                }
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["user"] = string.Empty;
            return RedirectToAction("ShowAllSanPham", "SanPham");
        }

        public ActionResult ThongTinCaNhan(int? flag)
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            if (!flag.HasValue)
            {
                flag = 0;
                Session["thongtin"] = "";
            }

            var khachhang = db.KhachHangs.SingleOrDefault(t => t.TenKH.Equals(user));
            if(khachhang == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            return View(khachhang);
        }

        public ActionResult LuuThongTin(string ten, string ngay, string thang, string nam, string gioitinh, string diachi, string sdt, string email)
        {
            int flagg = 0;
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            var khachhang = db.KhachHangs.SingleOrDefault(t => t.TenKH.Equals(user));

            if (thang == "4" || thang == "6" || thang == "9" || thang == "11")
            {
                if (Convert.ToInt32(ngay) > 30)
                {
                    flagg = 1;
                    Session["thongtin"] = "Tháng " + thang + " Không có 31 ngày, vui lòng nhập lại!";
                    return RedirectToAction("ThongTinCaNhan", "NguoiDung", new { flag = flagg });
                }
            }
            else if (thang == "2")
            {
                if (Convert.ToInt32(ngay) > 28)
                {
                    flagg = 1;
                    Session["thongtin"] = "Tháng " + thang + " chỉ có 28 ngày, vui lòng nhập lại!";
                    return RedirectToAction("ThongTinCaNhan", "NguoiDung", new { flag = flagg });
                }
            }
            DateTime ngaysinh = new DateTime(Convert.ToInt32(nam), Convert.ToInt32(thang), Convert.ToInt32(ngay));
            if (khachhang != null)
            {
                khachhang.TenKH = ten;
                khachhang.NgaySinh = ngaysinh;
                khachhang.DiaChi = diachi;
                khachhang.SDT = sdt;
                khachhang.Email = email;
                khachhang.Phai = gioitinh;
                db.SubmitChanges();
                Session["user"] = ten;
                Session["thongtin"] = "Lưu thay đổi thành công!";
            }
            db.SubmitChanges();
            return RedirectToAction("ThongTinCaNhan", "NguoiDung", new { flag = flagg });
        }

        public ActionResult BaoMat(int? flag)
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            if (!flag.HasValue)
            {
                flag = 0;
                Session["thongtin"] = "";
            }

            var khachhang = db.KhachHangs.SingleOrDefault(t => t.TenKH.Equals(user));
            if (khachhang == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            return View(khachhang);
        }

        public ActionResult DoiMatKhau(string mkcu, string mkmoi, string remkmoi)
        {
            int flagg = 0;
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            var khachhang = db.KhachHangs.SingleOrDefault(t => t.TenKH.Equals(user));

            if (mkcu == khachhang.MatKhau)
            {
                if (mkmoi == remkmoi)
                {
                    if (khachhang != null)
                    {
                        khachhang.MatKhau = mkmoi;
                        db.SubmitChanges();
                        Session["thongtin"] = "Đổi mật khẩu thành công!";
                    }
                }
                else
                    Session["thongtin"] = "Nhập lại mật khẩu không chính xác, vui lòng nhập lại!";
            }
            else
                Session["thongtin"] = "Mật khẩu ban đầu chính xác, vui lòng nhập lại!";

            db.SubmitChanges();
            return RedirectToAction("BaoMat", "NguoiDung", new { flag = flagg });
        }
    }
}
