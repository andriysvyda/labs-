using DemoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DemoWebApp.Helpers;

namespace DemoWebApp.Services.Repositories
{
    public class UsersRepositoryService : IRepository<User>
    {
        private readonly ApplicationDbContext context;

        public UsersRepositoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public User Create(User entity)
        {
            entity.Password = CryptoHelper.EncryptPassword(entity.Password);
            var entityEntry = context.Users.Add(entity);
            context.SaveChanges();

            return new User()
            {
                Id = entityEntry.Entity.Id,
                Name = entityEntry.Entity.Name,
                Email = entityEntry.Entity.Email,
                DisplayName = entityEntry.Entity.DisplayName,
                Age = entityEntry.Entity.Age,
                isAdmin = entityEntry.Entity.isAdmin
            };
        }

        public void Delete(int id)
        {
            var entity = context.Users.FirstOrDefault(user => user.Id == id);
            context.Remove(entity);
            context.SaveChanges();
        }

        public List<User> Read(User filterBy, string orderBy, string order, int page, int perPage)
        {
            var filteredItems = context.Users
                .Where(user =>
                    (filterBy.Name != null ? user.Name.ToLower().Contains(filterBy.Name.ToLower()) : true)
                    &&
                    (filterBy.Email != null ? user.Email.ToLower().Contains(filterBy.Email.ToLower()) : true));

            IOrderedQueryable<User> orderedItems;

            switch ($"{orderBy}_{order}".ToLower())
            {
                case "id_desc":
                    orderedItems = filteredItems.OrderByDescending(u => u.Id);
                    break;
                case "name_asc":
                    orderedItems = filteredItems.OrderBy(u => u.Name);
                    break;
                case "name_desc":
                    orderedItems = filteredItems.OrderByDescending(u => u.Name);
                    break;
                case "email_asc":
                    orderedItems = filteredItems.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    orderedItems = filteredItems.OrderByDescending(u => u.Email);
                    break;
                default:
                    orderedItems = filteredItems.OrderBy(u => u.Id);
                    break;
            };

            return orderedItems
                .Skip((page - 1)* perPage)
                .Take(perPage)
                .Select(user => new User()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    DisplayName = user.DisplayName,
                    Age = user.Age,
                    isAdmin = user.isAdmin
                })
                .ToList();
        }

        public User Read(int id)
        {
            var user = context.Users.FirstOrDefault(user => user.Id == id);

            if (user == null) return null;

            return new User()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Age = user.Age,
                isAdmin = user.isAdmin
            };
        }

        public void Update(User user)
        {
            var entity = context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (entity != null)
            {
                entity.Email = user.Email;
                entity.Name = user.Name;
                entity.DisplayName = user.DisplayName;
                entity.Age = user.Age;
                entity.isAdmin = user.isAdmin;

                context.SaveChanges();
            }
        }

        public int Count(User filterBy)
        {
            return context.Users
                .Count(user =>
                    (filterBy.Name != null ? user.Name.ToLower().Contains(filterBy.Name.ToLower()) : true)
                    &&
                    (filterBy.Email != null ? user.Email.ToLower().Contains(filterBy.Email.ToLower()) : true));
        }

        public bool UserNameExist(User user)
        {
            return context.Users.Any(u => u.Id != user.Id && u.Name.ToLower() == user.Name.ToLower());
        }

        public bool EmailExist(User user)
        {
            return context.Users.Any(u => u.Id != user.Id && u.Email.ToLower() == user.Email.ToLower());
        }
    }
}
