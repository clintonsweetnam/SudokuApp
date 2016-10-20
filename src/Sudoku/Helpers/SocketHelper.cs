using Newtonsoft.Json;
using Sudoku.Services.Messaging;
using Sudoku.Repositories;
using Sudoku.Types;
using Sudoku.Types.Exceptions;
using Sudoku.Types.Messages;
using Sudoku.Types.Messages.MessageContentTypes;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sudoku.Helpers
{
    public class SocketHelper
    {
        private readonly GameRepository _gameRepository;
        private readonly MessageHandlerFactory _messageHandlerFactory;

        public SocketHelper()
        {
            _gameRepository = new GameRepository();
            _messageHandlerFactory = new MessageHandlerFactory();
        }

        public async Task HandleConnection(WebSocket webSocket)
        {
            var message = await ReadAndParseMessage(webSocket);

            if (message.Type != MessageType.Connection)
                throw new Exception("Connection message must be of type Connection");

            var newUserMessage = JsonConvert.DeserializeObject<NewUserMessage>(message.Content);

            var game = CreateUserAndGame(newUserMessage, webSocket);

            while (true)
            {
                try
                {
                    message = await ReadAndParseMessage(webSocket);
                    var messageHandler = _messageHandlerFactory.CreateMessageHandler(message.Type);
                    await messageHandler.HandleMessage(message);
                }
                catch (SocketDisconnectedException)
                {
                    break;
                }
            }
        }

        #region Private Methods
        private async Task<Game> CreateUserAndGame(NewUserMessage newUserMessage, WebSocket webSocket)
        {
            var user = new User(webSocket);

            var game = _gameRepository.GetGame(newUserMessage.GameId);

            game.SetPlayer(user);

            if (game == null)
            {
                await user.TrySendMessage(new SocketMessage(MessageType.Info, "Successfully started a new game", game.Id));
            }
            else
            {
                var gameReadyToStartMessage = new GameReadyToStartMessage(3);

                await user.TrySendMessage(new SocketMessage(MessageType.Info, "Successfully joined the game", game.Id));
                await game.SendToBothPlayers(new SocketMessage(MessageType.GameReady, JsonConvert.SerializeObject(gameReadyToStartMessage), game.Id));
                
            };

            return game;
        }

        private async Task<SocketMessage> ReadAndParseMessage(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.CloseStatus.HasValue)
                throw new SocketDisconnectedException();

            var messageJson = System.Text.Encoding.UTF8.GetString(Decode(buffer));
            var socketMessage = JsonConvert.DeserializeObject<SocketMessage>(messageJson);
            return socketMessage;
        }

        private byte[] Decode(byte[] packet)
        {
            var i = packet.Length - 1;
            while (packet[i] == 0)
            {
                --i;
            }
            var temp = new byte[i + 1];
            Array.Copy(packet, temp, i + 1);
            return temp;
        }
        #endregion
    }
}
