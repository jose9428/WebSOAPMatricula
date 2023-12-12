using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSOAPMatricula.models
{
    public class Matricula
    {
        public int numMatricula { get; set; }
        public DateTime fecha { get; set; }
        public int totalHoras { get; set; }
        public decimal costo { get; set; }
        public string estado { get; set; }
        public List<DetalleMatricula> detalles { get; set; }
        public Estudiante estudiante { get; set; }

        public Matricula()
        {
            detalles = new List<DetalleMatricula>();
        }
    }
    public class MatriculaPagos
    {
        public int numMatricula { get; set; }
        public DateTime fecha { get; set; }
        public decimal costo { get; set; }
    }

}