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
    public class ClienteController : Controller
    {
        string cadena = "Data Source=.\\SQLEXPRESS;Initial Catalog= Prueba;User ID=sa;Password=1234;";
      


        public ActionResult AgregarCliente(Cliente c)
        {

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_AgregarCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@NombreCliente", c.NombreCliente);
                cmd.Parameters.AddWithValue("@ApellidoCliente", c.ApellidoCliente);
                cmd.Parameters.AddWithValue("@Telefono", c.Telefono);
                cn.Open();
                cmd.ExecuteNonQuery();

                return RedirectToAction("ListaCliente");
            }


        }


        public ActionResult ListaCliente()
        {

            var lista = ListaTablaCliente();

            return View(lista);
        }


        private List<Cliente> ListaTablaCliente()
        {

            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection cn = new SqlConnection(cadena))

            {
                SqlCommand cmd = new SqlCommand("sp_ListaCliente", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cliente
                        {
                            IdCliente = Convert.ToInt32(dr["IdCliente"]),
                            NombreCliente = dr["NombreCliente"].ToString(),
                            ApellidoCliente = dr["ApellidoCliente"].ToString(),
                            Telefono = dr["Telefono"].ToString()


                        });


                    }

                }
            }

            return lista;


        }

        public ActionResult EliminarCliente(int id)
        {
            using (SqlConnection cn = new SqlConnection(cadena))

            {
                SqlCommand cmd = new SqlCommand("sp_EliminarCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCliente", id);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    TempData["error"] = "No se puede eliminar Cliente" + ex.Message;

                }

            }

            return RedirectToAction("ListaCliente");
        }



        [HttpPost]
        public JsonResult EditarCliente(Cliente model)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_EditarCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCliente", model.IdCliente);
                cmd.Parameters.AddWithValue("@NombreCliente", model.NombreCliente);
                cmd.Parameters.AddWithValue("@ApellidoCliente", model.ApellidoCliente);
                cmd.Parameters.AddWithValue("@Telefono", model.Telefono); 

                con.Open();
                cmd.ExecuteNonQuery();
            }


            return Json(true);
        }

    }
}