using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_des_Vacataires.Data
{

    public class InfoEmploiDeTemps
    {
        HttpClient httpClient = new HttpClient();

        public InfoEmploiDeTemps()
        {
            httpClient.BaseAddress = new Uri("https://localhost:44365/api/");
        }

        public string cour { get; set; }
        public string Enseignant { get; set; }
        public string Date { get; set; }
        public string HeureDebut { get; set; }
        public string HeureFin { get; set; }

        #region Les Fonctions
        public List<InfoEmploiDeTemps> GetAllEmploiDeTemps()
        {
            string result = httpClient.GetStringAsync("EmploiDeTemps").Result;
            List<InfoEmploiDeTemps> emploiDeTemps = JsonConvert.DeserializeObject<List<InfoEmploiDeTemps>>(result);
            return emploiDeTemps;
        }

        public void ShowEmploiDeTemps(DataGridView dataGrid)
        {
            List<InfoEmploiDeTemps> emploiDeTemps= GetAllEmploiDeTemps();
            if (dataGrid.Rows.Count >= 0)
            {
                dataGrid.Rows.Clear();
                foreach (var item in emploiDeTemps)
                {
                    dataGrid.Rows.Add(item.cour, item.Enseignant, item.Date, item.HeureDebut, item.HeureFin);
                }
            }
            else
                foreach (var item in emploiDeTemps)
                    dataGrid.Rows.Add(item.cour, item.Enseignant, item.Date, item.HeureDebut, item.HeureFin);
        }

        public InfoEmploiDeTemps GetEmploiDeTempsById(int Id)
        {
            string result = httpClient.GetStringAsync("EmploiDeTemps/"+Id).Result;
            InfoEmploiDeTemps emploiDeTemps = JsonConvert.DeserializeObject<InfoEmploiDeTemps>(result);
            return emploiDeTemps;
        }

        #endregion
    }
}
