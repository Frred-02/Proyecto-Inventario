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
    public class RolController : Controller
    {
        string cadena = "Data Source=.\\SQLEXPRESS;Initial Catalog= Prueba;User ID=sa;Password=1234;";




        public ActionResult AgregarRol(Rol r)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_Rol", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NombreRol", r.NombreRol);
                cn.Open();
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction("ListaRol");



        }

        public ActionResult EditarRol (Rol r)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_EditarRol", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRol", r.IdRol);
                cmd.Parameters.AddWithValue("@NombreRol", r.NombreRol);
                cn.Open();
                cmd.ExecuteNonQuery();  


            }
            return Json(true);

        }




        public ActionResult ListaRol()
        {
            
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }

            List<Rol> lista = new List<Rol>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ListaRol", cn);
                cmd.CommandType = CommandType.StoredProcedure; 

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Rol r = new Rol
                        {
                            IdRol = Convert.ToInt32(dr["IdRol"]),
                            NombreRol = dr["NombreRol"].ToString()
                        };

                        lista.Add(r);
                    }
                }
            } 

            return View(lista);
        }

        public ActionResult EliminarRol(int id)
        {

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarRol", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRol", id);

                try
                {

                    cn.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                    TempData["error"] = "No se puede eliminar rol" + ex.Message;

                }

            }
            return RedirectToAction("ListaRol");


        }

    }
}