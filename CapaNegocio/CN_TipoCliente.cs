using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_TipoCliente
    {
       private CD_Tipo_Cliente objcd_TipoCliente = new CD_Tipo_Cliente();
      

        public List<Tipo_Cliente> Listar()
        {
            return objcd_TipoCliente.Listar();
        }
        //Procedimientos de mantenimiento
        /*
        public int Registrar(Especialidad obj, out string Mensaje)
        {

            //Validaciones
            Mensaje = string.Empty;
            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario definir una Especialidad\n";
            }
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Especialidad.Registrar(obj, out Mensaje);
            }

        }

        public bool Editar(Especialidad obj, out string Mensaje)
        {

            //Validaciones
            Mensaje = string.Empty;

            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario definir una Especialidad\n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Especialidad.Editar(obj, out Mensaje);
            }





            //return objcd_Especialidad.Editar(obj, out Mensaje);
        }

        public bool Eliminar(Especialidad obj, out string Mensaje)
        {
            return objcd_Especialidad.Eliminar(obj, out Mensaje);
        }*/
    }
}
