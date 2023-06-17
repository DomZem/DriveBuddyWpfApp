-- Categories
INSERT INTO Categories VALUES('AM');
INSERT INTO Categories VALUES('A1');
INSERT INTO Categories VALUES('A2');
INSERT INTO Categories VALUES('A');
INSERT INTO Categories VALUES('B1');
INSERT INTO Categories VALUES('B');
INSERT INTO Categories VALUES('C1');
INSERT INTO Categories VALUES('C');
INSERT INTO Categories VALUES('D1');
INSERT INTO Categories VALUES('D');
INSERT INTO Categories VALUES('BE');
INSERT INTO Categories VALUES('C1E');
INSERT INTO Categories VALUES('CE');
INSERT INTO Categories VALUES('D1E');
INSERT INTO Categories VALUES('DE');
INSERT INTO Categories VALUES('T');

-- CourseDetails
INSERT INTO CourseDetails VALUES(13, 9, 10, 1);
INSERT INTO CourseDetails VALUES(15, 9, 20, 2);
INSERT INTO CourseDetails VALUES(17, 9, 20, 3);
INSERT INTO CourseDetails VALUES(23, 9, 20, 4);
INSERT INTO CourseDetails VALUES(17, 9, 30, 5);
INSERT INTO CourseDetails VALUES(16, 0, 30, 6);
INSERT INTO CourseDetails VALUES(19, 9, 30, 7);
INSERT INTO CourseDetails VALUES(21, 0, 30, 8);
INSERT INTO CourseDetails VALUES(21, 0, 40, 9);
INSERT INTO CourseDetails VALUES(24, 0, 40, 10);
INSERT INTO CourseDetails VALUES(17, 9, 30, 11);
INSERT INTO CourseDetails VALUES(17, 9, 35, 12);
INSERT INTO CourseDetails VALUES(21, 0, 30, 13);
INSERT INTO CourseDetails VALUES(21, 0, 30, 14);
INSERT INTO CourseDetails VALUES(24, 0, 35, 15);
INSERT INTO CourseDetails VALUES(16, 0, 20, 16);

-- Instructors
INSERT INTO Instructors VALUES('Bruno', 'Chojnacki', 'bruno.chojnacki@gmail.com', '680616620');
INSERT INTO Instructors VALUES('Walenty', 'Wilczyński', 'walenty.wilczynski@gmail.com', '736703530');
INSERT INTO Instructors VALUES('Rozalia', 'Nawrocka', 'rozalia.nawrocka@gmail.com', '774157400');
INSERT INTO Instructors VALUES('Anna', 'Brzezińska', 'anna.brzezinska@gmail.com', '716821872');

-- InstructorsCategories
INSERT INTO InstructorsCategories VALUES(1, 5);
INSERT INTO InstructorsCategories VALUES(1, 6);
INSERT INTO InstructorsCategories VALUES(1, 7);
INSERT INTO InstructorsCategories VALUES(2, 5);
INSERT INTO InstructorsCategories VALUES(2, 11);

-- Students
INSERT INTO Students VALUES('Grzegorz', 'Wilk', 'grzegorz.wilk@gmail.com', '816303016', '2002-11-16');
INSERT INTO Students VALUES('Paula', 'Kozioł', 'paula.koziol@gmail.com', '596900325', '1995-09-18');
INSERT INTO Students VALUES('Julianna', 'Wrona', 'julianna.wrona@gmail.com', '818847602', '2001-07-07');
INSERT INTO Students VALUES('Norbert', 'Nawrocki', 'norbert.nawrocki@gmail.com', '872506253', '1998-03-21');

-- Courses
INSERT INTO Courses VALUES(1, 5);
INSERT INTO Courses VALUES(2, 5);
INSERT INTO Courses VALUES(3, 11);
INSERT INTO Courses VALUES(4, 6);

-- Cars
INSERT INTO Cars VALUES('Toyota', 'Yaris', 'KRK7321', GETDATE()+100, 5);
INSERT INTO Cars VALUES('Toyota', 'Yaris', 'KRK0928', GETDATE()+200, 11);
INSERT INTO Cars VALUES('Hyundai', 'i20', 'KRK2342', GETDATE()+300, 11);
INSERT INTO Cars VALUES('Hyundai', 'i20', 'KRK5331', GETDATE()+400, 5);

-- Lessons
INSERT INTO Lessons VALUES (GETDATE()+1, 'Jazda po mieście', 3, 5, 1, 1, 1);
INSERT INTO Lessons VALUES (GETDATE()+1, 'Jazda po mieście', 3, 11, 2, 2, 3);
INSERT INTO Lessons VALUES (GETDATE()+2, 'Jazda po placu manewrowym', 3, 5, 1, 4, 1);
INSERT INTO Lessons VALUES (GETDATE()+2, 'Jazda po placu manewrowym', 3, 11, 2, 3, 3);
INSERT INTO Lessons VALUES (GETDATE()+3, 'Jazda po mieście', 3, 5, 2, 1, 2);