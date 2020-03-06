using System;
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
