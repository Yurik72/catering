--USE [CateringPro]
GO
/****** Object:  Trigger [dbo].[DocLines_Trigger]    Script Date: 9/15/2020 11:14:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
				WHEN 3 THEN +d.Quantity
				WHEN 4 THEN +d.Quantity  
					  ELSE 0 
				END  
	From Ingredients i, deleted d, Docs doc 
	where i.Id=d.IngredientsId 
		AND doc.Id=d.Docsid 

Delete  from Consignment 
FROM Consignment c
inner join deleted d on d.CompanyId=c.CompanyId  and c.LineId=d.Id and c.IngredientsId=d.IngredientsId
inner join Docs doc on doc.Id=d.Docsid 
where doc.Type=1 or doc.Type=4

--recalc/restore consignment values
update Consignment 
Set Quantity=cons.Quantity+grp.quantity
from Consignment cons,
(
select sum(cm.quantity) as quantity,cm.IngredientsId, c.lineid,cm.companyid from ConsignmentMove cm
inner join deleted d on d.CompanyId=cm.CompanyId  and cm.LineOutId=d.Id and cm.IngredientsId=d.IngredientsId
inner join Docs doc on doc.Id=d.Docsid 
inner join Consignment c on c.CompanyId=d.CompanyId and c.IngredientsId=cm.IngredientsId and c.LineId=cm.LineId
where doc.Type=2 OR doc.Type=3
group by cm.IngredientsId, c.lineid,cm.companyid
) grp
where cons.CompanyId=grp.CompanyId and cons.LineId=grp.LineId and cons.IngredientsId=grp.IngredientsId


Delete  from ConsignmentMove 
from ConsignmentMove cm
inner join deleted d on d.CompanyId=cm.CompanyId  and cm.LineOutId=d.Id and cm.IngredientsId=d.IngredientsId
inner join Docs doc on doc.Id=d.Docsid 
where doc.Type=2 OR doc.Type=3

  Update Ingredients
	Set StockValue=i.StockValue + CASE doc.Type  
				WHEN 1 THEN ins.Quantity  
				WHEN 2 THEN -ins.Quantity
				WHEN 3 THEN ins.Quantity 
				WHEN 4 THEN ins.Quantity 
					  ELSE 0 
				END  
	From Ingredients i, inserted ins, Docs doc 
	where i.Id=ins.IngredientsId 
		AND doc.Id=ins.Docsid 

Insert into  Consignment(CompanyId,LineId,IngredientsId,InitialQuantity,Quantity,Price,ValidUntil)
Select i.CompanyId,i.Id,i.IngredientsId,i.Quantity,i.Quantity,i.Price,ISNULL(i.ValidTo,getdate())
From  inserted i, Docs doc 
where  doc.Id=i.Docsid 
      AND ( doc.Type=1 or doc.Type=4)
SET NOCOUNT ON;
--- Now Will Insert writeoff movemenents

--Declare @newConsignment table (CompanyId int,IngredientsId int,LineId int,LinoutId int,Quantity decimal(18,3))
-- first we will create consignment if they are not present (not any income)
Insert into Consignment(CompanyId,IngredientsId,LineId,Quantity)
--OUTPUT INSERTED.CompanyId,INSERTED.LineId,INSERTED.IngredientsId,INSERTED.Quantity INTO @newConsignment(CompanyId,LineId,IngredientsId,Quantity)
Select ins.CompanyId,ins.IngredientsId,0,0 as quantity 
from inserted ins,Docs
where NOT EXISTS (Select * from Consignment cons 
					where cons.CompanyId=ins.CompanyId and cons.IngredientsId=ins.IngredientsId and cons.LineId=0)
and Docs.CompanyId=ins.CompanyId and Docs.Id=ins.DocsId and Docs.Type=3
group by ins.Id,ins.IngredientsId,ins.CompanyId

--consignment  support
DECLARE @OutputTbl TABLE (CompanyId INT,LineId int,LineOutId int,IngredientsId int ,Quantity decimal(18,3));

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
	and c.LineId>0
--Order by d.id  -- first input firts output
)
/*
select Consignment_YTD.*,  
final=Consignment_YTD.Rest- Consignment_YTD.RestPrev,
ins=ins.Quantity,
WriteOffVal=
				CASE WHEN  ins.Quantity>Consignment_YTD.Rest
				THEN     Consignment_YTD.Quantity
				ELSE    CASE WHEN Consignment_YTD.RestPrev<=ins.Quantity
						THEN ins.Quantity-Consignment_YTD.RestPrev
						ELSE 0
						END
				END
from
Consignment_YTD,inserted ins
where Consignment_YTD.IngId=ins.IngredientsId
*/


Insert into  ConsignmentMove(CompanyId,LineId,LineOutId,IngredientsId,[Type],Quantity)
 OUTPUT INSERTED.CompanyId,INSERTED.LineId,INSERTED.LineOutId,INSERTED.IngredientsId,INSERTED.Quantity INTO @OutputTbl(CompanyId,LineId,LineOutId,IngredientsId,Quantity)
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
where Consignment_YTD.IngId=ins.IngredientsId and ins.DocsId=Consignment_YTD.DocId and ins.Id=Consignment_YTD.LineOutId
--order by  Consignment_YTD.DocId 
) ytd
--where ytd.WriteOffVal>0  --!!!!
--- handle negative by write off into the last consignment
Declare @MoveCount int
Select @MoveCount=count(*) from  @OutputTbl

IF (@MoveCount>0)  -- we have something to write off
BEGIN
Update  ConsignmentMove
set Quantity=mov.Quantity-negative.diff
from ConsignmentMove mov,
(
select (o.insquantity-ins.quantity) as diff ,ins.IngredientsId,ins.quantity, o.insquantity,ins.CompanyId,ins.id,o.lineoutid ,o.lineinid
from inserted ins,
(select sum(quantity) as insquantity,IngredientsId,CompanyId ,min(lineOutId)lineoutid,max(LineId) lineinid
from @OutputTbl group by IngredientsId, CompanyId
) o
where o.IngredientsId= ins.IngredientsId and o.CompanyId=ins.CompanyId and ins.Id=o.lineoutid
) negative

where negative.CompanyId=mov.CompanyId and negative.lineoutid=mov.LineOutId  and negative.lineinid=mov.LineId
and negative.IngredientsId=mov.IngredientsId
and negative.diff<0
END








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

SET NOCOUNT ON;
WITH StockVal (CompanyId,IngId,Quantity,LastPrice,AvgPrice)  
AS  
(
Select
CompanyId=c.CompanyId,
IngId=c.IngredientsId,

Quantity=Sum(c.Quantity),
 LastPrice=max(ins.Price),
 AvgPrice=CASE  WHEN sum(c.InitialQuantity)>0 THEN  sum(c.Price*c.InitialQuantity)/sum(c.InitialQuantity) ELSE 0 END
from Consignment c,inserted ins
where c.CompanyId=ins.CompanyId 
		AND c.IngredientsId= ins.IngredientsId
      --AND cm.Type=2   --currently all movements is writeof
group by c.CompanyId,c.IngredientsId
)

Update Ingredients
Set 
	StockValue=sv.Quantity,
	LastPurchasePrice=sv.LastPrice,
	AvgPrice=sv.AvgPrice
From Ingredients  i,StockVal sv
where   i.CompanyId=sv.CompanyId 
		AND i.Id= sv.IngId

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
