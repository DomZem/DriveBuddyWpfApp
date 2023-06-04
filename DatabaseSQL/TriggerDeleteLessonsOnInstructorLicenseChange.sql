CREATE TRIGGER TRG_DeleteLessonsOnInstructorLicenseChange
ON InstructorsCategories
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the InstructorID and CategoryID affected by the update or delete operation
    DECLARE @DeletedInstructorID INT, @DeletedCategoryID INT;
    
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        SELECT @DeletedInstructorID = InstructorID, @DeletedCategoryID = CategoryID FROM deleted;
    END;
    
    -- Check if the Instructor lost the license for the Category
    IF EXISTS (
            SELECT *
            FROM Lessons AS l
            WHERE (l.InstructorID = @DeletedInstructorID)
                AND (l.CourseCategoryID = @DeletedCategoryID)
                AND NOT EXISTS (
                    SELECT *
                    FROM InstructorsCategories AS ic
                    WHERE ic.InstructorID = l.InstructorID
                        AND ic.CategoryID = l.CourseCategoryID
                )
            )
    BEGIN
        -- Delete the Lessons associated with the Instructor and Category
        DELETE FROM Lessons
        WHERE (InstructorID = @DeletedInstructorID)
            AND (CourseCategoryID = @DeletedCategoryID);
    END
END;