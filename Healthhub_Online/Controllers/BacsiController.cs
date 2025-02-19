    using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Healthhub_Online.Models;
using PagedList;

namespace Healthhub_Online.Controllers
{
    public class BacsiController : Controller
    {
        private ModelWeb db = new ModelWeb();

        // GET: Bacsi
        public ActionResult Index()
        {
            var quanTris = db.QuanTris.Include(q => q.Khoa).Where(x => x.VaiTro == 2);
            return View(quanTris.ToList());
        }

        // GET: Bacsi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feedback = db.DanhGias.AsQueryable();
            QuanTri quanTri = db.QuanTris.Include(d => d.Khoa).SingleOrDefault(d => d.IDQuanTri == id);
            feedback = db.DanhGias.Include(f => f.DanhGiaChatLuong).Where(f => f.IDQuanTri == id);
            ViewBag.Feedback = feedback.ToList();
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }
        // GET: Bacsi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa", quanTri.IDKhoa);
            return View(quanTri);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDQuanTri,TaiKhoan,MatKhau,VaiTro,ThongTinBacSi,TrinhDo,IDKhoa,HoTen,AnhBia")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quanTri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa", quanTri.IDKhoa);
            return View(quanTri);
        }

        // GET: Admin/QuanTris/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        public ActionResult KiemTraDanhGia(int? page)
        {
            var danhgia = db.DanhGias.OrderBy(x => x.IDDanhGia).ThenBy(y => y.IDDanhGiaChatLuong).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(danhgia.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Lichdatuvan(int? page)
        {
            var lich = db.LichKhams.OrderByDescending(x => x.BatDau).ThenBy(y => y.IDLichKham).Where(x => x.TrangThai == 2).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(lich.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Lichdangcho(int? page)
        {
            var lich = db.LichKhams.OrderByDescending(x => x.BatDau).ThenBy(y => y.IDLichKham).Where(x => x.TrangThai == 0).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(lich.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Lichdaxacnhan(int? page)
        {
            var lich = db.LichKhams.OrderByDescending(x => x.BatDau).ThenBy(y => y.IDLichKham).Where(x => x.TrangThai == 1).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(lich.ToPagedList(pageNumber, pageSize));
        }

        // GET: Lichkham/Edit/5
        public ActionResult Xacnhanlichhen(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichKham lichKham = db.LichKhams.Find(id);
            if (lichKham == null)
            {
                return HttpNotFound();
            }
            // NguoiDung n = new NguoiDung();
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs.Where(x => x.IDNguoiDung == lichKham.IDNguoiDung), "IDNguoiDung", "HoTen", lichKham.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris.Where(x => x.IDQuanTri == lichKham.IDQuanTri), "IDQuanTri", "HoTen", lichKham.IDQuanTri);
            return View(lichKham);
        }

        // POST: Lichkham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Xacnhanlichhen([Bind(Include = "IDLichKham,ChuDe,MoTa,BatDau,KetThuc,TrangThai,ZoomInfo,KetQuaKham,IDNguoiDung,IDQuanTri")] LichKham lichKham)
        {
            // Check if the start time is before the current date
            if (lichKham.BatDau < DateTime.Now)
            {
                ModelState.AddModelError("BatDau", "Thời gian bắt đầu phải lớn hơn ngày hiện tại.");
            }

            // Check if the end time is before the start time
            if (lichKham.KetThuc < lichKham.BatDau)
            {
                ModelState.AddModelError("KetThuc", "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");
            }

            if (ModelState.IsValid)
            {
                db.Entry(lichKham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Lichdaxacnhan", "Bacsi");
            }

            // Check if ZoomInfo is empty
            if (string.IsNullOrWhiteSpace(lichKham.ZoomInfo))
            {
                ModelState.AddModelError("ZoomInfo", "Vui lòng nhập link Zoom.");
            }

            if (ModelState.IsValid)
            {
                db.Entry(lichKham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Lichdaxacnhan", "Bacsi");
            }

            // If ModelState is not valid, reload the view with errors and data
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", lichKham.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", lichKham.IDQuanTri);

            // Return the view with model and errors
            return View(lichKham);
        }


        // GET: Bacsi/Guibenhan
        public ActionResult Guibenhan(int id)
        {
            LichKham lichKham = db.LichKhams.Find(id);
            if (lichKham == null)
            {
                return HttpNotFound();
            }
            return View(lichKham);
        }

        // POST: Bacsi/Guibenhan
        [HttpPost]
        public ActionResult Guibenhan(int id, HttpPostedFileBase file)
        {
            LichKham lichKham = db.LichKhams.Find(id);
            if (lichKham == null)
            {
                return HttpNotFound();
            }

            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    // Lưu file vào thư mục trên server
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    file.SaveAs(path);

                    // Lưu tên file vào trường KetQuaKham của LichKham
                    lichKham.KetQuaKham = fileName;
                    db.SaveChanges();

                    ViewBag.Message = "File đã được lưu thành công.";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Lỗi: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Vui lòng chọn file.";
            }

            return RedirectToAction("Lichdatuvan", "Bacsi");
        }

        public ActionResult ListAll(int? page)
        {
            if (page == null) page = 1;
            var hoiDaps = db.HoiDaps.Include(h => h.NguoiDung).Include(h => h.QuanTri).Where(n => n.TrangThai == 1)
                .OrderByDescending(x => x.NgayGui).ThenBy(x => x.IDHoidap).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(hoiDaps.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Cauhoichoxuly(int? id, int? page)
        {
            var hoiDaps = db.HoiDaps.Include(h => h.NguoiDung).Include(h => h.QuanTri).Where(h => h.IDNguoiDung == id && h.TrangThai == 0)
                .OrderByDescending(x => x.NgayGui).ThenBy(x => x.IDHoidap).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.id = id;
            return View(hoiDaps.ToPagedList(pageNumber, pageSize));
        }


        // GET: Hoidap/Details/5
        public ActionResult DetailsHoiDap(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoiDap hoiDap = db.HoiDaps.Find(id);
            if (hoiDap == null)
            {
                return HttpNotFound();
            }
            return View(hoiDap);
        }

        // GET: Hoidap/Create
        public ActionResult CreateHoiDap(int? id)
        {
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs.Where(h => h.IDNguoiDung == id), "IDNguoiDung", "HoTen");
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan");
            return View();
        }

        // POST: Hoidap/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDHoidap,CauHoi,TraLoi,IDNguoiDung,IDQuanTri,NgayGui,GhiChu,TrangThai")] HoiDap hoiDap)
        {
            if (ModelState.IsValid)
            {
                var d = DateTime.Now;
                hoiDap.NgayGui = d;
                hoiDap.TrangThai = 0;
                db.HoiDaps.Add(hoiDap);
                db.SaveChanges();
                return RedirectToAction("Index", "Hoidap", new { id = hoiDap.IDNguoiDung });
            }

            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", hoiDap.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", hoiDap.IDQuanTri);
            return View(hoiDap);
        }

        public ActionResult Traloicauhoi(int? id)
        {
            if (id == null)

            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoiDap hoiDap = db.HoiDaps.Find(id);
            if (hoiDap == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", hoiDap.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", hoiDap.IDQuanTri);
            return View(hoiDap);
        }

        // POST: Hoidap/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Traloicauhoi([Bind(Include = "IDHoidap,CauHoi,TraLoi,IDNguoiDung,IDQuanTri,NgayGui,GhiChu,TrangThai")] HoiDap hoiDap)
        {
            if (ModelState.IsValid)
            {
                hoiDap.TrangThai = 1;
                db.Entry(hoiDap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Quanlyhoidap");
            }
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", hoiDap.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", hoiDap.IDQuanTri);
            return View(hoiDap);
        }

        // GET: Hoidap/Edit/5
        public ActionResult EditHoiDap(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoiDap hoiDap = db.HoiDaps.Find(id);
            if (hoiDap == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs.Where(x => x.IDNguoiDung == hoiDap.IDNguoiDung), "IDNguoiDung", "HoTen", hoiDap.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris.Where(x => x.IDQuanTri == hoiDap.IDQuanTri), "IDQuanTri", "HoTen", hoiDap.IDQuanTri);
            return View(hoiDap);
        }

        // POST: Hoidap/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDHoidap,CauHoi,TraLoi,IDNguoiDung,IDQuanTri,NgayGui,GhiChu,TrangThai")] HoiDap hoiDap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoiDap).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", hoiDap.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", hoiDap.IDQuanTri);
            return View(hoiDap);
        }

        // GET: Hoidap/Delete/5
        public ActionResult DeleteHoiDap(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoiDap hoiDap = db.HoiDaps.Find(id);
            if (hoiDap == null)
            {
                return HttpNotFound();
            }
            return View(hoiDap);
        }

        // POST: Hoidap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoiDap hoiDap = db.HoiDaps.Find(id);
            db.HoiDaps.Remove(hoiDap);
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
