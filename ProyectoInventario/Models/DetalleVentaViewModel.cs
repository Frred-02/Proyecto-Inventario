using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoInventario.Models
{
    public class DetalleVentaViewModel
    {
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

     
        public List<SelectListItem> ListaProductos { get; set; }

      
        public List<DetalleVentaPasarela> ProductosAgregados { get; set; }
    }

    public class DetalleVentaPasarela
    {
        public int IdDetalleVenta { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Total => Cantidad * Precio;
    }
}