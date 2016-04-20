using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using WebApi2.Models.Formulario;
using WebApi2.Models;
using System.Data.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Caching;
using System.Data.SqlClient;
using System.Data.Entity;

namespace WebApi2.Controllers
{
    public class GrupoController : ODataController
    {
        private SIGAEntities db = new SIGAEntities();
        private EPEDIDOSEntities1 db2 = new EPEDIDOSEntities1();

        // GET odata/Grupo
        [EnableQuery]
        [ODataRoute]
        public IQueryable<Grupo> GetGrupo()
        {
            return db.Grupo;
        }

        // GET odata/Grupo(5)
        [EnableQuery]
        [ODataRoute]
        public SingleResult<Grupo> GetGrupo([FromODataUri] long key)
        {
            return SingleResult.Create(db.Grupo.Where(grupo => grupo.IDN_GRUPO == key));
        }

        // PUT odata/Grupo(5)
        public IHttpActionResult Put([FromODataUri] long key, Grupo grupo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != grupo.IDN_GRUPO)
            {
                return BadRequest();
            }

            db.Entry(grupo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(grupo);
        }

        // POST odata/Grupo
        public IHttpActionResult Post(Grupo grupo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Grupo.Add(grupo);
            db.SaveChanges();

            return Created(grupo);
        }

        // PATCH odata/Grupo(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<Grupo> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Grupo grupo = db.Grupo.Find(key);
            if (grupo == null)
            {
                return NotFound();
            }

            patch.Patch(grupo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(grupo);
        }

        // DELETE odata/Grupo(5)
        public IHttpActionResult Delete([FromODataUri] long key)
        {
            Grupo grupo = db.Grupo.Find(key);
            if (grupo == null)
            {
                return NotFound();
            }

            db.Grupo.Remove(grupo);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        /* GET odata/Grupo(5)/GrupoUsuario
        [EnableQuery]
        [ODataRoute]
        public IQueryable<GrupoUsuario> GetGrupoUsuario([FromODataUri] long key)
        {
            return db.Grupo.Where(m => m.IDN_GRUPO == key).SelectMany(m => m.GrupoUsuario);
        }
        */
        // GET odata/Grupo(39)/GrupoUsuario
        [EnableQuery]
        [ODataRoute]
        public IEnumerable<pedido> GetGrupoUsuario()
        {
            var query = (from g3 in db.Grupo
                         select new { g3 });
            var pedidos = new List<pedido>(db2.pedido);
            var grupos = new List<Grupo>(db.Grupo);
            var formularios = new List<formulario>(db2.formulario);
            var roteadores = new List<roteador>(db2.roteador);

            /*
             DbSet<Grupo> grupos1 = db.Set<Grupo>();
             var GrupoG = grupos1.Select(g => g);
             * */
            var list = (from p in pedidos
                        join f in formularios on p.Idn_Formulario equals f.Idn_Formulario
                        join g1 in grupos on f.Idn_Grupo equals g1.IDN_GRUPO
                        join r in roteadores on f.Idn_Formulario equals r.Idn_Formulario into joined
                        //join g2 in db.Grupo on r.Idn_Grupo equals g2.IDN_GRUPO

                        select p).Take(50);
           
            return list.ToList();

        }

        [HttpGet]
        [ODataRoute("GetSalesTaxRate(PostalCode={postalCode})")]
        public IEnumerable GetSalesTaxRate([FromODataUri] int postalCode)
        {
            var list =
            (from p in db2.pedido
             join f in db2.formulario on p.Idn_Formulario equals f.Idn_Formulario
             from g2 in
                 (from g1 in db.Grupo
                  group g1 by g1.IDN_GRUPO)
             group g2 by f.Idn_Grupo
            ).Take(50);

            return list;
        }



        // GET odata/Grupo(5)/Perfil
        [EnableQuery]
        [ODataRoute]
        public IQueryable<Perfil> GetPerfil([FromODataUri] long key)
        {
            return db.Grupo.Where(m => m.IDN_GRUPO == key).SelectMany(m => m.Perfil);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GrupoExists(long key)
        {
            return db.Grupo.Count(e => e.IDN_GRUPO == key) > 0;
        }
    }
}