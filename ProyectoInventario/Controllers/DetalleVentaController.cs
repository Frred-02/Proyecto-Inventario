using ProyectoInventario.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoInventario.Controllers
{
    public class DetalleVentaController : Controller
    {
        private string cadena = "Data Source=.\\SQLEXPRESS;Initial Catalog=Prueba;User ID=sa;Password=1234;";


        public ActionResult ListaDetalleVenta(int idVenta)
        {
            DetalleVentaViewModel model = new DetalleVentaViewModel();
            model.IdVenta = idVenta;
            model.ListaProductos = new List<SelectListItem>();
            model.ProductosAgregados = new List<DetalleVentaPasarela>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();


                using (SqlCommand cmd = new SqlCommand("sp_listaProductos", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            model.ListaProductos.Add(new SelectListItem
                            {
                                Value = dr["IdProducto"].ToString(),
                                Text = dr["NombreProducto"].ToString()
                            });
                        }
                    }
                }


                using (SqlCommand cmd2 = new SqlCommand("sp_ListaDetalleVenta", cn))
                {
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@IdVenta", idVenta);

                    using (SqlDataReader dr2 = cmd2.ExecuteReader())
                    {
                        while (dr2.Read())
                        {
                            model.ProductosAgregados.Add(new DetalleVentaPasarela
                            {
                                IdDetalleVenta = Convert.ToInt32(dr2["IdDetalleVenta"]),
                                NombreProducto = dr2["NombreProducto"].ToString(),
                                Cantidad = Convert.ToInt32(dr2["Cantidad"]),
                                Precio = Convert.ToDecimal(dr2["Precio"])
                            });
                        }
                    }
                }
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult AgregarDetalleVenta(DetalleVentaViewModel v)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AgregarDetalleVenta", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdVenta", v.IdVenta);
                    cmd.Parameters.AddWithValue("@IdProducto", v.IdProducto);
                    cmd.Parameters.AddWithValue("@Cantidad", v.Cantidad);
                    cmd.Parameters.AddWithValue("@Precio", v.Precio);


                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }


            return RedirectToAction("ListaDetalleVenta", new { idVenta = v.IdVenta });
        }

    }
}