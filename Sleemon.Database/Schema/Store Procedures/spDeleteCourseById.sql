CREATE PROCEDURE [dbo].[spDeleteCourseById]
	@courseId			INT
AS
BEGIN TRY
	BEGIN TRANSACTION
		UPDATE [LearningCourse]
		SET [LearningCourse].[IsActive] = 0, [LearningCourse].[LastUpdateTime] = GETUTCDATE()
		FROM [dbo].[LearningCourse]
		WHERE [LearningCourse].[Id] = @courseId

		UPDATE [LearningChapter]
		SET [LearningChapter].[IsActive] = 0, [LearningChapter].[LastUpdateTime] = GETUTCDATE()
		FROM [dbo].[LearningChapter]
		WHERE [LearningChapter].[CourseId] = @courseId

		UPDATE [LearningFile]
		SET [LearningFile].[IsActive] = 0, [LearningFile].[LastUpdateTime] = GETUTCDATE()
		FROM [dbo].[LearningFile]
		JOIN [dbo].[LearningChapter]
			ON [LearningFile].[ChapterId] = [LearningChapter].[Id]
		WHERE [LearningChapter].[CourseId] = @courseId
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
	ROLLBACK TRANSACTION

	DECLARE @ErrMsg NVARCHAR(4000), @ErrSeverity INT
		SELECT @ErrMsg = ERROR_MESSAGE(),
				@ErrSeverity = ERROR_SEVERITY()

	RAISERROR(@ErrMsg, @ErrSeverity, 1)
END CATCH
