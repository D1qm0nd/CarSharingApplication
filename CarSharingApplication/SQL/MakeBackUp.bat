@echo off

set BACKUPPATH=%1

sqlcmd -S LOCALHOST -E -Q "EXEC MakeDataBaseBackup @backupLocation='%BACKUPPATH%\VehicleRental.bak', @databaseName='VehicleRental'"
