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
ALTER PROCEDURE [dbo].[ForecastStockProduction]
	-- Add the parameters for the stored procedure here
	@DayDate Date,
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
	    and ud.Date>@DayDate
		and ud.CompanyId=@CompanyId
		and d.CompanyId=@CompanyId
		and di.CompanyId=@CompanyId
		and i.CompanyId=@CompanyId
group by ud.date,di.IngredientId
)

Select p.Date,p.IngredientId,
Name=i.Name,
i.StockValue, 
BeginDay=i.StockValue-sum(p.ProductionQuantity) OVER(PARTITION BY p.IngredientId
                ORDER BY p.Date
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW)+p.ProductionQuantity,
ProductionQuantity=p.ProductionQuantity,
DayProduction=sum(p.ProductionQuantity) OVER(PARTITION BY p.IngredientId
                ORDER BY p.Date
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW),
AfterDayStockValue=i.StockValue-sum(p.ProductionQuantity) OVER(PARTITION BY p.IngredientId
                ORDER BY p.Date
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW),
MeasureUnit=i.MeasureUnit
from Production p, Ingredients i
where p.IngredientId=i.Id and i.CompanyId=@CompanyId
order by p.Date,p.IngredientId
	
END
GO


