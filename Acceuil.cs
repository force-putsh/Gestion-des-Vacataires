using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_des_Vacataires
{
    public partial class Acceuil : Form
    {
        public Acceuil()
        {
            InitializeComponent();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnFin_Paint(object sender, PaintEventArgs e)
        {
            RondButton(btnFin, e);
        }

        private void btnDebut_Paint(object sender, PaintEventArgs e)
        {
            RondButton(btnDebut, e);
        }

        #region Mes Functions
        /// <summary>
        /// Donner la forme ronde à un Button
        /// </summary>
        /// <param name="button"></param>
        /// <param name="e"></param>
        public void RondButton(Button button,PaintEventArgs e)
        {
            GraphicsPath graphicsPath = new GraphicsPath();

            Rectangle rectangle = button.ClientRectangle;

            rectangle.Inflate(-10, -10);
            e.Graphics.DrawEllipse(Pens.Black, rectangle);

            rectangle.Inflate(1, 1);

            graphicsPath.AddEllipse(rectangle);

            button.Region = new Region(graphicsPath);
            
        }
        #endregion

        private void btnClose_Paint(object sender, PaintEventArgs e)
        {
            RondButton((Button)sender, e);
        }

        private void btnLogin_Paint(object sender, PaintEventArgs e)
        {
            RondButton((Button)(sender), e);
        }
    }
}
