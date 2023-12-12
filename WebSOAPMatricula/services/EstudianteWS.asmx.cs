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
      public class EstudianteWS : System.Web.Services.WebService
    {

        [WebMethod]
        public List<Estudiante> ListarTodos()
        {
            List<Estudiante> lista = new EstudianteDAO().ListarTodos();
            return lista;
        }

        [WebMethod]
        public GenericApiResponse<object> Guardar(int codCarrera, string nombres, 
            string apellidos,string correo, string contraseña , string apoderado)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            EstudianteDAO estDao = new EstudianteDAO();

            Estudiante obj = new Estudiante()
            {
                apoderado = apoderado,
                usuario = new Usuario()
                {
                    nombres = nombres,
                    apellidos = apellidos,
                    correo = correo,
                    contraseña = contraseña
                },
                carrera = new Carrera()
                {
                    codCarrera = codCarrera
                }
            };

            api.msg = estDao.Mantenimiento(obj , 1);

            return api;
        }

        [WebMethod]
        public GenericApiResponse<object> Actualizar(int codEstudiante , int codCarrera, string nombres,
          string apellidos, string correo, string contraseña, string apoderado)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            EstudianteDAO estDao = new EstudianteDAO();

            Estudiante obj = new Estudiante()
            {
                codEstudiante = codEstudiante,
                apoderado = apoderado,
                usuario = new Usuario()
                {
                    nombres = nombres,
                    apellidos = apellidos,
                    correo = correo,
                    contraseña = contraseña
                },
                carrera = new Carrera()
                {
                    codCarrera = codCarrera
                }
            };

            api.msg = estDao.Mantenimiento(obj, 2);

            return api;
        }

        [WebMethod]
        public GenericApiResponse<object> Eliminar(int codEstudiante)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            EstudianteDAO estDao = new EstudianteDAO();

            Estudiante obj = new Estudiante();
            obj.codEstudiante = codEstudiante;  
         
            api.msg = estDao.Mantenimiento(obj, 3);

            return api;
        }
    }
}
