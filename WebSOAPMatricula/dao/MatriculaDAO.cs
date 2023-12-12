using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using WebSOAPMatricula.models;

namespace WebSOAPMatricula.dao
{
    public class MatriculaDAO
    {
        string strConn = ConfigurationManager.AppSettings["MatriculaConexion"].ToString();
        public List<Matricula> ListarTodos()
        {
            List<Matricula> lista = new List<Matricula>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Matricula", conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Matricula objMatr = new Matricula();
                        objMatr.numMatricula = (int)dr["num_matricula"];
                        objMatr.fecha = (DateTime)dr["fecha"];
                        objMatr.totalHoras = (int)dr["total_horas"];
                        objMatr.costo = (decimal)dr["costo"];
                        objMatr.estado = (string)dr["estado"];
                        objMatr.estudiante = new EstudianteDAO().BuscarPorId((int)dr["cod_estudiante"]);
                        objMatr.detalles = BuscarDetallesPorMatricula(objMatr.numMatricula);
                        lista.Add(objMatr);
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

        public Matricula BuscarPorId(int idMatricula)
        {
            Matricula objMatr = null;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Matricula WHERE num_matricula = @numMatricula", conn);
                    cmd.Parameters.AddWithValue("@numMatricula", idMatricula);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        objMatr = new Matricula();
                        objMatr.numMatricula = (int)dr["num_matricula"];
                        objMatr.fecha = (DateTime)dr["fecha"];
                        objMatr.totalHoras = (int)dr["total_horas"];
                        objMatr.costo = (decimal)dr["costo"];
                        objMatr.estado = (string)dr["estado"];
                        objMatr.estudiante = new EstudianteDAO().BuscarPorId((int)dr["cod_estudiante"]);
                        objMatr.detalles = BuscarDetallesPorMatricula(objMatr.numMatricula);
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
            return objMatr;
        }

        public List<DetalleMatricula> BuscarDetallesPorMatricula(int numMatricula)
        {
            List<DetalleMatricula> lista = new List<DetalleMatricula>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Seccion s " +
                        " INNER JOIN DetalleMatricula d ON s.num_seccion = d.num_seccion " +
                        " INNER JOIN Curso c ON c.cod_curso = s.cod_curso" +
                        " INNER JOIN Profesor p ON p.cod_profesor = s.cod_profesor " +
                        " INNER JOIN Usuario u ON u.cod_usuario = p.cod_usuario " +
                        " WHERE num_matricula = @numMatricula", conn);
                    cmd.Parameters.AddWithValue("@numMatricula", numMatricula);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        DetalleMatricula obj = new DetalleMatricula();
                        obj.diaSemana = (string)dr["dia_semana"];
                        obj.numSeccion = (int)dr["num_seccion"];
                        obj.horaInicio = (TimeSpan)dr["hora_inicio"];
                        obj.horaFin = (TimeSpan)dr["hora_fin"];
                        obj.aula = (string)dr["aula"];
                        obj.profesor = (string)dr["nombres"] + " " +(string)dr["apellidos"];
                        obj.curso = (string) dr["nombre_curso"];

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

        public string Guardar(int codEstudiante , XElement listSecciones)
        {
            string result = "";
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_MATRICULA", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@cod_estudiante", codEstudiante);
                    cmd.Parameters.AddWithValue("@strSecciones", listSecciones.ToString());
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

        public List<MatriculaPagos> BuscarPagosPorEstudiante(int codEstudiante)
        {
            List<MatriculaPagos> lista = new List<MatriculaPagos>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT num_matricula , fecha , costo" +
                        " FROM Matricula" +
                        " WHERE cod_estudiante = @codEstudiante", conn);
                    cmd.Parameters.AddWithValue("@codEstudiante", codEstudiante);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        MatriculaPagos obj = new MatriculaPagos();
                        obj.numMatricula = (int)dr["num_matricula"];
                        obj.fecha = (DateTime)dr["fecha"];
                        obj.costo = (decimal)dr["costo"];

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

    }
}