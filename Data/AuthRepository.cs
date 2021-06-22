using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoBackend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoBackend;
using MailKit.Net.Smtp;
using MimeKit;





namespace TodoBackend.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DbContextBase _context;
        private IEmailService email;
        private readonly Random _random = new Random();


        public AuthRepository(DbContextBase context, IEmailService email)
        {
            _context = context;
            this.email = email;

        }
        public async Task<Object> Login(string username, string password)
        {
            var usern = new User
            {
                Id = -1,
                Username = "User or Password Not Found",
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2]

            };
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username); //Get user from database.
            if (user == null)
                return GetTokenAuth(usern);
            // return usern; // User does not exist.

            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return GetTokenAuth(usern);
            // return usern;
            return GetTokenAuth(user);
            //  return user;
        }

        public Object GetTokenAuth(User user)
        {
            string key = "my_secret_key_12345"; //Secret key which will be used later during validation    
            var issuer = "http://mysite.com";  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", "1"));
            permClaims.Add(new Claim("userid", user.Id.ToString()));
            permClaims.Add(new Claim("name", user.Username));
            permClaims.Add(new Claim("verified", user.IsVerified.ToString()));


            var stauscode = false;

            if (user.Id == -1)
            {
                stauscode = false;
                permClaims.Add(new Claim("authenticated", "false"));

            }
            else
            {
                stauscode = true;
                permClaims.Add(new Claim("authenticated", "true"));
            }

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddMinutes(5),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);


            return new
            {
                access_token = jwt_token,
                // expires_in = DateTime.Now.AddDays(1),
                expires_in = DateTime.Now.AddMinutes(5),

                staus = stauscode
            };
        }
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Create hash using password salt.
                for (int i = 0; i < computedHash.Length; i++)
                { // Loop through the byte array
                    if (computedHash[i] != passwordHash[i]) return false; // if mismatch
                }
            }
            return true; //if no mismatches.
        }

        public async Task<User> Register(string user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var obj = new User
            {
                Id = 0,
                Username = user,
                Verification = _random.Next(1000, 9999),
                IsVerified = 0,
                PasswordHash = new byte[2],
                PasswordSalt = new byte[2]

            };
            obj.PasswordHash = passwordHash;
            obj.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(obj); // Adding the user to context of users.
            await _context.SaveChangesAsync(); // Save changes to database.

            this.SendEamilSubj(user, "Verification Code is " + obj.Verification);


            return obj;

        }

        public void SendEamilSubj(string email, string text)
        {
            var message = this.email.GetMessageObjForAuthLoginEmailEx(email, text);
            message.Subject = "Email Verification";
            message.Body = new TextPart("html")
            {
                Text = text
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp-mail.outlook.com", 587, false);
                client.Authenticate("todolistapp442@outlook.com", "P@kist0n123");
                client.Send(message);
                client.Disconnect(true);
            }
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }
    }
}