using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebSOAPMatricula.models
{
    [Serializable]
    public class Usuario
    {
        public int codUsuario { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string correo { get; set; }
        [XmlIgnore]
        public string contraseña { get; set; }
        public DateTime fechaRegistro { get; set; }
        public Rol rol { get; set; }
    }
}