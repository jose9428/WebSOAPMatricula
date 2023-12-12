using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebSOAPMatricula.models;

namespace WebSOAPMatricula.dao
{
    public class AuthDAO
    {
        string strConn = ConfigurationManager.AppSettings["MatriculaConexion"].ToString();

        public Usuario Login(string correo, string contrasennia)
        {
            Usuario objUsu = null;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Usuario u inner join Rol r on r.cod_rol = u.cod_rol " +
                        " where correo = @correo and contrasennia = @contrasennia", conn);
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@contrasennia", contrasennia);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        Rol objRol = new Rol();
                        objUsu = new Usuario();
                        objUsu.codUsuario = (int) dr["cod_usuario"];
                        objUsu.nombres = (string) dr["nombres"];
                        objUsu.apellidos = (string) dr["apellidos"];
                        objUsu.correo = (string)dr["correo"];
                        objUsu.fechaRegistro = (DateTime) dr["fecha_registro"];
                        objRol.codRol = (int)dr["cod_rol"];
                        objRol.nombre = (string)dr["nombre_rol"];
                        objUsu.rol = objRol;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
            }
            return objUsu;
        }
    }
}