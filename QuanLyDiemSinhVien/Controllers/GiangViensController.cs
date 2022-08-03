using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyDiemSinhVien.Models;
using QuanLyDiemSinhVien.Constant;
using QuanLyDiemSinhVien.Securities;
using QuanLyDiemSinhVien.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace QuanLyDiemSinhVien.Controllers
{

    public class GiangViensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                if (_signInManager == null)
                {
                    _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                }
                return _signInManager;
            }
            private set { _signInManager = value; }
        }

        ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return _userManager;
            }
        }
        // GET: GiangViens
        public ActionResult Index()
        {
            var giangViens = db.GiangViens.Include(g => g.Khoa);
            return View(giangViens.ToList());
        }

        public ActionResult GetLopOfGiangVien()
        {
            int id = int.Parse(Session["UserID"].ToString());

            List<LopTinChiMonHocViewModels> model = db.MonHocs
                       .Join(db.LopTinChis, l => l.MonHocID, l2 => l2.MonHocID, (l, l2) => new LopTinChiMonHocViewModels
                       {
                           GiangVienID = l2.GiangVienID
                           , LopTinChiID = l2.LopTinChiID
                           , MaLopTinChi = l2.MaLopTinChi
                           , TenLopTinChi = l2.TenLopTinChi
                           , TenMonHoc = l.TenMonHoc
                           , SoTinChi = l.SoTinChi
                           , NgayBatDau = l2.NgayBatDau
                           ,NgayKetThuc = l2.NgayKetThuc
                       }).Where(x => x.GiangVienID == id)
                       .OrderByDescending(x=>x.NgayBatDau)
                       .ToList();

            return View(model);
        }

        public JsonResult GetListGiangVienTheoKhoa(int khoaID)
        {
            var data = db.GiangViens.Where(x => x.KhoaID == khoaID).ToList();
            var result = new List<Dictionary<string, object>>();
            foreach (GiangVien item in data)
            {
                var row = new Dictionary<string, object>();
                row.Add("GiangVienID", item.GiangVienID);
                row.Add("HoTen", item.HoTen);
                result.Add(row);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        // GET: GiangViens/Details/5
        public ActionResult Details()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("LogOff2", "Account");
            }
            int id = int.Parse(Session["UserID"].ToString());
            GiangVien giangVien = db.GiangViens.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            GioiTinh gt = new GioiTinh();
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text", giangVien.GioiTinh);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", giangVien.KhoaID);
            return View(giangVien);
        }

        // GET: GiangViens/Create
        public ActionResult Create()
        {
            GioiTinh gt = new GioiTinh();
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text");
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa");
            return View();
        }

        // POST: GiangViens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GiangVienID,MaGiangVien,HoTen,NgaySinh,GioiTinh,DiaChi,SoDienThoai,SoCMT,QueQuan,KhoaID,NgayTao,Email")] GiangVien giangVien)
        {
            int checkMaGV = db.GiangViens.Count(x => x.MaGiangVien.Equals(giangVien.MaGiangVien));
            if(checkMaGV > 0)
            {
                ModelState.AddModelError("", "Mã giảng viên đã tồn tại trong hệ thống");
                return View(giangVien);
            }
            int checkEmailGV = db.GiangViens.Count(x => x.Email.Equals(giangVien.Email));
            if (checkEmailGV > 0)
            {
                ModelState.AddModelError("", "Email giảng viên đã tồn tại trong hệ thống");
                return View(giangVien);
            }
            if (ModelState.IsValid)
            {
                giangVien.NgayTao = DateTime.Now;
                db.GiangViens.Add(giangVien);
                db.SaveChanges();

                //Dữ liệu login
                ApplicationUser user = new ApplicationUser();
                user.Email = giangVien.Email;
                user.UserName = giangVien.MaGiangVien;
                user.FirstName = giangVien.HoTen;
                user.GiangVienID = giangVien.GiangVienID;
                user.IsSystemAdmin = false;

                UserManager.Create(user, "Abc@123");

                return RedirectToAction("Index");
            }
            GioiTinh gt = new GioiTinh();
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text", giangVien.GioiTinh);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", giangVien.KhoaID);
            return View(giangVien);
        }

        // GET: GiangViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangViens.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            GioiTinh gt = new GioiTinh();
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text", giangVien.GioiTinh);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", giangVien.KhoaID);
            return View(giangVien);
        }

        // POST: GiangViens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GiangVienID,MaGiangVien,HoTen,NgaySinh,GioiTinh,DiaChi,SoDienThoai,SoCMT,QueQuan,KhoaID,NgayTao,Email")] GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(giangVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            GioiTinh gt = new GioiTinh();
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text", giangVien.GioiTinh);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", giangVien.KhoaID);
            return View(giangVien);
        }

        // GET: GiangViens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangViens.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            GioiTinh gt = new GioiTinh();
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text", giangVien.GioiTinh);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", giangVien.KhoaID);
            return View(giangVien);
        }

        // POST: GiangViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GiangVien giangVien = db.GiangViens.Find(id);
            db.GiangViens.Remove(giangVien);
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
