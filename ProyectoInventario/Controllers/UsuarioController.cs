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
    public class UsuarioController : Controller
    {
        string cadena = "Data Source=.\\SQLEXPRESS;Initial Catalog=Prueba;User ID=sa;Password=1234;";
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AgregarUsuario(Usuario u)
        {

            string cadena = "Data Source=.\\SQLEXPRESS;Initial Catalog= Prueba;User ID=sa;Password=1234;";
            SqlConnection cn = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("sp_AgregarUsuario", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Usuario", u.User);
            cmd.Parameters.AddWithValue("@Clave", u.Password);
            cmd.Parameters.AddWithValue("@IdRol", u.IdRol);

            cn.Open();
            cmd.ExecuteNonQuery();
            return Content("agregado");


        }
        public ActionResult EditarUsuario(Usuario u)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_EditarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
                cmd.Parameters.AddWithValue("@Usuario", u.User);
                cmd.Parameters.AddWithValue("@Clave", u.Password);
                cmd.Parameters.AddWithValue("@IdRol", u.IdRol);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();


            }

            return RedirectToAction("ListaUsuario");


        }

        private List<Usuario> ListaTablaUusuario()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ListaUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Usuario
                        {

                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            User = dr["Usuario"].ToString(),
                            Password = dr["Clave"].ToString(),
                            NombreRol = dr["NombreRol"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        });
                    }
                }

            }

            return lista;
        }


        private List<SelectListItem> ComboRol()
        {


            List<SelectListItem> lista = new List<SelectListItem>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_ListaRol", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        lista.Add(new SelectListItem
                        {

                            Value = dr["IdRol"].ToString(),
                            Text = dr["NombreRol"].ToString()



                        });

                    }

                }

            }

            return lista;

        }


        public ActionResult ListaUsuario()
        {
            ViewBag.Rol = ComboRol();

            var Lista = ListaTablaUusuario();

            return View(Lista);

        }

        public ActionResult CambiarEstado(int id)
        {
        
            if (id == 0)
            {
                return Content("Error: El ID del usuario llegó en 0 o vacío.");
            }

            bool estadoActual = false;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
               
                string queryConsulta = "SELECT Activo FROM Usuario WHERE IdUsuario = @IdUsuario";
                SqlCommand cmdConsulta = new SqlCommand(queryConsulta, cn);
                cmdConsulta.Parameters.AddWithValue("@IdUsuario", id);

                try
                {
                    cn.Open();
                    var resultado = cmdConsulta.ExecuteScalar(); 
                    if (resultado != null && resultado != DBNull.Value)
                    {
                        estadoActual = Convert.ToBoolean(resultado);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error al consultar el usuario: " + ex.Message;
                    return RedirectToAction("ListaUsuario");
                }
            }

            bool nuevoEstado = !estadoActual;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                string queryUpdate = "UPDATE Usuario SET Activo = @Activo WHERE IdUsuario = @IdUsuario";
                SqlCommand cmdUpdate = new SqlCommand(queryUpdate, cn);
                cmdUpdate.Parameters.AddWithValue("@Activo", nuevoEstado);
                cmdUpdate.Parameters.AddWithValue("@IdUsuario", id);

                try
                {
                    cn.Open();
                    cmdUpdate.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error al actualizar el estado: " + ex.Message;
                }
            }

          
            return RedirectToAction("ListaUsuario");
        }


        public ActionResult EliminarUsuario(int id)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdUsuario", id);
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
            return RedirectToAction("ListaUsuario");


        }

    }
}