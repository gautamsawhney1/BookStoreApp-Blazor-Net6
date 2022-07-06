using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Static;
using BookStoreApp.API.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<BooksController> logger;

        public BooksController(BookStoreDbContext context, IMapper mapper, ILogger<BooksController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            logger.LogInformation($"Request to {nameof(GetBooks)}");
            try
            { //if (_context.Books == null)
              //{
              //    return NotFound();
              //}

                //var books = await _context.Books.Include(q => q.Author).ToListAsync();
                var bookDtos = await _context.Books
                    .Include(q => q.Author)
                    .ProjectTo<BookDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
                
                //var books = await _context.Books.Select(q => new {q.Id,q.Title }). Include(q => q.Author).ToListAsync();
               // var bookDtos = mapper.Map<IEnumerable<BookDto>>(books);
                return Ok(bookDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing Get in {nameof(GetBooks)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }
            // GET: api/Books/5
            [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {

            try
            {

                var bookDtos = await _context.Books
                    .Include(q => q.Author)
                    .ProjectTo<BookDto>(mapper.ConfigurationProvider)
                    .ToListAsync();

                //var book = await _context.Books.FindAsync(id);
                //if (_context.Authors == null)
                //{
                //    logger.LogWarning($"Record not found:{nameof(GetBook)} - ID:{id} ");
                //    return NotFound();
                //}


                if (bookDtos == null)
                {
                    return NotFound();
                }
                var bookDto = mapper.Map<BookDto>(bookDtos);
                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing Get in {nameof(GetBook)}");
                return StatusCode(500, Messages.Error500Message);
            }



          //  if (_context.Books == null)
          //{
          //    return NotFound();
          //}
          //  var book = await _context.Books.FindAsync(id);

          //  if (book == null)
          //  {
          //      return NotFound();
          //  }

            //return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
       //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutBook(int id, BookDto book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookDto>> PostBook(BookDto bookDto)
        {
            //  if (_context.Books == null)
            //  {
            //      return Problem("Entity set 'BookStoreDbContext.Books'  is null.");
            //  }
            //    _context.Books.Add(book);
            //    await _context.SaveChangesAsync();

            //    return CreatedAtAction("GetBook", new { id = book.Id }, book);
            //}

            try
            {
                var book = mapper.Map<Book>(bookDto);

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing POST in {nameof(PostBook)}", bookDto);
                return StatusCode(500, Messages.Error500Message);
            }
        }


        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
