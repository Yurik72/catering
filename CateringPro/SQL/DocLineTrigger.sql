USE [CateringPro]
GO
/****** Object:  Trigger [dbo].[DocLines_Trigger]    Script Date: 28.10.2019 18:41:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER TRIGGER [dbo].[DocLines_Trigger]
   ON  [dbo].[DocLines]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Update Ingredients
	Set StockValue=i.StockValue + CASE doc.Type  
				WHEN 1 THEN -d.Quantity  
				WHEN 2 THEN +d.Quantity  
					  ELSE 0 
				END  
	From Ingredients i, deleted d, Docs doc 
	where i.Id=d.IngredientsId 
		AND doc.Id=d.Docsid 

Delete  from Consignment 
FROM Consignment c
inner join deleted d on d.CompanyId=c.CompanyId  and c.LineId=d.Id and c.IngredientsId=d.IngredientsId
inner join Docs doc on doc.Id=d.Docsid 
where doc.Type=1


Delete  from ConsignmentMove 
from ConsignmentMove cm
inner join deleted d on d.CompanyId=cm.CompanyId  and cm.LineId=d.Id and cm.IngredientsId=d.IngredientsId
inner join Docs doc on doc.Id=d.Docsid 
where doc.Type=2

  Update Ingredients
	Set StockValue=i.StockValue + CASE doc.Type  
				WHEN 1 THEN ins.Quantity  
				WHEN 2 THEN -ins.Quantity  
					  ELSE 0 
				END  
	From Ingredients i, inserted ins, Docs doc 
	where i.Id=ins.IngredientsId 
		AND doc.Id=ins.Docsid 

Insert into  Consignment(CompanyId,LineId,IngredientsId,InitialQuantity,Quantity,Price)
Select i.CompanyId,i.Id,i.IngredientsId,i.Quantity,i.Quantity,i.Price
From  inserted i, Docs doc 
where  doc.Id=i.Docsid 
      AND doc.Type=1
SET NOCOUNT ON;
--- Now Will Insert writeoff movemenents
--consignment  support
WITH Consignment_YTD (CompanyId,DocId, LineId,LineOutId,IngId,Quantity, Rest,RestPrev)  
AS  
(  
select CompanyId=ins.CompanyId, DocId=d.id,LineId=c.LineId,LineOutId=ins.Id, IngId=C.ingredientsid,
--sum(dl.quantity) OVER(PARTITION BY dl.ingredientsid),
Quantity=c.quantity,
Rest=sum(c.quantity) OVER(PARTITION BY c.ingredientsid
                ORDER BY d.id --first input firts output
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW),
RestPrev=ISNULL(sum(c.quantity) OVER(PARTITION BY c.ingredientsid
                ORDER BY d.id --first input firts output
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND  1 PRECEDING),0)

from docs d, Consignment c,inserted ins
where c.CompanyId=ins.CompanyId and c.IngredientsId=ins.IngredientsId and  ins.DocsId=d.Id  and
	(d.Type=2 or d.Type=3)
--Order by d.id  -- first input firts output
)
--select * from Consignment_YTD

Insert into  ConsignmentMove(CompanyId,LineId,LineOutId,IngredientsId,[Type],Quantity)
Select ytd.CompanyId,ytd.LineId,ytd.LineOutId,ytd.IngId,2,ytd.WriteOffVal
From  (select Consignment_YTD.*,

			RestWithFinal=
				CASE WHEN  Consignment_YTD.Rest>Consignment_YTD.RestPrev 
				THEN Consignment_YTD.Rest
				ELSE ins.Quantity+Consignment_YTD.RestPrev
				END,
			WriteOffVal=
				CASE WHEN  ins.Quantity>Consignment_YTD.Rest
				THEN     Consignment_YTD.Quantity
				ELSE    CASE WHEN Consignment_YTD.RestPrev<ins.Quantity
						THEN ins.Quantity-Consignment_YTD.RestPrev
						ELSE 0
						END
				END
from Consignment_YTD,inserted ins
where Consignment_YTD.IngId=ins.IngredientsId
--order by  Consignment_YTD.DocId 
) ytd
where ytd.WriteOffVal>0


SET NOCOUNT ON;
--finally will calculate actual Stock values per consignment 
WITH WriteOff (CompanyId,IngId,LineId,Quantity)  
AS  
(
Select
CompanyId=cm.CompanyId,
IngId=cm.IngredientsId,
LineId=cm.LineId,
Quantity=Sum(cm.Quantity)
 
from ConsignmentMove cm,inserted ins
where cm.CompanyId=ins.CompanyId AND cm.IngredientsId= ins.IngredientsId
      --AND cm.Type=2   --currently all movements is writeof
group by cm.CompanyId,cm.LineId ,cm.IngredientsId
)
Update Consignment
Set Quantity=c.InitialQuantity-w.Quantity
From Consignment c,WriteOff w
Where c.CompanyId=w.CompanyId
		and c.LineId=w.LineId
		AND c.IngredientsId=w.IngId

/*
    -- Insert statements for trigger here
--consignment  support
WITH Quantity_CTE (DocId, IngId,Quantity, Rest,RestPrev)  
AS  
(  
select DocId=d.id, IngId=dl.ingredientsid,
--sum(dl.quantity) OVER(PARTITION BY dl.ingredientsid),
Quantity=dl.quantity,
Rest=sum(dl.quantity) OVER(PARTITION BY dl.ingredientsid
                ORDER BY d.id
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND CURRENT ROW),
RestPrev=ISNULL(sum(dl.quantity) OVER(PARTITION BY dl.ingredientsid
                ORDER BY d.id
                ROWS BETWEEN UNBOUNDED PRECEDING
                         AND  1 PRECEDING),0)

from docs d, doclines dl,inserted ins
where dl.docsid=d.id and dl.IngredientsId=ins.IngredientsId and d.Type=1
)



select Quantity_CTE.*,

RestWithFinal=
	CASE WHEN  Quantity_CTE.Rest>Quantity_CTE.RestPrev 
	THEN Quantity_CTE.Rest
	ELSE ins.Quantity+Quantity_CTE.RestPrev
	END,
RestWithFinal1=
	CASE WHEN  ins.Quantity>Quantity_CTE.Rest
	THEN     Quantity_CTE.Quantity
	ELSE    CASE WHEN Quantity_CTE.RestPrev<ins.Quantity
			THEN ins.Quantity-Quantity_CTE.RestPrev
			ELSE 0
			END
	END
from Quantity_CTE,inserted ins
where Quantity_CTE.IngId=ins.IngredientsId
order by  Quantity_CTE.DocId

*/
END
