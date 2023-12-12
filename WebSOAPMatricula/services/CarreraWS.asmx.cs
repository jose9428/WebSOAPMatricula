using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebSOAPMatricula.dao;
using WebSOAPMatricula.models;

namespace WebSOAPMatricula.services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CarreraWS : System.Web.Services.WebService
    {
        [WebMethod]
        public List<Carrera> ListarTodos()
        {
            List<Carrera> lista = new CarreraDAO().ListarTodos();
            return lista;
        }
    }
}
