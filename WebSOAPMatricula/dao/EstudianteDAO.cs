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
    public class EstudianteDAO
    {
        string strConn = ConfigurationManager.AppSettings["MatriculaConexion"].ToString();
        public List<Estudiante> ListarTodos()
        {
            List<Estudiante> lista = new List<Estudiante>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Estudiante e " +
                        " INNER JOIN Usuario u ON u.cod_usuario = e.cod_usuario " +
                        " INNER JOIN Rol r ON r.cod_rol = u.cod_rol" +
                        " INNER JOIN Carrera c ON c.cod_carrera = e.cod_carrera", conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Estudiante objEst = new Estudiante();
                        Usuario objUsu = new Usuario();
                        Carrera objCarr = new Carrera();
                        Rol objRol = new Rol();
                        objRol.codRol = (int)dr["cod_rol"];
                        objRol.nombre = (string)dr["nombre_rol"];
                        objUsu.codUsuario = (int)dr["cod_usuario"];
                        objUsu.nombres = (string)dr["nombres"];
                        objUsu.apellidos = (string)dr["apellidos"];
                        objUsu.correo = (string)dr["correo"];
                        objUsu.fechaRegistro = (DateTime)dr["fecha_registro"];
                        objEst.apoderado = (string)dr["apoderado"];
                        objCarr.codCarrera = (int)dr["cod_carrera"];
                        objCarr.nombre = (string)dr["nombre_carrera"];
                        objEst.codEstudiante = (int)dr["cod_estudiante"];
                        objUsu.rol = objRol;
                        objEst.usuario = objUsu;
                        objEst.carrera = objCarr;
                        lista.Add(objEst);
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

        public string Mantenimiento(Estudiante obj, int accion)
        {
            string result = "";
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_MANT_Estudiante", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@cod_estudiante", obj.codEstudiante == 0 ? 0 : obj.codEstudiante);
                    cmd.Parameters.AddWithValue("@nombres", obj.usuario.nombres == null ? "": obj.usuario.nombres.Trim());
                    cmd.Parameters.AddWithValue("@apellidos", obj.usuario.apellidos == null ? "" : obj.usuario.apellidos.Trim());
                    cmd.Parameters.AddWithValue("@correo", obj.usuario.correo == null ? "" : obj.usuario.correo.Trim());
                    cmd.Parameters.AddWithValue("@contrasennia", obj.usuario.contraseña == null ? "" : obj.usuario.contraseña);
                    cmd.Parameters.AddWithValue("@cod_carrera", obj.carrera.codCarrera == 0 ? 0: obj.carrera.codCarrera);
                    cmd.Parameters.AddWithValue("@apoderado", obj.apoderado == null ? "" : obj.apoderado.Trim());
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

        public Estudiante BuscarPorId(int codigo)
        {
            Estudiante objEst = null;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Estudiante e " +
                        " INNER JOIN Usuario u ON u.cod_usuario = e.cod_usuario " +
                        " INNER JOIN Rol r ON r.cod_rol = u.cod_rol" +
                        " INNER JOIN Carrera c ON c.cod_carrera = e.cod_carrera" +
                        " WHERE e.cod_estudiante = @id", conn);
                    cmd.Parameters.AddWithValue("@id", codigo);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        objEst = new Estudiante();
                        Usuario objUsu = new Usuario();
                        Carrera objCarr = new Carrera();
                        Rol objRol = new Rol();
                        objRol.codRol = (int)dr["cod_rol"];
                        objRol.nombre = (string)dr["nombre_rol"];
                        objUsu.codUsuario = (int)dr["cod_usuario"];
                        objUsu.nombres = (string)dr["nombres"];
                        objUsu.apellidos = (string)dr["apellidos"];
                        objUsu.correo = (string)dr["correo"];
                        objUsu.fechaRegistro = (DateTime)dr["fecha_registro"];
                        objEst.apoderado = (string)dr["apoderado"];
                        objCarr.codCarrera = (int)dr["cod_carrera"];
                        objCarr.nombre = (string)dr["nombre_carrera"];
                        objEst.codEstudiante = (int)dr["cod_estudiante"];
                        objUsu.rol = objRol;
                        objEst.usuario = objUsu;
                        objEst.carrera = objCarr;
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
            return objEst;
        }

    }
}