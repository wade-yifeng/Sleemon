CREATE PROCEDURE [dbo].[spDeleteExamById]
	@examId		INT
AS
BEGIN TRY
	BEGIN TRANSACTION
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

		UPDATE [dbo].[Exam]
		SET [Exam].[IsActive] = 0, [Exam].[LastUpdateTime] = GETUTCDATE()
		WHERE [Exam].[Id] = @examId

		UPDATE [dbo].[ExamQuestion]
		SET [ExamQuestion].[IsActive] = 0, [ExamQuestion].[LastUpdateTime] = GETUTCDATE()
		WHERE [ExamQuestion].[ExamId] = @examId

		UPDATE [ExamChoice]
		SET [ExamChoice].[IsActive] = 0, [ExamChoice].[LastUpdateTime] = GETUTCDATE()
		FROM [dbo].[ExamChoice]
		JOIN [dbo].[ExamQuestion]
			ON [ExamChoice].[ExamQuestionId] = [ExamQuestion].[Id]
		WHERE [ExamQuestion].[ExamId] = @examId
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