
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
	@CompanyId int
AS
BEGIN
DECLARE @UsedCatVar table
(  
    CatId int NOT NULL
)
Select ud.Date,g.name as groupname, cat.name,u.ChildNameSurname,cat.Name as Category,d.name as dishname,ud.IsDelivered

--COUNT(*) AS TotalOrdered 


from UserDayDish ud
 join aspnetusers u on u.Id=ud.userid
inner join complex c on c.CompanyId=ud.CompanyId and ud.complexid=c.id 
inner join DishComplex dc on dc.CompanyId=ud.CompanyId and dc.DishId=ud.DishId and dc.ComplexId=c.id
inner join Dishes d on d.CompanyId=ud.CompanyId and d.Id=dc.DishId 
inner join categories cat on cat.id=c.Categoriesid
left join DishesKind dk on dk.CompanyId=c.CompanyId and dk.Id=c.DishKindId 
inner join usersubgroups g on g.CompanyId=8 and u.UserSubGroupId=g.id
where ud.Date>=@DayFrom and ud.Date<=@DayTo   and ud.CompanyId=@companyId --and cat.id=20

order by ud.Date,g.name,cat.code,u.ChildNameSurname


END
GO


