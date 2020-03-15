using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace XUnitTestProject1.Controllers
{
    public class UsersInitTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;


        public UsersInitTests()
        {
            _server = ServerFactory.GetServerInstance();
            _client = _server.CreateClient();


            using (var scope = _server.Host.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<LibraryContext>();

                _db.User.Add(new User
                {
                    IdUser = 1,
                    Email = "jd@pja.edu.pl",
                    Name = "Daniel",
                    Surname = "Jabłoński",
                    Login = "jd",
                    Password = "ASNDKWQOJRJOP!JO@JOP"
                });

                _db.Book.Add(new Book
                {
                    IdBook = 1
                }); ;

                _db.SaveChanges();
            }
        }


        [Fact]
        public async Task GetUsers_200Ok()
        {
            //Arrange i Act
            var httpResponse = await _client.GetAsync($"{_client.BaseAddress.AbsoluteUri}api/users");

            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            // using (var scope = _server.Host.Services.CreateScope())
            // {
            //     var _db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
            //     Assert.True(_db.User.Any());
            // }

            Assert.True(users.Count() == 1);
            Assert.True(users.ElementAt(0).Login == "jd");
        }

        [Fact]
        public async Task Get_User_By_Id()
        {
            //Arrange i Act
            var httpResponse = await _client.GetAsync($"{_client.BaseAddress.AbsoluteUri}api/users/1");

            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(content);
          

            Assert.True(user.Login == "jd");
            Assert.True(user.Email == "jd@pja.edu.pl");
           
        }


        [Fact]
        public async Task Put_Book_Barrow()
        {
            var newBookBarrow = new BookBorrow()
            {

                
                IdUser = 1,
                IdBookBorrow = 1,
                IdBook = 1

            };

            var serializedUser = JsonConvert.SerializeObject(newBookBarrow);

            var payload = new StringContent(serializedUser, Encoding.UTF8, "application/json");


            // Act
            var response = await _client.PutAsync($"{ _client.BaseAddress.AbsoluteUri}api/book-borrows/1", payload);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task POST_Book_Barrow()
        {
            var newBookBarrow = new BookBorrow()
            {

                IdBook = 1,
                IdUser = 1,
                IdBookBorrow = 1,
               
            };

          

            var serializedUser = JsonConvert.SerializeObject(newBookBarrow);

            var payload = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            var postResponses = await _client.PostAsync($"{ _client.BaseAddress.AbsoluteUri}api/book-borrows", payload);

            postResponses.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_User()
        {
            //Arrange i Act


            var newUser = new User()
            {
                IdUser = 1,
                Email = "bkosl@pja.edu.pl",
                Name = "Grzegsorz",
                Surname = "Noswak",
                Login = "Woss",
                Password = "BSsNDKWQOJRJOP!JO@JOP"
            };

            List<User> usersToPost = new List<User>{
                new User
                {
                    IdUser = 32,
                    Email = "ws@pja.edu.pl",
                    Name = "Wojtek",
                    Surname = "Bob",
                    Login = "Zah",
                    Password = "ASNDKWQOJRJOP!JO@JOP"
                },
                new User
                    {
                        IdUser = 35,
                        Email = "ws@pja.edu.pl",
                        Name = "Wojtek",
                        Surname = "Bob",
                        Login = "Zah",
                        Password = "ASNDKWQOJRJOP!JO@JOP"
                    },
                new User
                    {
                        IdUser = 37,
                        Email = "bkol@pja.edu.pl",
                        Name = "Grzegorz",
                        Surname = "Nowak",
                        Login = "Wos",
                        Password = "ASNDKWQOJRJOP!JO@JOP"
                    }};

            var serializedUser = JsonConvert.SerializeObject(newUser);

            var payload = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            var postResponses = await _client.PostAsync($"{ _client.BaseAddress.AbsoluteUri}api/users", payload);

            postResponses.EnsureSuccessStatusCode();
           
      

        }

    }
}
