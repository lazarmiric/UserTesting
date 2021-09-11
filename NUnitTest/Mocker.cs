using Moq;
using Server.DataAccessLayer;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUnitTest
{
    public class Mocker
    {
        public static Mock<IPlaceRepository> MockPlaceRepository()
        {

            var places = new List<Place>()
            {
                new Place
                {
                    PlaceId = 1,
                    CityName = "Smederevo",
                    Zipcode = 11300,
                    Population = 66000
                },
                new Place
                {
                    PlaceId = 2,
                    CityName = "Beograd",
                    Zipcode = 11000,
                    Population = 950000,
                }
            };

            var mockRepository = new Mock<IPlaceRepository>();

            mockRepository.Setup(repo => repo.SelectAll()).Returns(places);

            mockRepository.Setup(repo => repo.SelectById(It.IsAny<int>())).Returns((int i) => places.SingleOrDefault(x => x.PlaceId == i));

            mockRepository.Setup(i => i.Insert(It.IsAny<Place>())).Callback((Place place) =>
            {
                var id = places.Count() + 1;
                place.PlaceId = id;
                places.Add(place);

            }).Returns("Success!");

            mockRepository.Setup(m => m.Update(It.IsAny<Place>())).Callback((Place target) =>
            {
                var original = places.FirstOrDefault(
                    q => q.PlaceId == target.PlaceId);

                if (original != null)
                {
                    original.CityName = target.CityName;
                    original.Population = target.Population;
                    original.Zipcode = target.Zipcode;
                }

            }).Returns("Success!");

            mockRepository.Setup(m => m.Delete(It.IsAny<Place>())).Callback((Place i) =>
            {
                var original = places.FirstOrDefault(
                    q => q.PlaceId == i.PlaceId);

                if (original != null)
                {
                    places.Remove(original);
                }

            }).Returns("Success!");

            return mockRepository;
        }

        public static Mock<IUserRepository> MockUserRepository()
        {
            var users = new List<User>()
            {
                new User
                {
                    UserID = 1,
                    SocialIdentityNumber = "2503998760010",
                    FirstName = "Lazar",
                    LastName = "Miric",
                    BirthDate = new DateTime(1998, 03, 25),
                    Height = 185,
                    Weight = 91,
                    PhoneNumber = "+381653222846",
                    Email = "lazamiric98@gmail.rs",
                    Address = "Zorke R 11",
                    EyeCollor = "Brown",
                    Note = "Student na fon-u",
                    PlaceId = 1,
                }
            };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.SelectAll()).Returns(users);

            mockRepository.Setup(repo => repo.SelectById(It.IsAny<int>())).Returns((int i) => users.SingleOrDefault(x => x.PlaceId == i));

            mockRepository.Setup(i => i.Insert(It.IsAny<User>())).Callback((User user) =>
            {
                var id = users.Count() + 1;
                user.UserID = id;
                users.Add(user);

            }).Returns("Success!");

            mockRepository.Setup(m => m.Update(It.IsAny<User>())).Callback((User target) =>
            {
                var original = users.FirstOrDefault(
                    q => q.UserID == target.UserID);

                if (original != null)
                {
                    original.FirstName = target.FirstName;
                    original.LastName = target.LastName;
                    original.SocialIdentityNumber = target.SocialIdentityNumber;
                    original.BirthDate = target.BirthDate;
                    original.PhoneNumber = target.PhoneNumber;
                    original.Height = target.Height;
                    original.Weight = target.Weight;
                    original.EyeCollor = target.EyeCollor;
                    original.Address = target.Address;
                    original.Email = target.Email;
                    original.Note = target.Note;
                    original.PlaceId = target.PlaceId;
                }

            }).Returns("Success!");

            mockRepository.Setup(m => m.Delete(It.IsAny<User>())).Callback((User i) =>
            {
                var original = users.FirstOrDefault(
                    q => q.UserID == i.UserID);

                if (original != null)
                {
                    users.Remove(original);
                }

            }).Returns("Success!");

            return mockRepository;
        }
    }
}
