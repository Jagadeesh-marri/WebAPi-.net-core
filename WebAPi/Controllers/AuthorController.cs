using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPi.Models;

namespace WebAPi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private webapiContext _webapiContext; //to add this we have to add db connection first in start up file

        public AuthorController(webapiContext webapiContext)
        {
            _webapiContext = webapiContext;
        }

        [HttpGet]
        public IActionResult GetAuthor()
        {
            try
            {
                var authorsCount = _webapiContext.Author.ToList();
                return Ok(authorsCount);
            }
            catch(Exception)
            {
                return StatusCode(500, "Error raised");
            }
        }

        [HttpGet]
        public IActionResult GetAuthorById()
        {
            try
            {
                var getAuthorbyID = _webapiContext.Author.Where(author_id => author_id.AuthorId == 59852).FirstOrDefault();
                return Ok(getAuthorbyID);
            }
            catch(Exception EX)
            {
                return (IActionResult)EX;
            }
        }

        [HttpGet]
        public IActionResult GetAuthorBy_FirstName(string f_name)
        {
            f_name = "Cheryl";
            var getAuthor_name = _webapiContext.Author.Where(author_fN => author_fN.FirstName == f_name).FirstOrDefault();
            return Ok(getAuthor_name);
        }

        [HttpPost]
        public IActionResult Insert_publisher_data()
        {
            try
            {
                Publisher publisher = new Publisher();
                publisher.PublisherName = "Test_one";
                publisher.State = "TS";
                publisher.City = "Hi-Tech City";
                publisher.Country = "India";
                _webapiContext.Publisher.Add(publisher);
                _webapiContext.SaveChanges();
                var Inserted_Publisher_data = _webapiContext.Publisher.Where(publisher_id => publisher_id.PubId == 43).FirstOrDefault();
                return Ok(Inserted_Publisher_data);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public IActionResult Updating_Pblshr()
        {
            try
            {
                Publisher publisher = _webapiContext.Publisher.Where(pblshr_id => pblshr_id.PubId == 45).FirstOrDefault();
                publisher.PublisherName = "Glen Maxwell";
                publisher.City = "Melbourne";
                publisher.State = "AP";
                publisher.Country = "Australia";
                _webapiContext.SaveChanges();
                var updated_data = _webapiContext.Publisher.Where(pblshrid => pblshrid.PubId == 45).FirstOrDefault();
                return Ok(updated_data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public IActionResult Deleting_publisher()
        {
            try
            {
                Publisher publisher = _webapiContext.Publisher.Where(pblshr_id => pblshr_id.PubId == 42).FirstOrDefault();
                _webapiContext.Publisher.Remove(publisher);
                _webapiContext.SaveChanges();
                var deleted_data = _webapiContext.Publisher.Where(pblshr_id => pblshr_id.PubId == 42).FirstOrDefault();
                return Ok(deleted_data);
            }
            catch
            {
                return StatusCode(500);
            }
        }


        #region doubt
        [HttpPost]
        public IActionResult Respone_data_Insert([FromBody] Publisher publisher_response)
        {
            try
            {
                Publisher publisher = new Publisher();
                publisher.PublisherName = publisher_response.PublisherName;
                publisher.State = publisher_response.State;
                publisher.City = publisher_response.City;
                publisher.Country = publisher_response.Country;
                _webapiContext.Publisher.Add(publisher);
                _webapiContext.SaveChanges();
                var result_= _webapiContext.Publisher.Where(pblshr_name => pblshr_name.PublisherName == "David").FirstOrDefault();
                return Ok(result_);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public IActionResult Updating_publisher_By_request([FromBody] Publisher publisher_response)
        {
            try
            {
                var publisher = _webapiContext.Publisher.Where(pblshr_id => pblshr_id.PubId == 44).FirstOrDefault();
                if(publisher == null)
                {
                    return StatusCode(404);
                }
                Publisher publisher1 = new Publisher();
                publisher1.PublisherName = publisher_response.PublisherName;
                publisher1.State = publisher_response.State;
                publisher1.City = publisher_response.City;
                publisher1.Country = publisher_response.Country;
                _webapiContext.Entry(publisher1).State = EntityState.Modified;
                _webapiContext.SaveChanges();
                var updated_pblshr_data = _webapiContext.Publisher.Where(pblshr_Id => pblshr_Id.PubId == 44).FirstOrDefault();
                return Ok(updated_pblshr_data);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("Delete/{PubId}")]
        public IActionResult Delete_pblshrbyrequest([FromRoute] int PubId)
        {
            try
            {
                var delte_request = _webapiContext.Publisher.Where(pblshr_id => pblshr_id.PubId == PubId);
                if(delte_request == null)
                {
                    return StatusCode(404);
                }
                _webapiContext.Entry(delte_request).State = EntityState.Deleted;
                _webapiContext.SaveChanges();
                return Ok(delte_request);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        #endregion 
    }
}
