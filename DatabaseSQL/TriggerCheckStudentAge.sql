CREATE TRIGGER TRG_CheckStudentAge
ON Courses
AFTER INSERT
AS
BEGIN
    -- Check the age of the student for new rows inserted/updated in the Courses table
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN Students s ON i.StudentID = s.StudentID
        INNER JOIN CourseDetails cd ON i.CourseDetailID = cd.CourseDetailID
        WHERE DATEDIFF(YEAR, s.BirthDate, GETDATE()) < cd.MinimumYears
            OR (DATEDIFF(YEAR, s.BirthDate, GETDATE()) = cd.MinimumYears
                AND DATEDIFF(MONTH, s.BirthDate, GETDATE()) < cd.MinimumMonths)
    )
    BEGIN
        RAISERROR('Kursant nie spełnia minimalnego wymaganego wieku dla wybranego kursu.', 16, 1)
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;