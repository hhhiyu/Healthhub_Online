using Healthhub_Online.Controllers;
using Healthhub_Online.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net;

namespace HealthhubOnline.Tests.Controllers
{
    [TestClass]
    public class NguoidungControllerTest
    {
        private Mock<modelWeb> mockContext;
        private Mock<DbSet<NguoiDung>> mockSet;
        private NguoidungController controller;

        [TestInitialize]
        public void Initialize()
        {
            mockSet = new Mock<DbSet<NguoiDung>>();
            mockContext = new Mock<modelWeb>();
            mockContext.Setup(m => m.NguoiDungs).Returns(mockSet.Object);

            controller = new NguoidungController(mockContext.Object);
        }


        [TestMethod]
        public void Details_WithNullId_ReturnsBadRequest()
        {
            // Arrange
            int? id = null;

            // Act
            var result = controller.Details(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            var httpResult = result as HttpStatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, httpResult.StatusCode);
        }

        [TestMethod]
        public void SaveButton_Click_UserWithId1_SuccessfulSave()
        {
            // Arrange
            GioiTinh testGender1 = new GioiTinh { IDGioiTinh = 1, GioiTinh1 = "Nam" };
            TinhThanh testCity1 = new TinhThanh { IDTinh = 1, TenTinh = "Thanh Hóa" };
            var data = new List<NguoiDung>
        {
            new NguoiDung { 
                IDGioiTinh = 1,
                HoTen = "huy", 
                GioiTinh = testGender1,
                TinhThanh = testCity1,
                IDTinh = testCity1.IDTinh,
                BenhAns = null,
                DiaChiCuThe = "hehe",
                DienThoai = "123123123",
                Email = "hihihaha@gmail.com",
                HoiDaps = null,
                IDNguoiDung = 696,
                LichKhams = null,
                MatKhau = "123123",
                NhomMau = "AB",
                SoCMND = 123123123,
                TaiKhoan = "hhhiyu",
                ThongTinKhac = null }
            // Add more users if needed
        }.AsQueryable();

            var mockSet = new Mock<DbSet<NguoiDung>>();
            mockSet.As<IQueryable<NguoiDung>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<NguoiDung>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<NguoiDung>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<NguoiDung>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<modelWeb>();
            mockContext.Setup(c => c.NguoiDungs).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChanges()).Returns(1); // Assume saving changes returns 1, indicating a successful operation

            var controller = new NguoidungController(mockContext.Object);

            var userToUpdate = new NguoiDung
            {
                HoTen = "huy",
                GioiTinh = testGender1,
                TinhThanh = testCity1,
                IDTinh = testCity1.IDTinh,
                BenhAns = null,
                DiaChiCuThe = "hehe",
                DienThoai = "123123123",
                Email = "hihihahahehe@gmail.com",
                HoiDaps = null,
                IDNguoiDung = 6962,
                LichKhams = null,
                MatKhau = "123123",
                NhomMau = "AB",
                SoCMND = 123123123,
                TaiKhoan = "hhhiyu2",
                ThongTinKhac = null
            };

            // Act
            var result = controller.Edit(userToUpdate); // Simulate clicking save by calling the Edit method

            // Assert
            mockContext.Verify(m => m.SaveChanges(), Times.Once); // Verify that SaveChanges was called exactly once
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult)); // Assume success is indicated by redirecting to another view
        }
        [TestMethod]
        public void Edit_POST_WithChangedInformation_UpdatesUserAndRedirects()
        {
            // Arrange
            var data = new List<NguoiDung>
    {
        new NguoiDung { IDNguoiDung = 1, HoTen = "Original Name", Email = "original@example.com", DienThoai = "1234567890" }
        // Populate with initial data as needed
    }.AsQueryable();

            var mockSet = new Mock<DbSet<NguoiDung>>();
            mockSet.As<IQueryable<NguoiDung>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<NguoiDung>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<NguoiDung>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<NguoiDung>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<modelWeb>();
            mockContext.Setup(c => c.NguoiDungs).Returns(mockSet.Object);

            var controller = new NguoidungController(mockContext.Object);
            var updatedUser = new NguoiDung { IDNguoiDung = 1, HoTen = "Updated Name", Email = "updated@example.com", DienThoai = "0987654321" };

            // Act
            var result = controller.Edit(updatedUser) as RedirectToRouteResult;

            // Assert
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsNotNull(result);
            // Add additional asserts here if needed to check for specific route values or model state
        }

    }
}