# ?? Gestion des Vacataires

Application de bureau Windows Forms pour la gestion des vacataires et leurs emplois du temps dans un établissement d'enseignement.

## ?? Description

**Gestion des Vacataires** est une application Windows développée en C# qui permet de gérer efficacement les vacataires (professeurs temporaires) et leurs emplois du temps. L'application offre une interface intuitive pour visualiser, filtrer et gérer les plannings des cours.

## ? Fonctionnalités

- ?? **Tableau de bord interactif** : Vue d'ensemble des emplois du temps
- ?? **Système de filtrage** : Recherche et filtrage avancé des emplois du temps
- ????? **Gestion des enseignants** : Suivi des vacataires et de leurs cours
- ?? **Planning des cours** : Visualisation des horaires (date, heure de début, heure de fin)
- ?? **API REST** : Communication avec un serveur backend pour la gestion des données
- ?? **Base de données** : Stockage persistant via SQL Server

## ??? Technologies Utilisées

- **Framework** : .NET 10
- **Interface** : Windows Forms (WinForms)
- **Base de données** : SQL Server / Entity Framework 6.5.1
- **API** : ASP.NET Core Web API
- **Sérialisation** : Newtonsoft.Json 13.0.3
- **Connectivité** : 
  - Microsoft.Data.SqlClient 5.2.2
  - MySql.Data 9.1.0

### Dépendances NuGet

```xml
- EntityFramework 6.5.1
- Microsoft.Data.SqlClient 5.2.2
- MySql.Data 9.1.0
- Newtonsoft.Json 13.0.3
- BouncyCastle.Cryptography 2.4.0
- Google.Protobuf 3.28.2
- K4os.Compression.LZ4 1.3.8
- K4os.Compression.LZ4.Streams 1.3.8
- K4os.Hash.xxHash 1.0.8
- System.Configuration.ConfigurationManager 9.0.0
```

## ?? Prérequis

- Windows 10 ou supérieur
- .NET 10 Runtime
- SQL Server (LocalDB ou instance complète)
- Visual Studio 2022 ou supérieur (pour le développement)

## ?? Installation

### 1. Cloner le repository

```bash
git clone https://github.com/force-putsh/Gestion-des-Vacataires.git
cd Gestion-des-Vacataires
```

### 2. Configuration de la base de données

Modifiez la chaîne de connexion dans le fichier `App.config` :

```xml
<connectionStrings>
    <add name="DbGestionnaireStagiaireEntities" 
         connectionString="Data Source=VOTRE_SERVEUR;Initial Catalog=Gestion_Etudiants;Integrated Security=True;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;" 
         providerName="Microsoft.Data.SqlClient" />
</connectionStrings>
```

### 3. Configuration de l'API

Dans le fichier `Data\InfoEmploiDeTemps.cs`, vérifiez l'URL de l'API :

```csharp
httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
```

?? **Important** : Assurez-vous que votre API backend est démarrée et accessible à cette adresse.

### 4. Restauration et compilation

```bash
dotnet restore
dotnet build
```

### 5. Lancement de l'application

```bash
dotnet run
```

Ou ouvrez le fichier `.sln` dans Visual Studio et appuyez sur F5.

## ??? Structure du Projet

```
Gestion-des-Vacataires/
??? Data/
?   ??? InfoEmploiDeTemps.cs      # Gestion des données d'emploi du temps
??? Controle Utlisateur/
?   ??? UCDashbord.cs              # Contrôle utilisateur du tableau de bord
?   ??? Filtre Emploi de Temps.cs # Contrôle de filtrage
??? Acceuil.cs                     # Formulaire d'accueil
??? Dashbord.cs                    # Formulaire principal
??? Program.cs                     # Point d'entrée
??? App.config                     # Configuration de l'application
??? Gestion des Vacataires.csproj # Fichier de projet
```

## ?? Utilisation

### Démarrage
L'application démarre directement sur le tableau de bord principal qui affiche l'ensemble des emplois du temps.

### Consultation des emplois du temps
- Les emplois du temps s'affichent automatiquement dans une grille (DataGridView)
- Chaque ligne contient : Cours, Enseignant, Date, Heure de début, Heure de fin

### Filtrage
- Cliquez sur le bouton de recherche pour accéder aux options de filtrage
- Appliquez des critères pour affiner votre recherche

## ?? Configuration Avancée

### Entity Framework
Le projet utilise Entity Framework 6.5.1 avec SQL Server Provider. La configuration se trouve dans `App.config`.

### API Backend
L'application communique avec une API REST qui doit implémenter au minimum :
- `GET /api/EmploiDeTemps` : Récupération de tous les emplois du temps

## ?? Contribution

Les contributions sont les bienvenues ! Pour contribuer :

1. Forkez le projet
2. Créez une branche pour votre fonctionnalité (`git checkout -b feature/AmazingFeature`)
3. Committez vos changements (`git commit -m 'Add some AmazingFeature'`)
4. Poussez vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrez une Pull Request

## ?? Licence

Ce projet est sous licence libre. Voir le fichier `LICENSE` pour plus de détails.

## ?? Auteurs

- **force-putsh** - [GitHub](https://github.com/force-putsh)

## ?? Signalement de bugs

Si vous rencontrez un problème, veuillez ouvrir une issue sur [GitHub Issues](https://github.com/force-putsh/Gestion-des-Vacataires/issues).

## ?? Contact

Pour toute question ou suggestion, n'hésitez pas à ouvrir une discussion sur GitHub.

---

? N'oubliez pas de mettre une étoile si ce projet vous a été utile !
