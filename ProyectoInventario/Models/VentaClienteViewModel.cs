using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoInventario.Models
{
    public class VentaClienteViewModel
    {
        public List<Venta> Ventas { get; set; }
        public List<Cliente> Clientes { get; set; }

    }
}