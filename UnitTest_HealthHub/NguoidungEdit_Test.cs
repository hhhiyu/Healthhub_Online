using Microsoft.VisualStudio.TestTools.UnitTesting;
using Healthhub_Online.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Healthhub_Online.Tests.Models
{
    [TestClass]
    public class NguoiDungTests
    {
        [TestMethod]
        public void NguoiDung_Model_WithValidFields_ShouldBeValid()
        {
            // Arrange
            var nguoiDung = new NguoiDung
            {
                HoTen = "Valid Name",
                Email = "email@example.com",
                DienThoai = "0123456789",
                TaiKhoan = "ValidAccount123",
                MatKhau = "ValidPassword",
                NhomMau = "B"
            };

            var context = new ValidationContext(nguoiDung, null, null);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(nguoiDung, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void NguoiDung_Model_WithInvalidEmail_ShouldBeInvalid()
        {
            // Arrange
            var nguoiDung = new NguoiDung
            {
                HoTen = "Valid Name",
                Email = "invalid-email",
                DienThoai = "0123456789",
                TaiKhoan = "ValidAccount123",
                MatKhau = "ValidPassword",
                NhomMau = "A"
            };

            var context = new ValidationContext(nguoiDung, null, null);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(nguoiDung, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(v => v.MemberNames.Contains("Email")));
        }


        [TestMethod]
        public void NguoiDung_Model_WithInvalidPhoneNumber_ShouldBeInvalid()
        {
            // Arrange
            var nguoiDung = new NguoiDung
            {
                HoTen = "Valid Name",
                Email = "email@example.com",
                DienThoai = "abx",
                TaiKhoan = "ValidAccount123",
                MatKhau = "ValidPassword",
                NhomMau = "A"
            };

            var context = new ValidationContext(nguoiDung, null, null);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(nguoiDung, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(v => v.MemberNames.Contains("DienThoai")), "The DienThoai field should have a validation error for invalid phone numbers.");
        }
        [TestMethod]
        public void NguoiDung_Model_MissingName_ShouldBeInvalid()
        {
            var nguoiDung = new NguoiDung
            {
                // HoTen is missing
                Email = "email@example.com",
                DienThoai = "0123456789",
                TaiKhoan = "ValidAccount",
                MatKhau = "ValidPassword",
                NhomMau = "A"
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(nguoiDung, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(nguoiDung, context, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("HoTen")));
        }

        [TestMethod]
        public void NguoiDung_Model_InvalidAccountName_ShouldBeInvalid()
        {
            var nguoiDung = new NguoiDung
            {
                HoTen = "Valid Name",
                Email = "email@example.com",
                DienThoai = "0123456789",
                TaiKhoan = "Invalid Account!", // Contains invalid characters
                MatKhau = "ValidPassword",
                NhomMau = "A"
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(nguoiDung, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(nguoiDung, context, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("TaiKhoan")));
        }

        [TestMethod]
        public void NguoiDung_Model_InvalidBloodType_ShouldBeInvalid()
        {
            var nguoiDung = new NguoiDung
            {
                HoTen = "Valid Name",
                Email = "email@example.com",
                DienThoai = "0123456789",
                TaiKhoan = "ValidAccount",
                MatKhau = "ValidPassword",
                NhomMau = "InvalidBloodType"
            };

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(nguoiDung, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(nguoiDung, context, validationResults, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("NhomMau")));
        }

        [TestMethod]
        public void NguoiDung_Model_WithValidEmailAndInvalidPhoneNumber_ShouldBeInvalid()
        {
            // Arrange
            var nguoiDung = new NguoiDung
            {
                HoTen = "John Doe",
                Email = "john.doe@example.com", // Valid email
                DienThoai = "abcdefghij", // Invalid phone number, assuming it should be numeric
                TaiKhoan = "JohnDoe123",
                MatKhau = "SecurePassword123",
                NhomMau = "O"
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(nguoiDung, null, null);

            // Act
            bool isValid = Validator.TryValidateObject(nguoiDung, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid, "Model should be invalid due to an invalid phone number.");
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("DienThoai")), "Expected a validation error for 'DienThoai'.");
        }
        [TestMethod]
        public void NguoiDung_Model_WithInvalidEmailAndValidPhoneNumber_ShouldBeInvalid()
        {
            // Arrange
            var nguoiDung = new NguoiDung   
            {
                HoTen = "Jane Doe",
                Email = "jane.doe.com", // Invalid email, missing '@'
                DienThoai = "0123456789", // Valid phone number
                TaiKhoan = "JaneDoe321",
                MatKhau = "SecurePassword321",
                NhomMau = "A"
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(nguoiDung, null, null);

            // Act
            bool isValid = Validator.TryValidateObject(nguoiDung, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid, "Model should be invalid due to an invalid email.");
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("Email")), "Expected a validation error for 'Email'.");
        }
        [TestMethod]
        public void NguoiDung_Model_MixedValidInvalidFields_ShouldBeInvalid()
        {
            // Arrange
            var nguoiDung = new NguoiDung
            {
                HoTen = "Mixed Name",
                Email = "validmixedemail@example.com", // Valid
                DienThoai = "0123456789", // Valid
                TaiKhoan = "Invalid Username", // Invalid due to spaces
                MatKhau = "pwd",
                NhomMau = "ABC" 
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(nguoiDung, null, null);

            // Act
            bool isValid = Validator.TryValidateObject(nguoiDung, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("TaiKhoan")), "Expected a validation error for 'TaiKhoan'.");
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains("NhomMau")), "Expected a validation error for 'NhomMau'.");

        }
    }
}