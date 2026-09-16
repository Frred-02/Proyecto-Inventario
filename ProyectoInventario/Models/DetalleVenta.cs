using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventario.Models
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

    }
}