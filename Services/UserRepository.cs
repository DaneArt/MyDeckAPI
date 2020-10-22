using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Exceptions;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using MyDeckApi_Experimental.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly  MDContext _context;
        private readonly DbSet<User> table;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public UserRepository(MDContext context, SnakeCaseConverter snakeCaseConverter, AuthUtils security, MailService mailService)
        {
            _context = context;
            table = _context.Set<User>();
            this.snakeCaseConverter = snakeCaseConverter;

        }

        public Task Delete(Guid Id)
        {
            var subscriptions = _context.Subscribes.Where(p => p.Publisher_Id == Id);
            _context.Subscribes.RemoveRange(subscriptions);
            subscriptions = _context.Subscribes.Where(f => f.Follower_Id == Id);
            _context.Subscribes.RemoveRange(subscriptions);
            User exists = table.Find(Id);
            table.Remove(exists);
            return Task.Run(() => _context.SaveChangesAsync());
        }

        public Task<List<User>> FindAll()
        {
            return table.ToListAsync();
        }

        public ValueTask<User> FindById(Guid Id)
        {
            return table.FindAsync(Id);
        }

        public  Task Insert(User user)
        {
             table.AddAsync(user);
           
            return Task.Run(() => Save());
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Task Update(User user)
        {
            table.Update(user);
            return Task.Run(() => Save());
        }
        //TODO cellphone  signin/signup      
        //TODO email signin/signup           
        //TODO apple ID signin               
        //TODO facebook signin               
        //TODO cellphone verify + code verfiy
        //TODO email verify + code verify    
        //TODO password change               
        //TODO username change               
        //TODO email change                  
        //TODO problems with username        
        //TODO pт others problems               

        public bool IsUserNameUnique(string name)
        {
            var username = name;
            var usr = table.FirstOrDefault(u => u.UserName == username);
            if (usr != null) { return false; }
            return true;
        }
        public bool IsEmailUnique(string email)
        {
            var tmpemail = email;
            var usr =  table.FirstOrDefault(u => u.Email == tmpemail);
            if (usr != null) { return false; }
            return true;
        }
        public string GetUniqueUserName(string name)
        {
            Random random = new Random();
            var tmp = random.Next(0, 9999999);
            var tmpname = name + Convert.ToString(tmp);
            if (IsUserNameUnique(tmpname))
            {
                return tmpname;
            }
            else
            {
                return GetUniqueUserName(name);
            }
        }

      

        public async ValueTask<string> GetUserProfile(Guid userId)
        {
            var usr = _context.Users.Find(userId);
            var user = new { usr.User_Id, usr.UserName/*,usr.Avatar_Path*/, usr.Locale };
            var subs = _context.Subscribes.Where(s => s.Publisher_Id == userId).Count();
            var follows = _context.Subscribes.Where(s => s.Follower_Id == userId).Count();
            var content = new { User = user, Subscribers = subs, Follows = follows };
            return snakeCaseConverter.ConvertToSnakeCase(content);
        }

       public async ValueTask<string> GetSubscribersOfDeck(Guid userId)
        {
            var usr = _context.Users.Find(userId);
            var subs = _context.Subscribes.Where(s => s.Publisher_Id == userId).Count();
            var follows = _context.Subscribes.Where(s => s.Follower_Id == userId).Count();
            var content = new { user = usr, subscribers = subs, follows };
            return snakeCaseConverter.ConvertToSnakeCase(content);
        }

        public  bool ValidateUserUnique(User user)
        {
           if ( IsEmailUnique(user.Email))
                {
                    if (IsUserNameUnique(user.UserName))
                    {
                        return true;

                    }
                    else
                    {
                        throw new AlreadyUsedNameException();
                    }
                }
                else
                {
                    throw new AlreadyUsedEmailException();
                }
        }

        public Task<User> getUserIfGoogleAuth(string gooleId)
        {
           return table.Where(u => u.GoogleId == gooleId).FirstOrDefaultAsync();
        }

        public Task<User> FindByUsername(string username)
        {
            return table.Where(u => u.UserName == username).FirstOrDefaultAsync();
        }

        public Task<User> FindByEmail(string email)
        {
            return table.Where(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}
