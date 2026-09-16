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
    public class CategoriaController : Controller
    {
        string cadena = "Data Source=.\\SQLEXPRESS;Initial Catalog= Prueba;User ID=sa;Password=1234;";
        public ActionResult Vcategoria()
        {
            return RedirectToAction("ListaCategoria");
        }



        public ActionResult AgregarCategorias(Categoria c)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_AgregarCategoria", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NombreCategoria", c.NombreCategoria);

                cn.Open();
                cmd.ExecuteNonQuery();


            }

            return RedirectToAction("ListaCategoria");

        }

        public ActionResult ActualizarCategoria(Categoria c)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_ActulizarCategoria", cn);

                cmd.Parameters.AddWithValue("@IdCategoria", c.IdCategoria);
                cmd.Parameters.AddWithValue("@NombreCategoria", c.NombreCategoria);


                cn.Open();

                cmd.ExecuteReader();
                cn.Close();


            }
            return RedirectToAction("ListaCategoria");


        }

        public ActionResult ListaCategoria()
        {

            var lista = ListaTablaCategoria();

            return View(lista);

        }
        private List<Categoria> ListaTablaCategoria()
        {

            List<Categoria> lista = new List<Categoria>();

            using (SqlConnection cn = new SqlConnection(cadena))

            {
                SqlCommand cmd = new SqlCommand("sp_ListaCategoria", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Categoria
                        {
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                            NombreCategoria = dr["NombreCategoria"].ToString()
                        });

                    }

                }
            }

            return lista;
        }


        public ActionResult EliminarCategoria(int id)
        {
            using (SqlConnection cn = new SqlConnection(cadena))

            {
                SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategoria", id);
                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    TempData["error"] = "No se puede eliminar Categoria" + ex.Message;

                }

            }

            return RedirectToAction("ListaCategoria");

        }


        [HttpPost]

        public JsonResult EditarCategoria(Categoria c)
        {

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_EditarCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategoria", c.IdCategoria);
                cmd.Parameters.AddWithValue("@NombreCategoria", c.NombreCategoria);

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(true);

        }

    }
}