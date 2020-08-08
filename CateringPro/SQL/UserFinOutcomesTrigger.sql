CREATE OR ALTER TRIGGER  [dbo].[UserFinOutcomes_Trigger]
   ON  [dbo].[UserFinOutComes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
SET NOCOUNT ON;


Update UserFinances
SET 
    TotalOutCome=ISNULL(d.Amount,0),
	Balance=TotalIncome-ISNULL(d.Amount,0),
	TotalPreOrderBalance = (Balance-ISNULL(d.Amount,0)) - TotalPreOrderedAmount
FROM UserFinances uf,
(
	select distinct Id,CompanyId from inserted
	Union 
	select Id,CompanyId from deleted
) upd
LEFT JOIN (
	Select  Sum(Amount) as Amount,Id ,CompanyId
	from [UserFinOutComes] 
	 
	group by Id,CompanyId
) d on d.Id=upd.Id and d.CompanyId=upd.CompanyId

where   uf.Id=upd.Id and uf.CompanyId=upd.CompanyId


END