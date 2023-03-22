using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;

namespace CapaDatos
{
    public class CD_Cliente
    {
        //Metodo para listar
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    
                    query.AppendLine("select IdCliente, CodigoCliente, td.IdTipoDocumento, td.Descripcion[Documentos], Documento, RUC, RazonSocial, tc.IdTipoCliente, tc.Descripcion[TipoCliente], NombreCompleto, Direccion,CMP,e.IdEspecialidad, e.Descripcion[Especialidad],NombreComercial, DireccionComercial, Correo1,Telefono1,Correo2,Telefono2, Departamento,  c.Estado from CLIENTE c");
                    query.AppendLine("inner join ESPECIALIDAD e on e.IdEspecialidad = c.IdEspecialidad");
                    query.AppendLine("inner join TIPODOCUMENTO td on td.IdTipoDocumento = c.IdTipoDocumento");
                    query.AppendLine("inner join TIPOCLIENTE tc on tc.IdTipoCliente = c.IdTipoCliente");
                    query.AppendLine("inner join Departamento d on d.Descripcion = c.Departamento");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente()
                            {
                                //Listar Clientes en tabla no necesariamente en orden

                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                CodigoCliente = dr["CodigoCliente"].ToString(),
                                oTipo_Documento = new Tipo_Documento() { IdTipoDocumento= Convert.ToInt32(dr["IdTipoDocumento"]), Descripcion = dr["Documentos"].ToString() },
                                Documento = dr["Documento"].ToString(),
                                RUC = dr["RUC"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                oTipo_Cliente = new Tipo_Cliente() { IdTipoCliente = Convert.ToInt32(dr["IdTipoCliente"]), Descripcion = dr["TipoCliente"].ToString() },
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                CMP = dr["CMP"].ToString(),
                                oEspecialidad = new Especialidad() { IdEspecialidad = Convert.ToInt32(dr["IdEspecialidad"]), /*Añadiendo alias*/ Descripcion = dr["Especialidad"].ToString() },
                                NombreComercial = dr["NombreComercial"].ToString(),
                                DireccionComercial = dr["DireccionComercial"].ToString(),
                                Correo1 = dr["Correo1"].ToString(),
                                Telefono1 = dr["Telefono1"].ToString(),
                                Correo2 = dr["Correo2"].ToString(),
                                Telefono2 = dr["Telefono2"].ToString(),
                                Departamento= dr["Departamento"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])

                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Cliente>();
                }
            }
            return lista;
        }
        //Los parametros de Mensaje es un parametro de salida y Cliente de entrada
        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idClientegenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {

                    //Declarando los parámetros de entrada
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCliente", oconexion);
                   // cmd.Parameters.AddWithValue("CodigoCliente", obj.CodigoCliente);
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);//Los parametros entre "" se escriben sin arroba, referencian a los campos con @ dentro del procedimiento almacenado, el de la derecha es la entidad
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("NombreComercial", obj.NombreComercial);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("DireccionComercial", obj.DireccionComercial);
                    cmd.Parameters.AddWithValue("IdTipoCliente", obj.oTipo_Cliente.IdTipoCliente);
                    cmd.Parameters.AddWithValue("IdTipoDocumento", obj.oTipo_Documento.IdTipoDocumento);
                    cmd.Parameters.AddWithValue("Departamento", obj.Departamento);
                    cmd.Parameters.AddWithValue("Provincia", obj.Provincia);
                    cmd.Parameters.AddWithValue("Distrito", obj.Distrito);
                    cmd.Parameters.AddWithValue("DepartamentoComercial", obj.DepartamentoComercial);
                    cmd.Parameters.AddWithValue("ProvinciaComercial", obj.ProvinciaComercial);
                    cmd.Parameters.AddWithValue("DistritoComercial", obj.DistritoComercial);
                    cmd.Parameters.AddWithValue("Correo1", obj.Correo1);
                    cmd.Parameters.AddWithValue("Correo2", obj.Correo2);
                    cmd.Parameters.AddWithValue("Telefono1", obj.Telefono1);
                    cmd.Parameters.AddWithValue("Telefono2", obj.Telefono2);
                    cmd.Parameters.AddWithValue("CMP", obj.CMP);
                    cmd.Parameters.AddWithValue("RazonSocial", obj.RazonSocial);
                    cmd.Parameters.AddWithValue("RUC", obj.RUC);
                    cmd.Parameters.AddWithValue("IdEspecialidad", obj.oEspecialidad.IdEspecialidad);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);

                    //Declarando parámetros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    //Obtener parametros de salida después de ejecución
                    idClientegenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                idClientegenerado = 0;
                Mensaje = ex.Message;
            }

            return idClientegenerado;
        }

        public bool Editar(Cliente obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {

                    //Declarando los parámetros de entrada
                    SqlCommand cmd = new SqlCommand("SP_ModificarCliente", oconexion);
                    cmd.Parameters.AddWithValue("IdCliente", obj.IdCliente);

                    cmd.Parameters.AddWithValue("Documento", obj.Documento);//Los parametros entre "" se escriben sin arroba, referencian a los campos con @ dentro del procedimiento almacenado
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("NombreComercial", obj.NombreComercial);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("DireccionComercial", obj.DireccionComercial);
                    cmd.Parameters.AddWithValue("IdTipoCliente", obj.oTipo_Cliente.IdTipoCliente);
                    cmd.Parameters.AddWithValue("IdTipoDocumento", obj.oTipo_Documento.IdTipoDocumento);
                    cmd.Parameters.AddWithValue("Departamento", obj.Departamento);
                    cmd.Parameters.AddWithValue("Provincia", obj.Provincia);
                    cmd.Parameters.AddWithValue("Distrito", obj.Distrito);
                    cmd.Parameters.AddWithValue("DepartamentoComercial", obj.DepartamentoComercial);
                    cmd.Parameters.AddWithValue("ProvinciaComercial", obj.ProvinciaComercial);
                    cmd.Parameters.AddWithValue("DistritoComercial", obj.DistritoComercial);
                    cmd.Parameters.AddWithValue("Correo1", obj.Correo1);
                    cmd.Parameters.AddWithValue("Correo2", obj.Correo2);
                    cmd.Parameters.AddWithValue("Telefono1", obj.Telefono1);
                    cmd.Parameters.AddWithValue("Telefono2", obj.Telefono2);
                    cmd.Parameters.AddWithValue("CMP", obj.CMP);
                    cmd.Parameters.AddWithValue("RazonSocial", obj.RazonSocial);
                    cmd.Parameters.AddWithValue("RUC", obj.RUC);
                    cmd.Parameters.AddWithValue("IdEspecialidad", obj.oEspecialidad.IdEspecialidad);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);

                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    //Obtener parametros de salida después de ejecución
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }

        public bool Eliminar(Cliente obj, out string Mensaje)
         {
             bool respuesta = false;
             Mensaje = string.Empty;
             try
             {
                 using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                 {

                     //Declarando los parámetros de entrada
                     SqlCommand cmd = new SqlCommand("delete from cliente where IdCliente =@id", oconexion);
                     cmd.Parameters.AddWithValue("@id", obj.IdCliente);
                   /*  cmd.Parameters.Add("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                     cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;*/
                     cmd.CommandType = CommandType.Text;
                     oconexion.Open();
                     respuesta = cmd.ExecuteNonQuery()>0? true : false ;

                     //Obtener parametros de salida después de ejecución
                     /*respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
                     Mensaje = cmd.Parameters["Mensaje"].Value.ToString();*/
                 }

             }
             catch (Exception ex)
             {
                 respuesta = false;
                 Mensaje = ex.Message;
             }

             return respuesta;
         }
    }
}
