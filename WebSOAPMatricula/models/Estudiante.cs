using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSOAPMatricula.models
{
    public class Estudiante
    {
        public int codEstudiante { get; set; }
        public string apoderado { get; set; }
        public Usuario usuario { get; set; }
        public Carrera carrera { get; set; }

        public Estudiante()
        {
            carrera = new Carrera();
            usuario = new Usuario();
        }
    }
}