using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using WebSOAPMatricula.dao;
using WebSOAPMatricula.models;

namespace WebSOAPMatricula.services
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SeccionWS : System.Web.Services.WebService
    {

        [WebMethod]
        public List<Seccion> ListarTodos()
        {
            List<Seccion> lista = new SeccionDAO().ListarTodos();
            return lista;
        }


        [WebMethod]
        public List<Seccion> ListarSeccionesCursosDisponibles()
        {
            List<Seccion> lista = new SeccionDAO().ListarCursosDisponibles();
            return lista;
        }

        [WebMethod]
        public GenericApiResponse<object> Guardar(int codProfesor, int codCurso, string dia,
            string horaInicio , int capacidad , string aula)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            SeccionDAO seccionDao = new SeccionDAO();

            Seccion obj = new Seccion()
            {
                aula = aula,
                diaSemana = dia,
                cHoraInicio = horaInicio,
                capacidad = capacidad, 
                profesor = new Profesor()
                {
                    codProfesor = codProfesor
                },
                curso = new Curso()
                {
                    codCurso = codCurso
                }

            };

            api.msg = seccionDao.Guardar(obj);

            return api;
        }


    }
}
