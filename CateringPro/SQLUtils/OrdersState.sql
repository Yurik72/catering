Declare @dayDate DateTime
Declare @companyId int
Declare @UserId nvarchar(100)
SET @dayDate='20200903'

SET @companyId=8

select ua.Email,ua.ChildNameSurname,c.name as CategoryName,ISNULL(t.TotalOrdered,0)  as TotalOrdered from aspnetusers ua
cross join categories c 
LEFT JOIN (
Select u.id, u.Email,u.ChildNameSurname,cat.name,cat.id as catid,

COUNT(*) AS TotalOrdered 


from UserDayDish ud
 join aspnetusers u on u.Id=ud.userid
inner join complex c on c.CompanyId=ud.CompanyId and ud.complexid=c.id 
inner join DishComplex dc on dc.CompanyId=ud.CompanyId and dc.DishId=ud.DishId and dc.ComplexId=c.id
inner join Dishes d on d.CompanyId=ud.CompanyId and d.Id=dc.DishId 
inner join categories cat on cat.id=c.Categoriesid
left join DishesKind dk on dk.CompanyId=c.CompanyId and dk.Id=c.DishKindId 
where ud.Date=@dayDate and ud.CompanyId=@companyId --and cat.id=20
group by u.id,u.Email,u.ChildNameSurname,cat.name,cat.id
 ) t on t.id=ua.id and t.catid=c.id
 where c.CompanyId=@companyId   and c.Id>=18 and c.id<=21 
 and ua.CompanyId=@companyId  and ua.ConfirmedByAdmin=1
 order by c.id,ua.Email
