using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatHub.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR.Client;
using Xunit;

namespace CombatServer.Tests
{
    public class ChatHubTests : IClassFixture<WebApiFactory>
    {
        private readonly WebApiFactory _factory;
        private const int MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS = 1000;

        public ChatHubTests(WebApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task BroadcastPlayerWinsShouldSendWinnersInfoToClients()
        {
            // Arrange
            var winnerId = "(id is not set yet)";
            var connection = GetConnection(_factory);
            var hasListenerBeenTriggered = false;
            var hasListenerBeenTriggeredAttemptCount = 0;
            connection.On<Player>("broadcastPlayerWins", (player) =>
            {
                winnerId = player.id;
                hasListenerBeenTriggered = true;
            });
            await connection.StartAsync();

            // Act
            await connection.InvokeAsync("BroadcastPlayerWins", new Player() { id = "1234" });

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }
            if (!hasListenerBeenTriggered)
            {
                throw new Exception("Listener wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            winnerId.Should().Be("1234");
        }

        [Fact]
        public async Task BroadcastGetTagPlayerIdShouldSendTagIdToClients()
        {
            // Arrange
            var tagPlayersId = "(id is not set yet)";
            var connection = GetConnection(_factory);
            var hasListenerBeenTriggered = false;
            var hasListenerBeenTriggeredAttemptCount = 0;

            connection.On<string>("broadcastGetTagPlayerId", (tagId) =>
            {
                tagPlayersId = tagId;
                hasListenerBeenTriggered = true;
            });
            await connection.StartAsync();

            // Act
            PersistingValues.TagPlayerId = "1113";            
            await connection.InvokeAsync("BroadcastGetTagPlayerId");

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }
            if (!hasListenerBeenTriggered)
            {
                throw new Exception("Listener wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            tagPlayersId.Should().Be("1113");
        }

        [Fact]
        public async Task BroadcastConnectionAmountDataShouldSendTheNumberOfConnectionsToClients()
        {
            // Arrange
            var numberOfConnections = 0;
            var connection = GetConnection(_factory);
            var hasListenerBeenTriggered = false;
            var hasListenerBeenTriggeredAttemptCount = 0;

            connection.On<int>("broadcastconnectionamountdata", (connections) =>
            {
                numberOfConnections = connections;
                hasListenerBeenTriggered = true;
            });
            await connection.StartAsync();

            // Act
            await connection.InvokeAsync("BroadcastConnectionAmountData", 1);

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }
            if (!hasListenerBeenTriggered)
            {
                throw new Exception("Listener wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            numberOfConnections.Should().Be(1);
        }

        [Fact]
        public async Task BroadcastPlayerDataMessageShouldSendPlayerInfoToClients()
        {
            // Arrange            
            Player receivedPlayer = null;
            var connection = GetConnection(_factory);
            var hasListenerBeenTriggered = false;
            var hasListenerBeenTriggeredAttemptCount = 0;

            connection.On<Player>("broadcastPlayerDataMessage", (player) =>
            {
                receivedPlayer = player;
                hasListenerBeenTriggered = true;
            });
            await connection.StartAsync();

            // Act
            var playerToSend = new Player()
            {
                id = Utils.GetId(),
                positionX = 2,
                positionY = 3,
                direction = 90,
                hitPoints = 5,
                manaAmount = 90,
                playerColor = "red",
                movementState = Enums.MovementState.Forward,
                sizeX = 5,
                sizeY = 5,
                score = 10,
                hasPlayerStartingPositionBeenSet = true,
                movementIntervalMs = 100
            };
            await connection.InvokeAsync("BroadcastPlayerDataMessage", playerToSend);

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }
            if (!hasListenerBeenTriggered)
            {
                throw new Exception("Listener wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            receivedPlayer.id.Should().Be(playerToSend.id);
            receivedPlayer.positionX.Should().Be(playerToSend.positionX);
            receivedPlayer.positionY.Should().Be(playerToSend.positionY);
            receivedPlayer.direction.Should().Be(playerToSend.direction);
            receivedPlayer.hitPoints.Should().Be(playerToSend.hitPoints);
            receivedPlayer.manaAmount.Should().Be(playerToSend.manaAmount);
            receivedPlayer.playerColor.Should().Be(playerToSend.playerColor);
            receivedPlayer.movementState.Should().Be(playerToSend.movementState);
            receivedPlayer.sizeX.Should().Be(playerToSend.sizeX);
            receivedPlayer.sizeY.Should().Be(playerToSend.sizeY);
            receivedPlayer.score.Should().Be(playerToSend.score);
            receivedPlayer.hasPlayerStartingPositionBeenSet.Should().Be(playerToSend.hasPlayerStartingPositionBeenSet);
            receivedPlayer.movementIntervalMs.Should().Be(playerToSend.movementIntervalMs);
        }

        [Fact]
        public async Task BroadcastFireballDataMessageShouldSendFireballDataToClients()
        {
            // Arrange
            Fireball receivedFireball = null;
            var connection = GetConnection(_factory);
            var hasListenerBeenTriggered = false;
            var hasListenerBeenTriggeredAttemptCount = 0;

            connection.On<Fireball>("broadcastFireballDataMessage", (fireball) =>
            {
                receivedFireball = fireball;
                hasListenerBeenTriggered = true;
            });
            await connection.StartAsync();

            // Act
            var fireballToSend = new Fireball()
            {
                id = Utils.GetId(),
                positionX = 2,
                positionY = 3,
                direction = 180,
                sizeX = 2,
                sizeY = 2,
                casterId = Utils.GetId(),
                isDestroyed = false,
                moveIntervalMs = 50
            };
            await connection.InvokeAsync("BroadcastFireballDataMessage", fireballToSend);

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }
            if (!hasListenerBeenTriggered)
            {
                throw new Exception("Listener wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            receivedFireball.id.Should().Be(fireballToSend.id);
            receivedFireball.positionX.Should().Be(fireballToSend.positionX);
            receivedFireball.positionY.Should().Be(fireballToSend.positionY);
            receivedFireball.direction.Should().Be(fireballToSend.direction);
            receivedFireball.sizeX.Should().Be(fireballToSend.sizeX);
            receivedFireball.sizeY.Should().Be(fireballToSend.sizeY);
            receivedFireball.moveIntervalMs.Should().Be(fireballToSend.moveIntervalMs);
            receivedFireball.isDestroyed.Should().Be(fireballToSend.isDestroyed);
            receivedFireball.casterId.Should().Be(fireballToSend.casterId);
        }

        [Fact]
        public async Task BroadcastFireballHitPlayerMessageShouldSendHittingDataToClients()
        {
            // Arrange
            FireballHitPlayerData fireballHitPlayerData = null;
            var connection = GetConnection(_factory);
            var hasListenerBeenTriggered = false;
            var hasListenerBeenTriggeredAttemptCount = 0;

            connection.On<Object>("broadcastFireballHitPlayerMessage", (hittingData) =>
            {
                //With some types, the conversion from json to a C# object must be done by hand:
                fireballHitPlayerData = Newtonsoft.Json.JsonConvert.DeserializeObject<FireballHitPlayerData>(hittingData.ToString());
                hasListenerBeenTriggered = true;
            });
            await connection.StartAsync();

            // Act
            await connection.InvokeAsync("BroadcastFireballHitPlayerMessage", new Fireball() { id = "2", casterId = "1" }, new Player() { id = "3", hitPoints = 4 });

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }            
            if (!hasListenerBeenTriggered)
            {
                throw new Exception("Listener wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            fireballHitPlayerData.fireballId.Should().Be("2");
            fireballHitPlayerData.playerId.Should().Be("3");
        }

        [Fact]
        public async Task BroadcastFireballHitPlayerMessageShouldSendNewTagToClients()
        {
            // Arrange
            string receivedNewTagPlayerId = null;
            var connection = GetConnection(_factory);
            var hasListenerBeenTriggered = false;
            var hasListenerBeenTriggeredAttemptCount = 0;

            connection.On<string>("broadcastPlayerBecomesTag", (tagId) =>
            {
                receivedNewTagPlayerId = tagId;
                hasListenerBeenTriggered = true;
            });
            await connection.StartAsync();

            // Act
            PersistingValues.TagPlayerId = "3";
            await connection.InvokeAsync("BroadcastFireballHitPlayerMessage", new Fireball() { id = "2", casterId = "1" }, new Player() { id = "3", hitPoints = 1 }); //Fireball hits the tag, who has 1 hp -> Change who is tag.

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }
            if (!hasListenerBeenTriggered)
            {
                throw new Exception("Listener wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            receivedNewTagPlayerId.Should().Be("1");
        }

        [Fact]
        public async Task BroadcastGetObstaclesShouldSendObstacleDataToClients()
        {
            // Arrange
            List<Obstacle> receivedObstacles = null;
            var connection = GetConnection(_factory);
            var hasListenerBeenTriggered = false;
            var hasListenerBeenTriggeredAttemptCount = 0;

            connection.On<List<Obstacle>>("broadcastGetObstacles", (obstacles) =>
            {
                receivedObstacles = obstacles;
                hasListenerBeenTriggered = true;
            });
            await connection.StartAsync();

            // Act
            PersistingValues.Obstacles = new List<Obstacle>() { new Obstacle() { positionX = 5, positionY = 10 } };
            await connection.InvokeAsync("BroadcastGetObstacles", false);

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }
            if (!hasListenerBeenTriggered)
            {
                throw new Exception("Listener wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            receivedObstacles.Count.Should().Be(1);
            receivedObstacles.First().positionX.Should().Be(5);
            receivedObstacles.First().positionY.Should().Be(10);
        }

        [Fact]
        public async Task BroadcastPlayerHitNewTagItemShouldSendNewTagIdAndNewTagItemDataToClients()
        {
            // Arrange
            string receivedTagId = null;
            NewTagItem receivedTagItem = null;

            var hasListenerBeenTriggeredPlayerBecomesTag = false;
            var hasListenerBeenTriggeredNewTagItem = false;
            var hasListenerBeenTriggeredAttemptCount = 0;
            var connection = GetConnection(_factory);
            connection.On<Object>("newTag", (tagItem) =>
            {
                //With some types, the conversion from json to a C# object must be done by hand:
                receivedTagItem = Newtonsoft.Json.JsonConvert.DeserializeObject<NewTagItem>(tagItem.ToString());
                hasListenerBeenTriggeredNewTagItem = true;
            });
            connection.On<string>("broadcastPlayerBecomesTag", (tagId) =>
            {
                receivedTagId = tagId;
                hasListenerBeenTriggeredPlayerBecomesTag = true;
            });            
            await connection.StartAsync();

            // Act
            await connection.InvokeAsync("BroadcastPlayerHitNewTagItem", "123");

            //Assert
            while ((!hasListenerBeenTriggeredPlayerBecomesTag || !hasListenerBeenTriggeredNewTagItem) && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }
            if (!hasListenerBeenTriggeredPlayerBecomesTag)
            {
                throw new Exception("Listener (broadcastPlayerBecomesTag) wasn't triggered.");
            }
            if (!hasListenerBeenTriggeredNewTagItem)
            {
                throw new Exception("Listener (newTag) wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            receivedTagId.Should().Be("123");
            receivedTagItem.isInPlay.Should().BeFalse();
        }

        [Fact]
        public async Task BroadcastNewTagItemDataShouldSendNewTagItemToClients()
        {
            // Arrange
            NewTagItem receivedTagItem = null;
            var connection = GetConnection(_factory);
            var hasListenerBeenTriggered = false;
            var hasListenerBeenTriggeredAttemptCount = 0;

            connection.On<Object>("newTag", (tagItem) =>
            {
                //With some types, the conversion from json to a C# object must be done by hand:
                receivedTagItem = Newtonsoft.Json.JsonConvert.DeserializeObject<NewTagItem>(tagItem.ToString());
                hasListenerBeenTriggered = true;
            });
            await connection.StartAsync();

            // Act
            PersistingValues.TagItem = new NewTagItem(4, 5, true);
            await connection.InvokeAsync("BroadcastNewTagItemData");

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
            }
            if (!hasListenerBeenTriggered)
            {
                throw new Exception("Listener wasn't triggered.");
            }

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            receivedTagItem.Should().NotBe(null);
            receivedTagItem.positionX.Should().Be(4);
        }

        private static HubConnection GetConnection(WebApiFactory factory)
        {
            factory.CreateClient(); // need to create a client for the server property to be available
            var server = factory.Server;
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:4200/hub", o =>
                {
                    o.HttpMessageHandlerFactory = _ => server.CreateHandler();
                })
                .Build();
            
            return hubConnection;
        }
    }
}