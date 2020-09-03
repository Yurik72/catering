
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
CREATE OR ALTER PROCEDURE [dbo].[OrderDayReport]
	-- Add the parameters for the stored procedure here
	@DayDate Date,
	@CompanyId int
AS
BEGIN
DECLARE @UsedCatVar table
(  
    CatId int NOT NULL
)
insert into @UsedCatVar(CatId)
select cat.id from DayComplex dc, Complex c,Categories cat

where dc.CompanyId=@CompanyId and dc.ComplexId=c.id
      and c.CompanyId=dc.CompanyId
	  and cat.CompanyId=dc.CompanyId and cat.Id=c.CategoriesId
group by cat.id

select ua.Email,IsNull(ua.ChildNameSurname,'') as ChildNameSurname,c.name as CategoryName,ISNULL(t.TotalOrdered,0)  as TotalOrdered from aspnetusers ua
cross join @UsedCatVar usedcat 
inner join Categories c on c.Id=usedcat.CatId and c.CompanyId=@CompanyId
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
 where ua.CompanyId=@companyId  and ua.ConfirmedByAdmin=1
 order by c.id,ua.Email
END
GO


