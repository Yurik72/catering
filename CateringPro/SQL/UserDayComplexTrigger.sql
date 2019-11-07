USE [CateringPro]
GO

/****** Object:  Trigger [dbo].[UserDayComplex_Trigger]    Script Date: 07.11.2019 11:01:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[UserDayComplex_Trigger]
   ON  [dbo].[UserDayComplex]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Delete  from UserDayDish 
		FROM UserDayDish  udd
		
		inner join deleted d on d.CompanyId=udd.CompanyId  and d.UserId=udd.UserId
					and d.[Date]=udd.[Date]
					And ( d.ComplexId=udd.ComplexId OR d.ComplexId=0)
		--inner join DishComplex dc on dc.CompanyId=d.CompanyId and	dc.ComplexId=d.ComplexId	and dc.DishId= udd.DishId
		
		where udd.IsComplex=1


		insert into  UserDayDish (CompanyId,UserId,[Date],DishId,ComplexId,Quantity,IsComplex)
		Select 
			CompanyId=i.CompanyId,
			UserId=i.UserId,
			[Date]=i.[Date],
			DishId=dc.DishId,
			ComplexId=dc.ComplexId,
			Quantity=i.Quantity,
			IsComplex=1
		from inserted  i
		inner join DishComplex dc on dc.CompanyId=i.CompanyId and  dc.ComplexId=i.ComplexId	

END
GO


