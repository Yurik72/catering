
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
CREATE OR ALTER PROCEDURE [dbo].[OrderFinDetails]
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
select * from(
Select 'Complex' as OrderType, ud.Date,
(CASE u.UserType WHEN 0 THEN 'Child' WHEN 1 THEN 'Adult' ELSE 'Employee' END) as UserType,
ISNULL(g.Name,'') as SubGroupName, (case  when u.UserType = 0 then Isnull(u.ChildNameSurname,'') else u.NameSurname end)as ChildNameSurname,

cat.Name as Category,ISNULL(dk.Name,'') as DishKind, 
--ud.IsDelivered, 
(udc.Price*udc.Quantity) -((udc.Price*udc.Quantity)*(ISNULL(uday.Discount,0)/uday.Total)) as Total, 
((udc.Price*udc.Quantity)*(ISNULL(uday.Discount,0)/uday.Total)) as Discount, 
udc.Price*udc.Quantity as TotalWithoutDiscount,
uday.Total as  TotalDay,
ISNULL(uday.Discount,0) as  TotalDayDiscount,
ud.OrderedDishes,DeliveredDishes

--COUNT(*) AS TotalOrdered 


from 
(select CompanyId,UserId,Date,ComplexId,
count(*) as OrderedDishes,Sum(CASE WHEN t1.IsDelivered=1 THEN 1 ELSE 0 END) as DeliveredDishes
from UserDayDish t1 
	where t1.Date>=@DayFrom and t1.Date<=@DayTo and t1.CompanyId=@companyId and t1.IsComplex=1 
	group by CompanyId,UserId,Date,ComplexId )  ud
 join aspnetusers u on u.Id=ud.userid
inner join complex c on c.CompanyId=ud.CompanyId and ud.complexid=c.id 
--inner join DishComplex dc on dc.CompanyId=ud.CompanyId and dc.DishId=ud.DishId and dc.ComplexId=c.id
--inner join Dishes d on d.CompanyId=ud.CompanyId and d.Id=dc.DishId 
inner join categories cat on cat.id=c.Categoriesid
inner join UserDay uday on uday.CompanyId=ud.CompanyId and uday.UserId=u.Id and uday.Date=ud.Date
inner join UserDayComplex  udc on   udc.CompanyId=ud.CompanyId and udc.UserId=u.Id  and udc.Date=ud.Date and udc.ComplexId=ud.ComplexId
left join DishesKind dk on dk.CompanyId=c.CompanyId and dk.Id=c.DishKindId 
inner join usersubgroups g on g.CompanyId=@companyId and ( u.UserSubGroupId=g.id )
inner join @UsedSubGroupsId usb on  g.id=usb.id
where ud.Date>=@DayFrom and ud.Date<=@DayTo   and ud.CompanyId=@companyId 
/*   to do with dishes
union   
select uday.Date, (case  when u.UserType = 0 then Isnull(u.ChildNameSurname,'') else u.NameSurname end)as ChildNameSurname,
null as Category,null as DishKind, 
null, uday.Total as Total, uday.Discount as Discount, uday.TotalWtithoutDiscount as TotalWithoutDiscount
from UserDay uday
 join aspnetusers u on u.Id=uday.userid
--inner join UserDay on uday.CompanyId=ud.CompanyId and uday.UserId=u.Id 
inner join usersubgroups g on g.CompanyId=@companyId and u.UserSubGroupId=g.id
inner join @UsedSubGroupsId usb on  g.id=usb.id
where uday.Date>=@DayFrom and uday.Date<=@DayTo   and uday.CompanyId=@companyId 
*/
) t
order by t.Date,t.ChildNameSurname



END
GO


