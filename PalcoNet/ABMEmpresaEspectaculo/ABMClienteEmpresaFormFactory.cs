﻿using PalcoNet.Classes.Util.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.ABMEmpresaEspectaculo
{
    public partial class ABMClienteEmpresaFormFactory : Form
    {
        private Form callerForm;
        public ABMClienteEmpresaFormFactory(Form CallerForm)
        {
            callerForm = CallerForm;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigableFormUtil.ForwardTo(this, new ABMEmpresaEspectaculo.AltaEmpresa(this));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigableFormUtil.ForwardTo(this, new ABMEmpresaEspectaculo.ListadoEmpresa(this));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NavigableFormUtil.BackwardTo(this, callerForm);
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            NavigableFormUtil.ForwardTo(this, new ABMEmpresaEspectaculo.BajaEmpresa(this));
        }


    }
}
