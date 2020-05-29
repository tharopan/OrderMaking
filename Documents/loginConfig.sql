USE [ePos_Master]
GO

/* For security reasons the login is created disabled and with a random password. */
/***** Object:  Login [eposLourdes]    Script Date: 06/04/2017 20:20:27 *****/

CREATE LOGIN [eposLourdes] WITH PASSWORD=N'KrishnaAndJesus', DEFAULT_DATABASE=[ePos_Master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [eposLourdes] DISABLE
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [eposLourdes]
GO


ALTER LOGIN [eposLourdes] ENABLE
GO