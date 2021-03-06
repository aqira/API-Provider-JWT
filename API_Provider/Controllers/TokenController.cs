﻿using API_Provider.Context;
using API_Provider.Models;
using API_Provider.ViewModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

namespace API_Provider.Controllers
{
    public class TokenController : ApiController
    {
        MyContext db = new MyContext();

        [Route("GetToken/{id}")]
        // GET: Token
        public string GetTokenCustomer(string id)
        {
            SqlParameter paramId = new SqlParameter("@Id", id);
            UserRoleVM getById = db.Database.SqlQuery<UserRoleVM>("SP_AspNetUserRoles_TokenMapping @Id", paramId).Single(); ;

            string key = "my_secret_key_12345"; //Secret key which will be used later during validation    
            var issuer = "http://localhost:59950/";  //normally this will be your site URL    
            var audience = "http://localhost:59950/";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("id", getById.Id));
            permClaims.Add(new Claim("name", getById.Name));
            permClaims.Add(new Claim("email", getById.Email));
            permClaims.Add(new Claim("role", getById.Role));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issuer    
                            audience,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddYears(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }

        [Route("GetName1")]
        [HttpPost]
        public String GetName1()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                }
                return "Valid";
            }
            else
            {
                return "Invalid";
            }
        }

        [Route("GetName2")]
        [Authorize]
        [HttpPost]
        public Object GetName2()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;

                var role = claims.Where(p => p.Type == "Role").FirstOrDefault()?.Value;
                return new
                {
                    data = role
                };

            }
            return null;
        }
    }

}
