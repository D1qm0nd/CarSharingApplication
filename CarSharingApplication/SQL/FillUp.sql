﻿USE VehicleRental

GO 
	USE VehicleRental

	INSERT Rental_Users VALUES
	('eqYY5T3L49ZRJbW/V9rosA==', 'rental_admin@rental_admin.rental_admin', 'eqYY5T3L49ZRJbW/V9rosA==', 1, 'AdminSurname', 'AdminName', 'AdminMiddlename', '2023-04-01'),
	('/dnKYZHqiXNRZX3gcUdwwg==', 'rental_user@rental_user.rental_user', '/dnKYZHqiXNRZX3gcUdwwg==', 1, 'rental_user', 'rental_user', 'rental_user','2023-04-06')

GO 
	USE VehicleRental
	INSERT DriversLicences VALUES
	('7878443544', '2002-03-12' ,2),
	('8989834554', '2005-04-12' ,1),
	('5463454355', '2006-09-12' ,2),
	('5434523453', '2007-05-12' ,1)
GO
	USE VehicleRental
	INSERT Classes (Class) VALUES
		('Бизнес'),
		('Эконом'),
		('Стандарт'),
		('Спорт'),
		('Комфорт')

GO
	USE VehicleRental

	INSERT Vehicles VALUES
		(1460.0,	'Бизнес',		NULL),
		(700.0,		'Эконом',		NULL),
		(970.0,		'Стандарт',		NULL),
		(2300.0,	'Спорт',		NULL),
		(1700.0,	'Бизнес',		NULL),
		(1300.0,	'Комфорт',		NULL),
		(1500.0,	'Стандарт',		NULL),
		(1680.0,	'Спорт',		NULL),
		(1460.0,	'Бизнес',		NULL),
		(700.0,		'Эконом',		NULL),
		(970.0,		'Стандарт',		NULL),
		(2300.0,	'Спорт',		NULL),
		(1700.0,	'Бизнес',		NULL),
		(1300.0,	'Комфорт',		NULL),
		(1500.0,	'Стандарт',		NULL),
		(1680.0,	'Спорт',		NULL)

GO
	USE VehicleRental
	INSERT VehicleRegistrCertificates VALUES
	(4234, 123343,	1	,'52LD5432123',   	'О435РУ777', 	'MERCEDES-BENZ',	'Gelandewagen',	'Внедорожник',	'B',	2021,	'NO',	'2342342341',	'Черный',		200,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(2343, 423423,	2	,'43MK3244212',   	'А435КА047', 	'LADA',				'Kalina',		'Седан',		'B',	2011,	'NO',	'234SDF2341',	'Синий',		120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(6432, 234212,	3	,'54TY4321134',   	'Л834РС777', 	'KIA',				'RIO',			'Седан',		'B',	2021,	'NO',	'234SDF2341',	'Белый',		120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(1243, 435344,	4	,'12HR6573423',   	'В888ВВ888', 	'Lamborghini ',		'Aventador',	'Спортивная',	'B',	2014,	'NO',	'435423FD342',	'Оранжевый',	450,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(3452, 123412,	5	,'52HJ5652523',   	'А555АА052', 	'MERCEDES-BENZ',	'C-CLASS',		'Седан',		'B',	2021,	'NO',	'2342342341',	'Серый',		200,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(5476, 452323,	6	,'42MK3244212',   	'С347БА112', 	'LADA',				'Granta',		'Седан',		'B',	2021,	'NO',	'234SDF2341',	'Коричневый',	120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(1234, 123124,	7	,'52TY5321334',   	'Р333УС321', 	'LADA',				'Riva',			'Седан',		'B',	2021,	'NO',	'234SDF2341',	'Вишневый',		120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(1443, 173412,	8	,'12HR6523223',   	'Н123НН123', 	'Lamborghini ',		'Gollardo',		'Спортивная',	'B',	2011,	'NO',	'435423FD342',	'Белый',		450,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(4313, 435123,	9	,'23HG5324143',   	'О435РП542', 	'SKODA',			'OCTAVIA II',	'Седан',		'B',	2021,	'NO',	'2342342341',	'Белый',		200,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(5674, 324344,	10	,'43DF2345243',   	'С435ВК047', 	'RENAULT',			'SCÉNICII',		'Седан',		'B',	2011,	'NO',	'234SDF2341',	'Коричневый',	120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(3245, 763456,	11	,'52TY4321234',   	'У834КО777', 	'BMW',				'X1 (E84)',		'Седан',		'B',	2021,	'NO',	'234SDF2341',	'Белый',		120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(1234, 123445,	12	,'12HR6573523',   	'К888ТО888', 	'SKODA ',			'KODIAQ',		'Внедорожник',	'B',	2014,	'NO',	'435423FD342',	'Белый',		450,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(3244, 342553,	13	,'51LD5432123',   	'В555СТ052', 	'OPEL',				'ASTRA',		'Седан',		'B',	2021,	'NO',	'2342342341',	'Серый',		200,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(5234, 143322,	14	,'43MK3243452',   	'С347УВ112', 	'TOYOTA',			'HILUX VII',	'Седан',		'B',	2021,	'NO',	'234SDF2341',	'Серый',		120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(3242, 423521,	15	,'54TY4321234',   	'О333КУ321', 	'MERCEDES-BENZ',	'E-CLASS',		'Седан',		'B',	2021,	'NO',	'234SDF2341',	'Серебристый',	120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(3544, 643422,	16	,'43HY1123423',   	'Н123ТК123', 	'Porsche  ',		'Cayenne',		'Внедорожник',	'B',	2011,	'NO',	'435423FD342',	'Синий',		450,	'Четвёртый',  1200,	1100, '2023-04-01')


GO 
	DECLARE @YESTERDAY SMALLDATETIME = DATEADD(day, -1, GETDATE())

	USE VehicleRental
	INSERT VehicleCoordinates VALUES
	(1,		37.628310329218785,		55.783562264913726,		@YESTERDAY),
	(2,		37.62496863084447,		55.77968409403343,		@YESTERDAY),
	(3,		37.60988838934319,		55.771697388745174,		@YESTERDAY),
	(4,		37.63784366449218,		55.80634960848172,		@YESTERDAY),
	(5,		37.64351317690392,		55.75885752573489,		@YESTERDAY),
	(6,		37.650152394817006,		55.79318597301102,		@YESTERDAY),
	(7,		37.64382665235103,		55.767218191902586,		@YESTERDAY),
	(8,		37.631991990646156,		55.754565408911894,		@YESTERDAY),
	(9,		37.60679471254363,		55.76311732978595,		@YESTERDAY),
	(10,	37.60886742602116,		55.7729332932597,		@YESTERDAY),
	(11,	37.61629258607224,		55.79079936483427,		@YESTERDAY),
	(12,	37.62285999673869,		55.81227549857955,		@YESTERDAY),
	(13,	37.61330564444986,		55.79759711490164,		@YESTERDAY),
	(14,	37.64433763929462,		55.81538058758365,		@YESTERDAY),
	(15,	37.644649815415036,		55.79659831246247,		@YESTERDAY),
	(16,	37.612114115149616,		55.78038656884442,		@YESTERDAY),

GO
	USE VehicleRental
	INSERT Rentals VALUES
	('8989834554', 1 , GETDATE(), '12:00', 2),
	('5463454355', 3 , GETDATE(), '15:00', 3),
	('8989834554', 4 , GETDATE(), '12:00', 1),
	('5463454355', 7 , GETDATE(), '15:00', 3)

GO 
	USE VehicleRental
	INSERT Rental_Admins VALUES
	(1)
	

