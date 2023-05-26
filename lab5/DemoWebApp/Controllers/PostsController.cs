using System;
using DemoWebApp.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DemoWebApp.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin,user")]
    public class PostsController : ControllerBase
    {
        private readonly PostsRepositoryService service;

        public PostsController(IRepository<Post> service)
        {
            this.service = (PostsRepositoryService)service;
        }

        [HttpGet("api/posts")]
        public IActionResult Read(
            [FromQuery] string orderBy = "Id",
            [FromQuery] string order = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 25)
        {
            // to-do: Додати можливість фільтрувати пости за назвою та контентом

            var filterBy = new Post();

            return Ok(new Response()
            {
                Status = 200,
                Data = new
                {
                    count = service.Count(filterBy),
                    items = service.Read(filterBy, orderBy, order, page, perPage)
                }
            });
        }

        [HttpGet("api/posts/{id}")]
        public IActionResult ReadById(int id)
        {
            return Ok(new Response()
            {
                Status = 200,
                Data = service.Read(id)
            });
        }

        [HttpPost("api/posts")]
        public IActionResult Create([FromBody] Post post)
        {
            var user = Helpers.AuthHelper.GetUser(HttpContext.User);
            post.UserId = user.Id;
            post.PublishedOn = DateTime.Now;

            return Created(nameof(Post), new Response()
            {
                Status = 201,
                Data = service.Create(post)
            });
        }

        [HttpDelete("api/posts/{id}")]
        public IActionResult Delete(int id)
        {
            // to-do: додати можливість видаляти пости. Користувач може видаляти тільки власні пости. Адміністратор може видаляти всі пости.

            return Ok(new Response()
            {
                Status = 200
            });
        }

        [HttpPut("api/posts/{id}")]
        public IActionResult Update(int id, [FromBody] Post post)
        {
            // to-do: додати можливість редагувати заголовок та вміст поста. Користувач може редагувати тільки власні пости. Адміністратор може редагувати всі пости.

            return Ok(new Response()
            {
                Status = 200
            });
        }

        [HttpPost("api/posts/{postId}/comments")]
        public IActionResult CreateComment(int postId, [FromBody] CommentToPost comment)
        {
            // to-do: додати перевірку - коментар не повинен бути порожнім рядком.

            var user = Helpers.AuthHelper.GetUser(HttpContext.User);
            comment.UserId = user.Id;
            comment.PostId = postId;
            comment.PublishedOn = DateTime.Now;

            return Created(nameof(CommentToPost), new Response()
            {
                Status = 201,
                Data = service.AddCommentToPost(comment)
            });
        }

        [HttpPut("api/posts/{postId}/comments/{commentId}")]
        public IActionResult UpdateComment(int postId, int commentId, [FromBody] CommentToPost comment)
        {
            // to-do: додати перевірку - коментар не повинен бути порожнім рядком.
            // to-do: додати можливість редагувати коментар. І користувач і адміністратор можуть редагувати тільки власні коментарі.

            return Ok(new Response()
            {
                Status = 200
            });
        }

        [HttpDelete("api/posts/{postId}/comments/{commentId}")]
        public IActionResult DeleteComment(int postId, int commentId, [FromBody] CommentToPost comment)
        {
            // to-do: додати можливість видаляти коментар. І користувач і адміністратор можуть видаляти тільки власні коментарі.

            return Ok(new Response()
            {
                Status = 200
            });
        }


    }
}
