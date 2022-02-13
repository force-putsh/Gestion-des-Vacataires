Create table Vacataire
(
	Id int primary key identity,
	Nom varchar(250),
	Prenom varchar(250),
	Email varchar(250),
	NumTel varchar(250),
	Password varchar(1000)
)

Create table Pointage
(
	Id int primary key identity,
	Date date,
	HeureDebut time,
	HeureFin time,
	IdVacataire int foreign key references Vacataire(Id),
	PathPhoto varchar(500),
)

create table EmploiDeTemps
(
	Id int primary key identity,
	Date date,
	NomCours varchar(500),
	HeureDebut time,
	HeureFin time,
	IdVacataire int foreign key references Vacataire(Id),
)

create table Contrat
(
	Id int primary key identity,
	NomCours varchar(250),
	NomVacataire varchar(250),
	IdVacataire int foreign key references Vacataire(Id),
	Duree int,
	SalaireHoraire money,
)
Create table Payement
(
	Id int primary key foreign key references Vacataire(Id),
	SalaireActuel Money,
	SalairePrevisionel Money
)