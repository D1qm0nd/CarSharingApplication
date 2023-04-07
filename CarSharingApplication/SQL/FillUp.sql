USE VehicleRental


GO
	USE VehicleRental
	INSERT Classes (Class) VALUES
		('Бизнес'),
		('Эконом'),
		('Стандарт'),
		('Спорт')

GO
	USE VehicleRental

	INSERT Vehicles (NumberTemplate, PricePerHour, Class) VALUES
		('А771ЕТ777', 3000.0, 'Бизнес'),
		('А761ВА091', 1000.0, 'Эконом'),
		('А666АА777', 1500.0, 'Стандарт')

GO 
	USE VehicleRental

	INSERT Rental_Users VALUES
	(1, '/dnKYZHqiXNRZX3gcUdwwg==', 'rental_user@rental_user.rental_user', '/dnKYZHqiXNRZX3gcUdwwg==', 1, 'rental_user', 'rental_user', 'rental_user','2023-04-06')


