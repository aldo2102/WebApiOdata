using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;
using WebApi2.Models;
using System.Collections;
using WebApi2.Models.Formulario;

namespace WebApi2.Controllers
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        private SIGAEntities db = new SIGAEntities();
        private EPEDIDOSEntities1 db2 = new EPEDIDOSEntities1();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod()]
        public string GetGrupoUsuario()
        {
            var query = (from g3 in db.Grupo
                         select new { g3 });

            var pedidos = new List<pedido>(db2.pedido);
            var formularios = new List<formulario>(db2.formulario);
            var grupos = new List<Grupo>(db.Grupo);
            var roteadores = new List<roteador>(db2.roteador);

            /*
             DbSet<Grupo> grupos1 = db.Set<Grupo>();
             var GrupoG = grupos1.Select(g => g);
             * */
            var list = (from p in pedidos
                        join f in formularios on p.Idn_Formulario equals f.Idn_Formulario
                        join g1 in grupos on f.Idn_Grupo equals g1.IDN_GRUPO
                        join r in roteadores on f.Idn_Formulario equals r.Idn_Formulario 
                        join g2 in grupos on r.Idn_Grupo equals g2.IDN_GRUPO

                        select p).Take(50);

            JavaScriptSerializer js = new JavaScriptSerializer();

            //Converte e retorna os dados em JSON
            return js.Serialize(list.ToList());

        }
    }
}
