using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CombatServer.Tests
{
    public class UnitTests
    {
        private CombatHub hub;
        public UnitTests()
        {
            //Create a hub with mock clients
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            hub = new CombatHub() { Clients = mockClients.Object };
        }

        [Fact]
        public void GetNewTagItemShouldReturnANewTagItem()
        {
            var result = GameMechanics.GetNewTagItem();
            var expected = typeof(NewTagItem);
            Assert.Equal(expected, result.GetType());
        }
    }
}
