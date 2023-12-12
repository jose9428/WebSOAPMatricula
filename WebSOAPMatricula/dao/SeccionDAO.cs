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
    public class SeccionDAO
    {
        string strConn = ConfigurationManager.AppSettings["MatriculaConexion"].ToString();
        public List<Seccion> ListarTodos()
        {
            List<Seccion> lista = new List<Seccion>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Seccion S" +
                        " INNER JOIN Curso c ON c.cod_curso = s.cod_curso", conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Curso objCurso = new Curso();
                        Seccion objSecc = new Seccion();
                        objSecc.diaSemana = (string)dr["dia_semana"];
                        objSecc.numSeccion = (int)dr["num_seccion"];
                        objSecc.horaInicio = (TimeSpan)dr["hora_inicio"];
                        objSecc.horaFin = (TimeSpan)dr["hora_fin"];
                        objSecc.vacantesDisponibles = (int)dr["vacantes"];
                        objSecc.capacidad = (int)dr["capacidad"];
                        objSecc.aula = (string)dr["aula"];

                        objCurso.codCurso = (int)dr["cod_curso"];
                        objCurso.nombre = (string)dr["nombre_curso"];
                        objCurso.creditos = (int)dr["creditos"];
                        objCurso.horas = (int)dr["horas"];

                        objSecc.curso = objCurso;
                        objSecc.profesor = new ProfesorDAO().BuscarPorId((int)dr["cod_profesor"]);    
                        lista.Add(objSecc);
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

        public List<Seccion> ListarCursosDisponibles()
        {
            List<Seccion> lista = new List<Seccion>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Seccion S" +
                        " INNER JOIN Curso c ON c.cod_curso = s.cod_curso" +
                        " WHERE s.vacantes > 0", conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Curso objCurso = new Curso();
                        Seccion objSecc = new Seccion();
                        objSecc.diaSemana = (string)dr["dia_semana"];
                        objSecc.numSeccion = (int)dr["num_seccion"];
                        objSecc.horaInicio = (TimeSpan)dr["hora_inicio"];
                        objSecc.horaFin = (TimeSpan)dr["hora_fin"];
                        objSecc.vacantesDisponibles = (int)dr["vacantes"];
                        objSecc.capacidad = (int)dr["capacidad"];
                        objSecc.aula = (string)dr["aula"];

                        objCurso.codCurso = (int)dr["cod_curso"];
                        objCurso.nombre = (string)dr["nombre_curso"];
                        objCurso.creditos = (int)dr["creditos"];
                        objCurso.horas = (int)dr["horas"];

                        objSecc.curso = objCurso;
                        objSecc.profesor = new ProfesorDAO().BuscarPorId((int)dr["cod_profesor"]);
                        lista.Add(objSecc);
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

        public string Guardar(Seccion obj)
        {
            string result = "";
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_SECCION", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@cod_profesor", obj.profesor.codProfesor == 0 ? 0 : obj.profesor.codProfesor);
                    cmd.Parameters.AddWithValue("@cod_curso", obj.curso.codCurso == 0 ? 0 : obj.curso.codCurso);
                    cmd.Parameters.AddWithValue("@dia", obj.diaSemana == null ? "" : obj.diaSemana.Trim());
                    cmd.Parameters.AddWithValue("@hora_inicio", obj.cHoraInicio);
                    cmd.Parameters.AddWithValue("@capacidad", obj.capacidad);
                    cmd.Parameters.AddWithValue("@aula", obj.aula == null ? "" : obj.aula.Trim());
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
    }
}