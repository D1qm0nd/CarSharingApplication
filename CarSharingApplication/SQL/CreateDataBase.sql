GO
	PRINT '==================================База Данных======================================='

	USE master
		IF EXISTS(SELECT * FROM sys.databases WHERE LOWER([name]) = LOWER(N'VehicleRental'))
		BEGIN
			ALTER DATABASE VehicleRental 
				SET SINGLE_USER WITH ROLLBACK IMMEDIATE
			DROP DATABASE VehicleRental
			PRINT 'Дропнул БД'
		END ELSE PRINT 'Базы Данных не существует'
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
	IF NOT EXISTS(SELECT * FROM sys.databases WHERE LOWER([name]) = LOWER(N'VehicleRental'))
	BEGIN
		EXEC CreateDataBase @DBName = 'VehicleRental', @DBPath = 'C:\Users\Public'
		PRINT 'Создал БД'
	END
/*Создание таблиц*/



GO 
	PRINT '==================================Таблицы======================================='
	USE VehicleRental

	CREATE TABLE Vehicle_Rental_logs
	(
		ID_Log INT IDENTITY(1,1) NOT NULL,
		LogString NVARCHAR(MAX) NOT NULL
		PRIMARY KEY (ID_Log)
	)

	CREATE TABLE Rental_Users
	(
		ID_User INT IDENTITY(1,1) NOT NULL,
		UserLogin NVARCHAR(120) UNIQUE NOT NULL,
		UserEMail NVARCHAR(120) UNIQUE NOT NULL,
		UserPassword NVARCHAR(120) NOT NULL,
		UserSurname NVARCHAR(120) NOT NULL,
		UserName NVARCHAR(120) NOT NULL,
		UserMiddleName NVARCHAR(120) NOT NULL,
		UserBirthDay DATE NOT NULL,
		PRIMARY KEY (ID_User)
	)
	PRINT 'Создал Таблицу Rental_Users'

	CREATE TABLE Rental_Admins
	(
		ID_Admin INT IDENTITY(1,1) NOT NULL,
		ID_User INT NOT NULL
	)

	PRINT 'Создал Таблицу Rental_Admins'

	CREATE TABLE DriversLicences
	(
		ID_DriverLicence CHAR(10) PRIMARY KEY NOT NULL 
			CHECK (
				ID_DriverLicence LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
				),
		ReceiptDate DATE NOT NULL,
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
		ReceiptDate DATE NOT NULL,
		EndDate DATE NOT NULL
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
		ID_Coordinates INT IDENTITY(1,1)  NOT NULL,
		ID_Vehicle INT NOT NULL,
		Longitude FLOAT NOT NULL,
		Latitude FLOAT NOT NULL,
		StayDateTime DATETIME NOT NULL
		PRIMARY KEY (ID_Coordinates)
	)
	PRINT 'Создал Таблицу VehicleCoordinates'

	CREATE TABLE Rentals
	(
		ID_Rental INT IDENTITY(1,1)  NOT NULL,
		ID_DriverLicence CHAR(10) NOT NULL 
			CHECK (
				ID_DriverLicence LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
				),
		ID_Vehicle INT NULL,
		StartDate DATE NOT NULL,
		RentalTime TIME NOT NULL,
		CountOfHours INT NOT NULL,
		TotalPrice MONEY NOT NULL,
		RentalStatus CHAR(12) DEFAULT 'стандартная' NOT NULL
		PRIMARY KEY (ID_Rental)
	)
	PRINT 'Создал Таблицу Rentals'

	CREATE TABLE TrafficAccidentTypes
	(
		ID_TrafficAccidentType INT IDENTITY(1,1) NOT NULL,
		TrafficAccidentTypeName NVARCHAR(60) NOT NULL
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
		Damage MONEY NOT NULL,
		TrafficAccidentDescription NVARCHAR(MAX) NOT NULL
		 PRIMARY KEY (ID_TrafficAccident)
	)
	PRINT 'Создал Таблицу TrafficAccidents'

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
		PermissibleMaximumWeightKg INTEGER NOT NULL,
		WeightWithoutLoadKg INTEGER NOT NULL,
		PRIMARY KEY (CertificateSeries, CertificateNumber)
	)

	PRINT 'Создал Таблицу VehicleRegistrCertificates'

	
/*Связи*/

GO
	PRINT '==================================Связи======================================='

	USE VehicleRental

		ALTER TABLE Rental_Admins
		ADD CONSTRAINT FK_Rental_Admins_Rental_Users 
			FOREIGN KEY (ID_User) REFERENCES Rental_Users (ID_User)
			ON DELETE CASCADE
			ON UPDATE CASCADE

		PRINT 'Создал Ключ FK_Rental_Admins_Rental_Users'
GO
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
		ON DELETE SET DEFAULT
		ON UPDATE CASCADE

	PRINT 'Создал Ключ FK_Rentals_DriversLicences'
GO	
	USE VehicleRental

	ALTER TABLE Rentals
	ADD CONSTRAINT FK_Rentals_Vehicles
		FOREIGN KEY (ID_Vehicle) REFERENCES Vehicles (ID_Vehicle)
		ON DELETE SET DEFAULT
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
		ON DELETE SET DEFAULT
		ON UPDATE CASCADE
	PRINT 'Создал Ключ FK_TrafficAccidents_Vehicles'

GO
	USE VehicleRental
	
	ALTER TABLE TrafficAccidents
	ADD CONSTRAINT FK_TrafficAccidents_TrafficAccidentTypes
		FOREIGN KEY (ID_TrafficAccidentType) REFERENCES TrafficAccidentTypes (ID_TrafficAccidentType)
		ON DELETE SET DEFAULT
		ON UPDATE CASCADE
	PRINT 'Создал Ключ FK_TrafficAccidents_TrafficAccidentTypes'

GO
	USE VehicleRental
	
	ALTER TABLE TrafficAccidents
	ADD CONSTRAINT FK_TrafficAccidents_DriversLincences
		FOREIGN KEY (ID_DriverLicence) REFERENCES DriversLicences (ID_DriverLicence)
		ON DELETE SET DEFAULT
		ON UPDATE CASCADE
	PRINT 'Создал Ключ FK_TrafficAccidents_DriversLincences'
	
GO
	USE VehicleRental

	ALTER TABLE VehicleRegistrCertificates
	ADD CONSTRAINT FK_VehicleRegistrCertificates_Vehicles
		FOREIGN KEY (ID_Vehicle) 
		REFERENCES Vehicles (ID_Vehicle)
		ON DELETE CASCADE
		ON UPDATE CASCADE


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
	BEGIN TRY
		BEGIN TRANSACTION Registration
			DECLARE @SqlCommand NVARCHAR(MAX) = 
			'USE VehicleRental '+
			'INSERT INTO Rental_Users '+
			'(UserLogin, UserEMail, UserPassword, UserSurname, UserName, UserMiddleName, UserBirthDay) VALUES ('+
				''''+@UserLogin+''', '''+@UserEmail+''', '''+@UserPassword+''', '+''''+@UserSurname+''', '''+@UserName+''', '''+@UserMiddleName+''', '''+@UserBirthDayDate+''')'
		
			EXEC (@SqlCommand)
		COMMIT TRANSACTION Registration
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION Registration
	END CATCH


GO
	PRINT 'Создал Хранимую процедуру REG_USER'

GO
	CREATE FUNCTION GetDriverLicenceByUserID(
		@User_ID INT
	)
	RETURNS TABLE
	AS
		RETURN SELECT TOP(1) * 
			FROM DriversLicences
			WHERE ID_User = @User_ID
			ORDER BY ReceiptDate DESC
GO	
	PRINT 'Создал Хранимую процедуру GetDriverLicenceByUserID'

GO 
	CREATE PROCEDURE AddDriverLicenceToUser
	(
		@User_ID INT,
		@DriverLicence CHAR(10),
		@ReceiptDate DATE
	)
	AS
	BEGIN TRY
		BEGIN TRANSACTION AddDriverLicence
			INSERT DriversLicences VALUES
			(@DriverLicence, @ReceiptDate, @User_ID)
		COMMIT TRANSACTION AddDriverLicence
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION AddDriverLicence
	END CATCH

GO
	PRINT 'Создал Хранимую процедуру AddDriverLicenceToUser'

GO 
	CREATE PROCEDURE AddCategoryToDriverLicence
	(
		@DriverLicence_ID CHAR(10),
		@Category CHAR(3),
		@ReceiptDate DATE,
		@EndDate DATE
	)
	AS
	BEGIN TRY
		BEGIN TRANSACTION AddCategory
			INSERT Categories VALUES
			(@DriverLicence_ID, @Category, @ReceiptDate, @EndDate)
		COMMIT TRANSACTION AddCategory
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION AddCategory
	END CATCH
GO
	PRINT 'Создал Хранимую процедуру AddCategoryToDriverLicence'

GO
	CREATE PROCEDURE Rent(
		@DriverLicence CHAR(10),
		@ID_Vehicle INT,
		@RentalTime TIME,
		@CountOfHours INT,
		@TotalPrice MONEY
	)
	AS
	BEGIN TRY
		BEGIN TRANSACTION RentVehicle
			IF NOT ((SELECT [dbo].GetVehicleStatus(@ID_Vehicle)) = 'доступен')
				ROLLBACK TRANSACTION RentVehicle
			INSERT Rentals VALUES 
				(@DriverLicence, @ID_Vehicle, GETDATE(), @RentalTime, @CountOfHours, @TotalPrice, 'стандартная')
		COMMIT TRANSACTION RentVehicle
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION RentVehicle
	END CATCH

GO
	PRINT 'Создал Хранимую процедуру Rent'

GO
	PRINT '==================================Функции======================================='

GO 
	CREATE FUNCTION CheckExistingUser
	(
		@login NVARCHAR(MAX),
		@Password NVARCHAR(MAX)
	) 
	RETURNS INT
	AS
	BEGIN
		DECLARE @ID INT = -1
		(SELECT @ID = ID_User FROM Rental_Users WHERE Rental_Users.UserLogin = @login AND UserPassword = @Password)
		RETURN @ID
	END
GO
	PRINT 'Создал Функции CheckExistingUser'

GO
	CREATE FUNCTION DBStatus(
		@ID_User INT
	)
	RETURNS char(24) 
	BEGIN
		DECLARE @answ CHAR(24); 
		IF (EXISTS (SELECT * FROM Rental_Admins WHERE ID_User = @ID_User))
			SET @answ = 'админ'
		ELSE IF (EXISTS (SELECT * FROM Rental_Users WHERE ID_User = @ID_User)) 
			SET @answ = 'пользователь'
		ELSE SET @answ = 'пользователь отсутствует'
		RETURN @answ
	END
GO
	PRINT 'Создал Функции DBStatus'

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
	RETURNS char(11)
	AS
	BEGIN
		DECLARE @ret char(11) = 'отсутствует'
		IF EXISTS(SELECT * FROM Vehicles WHERE ID_Vehicle = @Vehicle_ID)
			IF NOT EXISTS (SELECT *, 
				DATEADD(HOUR, ABS(DATEDIFF(HOUR,RentalTime,0))+ABS(CountOfHours),Convert(datetime, StartDate, 102)) as rt
						FROM Rentals 
						WHERE DATEADD(HOUR, ABS(DATEDIFF(HOUR,RentalTime,0))+ABS(CountOfHours),Convert(datetime, StartDate, 0)) 
						> GETDATE() 
						AND ID_Vehicle = @Vehicle_ID
						AND YEAR(DATEADD(HOUR, ABS(DATEDIFF(HOUR,RentalTime,0))+ABS(CountOfHours),Convert(datetime, StartDate, 0))) 
						= YEAR(GETDATE()) and RentalStatus = 'стандартная')
				SET @ret = 'доступен'
			ELSE SET @ret = 'занят'
		RETURN @ret
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

USE VehicleRental
GO
	CREATE FUNCTION GetUserDriverLicences
	(
		@ID_USER INT
	)
	RETURNS TABLE
	AS
	RETURN (SELECT * FROM DriversLicences WHERE DriversLicences.ID_User = @ID_USER)
GO
	PRINT 'Создал Функцию GetUserDriverLicences'

USE VehicleRental
GO
	CREATE FUNCTION UserRentalCount
	(
		@ID_USER INT
	)
	RETURNS INT
	AS
	BEGIN
		DECLARE @Count INT = 0
		SELECT @Count = COUNT(*) FROM Rentals
		INNER JOIN [dbo].GetUserDriverLicences(@ID_USER) as LICENCES
		ON LICENCES.ID_DriverLicence = Rentals.ID_DriverLicence
		RETURN @Count
	END
GO
	PRINT 'Создал Функцию UserRentalCount'

	USE VehicleRental
GO
	CREATE FUNCTION CountUserAccidents
	(
		@ID_User INT
	)
	RETURNS INT
	AS
	BEGIN
		DECLARE @Count INT
			SELECT @Count=COUNT(*) FROM TrafficAccidents
			INNER JOIN [dbo].GetUserDriverLicences(@ID_User) as LICENCES
			ON LICENCES.ID_DriverLicence = TrafficAccidents.ID_DriverLicence
		RETURN @Count
	END

GO
	PRINT 'Создал Функцию CountUserAccidents'

GO
	CREATE FUNCTION VehicleAccidents
	(
		@ID_Vehicle INT 
	)
	RETURNS	TABLE
	AS
	RETURN SELECT * FROM TrafficAccidents WHERE ID_Vehicle = @ID_Vehicle

GO
	PRINT 'Создал Функцию VehicleAccidents'

GO
	CREATE FUNCTION CountVehicleAccidents
	(
		@ID_Vehicle INT
	)
	RETURNS INT
	AS
	BEGIN
		DECLARE @Count INT
			SELECT @Count = COUNT(*) FROM TrafficAccidents WHERE ID_Vehicle = @ID_Vehicle
		RETURN @Count
	END

GO
	PRINT 'Создал Функцию CountVehicleAccidents'

GO
	CREATE FUNCTION TotalVehicleDamage
	(
		@ID_Vehicle INT
	)
	RETURNS MONEY
	AS
	BEGIN
		DECLARE @TotalSum MONEY
			SELECT @TotalSum = SUM(Damage) FROM [dbo].VehicleAccidents(@ID_Vehicle) 
		RETURN @TotalSum
	END

GO
	PRINT 'Создал Функцию TotalVehicleDamage'

GO
CREATE FUNCTION GetUserStatus
(
	@ID_DriverLicence CHAR(10)
)
RETURNS CHAR(9) -- 'арендует' 'нет'
AS
BEGIN
	DECLARE @ret char(9) = 'нет'
	if EXISTS( SELECT ID_DriverLicence 
			   FROM Rentals
			   WHERE Rentals.ID_DriverLicence = @ID_DriverLicence 
			   AND DATEADD(HOUR,Rentals.CountOfHours+DATEPART(HOUR,CONVERT(DATETIME,Rentals.RentalTime)),CONVERT(DATETIME,Rentals.StartDate)) < GETDATE()) 
		SET @ret = 'арендует'
	RETURN @ret
END

GO
	PRINT 'Создал Функцию GetUserStatus'



GO
	PRINT '==================================Представления======================================='
USE VehicleRental
GO
	CREATE VIEW RentalsINFO
	AS
	SELECT 
			ID_Rental, 
			ID_Vehicle, 
			ID_DriverLicence, 
			RentalStatus,
			TotalPrice,
			DATEADD(SECOND,DATEPART(SECOND,Rentals.RentalTime),
				DATEADD(MINUTE,DATEPART(MINUTE,Rentals.RentalTime),
					DATEADD(HOUR,Rentals.CountOfHours+DATEPART(HOUR,Rentals.RentalTime),
						CONVERT(DATETIME,StartDate)))) as EndTime
		FROM Rentals

GO
	USE VehicleRental
GO
	CREATE FUNCTION TripStatus
	( 
		@DriverLicence CHAR(10)
	)
	RETURNS CHAR(9)
	AS
	BEGIN
		DECLARE @Ret CHAR(9)
		IF EXISTS(SELECT TOP (1) *
			FROM RentalsINFO
			WHERE RentalsINFO.ID_DriverLicence = @DriverLicence 
			AND EndTime > GETDATE() AND LOWER(RentalStatus) = 'стандартная')
		SET @Ret = 'в поездке'
		ELSE
			SET @Ret = NULL
		RETURN @Ret
	END
GO
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
		[dbo].GetVehicleStatus(Vehicles.ID_Vehicle) as AccessStatus,
		[dbo].TotalVehicleDamage(Vehicles.ID_Vehicle) as DamageCost

	FROM
		Vehicles INNER JOIN VehicleRegistrCertificates
		ON Vehicles.ID_Vehicle = VehicleRegistrCertificates.ID_Vehicle

GO 
	PRINT 'Создал представление VehiclesINFO'

GO
	USE VehicleRental
GO
	CREATE VIEW UsersINFO
	AS
		SELECT 
			ID_User, 
			UserEMail, 
			UserSurname, 
			UserName, 
			UserMiddleName, 
			UserBirthDay, 
			[dbo].DBStatus(Rental_Users.ID_User) as Previlege, 
			[dbo].UserRentalCount(Rental_Users.ID_User) as RentalsCount, 
			[dbo].CountUserAccidents(Rental_Users.ID_User) as AccidentsCount,
			(SELECT ID_DriverLicence FROM [dbo].GetDriverLicenceByUserID(Rental_Users.ID_User)) as ID_DriverLicence, --??
			(SELECT ReceiptDate FROM [dbo].GetDriverLicenceByUserID(Rental_Users.ID_User)) as ReceiptDate, --??,
			[dbo].TripStatus((SELECT ID_DriverLicence FROM [dbo].GetDriverLicenceByUserID(Rental_Users.ID_User))) as RentStatus 
		FROM Rental_Users
GO 
	PRINT 'Создал представление UsersINFO'

GO
	CREATE VIEW DriversLicencesCategoriesINFO
	AS
		SELECT ID_DriverLicence,Category, ReceiptDate, EndDate 
		FROM Categories
		WHERE Categories.EndDate >= GETDATE()
GO 
	PRINT 'Создал представление DriversLicencesCategoriesINFO'

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
	CREATE FUNCTION GetDriverLicenceCategories
	(
		@ID_DriverLicence CHAR(10)
	)
	RETURNS TABLE
	AS
		RETURN 
		(SELECT 
			TRIM(LOWER(Category)) as Category,
			ReceiptDate,
			EndDate
		FROM 
		DriversLicencesCategoriesINFO
		WHERE 
		ID_DriverLicence = @ID_DriverLicence)

GO
	PRINT 'Создал Функцию GetDriverLicenceCategories'


GO
	PRINT '==================================Пользователи======================================='

GO
	PRINT '==================================Встроенные пользователи (приложние)======================================='
	USE VehicleRental
		CREATE LOGIN USERHANDLER WITH PASSWORD = 'USERHANDLER'
		CREATE USER DB_USER_USERHANDLER FOR LOGIN USERHANDLER
		
		GRANT INSERT, SELECT ON Rental_Users to DB_USER_USERHANDLER
		GRANT EXECUTE ON CheckExistingUser to DB_USER_USERHANDLER
		GRANT SELECT ON UsersINFO to DB_USER_USERHANDLER
		GRANT SELECT ON Rental_Admins to DB_USER_USERHANDLER
		GRANT SELECT ON GetDriverLicenceByUserID to DB_USER_USERHANDLER
		GRANT EXEC ON AddDriverLicenceToUser to DB_USER_USERHANDLER
		GRANT EXEC ON AddCategoryToDriverLicence to DB_USER_USERHANDLER
		GRANT EXEC ON REG_USER TO DB_USER_USERHANDLER
		GRANT EXEC ON Rent TO DB_USER_USERHANDLER
		
		--GRANT EXEC ON CHECK_USER TO DB_USER_USERHANDLER
		

		CREATE LOGIN CARHANDLER WITH PASSWORD = 'CARHANDLER'
		CREATE USER DB_USER_CARHANDLER FOR LOGIN CARHANDLER
		
		GRANT INSERT, SELECT, UPDATE ON Vehicles TO DB_USER_CARHANDLER
		GRANT INSERT, SELECT, UPDATE ON VehicleCoordinates TO DB_USER_CARHANDLER
		GRANT INSERT, SELECT, UPDATE ON Classes TO DB_USER_CARHANDLER
		--GRANT INSERT, SELECT, UPDATE, ON VehiclesPassports TO DB_USER_CARHANDLER
		GRANT INSERT, SELECT, UPDATE ON VehicleRegistrCertificates TO DB_USER_CARHANDLER
		GRANT SELECT ON VehiclesINFO TO DB_USER_CARHANDLER
		GRANT SELECT ON VehiclesWithStatus TO DB_USER_CARHANDLER
		GRANT SELECT ON RentalsINFO TO DB_USER_CARHANDLER
		GRANT SELECT, UPDATE ON Rentals TO DB_USER_CARHANDLER
		GRANT INSERT, SELECT ON TrafficAccidents TO DB_USER_CARHANDLER
		GRANT INSERT, SELECT ON TrafficAccidentTypes TO DB_USER_CARHANDLER


		CREATE LOGIN DLHANDLER WITH PASSWORD = 'DLHANDLER'
		CREATE USER DB_USER_DLHANDLER FOR LOGIN DLHANDLER
		GRANT INSERT, SELECT, UPDATE ON DriversLicences TO DB_USER_DLHANDLER
		GRANT SELECT ON GetDriverLicenceCategories TO DB_USER_DLHANDLER

		CREATE LOGIN DATABASEADMIN WITH PASSWORD = 'MODERATEDATABASE'
		CREATE USER DB_ADMIN_HANDLER FOR LOGIN DATABASEADMIN

		GRANT INSERT, SELECT, UPDATE, DELETE ON Vehicles TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON VehicleCoordinates TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON VehicleRegistrCertificates TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON Categories TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON Classes TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON DriversLicences TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON Rental_Users to DB_ADMIN_HANDLER
		GRANT SELECT ON Rental_Admins to DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON Rentals TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON TrafficAccidents TO DB_ADMIN_HANDLER
		GRANT INSERT, SELECT, UPDATE, DELETE ON TrafficAccidentTypes TO DB_ADMIN_HANDLER
		GRANT SELECT ON UsersINFO TO DB_ADMIN_HANDLER
		GRANT SELECT ON RentalsINFO TO DB_ADMIN_HANDLER
		GRANT SELECT ON DriversLicencesCategoriesINFO TO DB_ADMIN_HANDLER
		GRANT SELECT ON VehiclesINFO TO DB_ADMIN_HANDLER

		
		GRANT EXEC ON REG_USER TO DB_USER_USERHANDLER


		CREATE LOGIN DBLOGGER WITH PASSWORD = 'LOGDATABASE'
		CREATE USER LOGGER FOR LOGIN DBLOGGER

		GRANT SELECT, INSERT ON Vehicle_Rental_Logs TO LOGGER

GO
    PRINT('====================БЭКАПЫ==========================')
    USE MASTER
    GO
    CREATE PROCEDURE MakeDataBaseBackup
    (
        @databaseName NVARCHAR(MAX),
        @backupLocation NVARCHAR(MAX)
    )
    AS
        DECLARE @FName NVARCHAR(MAX) = 'Full Backup of ' + @databaseName
        BACKUP DATABASE VehicleRental 
        TO DISK = @backupLocation 
        WITH FORMAT, 
        MEDIANAME = 'SQLServerBackups',
        NAME =  @FName;