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
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
        }

        public string cour { get; set; }
        public string Enseignant { get; set; }
        public string Date { get; set; }
        public string HeureDebut { get; set; }
        public string HeureFin { get; set; }

        #region Les Fonctions

        /// <summary>
        /// Recupère Tous les emplois de temps de l'établissement
        /// </summary>
        /// <returns></returns>
        public List<InfoEmploiDeTemps> GetAllEmploiDeTemps()
        {
            string result = httpClient.GetStringAsync("EmploiDeTemps").Result;
            List<InfoEmploiDeTemps> emploiDeTemps = JsonConvert.DeserializeObject<List<InfoEmploiDeTemps>>(result);
            return emploiDeTemps;
        }
        /// <summary>
        /// Affiche Les Emplois de temps dans une DataGridView passée en paramètre
        /// </summary>
        /// <param name="dataGrid"></param>
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

        /// <summary>
        /// Affiche Les Emplois de temps par ensaeignant dans une DataGridView passée en paramètre
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="Id"></param>
        public void ShowEmploiDeTemps(DataGridView dataGrid,int Id)
        {
            List<InfoEmploiDeTemps> emploiDeTemps = GetEmploiDeTempsById(Id);
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

        /// <summary>
        /// Recupère les emplois de temps par Enseignant via l'identifiant passé en paramètre
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<InfoEmploiDeTemps> GetEmploiDeTempsById(int Id)
        {
            string result = httpClient.GetStringAsync("EmploiDeTemps/"+Id).Result;
            List<InfoEmploiDeTemps> emploiDeTemps = JsonConvert.DeserializeObject<List<InfoEmploiDeTemps>>(result);
            return emploiDeTemps;
        }

        #endregion
    }
}
