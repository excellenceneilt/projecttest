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
    public class CD_TipoDocumento
    {
        //Metodo para listar
        public List<Tipo_Documento> Listar()
        {
            List<Tipo_Documento> lista = new List<Tipo_Documento>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select IdTipoDocumento, Descripcion, Estado from TIPODOCUMENTO");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Tipo_Documento()
                            {
                                IdTipoDocumento = Convert.ToInt32(dr["IdTipoDocumento"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])

                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Tipo_Documento>();
                }
            }
            return lista;
        }
    }
}
