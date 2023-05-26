using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DemoWebApp.Hubs
{
    [Authorize]
    public class ChatHub: Microsoft.AspNetCore.SignalR.Hub
    {

        private readonly IChatService chat_service;

        public ChatHub(IChatService chatService)
        {
            chat_service = chatService;
        }

        /* to-do: видалити це поле після того як буде додано можливість передавати chatId як параметр */
        private readonly string CHAT_ID = "CommonChat";

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Join(/* to-do: додати вхідний параметр chatId */)
        {
            /* to-do: додати валідацію чи має право користувач під'єднатися до чату */

            await Groups.AddToGroupAsync(Context.ConnectionId, CHAT_ID);
        }

        public async Task Leave(/* to-do: додати вхідний параметр chatId */)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, CHAT_ID);
        }

        public async Task SendMessage(/* to-do: додати вхідний параметр chatId */ string message)
        {
            /* to-do: додати валідацію чи має право користувач надсилати повідомлення у чаті */

            var user = Helpers.AuthHelper.GetUser(Context.User);
            await Clients.Group(CHAT_ID).SendAsync("Message", new { user, message });

            /* to-do: зберегти меседж у БД у таблиці меседжів */
        }
    }
}