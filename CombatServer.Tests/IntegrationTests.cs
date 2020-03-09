using System;
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

            //Once we get here, the message has been sent by the server and the client has received it and reacted to it.
            numberOfConnections.Should().Be(1);
        }

        [Fact]
        public async Task BroadcastPlayerDataMessageShouldSendPlayerInfoToClients()
        {
            // Arrange
            var playerToSend = new Player() {
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
            await connection.InvokeAsync("BroadcastPlayerDataMessage", playerToSend);

            //Assert
            while (!hasListenerBeenTriggered && hasListenerBeenTriggeredAttemptCount < MAX_HAS_LISTENER_BEEN_TRIGGERED_ATTEMPTS)
            {
                Thread.Sleep(1);
                hasListenerBeenTriggeredAttemptCount++;
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