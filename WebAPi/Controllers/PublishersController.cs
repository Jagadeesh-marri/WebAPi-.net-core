using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPi.Models;

//This controller is autogenereated based on methods for more info see setup file

namespace WebAPi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly webapiContext _context;

        public PublishersController(webapiContext context)
        {
            _context = context;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublisher()
        {
            return await _context.Publisher.ToListAsync();
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            var publisher = await _context.Publisher.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // PUT: api/Publishers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.PubId)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
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

        // POST: api/Publishers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            _context.Publisher.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublisher", new { id = publisher.PubId }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Publisher>> DeletePublisher(int id)
        {
            var publisher = await _context.Publisher.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publisher.Remove(publisher);
            await _context.SaveChangesAsync();

            return publisher;
        }

        private bool PublisherExists(int id)
        {
            return _context.Publisher.Any(e => e.PubId == id);
        }

        #region this region of code written by me remeaing above was automatically generated
        //This region is created by me remaining is autocreated
        #region Eager Loading
        // GET: api/Publishers/5
        [HttpGet("GetpublisherDetails/{id}")]
        public async Task<ActionResult<Publisher>> GetpublisherDetails(int id)
        {
            //Eager Loading
            //Incluse and theninclude like joins here
            var publisher = await _context.Publisher.Include(pub => pub.Book)
                    .ThenInclude(pub=>pub.Sale)
                .Include(pub=>pub.User)
                .Where(pub => pub.PubId == id).FirstOrDefaultAsync();

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }
        #endregion

        #region Eage loading
        [HttpGet("Getpublisher/{id}")]
        public async Task<ActionResult<Publisher>> Getpublisher(int id)
        {
            //Eager Loading
            var publisher = await _context.Publisher.SingleAsync(pubid => pubid.PubId == id);
            _context.Entry(publisher)
                .Collection(pub => pub.User)
                .Query() //used for filtering
                //.Where(usr=>usr.EmailAddress.Contains("marcus")) if we want we can use where condition also
                .Load();

            _context.Entry(publisher)
                .Collection(pub => pub.Book)
                .Query()
                .Include(book=>book.BookAuthor)
                .Include(book=>book.Sale)
                .Load();

            var user = await _context.User.SingleAsync(usrid => usrid.UserId == id);
            _context.Entry(user)
                .Reference(usr => usr.Role)
                .Load();

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }
        #endregion
        [HttpGet("PostpublisherDetails/")] //Doubt PostpublisherDetails/ slash untene avutundi y?
        public async Task<ActionResult<Publisher>> PostpublisherDetails()
        {
            var publisher = new Publisher();
            publisher.City = "KKD";
            publisher.Country = "India";
            publisher.State = "AP";
            publisher.PublisherName = "Mahesh Chandra";
            _context.Publisher.Add(publisher);
            _context.SaveChanges();

            var publishers = _context.Publisher.Include(pub => pub.Book)
               .ThenInclude(pub => pub.Sale)
               .Include(pub => pub.User)
               .Where(pubid => pubid.PubId == publisher.PubId)
               .FirstOrDefault();
            return publisher;
        }
        #endregion

    }
}
