using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Wep_Api_Demo.Models;

namespace Wep_Api_Demo.Controllers
{
    public class BeersController : ApiController
    {
        private BeerDatabaseEntities db = new BeerDatabaseEntities();

        // GET: api/Beers/Brown Ale
        public List<Beer> GetBeer(string name = null)
        {
            List<Beer> data = new List<Beer>();
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    data = db.Beers.ToList();
                }
                else
                {
                    data = db.Beers.Where(u => u.Name.Contains(name)).ToList();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return data;
        }
        // PUT: api/Beers/
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBeer(Beer beer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Beer existingBeer = db.Beers.Find(beer.Id);
            if (existingBeer == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                existingBeer.Rating=
                beer.Rating = (existingBeer.Rating + beer.Rating) / 2;
            }

            db.Entry(existingBeer).State = EntityState.Detached;
            db.Entry(beer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerExists(beer.Id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.ExpectationFailed);
                }
            }
            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Beers
        [ResponseType(typeof(void))]
        public IHttpActionResult PostBeer(Beer beer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                db.Beers.Add(beer);
                db.SaveChanges();

                return StatusCode(HttpStatusCode.OK);
            }
            catch (System.Exception)
            {
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }
        }

        // DELETE: api/Beers/5
        [ResponseType(typeof(Beer))]
        public IHttpActionResult DeleteBeer(int id)
        {
            try
            {
                Beer beer = db.Beers.Find(id);
                if (beer == null)
                {
                    return NotFound();
                }
                db.Beers.Remove(beer);
                db.SaveChanges();
                return StatusCode(HttpStatusCode.OK);
            }
            catch (System.Exception)
            {
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeerExists(int id)
        {
            try
            {
                return db.Beers.Count(e => e.Id == id) > 0;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}