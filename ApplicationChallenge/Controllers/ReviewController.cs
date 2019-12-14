using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationChallenge.Models;

namespace ApplicationChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ReviewController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews.Include(m => m.Maker).Include(b => b.Bedrijf).ToListAsync();
        }


        // GET: api/Review/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IList<Review>>> GetReview(long id)
        {
            var review = await _context.Reviews.Include(r => r.Maker).Where(r => r.BedrijfId == id).Where(r => r.NaarBedrijf == true).ToListAsync();

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }
        [HttpGet("maker/{id}")]
        public async Task<ActionResult<IList<Review>>> GetReviewMakers(long id)
        {
            var review = await _context.Reviews.Include(r => r.Bedrijf).Where(r => r.MakerId == id).Where(r => r.NaarBedrijf == false).ToListAsync();

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }
        // GET: api/Review/5
        [HttpGet("getbyid{id}")]
        public async Task<ActionResult<Review>> GetReviewById(long id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // GET: api/Review/5
        [HttpGet("getReviewsBedrijf/{id}")]
        public async Task<ActionResult<double>> GetReviewBedrijf(long id)
        {
            var review = await _context.Reviews.Include(r => r.Maker).Include(r => r.Bedrijf).Where(r => r.BedrijfId == id).Where(r => r.NaarBedrijf == true).ToListAsync();

            double count = 0;
            double totaal = 0;

            foreach(var r in review)
            {
                totaal += r.Score;
                count++;
            }

            //var gemiddelde = new List<int>();

            //gemiddelde.Add(totaal / count);

            if (review == null)
            {
                return NotFound();
            }

            double t = totaal / count;

            return t;
        }        [HttpGet("getReviewsMaker/{id}")]
        public async Task<ActionResult<double>> GetReviewMaker(long id)
        {
            var review = await _context.Reviews.Include(r => r.Maker).Include(r => r.Bedrijf).Where(r => r.MakerId == id).Where(r => r.NaarBedrijf ==false).ToListAsync();

            double count = 0;
            double totaal = 0;

            foreach(var r in review)
            {
                totaal += r.Score;
                count++;
            }

            //var gemiddelde = new List<int>();

            //gemiddelde.Add(totaal / count);

            if (review == null)
            {
                return NotFound();
            }

            double t = totaal / count;

            return t;
        }

        // PUT: api/Review/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(long id, Review review)
        {
            

            review.Id = id;

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // PUT: api/Review/5
        [HttpPut("isreviewed/{id}")]
        public async Task<ActionResult<IList<Review>>> PutReviewed(long id, Review review)
        {
            var vreview = await _context.Reviews.Where(r => r.MakerId == review.MakerId).Where(r => r.BedrijfId == review.BedrijfId).Where(r => r.NaarBedrijf == true).ToListAsync();


            int count = 0;

            var reviewStatus = new List<Review>();

            foreach (var r in vreview)
            {
                reviewStatus.Add(new Review { Id = r.Id });
                count++;
            }

            if (count > 0)
            {
                return reviewStatus;
            }
            else
            {
                return NoContent();
            }

            return NoContent();
        }
        // PUT: api/Review/5
        [HttpPut("isreviewedMaker/{id}")]
        public async Task<ActionResult<IList<Review>>> PutReviewedMaker(long id, Review review)
        {
            var vreview = await _context.Reviews.Where(r => r.MakerId == review.MakerId).Where(r => r.BedrijfId == review.BedrijfId).Where(r => r.NaarBedrijf ==false).ToListAsync();


            int count = 0;

            var reviewStatus = new List<Review>();

            foreach (var r in vreview)
            {
                reviewStatus.Add(new Review { Id = r.Id });
                count++;
            }

            if (count > 0)
            {
                return reviewStatus;
            }
            else
            {
                return NoContent();
            }

            return NoContent();
        }
        // POST: api/Review
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteReview(long id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return review;
        }

        // DELETE: api/Review/makerid/5
        [HttpDelete("makerId/{makerId}")]
        public async Task<ActionResult<IEnumerable<Review>>> DeleteReviewWhereMakerId(int makerId)
        {
            var reviews = await _context.Reviews.Where(m => m.MakerId == makerId).ToListAsync();
            if (reviews == null)
            {
                return NotFound();
            }
            foreach (var review in reviews)
            {
                _context.Reviews.Remove(review);
            }
            await _context.SaveChangesAsync();

            return reviews;
        }

        private bool ReviewExists(long id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
