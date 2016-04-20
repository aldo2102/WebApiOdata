
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    /*
    To add a route for this controller, merge these statements into the Register method of the WebApiConfig class. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using WebApi2.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<pedido>("Pedido");
    builder.EntitySet<evento>("evento"); 
    builder.EntitySet<formulario>("formulario"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PedidoController : ODataController
    {
        private EPEDIDOSEntities1 db = new EPEDIDOSEntities1();

        // GET odata/Pedido
        [EnableQuery]
        [ODataRoute]
        public IQueryable<pedido> GetPedido()
        {
            return db.pedido;
        }

        //GET odata/Pedido(5)
        [EnableQuery]
        [ODataRoute]
        public SingleResult<pedido> Getpedido([FromODataUri] int key)
        {
            return SingleResult.Create(db.pedido.Where(pedido => pedido.Idn_Pedido == key));
        }
        
        // PUT odata/Pedido(5)
        public IHttpActionResult Put([FromODataUri] int key, pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != pedido.Idn_Pedido)
            {
                return BadRequest();
            }

            db.Entry(pedido).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pedidoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(pedido);
        }

        // POST odata/Pedido
        public IHttpActionResult Post(pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.pedido.Add(pedido);
            db.SaveChanges();

            return Created(pedido);
        }

        // PATCH odata/Pedido(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<pedido> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pedido pedido = db.pedido.Find(key);
            if (pedido == null)
            {
                return NotFound();
            }

            patch.Patch(pedido);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pedidoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(pedido);
        }

        // DELETE odata/Pedido(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            pedido pedido = db.pedido.Find(key);
            if (pedido == null)
            {
                return NotFound();
            }

            db.pedido.Remove(pedido);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Pedido(5)/evento
        [EnableQuery]
        [ODataRoute]
        public IQueryable<evento> Getevento([FromODataUri] int key)
        {
            return db.pedido.Where(m => m.Idn_Pedido == key).SelectMany(m => m.evento);
        }

        /* GET odata/Pedido(5)/evento
        [EnableQuery]
        [ODataRoute]
        public IQueryable<evento> Getevento()
        {
  
            string sql = string.Format("select top 50 * from dbo.pedido p inner join dbo.evento e on p.Idn_Pedido = e.Idn_Evento");
            //return db.pedido.Join(evento, p=>p.Idn_Pedido, e=>e.Idn_Evento,(a,b)=>a.Idn_Pedido==b.Idn_Evento);
            return db.ExecuteCommand(sql);
            return db.pedido.Where(m => m.Idn_Pedido == key).SelectMany(m => m.evento);
 
        }
        */
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool pedidoExists(int key)
        {
            return db.pedido.Count(e => e.Idn_Pedido == key) > 0;
        }
    }
}
