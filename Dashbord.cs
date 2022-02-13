using Gestion_des_Vacataires.Controle_Utlisateur;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_des_Vacataires
{
    public partial class Dashbord : Form
    {
        public Dashbord()
        {
            InitializeComponent();
        }

        private void Dashbord_Load(object sender, EventArgs e)
        {
            UCDashbord uCDashbord = new UCDashbord();
            uCDashbord.Dock = DockStyle.Fill;
            panel3.Controls.Add(uCDashbord);
        }
    }
}
