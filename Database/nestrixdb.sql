CREATE DATABASE  IF NOT EXISTS `nestrixdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `nestrixdb`;
-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: db4free.net    Database: nestrixdb
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Adres`
--

DROP TABLE IF EXISTS `Adres`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Adres` (
  `Id` varchar(45) NOT NULL,
  `Straat` varchar(256) NOT NULL,
  `Huisnummer` varchar(50) DEFAULT NULL,
  `Postcode` varchar(45) NOT NULL,
  `Gemeente` varchar(256) NOT NULL,
  `Land` varchar(256) NOT NULL,
  `is_visible` tinyint NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Adres`
--

LOCK TABLES `Adres` WRITE;
/*!40000 ALTER TABLE `Adres` DISABLE KEYS */;
INSERT INTO `Adres` VALUES ('2a034407-c41a-4aba-bb31-2048fec7e985','Hillarestraat','67','9160','Lokeren','België',1),('2a4a560f-0083-48ab-9bca-ea7e0a890aa8','Teststraat','22','9000','Gent','België',1);
/*!40000 ALTER TABLE `Adres` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetRoleClaims`
--

DROP TABLE IF EXISTS `AspNetRoleClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetRoleClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetRoleClaims`
--

LOCK TABLES `AspNetRoleClaims` WRITE;
/*!40000 ALTER TABLE `AspNetRoleClaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetRoleClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetRoles`
--

DROP TABLE IF EXISTS `AspNetRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetRoles` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetRoles`
--

LOCK TABLES `AspNetRoles` WRITE;
/*!40000 ALTER TABLE `AspNetRoles` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserClaims`
--

DROP TABLE IF EXISTS `AspNetUserClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserClaims`
--

LOCK TABLES `AspNetUserClaims` WRITE;
/*!40000 ALTER TABLE `AspNetUserClaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetUserClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserLogins`
--

DROP TABLE IF EXISTS `AspNetUserLogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserLogins` (
  `LoginProvider` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserLogins`
--

LOCK TABLES `AspNetUserLogins` WRITE;
/*!40000 ALTER TABLE `AspNetUserLogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetUserLogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserRoles`
--

DROP TABLE IF EXISTS `AspNetUserRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserRoles` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserRoles`
--

LOCK TABLES `AspNetUserRoles` WRITE;
/*!40000 ALTER TABLE `AspNetUserRoles` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetUserRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserTokens`
--

DROP TABLE IF EXISTS `AspNetUserTokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserTokens` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LoginProvider` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserTokens`
--

LOCK TABLES `AspNetUserTokens` WRITE;
/*!40000 ALTER TABLE `AspNetUserTokens` DISABLE KEYS */;
INSERT INTO `AspNetUserTokens` VALUES ('ca9f746b-9bb0-4582-af0e-926cfd412e87','[AspNetUserStore]','AuthenticatorKey','LUUD4SN62NBSZJTEVBUOJPPH4JCOCMZT'),('ca9f746b-9bb0-4582-af0e-926cfd412e87','[AspNetUserStore]','RecoveryCodes','Q846D-HPH8T;FVBW9-P34FF;XWW43-DBCN3;P9XBT-XXRJP;P84WV-7XK46;4T3TR-3YJYP;4469M-96WTG;N4RTN-HBGBF;FMWGF-47X46;QV3H7-HR7B4');
/*!40000 ALTER TABLE `AspNetUserTokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUsers`
--

DROP TABLE IF EXISTS `AspNetUsers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUsers` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  `Secret` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ValidUntil` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUsers`
--

LOCK TABLES `AspNetUsers` WRITE;
/*!40000 ALTER TABLE `AspNetUsers` DISABLE KEYS */;
INSERT INTO `AspNetUsers` VALUES ('ca9f746b-9bb0-4582-af0e-926cfd412e87','matthias.polen@hogent.be','MATTHIAS.POLEN@HOGENT.BE','matthias.polen@hogent.be','MATTHIAS.POLEN@HOGENT.BE',1,'AQAAAAIAAYagAAAAEPMC6DSCyOlPmZaAoDIZIqlaRcIGg+ZRMqdZpyWDB5dXe7knkTYR30Zien5QL2MiRw==','BL5QW5CGDF64VNEB3524LGBSFZM4ZX26','963c60ac-3cdc-4eb8-a3d1-6aaf233a2081',NULL,0,1,NULL,1,0,'104FBDBA9E9C28A829E0563C3A5EED6E5D51E941A8C527E20F19511ABFA898C7','2023-07-03 00:00:00.000000');
/*!40000 ALTER TABLE `AspNetUsers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Bank`
--

DROP TABLE IF EXISTS `Bank`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Bank` (
  `Id` varchar(45) NOT NULL,
  `GebruikersId` varchar(45) DEFAULT NULL,
  `Naam` varchar(256) NOT NULL,
  `AdresId` varchar(45) NOT NULL,
  `Telefoonnummer` varchar(45) NOT NULL,
  `is_visible` tinyint NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  UNIQUE KEY `gebruikersId_UNIQUE` (`GebruikersId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Bank`
--

LOCK TABLES `Bank` WRITE;
/*!40000 ALTER TABLE `Bank` DISABLE KEYS */;
INSERT INTO `Bank` VALUES ('6a32d332-9a6e-4654-a809-3dfdbac32212',NULL,'KBC','2a4a560f-0083-48ab-9bca-ea7e0a890aa8','0123456789',1);
/*!40000 ALTER TABLE `Bank` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DataProtectionKeys`
--

DROP TABLE IF EXISTS `DataProtectionKeys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DataProtectionKeys` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FriendlyName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Xml` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DataProtectionKeys`
--

LOCK TABLES `DataProtectionKeys` WRITE;
/*!40000 ALTER TABLE `DataProtectionKeys` DISABLE KEYS */;
INSERT INTO `DataProtectionKeys` VALUES (1,'key-56597690-fcc0-4bc4-a9bf-4d081ed351eb','<key id=\"56597690-fcc0-4bc4-a9bf-4d081ed351eb\" version=\"1\"><creationDate>2023-05-04T08:05:27.7086943Z</creationDate><activationDate>2023-05-04T08:05:25.8794631Z</activationDate><expirationDate>2023-05-11T08:05:25.8794631Z</expirationDate><descriptor deserializerType=\"Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60\"><descriptor><encryption algorithm=\"AES_256_CBC\" /><validation algorithm=\"HMACSHA256\" /><masterKey p4:requiresEncryption=\"true\" xmlns:p4=\"http://schemas.asp.net/2015/03/dataProtection\"><!-- Warning: the key below is in an unencrypted form. --><value>62FDnkn8GMY7is65rv55djZ4AGW0cCaetwrRcJ5EsMw4hdzvLIcF4n02TVP2LEMmebBvYbKhlNBeWF7zGbB1Sg==</value></masterKey></descriptor></descriptor></key>'),(2,'key-9613cc41-f5cc-4b22-839b-f97bdcc0e5cc','<key id=\"9613cc41-f5cc-4b22-839b-f97bdcc0e5cc\" version=\"1\"><creationDate>2023-05-11T21:44:40.5267089Z</creationDate><activationDate>2023-05-11T21:44:38.7908762Z</activationDate><expirationDate>2023-05-18T21:44:38.7908762Z</expirationDate><descriptor deserializerType=\"Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60\"><descriptor><encryption algorithm=\"AES_256_CBC\" /><validation algorithm=\"HMACSHA256\" /><masterKey p4:requiresEncryption=\"true\" xmlns:p4=\"http://schemas.asp.net/2015/03/dataProtection\"><!-- Warning: the key below is in an unencrypted form. --><value>ymIQgyDp8mVTVhmLywHI4preqeaAeI78qi7NrVZrCHTbUViBDl1PcWmdXgVvTSSC4+EaNirkdceo3+dN8UmMTw==</value></masterKey></descriptor></descriptor></key>'),(3,'key-5a9b956b-4398-439d-8c2c-0711a0733719','<key id=\"5a9b956b-4398-439d-8c2c-0711a0733719\" version=\"1\"><creationDate>2023-05-25T08:50:55.4681284Z</creationDate><activationDate>2023-05-25T08:50:48.325888Z</activationDate><expirationDate>2023-06-01T08:50:48.325888Z</expirationDate><descriptor deserializerType=\"Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60\"><descriptor><encryption algorithm=\"AES_256_CBC\" /><validation algorithm=\"HMACSHA256\" /><masterKey p4:requiresEncryption=\"true\" xmlns:p4=\"http://schemas.asp.net/2015/03/dataProtection\"><!-- Warning: the key below is in an unencrypted form. --><value>1VfkgiwVdwfyoapks4wqPcGeuBlwbsI+vd2AIEPkMQmHpQZkwLWFLX4mj7ihPxpUggUnZWxkkNkUboF77deYkw==</value></masterKey></descriptor></descriptor></key>');
/*!40000 ALTER TABLE `DataProtectionKeys` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Gebruiker`
--

DROP TABLE IF EXISTS `Gebruiker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Gebruiker` (
  `Id` varchar(45) NOT NULL,
  `Familienaam` varchar(256) NOT NULL,
  `Voornaam` varchar(256) NOT NULL,
  `Code` varchar(256) NOT NULL,
  `Email` varchar(256) NOT NULL,
  `AdresId` varchar(45) NOT NULL,
  `Geboortedatum` datetime NOT NULL,
  `Telefoonnummer` varchar(256) NOT NULL,
  `is_visible` tinyint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `adresId_idx` (`AdresId`),
  CONSTRAINT `adresId` FOREIGN KEY (`AdresId`) REFERENCES `Adres` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Gebruiker`
--

LOCK TABLES `Gebruiker` WRITE;
/*!40000 ALTER TABLE `Gebruiker` DISABLE KEYS */;
INSERT INTO `Gebruiker` VALUES ('dc8813fe-8ca6-4c4a-b777-f510949f8f85','Colombie','Glenn','$2a$10$sYJFjE7OZMpDnHPsnk8ur.CbDQx4uD.sR7pYjqDybOp5X.K4vd6TK','glenn.colombie@student.hogent.be','2a034407-c41a-4aba-bb31-2048fec7e985','1997-06-12 00:00:00','0494386634',1);
/*!40000 ALTER TABLE `Gebruiker` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Rekening`
--

DROP TABLE IF EXISTS `Rekening`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Rekening` (
  `Rekeningnummer` varchar(45) NOT NULL,
  `GebruikerId` varchar(45) NOT NULL,
  `RekeningType` varchar(256) NOT NULL,
  `Krediet` decimal(10,2) NOT NULL,
  `Saldo` decimal(10,2) NOT NULL,
  `TransactieId` varchar(45) DEFAULT NULL,
  `is_visible` tinyint NOT NULL,
  PRIMARY KEY (`Rekeningnummer`),
  UNIQUE KEY `rekeningnummer_UNIQUE` (`Rekeningnummer`),
  KEY `gebruikerId_idx` (`GebruikerId`),
  KEY `transactieId_idx` (`TransactieId`),
  CONSTRAINT `gebruikerId` FOREIGN KEY (`GebruikerId`) REFERENCES `Gebruiker` (`Id`),
  CONSTRAINT `transactieId` FOREIGN KEY (`TransactieId`) REFERENCES `Transactie` (`RekeningnummerBegunstigde`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Rekening`
--

LOCK TABLES `Rekening` WRITE;
/*!40000 ALTER TABLE `Rekening` DISABLE KEYS */;
INSERT INTO `Rekening` VALUES ('3c146461-8b97-4028-8a51-3511113d3e95','dc8813fe-8ca6-4c4a-b777-f510949f8f85','Zichtrekening',0.00,10000.00,NULL,1),('c94d0a24-d0f8-45fa-a9bd-8bc2a99cbd76','dc8813fe-8ca6-4c4a-b777-f510949f8f85','Zichtrekening',500.00,0.00,NULL,1);
/*!40000 ALTER TABLE `Rekening` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Transactie`
--

DROP TABLE IF EXISTS `Transactie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Transactie` (
  `Id` varchar(45) NOT NULL,
  `RekeningnummerBegunstigde` varchar(45) NOT NULL,
  `Datum` datetime NOT NULL,
  `TransactieType` varchar(256) NOT NULL,
  `Bedrag` decimal(10,2) NOT NULL,
  `Mededeling` varchar(256) DEFAULT NULL,
  `is_visible` tinyint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `rekeningnummer_idx` (`RekeningnummerBegunstigde`),
  CONSTRAINT `rekeningnummerBegunstigde` FOREIGN KEY (`RekeningnummerBegunstigde`) REFERENCES `Rekening` (`Rekeningnummer`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Transactie`
--

LOCK TABLES `Transactie` WRITE;
/*!40000 ALTER TABLE `Transactie` DISABLE KEYS */;
/*!40000 ALTER TABLE `Transactie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20230411165545_AddDataProtectionKeys','7.0.5'),('20230411170608_InitialCreate','7.0.5'),('20230426141619_CustomUserData','7.0.5'),('20230426142002_CustomUserDataRetry','7.0.5');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'nestrixdb'
--

--
-- Dumping routines for database 'nestrixdb'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-06-01 16:58:16
