using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebSOAPMatricula.models
{
    public class Seccion
    {
        public int numSeccion { get; set; }
        [XmlIgnore]
        public TimeSpan horaInicio { get; set; }
        [XmlIgnore]
        public TimeSpan horaFin { get; set; }
        public int capacidad { get; set; }
        public string diaSemana { get; set; }
        public int vacantesDisponibles { get; set; }
        public string aula { get; set; }

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

        public Curso curso { get; set; }
        public Profesor profesor { get; set; }

        public Seccion()
        {
            curso = new Curso();
            profesor = new Profesor();
        }
    }
}