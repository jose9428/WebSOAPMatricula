using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebSOAPMatricula.models;

namespace WebSOAPMatricula.dao
{
    public class CarreraDAO
    {
        string strConn = ConfigurationManager.AppSettings["MatriculaConexion"].ToString();
        public List<Carrera> ListarTodos()
        {
            List<Carrera> lista = new List<Carrera>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select * from Carrera order by nombre_carrera asc", conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Carrera obj = new Carrera();
                        obj.codCarrera = (int)dr["cod_carrera"];
                        obj.nombre = (string)dr["nombre_carrera"];

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