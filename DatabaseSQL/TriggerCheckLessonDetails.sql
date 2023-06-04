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

    -- Check if the Students are enrolled in the same CourseCategoryID as in the Lessons table
    IF EXISTS (
            SELECT *
            FROM Lessons AS l
            INNER JOIN Courses AS c ON l.StudentID1 = c.StudentID
            LEFT JOIN Courses AS c2 ON l.StudentID2 = c2.StudentID
            LEFT JOIN Courses AS c3 ON l.StudentID3 = c3.StudentID
            WHERE l.LessonID IN (SELECT LessonID FROM inserted)
                AND (
                    c.CourseDetailID NOT IN (SELECT CourseDetailID FROM CourseDetails WHERE CategoryID = l.CourseCategoryID)
                    OR (c2.CourseDetailID IS NOT NULL AND c2.CourseDetailID NOT IN (SELECT CourseDetailID FROM CourseDetails WHERE CategoryID = l.CourseCategoryID))
                    OR (c3.CourseDetailID IS NOT NULL AND c3.CourseDetailID NOT IN (SELECT CourseDetailID FROM CourseDetails WHERE CategoryID = l.CourseCategoryID))
                    )
            )
    BEGIN
        RAISERROR('One or more Students are not enrolled in the same CourseCategoryID as in the Lessons table.', 16, 1)
        ROLLBACK
        RETURN
    END
END;