using Sudoku.Types.Messages;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sudoku.Types.Exceptions;

namespace Sudoku.Types
{
    public class User
    {
        private WebSocket webSocket;
        public Guid Id { get; private set; }

        public bool IsUserSocketOpen {
            get
            {
                return webSocket.State == WebSocketState.Open;
            }
        }

        public User(WebSocket webSocket)
        {
            this.Id = Guid.NewGuid();
            this.webSocket = webSocket;
        }


        public async Task<bool> TrySendMessage(SocketMessage message)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                var byteMessage = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                await webSocket.SendAsync(new ArraySegment<byte>(byteMessage, 0, byteMessage.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
