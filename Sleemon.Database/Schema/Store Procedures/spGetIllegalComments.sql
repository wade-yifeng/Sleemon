CREATE PROCEDURE [dbo].[spGetIllegalComments]
AS

	DECLARE @Words VARCHAR(8000) 
	SELECT @Words = COALESCE(@Words + ' OR ', '') + '"' + IllegalWord + '"'
	FROM [dbo].[IllegalCharacters]
	WHERE IsActive = 1

	SET @Words = '''' + @Words + ''''

	SELECT  [UserComments].[Id],
			[Name],
			[Comment],
			[Category],
			[LegalStatus],
			[CommentTime]
	FROM [dbo].[UserComments]
	LEFT JOIN [dbo].[User]
        ON [User].[UserUniqueId] = [UserComments].[UserUniqueId] AND [User].[IsActive] = 1
	WHERE [UserComments].[IsActive] = 1 AND [LegalStatus] = 0
		AND FREETEXT(Comment, @Words)


GO


