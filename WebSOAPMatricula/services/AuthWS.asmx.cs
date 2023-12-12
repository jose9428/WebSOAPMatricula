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
    public class AuthWS : WebService
    {

        [WebMethod]
        public GenericApiResponse<Usuario> login(string correo , string contrasennia)
        {
            AuthDAO authDao = new AuthDAO();
            GenericApiResponse<Usuario> api = new GenericApiResponse<Usuario>();
            api.data = authDao.Login(correo, contrasennia); 

            if (api.data == null)
            {
                api.status = 404;
                api.msg = "Correo y/o contraseña incorrecto.";
            }

            return api;
        }
    }
}
