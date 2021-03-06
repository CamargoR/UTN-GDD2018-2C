﻿using Classes.DatabaseConnection;
using PalcoNet.Classes.Constants;
using PalcoNet.Classes.CustomException;
using PalcoNet.Classes.DatabaseConnection;
using PalcoNet.Classes.Util.Form;
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

namespace PalcoNet.ABMRol
{
    public partial class frmModificarNombre : Form
    {
        private decimal IdRol;
        private Form CallerForm;
        
        public frmModificarNombre(decimal IdRol,Form caller)
        {
            this.IdRol = IdRol;
            InitializeComponent();
            CallerForm = caller;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(!TextFieldUtils.IsAnyFieldEmpty(this))
            {
            StoredProcedureParameterMap inputParameters = new StoredProcedureParameterMap();
            inputParameters.AddParameter("@id_Rol",IdRol);
            inputParameters.AddParameter("@nombreRolNuevo",tbNombreNuevo.Text);
            try
            {
                ConnectionFactory.Instance()
                                 .CreateConnection()
                                 .ExecuteDataTableStoredProcedure(SpNames.ModificarRol,inputParameters);
                MessageBox.Show("Nombre del rol modificado exitosamente!");
                NavigableFormUtil.BackwardTo(this, CallerForm);
                
            }
            catch (StoredProcedureException ex) { MessageBox.Show(ex.Message); }
        }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigableFormUtil.BackwardTo(this, CallerForm);
        }
    }
}
