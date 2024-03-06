using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VatLieuXayDung.Models;
namespace VatLieuXayDung.Controllers
{
   
    public class HoaDonController : Controller
    {
        QLVLXDDataContext db = new QLVLXDDataContext();
        //
        // GET: /HoaDon/

        //public ActionResult HoaDonChoXacNhan()
        //{
        //    var user = Session["user"] as string;
        //    if (string.IsNullOrEmpty(user))
        //        Session["user"] = string.Empty;

        //    List<ChoXacNhan> listcxn = new List<ChoXacNhan>();
        //    ChoXacNhan cxn = new ChoXacNhan();
        //    cxn.LstCTHD = new List<ChiTietHoaDon>();
        //    cxn.lstHoaDon = new List<HoaDon>();

        //    var lsthoadon = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Chờ xác nhận").ToList();
        //    cxn.lstHoaDon = lsthoadon;
        //    if (lsthoadon.Count > 0)
        //    {
        //        for (int i = 0; i < lsthoadon.Count; i++)
        //        {
        //            var lstcthd = db.ChiTietHoaDons.Where(t => t.MaHD == lsthoadon[i].MaHD).ToList();
        //            for(int j = 0; j < lstcthd.Count; j++)
        //            {
        //                var cthd = db.ChiTietHoaDons.SingleOrDefault(t => t.MaSP == lstcthd[j].MaSP);
        //                cxn.LstCTHD.Add(cthd);
        //            }
        //            listcxn.Add(cxn);
        //        }
        //    }
        //    return View(listcxn);
        //}

        public ActionResult HoaDonChoXacNhan2()
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            ChoXacNhan cxn = new ChoXacNhan();
            cxn.LstCTHD = new List<ChiTietHoaDon>();
            cxn.lstHoaDon = new List<HoaDon>();

            var lsthoadon = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Chờ xác nhận").ToList();
            if(lsthoadon.Count == 0)
            {
                return RedirectToAction("ChoXacNhanRong", "HoaDon");
            }
            cxn.lstHoaDon = lsthoadon;
            if (lsthoadon.Count > 0)
            {
                for (int i = 0; i < lsthoadon.Count; i++)
                {
                    var lstcthd = db.ChiTietHoaDons.Where(t => t.MaHD == lsthoadon[i].MaHD).ToList();
                    for (int j = 0; j < lstcthd.Count; j++)
                    {
                        var cthd = db.ChiTietHoaDons.SingleOrDefault(t => t.MaSP == lstcthd[j].MaSP && t.MaHD == lstcthd[j].MaHD);
                        cxn.LstCTHD.Add(cthd);
                    }
                }
            }
            return View(cxn);
        }

        public ActionResult HoaDonChoLayHang()
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            ChoXacNhan cxn = new ChoXacNhan();
            cxn.LstCTHD = new List<ChiTietHoaDon>();
            cxn.lstHoaDon = new List<HoaDon>();

            var lsthoadon = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Chờ lấy hàng").ToList();
            if (lsthoadon.Count == 0)
            {
                return RedirectToAction("ChoLayHangRong", "HoaDon");
            }
            cxn.lstHoaDon = lsthoadon;
            if (lsthoadon.Count > 0)
            {
                for (int i = 0; i < lsthoadon.Count; i++)
                {
                    var lstcthd = db.ChiTietHoaDons.Where(t => t.MaHD == lsthoadon[i].MaHD).ToList();
                    for (int j = 0; j < lstcthd.Count; j++)
                    {
                        var cthd = db.ChiTietHoaDons.SingleOrDefault(t => t.MaSP == lstcthd[j].MaSP && t.MaHD == lstcthd[j].MaHD);
                        cxn.LstCTHD.Add(cthd);
                    }
                }
            }
            return View(cxn);
        }

        public ActionResult HoaDonDaXacNhan()
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            ChoXacNhan cxn = new ChoXacNhan();
            cxn.LstCTHD = new List<ChiTietHoaDon>();
            cxn.lstHoaDon = new List<HoaDon>();

            var lsthoadon = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Đang giao hàng").ToList();
            if (lsthoadon.Count == 0)
            {
                return RedirectToAction("DangGiaoHang", "HoaDon");
            }
            cxn.lstHoaDon = lsthoadon;
            if (lsthoadon.Count > 0)
            {
                for (int i = 0; i < lsthoadon.Count; i++)
                {
                    var lstcthd = db.ChiTietHoaDons.Where(t => t.MaHD == lsthoadon[i].MaHD).ToList();
                    for (int j = 0; j < lstcthd.Count; j++)
                    {
                        var cthd = db.ChiTietHoaDons.SingleOrDefault(t => t.MaSP == lstcthd[j].MaSP && t.MaHD == lstcthd[j].MaHD);
                        cxn.LstCTHD.Add(cthd);
                    }
                }
            }
            return View(cxn);
        }

        public ActionResult ChoXacNhanRong()
        {
            return View();
        }

        public ActionResult ChoLayHangRong()
        {
            return View();
        }

        public ActionResult DangGiaoHangRong()
        {
            return View();
        }
    }
}
