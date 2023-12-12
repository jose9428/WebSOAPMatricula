using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSOAPMatricula.models
{
    public class ReporteMatricula
    {
        public string estudiante { get; set; }
        public List<MatriculaPagos> pagos { get; set; }

        public ReporteMatricula()
        {
            pagos = new List<MatriculaPagos>();
        }
    }
}