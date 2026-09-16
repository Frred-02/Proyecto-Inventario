using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventario.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal TotalVenta { get; set; }

    }
}