using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebSOAPMatricula.models;

namespace WebSOAPMatricula.dao
{
    public class ProfesorDAO
    {
        string strConn = ConfigurationManager.AppSettings["MatriculaConexion"].ToString();
        public List<Profesor> ListarTodos()
        {
            List<Profesor> lista = new List<Profesor>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Profesor e " +
                        " INNER JOIN Usuario u ON u.cod_usuario = e.cod_usuario " +
                        " INNER JOIN Rol r ON r.cod_rol = u.cod_rol", conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Profesor objProf = new Profesor();
                        Usuario objUsu = new Usuario();
                        Rol objRol = new Rol();
                        objRol.codRol = (int)dr["cod_rol"];
                        objRol.nombre = (string)dr["nombre_rol"];
                        objUsu.codUsuario = (int)dr["cod_usuario"];
                        objUsu.nombres = (string)dr["nombres"];
                        objUsu.apellidos = (string)dr["apellidos"];
                        objUsu.correo = (string)dr["correo"];
                        objUsu.fechaRegistro = (DateTime)dr["fecha_registro"];
                        objProf.direccion = (string)dr["direccion"];
                        objProf.telefono = (string)dr["telefono"];
                        objProf.codProfesor = (int)dr["cod_profesor"];
                        objUsu.rol = objRol;
                        objProf.usuario = objUsu;
                        lista.Add(objProf);
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
            return lista;
        }

        public string Mantenimiento(Profesor obj, int accion)
        {
            string result = "";
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_MANT_Profesor", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@cod_profesor", obj.codProfesor == 0 ? 0 : obj.codProfesor);
                    cmd.Parameters.AddWithValue("@nombres", obj.usuario.nombres == null ? "" : obj.usuario.nombres.Trim());
                    cmd.Parameters.AddWithValue("@apellidos", obj.usuario.apellidos == null ? "" : obj.usuario.apellidos.Trim());
                    cmd.Parameters.AddWithValue("@correo", obj.usuario.correo == null ? "" : obj.usuario.correo.Trim());
                    cmd.Parameters.AddWithValue("@contrasennia", obj.usuario.contraseña == null ? "" : obj.usuario.contraseña);
                    cmd.Parameters.AddWithValue("@direccion", obj.direccion == null ? "" : obj.direccion);
                    cmd.Parameters.AddWithValue("@telefono", obj.telefono == null ? "" : obj.telefono.Trim());
                    cmd.Parameters.AddWithValue("@accion", accion);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        result = dr.GetString(0);
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        public Profesor BuscarPorId(int codigo)
        {
            Profesor objProf = null;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Profesor e " +
                        " INNER JOIN Usuario u ON u.cod_usuario = e.cod_usuario " +
                        " INNER JOIN Rol r ON r.cod_rol = u.cod_rol" +
                        " WHERE e.cod_profesor=@id", conn);
                    cmd.Parameters.AddWithValue("@id", codigo);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        objProf = new Profesor();
                        Usuario objUsu = new Usuario();
                        Rol objRol = new Rol();
                        objRol.codRol = (int)dr["cod_rol"];
                        objRol.nombre = (string)dr["nombre_rol"];
                        objUsu.codUsuario = (int)dr["cod_usuario"];
                        objUsu.nombres = (string)dr["nombres"];
                        objUsu.apellidos = (string)dr["apellidos"];
                        objUsu.correo = (string)dr["correo"];
                        objUsu.fechaRegistro = (DateTime)dr["fecha_registro"];
                        objProf.direccion = (string)dr["direccion"];
                        objProf.telefono = (string)dr["telefono"];
                        objProf.codProfesor = (int)dr["cod_profesor"];
                        objUsu.rol = objRol;
                        objProf.usuario = objUsu;
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
            return objProf;
        }
    }
}