using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyDiemSinhVien.Models;
using QuanLyDiemSinhVien.ViewModels;

namespace QuanLyDiemSinhVien.Controllers
{

    public class BangDiemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BangDiems
        public ActionResult Index()
        {
            ViewBag.LopTinChiID = new SelectList(db.LopTinChis, "LopTinChiID", "TenLopTinChi");
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa");
            ViewBag.MonHocID = new SelectList(db.MonHocs, "MonHocID", "TenMonHoc");
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh");
            return View();
        }

        public ActionResult GiangVienCapNhatDiem(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
                            DiemTrungBinh = (o1.DiemThanhPhan + o1.DiemThi) /2//so1.DiemTrungBinh
                                                   ,
                            DiemChu = o1.DiemChu
                        })
                   .Where(X => X.LopTinChiID == id).ToList();
            return View(data);
        }

        public ActionResult BangDiemSinhVien()
        {
            int id = int.Parse(Session["UserID"].ToString());
            List<BangDiemMonHocViewModels> model = db.BangDiems
                       .Join(db.LopTinChis, l => l.LopTinChiID, l2 => l2.LopTinChiID, (l, l2) => new { l, l2 })
                       .Join(db.MonHocs, m => m.l2.MonHocID, m2 => m2.MonHocID, (m, m2) => new BangDiemMonHocViewModels()
                       {
                           SinhVienID = m.l.SinhVienID
                              ,
                           TenMonHoc = m2.TenMonHoc
                              ,
                           SoTinChi = m2.SoTinChi
                              ,
                           DiemThanhPhan = m.l.DiemThanhPhan
                              ,
                           DiemThi = m.l.DiemThi
                              ,
                           DiemTrungBinh = (m.l.DiemThanhPhan  + m.l.DiemThi ) /2
                              ,
                           DiemChu = m.l.DiemChu
                       })
                       .Where(x => x.SinhVienID == id)
                       .ToList();

            return View(model);
        }

        public JsonResult LayDanhSachBangDiemTheoLopTinChi(int loptinchiID)
        {
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
                            DiemTrungBinh = (o1.DiemThanhPhan  + o1.DiemThi)/2 //so1.DiemTrungBinh
                                                   ,
                            DiemChu = o1.DiemChu
                        })
                   .Where(X => X.LopTinChiID == loptinchiID).ToList();

            var json = Json(data, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult CapNhatBangDiemLopHocSinhVien(int bangdiemID, double diemTP, double diemThi)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            BangDiem bangdiem = db.BangDiems.Find(bangdiemID);
            if (bangdiem == null)
            {
                result.Add("message", "Không tìm thấy dữ liệu");
                var json = Json(result, JsonRequestBehavior.AllowGet);
                return json;
            }
            bangdiem.DiemThanhPhan = diemTP;
            bangdiem.DiemThi = diemThi;
            db.Entry(bangdiem).State = EntityState.Modified;
            db.SaveChanges();

            result.Add("message", "Cập nhật thành công");
            var json2 = Json(result, JsonRequestBehavior.AllowGet);
            return json2;
        }


        // GET: BangDiems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangDiem bangDiem = db.BangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            return View(bangDiem);
        }

        // GET: BangDiems/Create
        public ActionResult Create()
        {
            ViewBag.LopTinChiID = new SelectList(db.LopTinChis, "LopTinChiID", "MaLopTinChi");
            return View();
        }

        // POST: BangDiems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BangDiemID,SinhVienID,LopTinChiID,DiemThanhPhan,DiemThi,DiemTrungBinh,DiemChu")] BangDiem bangDiem)
        {
            if (ModelState.IsValid)
            {
                db.BangDiems.Add(bangDiem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LopTinChiID = new SelectList(db.LopTinChis, "LopTinChiID", "MaLopTinChi", bangDiem.LopTinChiID);
            return View(bangDiem);
        }

        // GET: BangDiems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangDiem bangDiem = db.BangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            ViewBag.LopTinChiID = new SelectList(db.LopTinChis, "LopTinChiID", "MaLopTinChi", bangDiem.LopTinChiID);
            return View(bangDiem);
        }

        // POST: BangDiems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BangDiemID,SinhVienID,LopTinChiID,DiemThanhPhan,DiemThi,DiemTrungBinh,DiemChu")] BangDiem bangDiem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bangDiem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LopTinChiID = new SelectList(db.LopTinChis, "LopTinChiID", "MaLopTinChi", bangDiem.LopTinChiID);
            return View(bangDiem);
        }

        // GET: BangDiems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangDiem bangDiem = db.BangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            return View(bangDiem);
        }

        // POST: BangDiems/Delete/5

        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BangDiem bangDiem = db.BangDiems.Find(id);
            db.BangDiems.Remove(bangDiem);
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
