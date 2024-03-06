using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VatLieuXayDung.Models
{
    public class GioHang
    {
        public IEnumerable<SanPham_TrongGioHang> SanPham { get; set; }
        public IEnumerable<KhuyenMai> KhuyenMai { get; set; }
        public int? Index { get; set; }
        public int? Flag { get; set; }
        public string Magiamgia { get; set; }
    }
}