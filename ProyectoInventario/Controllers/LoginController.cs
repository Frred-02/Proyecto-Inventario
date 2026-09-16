using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoInventario.Controllers
{
    public class LoginController : Controller
    {
        string cadena = "Data Source=.\\SQLEXPRESS;Initial Catalog= Prueba;User ID=sa;Password=1234;";
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string clave)
        {

            SqlConnection cn = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("sp_Login", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            cmd.Parameters.AddWithValue("@Clave", clave);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
               
                bool estaActivo = Convert.ToBoolean(dr["Activo"]);

                if (!estaActivo)
                {
                    cn.Close();
                    ViewBag.Mensaje = "Tu cuenta se encuentra deshabilitada. Contacta al administrador.";
                    return View();
                }

              
                Session["Usuario"] = dr["Usuario"].ToString();
                Session["Rol"] = dr["NombreRol"].ToString();

                cn.Close();
                return RedirectToAction("listaProducto", "Producto");
            }

            cn.Close();
            ViewBag.Mensaje = "Usuario o Clave incorrecta";
            return View();
        }

        public ActionResult CerrarSesion()
        {
          
            Session.Clear(); 
            Session.Abandon(); 

         
            return RedirectToAction("Login");
        }

    }
}