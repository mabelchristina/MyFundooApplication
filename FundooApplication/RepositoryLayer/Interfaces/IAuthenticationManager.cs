using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
}
