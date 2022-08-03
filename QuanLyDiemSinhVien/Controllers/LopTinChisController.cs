using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyDiemSinhVien.Constant;
using QuanLyDiemSinhVien.Models;
using QuanLyDiemSinhVien.ViewModels;

namespace QuanLyDiemSinhVien.Controllers
{

    public class LopTinChisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LopTinChis
        public ActionResult Index()
        {
            var lopTinChis = db.LopTinChis.Include(l => l.MonHoc).Include(l => l.NganhDaoTao);
            return View(lopTinChis.ToList());
        }

        // GET: LopTinChis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopTinChi lopTinChi = db.LopTinChis.Find(id);
            if (lopTinChi == null)
            {
                return HttpNotFound();
            }
            var khoaID = db.NganhDaoTaos.FirstOrDefault(x => x.NganhDaoTaoID == lopTinChi.NganhDaoTaoID).KhoaID;
            KichHoatLopTinChi ltc = new KichHoatLopTinChi();
            ViewBag.KichHoat = new SelectList(ltc.GetListLopTinChi(), "Value", "Text", lopTinChi.KichHoat);
            ViewBag.GiangVienID = new SelectList(db.GiangViens, "GiangVienID", "HoTen", lopTinChi.GiangVienID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", khoaID);
            ViewBag.MonHocID = new SelectList(db.MonHocs, "MonHocID", "TenMonHoc", lopTinChi.MonHocID);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", lopTinChi.NganhDaoTaoID);

            var data = db.BangDiems
                   .Join(db.SinhViens, o1 => o1.SinhVienID, o2 => o2.SinhVienID, (o1, o2) =>
                        new DanhSachLopViewModels()
                        {
                            BangDiemID = o1.BangDiemID,

                            LopTinChiID = o1.LopTinChiID
                                                   ,
                            SinhVienID = o1.SinhVienID
                                                   ,
                            MaSinhVien = o2.MaSinhVien
                                                   ,
                            HoTen = o2.HoTen
                                                   ,
                            NgaySinh = o2.NgaySinh
                                                   ,
                            DiemThanhPhan = o1.DiemThanhPhan
                                                   ,
                            DiemThi = o1.DiemThi
                                                   ,
                            DiemTrungBinh = (o1.DiemThanhPhan + o1.DiemThi) /2 //so1.DiemTrungBinh
                                                   ,
                            DiemChu = o1.DiemChu
                        })
                   .Where(X => X.LopTinChiID == lopTinChi.LopTinChiID).ToList();

            ViewData["dataSinhVien"] = data;
            return View(lopTinChi);
        }

        // GET: LopTinChis/Create
        public ActionResult ThemSinhVienVaoLop(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopTinChi lopTinChi = db.LopTinChis.Find(id);
            if (lopTinChi == null)
            {
                return HttpNotFound();
            }
            var khoaID = db.NganhDaoTaos.FirstOrDefault(x => x.NganhDaoTaoID == lopTinChi.NganhDaoTaoID).KhoaID;
            KichHoatLopTinChi ltc = new KichHoatLopTinChi();
            ViewBag.KichHoat = new SelectList(ltc.GetListLopTinChi(), "Value", "Text", lopTinChi.KichHoat);
            ViewBag.GiangVienID = new SelectList(db.GiangViens, "GiangVienID", "HoTen", lopTinChi.GiangVienID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", khoaID);
            ViewBag.MonHocID = new SelectList(db.MonHocs, "MonHocID", "TenMonHoc", lopTinChi.MonHocID);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", lopTinChi.NganhDaoTaoID);
            var data = db.SinhViens.Where(x => x.KhoaID == khoaID && x.TinhTrang == 1).ToList();
            ViewData["dataSinhVien"] = data;
            return View(lopTinChi);
        }

        public JsonResult ActionThemSinhVienVaoLop(string sinhVienID, string lopTinChiID)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (sinhVienID == null || lopTinChiID == null)
            {
                result.Add("message", "Không tìm thấy thông tin.");
                var json2 = Json(result, JsonRequestBehavior.AllowGet);
                return json2;
            }
            var dataSV = sinhVienID.Split(',');
            int resultCount = 0;
            foreach (var item in dataSV)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    int idSV = int.Parse(item);
                    int idLTC = int.Parse(lopTinChiID);
                    int checkCount = db.LopTinChi_SinhVien.Where(x => x.SinhVienID == idSV && x.LopTinhChiID == idLTC).Count();
                    if (checkCount == 0)
                    {
                        //Thêm sv vào lớp
                        LopTinChi_SinhVien ltc_sv = new LopTinChi_SinhVien();
                        ltc_sv.LopTinhChiID = idLTC;
                        ltc_sv.SinhVienID = idSV;
                        db.LopTinChi_SinhVien.Add(ltc_sv);
                        db.SaveChanges();
                        //Thêm bảng điểm cho sinh viên tương ứng với lớp
                        BangDiem bd = new BangDiem();
                        bd.SinhVienID = idSV;
                        bd.LopTinChiID = idLTC;
                        bd.DiemThanhPhan = 0;
                        bd.DiemThi = 0;
                        db.BangDiems.Add(bd);
                        db.SaveChanges();
                        resultCount++;
                    }
                }
            }

            result.Add("message", "Đã thêm thành công " + resultCount + " sinh vào lớp");
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public JsonResult ActionXoaSinhVienVaoLop(int? sinhVienID, int? lopTinChiID)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            var ltc_sv = db.LopTinChi_SinhVien.FirstOrDefault(x => x.LopTinhChiID == lopTinChiID && x.SinhVienID == sinhVienID);
            var bd_sv = db.BangDiems.FirstOrDefault(x => x.LopTinChiID == lopTinChiID && x.SinhVienID == sinhVienID);
            if (sinhVienID == null || lopTinChiID == null || ltc_sv == null || bd_sv == null)
            {
                result.Add("message", "Không tìm thấy thông tin.");
                var json2 = Json(result, JsonRequestBehavior.AllowGet);
                return json2;
            }

            db.LopTinChi_SinhVien.Remove(ltc_sv);
            db.BangDiems.Remove(bd_sv);
            db.SaveChanges();
            result.Add("message", "Xóa thành công.");
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public JsonResult GetListLopTinChiTheoNganh(int nganhDaoTaoID)
        {
            var data = db.LopTinChis.Where(x => x.NganhDaoTaoID == nganhDaoTaoID).ToList();
            var result = new List<Dictionary<string, object>>();
            foreach (LopTinChi item in data)
            {
                var row = new Dictionary<string, object>();
                row.Add("LopTinChiID", item.LopTinChiID);
                row.Add("TenLopTinChi", item.TenLopTinChi);
                result.Add(row);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult GetListLopTinChiTheoMonHoc(int monHocID)
        {
            var data = db.LopTinChis.Where(x => x.MonHocID == monHocID).ToList();
            var result = new List<Dictionary<string, object>>();
            foreach (LopTinChi item in data)
            {
                var row = new Dictionary<string, object>();
                row.Add("LopTinChiID", item.LopTinChiID);
                row.Add("TenLopTinChi", item.TenLopTinChi);
                result.Add(row);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }


        // GET: LopTinChis/Create
        public ActionResult Create()
        {
            KichHoatLopTinChi ltc = new KichHoatLopTinChi();
            ViewBag.KichHoat = new SelectList(ltc.GetListLopTinChi(), "Value", "Text");
            ViewBag.GiangVienID = new SelectList(db.GiangViens, "GiangVienID", "HoTen");
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa");
            ViewBag.MonHocID = new SelectList(db.MonHocs, "MonHocID", "TenMonHoc");
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh");
            return View();
        }

        // POST: LopTinChis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LopTinChiID,MaLopTinChi,TenLopTinChi,SoLuongToiDa,NgayBatDau,NgayKetThuc,GiangVienID,MonHocID,NganhDaoTaoID,KichHoat")] LopTinChi lopTinChi)
        {
            if (ModelState.IsValid)
            {
                db.LopTinChis.Add(lopTinChi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MonHocID = new SelectList(db.MonHocs, "MonHocID", "MaMonHoc", lopTinChi.MonHocID);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", lopTinChi.NganhDaoTaoID);
            return View(lopTinChi);
        }

        // GET: LopTinChis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopTinChi lopTinChi = db.LopTinChis.Find(id);
            if (lopTinChi == null)
            {
                return HttpNotFound();
            }
            var khoaID = db.NganhDaoTaos.FirstOrDefault(x => x.NganhDaoTaoID == lopTinChi.NganhDaoTaoID).KhoaID;
            KichHoatLopTinChi ltc = new KichHoatLopTinChi();
            ViewBag.KichHoat = new SelectList(ltc.GetListLopTinChi(), "Value", "Text", lopTinChi.KichHoat);
            ViewBag.GiangVienID = new SelectList(db.GiangViens, "GiangVienID", "HoTen", lopTinChi.GiangVienID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", khoaID);
            ViewBag.MonHocID = new SelectList(db.MonHocs, "MonHocID", "TenMonHoc", lopTinChi.MonHocID);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", lopTinChi.NganhDaoTaoID);
            return View(lopTinChi);
        }

        // POST: LopTinChis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LopTinChiID,MaLopTinChi,TenLopTinChi,SoLuongToiDa,NgayBatDau,NgayKetThuc,GiangVienID,MonHocID,NganhDaoTaoID,KichHoat")] LopTinChi lopTinChi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lopTinChi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var khoaID = db.NganhDaoTaos.FirstOrDefault(x => x.NganhDaoTaoID == lopTinChi.NganhDaoTaoID).KhoaID;
            KichHoatLopTinChi ltc = new KichHoatLopTinChi();
            ViewBag.KichHoat = new SelectList(ltc.GetListLopTinChi(), "Value", "Text", lopTinChi.KichHoat);
            ViewBag.GiangVienID = new SelectList(db.GiangViens, "GiangVienID", "HoTen", lopTinChi.GiangVienID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", khoaID);
            ViewBag.MonHocID = new SelectList(db.MonHocs, "MonHocID", "TenMonHoc", lopTinChi.MonHocID);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", lopTinChi.NganhDaoTaoID);
            return View(lopTinChi);
        }


        // GET: LopTinChis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopTinChi lopTinChi = db.LopTinChis.Find(id);
            if (lopTinChi == null)
            {
                return HttpNotFound();
            }
            var khoaID = db.NganhDaoTaos.FirstOrDefault(x => x.NganhDaoTaoID == lopTinChi.NganhDaoTaoID).KhoaID;
            KichHoatLopTinChi ltc = new KichHoatLopTinChi();
            ViewBag.KichHoat = new SelectList(ltc.GetListLopTinChi(), "Value", "Text", lopTinChi.KichHoat);
            ViewBag.GiangVienID = new SelectList(db.GiangViens, "GiangVienID", "HoTen", lopTinChi.GiangVienID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", khoaID);
            ViewBag.MonHocID = new SelectList(db.MonHocs, "MonHocID", "TenMonHoc", lopTinChi.MonHocID);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", lopTinChi.NganhDaoTaoID);
            return View(lopTinChi);
        }

        // POST: LopTinChis/Delete/5

        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LopTinChi lopTinChi = db.LopTinChis.Find(id);
            db.LopTinChis.Remove(lopTinChi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
