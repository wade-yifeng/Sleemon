CREATE PROCEDURE [dbo].[spDeleteQuestionnaireById]
	@questionnaireId		INT
AS
BEGIN TRY
	BEGIN TRANSACTION
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

		UPDATE [dbo].[Questionnaire]
		SET [Questionnaire].[IsActive] = 0, [Questionnaire].[LastUpdateTime] = GETUTCDATE()
		WHERE [Questionnaire].[Id] = @questionnaireId

		UPDATE [dbo].[QuestionnaireItem]
		SET [QuestionnaireItem].[IsActive] = 0, [QuestionnaireItem].[LastUpdateTime] = GETUTCDATE()
		WHERE [QuestionnaireItem].[QuestionnaireId] = @questionnaireId

		UPDATE [QuestionnaireChoice]
		SET [QuestionnaireChoice].[IsActive] = 0, [QuestionnaireChoice].[LastUpdateTime] = GETUTCDATE()
		FROM [dbo].[QuestionnaireChoice]
		JOIN [dbo].[QuestionnaireItem]
			ON [QuestionnaireChoice].[QuestionnaireItemId] = [QuestionnaireItem].[Id]
		WHERE [QuestionnaireItem].[QuestionnaireId] = @questionnaireId

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