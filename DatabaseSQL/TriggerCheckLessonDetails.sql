CREATE TRIGGER TRG_CheckLessonDetails
ON Lessons
AFTER INSERT, UPDATE
AS
BEGIN
    -- Check if the Car has the same CourseCategoryID as in the Lessons table
    IF EXISTS (
            SELECT *
            FROM Lessons AS l
            INNER JOIN Cars AS c ON l.CarID = c.CarID
            WHERE l.LessonID IN (SELECT LessonID FROM inserted)
                AND c.CategoryID != l.CourseCategoryID
            )
    BEGIN
        RAISERROR('The Car does not have the same CourseCategoryID as in the Lessons table.', 16, 1)
        ROLLBACK
        RETURN
    END

    -- Check if the Instructor has a license for the CourseCategoryID
    IF EXISTS (
            SELECT *
            FROM Lessons AS l
            INNER JOIN Instructors AS i ON l.InstructorID = i.InstructorID
            INNER JOIN InstructorsCategories AS ic ON i.InstructorID = ic.InstructorID
            WHERE l.LessonID IN (SELECT LessonID FROM inserted)
                AND ic.CategoryID != l.CourseCategoryID
                AND NOT EXISTS (
                    SELECT *
                    FROM InstructorsCategories AS ic2
                    WHERE ic2.InstructorID = i.InstructorID
                        AND ic2.CategoryID = l.CourseCategoryID
                )
            )
    BEGIN
        RAISERROR('The Instructor does not have a license for the CourseCategoryID.', 16, 1)
        ROLLBACK
        RETURN
    END

    -- Check if the Student is enrolled in the same CourseCategoryID as in the Lessons table
    IF EXISTS (
            SELECT *
            FROM Lessons AS l
            INNER JOIN Students AS s ON l.StudentID = s.StudentID
            INNER JOIN Courses AS c ON s.StudentID = c.StudentID
            INNER JOIN CourseDetails AS cd ON c.CourseDetailID = cd.CourseDetailID
            WHERE l.LessonID IN (SELECT LessonID FROM inserted)
                AND cd.CategoryID != l.CourseCategoryID
            )
    BEGIN
        RAISERROR('The Student does not have the selected driver''s license category.', 16, 1)
        ROLLBACK
        RETURN
    END

    -- Check if the Car has exceeded the lesson limit for the given day
    IF EXISTS (
            SELECT l.CarID
            FROM Lessons l
            INNER JOIN inserted i ON l.LessonDate = i.LessonDate
            WHERE l.CarID IN (SELECT CarID FROM inserted)
            GROUP BY l.CarID
            HAVING COUNT(*) > 3
            )
    BEGIN
        RAISERROR('The Car has exceeded the lesson limit for the given day.', 16, 1)
        ROLLBACK
        RETURN
    END

    -- Check if the Instructor has exceeded the lesson limit for the given day
    IF EXISTS (
            SELECT l.InstructorID
            FROM Lessons l
            INNER JOIN inserted i ON l.LessonDate = i.LessonDate
            WHERE l.InstructorID IN (SELECT InstructorID FROM inserted)
            GROUP BY l.InstructorID
            HAVING COUNT(*) > 3
            )
    BEGIN
        RAISERROR('The Instructor has exceeded the lesson limit for the given day.', 16, 1)
        ROLLBACK
        RETURN
    END
END;
