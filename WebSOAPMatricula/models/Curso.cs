using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSOAPMatricula.models
{
    [Serializable]
    public class Curso
    {
        public int codCurso { get; set; }
        public string nombre { get; set; }
        public int creditos { get; set; }
        public int horas { get; set; }
    }
}