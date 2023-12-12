using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSOAPMatricula.models
{
    public class Profesor
    {
        public int codProfesor { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public Usuario usuario { get; set; }

        public Profesor()
        {
           
            usuario = new Usuario();
        }
    }
}