using System;
using DemoWebApp.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DemoWebApp.Controllers
{
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        public ChatsController(IRepository<Chat> service)
        {
            this.service = (ChatsRepositoryService)service;
        }

        [HttpGet("api/chats")]
        public IActionResult Read(
            [FromQuery] string orderBy = "Id",
            [FromQuery] string order = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 25)
        {
            // to-do: додати можливість отримувати cписок доступних для користувача чатів
            var userId = Helpers.AuthHelper.GetUserId(HttpContext.User);

            var chats = service.GetChatsForUser(userId, orderBy, order, page, perPage);

            return Ok();
        }

        [HttpPost("api/chats")]
        public IActionResult Create()
        {
            // to-do: додати можливість створювати чат
            var chat = new Chat();
            service.Create(chat);

            return Ok();
        }

        [HttpDelete("api/chats/{id}")]
        public IActionResult Delete(int id)
        {
            // to-do: додати можливість видаляти чат
             var userId = Helpers.AuthHelper.GetUserId(HttpContext.User);

            if (service.CanUserDeleteChat(id, userId))
            {
                service.Delete(id);

                return Ok();
            }
        }

        [HttpGet("api/chats/{chatId}/messages")]
        public IActionResult ReadMessages(
            int chatId,
            [FromQuery] string orderBy = "Id",
            [FromQuery] string order = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 25)
        {
            // to-do: додати можливість отримувати список повідомлень у чаті
            var messages = service.GetMessagesForChat(chatId, orderBy, order, page, perPage);

            return Ok();
        }
    }
}
