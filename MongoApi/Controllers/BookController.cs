using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoApi.DataAccess;
using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(BookDTO Book)
        {
            await MongoDataAccess.Create(new Book() { Name = Book.Name });
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await MongoDataAccess.GetAll<Book>();
            return Ok(books);
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id, BookDTO book)
        {
            await MongoDataAccess.Update(Id, new Book() { Name = book.Name });
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            await MongoDataAccess.Delete<Book>(Id);
            return Ok();
        }

    }
}
