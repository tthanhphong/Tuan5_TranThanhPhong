using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tuan5_TranThanhPhong.Models;

namespace Tuan5_TranThanhPhong.Models
{
    public class Giohang
    {
        MyDataDataContext data = new MyDataDataContext();
        public int masach { get; set; }

        [Display(Name = "Tên sách")]
        public string tenSach { get; set; }

        [Display(Name = "Ảnh bìa")]
        public string hinh { get; set; }

        [Display(Name = "Giá bán")]
        public Double giaBan { get; set; }

        [Display(Name = "Số lượng")]
        public int iSoLuong { get; set; }

        [Display(Name = "Thành tiền")]
        public Double dThanhTien
        {
            get { return iSoLuong * giaBan; }
        }

        public Giohang(int id)
        {
            masach = id;
            Sach s = data.Saches.Single(n => n.masach == masach);
            tenSach = s.tensach;
            hinh = s.hinh;
            giaBan = double.Parse(s.giaban.ToString());
            iSoLuong = 1;
        }
    }
}