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

    public class NganhDaoTaosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NganhDaoTaos
        public ActionResult Index()
        {
            var nganhDaoTaos = db.NganhDaoTaos.Include(n => n.Khoa);
            return View(nganhDaoTaos.ToList());
        }

        public ActionResult ThemMonHocVaoNganh(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhDaoTao nganhDaoTao = db.NganhDaoTaos.Find(id);
            if (nganhDaoTao == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", nganhDaoTao.KhoaID);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", id);
            var data = db.MonHocs.ToList();
            ViewData["dataMonHoc"] = data;
            return View(nganhDaoTao);
        }

        public JsonResult ActionXoaMonHocTrongNganh(int? monHocID, int? nganhDaoTaoID)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            var ndt_mh = db.NganhDaoTao_MonHoc.FirstOrDefault(x => x.MonHocID == monHocID && x.NganhDaoTaoID == nganhDaoTaoID);

            if (monHocID == null || nganhDaoTaoID == null || ndt_mh == null)
            {
                result.Add("message", "Không tìm thấy thông tin.");
                var json2 = Json(result, JsonRequestBehavior.AllowGet);
                return json2;
            }

            db.NganhDaoTao_MonHoc.Remove(ndt_mh);
            db.SaveChanges();
            result.Add("message", "Xóa thành công.");
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public JsonResult ActionThemLopVaoNganh(string monHocID, string nganhDaoTaoID)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (monHocID == null || nganhDaoTaoID == null)
            {
                result.Add("message", "Không tìm thấy thông tin.");
                var json2 = Json(result, JsonRequestBehavior.AllowGet);
                return json2;
            }
            var dataMH = monHocID.Split(',');
            int resultCount = 0;
            foreach (var item in dataMH)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    int idMH = int.Parse(item);
                    int idNDT = int.Parse(nganhDaoTaoID);
                    int checkCount = db.NganhDaoTao_MonHoc.Where(x => x.NganhDaoTaoID == idNDT && x.MonHocID == idMH).Count();
                    if (checkCount == 0)
                    {
                        //Thêm sv vào lớp
                        NganhDaoTao_MonHoc ndt_mh = new NganhDaoTao_MonHoc();
                        ndt_mh.NganhDaoTaoID = idNDT;
                        ndt_mh.MonHocID = idMH;
                        db.NganhDaoTao_MonHoc.Add(ndt_mh);
                        db.SaveChanges();
                        resultCount++;
                    }
                }
            }

            result.Add("message", "Đã thêm thành công " + resultCount + " môn học vào ngành");
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public JsonResult GetListNganhDaoTaoTheoKhoa(int khoaID)
        {
            var data = db.NganhDaoTaos.Where(x => x.KhoaID == khoaID).ToList();
            var result = new List<Dictionary<string, object>>();
            foreach (NganhDaoTao item in data)
            {
                var row = new Dictionary<string, object>();
                row.Add("NganhDaoTaoID", item.NganhDaoTaoID);
                row.Add("TenNganh", item.TenNganh);
                result.Add(row);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        // GET: NganhDaoTaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhDaoTao nganhDaoTao = db.NganhDaoTaos.Find(id);
            if (nganhDaoTao == null)
            {
                return HttpNotFound();
            }
            var danhsachMonHoc = db.NganhDaoTao_MonHoc
                                    .Join(db.MonHocs, ndt => ndt.MonHocID, mh => mh.MonHocID, (ndt, mh) => new { ndt, mh })
                                    .Join(db.NganhDaoTaos, o1 => o1.ndt.NganhDaoTaoID, o2 => o2.NganhDaoTaoID, (o1, o2) => new NganhDaoTaoMonHocViewModels
                                    {
                                        NganhDaoTao = o2
                                        ,
                                        MonHoc = o1.mh
                                    })
                                    .Where(x => x.NganhDaoTao.NganhDaoTaoID == id)
                                    .ToList();
            //.Where(x => x.ndt.NganhDaoTaoID == id).ToList();
            //.Select(x => new MonHoc
            //{
            //    MonHocID = x.mh.MonHocID
            //    ,
            //    MaMonHoc = x.mh.MaMonHoc
            //    ,
            //    TenMonHoc = x.mh.TenMonHoc
            //    ,
            //    SoTinChi = x.mh.SoTinChi
            //    ,
            //    LoaiMonHoc = x.mh.LoaiMonHoc
            //}).ToList();
            ViewData["DanhSachMonHoc"] = danhsachMonHoc;
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa",nganhDaoTao.KhoaID);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", id);
            return View(nganhDaoTao);
        }

        // GET: NganhDaoTaos/Create
        public ActionResult Create()
        {
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa");
            return View();
        }

        // POST: NganhDaoTaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NganhDaoTaoID,TenNganh,KhoaID")] NganhDaoTao nganhDaoTao)
        {
            if (ModelState.IsValid)
            {
                db.NganhDaoTaos.Add(nganhDaoTao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", nganhDaoTao.KhoaID);
            return View(nganhDaoTao);
        }

        // GET: NganhDaoTaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhDaoTao nganhDaoTao = db.NganhDaoTaos.Find(id);
            if (nganhDaoTao == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", nganhDaoTao.KhoaID);
            return View(nganhDaoTao);
        }

        // POST: NganhDaoTaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NganhDaoTaoID,TenNganh,KhoaID")] NganhDaoTao nganhDaoTao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nganhDaoTao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", nganhDaoTao.KhoaID);
            return View(nganhDaoTao);
        }

        // GET: NganhDaoTaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhDaoTao nganhDaoTao = db.NganhDaoTaos.Find(id);
            if (nganhDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(nganhDaoTao);
        }

        // POST: NganhDaoTaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NganhDaoTao nganhDaoTao = db.NganhDaoTaos.Find(id);
            db.NganhDaoTaos.Remove(nganhDaoTao);
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
