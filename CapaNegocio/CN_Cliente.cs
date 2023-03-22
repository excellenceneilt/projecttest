using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objcd_Cliente = new CD_Cliente();

        public List<Cliente> Listar()
        {
            return objcd_Cliente.Listar();
        }
        //Procedimientos de mantenimiento
        public int Registrar(Cliente obj, out string Mensaje)
        {

            //Validaciones por si se dejan campos vacíos
            Mensaje = string.Empty;
            if (obj.Documento == "")
            {
                Mensaje += "Es necesario definir un dni de Cliente\n";
            }
            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario definir un nombre de Cliente\n";
            }
            
            if (obj.Correo1 == "")
            {
                Mensaje += "Es necesario definir un correo del Cliente\n";
            }
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Cliente.Registrar(obj, out Mensaje);
            }

        }

        public bool Editar(Cliente obj, out string Mensaje)
        {

            //Validaciones
            Mensaje = string.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario definir un dni de Cliente\n";
            }
            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario definir un nombre de Cliente\n";
            }

            if (obj.Correo1 == "")
            {
                Mensaje += "Es necesario definir un correo del Cliente\n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Cliente.Editar(obj, out Mensaje);
            }





            //return objcd_Cliente.Editar(obj, out Mensaje);
        }

        public bool Eliminar(Cliente obj, out string Mensaje)
        {
            return objcd_Cliente.Eliminar(obj, out Mensaje);
        }
    }
}
