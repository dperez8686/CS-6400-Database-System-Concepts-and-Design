DROP DATABASE IF EXISTS `cs6400_sp17_team090`; 
/* 
Optional: MySQL centric items 
MySQL: DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
MySQL Storage Engines: SET default_storage_engine=InnoDB;
Note: "IF EXISTS" is not universal, and the "IF NOT EXISTS" is uncommonly supported, so this functionaly may not work if outside MySQL RDBMS.

Resources:
https://dev.mysql.com/doc/refman/5.7/en/storage-engines.html
https://bitnami.com/stacks/infrastructure
https://www.jetbrains.com/phpstorm/
http://www.w3schools.com/
*/

SET default_storage_engine=InnoDB;


CREATE DATABASE IF NOT EXISTS cs6400_sp17_team090 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE cs6400_sp17_team090;

CREATE TABLE `User` (
  Username varchar(80) NOT NULL,
  Password varchar(250) NOT NULL,
  FirstName varchar(250) NOT NULL,
  MiddleName varchar(250),
  LastName varchar(250) NOT NULL,
  EmailAddress varchar(250) NOT NULL,
  SiteID int(16) unsigned NOT NULL,
  PRIMARY KEY (Username)
);

CREATE TABLE Site (
  SiteID int(16) unsigned NOT NULL AUTO_INCREMENT,
  SiteName varchar(250) NOT NULL,
  StreetAddress varchar(250) NOT NULL,
  City varchar(250) NOT NULL,
  `State` varchar(250) NOT NULL,
  ZipCode varchar(250) NOT NULL,
  PrimaryContactNumber varchar(250) NOT NULL,
  PRIMARY KEY (SiteID)
);

CREATE TABLE FoodBank (
  SiteID int(16) unsigned NOT NULL,
  PRIMARY KEY (SiteID)
);

CREATE TABLE Shelter (
  SiteID int(16) unsigned NOT NULL,
  MaleBunksAvailable int(16) unsigned NOT NULL DEFAULT '0',
  FemaleBunksAvailable int(16) unsigned NOT NULL DEFAULT '0',
  MixedBunksAvailable int(16) unsigned NOT NULL DEFAULT '0',
  RoomsAvailable int(16) unsigned NOT NULL DEFAULT '0',
  HoursOfOperation varchar(1000) NOT NULL,
  ConditionsForUse varchar(1000),
  PRIMARY KEY (SiteID)
);

CREATE TABLE FoodPantry (
  SiteID int(16) unsigned NOT NULL,
  HoursOfOperation varchar(1000) NOT NULL,
  ConditionsForUse varchar(1000),
  PRIMARY KEY (SiteID)
);

CREATE TABLE SoupKitchen (
  SiteID int(16) unsigned NOT NULL,
  TotalSeatsAvailable int(16) unsigned NOT NULL DEFAULT '0',
  RemainingSeatsAvailable int(16) unsigned NOT NULL DEFAULT '0',
  HoursOfOperation varchar(1000) NOT NULL,
  ConditionsForUse varchar(1000),
  PRIMARY KEY (SiteID)
);

CREATE TABLE Request (
  Username varchar(80) NOT NULL,
  ItemID int(16) unsigned NOT NULL,
  RequestedQuantity int(16) unsigned NOT NULL DEFAULT '0',
  FulfilledQuantity int(16) unsigned NOT NULL DEFAULT '0',
  Status varchar(20) NOT NULL,
  PRIMARY KEY (Username,ItemID)
);

CREATE TABLE Item (
  ItemID int(16) unsigned NOT NULL AUTO_INCREMENT,
  ItemName varchar(250) NOT NULL,
  NumberOfUnits int(16) unsigned NOT NULL DEFAULT '0',
  ExpirationDate datetime NOT NULL,
  Category1 varchar(80) NOT NULL,
  Category2 varchar(80) NOT NULL,
  StorageType varchar(80) NOT NULL,
  SiteID int(16) unsigned NOT NULL,
  PRIMARY KEY (ItemID)
);

CREATE TABLE Client (
  ClientID int(16) unsigned NOT NULL AUTO_INCREMENT,
  DescriptiveID varchar(250) NOT NULL,
  FirstName varchar(250) NOT NULL,
  MiddleName varchar(250),
  LastName varchar(250) NOT NULL,
  PhoneNumber varchar(80),
  PRIMARY KEY (ClientID),
  UNIQUE KEY DescriptiveID (DescriptiveID)
);

CREATE TABLE WaitList (
  ClientID int(16) unsigned NOT NULL,
  SiteID int(16) unsigned NOT NULL,
  Ranking int(16) unsigned NOT NULL,
  PRIMARY KEY (ClientID,SiteID)
);

CREATE TABLE ClientLogEntry (
  LogID int(16) unsigned NOT NULL AUTO_INCREMENT,
  ClientID int(16) unsigned NOT NULL,
  DateTimeStamp datetime NOT NULL,
  SiteName varchar(250) NOT NULL,
  Description varchar(1000) NOT NULL,
  PRIMARY KEY (LogID)
);

--  Table Constraints 

ALTER TABLE `User`
  ADD CONSTRAINT user_ibfk_1 FOREIGN KEY (SiteID) REFERENCES `Site` (SiteID);
  
ALTER TABLE `FoodBank`
  ADD CONSTRAINT foodbank_ibfk_1 FOREIGN KEY (SiteID) REFERENCES `Site` (SiteID);
  
ALTER TABLE `Shelter`
  ADD CONSTRAINT shelter_ibfk_1 FOREIGN KEY (SiteID) REFERENCES `Site` (SiteID);
  
ALTER TABLE `FoodPantry`
  ADD CONSTRAINT foodpantry_ibfk_1 FOREIGN KEY (SiteID) REFERENCES `Site` (SiteID);
  
ALTER TABLE `SoupKitchen`
  ADD CONSTRAINT soupkitchen_ibfk_1 FOREIGN KEY (SiteID) REFERENCES `Site` (SiteID);

ALTER TABLE `Request`
  ADD CONSTRAINT request_ibfk_1 FOREIGN KEY (Username) REFERENCES `User` (Username),
  ADD CONSTRAINT request_ibfk_2 FOREIGN KEY (ItemID) REFERENCES `Item` (ItemID);
    
ALTER TABLE `WaitList`
  ADD CONSTRAINT waitlist_ibfk_1 FOREIGN KEY (ClientID) REFERENCES `Client` (ClientID),
  ADD CONSTRAINT waitlist_ibfk_2 FOREIGN KEY (SiteID) REFERENCES `Site` (SiteID);
  
ALTER TABLE `ClientLogEntry`
  ADD CONSTRAINT clientlogentry_ibfk_1 FOREIGN KEY (ClientID) REFERENCES `Client` (ClientID);

INSERT INTO site (SiteName, StreetAddress, City, State, ZipCode, PrimaryContactNumber)
VALUES ('Food City #1', '55 Andrews Ave', 'Fort Lauderdale', 'FL', '33301', '9545556783');

INSERT INTO site (SiteName, StreetAddress, City, State, ZipCode, PrimaryContactNumber)
VALUES ('Main Street Cafe', '123 Main St', 'Fort Lauderdale', 'FL', '33301', '9545556789');

INSERT INTO user (Username, Password, FirstName, MiddleName, LastName, EmailAddress, SiteID)
VALUES ('jeffro96', 'admin', 'Jeff', 'P', 'Ross', 'jross323@gmail.com', 1);

INSERT INTO user (Username, Password, FirstName, MiddleName, LastName, EmailAddress, SiteID)
VALUES ('billybob1', 'admin', 'Billy', 'G', 'Bob', 'billy.g.bob@gmail.com', 2);

INSERT INTO foodbank (SiteID) VALUES (1);

INSERT INTO foodbank (SiteID) VALUES (2);

INSERT INTO soupkitchen (SiteID, TotalSeatsAvailable, RemainingSeatsAvailable, HoursOfOperation, ConditionsForUse)
VALUES (1, 30, 18, 'Wednesday to Sunday 7AM to Noon', 'Shirt and shoes required');

INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Carrots', 5, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1);

INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Salmon', 5, '2017-08-15', 'Food', 'Meat/seafood', 'Frozen', 1);

INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Beans', 12, '2019-01-03', 'Food', 'Vegetables', 'Dry Good', 1);

INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Soap', 100, '9999-01-01', 'Supply', 'Personal hygiene', 'Dry Good', 1);

INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('T-shirts', 35, '9999-01-01', 'Supply', 'Clothing', 'Dry Good', 2);

INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Ice cream', 2, '2018-03-01', 'Food', 'Dairy/eggs', 'Frozen', 2);

