using Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Lab2.Data;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Services.Repositories
{
    public class PostsRepositoryService : IRepository<Post>
    {
        private readonly ApplicationDbContext context;

        public PostsRepositoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Post Create(Post entity)
        {
            var entityEntry = context.Posts.Add(entity);
            context.SaveChanges();

            return entityEntry.Entity;
        }

        public void Delete(int id)
        {
            // to-do: додати можливість видалення поста за ідентифікатором
        }

        public List<Post> Read(Post filterBy, string orderBy, string order, int page, int perPage)
        {
            // to-do: додати можливість фільтрувати пости за заголовком чи вмістом
            // to-do: додати можливість сортувати пости за заголовком або датою публікації

            return context.Posts.OrderBy(post => post.Id).Skip((page - 1) * perPage).Take(perPage).ToList();
        }

        public Post Read(int id)
        {
            return context.Posts.Include(post => post.Comments).FirstOrDefault(post => post.Id == id);
        }

        public void Update(Post updatedPost)
        {
            Post post = context.Posts.FirstOrDefault(p => p.Id == updatedPost.Id);

            if (post != null)
            {
                post.Title = updatedPost.Title;
                post.Content = updatedPost.Content;

            }
            else
            {
                Console.WriteLine("Пост з вказаним ідентифікатором не знайдений.");
            }
        }

        public int Count(Post filterBy)
        {
            // to-do: додати значення фільтра при підрахунку

            return context.Posts.Count();
        }

        public CommentToPost AddCommentToPost(CommentToPost comment)
        {
            var entityEntry = context.Comments.Add(comment);
            context.SaveChanges();

            return entityEntry.Entity;
        }

        public void UpdateCommentToPost(int id, [FromBody] string newContent)
        {
            // to-do: додати можливість редагувати коментар за ідентифікатором

            CommentToPost commentToUpdate = context.Comments.FirstOrDefault(c => c.Id == id);
            if (commentToUpdate != null)
            {
                commentToUpdate.Content = newContent;
                commentToUpdate.PublishedOn = DateTime.Now;
            }
            else
            {
                Console.WriteLine("Коментар з вказаним ідентифікатором не знайдений.");
            }
        }

        public void DeleteCommentToPost(int id, [FromBody] User user)
        {
            // to-do: додати можливість видаляти коментар за ідентифікатором
        }
        public bool IsAuthorizedToDelete([FromBody] User user, CommentToPost comment)
        {
            // Логіка перевірки прав доступу користувача
            if (user.isAdmin == true)
            {
                // Адміністратор має право видаляти будь-які публікації та коментарі
                return true;
            }
            if (comment.UserId == user.Id)
            {
                return true;
            }

            return false;
        }
    }
}
