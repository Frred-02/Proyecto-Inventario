using ProyectoInventario.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ProyectoInventario.Models;
using System.Web.Mvc;

namespace ProyectoInventario.Controllers
{
    public class VentaController : Controller
    {

        string cadena = "Data Source=.\\SQLEXPRESS;Initial Catalog=Prueba;User ID=sa;Password=1234;";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgregarVenta(int? idCliente)
        {
            ViewBag.IdClienteSeleccionado = idCliente;

            return View();
        }

        [HttpPost]
        public ActionResult GuardarVenta(Venta v)
        {

            SqlConnection cn = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("sp_AgregarVenta", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IdCliente", v.IdCliente);
            cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            return RedirectToAction("ListaVenta");


        }


        public ActionResult ListaVenta()
        {
            List<Venta> listaventas = new List<Venta>();
            List<Cliente> listaclientes = new List<Cliente>();

            SqlConnection cn = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("sp_ListaVenta", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Venta venta = new Venta();

                venta.IdVenta = Convert.ToInt32(dr["IdVenta"]);
                venta.NombreCliente = dr["NombreCliente"].ToString();
                venta.ApellidoCliente = dr["ApellidoCliente"].ToString();
                venta.Fecha = Convert.ToDateTime(dr["Fecha"]);
                venta.Estado = dr["Estado"] != DBNull.Value ? dr["Estado"].ToString() : "Activo";
                venta.TotalVenta = Convert.ToDecimal(dr["TotalVenta"]);
                listaventas.Add(venta);

            }

            cn.Close();

            VentaClienteViewModel modelcompleto = new VentaClienteViewModel
            {
                Ventas = listaventas,
                Clientes = listaclientes
            };

            return View(modelcompleto);
        }


        public ActionResult BuscaCliente(string NombreCliente)
        {

            List<Cliente> listaclientefiltrado = new List<Cliente>();
            List<Venta> listaventas = new List<Venta>();


            SqlConnection cn = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("sp_BuscarCliente", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NombreCliente", NombreCliente);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                listaclientefiltrado.Add(new Cliente
                {

                    IdCliente = Convert.ToInt32(dr["IdCliente"]),
                    NombreCliente = dr["NombreCliente"].ToString(),
                    ApellidoCliente = dr["ApellidoCliente"].ToString()



                });


            }


            cn.Close();


            VentaClienteViewModel modelcompleto = new VentaClienteViewModel
            {
                Clientes = listaclientefiltrado, 
            };
            return View("ListaVenta", modelcompleto);





        }


        [HttpPost]
        public ActionResult AgregarProducto(DetalleVentaViewModel postModel)
        {

            decimal precioProducto = 0;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

             
                string qPrecio = "SELECT Precio FROM Producto WHERE IdProducto = @IdProducto";
                using (SqlCommand cmdPrecio = new SqlCommand(qPrecio, cn))
                {
                    cmdPrecio.Parameters.AddWithValue("@IdProducto", postModel.IdProducto);
                    precioProducto = Convert.ToDecimal(cmdPrecio.ExecuteScalar());
                }

                string qInsert = @"INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, Precio) 
                                   VALUES (@IdVenta, @IdProducto, @Cantidad, @Precio)";
                using (SqlCommand cmdInsert = new SqlCommand(qInsert, cn))
                {
                    cmdInsert.Parameters.AddWithValue("@IdVenta", postModel.IdVenta);
                    cmdInsert.Parameters.AddWithValue("@IdProducto", postModel.IdProducto);
                    cmdInsert.Parameters.AddWithValue("@Cantidad", postModel.Cantidad);
                    cmdInsert.Parameters.AddWithValue("@Precio", precioProducto); 

                    cmdInsert.ExecuteNonQuery();
                }
            }

            return RedirectToAction("DetalleVenta", new { idVenta = postModel.IdVenta });
        }


        public ActionResult AnularVenta(int idVenta)
        {

            using (SqlConnection cm = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_AnularVenta", cm);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdVenta", idVenta);


                cm.Open();
                cmd.ExecuteNonQuery();

            }

        
            TempData["Mensaje"] = "La venta #" + idVenta + " fue anulada correctamente.";

            return RedirectToAction("ListaVenta");

        }

    }
}