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
  Description varchar(1000),
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
  Description varchar(1000) NOT NULL,
  PRIMARY KEY (SiteID)
);

CREATE TABLE FoodPantry (
  SiteID int(16) unsigned NOT NULL,
  HoursOfOperation varchar(1000) NOT NULL,
  ConditionsForUse varchar(1000),
  Description varchar(1000) NOT NULL,
  PRIMARY KEY (SiteID)
);

CREATE TABLE SoupKitchen (
  SiteID int(16) unsigned NOT NULL,
  TotalSeatsAvailable int(16) unsigned NOT NULL DEFAULT '0',
  RemainingSeatsAvailable int(16) unsigned NOT NULL DEFAULT '0',
  HoursOfOperation varchar(1000) NOT NULL,
  ConditionsForUse varchar(1000),
  Description varchar(1000) NOT NULL,
  PRIMARY KEY (SiteID)
);

CREATE TABLE Request (
  RequestID int(16) unsigned NOT NULL AUTO_INCREMENT,
  Username varchar(80) NOT NULL,
  ItemID int(16) unsigned NOT NULL,
  RequestedQuantity int(16) unsigned NOT NULL DEFAULT '0',
  FulfilledQuantity int(16) unsigned NOT NULL DEFAULT '0',
  Status varchar(20) NOT NULL,
  PRIMARY KEY (RequestID)
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
  DateTimeStamp datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  SiteName varchar(250),
  ServiceName varchar(250),
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
  ADD CONSTRAINT request_ibfk_1 FOREIGN KEY (Username) REFERENCES `User` (Username) ON DELETE CASCADE,
  ADD CONSTRAINT request_ibfk_2 FOREIGN KEY (ItemID) REFERENCES `Item` (ItemID) ON DELETE CASCADE;
    
ALTER TABLE `WaitList`
  ADD CONSTRAINT waitlist_ibfk_1 FOREIGN KEY (ClientID) REFERENCES `Client` (ClientID) ON DELETE CASCADE,
  ADD CONSTRAINT waitlist_ibfk_2 FOREIGN KEY (SiteID) REFERENCES `Shelter` (SiteID) ON DELETE CASCADE;
  
ALTER TABLE `ClientLogEntry`
  ADD CONSTRAINT clientlogentry_ibfk_1 FOREIGN KEY (ClientID) REFERENCES `Client` (ClientID) ON DELETE CASCADE;

-- Start seed data inserts

INSERT INTO site (SiteName, StreetAddress, City, State, ZipCode, PrimaryContactNumber)
VALUES ('site1', '111 Main St', 'Fort Lauderdale', 'FL', '33301', '9545551111'),
       ('site2', '222 Main St', 'Miami', 'FL', '33002', '3055552222'),
       ('site3', '333 Main St', 'San Jose', 'CA', '90210', '9055553333');

INSERT INTO user (Username, Password, FirstName, MiddleName, LastName, EmailAddress, SiteID)
VALUES ('emp1', 'gatech123', 'Site1', 'A', 'Employee1', 'emp1@site1.com', 1),
       ('vol1', 'gatech123', 'Site1', 'A', 'Volunteer1', 'vol1@site1.com', 1),
       ('emp2', 'gatech123', 'Site2', 'A', 'Employee2', 'emp2@site2.com', 2),
       ('vol2', 'gatech123', 'Site2', 'A', 'Volunteer2', 'vol2@site2.com', 2),
       ('emp3', 'gatech123', 'Site3', 'A', 'Employee3', 'emp3@site1.com', 3),
       ('vol3', 'gatech123', 'Site3', 'A', 'Volunteer3', 'vol3@site1.com', 3);

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client1', 'Joe', 'A', 'Client1','5555555555');

INSERT INTO client (DescriptiveID, FirstName, MiddleName,LastName, PhoneNumber)
VALUES ('client2', 'Jane', 'B','Client2','1234567890');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client3', 'Joe', 'C', 'Client3','2345678901');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client4', 'Jane', 'D', 'Client4','3456789012');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client5', 'Joe','E', 'Client5','4567890123');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client6', 'Jane', 'F', 'Client6','5678901234');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client7', 'Joe', 'G', 'Client7','67890123456');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client8', 'Jane', 'H','Client8','6789012345');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client9', 'Joe', 'I', 'Client9','0000000000');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client10', 'Jane', 'J', 'Client10', '3456789012');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client11', 'Joe', 'K', 'Client11', '3456789012');

INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber)
VALUES ('client12', 'Jane', 'L', 'Client12', '3456789012');

INSERT INTO foodpantry (SiteID, HoursOfOperation, ConditionsForUse, Description)
VALUES (1, 'Monday-Friday 9am-5pm', 'No jerks allowed', 'pantry1'),
       (3, 'Monday-Friday 10am-6pm', 'No jerks allowed', 'pantry3');

INSERT INTO soupkitchen (SiteID, TotalSeatsAvailable, RemainingSeatsAvailable, HoursOfOperation, ConditionsForUse, Description)
VALUES (2, 30, 25, 'Wednesday to Sunday 7AM to Noon', 'Shirt and shoes required', 'soup2'),
       (3, 60, 12, 'Tuesday to Sunday 8AM to Noon', 'Shirt and shoes required', 'soup3');

INSERT INTO shelter (SiteID, MaleBunksAvailable, FemaleBunksAvailable, MixedBunksAvailable, RoomsAvailable, HoursOfOperation, ConditionsForUse, Description)
VALUES (2, 4, 4, 4, 0, '24hours 7days', 'All are welcome', 'shelter2'),
       (3, 4, 4, 4, 4, '24hours 7days', 'All are welcome', 'shelter3');

INSERT INTO foodbank (SiteID) VALUES (1);
INSERT INTO foodbank (SiteID) VALUES (2);
INSERT INTO foodbank (SiteID) VALUES (3);

INSERT INTO waitlist (ClientID, SiteID, Ranking)
VALUES (9, 3, 1),
       (10, 3, 2),
       (11, 3, 3),
       (12, 3, 4);

       INSERT INTO clientlogentry (ClientID, Description)
VALUES (1, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (2, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (3, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (4, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (5, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (6, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (7, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (8, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (9, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (10, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (11, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (12, 'Profile Created');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (1, 'site1', 'pantry1','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (2, 'site1', 'pantry1','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (3, 'site1', 'pantry1','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (4, 'site1', 'pantry1','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (1, 'site3', 'pantry3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (2, 'site3', 'pantry3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (3, 'site3', 'pantry3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (4, 'site3', 'pantry3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (5, 'site2', 'soup2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (6, 'site2', 'soup2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (7, 'site2', 'soup2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (8, 'site2', 'soup2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (5, 'site3', 'soup3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (6, 'site3', 'soup3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (7, 'site3', 'soup3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (8, 'site3', 'soup3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (9, 'site2', 'shelter2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (10, 'site2', 'shelter2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (11, 'site2', 'shelter2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (12, 'site2', 'shelter2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (9, 'site3', 'shelter3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (10, 'site3', 'shelter3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (11, 'site3', 'shelter3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (12, 'site3', 'shelter3','Visit');

-- Food bank #1 : 10 Food Items: (storage_type= refrigerated, food_catogory=vegetables) (only insert leafy vegetables into this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Leafy vegetable #1', 14, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1),
       ('Leafy vegetable #2', 20, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1),
       ('Leafy vegetable #3', 63, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1),
       ('Leafy vegetable #4', 205, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1),
       ('Leafy vegetable #5', 12, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1),
       ('Leafy vegetable #6', 50, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1),
       ('Leafy vegetable #7', 60, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1),
       ('Leafy vegetable #8', 50, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1),
       ('Leafy vegetable #9', 200, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1),
       ('Leafy vegetable #10', 100, '2017-08-01', 'Food', 'Vegetables', 'Refrigerated', 1);

-- Food bank #1 : 10 Food Items: (storage_type=drygoods, food_catogory=nuts/grains/beans) (only insert nuts products into this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Nut #1', 100, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1),
       ('Nut #2', 10, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1),
       ('Nut #3', 500, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1),
       ('Nut #4', 400, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1),
       ('Nut #5', 20, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1),
       ('Nut #6', 102, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1),
       ('Nut #7', 16, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1),
       ('Nut #8', 40, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1),
       ('Nut #9', 2000, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1),
       ('Nut #10', 170, '2017-09-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 1);

-- Food bank #1 : 10 Food Items: (storage_type=drygoods, food_catogory=sauce/condiments)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Sauce #1', 18, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1),
       ('Sauce #2', 50, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1),
       ('Sauce #3', 11, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1),
       ('Sauce #4', 10, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1),
       ('Sauce #5', 20, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1),
       ('Sauce #6', 40, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1),
       ('Sauce #7', 970, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1),
       ('Sauce #8', 140, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1),
       ('Sauce #9', 70, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1),
       ('Sauce #10', 170, '2017-10-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 1);
      
-- Food bank #1 : 10 Food Items: (storage_type=refrigerated, food_catogory= juice/drink) (only insert soda/pop drinks items in this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Soda #1', 5, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1),
       ('Soda #2', 58, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1),
       ('Soda #3', 30, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1),
       ('Soda #4', 6, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1),
       ('Soda #5', 50, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1),
       ('Soda #6', 5, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1),
       ('Soda #7', 88, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1),
       ('Soda #8', 94, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1),
       ('Soda #9', 54, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1),
       ('Soda #10', 30, '2017-11-01', 'Food', 'Juice/Drink', 'Refrigerated', 1);
      
-- Food bank #1 : 10 Food Items: (storage_type=frozen, food_catogory=meat/seafood) (only insert red-meat only items in this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Steak #1', 3, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1),
       ('Steak #2', 32, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1),
       ('Steak #3', 9, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1),
       ('Steak #4', 8, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1),
       ('Steak #5', 5, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1),
       ('Steak #6', 5, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1),
       ('Steak #7', 7, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1),
       ('Steak #8', 8, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1),
       ('Steak #9', 9, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1),
       ('Steak #10', 30, '2017-12-01', 'Food', 'Meat/seafood', 'Frozen', 1);

-- Food bank #1 : 10 Food Items: (storage_type=refrigerated, food_catogory=dairy/eggs) (only insert cheese products in this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Cheese #1', 29, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1),
       ('Cheese #2', 10, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1),
       ('Cheese #3', 3, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1),
       ('Cheese #4', 120, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1),
       ('Cheese #5', 41, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1),
       ('Cheese #6', 257, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1),
       ('Cheese #7', 37, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1),
       ('Cheese #8', 20, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1),
       ('Cheese #9', 340, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1),
       ('Cheese #10', 30, '2018-01-01', 'Food', 'Dairy/eggs', 'Refrigerated', 1);

-- Food bank #1 : 5 Supply Items: (type: personal hygiene) examples: toothbrush, toothpaste, shampoo, deodorant, soap/detergent, baby wipes, etc.
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('toothbrush', 12, '9999-01-01', 'Supply', 'Personal hygiene', 'Dry Good', 1),
       ('shampoo', 8, '2020-01-01', 'Supply', 'Personal hygiene', 'Dry Good', 1),
       ('deodorant', 12, '2021-01-01', 'Supply', 'Personal hygiene', 'Dry Good', 1),
       ('soap', 34, '9999-01-01', 'Supply', 'Personal hygiene', 'Dry Good', 1),
       ('baby wipes', 30, '2018-06-01', 'Supply', 'Personal hygiene', 'Dry Good', 1);

-- Food bank #1 : 5 Supply Items: (type: clothing) examples: shirts, pants, underwear, etc.
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('shirts', 30, '9999-01-01', 'Supply', 'Clothing', 'Dry Good', 1),
       ('pants', 300, '9999-01-01', 'Supply', 'Clothing', 'Dry Good', 1),
       ('underwear', 36, '9999-01-01', 'Supply', 'Clothing', 'Dry Good', 1),
       ('socks', 42, '9999-01-01', 'Supply', 'Clothing', 'Dry Good', 1),
       ('dress shoes', 14, '9999-01-01', 'Supply', 'Clothing', 'Dry Good', 1);

-- Food bank #2 : 10 Food Items: (storage_type= refrigerated, food_catogory=vegetables) (only insert root veggies into this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Root Veggie #1', 32, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2),
       ('Root Veggie #2', 31, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2),
       ('Root Veggie #3', 62, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2),
       ('Root Veggie #4', 12, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2),
       ('Root Veggie #5', 22, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2),
       ('Root Veggie #6', 66, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2),
       ('Root Veggie #7', 82, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2),
       ('Root Veggie #8', 102, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2),
       ('Root Veggie #9', 30, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2),
       ('Root Veggie #10', 99, '2018-02-01', 'Food', 'Vegetables', 'Refrigerated', 2);
       
-- Food bank #2 : 10 Food Items: (storage_type=drygoods, food_catogory= nuts/grains/beans) (only insert grains only items in this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Grain #1', 32, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2),
       ('Grain #2', 12, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2),
       ('Grain #3', 13, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2),
       ('Grain #4', 77, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2),
       ('Grain #5', 16, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2),
       ('Grain #6', 5, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2),
       ('Grain #7', 5, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2),
       ('Grain #8', 7, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2),
       ('Grain #9', 398, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2),
       ('Grain #10', 123, '2018-03-01', 'Food', 'Nuts/grains/beans', 'Dry Good', 2);
       
-- Food bank #2 : 10 Food Items: (storage_type=drygoods, food_catogory=sauce/condiments)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Ketchup #1', 7, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2),
       ('Ketchup #2', 8, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2),
       ('Ketchup #3', 8, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2),
       ('Ketchup #4', 16, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2),
       ('Ketchup #5', 92, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2),
       ('Ketchup #6', 52, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2),
       ('Ketchup #7', 5, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2),
       ('Ketchup #8', 82, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2),
       ('Ketchup #9', 322, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2),
       ('Ketchup #10', 32, '2018-04-01', 'Food', 'Sauce/Condiment/Seasoning', 'Dry Good', 2);

-- Food bank #2 : 10 Food Items: (storage_type=refrigerated, food_catogory= juice/drink) (only insert fruit juice only items in this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Apple Juice', 30, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2),
       ('Orange Juice', 26, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2),
       ('Grape Juice', 50, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2),
       ('Pineapple Juice', 200, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2),
       ('Banana Juice', 80, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2),
       ('Kiwi Juice', 60, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2),
       ('Pear Juice', 40, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2),
       ('Tangarine Juice', 42, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2),
       ('Grapefruit Juice', 62, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2),
       ('Watermellon Juice', 30, '2018-05-01', 'Food', 'Juice/Drink', 'Refrigerated', 2);

-- Food bank #2 : 10 Food Items: (storage_type=frozen, food_catogory= meat/seafood) (only insert seafood only items in this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('White Fish', 20, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2),
       ('Red Fish', 90, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2),
       ('Green Fish', 50, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2),
       ('Blue Fish', 32, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2),
       ('Orange Fish', 82, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2),
       ('Purple Fish', 7, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2),
       ('Black Fish', 30, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2),
       ('Brown Fish', 10, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2),
       ('Salmon', 36, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2),
       ('Crab Legs', 30, '2018-06-01', 'Food', 'Meat/seafood', 'Frozen', 2);

-- Food bank #2 : 10 Food Items: (storage_type=refrigerated, food_catogory= dairy/eggs) (only insert egg containing products in this bank)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Eggs #1', 40, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2),
       ('Eggs #2', 20, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2),
       ('Eggs #3', 50, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2),
       ('Eggs #4', 90, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2),
       ('Eggs #5', 70, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2),
       ('Eggs #6', 40, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2),
       ('Eggs #7', 80, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2),
       ('Eggs #8', 40, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2),
       ('Eggs #9', 70, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2),
       ('Eggs #10', 42, '2018-07-01', 'Food', 'Dairy/eggs', 'Refrigerated', 2);

-- Food bank #2 : 5 Supply Items: (type: shelter) examples: tent, sleeping bags, blankets, winter jackets, rain coat, etc.
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('tents', 18, '9999-01-01', 'Supply', 'Shelter', 'Dry Good', 2),
       ('sleeping bags', 50, '9999-01-01', 'Supply', 'Shelter', 'Dry Good', 2),
       ('blankets', 99, '9999-01-01', 'Supply', 'Shelter', 'Dry Good', 2),
       ('winter jackets', 42, '9999-01-01', 'Supply', 'Shelter', 'Dry Good', 2),
       ('rain coat', 12, '9999-01-01', 'Supply', 'Shelter', 'Dry Good', 2);
       
-- Food bank #2 : 5 Supply Items: (type: other) examples: paper products, toilet paper, pet food, batteries, etc.
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Napkins', 100, '9999-01-01', 'Supply', 'Other', 'Dry Good', 2),
       ('Dog food', 21, '9999-01-01', 'Supply', 'Other', 'Dry Good', 2),
       ('Cat food', 20, '9999-01-01', 'Supply', 'Other', 'Dry Good', 2),
       ('D Batteries', 17, '9999-01-01', 'Supply', 'Other', 'Dry Good', 2),
       ('AA Batteries', 19, '9999-01-01', 'Supply', 'Other', 'Dry Good', 2);

-- Food bank #3 : 6 Food Items: (storage_type=refrigerated, food_catogory= meat/seafood) (only insert expired chicken products only)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Bad Chicken Legs', 20, NOW() - INTERVAL 10 DAY, 'Food', 'Meat/seafood', 'Refrigerated', 3),
       ('Bad Chicken Breasts', 10, NOW() - INTERVAL 10 DAY, 'Food', 'Meat/seafood', 'Refrigerated', 3),
       ('Bad Chicken Wings', 60, NOW() - INTERVAL 10 DAY, 'Food', 'Meat/seafood', 'Refrigerated', 3),
       ('Bad Chicken Arms', 30, NOW() - INTERVAL 10 DAY, 'Food', 'Meat/seafood', 'Refrigerated', 3),
       ('Bad Chicken Feet', 30, NOW() - INTERVAL 10 DAY, 'Food', 'Meat/seafood', 'Refrigerated', 3),
       ('Bad Chicken Thighs', 40, NOW() - INTERVAL 10 DAY, 'Food', 'Meat/seafood', 'Refrigerated', 3);

-- Food bank #3 : 6 Food Items: (storage_type=refrigerated, food_catogory= dairy/eggs) (only insert expired milk (non-cheese) products only)
INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID)
VALUES ('Bad 1% Milk', 20, NOW() - INTERVAL 10 DAY, 'Food', 'Dairy/eggs', 'Refrigerated', 3),
       ('Bad 2% Milk', 20, NOW() - INTERVAL 10 DAY, 'Food', 'Dairy/eggs', 'Refrigerated', 3),
       ('Bad 3% Milk', 20, NOW() - INTERVAL 10 DAY, 'Food', 'Dairy/eggs', 'Refrigerated', 3),
       ('Bad 4% Milk', 20, NOW() - INTERVAL 10 DAY, 'Food', 'Dairy/eggs', 'Refrigerated', 3),
       ('Bad 5% Milk', 20, NOW() - INTERVAL 10 DAY, 'Food', 'Dairy/eggs', 'Refrigerated', 3),
       ('Bad 6% Milk', 20, NOW() - INTERVAL 10 DAY, 'Food', 'Dairy/eggs', 'Refrigerated', 3);

-- Pending Requests from Employee Users (for 'site1', 'site2', and 'site3')
-- 'emp1' (bankID=1) 10 requests from non-associated bankID=2 to 'site1' (bankID=2 specific food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp1', 71, 3, 0, 'Pending'),
       ('emp1', 74, 10, 0, 'Pending'),
       ('emp1', 78, 101, 0, 'Pending'),
       ('emp1', 84, 7, 0, 'Pending'),
       ('emp1', 87, 3, 0, 'Pending'),
       ('emp1', 91, 7, 0, 'Pending'),
       ('emp1', 106, 6, 0, 'Pending'),
       ('emp1', 119, 16, 0, 'Pending'),
       ('emp1', 126, 4, 0, 'Pending'),
       ('emp1', 130, 42, 0, 'Pending');

-- 'emp1' 4 request from non-associated bankID=3 to 'site1' (bankID=3 expired food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp1', 141, 10, 0, 'Pending'),
       ('emp1', 144, 30, 0, 'Pending'),
       ('emp1', 147, 20, 0, 'Pending'),
       ('emp1', 148, 5, 0, 'Pending');
       
-- 'emp2' (bankID=2) 10 requests from non-associated bankID=1 to 'site2' (bankID=1 specific food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp2', 1, 14, 0, 'Pending'),
       ('emp2', 10, 10, 0, 'Pending'),
       ('emp2', 12, 10, 0, 'Pending'),
       ('emp2', 19, 190, 0, 'Pending'),
       ('emp2', 21, 5, 0, 'Pending'),
       ('emp2', 24, 10, 0, 'Pending'),
       ('emp2', 31, 5, 0, 'Pending'),
       ('emp2', 32, 5, 0, 'Pending'),
       ('emp2', 51, 10, 0, 'Pending'),
       ('emp2', 52, 10, 0, 'Pending');
        
-- 'emp2' 4 request from non-associated bankID=3 to 'site2' (bankID=3 expired food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp2', 143, 60, 0, 'Pending'),
       ('emp2', 144, 10, 0, 'Pending'),
       ('emp2', 145, 10, 0, 'Pending'),
       ('emp2', 150, 10, 0, 'Pending');

-- 'emp2' 7 requests from non-associated bankID=1 to 'site2' (bankID=1 supply items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp2', 61, 12, 0, 'Pending'),
       ('emp2', 62, 6, 0, 'Pending'),
	   ('emp2', 63, 12, 0, 'Pending'),
       ('emp2', 64, 10, 0, 'Pending'),
       ('emp2', 69, 42, 0, 'Pending'),
       ('emp2', 70, 14, 0, 'Pending'),
       ('emp2', 68, 10, 0, 'Pending');
  
-- 'emp3' (bankID=3) 10 requests from non-associated bankID=1 to 'site3' (bankID=1 specific food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp3', 5, 12, 0, 'Pending'),
       ('emp3', 6, 5, 0, 'Pending'),
	   ('emp3', 7, 10, 0, 'Pending'),
       ('emp3', 24, 10, 0, 'Pending'),
       ('emp3', 25, 1, 0, 'Pending'),
       ('emp3', 31, 1, 0, 'Pending'),
       ('emp3', 32, 58, 0, 'Pending'),
       ('emp3', 33, 3, 0, 'Pending'),
       ('emp3', 41, 3, 0, 'Pending'),
       ('emp3', 42, 32, 0, 'Pending');

-- 'emp3' 10 requests from non-associated bankID=2 to 'site3' (bankID=2 specific food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp3', 78, 10, 0, 'Pending'),
       ('emp3', 79, 10, 0, 'Pending'),
       ('emp3', 80, 20, 0, 'Pending'),
       ('emp3', 89, 200, 0, 'Pending'),
       ('emp3', 90, 40, 0, 'Pending'),
       ('emp3', 95, 40, 0, 'Pending'),
       ('emp3', 96, 52, 0, 'Pending'),
       ('emp3', 104, 198, 0, 'Pending'),
       ('emp3', 105, 8, 0, 'Pending'),
       ('emp3', 112, 9, 0, 'Pending');

-- 'emp3' 7 requests from non-associated bankID=1 (bankID=1 specific supply items only) to 'site3'
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp3', 67, 20, 0, 'Pending'),
       ('emp3', 68, 30, 0, 'Pending'),
       ('emp3', 69, 10, 0, 'Pending'),
       ('emp3', 70, 14, 0, 'Pending'),
       ('emp3', 66, 10, 0, 'Pending'),
       ('emp3', 65, 10, 0, 'Pending'),
       ('emp3', 64, 10, 0, 'Pending');

-- 'emp3' 7 requests from non-associated bankID=2 (bankID=2 specific supply items only) to 'site3'
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp3', 131, 5, 0, 'Pending'),
       ('emp3', 132, 10, 0, 'Pending'),
       ('emp3', 133, 10, 0, 'Pending'),
       ('emp3', 134, 10, 0, 'Pending'),
       ('emp3', 135, 12, 0, 'Pending'),
       ('emp3', 136, 20, 0, 'Pending'),
       ('emp3', 137, 10, 0, 'Pending');

-- 4 Closed Requests per Employee User
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('emp1', 2, 5, 0, 'Closed'),
       ('emp1', 81, 57, 57, 'Closed'),
	   ('emp1', 132, 1000, 500, 'Closed'),
	   ('emp1', 140, 12, 12, 'Closed'),
	   ('emp2', 152, 5, 5, 'Closed'),
	   ('emp2', 142, 12, 0, 'Closed'),
	   ('emp2', 16, 54, 54, 'Closed'),
	   ('emp2', 22, 2, 2, 'Closed'),
	   ('emp3', 108, 50, 00, 'Closed'),
	   ('emp3', 117, 6, 0, 'Closed'),
	   ('emp3', 33, 20, 15, 'Closed'),
	   ('emp3', 44, 80, 80, 'Closed');

-- Of 3 Volunteer Users above, each will have:
-- Pending Requests for Volunteer Users
-- **Same as Employee requests, just change 'emp#' to 'vol#'**
-- 4 Closed Requests per Volunteer Users

INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol1', 71, 1, 0, 'Pending'),
       ('vol1', 74, 2, 0, 'Pending'),
       ('vol1', 78, 1, 0, 'Pending'),
       ('vol1', 84, 1 0, 'Pending'),
       ('vol1', 87, 1, 0, 'Pending'),
       ('vol1', 91, 1, 0, 'Pending'),
       ('vol1', 106, 2, 0, 'Pending'),
       ('vol1', 119, 1, 0, 'Pending'),
       ('vol1', 126, 1, 0, 'Pending'),
       ('vol1', 130, 1, 0, 'Pending');

-- 'vol1' 4 request from non-associated bankID=3 to 'site1' (bankID=3 expired food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol1', 141, 3, 0, 'Pending'),
       ('vol1', 144, 2, 0, 'Pending'),
       ('vol1', 147, 1, 0, 'Pending'),
       ('vol1', 148, 2, 0, 'Pending');
       
-- 'vol2' (bankID=2) 10 requests from non-associated bankID=1 to 'site2' (bankID=1 specific food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol2', 1, 2, 0, 'Pending'),
       ('vol2', 10, 2, 0, 'Pending'),
       ('vol2', 12, 1, 0, 'Pending'),
       ('vol2', 19, 3, 0, 'Pending'),
       ('vol2', 21, 4, 0, 'Pending'),
       ('vol2', 24, 2, 0, 'Pending'),
       ('vol2', 31, 5, 0, 'Pending'),
       ('vol2', 32, 8, 0, 'Pending'),
       ('vol2', 51, 5, 0, 'Pending'),
       ('vol2', 52, 4, 0, 'Pending');
        
-- 'vol2' 4 request from non-associated bankID=3 to 'site2' (bankID=3 expired food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol2', 143, 5, 0, 'Pending'),
       ('vol2', 144, 4, 0, 'Pending'),
       ('vol2', 145, 4, 0, 'Pending'),
       ('vol2', 150, 4, 0, 'Pending');

-- 'vol2' 7 requests from non-associated bankID=1 to 'site2' (bankID=1 supply items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol2', 61, 5, 0, 'Pending'),
       ('vol2', 62, 6, 0, 'Pending'),
	   ('vol2', 63, 6, 0, 'Pending'),
       ('vol2', 64, 4, 0, 'Pending'),
       ('vol2', 69, 1, 0, 'Pending'),
       ('vol2', 70, 2, 0, 'Pending'),
       ('vol2', 68, 1, 0, 'Pending');
  
-- 'vol3' (bankID=3) 10 requests from non-associated bankID=1 to 'site3' (bankID=1 specific food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol3', 5, 6, 0, 'Pending'),
       ('vol3', 6, 5, 0, 'Pending'),
	   ('vol3', 7, 5, 0, 'Pending'),
       ('vol3', 24, 5, 0, 'Pending'),
       ('vol3', 25, 1, 0, 'Pending'),
       ('vol3', 31, 1, 0, 'Pending'),
       ('vol3', 32, 2, 0, 'Pending'),
       ('vol3', 33, 3, 0, 'Pending'),
       ('vol3', 41, 3, 0, 'Pending'),
       ('vol3', 42, 6, 0, 'Pending');

-- 'vol3' 10 requests from non-associated bankID=2 to 'site3' (bankID=2 specific food items only)
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol3', 78, 5, 0, 'Pending'),
       ('vol3', 79, 4, 0, 'Pending'),
       ('vol3', 80, 5, 0, 'Pending'),
       ('vol3', 89, 8, 0, 'Pending'),
       ('vol3', 90, 5, 0, 'Pending'),
       ('vol3', 95, 4, 0, 'Pending'),
       ('vol3', 96, 5, 0, 'Pending'),
       ('vol3', 104, 2, 0, 'Pending'),
       ('vol3', 105, 1, 0, 'Pending'),
       ('vol3', 112, 2, 0, 'Pending');

-- 'vol3' 7 requests from non-associated bankID=1 (bankID=1 specific supply items only) to 'site3'
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol3', 67, 4, 0, 'Pending'),
       ('vol3', 68, 2, 0, 'Pending'),
       ('vol3', 69, 5, 0, 'Pending'),
       ('vol3', 70, 4, 0, 'Pending'),
       ('vol3', 66, 3, 0, 'Pending'),
       ('vol3', 65, 2, 0, 'Pending'),
       ('vol3', 64, 2, 0, 'Pending');

-- 'vol3' 7 requests from non-associated bankID=2 (bankID=2 specific supply items only) to 'site3'
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol3', 131, 5, 0, 'Pending'),
       ('vol3', 132, 5, 0, 'Pending'),
       ('vol3', 133, 5, 0, 'Pending'),
       ('vol3', 134, 5, 0, 'Pending'),
       ('vol3', 135, 5, 0, 'Pending'),
       ('vol3', 136, 5, 0, 'Pending'),
       ('vol3', 137, 5, 0, 'Pending');

-- 4 Closed Requests per volloyee User
INSERT INTO request (Username, ItemID, RequestedQuantity, FulfilledQuantity, Status)
VALUES ('vol1', 2, 5, 0, 'Closed'),
       ('vol1', 81, 5, 5, 'Closed'),
	   ('vol1', 132, 1000, 500, 'Closed'),
	   ('vol1', 140, 12, 12, 'Closed'),
	   ('vol2', 152, 5, 5, 'Closed'),
	   ('vol2', 142, 12, 0, 'Closed'),
	   ('vol2', 16, 54, 54, 'Closed'),
	   ('vol2', 22, 2, 2, 'Closed'),
	   ('vol3', 108, 50, 00, 'Closed'),
	   ('vol3', 117, 6, 0, 'Closed'),
	   ('vol3', 33, 20, 15, 'Closed'),
	   ('vol3', 44, 80, 80, 'Closed');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (8, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (9, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (10, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (11, 'Profile Created');

INSERT INTO clientlogentry (ClientID, Description)
VALUES (12, 'Profile Created');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (1, 'site1', 'pantry1','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (2, 'site1', 'pantry1','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (3, 'site1', 'pantry1','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (4, 'site1', 'pantry1','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (1, 'site3', 'pantry3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (2, 'site3', 'pantry3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (3, 'site3', 'pantry3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (4, 'site3', 'pantry3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (5, 'site2', 'soup2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (6, 'site2', 'soup2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (7, 'site2', 'soup2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (8, 'site2', 'soup2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (5, 'site3', 'soup3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (6, 'site3', 'soup3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (7, 'site3', 'soup3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (8, 'site3', 'soup3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (9, 'site2', 'shelter2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (10, 'site2', 'shelter2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (11, 'site2', 'shelter2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (12, 'site2', 'shelter2','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (9, 'site3', 'shelter3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (10, 'site3', 'shelter3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (11, 'site3', 'shelter3','Visit');

INSERT INTO clientlogentry (ClientID, SiteName, ServiceName, Description)
VALUES (12, 'site3', 'shelter3','Visit');
