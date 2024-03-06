using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VatLieuXayDung.Models;

namespace VatLieuXayDung.Controllers
{
    public class SanPhamController : Controller
    {

        QLVLXDDataContext db = new QLVLXDDataContext();
        //
        // GET: /SanPham/

        public ActionResult ShowAllSanPham(int? page)
        {
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
                Session["user"] = string.Empty;

            Session["magiamgia"] = "Không có mã giảm giá";
            Session["favorite"] = db.SanPham_YeuThiches.Where(t => t.KhachHang.TenKH == user).Count();
            Session["cart"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Count();
            Session["tongtien"] = db.SanPham_TrongGioHangs.Where(t => t.KhachHang.TenKH == user).Sum(t => t.SanPham.GiaBan * t.SoLuong).ToString();
            Session["cxn"] = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Chờ xác nhận").Count();
            Session["clh"] = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Chờ lấy hàng").Count();
            Session["dgh"] = db.HoaDons.Where(t => t.KhachHang.TenKH == user && t.TrangThaiDonHang == "Đang giao hàng").Count();

            var s = Session["tongtien"] as string;
            if(!string.IsNullOrEmpty(s))
                Session["tongtien"] = Convert.ToInt32(s);

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

            int itemsPerPage = 9;
            int pageNumber = (page ?? 1);

            var listSanPham = db.SanPhams.OrderBy(t => t.TenSP).Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
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
                ListSanPham = listSanPham,
                ListLoaiVL = listvl,
                ListLoaiTB = listtb,
                ListSL = lsll
            };
            ViewBag.TotalPages = Math.Ceiling((double)db.SanPhams.Count() / itemsPerPage);
            ViewBag.CurrentPage = pageNumber;
            return View(listallsp);
        }

        public ActionResult XemSanPhamTheoLoai(int? page, int loaisp)
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

            int itemsPerPage = 9;
            int pageNumber = (page ?? 1);

            var listSanPham = db.SanPhams.OrderBy(t => t.TenSP).Where(t => t.MaLoai == loaisp).Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
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
                ListSanPham = listSanPham,
                ListLoaiVL = listvl,
                ListLoaiTB = listtb,
                ListSL = lsll
            };
            ViewBag.TotalPages = Math.Ceiling((double)listSanPham.Count() / itemsPerPage);
            ViewBag.CurrentPage = pageNumber;
            return View(listallsp);
        }

        public ActionResult TimKiemSanPham(int? page, string search)
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

            int itemsPerPage = 9;
            int pageNumber = (page ?? 1);

            var listSanPham = db.SanPhams.OrderBy(t => t.TenSP).Where(t => t.TenSP.Contains(search)).Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            var listvl = db.LoaiSanPhams.OrderBy(t => t.MaLoai).Where(t => t.MoTa == "VL").ToList();
            var listtb = db.LoaiSanPhams.OrderBy(t => t.MaLoai).Where(t => t.MoTa == "TB").ToList();
            var listloai = db.LoaiSanPhams.OrderBy(t => t.MaLoai).ToList();

            if(listSanPham.Count == 0)
            {
                Session["search"] = "Không có sản phẩm "+'"'+ search + '"';
            }
            else
            {
                Session["search"] = "Có " + listSanPham.Count + " sản phẩm " + '"' + search + '"';
            }
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
                ListSanPham = listSanPham,
                ListLoaiVL = listvl,
                ListLoaiTB = listtb,
                ListSL = lsll
            };
            ViewBag.TotalPages = Math.Ceiling((double)listSanPham.Count() / itemsPerPage);
            ViewBag.CurrentPage = pageNumber;
            return View(listallsp);
        }

        public ActionResult XemVatLieu(int? page)
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

            int itemsPerPage = 9;
            int pageNumber = (page ?? 1);

            var listSanPham = db.SanPhams.OrderBy(t => t.TenSP).Where(t => t.LoaiSanPham.MoTa == "VL").Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
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
                ListSanPham = listSanPham,
                ListLoaiVL = listvl,
                ListLoaiTB = listtb,
                ListSL = lsll
            };
            ViewBag.TotalPages = Math.Ceiling((double)listSanPham.Count() / itemsPerPage);
            ViewBag.CurrentPage = pageNumber;
            return View(listallsp);
        }

        public ActionResult XemThietBi(int? page)
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

            int itemsPerPage = 9;
            int pageNumber = (page ?? 1);

            var listSanPham = db.SanPhams.OrderBy(t => t.TenSP).Where(t => t.LoaiSanPham.MoTa == "TB").Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
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
                ListSanPham = listSanPham,
                ListLoaiVL = listvl,
                ListLoaiTB = listtb,
                ListSL = lsll
            };
            ViewBag.TotalPages = Math.Ceiling((double)listSanPham.Count() / itemsPerPage);
            ViewBag.CurrentPage = pageNumber;
            return View(listallsp);
        }

        public ActionResult XemChiTietSanPham(int masp, int maloai, int? flag)
        {
            if (!flag.HasValue)
                flag = 0;
            if (flag == 1)
                Session["DG"] = "Bạn đã đánh giá sản phẩm này";
            else
                Session["DG"] = "";

            var sanpham = db.SanPhams.SingleOrDefault(t => t.MaSP == masp);
            var listsanpham = db.SanPhams.Where(t => t.MaLoai == maloai).ToList();
            var listdanhgia = db.DANHGIAs.Where(t => t.MaSP == masp).ToList();
            SanPhamViewModel viewModel = new SanPhamViewModel
            {
                SanPham = sanpham,
                ListSanPham = listsanpham,
                ListDanhGia = listdanhgia
            };
            return View(viewModel);
        }

        public ActionResult SanPhamCungLoaiPartial()
        {
            //int loaisp = Convert.ToInt32(Session["loaisp"]);
            //var listsanpham = db.SanPhams.Where(t => t.MaLoai == loaisp).ToList();
            //ViewBag.ListSanPham = listsanpham;
            return View();
        }

        public ActionResult DanhGia(DANHGIA dg, string tieude, string comment, int sao, int maSp)
        {
            int Flag = 1;
            var user = Session["user"] as string;
            if (string.IsNullOrEmpty(user))
            {
                Session["user"] = string.Empty;
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            var anh = "";
            var kh = db.KhachHangs.SingleOrDefault(t => t.TenKH == user);
            var sanpham = db.SanPhams.SingleOrDefault(t => t.MaSP == maSp);
            var danhgia = db.DANHGIAs.SingleOrDefault(t => t.MaKH == kh.MaKH && t.MaSP == maSp);

            if (kh.Phai == "Nam")
                anh = "review_2.jpg";
            else
                anh = "review_1.jpg";

            if (danhgia == null)
            {
                dg.MaKH = kh.MaKH;
                dg.MaSP = sanpham.MaSP;
                dg.TIEUDE = tieude;
                dg.NOIDUNG = comment;
                dg.SOSAO = sao;
                dg.HINHANH = anh;
                dg.NGAYDG = DateTime.Now;
                db.DANHGIAs.InsertOnSubmit(dg);
                Flag = 0;
            }
            else
                return RedirectToAction("XemChiTietSanPham", "SanPham", new { masp = maSp, maloai = sanpham.MaLoai, flag = Flag});
            db.SubmitChanges();
            return RedirectToAction("XemChiTietSanPham", "SanPham", new { masp = maSp, maloai = sanpham.MaLoai, flag = Flag });
        }
    }
}
