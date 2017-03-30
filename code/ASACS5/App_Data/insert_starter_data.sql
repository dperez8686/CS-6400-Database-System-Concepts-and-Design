INSERT INTO site (SiteName, StreetAddress, City, State, ZipCode, PrimaryContactNumber)
VALUES ('Food City #2', '123 Main St', 'Fort Lauderdale', 'FL', '33301', '9545556789');

INSERT INTO user (Username, Password, FirstName, MiddleName, LastName, EmailAddress, SiteID)
VALUES ('jeffro96', 'admin', 'Jeff', 'P', 'Ross', 'jross323@gmail.com', 1);

INSERT INTO soupkitchen (SiteID, TotalSeatsAvailable, RemainingSeatsAvailable, HoursOfOperaion, ConditionsForUse)
VALUES (1, 30, 18, 'Wednesday to Sunday 7AM to Noon', 'Shirt and shoes required');