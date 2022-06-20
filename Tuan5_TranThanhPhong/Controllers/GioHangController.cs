using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan5_TranThanhPhong.Models;
namespace Tuan5_TranThanhPhong.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        MyDataDataContext data = new MyDataDataContext();

        public List<Giohang> LayGH()
        {
            List<Giohang> listGH = Session["Giohang"] as List<Giohang>;
            if (listGH == null)
            {
                listGH = new List<Giohang>();
                Session["Giohang"] = listGH;
            }
            return listGH;
        }
        public ActionResult ThemGioHang(int id, string strUrl)
        {
            List<Giohang> listGH = LayGH();
            Giohang sp = listGH.Find(n => n.masach == id);
            if (sp == null)
            {
                sp = new Giohang(id);
                listGH.Add(sp);
                return Redirect(strUrl);
            }
            else
            {
                sp.iSoLuong++;
                return Redirect(strUrl);
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<Giohang> listGH = Session["Giohang"] as List<Giohang>;
            if (listGH != null)
            {
                tsl = listGH.Sum(n => n.iSoLuong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tslsp = 0;
            List<Giohang> listGH = Session["Giohang"] as List<Giohang>;
            if (listGH != null)
            {
                tslsp = listGH.Count;
            }
            return tslsp;
        }
        private Double TongTien()
        {
            double tt = 0;
            List<Giohang> listGH = Session["Giohang"] as List<Giohang>;
            if (listGH != null)
            {
                tt = listGH.Sum(n => n.dThanhTien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<Giohang> listGH = LayGH();
            if (listGH.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGH);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult XoaGioHang(int id)
        {
            List<Giohang> listGH = LayGH();
            Giohang sp = listGH.SingleOrDefault(n => n.masach == id);
            if (sp != null)
            {
                listGH.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
            }
            if (listGH.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<Giohang> listGH = LayGH();
            Giohang sp = listGH.SingleOrDefault(n => n.masach == id);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(collection["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> listGH = LayGH();
            listGH.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<Giohang> listGH = LayGH();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGH);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();

            List<Giohang> gh = LayGH();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = item.masach;
                ctdh.soluong = item.iSoLuong;
                ctdh.gia = (decimal)item.giaBan;
                s = data.Saches.Single(n => n.masach == item.masach);
                s.soluongton -= ctdh.soluong;
                data.SubmitChanges();

                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonHang", "GioHang");
        }
        public ActionResult XacnhanDonhang()
        {
            return View();
        }
    }
}