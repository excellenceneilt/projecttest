using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_TipoDocumento
    {
        private CD_TipoDocumento objcd_TipoDocumento = new CD_TipoDocumento();

        public List<Tipo_Documento> Listar()
        {
            return objcd_TipoDocumento.Listar();
        }
    }
}
