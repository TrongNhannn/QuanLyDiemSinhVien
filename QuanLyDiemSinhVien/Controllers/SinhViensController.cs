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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace QuanLyDiemSinhVien.Controllers
{

    public class SinhViensController : Controller
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
        // GET: SinhViens
        public ActionResult Index()
        {
            var sinhViens = db.SinhViens.Include(s => s.Khoa);
            return View(sinhViens.ToList());
        }

        // GET: SinhViens/Details/5
        public ActionResult Details()
        {
            
            if (Session["UserID"] == null)
            {
                return RedirectToAction("LogOff2", "Account");
            }
            int id = int.Parse(Session["UserID"].ToString());
            SinhVien sinhVien = db.SinhViens.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            GioiTinh gt = new GioiTinh();
            TinhTrangSinhVien tt = new TinhTrangSinhVien();
            DanToc dt = new DanToc();
            ViewBag.DanToc = new SelectList(dt.GetListDanToc(), "DanTocID", "TenDanToc", sinhVien.DanToc);
            ViewBag.TinhTrang = new SelectList(tt.GetListTinhTrang(), "TinhTrangID", "TenTinhTrang", sinhVien.TinhTrang);
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text", sinhVien.GioiTinh);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", sinhVien.NganhDaoTaoID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", sinhVien.KhoaID);
            return View(sinhVien);
        }

        // GET: SinhViens/Create
        public ActionResult Create()
        {
            GioiTinh gt = new GioiTinh();
            TinhTrangSinhVien tt = new TinhTrangSinhVien();
            DanToc dt = new DanToc();
            ViewBag.DanToc = new SelectList(dt.GetListDanToc(), "DanTocID", "TenDanToc");
            ViewBag.TinhTrang = new SelectList(tt.GetListTinhTrang(), "TinhTrangID", "TenTinhTrang");
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text");
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa");
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh");
            return View();
        }

        // POST: SinhViens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SinhVienID,MaSinhVien,HoTen,NgaySinh,GioiTinh,DanToc,SoCMT,TonGiao,TinhTrang,SoDTGD,SoDTDiDong,Email,DiaChi,GhiChu,KhoaID,NganhDaoTaoID")] SinhVien sinhVien)
        {
            int checkMaSV = db.SinhViens.Count(x => x.MaSinhVien.Equals(sinhVien.MaSinhVien));
            if (checkMaSV > 0)
            {
                ModelState.AddModelError("", "Mã sinh viên đã tồn tại trong hệ thống");
                return View(sinhVien);
            }
            int checkEmailSV = db.SinhViens.Count(x => x.Email.Equals(sinhVien.Email));
            if (checkEmailSV > 0)
            {
                ModelState.AddModelError("", "Email giảng viên đã tồn tại trong hệ thống");
                return View(sinhVien);
            }
            if (ModelState.IsValid)
            {
                db.SinhViens.Add(sinhVien);
                db.SaveChanges();
                //Dữ liệu login
                ApplicationUser user = new ApplicationUser();
                user.Email = sinhVien.Email;
                user.UserName = sinhVien.MaSinhVien;
                user.FirstName = sinhVien.HoTen;
                user.SinhVienID = sinhVien.SinhVienID;
                user.IsSystemAdmin = false;
                UserManager.Create(user, "Abc@123");
                return RedirectToAction("Index");
            }
            GioiTinh gt = new GioiTinh();
            TinhTrangSinhVien tt = new TinhTrangSinhVien();
            DanToc dt = new DanToc();
            ViewBag.DanToc = new SelectList(dt.GetListDanToc(), "DanTocID", "TenDanToc", sinhVien.DanToc);
            ViewBag.TinhTrang = new SelectList(tt.GetListTinhTrang(), "TinhTrangID", "TenTinhTrang", sinhVien.TinhTrang);
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text");
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", sinhVien.NganhDaoTaoID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", sinhVien.KhoaID);
            return View(sinhVien);
        }

        // GET: SinhViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhViens.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            GioiTinh gt = new GioiTinh();
            TinhTrangSinhVien tt = new TinhTrangSinhVien();
            DanToc dt = new DanToc();
            ViewBag.DanToc = new SelectList(dt.GetListDanToc(), "DanTocID", "TenDanToc", sinhVien.DanToc);
            ViewBag.TinhTrang = new SelectList(tt.GetListTinhTrang(), "TinhTrangID", "TenTinhTrang", sinhVien.TinhTrang);
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text", sinhVien.GioiTinh);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", sinhVien.NganhDaoTaoID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", sinhVien.KhoaID);
            return View(sinhVien);
        }

        // POST: SinhViens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SinhVienID,MaSinhVien,HoTen,NgaySinh,GioiTinh,DanToc,SoCMT,TonGiao,TinhTrang,SoDTGD,SoDTDiDong,Email,DiaChi,GhiChu,KhoaID,NganhDaoTaoID")] SinhVien sinhVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sinhVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            GioiTinh gt = new GioiTinh();
            TinhTrangSinhVien tt = new TinhTrangSinhVien();
            DanToc dt = new DanToc();
            ViewBag.DanToc = new SelectList(dt.GetListDanToc(), "DanTocID", "TenDanToc", sinhVien.DanToc);
            ViewBag.TinhTrang = new SelectList(tt.GetListTinhTrang(), "TinhTrangID", "TenTinhTrang", sinhVien.TinhTrang);
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text", sinhVien.GioiTinh);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", sinhVien.NganhDaoTaoID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", sinhVien.KhoaID);
            return View(sinhVien);
        }

        // GET: SinhViens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhViens.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            GioiTinh gt = new GioiTinh();
            TinhTrangSinhVien tt = new TinhTrangSinhVien();
            DanToc dt = new DanToc();
            ViewBag.DanToc = new SelectList(dt.GetListDanToc(), "DanTocID", "TenDanToc", sinhVien.DanToc);
            ViewBag.TinhTrang = new SelectList(tt.GetListTinhTrang(), "TinhTrangID", "TenTinhTrang", sinhVien.TinhTrang);
            ViewBag.GioiTinh = new SelectList(gt.GetListGioiTinh(), "Value", "Text", sinhVien.GioiTinh);
            ViewBag.NganhDaoTaoID = new SelectList(db.NganhDaoTaos, "NganhDaoTaoID", "TenNganh", sinhVien.NganhDaoTaoID);
            ViewBag.KhoaID = new SelectList(db.Khoas, "KhoaID", "TenKhoa", sinhVien.KhoaID);
            return View(sinhVien);
        }

        // POST: SinhViens/Delete/5

        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SinhVien sinhVien = db.SinhViens.Find(id);
            db.SinhViens.Remove(sinhVien);
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
