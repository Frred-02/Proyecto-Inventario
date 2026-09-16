using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventario.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string Telefono { get; set; }
    }
}