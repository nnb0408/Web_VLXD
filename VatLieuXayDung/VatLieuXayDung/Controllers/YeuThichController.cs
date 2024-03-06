using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VatLieuXayDung.Models;

namespace VatLieuXayDung.Controllers
{
    public class YeuThichController : Controller
    {
        QLVLXDDataContext db = new QLVLXDDataContext();
        //
        // GET: /YeuThich/

        public ActionResult SanPhamYeuThich()
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            Session["favorite"] = db.SanPham_YeuThiches.Where(t => t.KhachHang.TenKH == user).Count();
            Session["cart"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Count();
            Session["tongtien"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Sum(t => t.SanPham.GiaBan).ToString();

            var settim = db.SanPhams.OrderBy(t => t.TenSP).ToList();
            if (settim.Count > 0)
            {
                for (int i = 0; i < settim.Count; i++)
                {
                    settim[i].YeuThich = false;
                    db.SubmitChanges();
                }
            }

            if (!string.IsNullOrEmpty(user))
            {
                var kh = db.KhachHangs.SingleOrDefault(t => t.TenKH == user);
                var veyeuthich = db.SanPham_YeuThiches.Where(t => t.MaKH == kh.MaKH).ToList();

                if (veyeuthich.Count > 0)
                {
                    for (int i = 0; i < settim.Count; i++)
                    {
                        for (int j = 0; j < veyeuthich.Count; j++)
                        {
                            if (settim[i].MaSP == veyeuthich[j].MaSP)
                            {
                                settim[i].YeuThich = true;
                                db.SubmitChanges();
                            }
                        }
                    }
                }
            }

            var favorite = db.SanPham_YeuThiches.Where(t => t.KhachHang.TenKH == user).ToList();
            if (favorite.Count == 0)
                return RedirectToAction("YeuThichRong", "YeuThich");
            return View(favorite);
        }

        public ActionResult YeuThichRong()
        {
            return View();
        }

        public ActionResult ThemSanPhamYeuThich(SanPham_YeuThich spyt, int masp)
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                return RedirectToAction("DangNhap", "NguoiDung");

            var sp = db.SanPham_YeuThiches.Where(n => n.KhachHang.TenKH == user).Where(m => m.MaSP == masp).ToList();
            var kh = db.KhachHangs.SingleOrDefault(k => k.TenKH.Equals(user));
            if (sp.Count == 0)
            {
                spyt.MaKH = kh.MaKH;
                spyt.MaSP = masp;
                db.SanPham_YeuThiches.InsertOnSubmit(spyt);
                db.SubmitChanges();
                ViewBag.TB = "Đã thêm sản phẩm vào danh sách yêu thích!";
            }
            else
                ViewBag.TB = "Sản phẩm đã được yêu thích!";

            return RedirectToAction("SanPhamYeuThich", "YeuThich");
        }

        public ActionResult XoaSanPhamYeuThich(int masp)
        {
            var user = Session["user"] as string;

            SanPham_YeuThich sp = db.SanPham_YeuThiches.Where(n => n.KhachHang.TenKH == user).Where(m => m.MaSP == masp).Single();
            var favorite = db.SanPham_YeuThiches.Where(t => t.KhachHang.TenKH == user).ToList();
            if (sp != null)
            {
                db.SanPham_YeuThiches.DeleteOnSubmit(sp);
                db.SubmitChanges();
                ViewBag.TB = "Đã xóa sản phẩm khỏi danh sách yêu thích!";
                return RedirectToAction("SanPhamYeuThich", "YeuThich");
            }
            else
                ViewBag.TB = "Xóa thất bại!";
            if(favorite.Count == 0)
                return RedirectToAction("YeuThichRong", "YeuThich");
            return RedirectToAction("SanPhamYeuThich", "YeuThich");
        }
    }
}
