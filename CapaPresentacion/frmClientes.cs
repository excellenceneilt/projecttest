#region USING LIBRARIES
using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

namespace CapaPresentacion
{
    public partial class frmClientes : Form
    {
        public frmClientes()
        {
            InitializeComponent();
        }

        private void frmClientes_Load(object sender, EventArgs e)
        {
           ListarDepartamento();

            #region COLUMNAS
            foreach (DataGridViewColumn c in dgvdata.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
                c.Selected = false;
            }
            dgvdata.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
            dgvdata.Columns[0].Selected = true;

            //Seleccionar columnas
            this.dgvdata.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvdata.MultiSelect = false;
            #endregion

            #region COMBOBOX

            //Estados 
            cboestado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboestado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No activo" });
            cboestado.DisplayMember = "Texto";
            cboestado.ValueMember = "Valor";
            cboestado.SelectedIndex = 0;

            //Listar Especialidad en combobox
            List<Especialidad> listaEspecialidad = new CN_Especialidad().Listar();
            foreach (Especialidad item in listaEspecialidad)
            {
                cboespecialidad.Items.Add(new OpcionCombo() { Valor = item.IdEspecialidad, Texto = item.Descripcion });
            }
            cboespecialidad.DisplayMember = "Texto";
            cboespecialidad.ValueMember = "Valor";
            cboespecialidad.SelectedIndex = 0;

            //Listar TipoCliente en combobox
            List<Tipo_Cliente> listaTipoCliente = new CN_TipoCliente().Listar();
            foreach (Tipo_Cliente item in listaTipoCliente)
            {
                cbotipocliente.Items.Add(new OpcionCombo() { Valor = item.IdTipoCliente, Texto = item.Descripcion });
            }
            cbotipocliente.DisplayMember = "Texto";
            cbotipocliente.ValueMember = "Valor";
            cbotipocliente.SelectedIndex = 0;

            //Listar TipoDocumento en combobox
            List<Tipo_Documento> listaTipoDocumento = new CN_TipoDocumento().Listar();
            foreach (Tipo_Documento item in listaTipoDocumento)
            {
                cbotipodocumento.Items.Add(new OpcionCombo() { Valor = item.IdTipoDocumento, Texto = item.Descripcion });
            }
            cbotipodocumento.DisplayMember = "Texto";
            cbotipodocumento.ValueMember = "Valor";
            cbotipodocumento.SelectedIndex = 0;

            #endregion

            #region FILTRAR REGISTROS EN TABLA

            //Para filtrar registros en el datagrid
            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnseleccionar")
                {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;
            #endregion

            #region LLENAR DATAGRID
            
            List<Cliente> listaCliente = new CN_Cliente().Listar();
            foreach (Cliente item in listaCliente)
            {//Colocar los items en orden, tal cual las columnas del datagrid
                dgvdata.Rows.Add(new object[] {
                     "",
                    item.IdCliente,
                    item.CodigoCliente,
                    item.oTipo_Documento.IdTipoDocumento,
                    item.oTipo_Documento.Descripcion,
                    item.Documento,
                    item.RUC,
                    item.RazonSocial,
                    item.oTipo_Cliente.IdTipoCliente,
                    item.oTipo_Cliente.Descripcion,
                    item.NombreCompleto,
                    item.Direccion,
                    item.CMP,
                    item.oEspecialidad.IdEspecialidad,
                    item.oEspecialidad.Descripcion,
                    item.NombreComercial,
                    item.DireccionComercial,
                    item.Correo1,
                    item.Telefono1,
                    item.Correo2,
                    item.Telefono2,
                    item.Departamento,
                    item.Estado==true ?1 : 0,
                    item.Estado==true ?"Activo":"Inactivo",
                });
            }
            #endregion

        }



        #region LISTAR DEPARTAMENTOS PROVINCIAS Y DISTRITOS
        public void ListarDepartamento()
        {
            //Facturación
            cbodepartamento.DataSource = new OperacionesDPD().ObtenerDepartamento();
            cbodepartamento.ValueMember = "IdDepartamento";
            cbodepartamento.DisplayMember = "Descripcion";
            //Comercial
            cbodepartamentocomercial.DataSource = new OperacionesDPD().ObtenerDepartamento();
            cbodepartamentocomercial.ValueMember = "IdDepartamento";
            cbodepartamentocomercial.DisplayMember = "Descripcion";
        }
        //Datos de facturación
        private void cbodepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            Departamento odepartamentoSeleccionado = (Departamento)cbodepartamento.SelectedItem;
            cboprovincia.DataSource = new OperacionesDPD().ObtenerProvincia(odepartamentoSeleccionado.IdDepartamento);
            cboprovincia.ValueMember = "IdProvincia";
            cboprovincia.DisplayMember = "Descripcion";
        }
        private void cboprovincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            Provincia oprovinciaSeleccionado = (Provincia)cboprovincia.SelectedItem;
            cbodistrito.DataSource = new OperacionesDPD().ObtenerDistrito(oprovinciaSeleccionado.IdProvincia);
            cbodistrito.ValueMember = "IdDistrito";
            cbodistrito.DisplayMember = "Descripcion";
        }
        //Datos comerciales
        private void cbodepartamentocomercial_SelectedIndexChanged(object sender, EventArgs e)
        {
            Departamento odepartamentoSeleccionado = (Departamento)cbodepartamentocomercial.SelectedItem;
            cboprovinciacomercial.DataSource = new OperacionesDPD().ObtenerProvincia(odepartamentoSeleccionado.IdDepartamento);
            cboprovinciacomercial.ValueMember = "IdProvincia";
            cboprovinciacomercial.DisplayMember = "Descripcion";
        }
        private void cboprovinciacomercial_SelectedIndexChanged(object sender, EventArgs e)
        {
            Provincia oprovinciaSeleccionado = (Provincia)cboprovinciacomercial.SelectedItem;
            cbodistritocomercial.DataSource = new OperacionesDPD().ObtenerDistrito(oprovinciaSeleccionado.IdProvincia);
            cbodistritocomercial.ValueMember = "IdDistrito";
            cbodistritocomercial.DisplayMember = "Descripcion";
        }

        #endregion

        #region BOTONES
        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtid.Text) != 0)
            {
                if (MessageBox.Show("¿Desea eliminar el cliente?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Cliente objCliente = new Cliente()
                    {
                        //Llenando atributos de la clase Cliente
                        IdCliente = Convert.ToInt32(txtid.Text),


                    };

                    bool respuesta = new CN_Cliente().Eliminar(objCliente, out mensaje);
                    if (respuesta)
                    {
                        dgvdata.Rows.RemoveAt(Convert.ToInt32(txtindice.Text));
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
            }
        }
        private void btnbuscar_Click(object sender, EventArgs e)
        {
            buscar();
        }
        private void btnguardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Cliente objCliente = new Cliente()
            {
                //Asociando los atributos de la entidad con los datos obtenidos en cada entrada (textbox, combobox)
                IdCliente = Convert.ToInt32(txtid.Text),
                CodigoCliente= txtcodigocliente.Text,
                Documento =txtdocumento.Text,
                NombreCompleto = txtnombrecompleto.Text,
                NombreComercial = txtnombrecomercial.Text,
                Direccion = txtdireccion.Text,
                DireccionComercial = txtdireccioncomercial.Text,
                oTipo_Cliente = new Tipo_Cliente() { IdTipoCliente = Convert.ToInt32(((OpcionCombo)cbotipocliente.SelectedItem).Valor) },
                oTipo_Documento = new Tipo_Documento() { IdTipoDocumento = Convert.ToInt32(((OpcionCombo)cbotipodocumento.SelectedItem).Valor) },
                Departamento = this.cbodepartamento.GetItemText(this.cbodepartamento.SelectedItem),
                Provincia = this.cboprovincia.GetItemText(this.cboprovincia.SelectedItem),
                Distrito = this.cbodistrito.GetItemText(this.cbodistrito.SelectedItem),
                DepartamentoComercial = this.cbodepartamento.GetItemText(this.cbodepartamentocomercial.SelectedItem),
                ProvinciaComercial = this.cboprovincia.GetItemText(this.cboprovinciacomercial.SelectedItem),
                DistritoComercial = this.cbodistrito.GetItemText(this.cbodistritocomercial.SelectedItem),
                Correo1 = txtcorreo1.Text,
                Telefono1 = txttelefono1.Text,
                RazonSocial = txtrazonsocial.Text,
                CMP = txtcmp.Text,
                RUC = txtruc.Text,
                oEspecialidad = new Especialidad() { IdEspecialidad = Convert.ToInt32(((OpcionCombo)cboespecialidad.SelectedItem).Valor) },
                Correo2 = txtcorreo2.Text,
                Telefono2 = txttelefono2.Text,
                
                //El item seleccionado se convierte a la clase opcioncombo, y se accede a su propiedad valor, si es igual a 1 será true caso contrario false
                Estado = Convert.ToInt32(((OpcionCombo)cboestado.SelectedItem).Valor) == 1 ? true : false
            };


            if (objCliente.IdCliente == 0)
            {
                //Se usa el método registrar desde la capa negocio
                int idClientegenerado = new CN_Cliente().Registrar(objCliente, out mensaje);

                //El registro se hará siempre y cuando idClientegenerado sea diferente a cero, si es igual a cero indica que o se registró
                if (idClientegenerado != 0)
                {
                    //En el mismo orden que la tabla por favor es acá
                    dgvdata.Rows.Add(new object[] {"",
                        idClientegenerado,
                        txtcodigocliente.Text,
                         ((OpcionCombo) cbotipodocumento.SelectedItem).Valor.ToString(),
                         ((OpcionCombo) cbotipodocumento.SelectedItem).Texto.ToString(),
                        txtdocumento.Text,
                        txtruc.Text,
                        txtrazonsocial.Text,
                        ((OpcionCombo) cbotipocliente.SelectedItem).Valor.ToString(),
                        ((OpcionCombo) cbotipocliente.SelectedItem).Texto.ToString(),
                        txtnombrecompleto.Text,
                        txtdireccion.Text,
                        txtcmp.Text,
                        ((OpcionCombo) cboespecialidad.SelectedItem).Valor.ToString(),
                         ((OpcionCombo) cboespecialidad.SelectedItem).Texto.ToString(),
                         txtnombrecomercial.Text,
                         txtdireccioncomercial.Text,
                         txtcorreo1.Text,
                        txttelefono1.Text,
                        txtcorreo2.Text,
                        txttelefono2.Text,
                
                        
                         
                        
                        
                        
                        
                      
                        
                        
                        
                        
                      ((OpcionCombo) cboestado.SelectedItem).Valor.ToString(),
                         ((OpcionCombo) cboestado.SelectedItem).Texto.ToString(),
                });

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
            else
            {
                bool resultado = new CN_Cliente().Editar(objCliente, out mensaje);

                if (resultado)
                {

                    
                    DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtindice.Text)];
                    row.Cells["Id"].Value = txtid.Text;
                    row.Cells["IdTipoDocumento"].Value = ((OpcionCombo)cbotipodocumento.SelectedItem).Valor.ToString();
                    row.Cells["TipoDocumento"].Value = ((OpcionCombo)cbotipodocumento.SelectedItem).Texto.ToString();
                    row.Cells["Documento"].Value = txtdocumento.Text;
                    row.Cells["RUC"].Value = txtruc.Text;
                    row.Cells["RazonSocial"].Value = txtrazonsocial.Text;
                    row.Cells["IdTipoCliente"].Value = ((OpcionCombo)cbotipocliente.SelectedItem).Valor.ToString();
                    row.Cells["TipoCliente"].Value = ((OpcionCombo)cbotipocliente.SelectedItem).Texto.ToString();
                    row.Cells["NombreCompleto"].Value = txtnombrecompleto.Text;
                    row.Cells["Direccion"].Value =txtdireccion.Text;
                    row.Cells["CMP"].Value = txtcmp.Text;
                    row.Cells["IdEspecialidad"].Value = ((OpcionCombo)cboespecialidad.SelectedItem).Valor.ToString();
                    row.Cells["Especialidad"].Value = ((OpcionCombo)cboespecialidad.SelectedItem).Texto.ToString();
                    row.Cells["NombreComercial"].Value = txtnombrecomercial.Text;
                    row.Cells["DireccionComercial"].Value = txtdireccioncomercial.Text;
                    row.Cells["Correo1"].Value = txtcorreo1.Text;
                    row.Cells["Telefono1"].Value = txttelefono1.Text;
                    row.Cells["Correo2"].Value = txtcorreo2.Text;
                    row.Cells["Telefono2"].Value = txttelefono2.Text;
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cboestado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cboestado.SelectedItem).Texto.ToString();
                    Limpiar();

                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }


        }
        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                row.Visible = true;
            }
        }

        #endregion

        #region PROCEDIMIENTOS
        private void Limpiar()
        {
            txtindice.Text = "-1";
            txtid.Text = "0";
            txtdocumento.Text = "";
            txtnombrecompleto.Text = "";
            txtcorreo1.Text = "";
            txttelefono1.Text = "";
            txtrazonsocial.Text = "";
            txtcmp.Text = "";
            txtruc.Text = "";
            cboespecialidad.SelectedIndex = 0;
            cboestado.SelectedIndex = 0;
            txtcorreo2.Text = "";
            txttelefono2.Text = "";
            txtcodigocliente.Text = "";
            cbodepartamento.SelectedIndex= 0;
            cbodepartamentocomercial.SelectedIndex= 0;

        }
        private void buscar()
        {
            string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();
            if (dgvdata.Rows.Count > 0)
            {

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }
        private void txtbusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                buscar();
            }
        }
        private void frmClientes_Shown(object sender, EventArgs e)
        {
            txtdocumento.Focus();
        }
        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                //Obteniendo las dimensiones de la imagen
                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                //Centrando la imagen en la celda
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                //Si la acción del click puede continuar
                e.Handled = true;
            }
        }
        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {

                    //No debe estar en orden necesariamente
                    txtindice.Text = indice.ToString();
                    //Setear en campos la información del Cliente (obtener los datos de una fila y ponerlos en edicion)
                    txtid.Text = dgvdata.Rows[indice].Cells["Id"].Value.ToString();
                    txtcodigocliente.Text = dgvdata.Rows[indice].Cells["CodigoCliente"].Value.ToString(); //No se edita, solo se muestra
                   
                    txtdocumento.Text = dgvdata.Rows[indice].Cells["Documento"].Value.ToString();
                    txtnombrecompleto.Text = dgvdata.Rows[indice].Cells["NombreCompleto"].Value.ToString();
                    txtcorreo1.Text = dgvdata.Rows[indice].Cells["Correo1"].Value.ToString();
                    txttelefono1.Text = dgvdata.Rows[indice].Cells["Telefono1"].Value.ToString();
                    txtrazonsocial.Text = dgvdata.Rows[indice].Cells["RazonSocial"].Value.ToString();
                    txtcmp.Text = dgvdata.Rows[indice].Cells["CMP"].Value.ToString();
                    txtdireccion.Text = dgvdata.Rows[indice].Cells["Direccion"].Value.ToString();
                    txtnombrecomercial.Text = dgvdata.Rows[indice].Cells["NombreComercial"].Value.ToString();
                    txtdireccioncomercial.Text = dgvdata.Rows[indice].Cells["DireccionComercial"].Value.ToString();
                    txtruc.Text = dgvdata.Rows[indice].Cells["RUC"].Value.ToString();
                    //TXT DE PRUEBA PAA EL COMBOBOX COMO 0 Y -1
             //       cbodepartamento.ValueMember= "Piura"; //Comparar el valor devuelto

                    //Departamento = this.cbodepartamento.GetItemText(this.cbodepartamento.SelectedItem),

                    
                    

                    //Setear en el combobox el rol del Cliente oc es el elemento que recorre toda la lista
                    foreach (OpcionCombo oc in cboespecialidad.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["IdEspecialidad"].Value))
                        {
                            int indice_combo = cboespecialidad.Items.IndexOf(oc);
                            cboespecialidad.SelectedIndex = indice_combo;
                            break; //Para cuando lo encuentre debe terminar
                        }
                    }
                    foreach (OpcionCombo oc in cbotipodocumento.Items)
                    {

                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["IdTipoDocumento"].Value))
                        {
                            int indice_combo = cbotipodocumento.Items.IndexOf(oc);
                            cbotipodocumento.SelectedIndex = indice_combo;
                            break; //Para cuando lo encuentre debe terminar
                        }
                    }
                    foreach (OpcionCombo oc in cbotipocliente.Items)
                    {

                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["IdTipoCliente"].Value))
                        {
                            int indice_combo = cbotipocliente.Items.IndexOf(oc);
                            cbotipocliente.SelectedIndex = indice_combo;
                            break; //Para cuando lo encuentre debe terminar
                        }
                    }
                    txtcorreo2.Text = dgvdata.Rows[indice].Cells["Correo2"].Value.ToString();

                    txttelefono2.Text = dgvdata.Rows[indice].Cells["Telefono2"].Value.ToString();

                    //Setear en el combobox el rol del Cliente oc es el elemento que recorre toda la lista
                    foreach (OpcionCombo oc in cboestado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = cboestado.Items.IndexOf(oc);
                            cboestado.SelectedIndex = indice_combo;
                            break; //Para cuando lo encuentre debe terminar
                        }
                    }

                }
            }
        }
















        #endregion

        
    }
}
