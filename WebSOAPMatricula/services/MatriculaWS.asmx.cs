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
    public class MatriculaWS : System.Web.Services.WebService
    {
        [WebMethod]
        public List<Matricula> ReporteDeMatriculasRegistradas()
        {
            List<Matricula> lista = new MatriculaDAO().ListarTodos();
            return lista;
        }

        [WebMethod]
        public GenericApiResponse<Matricula> ConsultarMatricula(int numeroMatricula)
        {
            MatriculaDAO matrDao = new MatriculaDAO();
            GenericApiResponse<Matricula> api = new GenericApiResponse<Matricula>();
            api.data = matrDao.BuscarPorId(numeroMatricula);

            if (api.data == null)
            {
                api.status = 404;
                api.msg = "No se encontró matricula con el numero "+numeroMatricula+".";
            }

            return api;
        }

        [WebMethod]
        public GenericApiResponse<object> RegistrarMatricula(int codEstudiante, string strSecciones)
        {
            GenericApiResponse<object> api = new GenericApiResponse<object>();
            MatriculaDAO matriculaDao = new MatriculaDAO();

            string[] arraySecciones = strSecciones.Split(';');
            int cantSecciones = 0;

            XElement detallesXML = new XElement("Tabla");
            foreach (string item in arraySecciones)
            {
                if (!(item.Equals("") || item.Equals(";")))
                {
                    detallesXML.Add(new XElement("Item",
                    new XElement("numSeccion", item)
                    ));
                    cantSecciones++;
                }
            }

            if (arraySecciones.Length > 0 && cantSecciones > 0)
            {

                api.msg = matriculaDao.Guardar(codEstudiante, detallesXML);
            }
            else
            {
                api.msg = "Se debe ingresar secciones separado por un punto y coma.";
            }

            return api;
        }

        [WebMethod]
        public GenericApiResponse<ReporteMatricula>consultarPagosPorEstudiante(int codEstudiante)
        {
            GenericApiResponse<ReporteMatricula> api = new GenericApiResponse<ReporteMatricula>();
            EstudianteDAO estDao = new EstudianteDAO();
            MatriculaDAO matriculaDao = new MatriculaDAO();

            Estudiante objEst = estDao.BuscarPorId(codEstudiante);

            if (objEst != null)
            {
                List<MatriculaPagos> listPagos = matriculaDao.BuscarPagosPorEstudiante(codEstudiante);
                ReporteMatricula report = new ReporteMatricula();
                
                report.estudiante = objEst.usuario.nombres + " " + objEst.usuario.apellidos;
                report.pagos = listPagos;
                api.data = report;

                if (listPagos.Count == 0)
                {
                    api.msg = "El estudiante no cuenta con pagos registrados!";
                }
            }
            else
            {
                api.msg = "No se encontró estudiante con el codigo " + codEstudiante;
            }
            return api;
        }
    }
}
