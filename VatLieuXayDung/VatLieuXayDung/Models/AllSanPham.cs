using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VatLieuXayDung.Models
{
    public class AllSanPham
    {
        public IEnumerable<SanPham> ListSanPham { get; set; }
        public IEnumerable<LoaiSanPham> ListLoaiVL { get; set; }
        public IEnumerable<LoaiSanPham> ListLoaiTB { get; set; }
        public IEnumerable<SoLuongLoaiSP> ListSL { get; set; }
    }
}