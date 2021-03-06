

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
ALTER PROCEDURE [dbo].[ForecastStockProduction]
	-- Add the parameters for the stored procedure here
	@DateFrom Date,
	@DateTo Date,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
SET NOCOUNT ON;
WITH Production (Date,IngredientId,ProductionQuantity,CompanyId)  
AS (
select  
Date=ud.date,  
IngredientId=di.IngredientId,
ProductionQuantity=Sum(ud.Quantity*di.Proportion),
CompanyId=@CompanyId
from UserDayDish ud, Dishes d,DishIngredients di,Ingredients i


where 
		ud.DishId=d.id
		and di.DishId=d.Id
		and i.Id=di.IngredientId
	    and ud.Date>=@DateFrom
		and ud.Date<=@DateTo
		and ud.CompanyId=@CompanyId
		and d.CompanyId=@CompanyId
		and di.CompanyId=@CompanyId
		and i.CompanyId=@CompanyId
group by ud.date,di.IngredientId
)

Select 
DayDate=p.Date, --0
p.IngredientId, --1
Name=i.Name, --2
i.StockValue, --3
BeginDay=i.StockValue-sum(p.ProductionQuantity) OVER(PARTITION BY p.IngredientId
                ORDER BY p.Date
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW)+p.ProductionQuantity, --4
ProductionQuantity=p.ProductionQuantity, --5
DayProduction=sum(p.ProductionQuantity) OVER(PARTITION BY p.IngredientId
                ORDER BY p.Date
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW), --6
AfterDayStockValue=i.StockValue-sum(p.ProductionQuantity) OVER(PARTITION BY p.IngredientId
                ORDER BY p.Date
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW), --7
MeasureUnit=i.MeasureUnit, --8,
IngredientCategoriesId=ic.id, --9
IngredientCategoriesName=ic.Name --10
from Production p, Ingredients i, IngredientCategories ic
where p.IngredientId=i.Id and i.CompanyId=@CompanyId and i.IngredientCategoriesId=ic.id
order by p.Date,p.IngredientId
	
END
GO


