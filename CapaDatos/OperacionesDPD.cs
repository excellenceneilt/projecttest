using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class OperacionesDPD
    {

        public List<Departamento> ObtenerDepartamento()
        {
            List<Departamento> olistaDepartamento = new List<Departamento>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                
                    SqlCommand cmd = new SqlCommand("SP_ObtenerDepartamento", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;
                    oconexion.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                        {
                            olistaDepartamento.Add(new Departamento
                            {
                                IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]),
                                Descripcion = Convert.ToString(dr["Descripcion"].ToString())
                            });
                        }
                        dr.Close();
                    
               
            }
            return olistaDepartamento;
        }

        public List<Provincia> ObtenerProvincia(int IdDepartamento)
        {
            List<Provincia> olistaProvincia = new List<Provincia>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
               
                    SqlCommand cmd = new SqlCommand("SP_ObtenerProvincia", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdDepartamento", IdDepartamento);
                    cmd.CommandTimeout = 600;
                    oconexion.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                        {
                            olistaProvincia.Add(new Provincia
                            {
                                IdProvincia = Convert.ToInt32(dr["IdProvincia"]),
                                Descripcion = Convert.ToString(dr["Descripcion"].ToString())

                            });
                        }
                        dr.Close();
                    
                
            }
            return olistaProvincia;
        }


        public List<Distrito> ObtenerDistrito(int IdProvincia)
        {
            List<Distrito> olistaDistrito = new List<Distrito>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {



                
                    SqlCommand cmd = new SqlCommand("SP_ObtenerDistrito", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdProvincia", IdProvincia);
                    cmd.CommandTimeout = 600;
                    oconexion.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                    
                        while (dr.Read())
                        {
                            olistaDistrito.Add(new Distrito
                            {
                                IdDistrito = Convert.ToInt32(dr["IdDIstrito"]),
                                Descripcion = Convert.ToString(dr["Descripcion"].ToString())
                            });
                        }
                        dr.Close();
                    
                    
                
            }
            return olistaDistrito;
        }





    }
}
