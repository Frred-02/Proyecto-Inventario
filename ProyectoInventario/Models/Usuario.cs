using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventario.Models
{
    public class Usuario
    {

        public int IdUsuario { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string NombreRol { get; set; }
        public int IdRol { get; set; }
        public bool Activo { get; set; }


    }
}