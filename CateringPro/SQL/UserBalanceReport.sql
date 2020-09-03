
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
CREATE OR ALTER PROCEDURE [dbo].[UserBalanceReport]
	-- Add the parameters for the stored procedure here
	@DayDate Date,
	@CompanyId int
AS
BEGIN

Select u.Email,u.NameSurname,u.ChildNameSurname,
ISNULL(uf.Balance,0) as Balance,
ISNULL(uf.TotalPreOrderBalance,0) as TotalPreOrderBalance,
ISNULL(uf.TotalIncome,0) as TotalIncome,
ISNULL(uf.TotalOutCome,0) as TotalOutCome
from AspNetUsers u
inner join CompanyUserCompanies cu on cu.CompanyId=@CompanyId and cu.CompanyUserId=u.Id
left join UserFinances uf on uf.CompanyId=@CompanyId and uf.Id=u.Id
order by uf.Balance asc
END
GO


