﻿using Classes.DatabaseConnection;
using PalcoNet.Classes.Constants;
using PalcoNet.Classes.CustomException;
using PalcoNet.Classes.DatabaseConnection;
using PalcoNet.Classes.Model;
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
    public partial class AltaRol : Form
    {
        private Form CallerForm;
        private decimal IdRol;
        public AltaRol(Form caller)
        {
            InitializeComponent();
            CallerForm = caller;
            DataTable dt = new DataTable();
            try
            {
             
                dt = ConnectionFactory.Instance().CreateConnection()
                        .ExecuteDataTableSqlQuery("SELECT id_Funcionalidad,nombre FROM LOS_DE_GESTION.Funcionalidad");
               
              
                dgvFuncionalidades.AllowUserToAddRows = false;
                dgvFuncionalidades.ReadOnly = true;
                dgvFuncionalidades.DataSource = dt;
                dgvFuncionalidades.Columns[0].Visible = false;
            }
            catch (SqlQueryException ex) { MessageBox.Show(ex.Message); }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!TextFieldUtils.IsAnyFieldEmpty(this))
            {
                
                IdRol = ConnectionFactory.Instance().CreateConnection().ExecuteSingleOutputSqlQuery<decimal>("SELECT TOP 1 (id_Rol+1) FROM LOS_DE_GESTION.Rol ORDER BY id_Rol DESC ");
                StoredProcedureParameterMap inputParameters = new StoredProcedureParameterMap();
                inputParameters.AddParameter("@nombreRol", tbRolNombre.Text);
                inputParameters.AddParameter("@id_rol", IdRol);
                inputParameters.AddParameter("@habilitado", cbHabilitado.Checked);
                try
                {
                    ConnectionFactory.Instance()
                                     .CreateConnection()
                                     .ExecuteDataTableStoredProcedure(SpNames.AltaRol,inputParameters);
                   
                    inputParameters.RemoveParameters();
                    
                    foreach (DataGridViewCell c in dgvFuncionalidades.SelectedCells)
                    {
                        inputParameters.AddParameter("@id_Rol", IdRol);
                    
                        decimal id_funcionalidad = ConnectionFactory.Instance()
                                                                    .CreateConnection()
                                                                    .ExecuteSingleOutputSqlQuery<decimal>( @"SELECT id_Funcionalidad FROM LOS_DE_GESTION.Funcionalidad
                                                                                                           WHERE nombre=" + "'" + c.Value.ToString() + "'");
                        inputParameters.AddParameter("@funcionalidadRol",id_funcionalidad);
                        ConnectionFactory.Instance()
                                         .CreateConnection()
                                         .ExecuteDataTableStoredProcedure(SpNames.AgregarFuncionalidadRol, inputParameters);
                        inputParameters.RemoveParameters();
                    }
                                    
                    MessageBox.Show("Rol dado de alta correctamente!");
                    NavigableFormUtil.BackwardTo(this, CallerForm);
                }
                catch (StoredProcedureException ex) { MessageBox.Show(ex.Message); }
            }
            else { MessageBox.Show("Por favor rellena todos los campos"); }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            NavigableFormUtil.BackwardTo(this, CallerForm);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            NavigableFormUtil.BackwardTo(this, CallerForm);
        }
    }
}
