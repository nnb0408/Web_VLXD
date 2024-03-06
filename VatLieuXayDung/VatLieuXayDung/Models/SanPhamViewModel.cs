using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VatLieuXayDung.Models
{
    public class SanPhamViewModel
    {
        public SanPham SanPham { get; set; }
        public IEnumerable<SanPham> ListSanPham { get; set; }
        public IEnumerable<DANHGIA> ListDanhGia { get; set; }
    }
}