using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_des_Vacataires.Controle_Utlisateur
{
    public partial class Filtre_Emploi_de_Temps : UserControl
    {
        HttpClient httpClient=new HttpClient();

        private DataGridView _dataGrid;
        public Filtre_Emploi_de_Temps(DataGridView dataGrid)
        {
            InitializeComponent();
            _dataGrid = dataGrid;
        }




    }
}
