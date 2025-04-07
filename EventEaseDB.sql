--DATABASE CREATION
USE master
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'EventEaseDB')
DROP DATABASE EventEaseDB
CREATE DATABASE EventEaseDB

USE EventEaseDB

--TABLE CREATION
CREATE TABLE Venue(
	Id INT IDENTITY(1,1) PRIMARY KEY, 
	VenueName VARCHAR (250) NOT NULL,
	Location VARCHAR (250) NOT NULL,
	Capacity INT,
	ImageURL VARCHAR (250) NOT NULL
);

CREATE TABLE Event(
	Id INT IDENTITY(1,1) PRIMARY KEY, 
	VenueId INT NOT NULL,
	EventName VARCHAR (250) NOT NULL,
	EventDate DATE NOT NULL,
	Description VARCHAR (250) NOT NULL,
	CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueId) REFERENCES Venue(Id), 
);

CREATE TABLE Booking(
	Id INT IDENTITY(1,1) PRIMARY KEY, 
	VenueId INT NOT NULL, 
	EventId INT NOT NULL, 
	BookingDate DATE NOT NULL,
	CONSTRAINT FK_Booking_Venue FOREIGN KEY (VenueId) REFERENCES Venue(Id), 
	CONSTRAINT FK_Booking_Event FOREIGN KEY (EventId) REFERENCES Event(Id) 
);

--Insert sample data 
INSERT INTO Venue (VenueName,Location, Capacity, ImageURL) 
VALUES ('Billiards', '27 Clifton Street', 70, 'htps://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.facebook.com%2Fbilliardcafesa%2F&psig=AOvVaw0AImhiBzrTa5XMFFvpudRW&ust=1744116194122000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCNDhlaz5xYwDFQAAAAAdAAAAABAE')

INSERT INTO Event (VenueId, EventName,EventDate, Description)
VALUES (1, 'Jeremys Pool Tournament', '2025-04-09', 'Jeremy wants to have a pool tournament at Billiards')

INSERT INTO Booking (VenueId, EventId,BookingDate)
VALUES (1, 1, '2025-04-09')