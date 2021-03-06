﻿using Classes.DatabaseConnection;
using PalcoNet.Classes.Constants;
using PalcoNet.Classes.CustomException;
using PalcoNet.Classes.DatabaseConnection;
using PalcoNet.Classes.Model;
using PalcoNet.Classes.Util;
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
using PalcoNet.Classes.Repository;
using Classes.Configuration;

namespace PalcoNet.Comprar
{
    public partial class Comprar : Form
    {
        private Form previousForm;
        private RubroRepository rubroRepository;

        //public Comprar()
        //{
        //    InitializeComponent();
        //    inicializarPantalla();
        //}

        public Comprar(Form previousForm)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            inicializarPantalla();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            NavigableFormUtil.BackwardTo(this, previousForm);
        }

        private void ComprarForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

      
        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Text = "";
            LbCategoria.Items.Clear();
            cmbCategoria.SelectedIndex = 0;
            cmbCategoria.Focus();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if ((DateTime.Compare(Convert.ToDateTime(dtpFechaInicial.Text), Convert.ToDateTime(dtpFechaFinal.Text)) > 0)
                || (DateTime.Compare(Convert.ToDateTime(dtpFechaFinal.Text), Convert.ToDateTime(dtpFechaInicial.Text)) < 0))
            {
                MessageBox.Show("Verifique las fechas");
            }
            else
            {
                try
                {
                    this.Hide();
                    List<String> categorias = new List<String>();
                    foreach (var cat in LbCategoria.Items)
                    {
                        categorias.Add(cat.ToString());
                    }
                    ResPublicacion publicacion = new ResPublicacion(this, categorias, txtDescripcion.Text, DateTimeUtil.Of(dtpFechaInicial.Value, dtpHoraInicial.Value).ToString("yyyy-dd-MM HH:mm:ss"), DateTimeUtil.Of(dtpFechaFinal.Value, dtpHoraFinal.Value).ToString("yyyy-dd-MM HH:mm:ss"));
                    publicacion.Show();


                }
                catch (StoredProcedureException ex)
                {
                    MessageBoxUtil.ShowError("Error al mostrar las publicaciones.");
                }

            }
        }

        private void BtnQuitar_Click(object sender, EventArgs e)
        {
           LbCategoria.Items.Remove(LbCategoria.SelectedItem);
        }

        private void LbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(cmbCategoria.Text != "" &&  !LbCategoria.Items.Contains(cmbCategoria.Text))
            LbCategoria.Items.Add(cmbCategoria.Text) ;
        }
    #region inicializar
        private void inicializarPantalla()
        {
           
            DataSet ds = ConnectionFactory.Instance()
                                              .CreateConnection()
                                              .ExecuteDataSetSqlQuery("SELECT descripcion FROM LOS_DE_GESTION.Rubro", "descripcion");

            cmbCategoria.DisplayMember = "descripcion";
            cmbCategoria.DataSource = ds.Tables["descripcion"];
          cmbCategoria.SelectedIndex = 0;
          
          dtpFechaInicial.Value =ConfigurationManager.Instance().GetSystemDateTime();
          dtpFechaFinal.Value = ConfigurationManager.Instance().GetSystemDateTime();
          dtpHoraFinal.Value = ConfigurationManager.Instance().GetSystemDateTime();
          dtpHoraInicial.Value = ConfigurationManager.Instance().GetSystemDateTime();

        }
    #endregion



    }
}
