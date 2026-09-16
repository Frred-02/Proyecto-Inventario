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
    public class ProductoController : Controller
    {
        // GET: Producto
        string cadena = "Data Source=.\\SQLEXPRESS;Initial Catalog=Prueba;User ID=sa;Password=1234;";

        public ActionResult ListaProct()
        {

            return View();
        }

        public ActionResult AgregarProducto(Producto p)
        {

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_AgregrarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreProducto", p.NombreProducto);
                cmd.Parameters.AddWithValue("IdCategoria", p.IdCategoria);
                cn.Open();
                cmd.ExecuteNonQuery();

            }


            return RedirectToAction("ListaProducto");


        }

        private List<SelectListItem> ComboCategoria()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_ListaCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        lista.Add(new SelectListItem
                        {
                            Value = dr["IdCategoria"].ToString(),
                            Text = dr["NombreCategoria"].ToString()
                        });

                    }

                }

            }
            return lista;

        }


        public ActionResult EditarProducto(Producto P)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_EditarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdProducto", P.IdProducto);
                cmd.Parameters.AddWithValue("@IdCategoria", P.IdCategoria);
                cmd.Parameters.AddWithValue("@NombreProducto", P.NombreProducto);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();


            }

            return RedirectToAction("ListaProducto");
        }
        private List<Producto> ListaTablaProducto()
        {

            List<Producto> lista = new List<Producto>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_listaProductos", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        lista.Add(new Producto
                        {

                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            NombreCategoria = dr["NombreCategoria"].ToString(),
                            NombreProducto = dr["NombreProducto"].ToString()

                        });

                    }

                }

            }

            return lista;

        }

        public ActionResult ListaProducto()
        {

            ViewBag.ComboCategoria = ComboCategoria();
            var listaProducto = ListaTablaProducto();

            return View(listaProducto);
        }


        public ActionResult EliminarProducto(int id)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdProducto", id);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    TempData["error"] = "No se puede eliminar Producto" + ex.Message;

                }

            }

            return RedirectToAction("ListaProducto");

        }
    }
}