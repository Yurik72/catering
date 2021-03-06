--USE [CateringPro]
GO
/****** Object:  Trigger [dbo].[UserFinOutcomes_Trigger]    Script Date: 8/11/2020 11:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   TRIGGER  [dbo].[UserFinOutcomes_Trigger]
   ON  [dbo].[UserFinOutComes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
SET NOCOUNT ON;


Update UserFinances
SET 
    TotalOutCome=ISNULL(d.Amount,0),
	Balance=TotalIncome-ISNULL(d.Amount,0),
	TotalPreOrderBalance = ((TotalIncome-ISNULL(d.Amount,0))) - TotalPreOrderedAmount
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