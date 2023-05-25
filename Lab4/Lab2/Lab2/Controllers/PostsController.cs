using Lab2.Models;
using Lab2.Data;
using Lab2.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Lab2.Services;
using System.ComponentModel.Design;

namespace Lab2.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin,user")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _postcontext;

        public PostsController(ApplicationDbContext context)
        {
            this._postcontext = context;
        }
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

        // GET: api/posts?title={title}&content={content}
        [HttpGet]
        public IActionResult GetPosts(string title, string content)
        {
            var filteredPosts = _postcontext.Posts;

            if (!string.IsNullOrEmpty(title))
            {
                filteredPosts = filteredPosts.Where(p => p.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(content))
            {
                filteredPosts = filteredPosts.Where(p => p.Content.Contains(content));
            }

            return Ok(filteredPosts);
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
            var post = _postcontext.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            // Перевірка прав доступу для видалення публікації
            if (!IsAuthorizedToDelete(post))
            {
                return Unauthorized();
            }

            _postcontext.Posts.Remove(post);
            _postcontext.SaveChanges();


            return Ok(new Response()
            {
                Status = 200
            });
        }

        [HttpPut("api/posts/{id}")]
        public IActionResult Update(int id, [FromBody] Post post)
        {
            // to-do: додати можливість редагувати заголовок та вміст поста. Користувач може редагувати тільки власні пости. Адміністратор може редагувати всі пости.
            var publication = _postcontext.Posts.FirstOrDefault(p => p.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

            publication.Title = post.Title;
            publication.Content = post.Content;

            _postcontext.Posts.Update(publication);

            return Ok(new Response()
            {
                Status = 200
            });
        }

        [HttpPost("api/posts/{postId}/comments")]
        public IActionResult CreateComment(int postId, [FromBody] CommentToPost comment)
        {
            // to-do: додати перевірку - коментар не повинен бути порожнім рядком.
            if (string.IsNullOrEmpty(comment.Content))
            {
                return BadRequest("Коментар не може бути порожнім.");
            }


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
        public IActionResult UpdateComment(int postId, int commentId, [FromBody] CommentToPost updatedComment)
        {
            // to-do: додати перевірку - коментар не повинен бути порожнім рядком.
            if (string.IsNullOrEmpty(updatedComment.Content))
            {
                return BadRequest("Коментар не може бути порожнім.");
            }

            // to-do: додати можливість редагувати коментар. І користувач і адміністратор можуть редагувати тільки власні коментарі.

            var comment = _postcontext.Comments.FirstOrDefault(с => с.Id == commentId); ;
            if (comment == null)
            {
                return NotFound();
            }

            comment.Content = updatedComment.Content;

            _postcontext.Comments.Update(comment);

            return Ok(new Response()
            {
                Status = 200
            });
        }

        [HttpDelete("api/posts/{postId}/comments/{commentId}")]
        public IActionResult DeleteComment(int postId, int commentId, [FromBody] CommentToPost comment)
        {
            // to-do: додати можливість видаляти коментар. І користувач і адміністратор можуть видаляти тільки власні коментарі.

             _postcontext.Comments.FirstOrDefault(comment => comment.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            // Перевірка прав доступу для видалення коментаря
            if (!service.IsAuthorizedToDelete(comment))
            {
                return Unauthorized();
            }

            _postcontext.Comments.Remove(comment);
            _postcontext.SaveChanges();


            return Ok(new Response()
            {
                Status = 200
            });
        }
        [HttpPost("{id}/like")]
        public IActionResult LikeComment(int id)
        {
            var comment = _postcontext.Comments.FirstOrDefault(comment => comment.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            // Логіка для додавання вподобання до коментарія
            comment.Likes++;

            _postcontext.Comments.Update(comment);
            return Ok(comment);
        }

    }
}
