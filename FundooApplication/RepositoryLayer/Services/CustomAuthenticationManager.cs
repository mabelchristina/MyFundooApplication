using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CustomAuthenticationManager : ICustomAuthenticationManager
    {
        private readonly IList<UserCredentials> users = new List<UserCredentials>
        {
            new UserCredentials{ username= "Jane@gmail.com",password="Jane",Role="Administrator"},
            new UserCredentials{ username= "kir@gmail.com",password="kir@123",Role="User"},
            new UserCredentials{ username= "Jane@gmail.com",password="Jane",Role="User"},
        };
        private readonly IDictionary<string, Tuple<string, string>> tokens =
    new Dictionary<string, Tuple<string, string>>();

        public IDictionary<string, Tuple<string, string>> Tokens => tokens;

        public string Authenticate(string username, string password)
        {
            if (!users.Any(u => u.username == username && u.password == password))
            {
                return null;
            }

            var token = Guid.NewGuid().ToString();

            tokens.Add(token, new Tuple<string, string>(username,
                users.First(u => u.username == username && u.password == password).Role));

            return token;
        }
    }
}
