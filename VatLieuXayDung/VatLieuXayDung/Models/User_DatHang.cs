using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VatLieuXayDung.Models
{
    public class User_DatHang
    {
        public IEnumerable<SanPham_TrongGioHang> SanPham { get; set; }
        public KhachHang KhachHang { get; set; }
        public string Magiamgia { get; set; }
        public int Giamgia { get; set; }
    }
}