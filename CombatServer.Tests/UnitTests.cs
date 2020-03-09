using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace CombatServer.Tests
{
    public class UnitTests
    {
        [Fact]
        public void GetNewTagItemShouldReturnANewTagItem()
        {
            var result = GameMechanics.GetNewTagItem();
            var expected = typeof(NewTagItem);
            expected.Should().Be(result.GetType());
        }

        [Fact]
        public void GetIdShouldReturnUniqueIds()
        {
            var id1 = Utils.GetId();
            var id2 = Utils.GetId();
            var id3 = Utils.GetId();
            var id4 = Utils.GetId();
            var id5 = Utils.GetId();
            var listOfIds = new List<string>() { id1, id2, id3, id4, id5 };

            foreach (var id in listOfIds)
            {
                var otherIds = listOfIds.Where(x => x != id).ToList();
                otherIds.Count.Should().Be(listOfIds.Count - 1);
                foreach (var otherId in otherIds)
                {
                    id.Should().NotBe(otherId);
                }
            }            
        }

        [Fact]
        public void GetObstaclesShouldGenerateNewObstacles()
        {
            GameMechanics.GetObstacles(true);
            PersistingValues.Obstacles.Count.Should().BeGreaterOrEqualTo(Constants.OBSTACLE_AMOUNT_MIN);
        }
        
    }
}
