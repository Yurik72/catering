
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
CREATE OR ALTER PROCEDURE [dbo].[DeliveryReportSummary]
	-- Add the parameters for the stored procedure here
	@DayDate Date,
	@CompanyId int
AS
BEGIN

Select u.Email,u.ChildNameSurname,
Sum(case ud.isDelivered when 1 then 1 else 0 end)  AS Delivered ,
COUNT(*) AS TotalOrdered 


from UserDayDish ud
inner join aspnetusers u on u.Id=ud.userid
inner join complex c on c.CompanyId=ud.CompanyId and ud.complexid=c.id 
inner join DishComplex dc on dc.CompanyId=ud.CompanyId and dc.DishId=ud.DishId and dc.ComplexId=c.id
inner join Dishes d on d.CompanyId=ud.CompanyId and d.Id=dc.DishId 
inner join categories cat on cat.id=c.Categoriesid
left join DishesKind dk on dk.CompanyId=c.CompanyId and dk.Id=c.DishKindId 
where ud.Date=@dayDate and ud.CompanyId=@companyId --and cat.id=20
group by u.Email,u.ChildNameSurname
 
END
GO


