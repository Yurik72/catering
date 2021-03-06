--USE [CateringPro]
GO
/****** Object:  Trigger [dbo].[UserFinIncomes_Trigger]    Script Date: 8/11/2020 11:09:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER     TRIGGER  [dbo].[UserFinIncomes_Trigger]
   ON  [dbo].[UserFinIncomes]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
SET NOCOUNT ON;


Update UserFinances
SET 
    TotalIncome=ISNULL(d.Amount,0),
	Balance=ISNULL(d.Amount,0) -TotalOutCome,
	--TotalPreOrderBalance = (Balance-ISNULL(d.,0))
	TotalPreOrderBalance = ((ISNULL(d.Amount,0) -TotalOutCome)) - TotalPreOrderedAmount
FROM UserFinances uf,
(
	select distinct Id,CompanyId from inserted
	Union 
	select Id,CompanyId from deleted
) upd
LEFT JOIN (
	Select  Sum(Amount) as Amount,Id ,CompanyId
	from [UserFinIncomes] 
	 
	group by Id,CompanyId
) d on d.Id=upd.Id and d.CompanyId=upd.CompanyId

where   uf.Id=upd.Id and uf.CompanyId=upd.CompanyId


END