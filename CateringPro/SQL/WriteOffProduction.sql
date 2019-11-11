USE [CateringPro]
GO

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
ALTER PROCEDURE [dbo].[WriteOffProduction]
	-- Add the parameters for the stored procedure here
	@DayDate Date,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	delete from DocLines
	From Doclines dl,Docs doc
	where doc.ID=dl.DocsId 
		 AND dl.CompanyId= @CompanyId
		  AND doc.CompanyId= @CompanyId
		  AND doc.ProductionDay = @DayDate

delete from Docs
where CompanyId= @CompanyId
		  AND ProductionDay = @DayDate
Declare @DocId int
	insert into Docs(CompanyId,[Date],[Type],ProductionDay)
	Values(@CompanyId,@DayDate,3,@DayDate)

	SELECT @DocId=SCOPE_IDENTITY()  

	SET NOCOUNT ON;
WITH Prod (IngId,Quanity)  
AS  

(  
 Select 
 IngId=di.IngredientId,
 Quanity=Sum(di.Proportion*ud.Quantity)

 from UserDayDish ud,Dishes d,DishIngredients di
 where ud.CompanyId=@CompanyId 
		AND d.CompanyId=@CompanyId 
		AND di.CompanyId=@CompanyId 
		AND ud.Date=@DayDate
		AND d.Id=ud.DishId
		AND di.DishId=d.Id
Group by di.IngredientId
)
insert into DocLines(CompanyId,DocsId,IngredientsId,Quantity)
select @CompanyId,@DocId,Prod.IngId,Prod.Quanity
from Prod
	
END
GO


