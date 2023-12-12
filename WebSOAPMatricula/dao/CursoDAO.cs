using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebSOAPMatricula.models;

namespace WebSOAPMatricula.dao
{
    public class CursoDAO
    {
        string strConn = ConfigurationManager.AppSettings["MatriculaConexion"].ToString();
        public List<Curso> ListarTodos()
        {
            List<Curso> lista = new List<Curso>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Curso order by nombre_curso asc", conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    
                    while (dr.Read())
                    {
                        Curso obj = new Curso();
                        obj.codCurso = (int)dr["cod_curso"];
                        obj.nombre = (string)dr["nombre_curso"];
                        obj.creditos = (int)dr["creditos"];
                        obj.horas = (int)dr["horas"];

                        lista.Add(obj);
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

        public string Guardar(Curso obj)
        {
            string result = "";
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into Curso(nombre_curso,creditos,horas) " +
                        " values(@nombre,@creditos,@horas)", conn);
                    cmd.Parameters.AddWithValue("@nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("@creditos", obj.creditos);
                    cmd.Parameters.AddWithValue("@horas", obj.horas);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = "Lo sentimos no se pudieron guardaron datos.";
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

        public string Actualizar(Curso obj)
        {
            string result = "";
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update Curso set nombre_curso=@nombre," +
                        " creditos=@creditos,horas=@horas" +
                        " where cod_curso=@id", conn);
                    cmd.Parameters.AddWithValue("@nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("@creditos", obj.creditos);
                    cmd.Parameters.AddWithValue("@horas", obj.horas);
                    cmd.Parameters.AddWithValue("@id", obj.codCurso);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = "Lo sentimos no se pudieron actualizar datos.";
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

        public Curso BuscarPorId(int codigo)
        {
            Curso obj = null;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Curso where cod_curso=@id", conn);
                    cmd.Parameters.AddWithValue("@id", codigo);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        obj = new Curso();
                        obj.codCurso = (int)dr["cod_curso"];
                        obj.nombre = (string)dr["nombre_curso"];
                        obj.creditos = (int)dr["creditos"];
                        obj.horas = (int)dr["horas"];
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
            return obj;
        }

        public string Eliminar(int codigo)
        {
            string result = "";
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from Curso where cod_curso=@id", conn);
                    cmd.Parameters.AddWithValue("@id", codigo);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = "Lo sentimos! no se pudo eliminar registro.";
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

        public bool ExisteCurso(Curso obj)
        {
            int result = 0;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select count(1) as 'cantidad'  from Curso " +
                        " where nombre_curso = @nombre and (@id = 0 or cod_curso!=@id)", conn);
                    cmd.Parameters.AddWithValue("@nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("@id", obj.codCurso);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        result = (int)dr["cantidad"];

                        return result > 0;
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
            return false;
        }

    }
}