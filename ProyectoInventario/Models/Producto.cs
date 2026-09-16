using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventario.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreProducto { get; set; }
        public int IdCategoria { get; set; }
    }
}