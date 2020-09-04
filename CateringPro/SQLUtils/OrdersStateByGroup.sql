Declare @dayDate DateTime
Declare @companyId int
Declare @UserId nvarchar(100)
SET @dayDate='20200904'

SET @companyId=8

Select g.name as groupname, cat.name,u.ChildNameSurname,d.name as dishname

--COUNT(*) AS TotalOrdered 


from UserDayDish ud
 join aspnetusers u on u.Id=ud.userid
inner join complex c on c.CompanyId=ud.CompanyId and ud.complexid=c.id 
inner join DishComplex dc on dc.CompanyId=ud.CompanyId and dc.DishId=ud.DishId and dc.ComplexId=c.id
inner join Dishes d on d.CompanyId=ud.CompanyId and d.Id=dc.DishId 
inner join categories cat on cat.id=c.Categoriesid
left join DishesKind dk on dk.CompanyId=c.CompanyId and dk.Id=c.DishKindId 
inner join usersubgroups g on g.CompanyId=8 and u.UserSubGroupId=g.id
where ud.Date=@dayDate and ud.CompanyId=@companyId --and cat.id=20
and (g.name='0.1' or g.name='0.2')
order by g.name,cat.code,u.ChildNameSurname
--group by u.id,u.Email,u.ChildNameSurname,cat.name,cat.id
 