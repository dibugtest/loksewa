using Lok.Data.Interface;
using Lok.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class AuthRepository:IAuthinterface
    {
        private readonly IUnitOfWork _uow;
        private readonly ILoginInterface _Login;
        protected readonly IMangoContext Context;
        protected IMongoCollection<Login> DbSet;

        public AuthRepository(IUnitOfWork uow,ILoginInterface login, IMangoContext context)
        {
            _uow = uow;
            _Login = login;
            Context = context;
        }
        private void ConfigDbSet()
        {
            DbSet = Context.GetCollection<Login>(typeof(Login).Name);
        }

        public async Task<bool> IsUserExists(string Email)
        {
            ConfigDbSet();
            var user =await DbSet.FindAsync(Builders<Login>.Filter.Where(m=>m.Email==Email));
            var a=user.SingleOrDefault();

            if (a!=null)
            {
                return true;
            }
            else
            {
                return false;

            }
        }
        public async Task<Login> GetUser(string Email)
        {
            ConfigDbSet();
            var user = await DbSet.FindAsync(Builders<Login>.Filter.Where(m => m.Email == Email));
            var a = user.SingleOrDefault();

            if (a != null)
            {
                return a;
            }
            else
            {
                return null;

            }
        }






        public async Task<Login> Login(string Email, string Password)
        {
            ConfigDbSet();
            var user = await DbSet.FindAsync(Builders<Login>.Filter.Where(m => m.Email == Email ));
            var a = user.SingleOrDefault();
            if (a == null)
            {
                return null;
            }
            else
            {
                if (a.PasswordHash != null && a.PasswordSalt != null)
                {
                    if (!VerifyPasswordHash(Password, a.PasswordHash, a.PasswordSalt))
                    {
                        return null;
                    }
                    else
                    {
                        return a;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passWordHash, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                var code = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < code.Length; i++)
                {
                    if (code[i] != passWordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //password change
        public async Task<Login> ChangePass(Login Login, string password)
        {
            byte[] passwordhash, passwordsalt;
            createpasswordhash(password, out passwordhash, out passwordsalt);
            ConfigDbSet();
            var user = await DbSet.FindAsync(Builders<Login>.Filter.Where(m => m.Email == Login.Email));
            var logindata = user.SingleOrDefault();
            logindata.PasswordHash = passwordhash;
            logindata.RandomPass = "";
            logindata.PasswordSalt = passwordsalt;
            Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<Login>.Filter.Eq("_id", Login.Id), logindata));
            await _uow.Commit();
            return Login;
        }
       
        private void createpasswordhash(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
    }
}
