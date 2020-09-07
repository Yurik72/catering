
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
CREATE OR ALTER PROCEDURE [dbo].[OrderPeriodDetailReport]
	-- Add the parameters for the stored procedure here
	@DayFrom Date,
	@DayTo Date,
	@CompanyId int,
	@UserSubGroupId int =NULL
AS
BEGIN

Declare @UsedSubGroupsId table
(
	ID int not null
)

DECLARE @UsedCatVar table
(  
    CatId int NOT NULL
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
--select * from @UsedSubGroupsId
Select ud.Date,g.name as GroupName, Isnull(u.ChildNameSurname,'') as ChildNameSurname,cat.Name as Category,ISNULL(dk.Name,'') as DishKind, d.name as DishName,ud.IsDelivered

--COUNT(*) AS TotalOrdered 


from UserDayDish ud
 join aspnetusers u on u.Id=ud.userid
inner join complex c on c.CompanyId=ud.CompanyId and ud.complexid=c.id 
inner join DishComplex dc on dc.CompanyId=ud.CompanyId and dc.DishId=ud.DishId and dc.ComplexId=c.id
inner join Dishes d on d.CompanyId=ud.CompanyId and d.Id=dc.DishId 
inner join categories cat on cat.id=c.Categoriesid
left join DishesKind dk on dk.CompanyId=c.CompanyId and dk.Id=c.DishKindId 
inner join usersubgroups g on g.CompanyId=@companyId and u.UserSubGroupId=g.id
inner join @UsedSubGroupsId usb on  g.id=usb.id
where ud.Date>=@DayFrom and ud.Date<=@DayTo   and ud.CompanyId=@companyId 

order by ud.Date,g.name,cat.code,u.ChildNameSurname


END
GO


