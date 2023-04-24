/*Удаление БД с тем же названием (если есть)*/
GO
	PRINT '==================================База Данных======================================='
	USE master
		--IF EXISTS(SELECT * FROM SYSOBJECTS WHERE lower(NAME) = LOWER('VehicleRental') AND TYPE = 'u')
		--BEGIN
			DROP DATABASE VehicleRental
			PRINT 'Дропнул БД'
		--END ELSE PRINT 'Error: Не удалось дропнуть БД'
GO
	IF EXISTS (SELECT * FROM master.INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = 'CreateDataBase')
	BEGIN
		DROP PROCEDURE CreateDataBase
		PRINT 'Дропнул Процедуру создания БД'
	END ELSE PRINT 'Error: Не удалось дропнуть процедуру создания БД'
GO
	CREATE PROCEDURE CreateDataBase (@DBName NVARCHAR(max), @DBPath NVARCHAR(max))
	AS
	BEGIN
		DECLARE @SQL NVARCHAR(MAX) = 'CREATE DATABASE ' + @DBName + ' ON PRIMARY ' +
		'( NAME = ' + @DBName + ', ' +
		' FILENAME = ''' + @DBPath + '\' + @DBName + '.mdf'' , ' +
		' SIZE = 5MB , MAXSIZE = UNLIMITED, FILEGROWTH = 5%) ' +
		' LOG ON ' +
		'( NAME = ' + @DBName + '_Log, ' +
		' FILENAME = ''' + @DBPath + '\' + @DBName + '_Log.ldf'', ' +
		' SIZE = 1MB , MAXSIZE = UNLIMITED, FILEGROWTH = 1%)'
		EXEC sp_executesql @SQL
	END
	PRINT 'Создал Процедуру создания БД'


/*Создание БД*/
GO 
	EXEC CreateDataBase @DBName = 'VehicleRental', @DBPath = 'C:\Users\Public'
	PRINT 'Создал БД'

/*Создание таблиц*/
GO 
	PRINT '==================================Таблицы======================================='
	USE VehicleRental

	CREATE TABLE Rental_Users
	(
		ID_User INT IDENTITY(1,1) NOT NULL,
		UserLogin NVARCHAR(120) UNIQUE NOT NULL,
		UserEMail NVARCHAR(120) UNIQUE NOT NULL,
		UserPassword NVARCHAR(120) NOT NULL,
		UserStatus INT NOT NULL,
		UserSurname NVARCHAR(120) NOT NULL,
		UserName NVARCHAR(120) NOT NULL,
		UserMiddleName NVARCHAR(120) NOT NULL,
		UserBirthDay DATE NOT NULL,
		--isAdmin BIT DEFAULT 0 NOT NULL
		PRIMARY KEY (ID_User)
	)
	PRINT 'Создал Таблицу Rental_Users'

	CREATE TABLE Rental_Admins
	(
		ID_Admin INT IDENTITY(1,1) NOT NULL,
		ID_User INT NOT NULL
		FOREIGN KEY (ID_User) REFERENCES Rental_Users (ID_User)
	)

	PRINT 'Создал Таблицу Rental_Admins'


	CREATE TABLE DriversLicences
	(
		ID_DriverLicence CHAR(10) PRIMARY KEY NOT NULL 
			CHECK (
				ID_DriverLicence LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
				),
		ReceiptDate SMALLDATETIME NOT NULL,
		ID_User INT NOT NULL
	)
	PRINT 'Создал Таблицу DriversLicences'

	CREATE TABLE Categories 
	(
		ID_Category INT IDENTITY(1,1) NOT NULL,
		ID_DriverLicence CHAR(10) NOT NULL 
			CHECK (
				ID_DriverLicence LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
				),
		Category CHAR(3) NOT NULL,
		ReceiptDate SMALLDATETIME NOT NULL,
		EndDate SMALLDATETIME NOT NULL
		PRIMARY KEY (ID_Category)
	)
	PRINT 'Создал Таблицу Categories'

	CREATE TABLE Classes
	(
		Class CHAR(20) PRIMARY KEY NOT NULL 
	)

	PRINT 'Создал Таблицу Classes'


	CREATE TABLE Vehicles 
	(
		ID_Vehicle INT IDENTITY(1,1) NOT NULL,
		PricePerHour MONEY NOT NULL,
		Class CHAR(20) NOT NULL,
		CarPicture VARBINARY(MAX)
		PRIMARY KEY (ID_Vehicle)
	)
	PRINT 'Создал Таблицу Vehicles'

	CREATE TABLE VehicleCoordinates
	(
		ID_Coodinates INT IDENTITY(1,1)  NOT NULL,
		ID_Vehicle INT NOT NULL,
		Longitude FLOAT NOT NULL,
		Latitude FLOAT NOT NULL,
		StayDateTime DATETIME NOT NULL
		PRIMARY KEY (ID_Coodinates)
	)
	PRINT 'Создал Таблицу VehicleCoordinates'

	CREATE TABLE Rentals
	(
		ID_Rental INT IDENTITY(1,1)  NOT NULL,
		ID_DriverLicence CHAR(10) NOT NULL 
			CHECK (
				ID_DriverLicence LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
				),
		ID_Vehicle INT NOT NULL,
		StartDate SMALLDATETIME NOT NULL,
		RentalTime TIME NOT NULL,
		CountOfHours FLOAT NOT NULL
		PRIMARY KEY (ID_Rental)
	)
	PRINT 'Создал Таблицу Rentals'

	CREATE TABLE TrafficAccidentTypes
	(
		ID_TrafficAccidentType INT IDENTITY(1,1) NOT NULL,
		TrafficAccidentTypeName NVARCHAR(60)
		PRIMARY KEY (ID_TrafficAccidentType)
	)
	PRINT 'Создал Таблицу TrafficAccidentTypes'

	CREATE TABLE TrafficAccidents
	(
		ID_TrafficAccident INT IDENTITY(1,1) NOT NULL,
		ID_Vehicle INT NOT NULL,
		ID_DriverLicence CHAR(10) NOT NULL 
			CHECK (
				ID_DriverLicence LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
				),
		ID_TrafficAccidentType INT NOT NULL,
		Damage NVARCHAR(1) NOT NULL,
		TrafficAccidentDescription NVARCHAR(MAX) NOT NULL
		 PRIMARY KEY (ID_TrafficAccident)
	)
	PRINT 'Создал Таблицу TrafficAccidents'

	--CREATE TABLE VehiclesPassports
	--(
	--	ID_Vehicle INT NOT NULL,
	--	ID_VehiclePassport CHAR(10) NOT NULL CHECK(ID_VehiclePassport LIKE '[0-9][0-9][А-Я][А-Я][0-9][0-9][0-9][0-9][0-9][0-9]'),
	--	VIN CHAR(14) PRIMARY KEY NOT NULL,
	--	Vehicle_Brand NVARCHAR(100) NOT NULL,
	--	Vehicle_Category CHAR(6) NOT NULL 
	--		CHECK (
	--			Vehicle_Category = 'A' 
	--			OR Vehicle_Category = 'B' 
	--			OR Vehicle_Category = 'C' 
	--			OR Vehicle_Category = 'D' 
	--			OR Vehicle_Category = 'прицеп'
	--			),
	--	YearOfManufacture INT NOT NULL
	--		CHECK (
	--			YearOfManufacture >= 1970 
	--			AND YearOfManufacture <= 2100
	--			),
	--	ModelAndEngineNumber CHAR(30) NOT NULL,
	--	Chassis CHAR(100) NOT NULL,
	--	BodyNo CHAR(100) NOT NULL,
	--	BodyColor CHAR(50) NOT NULL,
	--	Horsepower_kW FLOAT NOT NULL,
	--	EngineDisplacementCmCubic INTEGER NOT NULL,
	--	EngineType CHAR(50) NOT NULL,
	--	PermissibleMaximumWeightKg INTEGER NOT NULL,
	--	WeightWithoutLoadKg INTEGER NOT NULL,
	--	ManufacturerOrganizationOrCountry CHAR(100) NOT NULL,
	--	VehicleTypeApproval CHAR(123) NOT NULL
	--		CHECK (
	--			  VehicleTypeApproval LIKE '% % %'
	--		),
	--	ExportCountryOfVehicle CHAR(100) NOT NULL,
	--	Series_TD_Number_TPO_Number CHAR(60) NOT NULL
	--		CHECK (
	--			   Series_TD_Number_TPO_Number 
	--			   LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]/[0-9][0-9][0-9][0-9][0-9][0-9]/[0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
	--			  ),
	--	CustomsRestrictions NVARCHAR(256) DEFAULT NULL,
	--	VehicleOwner CHAR(120) NOT NULL,
	--	VehicleOwnerAddress CHAR(256) NOT NULL,
	--	OrganizationIssuedPassport CHAR(256) NOT NULL,
	--	OrganizationAddress CHAR(256) NOT NULL,
	--	DateOfThePassport DATE NOT NULL
	--)
	--PRINT 'Создал Таблицу VehiclesPassports'


GO
	USE VehicleRental

	CREATE TABLE VehicleRegistrCertificates
	(
		CertificateSeries INT NOT NULL,
		CertificateNumber INT NOT NULL,
		ID_Vehicle INT NOT NULL,
		VIN CHAR(14) NOT NULL,
		RegistrationNum CHAR(10) UNIQUE NOT NULL 
			CHECK (
				   RegistrationNum 
				   LIKE '[А-Я][0-9][0-9][0-9][А-Я][А-Я][0-9][0-9][0-9]'
				  ),
		Brand NVARCHAR(100) NOT NULL,
		Mark NVARCHAR(100) NOT NULL,
		Vehicle_Type VARCHAR(100) NOT NULL,
		Vehicle_Category VARCHAR(6) NOT NULL 
			CHECK (
				Vehicle_Category = 'A' 
				OR Vehicle_Category = 'B' 
				OR Vehicle_Category = 'C' 
				OR Vehicle_Category = 'D' 
				OR Vehicle_Category = 'прицеп'
				),
		ManufactureYear INT NOT NULL,
		Chassis VARCHAR(100) NOT NULL,
	    BodyNo VARCHAR(40) NOT NULL,
		Color VARCHAR(120) NOT NULL,
		EngineHP_kW FLOAT NOT NULL,
		EcologicalClass CHAR(100) NOT NULL,
		--VehiclePassportSeries CHAR(4) NOT NULL 
		--	CHECK (
		--		   VehiclePassportSeries LIKE '[0-9][0-9][А-Я][А-Я]'
		--		  ),
		--VehiclePassportNum CHAR(6) NOT NULL 
		--	CHECK (
		--		   VehiclePassportNum LIKE '[0-9][0-9][0-9][0-9][0-9][0-9]'
		--		  ),
		PermissibleMaximumWeightKg INTEGER NOT NULL,
		WeightWithoutLoadKg INTEGER NOT NULL,
		DateOfRecord DATE NOT NULL,
		PRIMARY KEY (CertificateSeries, CertificateNumber)
	)

	PRINT 'Создал Таблицу VehicleRegistrCertificates'

	
/*Связи*/
GO
	PRINT '==================================Связи======================================='
	USE VehicleRental

	ALTER TABLE DriversLicences
	ADD CONSTRAINT FK_DriversLicenses_Rental_Users 
		FOREIGN KEY (ID_User) REFERENCES Rental_Users (ID_User)
		ON DELETE CASCADE
		ON UPDATE CASCADE

	PRINT 'Создал Ключ FK_DriversLicenses_Rental_Users'

GO
	USE VehicleRental
	
	ALTER TABLE Categories
	ADD CONSTRAINT FK_Categories_DriversLicenses 
		FOREIGN KEY (ID_DriverLicence) REFERENCES DriversLicences (ID_DriverLicence)
		ON DELETE CASCADE
		ON UPDATE CASCADE

	PRINT 'Создал Ключ FK_Categories_DriversLicenses'
GO
	USE VehicleRental

	ALTER TABLE Rentals
	ADD CONSTRAINT FK_Rentals_DriversLicences
		FOREIGN KEY (ID_DriverLicence) REFERENCES DriversLicences (ID_DriverLicence)
		ON DELETE CASCADE
		ON UPDATE CASCADE

	PRINT 'Создал Ключ FK_Rentals_DriversLicences'
GO	
	USE VehicleRental

	ALTER TABLE Rentals
	ADD CONSTRAINT FK_Rentals_Vehicles
		FOREIGN KEY (ID_Vehicle) REFERENCES Vehicles (ID_Vehicle)
		ON DELETE CASCADE
		ON UPDATE CASCADE
	PRINT 'Создал Ключ FK_Rentals_Vehicles'


GO
	USE VehicleRental
	
	ALTER TABLE VehicleCoordinates
	ADD CONSTRAINT FK_VehicleCoordinates_Vehicles
		FOREIGN KEY (ID_Vehicle) REFERENCES Vehicles (ID_Vehicle)
		ON DELETE CASCADE
		ON UPDATE CASCADE
	PRINT 'Создал Ключ FK_VehicleCoordinates_Vehicles'


GO
	USE VehicleRental
	
	ALTER TABLE TrafficAccidents
	ADD CONSTRAINT FK_TrafficAccidents_Vehicles
		FOREIGN KEY (ID_Vehicle) REFERENCES Vehicles (ID_Vehicle)
		ON DELETE CASCADE
		ON UPDATE CASCADE
	PRINT 'Создал Ключ FK_TrafficAccidents_Vehicles'

GO
	USE VehicleRental
	
	ALTER TABLE TrafficAccidents
	ADD CONSTRAINT FK_TrafficAccidents_TrafficAccidentTypes
		FOREIGN KEY (ID_TrafficAccidentType) REFERENCES TrafficAccidentTypes (ID_TrafficAccidentType)
		ON DELETE CASCADE
		ON UPDATE CASCADE
	PRINT 'Создал Ключ FK_TrafficAccidents_TrafficAccidentTypes'

GO
	USE VehicleRental
	
	ALTER TABLE TrafficAccidents
	ADD CONSTRAINT FK_TrafficAccidents_DriversLincences
		FOREIGN KEY (ID_DriverLicence) REFERENCES DriversLicences (ID_DriverLicence)
		ON DELETE CASCADE
		ON UPDATE CASCADE
	PRINT 'Создал Ключ FK_TrafficAccidents_DriversLincences'
	
--GO 
--	USE VehicleRental

--	ALTER TABLE VehiclesPassports
--	ADD CONSTRAINT FK_Passports_Vehicles_Vehicles 
--		FOREIGN KEY (ID_Vehicle) REFERENCES Vehicles (ID_Vehicle)
--		ON DELETE CASCADE
--		ON UPDATE CASCADE
--	PRINT 'Создал Ключ FK_VehiclesPassports_Vehicles'

GO
	USE VehicleRental

	ALTER TABLE VehicleRegistrCertificates
	ADD CONSTRAINT FK_VehicleRegistrCertificates_Vehicles
		FOREIGN KEY (ID_Vehicle) 
		REFERENCES Vehicles (ID_Vehicle)

	PRINT 'Создал Ключ FK_VehicleRegistrCertificates_Vehicles'
GO
	USE VehicleRental
	ALTER TABLE Vehicles 
	ADD CONSTRAINT FK_Vehicles_Classes
		FOREIGN KEY (Class) REFERENCES Classes (Class)
		ON DELETE CASCADE
		ON UPDATE CASCADE
	PRINT 'Создал Ключ FK_Vehicles_Classes'


/*Роли*/

GO
	PRINT '==================================Роли======================================='
	USE VehicleRental

	--REVOKE CREATE ON SCHEMA FROM public
	--REVOKE ALL ON VehicleRental FROM public
	--PRINT 'Пробую дропнуть роль Rental_Customer'
	--DROP ROLE Rental_Customer


	--CREATE ROLE Rental_Customer
	--PRINT 'Создал роль Rental_Customer'

/*Хранимые процедуры*/

GO
	PRINT '==================================Хранимые процедуры======================================='
GO
	CREATE PROCEDURE REG_USER
		@UserLogin NVARCHAR(MAX),
		@UserEmail NVARCHAR(MAX),
		@UserPassword NVARCHAR(MAX),
		@UserSurname NVARCHAR(MAX),
		@UserName NVARCHAR(MAX),
		@UserMiddleName NVARCHAR(MAX),
		@UserBirthDayDate NVARCHAR(10)
	AS
	BEGIN
		--DECLARE @SqlCommand1 NVARCHAR(MAX) =
		--'CREATE LOGIN '+ @UserLogin + ' WITH PASSWORD = '''+@UserPassword+''' '+
		--'CREATE USER USER_'+ @UserLogin + ' FOR LOGIN '+ @UserLogin
		--EXEC (@SqlCommand1)

		--DECLARE @ID NVARCHAR(MAX) = USER_ID('USER_'+@UserLogin) 
		
		--PRINT @SqlCommand1

		--DECLARE @SqlCommand2 NVARCHAR(MAX) = 
		--'USE VehicleRental '+
		--'INSERT Rental_Users VALUES ('+
		--	@ID+', '''+@UserLogin+''', '''+@UserEmail+''', '''+@UserPassword+''', '+
		--	'1, '''+@UserSurname+''', '''+@UserName+''', '''+@UserMiddleName+''', '''+@UserBirthDayDate+''')'

		DECLARE @SqlCommand2 NVARCHAR(MAX) = 
		'USE VehicleRental '+
		'INSERT INTO Rental_Users '+
		'(UserLogin, UserEMail, UserPassword, UserStatus, UserSurname, UserName, UserMiddleName, UserBirthDay) VALUES ('+
			''''+@UserLogin+''', '''+@UserEmail+''', '''+@UserPassword+''', '+
			'1, '''+@UserSurname+''', '''+@UserName+''', '''+@UserMiddleName+''', '''+@UserBirthDayDate+''')'

		EXEC (@SqlCommand2)
		
		--PRINT @SqlCommand2

		--DECLARE @SqlCommand3 NVARCHAR(MAX) =
		--'USE VehicleRental '+
		--'ALTER ROLE Rental_Customer ADD MEMBER USER_'+@UserLogin
		--EXEC (@SqlCommand3)

		--PRINT @SqlCommand3
	END
GO
	PRINT 'Создал Хранимую процедуру REG_USER'


--GO
--	CREATE PROCEDURE CHECK_USER
--	@UserLogin NVARCHAR(MAX),
--	@UserPassword NVARCHAR(MAX)
--	AS
--	BEGIN
--		RETURN 
--			(SELECT COUNT(*) 
--				FROM Rental_Users 
--			WHERE UserLogin = @UserLogin AND UserPassword = @UserPassword)
--	END

GO
	CREATE PROCEDURE GetDriverLicenceByUserID(
		@User_ID INT
	)
	AS
	BEGIN
		SELECT TOP(1) * 
		FROM DriversLicences
		WHERE ID_User = @User_ID
		ORDER BY ReceiptDate DESC
	END
GO	
	PRINT 'Создал Хранимую процедуру GetDriverLicenceByUserID'

GO 
	CREATE PROCEDURE AddDriverLicenceToUser
	(
		@User_ID INT,
		@DriverLicence CHAR(10),
		@ReceiptDate SMALLDATETIME
	)
	AS
	BEGIN 
		INSERT DriversLicences VALUES
		(@DriverLicence, @ReceiptDate, @User_ID)
	END 

GO
	PRINT 'Создал Хранимую процедуру AddDriverLicenceToUser'

GO 
	CREATE PROCEDURE AddCategoryToDriverLicence
	(
		@DriverLicence_ID CHAR(10),
		@Category CHAR(3),
		@ReceiptDate SMALLDATETIME,
		@EndDate SMALLDATETIME
	)
	AS
	BEGIN
		INSERT Categories VALUES
		(@DriverLicence_ID, @Category, @ReceiptDate, @EndDate)
	END

GO
	PRINT 'Создал Хранимую процедуру AddCategoryToDriverLicence'


GO
	PRINT '==================================Функции======================================='
GO
	CREATE FUNCTION GetCoordinatesFunc(
		@Vehicle_ID INT
	)
	RETURNS table
	AS
	RETURN(
		SELECT TOP(1) *
		FROM VehicleCoordinates
		WHERE VehicleCoordinates.ID_Vehicle = @Vehicle_ID
		ORDER BY StayDateTime DESC
	)

GO
	PRINT 'Создал Функцию GetCoordinatesFunc'

GO	
	CREATE FUNCTION GetVehicleStatus(
		@Vehicle_ID INT
	)
	RETURNS char(8)
	AS
	BEGIN
		IF NOT EXISTS (SELECT *, 
			DATEADD(HOUR, ABS(DATEDIFF(HOUR,RentalTime,0))+ABS(CountOfHours),Convert(datetime, StartDate, 102)) as rt
					FROM Rentals 
					WHERE DATEADD(HOUR, ABS(DATEDIFF(HOUR,RentalTime,0))+ABS(CountOfHours),Convert(datetime, StartDate, 0)) 
					> GETDATE() 
					AND ID_Vehicle = @Vehicle_ID
					AND YEAR(DATEADD(HOUR, ABS(DATEDIFF(HOUR,RentalTime,0))+ABS(CountOfHours),Convert(datetime, StartDate, 0))) 
					= YEAR(GETDATE()))
		BEGIN
			RETURN 'доступен'
		END
		RETURN 'занят'
	END

GO
	PRINT 'Создал Функцию GetVehicleStatus'

GO
	CREATE FUNCTION GetCoordinates(
		@Vehicle_ID INT
	)
	RETURNS table
	AS
		RETURN 
			SELECT TOP(1) ID_Vehicle, Latitude, Longitude 
			FROM VehicleCoordinates 
			WHERE ID_Vehicle = @Vehicle_ID
			ORDER BY StayDateTime DESC

GO
	PRINT 'Создал Функцию GetCoordinates'

	

GO
	PRINT '==================================Представления======================================='
	USE VehicleRental

USE VehicleRental
GO
	CREATE VIEW VehiclesINFO
	AS
	SELECT DISTINCT
		Vehicles.ID_Vehicle, 
		VehicleRegistrCertificates.Brand,
		VehicleRegistrCertificates.Mark,
		VehicleRegistrCertificates.Color,
		Vehicles.Class, 
		VehicleRegistrCertificates.Vehicle_Category,
		Vehicles.PricePerHour, 
		Vehicles.CarPicture,
		(SELECT TOP(1) Latitude 
			FROM VehicleCoordinates 
			WHERE VehicleCoordinates.ID_Vehicle = Vehicles.ID_Vehicle
			ORDER BY StayDateTime DESC) as Lat,
		(SELECT TOP(1) Longitude 
			FROM VehicleCoordinates 
			WHERE VehicleCoordinates.ID_Vehicle = Vehicles.ID_Vehicle
			ORDER BY StayDateTime DESC) as Lng,
		[dbo].GetVehicleStatus(Vehicles.ID_Vehicle) as AccessStatus 
	FROM
		Vehicles INNER JOIN VehicleRegistrCertificates
		ON Vehicles.ID_Vehicle = VehicleRegistrCertificates.ID_Vehicle

GO
	PRINT '==================================Функции 2====================================='
GO
	CREATE FUNCTION VehiclesWithStatus 
	(
		@Status char(8)
	)
		RETURNS table
	AS
	RETURN (
		SELECT * FROM VehiclesINFO
		WHERE LOWER(AccessStatus) = LOWER(@Status))
GO
	PRINT 'Создал Функцию VehiclesWithStatus'


GO
	PRINT '==================================Пользователи======================================='
	--USE VehicleRental
--GO
--	EXEC REG_USER 
--		@UserLogin='rental_admin', 
--		@UserEmail='maksim_razukhin@inbox.ru', 
--		@UserPassword='rental_admin', 
--		@UserSurname='Admin', 
--		@UserName='Admin', 
--		@UserMiddleName='Admin',
--		@UserBirthDayDate='12.01.2005'

--UPDATE Rental_Users SET isAdmin=1 WHERE UserLogin='rental_admin'

USE VehicleRental
	SELECT * FROM Rental_Users

GO
	PRINT '==================================Встроенные пользователи (приложние)======================================='
	USE VehicleRental
		CREATE LOGIN USERHANDLER WITH PASSWORD = 'USERHANDLER'
		CREATE USER DB_USER_USERHANDLER FOR LOGIN USERHANDLER
		
		GRANT INSERT, SELECT ON Rental_Users to DB_USER_USERHANDLER
		GRANT SELECT ON Rental_Admins to DB_USER_USERHANDLER
		GRANT EXEC ON GetDriverLicenceByUserID to DB_USER_USERHANDLER
		GRANT EXEC ON AddDriverLicenceToUser to DB_USER_USERHANDLER
		GRANT EXEC ON AddCategoryToDriverLicence to DB_USER_USERHANDLER
		GRANT EXEC ON REG_USER TO DB_USER_USERHANDLER
		--GRANT EXEC ON CHECK_USER TO DB_USER_USERHANDLER

		CREATE LOGIN CARHANDLER WITH PASSWORD = 'CARHANDLER'
		CREATE USER DB_USER_CARHANDLER FOR LOGIN CARHANDLER
		
		GRANT INSERT, SELECT, UPDATE, DELETE ON Vehicles TO DB_USER_CARHANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON VehicleCoordinates TO DB_USER_CARHANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON Classes TO DB_USER_CARHANDLER
		--GRANT INSERT, SELECT, UPDATE, DELETE ON VehiclesPassports TO DB_USER_CARHANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON VehicleRegistrCertificates TO DB_USER_CARHANDLER
		GRANT SELECT ON VehiclesINFO TO DB_USER_CARHANDLER
		GRANT SELECT ON VehiclesWithStatus TO DB_USER_CARHANDLER

		CREATE LOGIN DLHANDLER WITH PASSWORD = 'DLHANDLER'
		CREATE USER DB_USER_DLHANDLER FOR LOGIN DLHANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON DriversLicences TO DB_USER_DLHANDLER

		CREATE LOGIN DATABASEADMIN WITH PASSWORD = 'MODERATEDATABASE'
		CREATE USER DB_ADMIN_HANDLER FOR LOGIN DATABASEADMIN

		GRANT INSERT, SELECT, UPDATE, DELETE ON Vehicles TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON VehicleCoordinates TO DB_ADMIN_HANDLER
		--GRANT INSERT, SELECT, UPDATE, DELETE ON VehiclesPassports TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON VehicleRegistrCertificates TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON Categories TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON Classes TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON DriversLicences TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON Rental_Users to DB_ADMIN_HANDLER
		GRANT SELECT ON Rental_Admins to DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON Rentals TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON TrafficAccidents TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON TrafficAccidentTypes TO DB_ADMIN_HANDLER
		
		GRANT EXEC ON REG_USER TO DB_USER_USERHANDLER
