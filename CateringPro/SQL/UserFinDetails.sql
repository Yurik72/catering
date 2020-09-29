
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
CREATE OR ALTER PROCEDURE [dbo].[UserFinDetails]
	-- Add the parameters for the stored procedure here
	@DayFrom Date,
	@DayTo Date,
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

--Insert into @UsedSubGroupsId(ID) values(NULL)  -- to handle not assigned

select * from
(
select (CASE u.UserType WHEN 0 THEN 'Child' WHEN 1 THEN 'Adult' ELSE 'Employee' END) as UserType,
	ISNULL(g.Name,'') as SubGroupName, 
	cast(ufi.TransactionDate as date) as Date,
	(case  when u.UserType = 0 then Isnull(u.ChildNameSurname,'') else u.NameSurname end)as ChildNameSurname,
	1 as IsIncome,
	ISNULL(ufi.Amount,0) as Income,
	0.0 as Outcome,
	ufi.Comments as Comments,
	ufi.TransactionData as TransactionData
 from aspnetusers u  
inner join UserFinances uf on uf.CompanyId=@companyId and uf.ID=u.id 
left join UserFinIncomes ufi on ufi.CompanyId=@companyId and ufi.Id=u.Id 
		and ufi.IsProjection=0
		and ufi.TransactionDate>=@DayFrom and ufi.TransactionDate<=@DayTo

left join @UsedSubGroupsId usb on  u.UserSubGroupId=usb.id 
left join usersubgroups g on g.CompanyId=@companyId and  usb.id=g.id 
union ALL
select (CASE u.UserType WHEN 0 THEN 'Child' WHEN 1 THEN 'Adult' ELSE 'Employee' END) as UserType,
	ISNULL(g.Name,'') as SubGroupName, 
	cast(ufo.DayDate as date) as Date,
	(case  when u.UserType = 0 then Isnull(u.ChildNameSurname,'') else u.NameSurname end)as ChildNameSurname,
	0 as IsIncome,
	0.0 as Income,
	ISNULL(-ufo.Amount,0) as Outcome,
	ufo.Comments as Comments,
	'' as TransactionData
 from aspnetusers u  
inner join UserFinances uf on uf.CompanyId=@companyId and uf.ID=u.id 
left join UserFinOutComes ufo on ufo.CompanyId=@companyId and ufo.Id=u.Id 
	
		and ufo.DayDate>=@DayFrom and ufo.DayDate<=@DayTo

left join @UsedSubGroupsId usb on  u.UserSubGroupId=usb.id 
left join usersubgroups g on g.CompanyId=@companyId and  usb.id=g.id 
--where ufi.TransactionDate>=@DayFrom and ufi.TransactionDate<=@DayTo   


) t
order by t.Date ASC ,t.ChildNameSurname ASC ,t.Income DESC

END
GO


