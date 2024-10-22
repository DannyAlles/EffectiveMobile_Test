using Data.Interfaces;
using Data.Models;
using Domain.Exceptions;
using Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public class LogbookTests
    {
        private MockRepository mockRepository;

        private Mock<ILogbookRepository> mockLogbookRepository;

        [SetUp]
        public void Setup()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLogbookRepository = this.mockRepository.Create<ILogbookRepository>();
        }

        private LogbookService CreateService()
        {
            return new LogbookService(
                this.mockLogbookRepository.Object);
        }

        [TestCase("195.192.190.170", "195.192.190.179", "2023-12-16 14:15:00.000", "2024-12-16 14:15:00.000")]
        public async Task GetLogsByIp_ExpectedBehavior(string startIp, string endIp, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var service = this.CreateService();

            mockLogbookRepository.Setup(x => x.GetLogbooksByIpAsync(IPAddress.Parse(startIp), IPAddress.Parse(endIp), startDate.DateTime, endDate.DateTime))
                                .ReturnsAsync([new() { AccessAt = DateTime.Parse("2023-12-16 14:20:00.000"), IpAddress = "195.192.190.172" }]);

            var result = await service.GetLogsByIpAsync(startIp, endIp, startDate, endDate);

            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [TestCase("195.192.190.170", "195.192", "2023-12-16 14:15:00.000", "2024-12-16 14:15:00.000")]
        [TestCase("195.192", "195.192.190.179", "2023-12-16 14:15:00.000", "2024-12-16 14:15:00.000")]
        [TestCase("asdasdas", "195.192.190.179", "2023-12-16 14:15:00.000", "2024-12-16 14:15:00.000")]
        [TestCase("195.192.190.170", "12312312", "2023-12-16 14:15:00.000", "2024-12-16 14:15:00.000")]
        public async Task GetLogsByIp_NotValidIp(string startIp, string endIp, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var service = this.CreateService();

            Assert.ThrowsAsync<IpAddressNotValidException>(
                       async () =>
                       {
                           await service.GetLogsByIpAsync(startIp, endIp, startDate, endDate);
                       });
        }
    }
}
