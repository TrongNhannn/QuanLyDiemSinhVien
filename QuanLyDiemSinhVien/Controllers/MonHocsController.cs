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

namespace QuanLyDiemSinhVien.Controllers
{

    public class MonHocsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: MonHocs
        public ActionResult Index()
        {
            return View(db.MonHocs.ToList());
        }

        // GET: MonHocs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHocs.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return View(monHoc);
        }

        public JsonResult GetListMonHocTheoNganh(int nganhDaoTaoID)
        {
            var data = db.NganhDaoTao_MonHoc
                .Join(db.MonHocs, n => n.MonHocID, m => m.MonHocID, (n, m) => new { m.MonHocID, m.TenMonHoc, n.NganhDaoTaoID })
                .Where(x => x.NganhDaoTaoID == nganhDaoTaoID).ToList();

            var json = Json(data, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        // GET: MonHocs/Create
        public ActionResult Create()
        {
            LoaiMonHoc lmh = new LoaiMonHoc();
            ViewBag.LoaiMonHoc = new SelectList(lmh.GetListLoaiMonHoc(), "LoaiMonHocID", "TenLoaiMonHoc");
            return View();
        }

        // POST: MonHocs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MonHocID,MaMonHoc,TenMonHoc,SoTinChi,LoaiMonHoc")] MonHoc monHoc)
        {
            MonHoc mh = db.MonHocs.FirstOrDefault(x => x.MaMonHoc == monHoc.MaMonHoc);
            LoaiMonHoc lmh = new LoaiMonHoc();
            ViewBag.LoaiMonHoc = new SelectList(lmh.GetListLoaiMonHoc(), "LoaiMonHocID", "TenLoaiMonHoc");
            if (mh != null)
            {
                ModelState.AddModelError("", "Mã môn học đã tồn tại trong hệ thống");
                return View(monHoc);
            }
            if (ModelState.IsValid)
            {
                db.MonHocs.Add(monHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(monHoc);
        }

        // GET: MonHocs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHocs.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            LoaiMonHoc lmh = new LoaiMonHoc();
            ViewBag.LoaiMonHoc = new SelectList(lmh.GetListLoaiMonHoc(), "LoaiMonHocID", "TenLoaiMonHoc",monHoc.LoaiMonHoc);
            return View(monHoc);
        }

        // POST: MonHocs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MonHocID,MaMonHoc,TenMonHoc,SoTinChi,LoaiMonHoc")] MonHoc monHoc)
        {
            LoaiMonHoc lmh = new LoaiMonHoc();
            ViewBag.LoaiMonHoc = new SelectList(lmh.GetListLoaiMonHoc(), "LoaiMonHocID", "TenLoaiMonHoc");
            MonHoc mh = db.MonHocs.FirstOrDefault(x => x.MaMonHoc == monHoc.MaMonHoc);
            if (mh != null)
            {
                ModelState.AddModelError("", "Mã môn học đã tồn tại trong hệ thống");
                return View(monHoc);
            }
            if (ModelState.IsValid)
            {
                db.Entry(monHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(monHoc);
        }

        // GET: MonHocs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHocs.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            LoaiMonHoc lmh = new LoaiMonHoc();
            ViewBag.LoaiMonHoc = new SelectList(lmh.GetListLoaiMonHoc(), "LoaiMonHocID", "TenLoaiMonHoc", monHoc.LoaiMonHoc);
            return View(monHoc);
        }

        // POST: MonHocs/Delete/5

        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonHoc monHoc = db.MonHocs.Find(id);
            db.MonHocs.Remove(monHoc);
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
