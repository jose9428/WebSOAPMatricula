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
    public class RolWS : System.Web.Services.WebService
    {
        [WebMethod]
        public List<Rol> ListarTodos()
        {
            List<Rol> lista = new RolDAO().ListarTodos();
            return lista;
        }
    }
}
