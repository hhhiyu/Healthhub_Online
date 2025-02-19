using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Healthhub_Online.Models;
using static System.Net.WebRequestMethods;

namespace Healthhub_Online.Controllers
{
 

    public class HomeController : Controller
    {
        private ModelWeb db = new ModelWeb();
        public ActionResult Index()
        {
            var solieu = db.Solieudichbenhs.ToList();
            return View(solieu);
        }

        public ActionResult Trangchu()
        {
            

            return View();
        }

        public ActionResult Dangky()
        {
            // Truy vấn danh sách giới tính và danh sách tỉnh từ cơ sở dữ liệu
            var gioiTinhs = db.GioiTinhs.ToList();
            var tinhThanhs = db.TinhThanhs.ToList();

            // Truyền danh sách giới tính và danh sách tỉnh đến view
            ViewBag.GioiTinhs = new SelectList(gioiTinhs, "IDGioiTinh", "GioiTinh1");
            ViewBag.TinhThanhs = new SelectList(tinhThanhs, "IDTinh", "TenTinh");

            return View();         
        }
        public ActionResult VerifyOTP()
        {
            // Display a view to verify OTP
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dangky([Bind(Include = "IDNguoiDung,HoTen,Email,DienThoai,TaiKhoan,MatKhau,IDGioiTinh,IDTinh")] NguoiDung nguoiDung, string ConfirmMatKhau)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem mật khẩu và mật khẩu xác nhận có khớp nhau không
                if (nguoiDung.MatKhau != ConfirmMatKhau)
                {
                    ModelState.AddModelError("ConfirmMatKhau", "Mật khẩu xác nhận không khớp.");

                    var gioiTinhs = db.GioiTinhs.ToList();
                    var tinhThanhs = db.TinhThanhs.ToList();
                    ViewBag.GioiTinhs = new SelectList(gioiTinhs, "IDGioiTinh", "GioiTinh1");
                    ViewBag.TinhThanhs = new SelectList(tinhThanhs, "IDTinh", "TenTinh");
                    return View(nguoiDung);
                }

                if (db.NguoiDungs.Any(u => u.TaiKhoan == nguoiDung.TaiKhoan.ToLower().Trim()))
                {
                    ModelState.AddModelError("TaiKhoan", "Tên đăng nhập đã tồn tại.");
                    var gioiTinhs = db.GioiTinhs.ToList();
                    var tinhThanhs = db.TinhThanhs.ToList();

                    // Truyền danh sách giới tính và danh sách tỉnh đến view
                    ViewBag.GioiTinhs = new SelectList(gioiTinhs, "IDGioiTinh", "GioiTinh1");
                    ViewBag.TinhThanhs = new SelectList(tinhThanhs, "IDTinh", "TenTinh");
                    return View(nguoiDung);
                }
                Random random = new Random();
                int otp = random.Next(100000, 999999);
                if (SendOTP(nguoiDung.Email, otp))
                {
                    TempData["OTP"] = otp;
                    TempData["NguoiDung"] = nguoiDung;
                    return RedirectToAction("VerifyOTP", "Home");
                }



            }

            // Nếu model không hợp lệ, trả về view với model
            return View(nguoiDung);
        }

        [HttpPost]
        public ActionResult VerifyOTP(int otp)
        {
            try
            {
                int storedOTP = Convert.ToInt32(TempData["OTP"]);
                if (otp == storedOTP)
                {
                    ViewBag.Message = "OTP xác thực thành công";
                    NguoiDung nguoiDung = TempData["NguoiDung"] as NguoiDung;
                    nguoiDung.DienThoai = nguoiDung.DienThoai.Trim();
                    nguoiDung.TaiKhoan = nguoiDung.TaiKhoan.Trim();

                    db.NguoiDungs.Add(nguoiDung);
                    db.SaveChanges();
                    return RedirectToAction("Dangnhap", "Home");
                }
                else
                {
                    ModelState.AddModelError("otp", "Mã OTP không đúng vui lòng thử lại");
                    return View();
                }
            }catch (Exception)
            {
                ModelState.AddModelError("otp", "Mã OTP Không hợp lệ !");
                return View();

            }

           
        }

        private bool SendOTP(string email, int otp)
        {
            try
            {
                // Configure SMTP client
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); // Update with your SMTP server details
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("haizaki505@gmail.com", "dnwhqnezsnhqpgmg"); // Update with your email credentials
                smtpClient.EnableSsl = true;

                // Compose email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("haizaki505@gmail.com"); // Update with your email address
                mailMessage.To.Add(email);
                mailMessage.Subject = "Your OTP for Registration";
                mailMessage.Body = "Your OTP is: " + otp.ToString();

                // Send email
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Failed to send email
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public ActionResult Dangnhap()
        {
            return View();
        }

        public ActionResult Dangxuat()
        {

            // Xóa session người dùng khi đăng xuất
            Session["user"] = null;
            Session["userBS"] = null;
            Session["admin"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dangnhap(string username, string password)
        {
            var nguoiDung = db.NguoiDungs.FirstOrDefault(u => u.TaiKhoan == username.Trim() && u.MatKhau == password);
            var quanTri = db.QuanTris.FirstOrDefault(u => u.TaiKhoan == username.Trim() && u.MatKhau == password);
            if (nguoiDung != null)
            {

                Session["user"] = nguoiDung;
                return RedirectToAction("Details", "Nguoidung", new { id = nguoiDung.IDNguoiDung });
            }
            else if (quanTri != null)
            {
                if (quanTri.VaiTro == 1)
                {
                    Session["admin"] = quanTri;
                    return RedirectToAction("QuanTris", "Admin");
                }
                else
                {
                    Session["userBS"] = quanTri;
                    return RedirectToAction("Index", "Home");

                }
               
            }
            else
            {
                // Authentication failed, return back to login view with error message
                ModelState.AddModelError("dangnhasai", "Tên đăng nhập hoặc mật khẩu không đúng !.");
                return View();
            }
        }
        public ActionResult Quenmatkhau()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Quenmatkhau(string email)
        {
            var user = db.NguoiDungs.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("email", "Email không tồn tại trong hệ thống.");
                return View();
            }

            // Gửi OTP đến email của người dùng
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            if (SendOTPForgotPass(email, otp))
            {
                TempData["OTP"] = otp;
                TempData["UserID"] = user.IDNguoiDung;
                return RedirectToAction("VerifyOTPForgotPassword");
            }

            return View();
        }
        public ActionResult VerifyOTPForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyOTPForgotPassword(int otp)
        {
            try
            {
                int storedOTP = Convert.ToInt32(TempData["OTP"]);
                if (otp == storedOTP)
                {
                    ViewBag.Message = "OTP xác thực thành công";
                    TempData["UserID"] = TempData["UserID"];

                    return RedirectToAction("Datlaimatkhau");
                }
                else
                {
                    ModelState.AddModelError("otp", "Mã OTP không đúng vui lòng thử lại");
                    return View();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("otp", "Mã OTP Không hợp lệ !");
                return View();

            }
        }

        private bool SendOTPForgotPass(string email, int otp)
        {
            try
            {
                // Configure SMTP client
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); // Update with your SMTP server details
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("haizaki505@gmail.com", "dnwhqnezsnhqpgmg"); // Update with your email credentials
                smtpClient.EnableSsl = true;

                // Compose email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("haizaki505@gmail.com"); // Update with your email address
                mailMessage.To.Add(email);
                mailMessage.Subject = "Mã OTP để đặt lại mật khẩu";
                mailMessage.Body = "Mã OTP của bạn là: " + otp.ToString();

                // Send email
                smtpClient.Send(mailMessage);
                return true;
            }


            catch (Exception ex)
            {
                // Failed to send email
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public ActionResult Datlaimatkhau()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Datlaimatkhau(string password, string confirmPassword)
        {
            try
            {
                int userId = Convert.ToInt32(TempData["UserID"]);
                var user = db.NguoiDungs.FirstOrDefault(x => x.IDNguoiDung == userId);

                if (password != confirmPassword)
                {
                    ModelState.AddModelError("confirmPassword", "Mật khẩu xác nhận không khớp.");
                    return View();
                }

                user.MatKhau = password;
                user.DienThoai=user.DienThoai.Trim();
                user.TaiKhoan = user.TaiKhoan.Trim();
                db.SaveChanges();
                ViewBag.Message = "Đặt lại mật khẩu thành công.";
                return RedirectToAction("Dangnhap", "Home");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            ViewBag.Message = "Đặt lại mật khẩu thành công.";
            return RedirectToAction("Dangnhap", "Home");
        }

        public ActionResult DangXuatU()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DangXuatBs()
        {
            Session["userBS"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult TintucNewPartial()
        {
            String n = "new";
            var tintuc = db.Tintucs.Where(x => x.TheLoai.Equals(n));
            return PartialView(tintuc);

        }
        public ActionResult TintucHotPartial()
        {
            String h = "hot";
            var tintuc = db.Tintucs.Where(x => x.TheLoai.Equals(h));
            return PartialView(tintuc);

        }
        public ActionResult Xemchitiet(int IDTintuc = 0)
        {
            var chitiet = db.Tintucs.SingleOrDefault(n => n.IDTintuc == IDTintuc);
            if (chitiet == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chitiet);
        }
        [HttpGet]
        public ActionResult TraCuu()
        {
            return View();
        }
    }
   

} 