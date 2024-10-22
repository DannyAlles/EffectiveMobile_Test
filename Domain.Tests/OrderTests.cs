using Data.Interfaces;
using Data.Models;
using Domain.Exceptions;
using Domain.Services;
using Infrastructure;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public class OrderTests
    {
        private MockRepository mockRepository;

        private Mock<IOrderRepository> mockOrderRepository;
        private Mock<ILogbookRepository> mockLogbookRepository;
        private Mock<IDistrictRepository> mockDistrictRepository;
        private Mock<IOptions<DeliveryConfigurationOptions>> mockOptions;

        private readonly DeliveryConfigurationOptions _configData = new()
        {
            OrderOptions = new()
            {
                MinutesAfterFirst = 30,
                DecimalPlacesNum = 3
            },
            ConnectionStrings = new()
            {
                Postgres = ""
            }
        };

        [SetUp]
        public void Setup()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockOrderRepository = this.mockRepository.Create<IOrderRepository>();
            this.mockLogbookRepository = this.mockRepository.Create<ILogbookRepository>();
            this.mockDistrictRepository = this.mockRepository.Create<IDistrictRepository>();
            this.mockOptions = this.mockRepository.Create<IOptions<DeliveryConfigurationOptions>>();

            mockOptions.Setup(x => x.Value).Returns(_configData);
        }

        private OrderService CreateService()
        {
            return new OrderService(
                this.mockOrderRepository.Object,
                this.mockLogbookRepository.Object,
                this.mockDistrictRepository.Object,
                this.mockOptions.Object);
        }

        [Test]
        public async Task CreateOrder_ExpectedBehavior()
        {
            var service = this.CreateService();

            Order newOrder = new()
            {
                DeliveryTime = DateTime.Now,
                DistrictId = Guid.NewGuid(),
                Number = "123456",
                Weight = 0.512
            };

            mockOrderRepository.Setup(x => x.GetOrderByOrderNumberAsync(It.IsAny<string>())).ReturnsAsync((Order)null);
            mockOrderRepository.Setup(x => x.CreateOrderAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            mockLogbookRepository.Setup(x => x.CreateLogbookAsync(It.IsAny<Logbook>())).Returns(Task.CompletedTask);

            mockOrderRepository.Setup(x => x.GetOrderByIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(newOrder);

            var result = await service.CreateOrderAsync(newOrder, IPAddress.Parse("192.137.123.209"));

            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateOrder_NullResponse()
        {
            var service = this.CreateService();

            Order newOrder = new()
            {
                DeliveryTime = DateTime.Now,
                DistrictId = Guid.NewGuid(),
                Number = "123456",
                Weight = 0.512
            };

            mockOrderRepository.Setup(x => x.GetOrderByOrderNumberAsync(It.IsAny<string>())).ReturnsAsync((Order)null);
            mockOrderRepository.Setup(x => x.CreateOrderAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            mockLogbookRepository.Setup(x => x.CreateLogbookAsync(It.IsAny<Logbook>())).Returns(Task.CompletedTask);

            mockOrderRepository.Setup(x => x.GetOrderByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Order)null);

            Assert.ThrowsAsync<OrderNotFoundException>(
                       async () =>
                       {
                           await service.CreateOrderAsync(newOrder, IPAddress.Parse("192.137.123.209"));
                       });
            this.mockRepository.VerifyAll();
        }

        [TestCase("ee7c9c25-955b-44cd-93dc-80630636f167", "2023-12-16 14:15:00.000", 1, 2)]
        public async Task GetOrdersByDistrict_ExpectedBehavior(Guid districtId, DateTimeOffset firstDeliveryDateTime, int page, int perPage)
        {
            var service = this.CreateService();

            mockDistrictRepository.Setup(x => x.GetDistrictByIdAsync(districtId)).ReturnsAsync(new District()
            {
                Titile = "New"
            });

            mockOrderRepository.Setup(x => x.GetOrderByFirstDeliveryDateTimeAsync(firstDeliveryDateTime.DateTime, districtId)).ReturnsAsync(new Order()
            {
                DeliveryTime = DateTime.Now,
                DistrictId = districtId,
                Number = "123456",
                Weight = 3
            });

            mockOrderRepository.Setup(x => x.GetOrdersByDistrictAndTimeAsync(districtId, firstDeliveryDateTime.DateTime, _configData.OrderOptions.MinutesAfterFirst, page, perPage)).ReturnsAsync(
            [
                new()
                {
                    DeliveryTime = firstDeliveryDateTime.DateTime,
                    DistrictId = districtId,
                    Number = "123456",
                    Weight = 3
                },
                new()
                {
                    DeliveryTime = firstDeliveryDateTime.DateTime,
                    DistrictId = districtId,
                    Number = "123457",
                    Weight = 3
                }
            ]);

            var result = await service.GetOrdersByDistrictAsync(districtId, firstDeliveryDateTime, page, perPage);

            Assert.That(result.Count(), Is.EqualTo(2));
            this.mockRepository.VerifyAll();
        }

        [TestCase("ee7c9c25-955b-44cd-93dc-80630636f167", "2023-12-16 14:15:00.000", 1, 2)]
        public async Task GetOrdersByDistrict_DistrictNotFound(Guid districtId, DateTimeOffset firstDeliveryDateTime, int page, int perPage)
        {
            var service = this.CreateService();

            mockDistrictRepository.Setup(x => x.GetDistrictByIdAsync(districtId)).ReturnsAsync((District)null);

            Assert.ThrowsAsync<DistrictNotFoundException>(
                       async () =>
                       {
                           await service.GetOrdersByDistrictAsync(districtId, firstDeliveryDateTime, page, perPage);
                       });
            this.mockRepository.VerifyAll();
        }

        [TestCase("ee7c9c25-955b-44cd-93dc-80630636f167", "2023-12-16 14:15:00.000", 1, 2)]
        public async Task GetOrdersByDistrict_OrdertNotFound(Guid districtId, DateTimeOffset firstDeliveryDateTime, int page, int perPage)
        {
            var service = this.CreateService();

            mockDistrictRepository.Setup(x => x.GetDistrictByIdAsync(districtId)).ReturnsAsync(new District()
            {
                Titile = "New"
            });

            mockOrderRepository.Setup(x => x.GetOrderByFirstDeliveryDateTimeAsync(firstDeliveryDateTime.DateTime, districtId)).ReturnsAsync((Order)null);

            Assert.ThrowsAsync<OrderNotFoundException>(
                       async () =>
                       {
                           await service.GetOrdersByDistrictAsync(districtId, firstDeliveryDateTime, page, perPage);
                       });
            this.mockRepository.VerifyAll();
        }
    }
}
