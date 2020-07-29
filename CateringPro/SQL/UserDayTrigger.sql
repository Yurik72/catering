CREATE OR ALTER TRIGGER  [dbo].[UserDay_Trigger]
   ON  [dbo].[UserDay]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
SET NOCOUNT ON;

Insert into UserFinances(Id,CompanyId)
	Select i.UserId,i.CompanyId
	from inserted i
	Left Outer Join UserFinances uf  on uf.Id=i.UserId and uf.CompanyId=i.CompanyId
	Where uf.Id is Null
	Group By i.UserId,i.CompanyId
/*
Update UserFinances
SET 

	TotalPreOrders=ISNULL(ins.cnt,0)-ISNULL(del.cnt,0),
	TotalPreOrderedAmount=ISNULL(ins.Total,0)-ISNULL(del.Total,0)
FROM UserFinances uf,
(Select Count(*) as cnt, Sum(Total) as Total,UserId from deleted where isPaid=0 group by UserId) del,
(Select Count(*) as cnt, Sum(Total) as Total,UserId from inserted where isPaid=0 group by UserId) ins
Where uf.Id=del.UserId AND uf.Id=ins.UserId
*/
Update UserFinances
SET 

	TotalPreOrders=ISNULL(d.cnt,0),
	TotalPreOrderedAmount=ISNULL(d.Total,0),
	LastUpdated=getdate()
FROM UserFinances uf,
(
	select distinct UserId,CompanyId from inserted
	Union 
	select UserId,CompanyId from deleted
) upd
LEFT JOIN (
	Select Count(*) as cnt, Sum(Total) as Total,UserId,CompanyId 
	from UserDay 
	where IsConfirmed=1 and IsPaid=0
	group by UserId,CompanyId
) d on d.UserId=upd.UserId and  d.CompanyId=upd.CompanyId

where   uf.Id=upd.UserId and uf.CompanyId=upd.CompanyId


END