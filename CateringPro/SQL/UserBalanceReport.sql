
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
	@DayFake Date =null,
	@CompanyId int,
	@UserSubGroupId int =NULL
AS
BEGIN

Declare @UsedSubGroupsId table
(
	ID int  null
);


WITH RecursiveQuery (ID, ParentID,Level,TopParentId) 
AS
( 
	 SELECT ID, ParentID,0 as Level,ParentID as TopParentId
	 FROM UserSubGroups usb where   
	 --( usb.parentid is not null or @UserSubGroupId IS NULL ) and 
	 usb.CompanyId=@CompanyId
	 UNION ALL 
	 SELECT usb.ID, usb.ParentID,rec.Level+1 as Level,rec.ParentID as TopParentId
	 FROM UserSubGroups usb 
    JOIN RecursiveQuery rec ON usb.ParentID = rec.ID and usb.CompanyId=@CompanyId and usb.CompanyId=@CompanyId
    )
Insert into @UsedSubGroupsId(ID)
	SELECT DISTINCT ID
	FROM RecursiveQuery  
where TopParentId=@UserSubGroupId or id=@UserSubGroupId or @UserSubGroupId IS NULL



Select (CASE u.UserType WHEN 0 THEN 'Child' WHEN 1 THEN 'Adult' ELSE 'Employee' END) as UserType,
	ISNULL(g.Name,'') as SubGroupName, u.Email,u.NameSurname,u.ChildNameSurname,
ISNULL(uf.Balance,0) as Balance,
ISNULL(uf.TotalPreOrderBalance,0) as TotalPreOrderBalance,
ISNULL(uf.TotalIncome,0) as TotalIncome,
ISNULL(uf.TotalOutCome,0) as TotalOutCome
from AspNetUsers u
inner join CompanyUserCompanies cu on cu.CompanyId=@CompanyId and cu.CompanyUserId=u.Id
left join UserFinances uf on uf.CompanyId=@CompanyId and uf.Id=u.Id
left join @UsedSubGroupsId usb on  u.UserSubGroupId=usb.id 
left join usersubgroups g on g.CompanyId=@companyId and  usb.id=g.id 

order by u.UserType asc,g.name asc,u.NameSurname,u.ChildNameSurname
END
GO


