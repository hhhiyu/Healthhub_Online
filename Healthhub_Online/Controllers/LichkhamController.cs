using System;
using System.Collections.Generic;
using System.Data;
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
    public class LichkhamController : Controller
    {
        public ModelWeb db = new ModelWeb();
        // GET: Lichkham
        public ActionResult Index(int? id, int? page)
        {
            var lichKhams = db.LichKhams.Include(l => l.NguoiDung).Include(l => l.QuanTri).
                Where(h => h.IDNguoiDung == id).OrderByDescending(x => x.BatDau).ThenBy(x => x.IDLichKham);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.id = id;
            return View(lichKhams.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Dangxuly(int? id, int? page)
        {
            var lichKhams = db.LichKhams.Include(l => l.NguoiDung).Include(l => l.QuanTri).
                Where(h => h.IDNguoiDung == id && h.TrangThai == 0).OrderByDescending(x => x.BatDau).ThenBy(x => x.IDLichKham);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.id = id;
            return View(lichKhams.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Daxacnhan(int? id, int? page)
        {
            var lichKhams = db.LichKhams.Include(l => l.NguoiDung).Include(l => l.QuanTri).
                Where(h => h.IDNguoiDung == id && h.TrangThai == 1).OrderByDescending(x => x.BatDau).ThenBy(x => x.IDLichKham);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.id = id;
            return View(lichKhams.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Datuvanxong(int? id, int? page)
        {
            var lichKhams = db.LichKhams.Include(l => l.NguoiDung).Include(l => l.QuanTri).
                Where(h => h.IDNguoiDung == id && h.TrangThai == 2).OrderByDescending(x => x.BatDau).ThenBy(x => x.IDLichKham);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.id = id;
            return View(lichKhams.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DanhGia(int id)
        {
            // Check for success message
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }

            var lichKham = db.LichKhams.Find(id);
            if (lichKham == null)
            {
                return HttpNotFound();
            }

            // Create a new DanhGia object for the view
            var danhGia = new DanhGia
            {
                IDNguoiDung = lichKham.IDNguoiDung,
                IDQuanTri = lichKham.IDQuanTri,
                LichKham = lichKham
            };

            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", danhGia.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "HoTen", danhGia.IDQuanTri);
            ViewBag.IDDanhGiaChatLuong = new SelectList(db.DanhGiaChatLuongs, "IDDanhGiaChatLuong", "DanhGiaChatLuong1", danhGia.IDDanhGiaChatLuong);

            return View(danhGia);
        }


        // POST: Lichkham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DanhGia(DanhGia danhGia)
        {
            if (ModelState.IsValid)
            {
                // Save danhGia to the database
                db.DanhGias.Add(danhGia);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đánh giá của bạn đã được gửi thành công.";

                // Redirect to Datuvanxong action with the user's ID
                return RedirectToAction("Datuvanxong", new { id = danhGia.IDNguoiDung });
            }

            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", danhGia.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "HoTen", danhGia.IDQuanTri);
            ViewBag.IDDanhGiaChatLuong = new SelectList(db.DanhGiaChatLuongs, "IDDanhGiaChatLuong", "DanhGiaChatLuong1", danhGia.IDDanhGiaChatLuong);

            // If ModelState is not valid, return back to the form with validation errors
            return View(danhGia);
        }


        // GET: LichKham/Benhan
        public ActionResult Benhan(int id)
        {
            LichKham lichKham = db.LichKhams.Find(id);
            if (lichKham == null || string.IsNullOrEmpty(lichKham.KetQuaKham))
            {
                ViewBag.Message = "File bệnh án chưa được gửi.";
                return View();
            }

            // Truyền tên file bệnh án vào view
            string path = Path.Combine(Server.MapPath("~/UploadedFiles"), lichKham.KetQuaKham);
            ViewBag.FilePath = path;
            ViewBag.IDLichKham = id; // Truyền ID của lịch khám

            return View();
        }

        // GET: LichKham/DownloadFile
        public ActionResult DownloadFile(int id)
        {
            LichKham lichKham = db.LichKhams.Find(id);
            if (lichKham == null || string.IsNullOrEmpty(lichKham.KetQuaKham))
            {
                ViewBag.Message = "File bệnh án chưa được gửi.";
                return View("Benhan");
            }

            // Tạo đường dẫn đến file
            string path = Path.Combine(Server.MapPath("~/UploadedFiles"), lichKham.KetQuaKham);

            // Kiểm tra file tồn tại trên server
            if (!System.IO.File.Exists(path))
            {
                ViewBag.Message = "File không tồn tại trên server.";
                return View("Benhan");
            }

            // Tải file về
            return File(path, "application/force-download", Path.GetFileName(path));
        }






        // GET: Lichkham/Details/5
        public ActionResult Details(int? id)
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
            return View(lichKham);
        }
        // GET: Lichkham/Create
        public ActionResult Create()
        {
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen");
            ViewBag.IDQuanTri = new SelectList(db.QuanTris.Where(n => n.VaiTro == 2), "IDQuanTri", "HoTen");
            return View();
        }
        // POST: Lichkham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDLichKham,ChuDe,MoTa,BatDau,KetThuc,TrangThai,ZoomInfo,KetQuaKham,IDNguoiDung,IDQuanTri")] LichKham lichKham)
        {
            // Kiểm tra nếu chủ đề trống
            if (string.IsNullOrWhiteSpace(lichKham.ChuDe))
            {
                ModelState.AddModelError("ChuDe", "Chủ đề không được để trống.");
            }

            // Kiểm tra nếu chủ đề chứa ký tự đặc biệt hoặc số
            if (!string.IsNullOrEmpty(lichKham.ChuDe) && (lichKham.ChuDe.Any(char.IsSymbol) || lichKham.ChuDe.Any(char.IsDigit)))
            {
                ModelState.AddModelError("ChuDe", "Chủ đề không được chứa ký tự đặc biệt hoặc số.");
            }

            // Kiểm tra nếu ngày bắt đầu nhỏ hơn ngày hiện tại
            if (lichKham.BatDau < DateTime.Now)
            {
                ModelState.AddModelError("BatDau", "Ngày bắt đầu phải lớn hơn ngày hiện tại.");
            }

            // Kiểm tra nếu ngày bắt đầu trùng với ngày bắt đầu của lịch đã tạo
            var existingLichKham = db.LichKhams.FirstOrDefault(l => l.BatDau == lichKham.BatDau);
            if (existingLichKham != null)
            {
                ModelState.AddModelError("BatDau", "Ngày này đã được tạo rồi.");
            }

            if (ModelState.IsValid)
            {
                lichKham.TrangThai = 0;
                db.LichKhams.Add(lichKham);
                db.SaveChanges();
                return RedirectToAction("Index", "LichKham", new { id = lichKham.IDNguoiDung });
            }

            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", lichKham.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "HoTen", lichKham.IDQuanTri);
            return View(lichKham);
        }


        // GET: Lichkham/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", lichKham.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", lichKham.IDQuanTri);
            return View(lichKham);
        }

        // POST: Lichkham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDLichKham,ChuDe,MoTa,BatDau,KetThuc,TrangThai,ZoomInfo,KetQuaKham,IDNguoiDung,IDQuanTri")] LichKham lichKham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lichKham).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", lichKham.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", lichKham.IDQuanTri);
            return View(lichKham);
        }



        // GET: Lichkham/Delete/5
        public ActionResult Delete(int? id)
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
            return View(lichKham);
        }

        // POST: Lichkham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LichKham lichKham = db.LichKhams.Find(id);
            db.LichKhams.Remove(lichKham);
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