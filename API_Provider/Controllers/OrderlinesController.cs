using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using API_Provider.Context;
using API_Provider.Models;

namespace API_Provider.Controllers
{
    [Authorize (Roles ="Administrator")]
    public class OrderlinesController : ApiController
    {
        private MyContext db = new MyContext();

        // GET: api/Orderlines
        public IQueryable<Orderline> GetOrderlines()
        {
            return db.Orderlines;
        }

        // GET: api/Orderlines/5
        [ResponseType(typeof(Orderline))]
        public async Task<IHttpActionResult> GetOrderline(int id)
        {
            Orderline orderline = await db.Orderlines.FindAsync(id);
            if (orderline == null)
            {
                return NotFound();
            }

            return Ok(orderline);
        }

        // PUT: api/Orderlines/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrderline(int id, Orderline orderline)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderline.Id)
            {
                return BadRequest();
            }

            db.Entry(orderline).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderlineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orderlines
        [ResponseType(typeof(Orderline))]
        public async Task<IHttpActionResult> PostOrderline(Orderline orderline)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orderlines.Add(orderline);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = orderline.Id }, orderline);
        }

        // DELETE: api/Orderlines/5
        [ResponseType(typeof(Orderline))]
        public async Task<IHttpActionResult> DeleteOrderline(int id)
        {
            Orderline orderline = await db.Orderlines.FindAsync(id);
            if (orderline == null)
            {
                return NotFound();
            }

            db.Orderlines.Remove(orderline);
            await db.SaveChangesAsync();

            return Ok(orderline);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderlineExists(int id)
        {
            return db.Orderlines.Count(e => e.Id == id) > 0;
        }
    }
}