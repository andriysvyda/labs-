using DemoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.Services.Repositories
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

            return context.Posts.OrderBy(post => post.Id).Skip((page - 1)* perPage).Take(perPage).ToList();
        }

        public Post Read(int id)
        {
            return context.Posts.Include(post => post.Comments).FirstOrDefault(post => post.Id == id);
        }

        public void Update(Post post)
        {
            // to-do: додати можливість модифікації заголовку та вмісту поста
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

        public void UpdateCommentToPost(int id)
        {
            // to-do: додати можливість редагувати коментар за ідентифікатором
        }

        public void DeleteCommentToPost(int id)
        {
            // to-do: додати можливість видаляти коментар за ідентифікатором
        }
    }
}
