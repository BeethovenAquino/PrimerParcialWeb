﻿using BLL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlBancario.UI.Registros
{
    public partial class Depositos : System.Web.UI.Page

    {
        string condicion = "Select One";
        RepositorioBase<Deposito> repositorio = new RepositorioBase<Deposito>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {


                LlenaComboCuentaID();
                FechadateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

        }

        public Deposito LlenaClase(Deposito deposito)
        {
            int id;
            bool result = int.TryParse(DepositoIDTextbox.Text, out id);
            if (result == true)
            {
                deposito.DepositoID = id;
            }
            else
            {
                deposito.DepositoID = 0;
            }

            deposito.Concepto = ConceptoTextbox.Text;
            deposito.Fecha = Convert.ToDateTime(FechadateTime.Text);
            deposito.Monto = Convert.ToDecimal(MontoTexbox.Text.ToString());

            return deposito;
        }

        private void Limpiar()
        {
            DepositoIDTextbox.Text = "";
            LlenaComboCuentaID();
            ConceptoTextbox.Text = "";
            MontoTexbox.Text = "";
            FechadateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

        }

        void MostrarMensaje(TiposMensajes tipo, string mensaje)

        {

            ErrorLabel.Text = mensaje;

            if (tipo == TiposMensajes.Success)

                ErrorLabel.CssClass = "alert-success";

            else

                ErrorLabel.CssClass = "alert-danger";

        }

        private void LlenaComboCuentaID()
        {
            BLL.RepositorioBase<Cuentas> cuentas = new BLL.RepositorioBase<Cuentas>();
            DropDownList.Items.Clear();
            DropDownList.DataSource = cuentas.GetList(x => true);
            DropDownList.DataValueField = "CuentaID";
            DropDownList.DataTextField = "Nombre";
            DropDownList.DataBind();
        }

        protected void NuevoButton_Click(object sender, EventArgs e)
        {
            Limpiar();

        }

        protected void GuardarButton_Click(object sender, EventArgs e)
        {

            if (DropDownList.SelectedValue == condicion)
                return;


            Deposito deposito = new Deposito();
            bool paso = false;

            LlenaClase(deposito);
            //Validacion
            if (deposito.DepositoID == 0)

                paso = repositorio.Guardar(deposito);
            else
                paso = repositorio.Modificar(deposito);
            if (paso)

            {
                MostrarMensaje(TiposMensajes.Success, "Registro Exitoso!");
                Limpiar();

            }
            else
                MostrarMensaje(TiposMensajes.Error, "No fue posible Guardar el Registro");

            Limpiar();
        }

        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Deposito> repositorio = new RepositorioBase<Deposito>();

            int id = Convert.ToInt32(DepositoIDTextbox.Text);



            var usuario = repositorio.Buscar(id);



            if (usuario == null)

                MostrarMensaje(TiposMensajes.Error, "Registro no encontrado");

            else

                repositorio.Eliminar(id);
        }
    }
}