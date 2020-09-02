Declare @dayDate DateTime
Declare @companyId int
Declare @UserId nvarchar(100)
SET @dayDate='20200902'
SET @UserId='03127de6-c34d-4651-b311-10028c18fc3f'
SET @companyId=8
--select Email,ChildNameSurname from
--(
Select u.Email,u.ChildNameSurname,dk.Name as KindName,c.Name As ComplexName,d.Name as DishName,ud.isDelivered,u.id,
COUNT(*) OVER(PARTITION BY  u.id ) AS TotalOrdered, 
Sum(case ud.isDelivered when 1 then 1 else 0 end) OVER(PARTITION BY  u.id) AS Delivered ,
cat.Name,cat.id
from UserDayDish ud
inner join aspnetusers u on u.Id=ud.userid
inner join complex c on c.CompanyId=ud.CompanyId and ud.complexid=c.id 
inner join DishComplex dc on dc.CompanyId=ud.CompanyId and dc.DishId=ud.DishId and dc.ComplexId=c.id
inner join Dishes d on d.CompanyId=ud.CompanyId and d.Id=dc.DishId 
inner join categories cat on cat.id=c.Categoriesid
left join DishesKind dk on dk.CompanyId=c.CompanyId and dk.Id=c.DishKindId 
where ud.Date=@dayDate and ud.CompanyId=@companyId and cat.id=20
order by u.id,ud.isDelivered 
--) t

--where  Delivered=0
--group by Email,ChildNameSurname
--order by id,isDelivered

--
--        and ud.userid=@UserId
		
		--and c.CompanyId=ud.CompanyId
	-- and dc.ComplexId=c.id and uc.complexid=c.id 
	--and dc.CompanyId=ud.CompanyId and dc.DishId=ud.DishId 