namespace Gestion_des_Vacataires.Controle_Utlisateur
{
    partial class Filtre_Emploi_de_Temps
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.CbNom = new System.Windows.Forms.ComboBox();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(137, 116);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(146, 42);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Appliquer";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = " Nom de l\'Enseignant";
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.CbNom);
            this.gbFilter.Controls.Add(this.btnOk);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Location = new System.Drawing.Point(0, 3);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(474, 339);
            this.gbFilter.TabIndex = 3;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filtrer";
            // 
            // CbNom
            // 
            this.CbNom.FormattingEnabled = true;
            this.CbNom.Items.AddRange(new object[] {
            "1",
            "2"});
            this.CbNom.Location = new System.Drawing.Point(202, 69);
            this.CbNom.Name = "CbNom";
            this.CbNom.Size = new System.Drawing.Size(190, 28);
            this.CbNom.TabIndex = 3;
            // 
            // Filtre_Emploi_de_Temps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gbFilter);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ForeColor = System.Drawing.Color.RoyalBlue;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Filtre_Emploi_de_Temps";
            this.Size = new System.Drawing.Size(466, 347);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.ComboBox CbNom;
    }
}
