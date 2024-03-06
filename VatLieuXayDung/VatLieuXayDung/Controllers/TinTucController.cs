using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VatLieuXayDung.Models;

namespace VatLieuXayDung.Controllers
{
    public class TinTucController : Controller
    {
        QLVLXDDataContext db = new QLVLXDDataContext();
        //
        // GET: /TinTuc/

        public ActionResult TinTuc()
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            Session["magiamgia"] = "Không có mã giảm giá";
            Session["favorite"] = db.SanPham_YeuThiches.Where(t => t.KhachHang.TenKH == user).Count();
            Session["cart"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Count();
            Session["tongtien"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Sum(t => t.SanPham.GiaBan * t.SoLuong).ToString();
            var s = Session["tongtien"] as string;
            if (!string.IsNullOrEmpty(s))
                Session["tongtien"] = Convert.ToInt32(s);

            var listvl = db.LoaiSanPhams.OrderBy(t => t.MaLoai).Where(t => t.MoTa == "VL").ToList();
            var listtb = db.LoaiSanPhams.OrderBy(t => t.MaLoai).Where(t => t.MoTa == "TB").ToList();
            var listloai = db.LoaiSanPhams.OrderBy(t => t.MaLoai).ToList();

            List<SoLuongLoaiSP> lsll = new List<SoLuongLoaiSP>();

            for (int i = 0; i < listloai.Count; i++)
            {
                var loai = db.LoaiSanPhams.SingleOrDefault(t => t.MaLoai == listloai[i].MaLoai);
                var sl = db.SanPhams.Where(t => t.MaLoai == loai.MaLoai).ToList();
                int maloai = loai.MaLoai;
                int soluong = sl.Count();
                SoLuongLoaiSP sll = new SoLuongLoaiSP
                {
                    LoaiSP = maloai,
                    SoLuong = soluong
                };
                lsll.Add(sll);
            }

            AllSanPham listallsp = new AllSanPham
            {
                ListLoaiVL = listvl,
                ListLoaiTB = listtb,
                ListSL = lsll
            };
            return View(listallsp);
        }

    }
}
