using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CombatServer.Tests
{
    public class UnitTests
    {
        [Fact]
        public void GetNewTagItemShouldReturnANewTagItem()
        {
            var result = GameMechanics.GetNewTagItem();
            var expected = typeof(NewTagItem);
            Assert.Equal(expected, result.GetType());
        }
    }
}
