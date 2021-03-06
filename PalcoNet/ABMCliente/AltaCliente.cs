﻿using Classes.DatabaseConnection;
using Classes.Util;
using PalcoNet.Classes.Constants;
using PalcoNet.Classes.CustomException;
using PalcoNet.Classes.DatabaseConnection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFUtilites;
using PalcoNet.Classes.Util.Form;
using PalcoNet.Classes.Model;
using PalcoNet.Classes.Interfaces;
using PalcoNet.Classes.Misc;
using Classes.Configuration;

namespace PalcoNet.ABMCliente
{
    public partial class AltaCliente : Form
    {
        private Form CallerForm;
        private Usuario newUser;
        private IAccionPostCreacionUsuario accionPostCreacion = new NoVolverALogin();
        int HABILITADO;

        public AltaCliente(Form caller)
        {
            InitializeComponent();
            CallerForm = caller;
            HABILITADO = -1;
        }

        public AltaCliente(Form callerForm, Usuario newUser, IAccionPostCreacionUsuario accionPostCreacion) 
        {
            InitializeComponent();
            CallerForm = callerForm;
            this.newUser = newUser;
            this.accionPostCreacion = accionPostCreacion;
            HABILITADO = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string username="",password = "";
         
            if(!TextFieldUtils.IsValidNumericField(Verificador1,DNI,DigitoVerificador,NroDocumento,Telefono,Numero,Tarjeta)
                || !TextFieldUtils.IsValidTextField(Nombre,Apellido,Localidad))
            {
               
            }
            else{
            var cuil = Verificador1.Text + DNI.Text + DigitoVerificador.Text;
            if (!VerificarCamposNoVacios() && StringUtil.MailUtil.IsValidEmail(Mail.Text))
            {
                if (TextFieldUtils.CUIT.EsCuilValido(cuil) && NroDocumento.Text == DNI.Text)
                {
                    StoredProcedureParameterMap inputParameters = new StoredProcedureParameterMap();
                    StoredProcedureParameterMap userParameters = new StoredProcedureParameterMap();
                    inputParameters.AddParameter("@habilitado", true); // habilitado por defecto
                    inputParameters.AddParameter("@id_rol", 2); // 2 Rol Cliente
                    inputParameters.AddParameter("@nombre", Nombre.Text);
                    inputParameters.AddParameter("@apellido", Apellido.Text);
                    inputParameters.AddParameter("@tipo_documento", cboTipoDoc.Text);
                    inputParameters.AddParameter("@nro_documento", decimal.Parse(NroDocumento.Text));
                    inputParameters.AddParameter("@cuil", StringUtil.FormatCuil(cuil));
                    inputParameters.AddParameter("@mail", Mail.Text);
                    inputParameters.AddParameter("@telefono", Telefono.Text);
                    inputParameters.AddParameter("@direccion_calle", Calle.Text);
                    inputParameters.AddParameter("@numero_calle", decimal.Parse(Numero.Text));
                    if (Piso.Text == "")
                    {
                        inputParameters.AddParameter("@numero_piso", DBNull.Value);
                    }
                    else
                    {
                        inputParameters.AddParameter("@numero_piso", decimal.Parse(Piso.Text));
                    }
                    if (Departamento.Text == "")
                    {
                        inputParameters.AddParameter("@departamento", DBNull.Value);
                    }
                    else
                    {
                        inputParameters.AddParameter("@departamento", Departamento.Text);
                    }
                    inputParameters.AddParameter("@localidad", Localidad.Text);
                    inputParameters.AddParameter("@codigo_postal", Codigo_Postal.Text);
                    inputParameters.AddParameter("@fecha_de_nacimiento", Convert.ToDateTime(dtpFechaNacimiento.Text));
                    inputParameters.AddParameter("@fecha_de_creacion", ConfigurationManager.Instance().GetSystemDateTime());
                    inputParameters.AddParameter("@tarjeta", Tarjeta.Text);

                    if (newUser == null)
                    {   
                        //Si el usuario es creado desde el abm se genera el usuario y la contraseña de forma aleatoria
                        username = StringUtil.GenerateRandomUsername(Mail.Text,Convert.ToDateTime(dtpFechaNacimiento.Text).Year.ToString());
                        password = StringUtil.GenerateRandomPassword();
                        inputParameters.AddParameter("@username",username);
                        inputParameters.AddParameter("@password",password );
                    }
                    else
                    {
                        username = newUser.Username;
                        password = newUser.Password;
                        inputParameters.AddParameter("@username", newUser.Username);
                        inputParameters.AddParameter("@password", newUser.Password);
                    }

                    try
                    {
                        //Crear Usuario
                        userParameters.AddParameter("@username",username);
                        userParameters.AddParameter("@password",password);
                        userParameters.AddParameter("@idRol", 2);
                        userParameters.AddParameter("@intentos_login", HABILITADO);
                        ConnectionFactory.Instance().CreateConnection().ExecuteDataTableStoredProcedure(SpNames.AltaDeUsuario,userParameters);

                        //Crear Cliente
                        ConnectionFactory.Instance()
                                         .CreateConnection()
                                         .ExecuteDataTableStoredProcedure(SpNames.AltaCliente, inputParameters);
                        MessageBoxUtil.ShowInfo("Cliente agreagado exitosamente! El usuario para ingresar es:"+username+" y el password es:"+password);

                        accionPostCreacion.Do(this);
                    }
                    catch (StoredProcedureException ex) { MessageBox.Show(ex.Message); }
                }
                else { MessageBox.Show("Por favor verifique el cuil"); }
            }
           
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            TextFieldUtils.CleanAllControls(this);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            NavigableFormUtil.BackwardTo(this, CallerForm);
        }

        #region Auxilliary Methods

        public bool VerificarCamposNoVacios()
        {
            if (Nombre.Text == "" || Apellido.Text == "" || cboTipoDoc.Text == "" || NroDocumento.Text == ""
                || Mail.Text == "" || Telefono.Text == "" || Calle.Text == "" || Numero.Text == "" ||
                Codigo_Postal.Text == "" || Localidad.Text == "" || Verificador1.Text == "" || DNI.Text == ""
                || DigitoVerificador.Text == "" || Tarjeta.Text == "")
            {
                MessageBox.Show("Complete los campos necesarios");
                return true;
            }
            return false;
        }


        #endregion
    }
}
