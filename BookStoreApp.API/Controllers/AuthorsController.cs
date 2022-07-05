using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using AutoMapper;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<AuthorsController> logger;

       
        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            logger.LogInformation($"Request to {nameof(GetAuthors)}");
            try
            {
                //if (_context.Authors == null)
                //{
                //    return NotFound();
                //}
                var authors = await _context.Authors.ToListAsync();
                var authorDto = mapper.Map<IEnumerable<AuthorDto>>(authors);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing Get in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto >> GetAuthor(int id)
        {

            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (_context.Authors == null)
                {
                    logger.LogWarning($"Record not found:{nameof(GetAuthor)} - ID:{id} ");
                    return NotFound();
                }


                if (author == null)
                {
                    return NotFound();
                }
                var authorDto = mapper.Map<AuthorDto>(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing Get in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDto authorDto)
        {
     
            
            if (id != authorDto.Id)
            {
                logger.LogWarning($"Update ID invalid in {nameof(Author)} - ID: {id}");
                return BadRequest();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author== null)
            {
                logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} - ID: {id}");
                return BadRequest();
            }
            mapper.Map(authorDto,author);
           _context.Entry(author).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(AuthorDto authorDto)
        {
            //if (_context.Authors == null)
            //{
            //    return Problem("Entity set 'BookStoreDbContext.Authors'  is null.");
            //}
            try
            {
                var author = mapper.Map<Author>(authorDto);

                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Error performing POST in {nameof(PostAuthor)}", authorDto);
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                if (_context.Authors == null)
                {
                    return NotFound();
                }
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    logger.LogWarning($"{nameof(Author)} record not found in {nameof(DeleteAuthor)} - ID:{id}");
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex,$"Error Performing DELETE in {nameof(Author)}");
                return StatusCode(500, Messages.Error500Message);
            }

        }

        private async Task<bool> AuthorExists(int id)
        {

            //return await (_context.Authors?.AnyAsync(e => e.Id == id)).GetValueOrDefault();
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}
