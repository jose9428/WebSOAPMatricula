using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebSOAPMatricula.models
{
    public class DetalleMatricula
    {
        public int numSeccion { get; set; }
        public string aula { get; set; }
        [XmlIgnore]
        public TimeSpan horaInicio { get; set; }
        [XmlIgnore]
        public TimeSpan horaFin { get; set; }
        public string profesor { get; set; }
        public string curso { get; set; }
        public string diaSemana { get; set; }
        public string cHoraInicio
        {
            get { return horaInicio.ToString(); }
            set { horaInicio = TimeSpan.Parse(value); }
        }
        public string cHoraFin
        {
            get { return horaFin.ToString(); }
            set { horaFin = TimeSpan.Parse(value); }
        }
    }
}