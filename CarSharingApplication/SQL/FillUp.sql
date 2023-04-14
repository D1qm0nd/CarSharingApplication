USE VehicleRental

GO 
	USE VehicleRental

	INSERT Rental_Users VALUES
	('eqYY5T3L49ZRJbW/V9rosA==', 'rental_admin@rental_admin.rental_admin', 'eqYY5T3L49ZRJbW/V9rosA==', 1, 'AdminSurname', 'AdminName', 'AdminMiddlename', '2023-04-01'),
	('/dnKYZHqiXNRZX3gcUdwwg==', 'rental_user@rental_user.rental_user', '/dnKYZHqiXNRZX3gcUdwwg==', 1, 'rental_user', 'rental_user', 'rental_user','2023-04-06')

GO 
	USE VehicleRental
	INSERT DriversLicences VALUES
	('8989834554', '2005-04-12' ,1),
	('5463454355', '2005-04-12' ,2)

GO
	USE VehicleRental
	INSERT Classes (Class) VALUES
		('Бизнес'),
		('Эконом'),
		('Стандарт'),
		('Спорт')

GO
	USE VehicleRental

	INSERT Vehicles VALUES
		(3000.0, 'Бизнес', NULL),
		(1000.0, 'Эконом', NULL),
		(1500.0, 'Стандарт', NULL),
		(5000.0, 'Спорт', NULL)

GO
	USE VehicleRental
	INSERT VehicleRegistrCertificates VALUES
	(4234, 123343,	1	,'52LD5432123',   	'О435РУ777', 	'Мерседес',	'Гелентваген',	'Вездеход',	'B',	2021,	'NO',	'2342342341',	'Черный',	200,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(2343, 423423,	2	,'43MK3244212',   	'А435КА047', 	'Лада',	'Калина',	'Легковая',	'B',	2011,	'NO',	'234SDF2341',	'Синий',	120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(6432, 423423,	3	,'54TY4321234',   	'Л834РС777', 	'Киа',	'Рио',	'Легковая',	'B',	2021,	'NO',	'234SDF2341',	'Красный',	120,	'Четвёртый',  1200,	1100, '2023-04-01'),
	(1243, 123412,	4	,'12HR6573423',   	'В888ВВ888', 	'Ламборгини',	'Авентадор',	'Спортивная',	'B',	2014,	'NO',	'435423FD342',	'Оранжевый',	450,	'Четвёртый',  1200,	1100, '2023-04-01')

GO 
	USE VehicleRental
	INSERT VehicleCoordinates VALUES
	(1, 37.621094, 55.754000, '2023-04-01'),
	(2, 37.622095, 55.755000, '2023-04-01'),
	(3, 37.623096, 55.756000, '2023-04-01'),
	(4, 37.624097, 55.757000, '2023-04-01')


GO
	USE VehicleRental
	INSERT Rentals VALUES
	('8989834554', 1 , GETDATE(), '12:00', 2),
	('5463454355', 3 , GETDATE(), '15:00', 3)

GO 
	USE VehicleRental
	INSERT Rental_Admins VALUES
	(1)
	

