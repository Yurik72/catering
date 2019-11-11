Declare @dayDate DateTime
Select @dayDate='2019-11-10';
Declare @CompanyId int

WITH Production (Date,IngId,ProductionQuantity,CompanyId)  
AS (
select  
Date=ud.date,
IngId=di.IngredientId,
ProductionQuantity=Sum(ud.Quantity*di.Proportion)         
from UserDayDish ud, Dishes d,DishIngredients di,Ingredients i


where 
		ud.DishId=d.id
		and di.DishId=d.Id
		and i.Id=di.IngredientId
	    and ud.Date>@dayDate
group by ud.date,di.IngredientId
)

Select p.*,i.StockValue, 
BeginDay=i.StockValue-sum(p.ProductionQuantity) OVER(PARTITION BY p.IngId
                ORDER BY p.Date
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW)+p.ProductionQuantity,

DayProduction=sum(p.ProductionQuantity) OVER(PARTITION BY p.IngId
                ORDER BY p.Date
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW),
AfterDayStockValue=i.StockValue-sum(p.ProductionQuantity) OVER(PARTITION BY p.IngId
                ORDER BY p.Date
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW)

from Production p, Ingredients i
where p.IngId=i.Id
order by p.Date,p.IngId