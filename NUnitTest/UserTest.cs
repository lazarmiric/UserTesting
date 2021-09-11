using Moq;
using NUnit.Framework;
using Server.ClientBuisnessLogicLayer;
using Server.DataAccessLayer;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTest
{
    public class UserTest
    {
        private Mock<IUserRepository> mockUserRepository = Mocker.MockUserRepository();
        private UserService service;

        public UserTest()
        {
            service = new UserService(mockUserRepository.Object);
        }

        [Test]
        public void TestWithValidUser_GetAll()
        {
            var result = service.SelectAll();

            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<List<User>>(result);
        }

        [TestCase(null)]
        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(100)]
        public void TestWithUser_GetByID(int? id)
        {

            switch (id)
            {
                case 1:
                    var user = service.SelectById(id);
                    Assert.That(user.UserID == 1);
                    Assert.IsNotNull(user);
                    break;

                case -1:
                    Assert.Throws<Exception>(() =>
                                  service.SelectById(id), "UserId can't be less then 0!");
                    break;

                case null:
                    Assert.Throws<Exception>(() =>
                                  service.SelectById(id), "UserId can't be null!");
                    break;

                default:
                    Assert.Throws<Exception>(() =>
                                 service.SelectById(id), "User doesn't exist!");
                    break;
            }
        }

        [TestCase(1)]
        [TestCase(null)]
        public void TestWithValidUser_Delete(int? id)
        {
            switch (id)
            {
                case null:
                    Assert.Throws<ArgumentNullException>(() =>
                               service.Delete(null));
                    break;
                case 1:
                    var user = service.SelectById(1);
                    var result = service.Delete(user);

                    Assert.That(result.Equals("Successful!"));
                    Assert.Throws<Exception>(() =>
                                service.SelectById(1), "UserID can't be null!");
                    break;
                default: break;
            }
        }

        [Test]
        public void TestWithINvalidUser_Insert()
        {
            var user = new User
            {
                FirstName = "Lazar",
                LastName = "Miric",
                SocialIdentityNumber = "2503998765656",
                Height = 195,
                Weight = 95,
                Address = "Zorke Radulovic 11",
                Email = "laza@gmail.rs",
                PhoneNumber = "+381653222846",
                EyeCollor = "Brown",
                BirthDate = new DateTime(1998, 03, 25),
                PlaceId = 1,
                Note = "masterr"
            };

            var result = service.Insert(user);

            Assert.NotNull(result);
            Assert.That(result.Equals("Successful!"));

            var addedUser = service.SelectAll().Last();

            Assert.AreEqual(user.SocialIdentityNumber, addedUser.SocialIdentityNumber);
        }

        [TestCase("FirstName", "laki")]
        [TestCase("LastName", "")]
        [TestCase("RegistrationNumber", "25322222255")]
        [TestCase("Height", 18)]
        [TestCase("Weight", 565)]
        [TestCase("Email", "laza@gmail.com")]
        [TestCase("DateOfBirth", null)]
        [TestCase("PlaceId", null)]
        public void TestWithValidUser_Insert(string field, object value)
        {
            User user = new User
            {
                UserID = 1,
                FirstName = "Pera",
                LastName = "Peric",
                SocialIdentityNumber = "2503998765555",
                Height = 185,
                Weight = 85,
                Email = "pera@gmail.rs",
                PhoneNumber = "+381653224845",
                EyeCollor = "Brown",
                Address = "Zorke R 11",
                BirthDate = new DateTime(1998, 03, 25),
                PlaceId = 1,
                Note = "master"
            };

            switch (field)
            {
                case "FirstName":
                    user.FirstName = value.ToString();
                    Assert.Throws<Exception>(() => service.Insert(user), "First name must start with upper case!");
                    break;
                case "LastName":
                    user.LastName = value.ToString();
                    Assert.Throws<Exception>(() => service.Insert(user), "Last name cannot be empty!");
                    break;
                case "SocialIdentityNumber":
                    user.SocialIdentityNumber = value.ToString();
                    Assert.Throws<Exception>(() => service.Insert(user), "Invalid SocialIdentityNumber!");
                    break;
                case "Height":
                    user.Height = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Insert(user), "Minimum height is 35");
                    break;
                case "Weight":
                    user.Weight = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Insert(user), "Range for weight is [10,250]");
                    break;
                case "Email":
                    user.Email = value.ToString();
                    Assert.Throws<Exception>(() => service.Insert(user), "Email must contain @ and .rs domain!");
                    break;
                case "DateOfBirth":
                    user.BirthDate = null;
                    Assert.Throws<Exception>(() => service.Insert(user), "Invalid Date of birth!");
                    break;
                case "PlaceId":
                    user.PlaceId = null;
                    Assert.Throws<Exception>(() => service.Insert(user), "Invalid PlaceId!");
                    break;
                default: break;
            }
        }

        [Test]
        public void TestWithValidUser_Update()
        {
            var user = service.SelectById(1);
            user.LastName = "Perkoivc";

            var result = service.Update(user);

            Assert.That(result.Equals("Successful!"));

            var updatedUser = service.SelectById(1);

            Assert.AreEqual(user.LastName, updatedUser.LastName);

        }

        [TestCase("UserId", -1)]
        [TestCase("FirstName", "pera")]
        [TestCase("LastName", "")]
        [TestCase("RegistrationNumber", "551415451")]
        [TestCase("Height", 25)]
        [TestCase("Weight", 566)]
        [TestCase("Email", "pera@sd.com")]
        [TestCase("DateOfBirth", null)]
        [TestCase("PlaceId", null)]
        public void TestWithInvalidUser_Update(string field, object value)
        {
            switch (field)
            {
                case "UserId":
                    var user = service.SelectById(1);
                    user.UserID = -1;
                    Assert.Throws<Exception>(() => service.Update(user), "Invalid UserID!");
                    break;
                case "FirstName":
                    user = service.SelectById(1);
                    user.FirstName = value.ToString();
                    Assert.Throws<Exception>(() => service.Update(user), "First name must start wth upper case!");
                    break;
                case "LastName":
                    user = service.SelectById(1);
                    user.LastName = value.ToString();
                    Assert.Throws<Exception>(() => service.Update(user), "Enter Last name!");
                    break;
                case "SocialIdentityNumber":
                    user = service.SelectById(1);
                    user.SocialIdentityNumber = value.ToString();
                    Assert.Throws<Exception>(() => service.Update(user), "Invalid SocialIdentityNumber number!");
                    break;
                case "Height":
                    user = service.SelectById(1);
                    user.Height = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Update(user), "Minimum height is 35!");
                    break;
                case "Weight":
                    user = service.SelectById(1);
                    user.Weight = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Update(user), "Range for weight is [10,250]");
                    break;
                case "Email":
                    user = service.SelectById(1);
                    user.Email = value.ToString();
                    Assert.Throws<Exception>(() => service.Update(user), "Email must contain @ wuth .rs domain!");
                    break;
                case "DateOfBirth":
                    user = service.SelectById(1);
                    user.BirthDate = null;
                    Assert.Throws<Exception>(() => service.Update(user), "Invalid Date of birth!");
                    break;
                case "PlaceId":
                    user = service.SelectById(1);
                    user.PlaceId = null;
                    Assert.Throws<Exception>(() => service.Update(user), "Invalid PlaceId!");
                    break;
                default: break;
            }
        }
    }
}