﻿CREATE TABLE Instructors (
	InstructorID INT IDENTITY(1, 1) PRIMARY KEY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Email VARCHAR(50) NOT NULL UNIQUE,
	Phone CHAR(9) NOT NULL UNIQUE,

	CONSTRAINT CHK_InstructorEmail CHECK (email LIKE '%_@__%.__%'),
	CONSTRAINT CHK_InstructorPhone CHECK(Phone LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
);

CREATE TABLE Students (
	StudentID INT IDENTITY(1, 1) PRIMARY KEY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Email VARCHAR(50) NOT NULL UNIQUE,
	Phone CHAR(9) NOT NULL UNIQUE,
	BirthDate DATE NOT NULL,

	CONSTRAINT CHK_StudentEmail CHECK (email LIKE '%_@__%.__%'),
	CONSTRAINT CHK_StudentPhone CHECK(Phone LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	CONSTRAINT CHK_StudentBirthDate CHECK(BirthDate < GETDATE())
);

CREATE TABLE Cars (
	CarID INT IDENTITY(1, 1) PRIMARY KEY,
	Mark VARCHAR(20) NOT NULL,
	Model VARCHAR(20) NOT NULL,
	RegistrationNumber CHAR(7) UNIQUE NOT NULL,
	ReviewDate DATE NOT NULL,
	CategoryID INT NOT NULL,

	CONSTRAINT CHK_CarRegistrationNumber CHECK(RegistrationNumber LIKE '[A-Z][A-Z][A-Z][A-Z0-9][A-Z0-9][A-Z0-9][A-Z0-9]'),
	CONSTRAINT CHK_CarReviewDate CHECK(ReviewDate > GETDATE()),
);

CREATE TABLE Lessons (
	LessonID INT IDENTITY(1, 1) PRIMARY KEY,
	LessonDate DATE NOT NULL,
	Title VARCHAR(100) NOT NULL,
	HoursNumber TINYINT DEFAULT 3,
	CourseCategoryID INT NOT NULL,
	InstructorID INT NOT NULL,
	CarID INT NOT NULL,
	StudentID1 INT NOT NULL,
	StudentID2 INT,
	StudentID3 INT,

	CONSTRAINT UNQ_LessonInstructor UNIQUE(LessonDate, InstructorID),
	CONSTRAINT UNQ_LessonCar UNIQUE(LessonDate, CarID),
	CONSTRAINT CHK_LessonDate CHECK(LessonDate >= GETDATE()),
	CONSTRAINT CHK_LessonHoursNumber CHECK(HoursNumber > 0),
	CONSTRAINT CHK_LessonStudentID CHECK (StudentID1 != StudentID2 AND StudentID1 != StudentID3 AND (StudentID2 != StudentID3 OR StudentID2 IS NULL OR StudentID3 IS NULL))
);

CREATE TABLE Categories (
	CategoryID INT IDENTITY(1,1) PRIMARY KEY,
	CategoryName VARCHAR(5) NOT NULL UNIQUE,
);

CREATE TABLE Courses (
	StudentID INT NOT NULL,
	CourseDetailID INT NOT NULL,
	
	CONSTRAINT PK_Courses PRIMARY KEY (StudentID, CourseDetailID)
);

CREATE TABLE CourseDetails (
	CourseDetailID INT IDENTITY(1,1) PRIMARY KEY,
	MinimumYears INT NOT NULL,
	MinimumMonths INT NOT NULL,
	HoursNumber INT NOT NULL,
	CategoryID INT UNIQUE NOT NULL,

	CONSTRAINT CHK_CourseDetailMinimumYears CHECK(MinimumYears > 0),
	CONSTRAINT CHK_CourseDetailMinimumMonths CHECK(MinimumMonths >= 0)
);

CREATE TABLE InstructorsCategories (
	InstructorID INT NOT NULL,
	CategoryID INT NOT NULL,

	CONSTRAINT PK_InstructorsCategories PRIMARY KEY (InstructorID, CategoryID)
);

ALTER TABLE Cars ADD CONSTRAINT FK_Cars_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) 
	ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE Lessons ADD CONSTRAINT FK_Lessons_Instructors FOREIGN KEY (InstructorID) REFERENCES Instructors(InstructorID)
	ON DELETE CASCADE ON UPDATE NO ACTION;

ALTER TABLE Lessons ADD CONSTRAINT FK_Lessons_Categories FOREIGN KEY (CourseCategoryID) REFERENCES Categories(CategoryID)
	ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE Lessons ADD	CONSTRAINT FK_Lessons_Cars FOREIGN KEY (CarID) REFERENCES Cars(CarID)
	ON DELETE CASCADE ON UPDATE NO ACTION;

ALTER TABLE Lessons ADD	CONSTRAINT FK_Lessons_Students_StudentID1 FOREIGN KEY (StudentID1) REFERENCES Students(StudentID)
	ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE Lessons ADD	CONSTRAINT FK_Lessons_Students_StudentID2 FOREIGN KEY (StudentID2) REFERENCES Students(StudentID)
	ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE Lessons ADD CONSTRAINT FK_Lessons_Students_StudentID3 FOREIGN KEY (StudentID3) REFERENCES Students(StudentID)
	ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE Courses ADD CONSTRAINT FK_Courses_Students FOREIGN KEY (StudentID) REFERENCES Students(StudentID)
	ON DELETE CASCADE ON UPDATE NO ACTION;

ALTER TABLE Courses ADD	CONSTRAINT FK_Courses_CourseDetails FOREIGN KEY (CourseDetailID) REFERENCES CourseDetails(CourseDetailID)
	ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE CourseDetails ADD CONSTRAINT FK_CourseDetails_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
	ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE InstructorsCategories ADD CONSTRAINT FK_InstructorsCategories_Instructors FOREIGN KEY (InstructorID) REFERENCES Instructors(InstructorID)
	ON DELETE CASCADE ON UPDATE NO ACTION;

ALTER TABLE InstructorsCategories ADD CONSTRAINT FK_InstructorsCategories_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
	ON DELETE NO ACTION ON UPDATE NO ACTION;
