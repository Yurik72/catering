
/****** Object:  StoredProcedure [dbo].[WriteOffProduction]    Script Date: 01.11.2019 12:45:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[CleanUp]
	-- Add the parameters for the stored procedure here
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @today date
declare @beforetoday date
SET @today= getdate()
SET @beforetoday=DATEADD(day, -1, @today)

Select @today,@beforetoday

END
GO


