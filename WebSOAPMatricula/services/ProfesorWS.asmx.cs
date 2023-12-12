using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebSOAPMatricula.dao;
using WebSOAPMatricula.models;

namespace WebSOAPMatricula.services
{
    /// <summary>
    /// Descripción breve de ProfesorWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ProfesorWS : System.Web.Services.WebService
    {
        [WebMethod]
        public List<Profesor> ListarTodos()
        {
            List<Profesor> lista = new ProfesorDAO().ListarTodos();
            return lista;
        }

        [WebMethod]
        public GenericApiResponse<object> Guardar(string nombres,
            string apellidos, string correo, string contraseña, string direccion , string telefono)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            ProfesorDAO profDao = new ProfesorDAO();

            Profesor obj = new Profesor()
            {
                telefono = telefono,
                direccion = direccion,
                usuario = new Usuario()
                {
                    nombres = nombres,
                    apellidos = apellidos,
                    correo = correo,
                    contraseña = contraseña
                }
            };

            api.msg = profDao.Mantenimiento(obj, 1);

            return api;
        }

        [WebMethod]
        public GenericApiResponse<object> Actualizar(int codProfesor, string nombres,
            string apellidos, string correo, string contraseña, string direccion, string telefono)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            ProfesorDAO profDao = new ProfesorDAO();

            Profesor obj = new Profesor()
            {
                codProfesor = codProfesor,
                telefono = telefono,
                direccion = direccion,
                usuario = new Usuario()
                {
                    nombres = nombres,
                    apellidos = apellidos,
                    correo = correo,
                    contraseña = contraseña
                }
            };

            api.msg = profDao.Mantenimiento(obj, 2);

            return api;
        }

        [WebMethod]
        public GenericApiResponse<object> Eliminar(int codProfesor)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            ProfesorDAO profDao = new ProfesorDAO();

            Profesor obj = new Profesor();
            obj.codProfesor = codProfesor;

            api.msg = profDao.Mantenimiento(obj, 3);

            return api;
        }
    }
}
