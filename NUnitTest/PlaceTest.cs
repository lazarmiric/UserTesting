using Moq;
using NUnit.Framework;
using Server.ClientBuisnessLogicLayer;
using Server.DataAccessLayer;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUnitTest
{
    public class PlaceTest
    {
        private Mock<IPlaceRepository> mockPlaceRepository =  Mocker.MockPlaceRepository();
        private PlaceService service;

        public PlaceTest()
        {
            service = new PlaceService(mockPlaceRepository.Object);
        }

        [Test]
        public void TestPlace_Get()
        {
            var result = service.SelectAll();
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<List<Place>>(result);
        }

        [TestCase(null)]
        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(100)]
        public void TestPlace_GetByID(int? id)
        {
            switch (id)
            {
                case 1:
                    var place = service.SelectById(id);
                    Assert.That(place.PlaceId == 1);
                    Assert.IsNotNull(place);
                    break;

                case -1:
                    Assert.Throws<Exception>(() =>
                                  service.SelectById(id), "PlaceId can't be less then 0!");
                    break;

                case null:
                    Assert.Throws<Exception>(() =>
                                  service.SelectById(id), "PlaceId can't be null!");
                    break;

                default:
                    Assert.Throws<Exception>(() =>
                                 service.SelectById(id), "Place doesn't exist!");
                    break;
            }
        }

        [Test]
        public void TestWithValidPlace_Insert()
        {
            var place = new Place
            {
                CityName = "Smederevo",
                Zipcode = 11300,
                Population = 60000
            };

            var result = service.Insert(place);

            Assert.NotNull(result);

            Assert.That(result.Equals("Successful!"));

            var addedPlace = service.SelectAll().Last();

            Assert.AreEqual(place.CityName, addedPlace.CityName);
        }

        [TestCase("Name", "bor")]
        public void TestWithInvalidPlace_Insert(string field, object value)
        {
            Place place = new Place
            {
                CityName = "Name",
                Zipcode = 11000,
                Population = 1000000
            };

            switch (field)
            {
                case "Name":
                    place.CityName = value.ToString();
                    Assert.Throws<Exception>(() => service.Insert(place), "Name must start with Upper case!");
                    break;
                case "Zipcode":
                    place.Zipcode = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Insert(place), "Not a valid zipcode!");
                    break;
                case "Population":
                    place.Population = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Insert(place), "Population can't be less then 0");
                    break;
                default: break;
            }
        }

        [Test]
        public void TestWithValidPlace_Update()
        {
            var place = service.SelectById(1);

            place.CityName = "Updated name";

            var result = service.Update(place);

            Assert.That(result.Equals("Successful!"));

            var updatedPlace = service.SelectById(1);

            Assert.AreEqual(place.CityName, updatedPlace.CityName);
        }

        [TestCase("PlaceId", -1)]
        [TestCase("Name", "Nis")]
        [TestCase("Zipcode", 10)]
        [TestCase("Population", -1)]
        public void TestWithInvalidPlace_Update(string field, object value)
        {
            var places = service.SelectAll();
            Place place = places.First();

            switch (field)
            {
                case "PlaceId":
                    Place invalidPlace = new Place()
                    {
                        PlaceId = Convert.ToInt32(value),
                        CityName = place.CityName,
                        Population = place.Population,
                        Zipcode = place.Zipcode
                    };

                    Assert.Throws<Exception>(() => service.Update(invalidPlace), "PlaceId can't be under 1!");
                    break;
                case "Name":
                    invalidPlace = new Place()
                    {
                        PlaceId = place.PlaceId,
                        CityName = value.ToString(),
                        Population = place.Population,
                        Zipcode = place.Zipcode
                    };
                    Assert.Throws<Exception>(() => service.Update(invalidPlace), "Name must be Upper case!");
                    break;
                case "Zipcode":
                    invalidPlace = new Place()
                    {
                        PlaceId = place.PlaceId,
                        CityName = place.CityName,
                        Population = place.Population,
                        Zipcode = Convert.ToInt32(value)
                    };
                    Assert.Throws<Exception>(() => service.Update(invalidPlace), "Invalid zipcode!");
                    break;
                case "Population":
                    invalidPlace = new Place()
                    {
                        PlaceId = place.PlaceId,
                        CityName = place.CityName,
                        Population = Convert.ToInt32(value),
                        Zipcode = place.Zipcode
                    };
                    Assert.Throws<Exception>(() => service.Update(invalidPlace), "Population can't be under 0!");
                    break;
                default: break;
            }
        }

        [TestCase(1)]
        [TestCase(null)]
        public void TestWithValidPlace_Delete(int? id)
        {
            switch (id)
            {
                case null:
                    Assert.Throws<ArgumentNullException>(() =>
                               service.Delete(null));
                    break;
                case 1:
                    var place = service.SelectById(1);
                    var result = service.Delete(place);
                    Assert.That(result.Equals("Successful!"));
                    Assert.Throws<Exception>(() =>
                                service.SelectById(1), "PlaceId can't be null!");

                    break;
                default: break;
            }
        }
    }
}
