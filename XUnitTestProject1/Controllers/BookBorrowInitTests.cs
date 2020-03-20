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

    public class BookBorrowInitTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public BookBorrowInitTests()
        {
            _server = ServerFactory.GetServerInstance();
            _client = _server.CreateClient();



            using (var scope = _server.Host.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<LibraryContext>();

                _db.User.Add(new User
                {

                    Email = "jd@pja.edu.pl",
                    Name = "Daniel",
                    Surname = "Jabłoński",
                    Login = "jd",
                    Password = "ASNDKWQOJRJOP!JO@JOP"
                }
                );


                _db.Book.Add(new Book
                {
                    IdBook = 1
                }); ;


                _db.BookBorrow.Add(new BookBorrow
                {
                    IdBookBorrow = 1,
                    IdBook = 1,
                    IdUser = 1
                }); ;




                _db.SaveChanges();
            }
        }
             [Fact]
        public async Task Put_Book_Barrow()
        {
            var newBookBarrow = new BookBorrow()
            {


                IdUser = 1,
                IdBook = 1,
                Comments = "Najlepsza"

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
                IdBookBorrow = 2,
                Comments = "Good"

            };



            var serializedUser = JsonConvert.SerializeObject(newBookBarrow);

            var payload = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            var postResponses = await _client.PostAsync($"{ _client.BaseAddress.AbsoluteUri}api/book-borrows", payload);

            postResponses.EnsureSuccessStatusCode();

            var responseString = await postResponses.Content.ReadAsStringAsync();

            Assert.Contains("Good", responseString);

        }


    }

}
