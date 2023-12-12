using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using WebSOAPMatricula.dao;
using WebSOAPMatricula.models;

namespace WebSOAPMatricula.services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class CursoWS : WebService
    {
        [WebMethod]
        public List<Curso> ListarTodos() { 
            List<Curso> lista = new CursoDAO().ListarTodos(); 
            return lista;
        }

        [WebMethod]
        public GenericApiResponse<object> Guardar(string nombre , int horas , int creditos)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            CursoDAO cursoDao = new CursoDAO();

            Curso obj = new Curso()
            {
                nombre = nombre,
                horas = horas,
                creditos = creditos
            };

            if (cursoDao.ExisteCurso(obj) == false)
            {
                api.msg = cursoDao.Guardar(obj);
            }
            else
            {
                api.msg = "El curso "+ obj.nombre+" ya se encuentra registrado!";
            }

            return api;
        }

        [WebMethod]
        public GenericApiResponse<object> Actualizar(int codigo,string nombre, int horas, int creditos)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            CursoDAO cursoDao = new CursoDAO();

            Curso obj = new Curso()
            {
                codCurso = codigo,  
                nombre = nombre,
                horas = horas,
                creditos = creditos
            };
            if (cursoDao.BuscarPorId(codigo) != null)
            {
                if (cursoDao.ExisteCurso(obj) == false)
                {
                    api.msg = cursoDao.Actualizar(obj);
                }
                else
                {
                    api.msg = "El curso " + obj.nombre + " ya se encuentra registrado!";
                }
            }
            else
            {
                api.msg = "No existe curso con el codigo "+ codigo;
                api.status = 404;
            }

            return api;
        }

        [WebMethod]
        public GenericApiResponse<object> Eliminar(int codigo)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            CursoDAO cursoDao = new CursoDAO();

            if (cursoDao.BuscarPorId(codigo) != null)
            {
                api.msg = cursoDao.Eliminar(codigo);

                if (!api.msg.Equals("OK"))
                {
                    api.status = 500;
                }
            }
            else
            {
                api.msg = "No existe curso con el codigo " + codigo;
                api.status = 404;
            }

            return api;
        }
    }
}
