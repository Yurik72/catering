ALTER TRIGGER [dbo].[UserDayDish_Trigger]
   ON  [dbo].[UserDayDish]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	return;
	Insert into UserDay(CompanyId,UserId,Date)
	Select i.CompanyId,i.UserId,i.date
	from inserted i
	Left Outer Join UserDay ud  on ud.CompanyId=i.CompanyId and ud.UserId=i.UserId and ud.date=i.date
	Where ud.CompanyId is Null
	Group By i.CompanyId,i.UserId,i.date

	Update UserDay
	SET
		Quantity=Quantity-sub.TotalQuantity,
		Total=Total-sub.TotalAmount

	from (Select 
			CompanyId=d.CompanyId,
			UserId=d.UserId,
			Date=d.date,
			TotalQuantity=Sum(d.Quantity),
			TotalAmount=Sum(d.Quantity*d.Quantity)
			from deleted d
			where d.IsComplex=0
			Group by d.CompanyId,d.UserId,d.date) sub
	where UserDay.CompanyId=sub.CompanyId and UserDay.UserId=sub.UserId and UserDay.date=sub.date

	Update UserDay
	SET
		Quantity=Quantity+sub.TotalQuantity,
		Total=Total+sub.TotalAmount

	from (Select 
			CompanyId=i.CompanyId,
			UserId=i.UserId,
			Date=i.date,
			TotalQuantity=Sum(i.Quantity),
			TotalAmount=Sum(i.Quantity*i.Quantity)
			from inserted i
			where i.IsComplex=0
			Group by i.CompanyId,i.UserId,i.date) sub
	where UserDay.CompanyId=sub.CompanyId and UserDay.UserId=sub.UserId and UserDay.date=sub.date

	Update UserDay
	SET
		Quantity=Quantity -sub.TotalQuantity,
		Total=Total-sub.TotalAmount

	from (Select 
			CompanyId=i.CompanyId,
			UserId=i.UserId,
			Date=i.date,
			TotalQuantity=Sum(uc.Quantity),
			TotalAmount=Sum(uc.Quantity*uc.Price)
			from (Select  CompanyId,UserId,date
					from deleted 
					where IsComplex=1
					Group by CompanyId,UserId,date
				)i,
			UserDayComplex uc
			where  uc.CompanyId=i.CompanyId
				  And uc.UserId=i.UserId
				  And uc.Date=i.Date
			Group by i.CompanyId,i.UserId,i.date) sub
	where UserDay.CompanyId=sub.CompanyId and UserDay.UserId=sub.UserId and UserDay.date=sub.date

	Update UserDay
	SET
		Quantity=Quantity +sub.TotalQuantity,
		Total=Total+sub.TotalAmount

	from (Select 
			CompanyId=i.CompanyId,
			UserId=i.UserId,
			Date=i.date,
			TotalQuantity=Sum(uc.Quantity),
			TotalAmount=Sum(uc.Quantity*uc.Price)
			from (Select  CompanyId,UserId,date
					from inserted 
					where IsComplex=1
					Group by CompanyId,UserId,date
				)i,
			UserDayComplex uc
			where  uc.CompanyId=i.CompanyId
				  And uc.UserId=i.UserId
				  And uc.Date=i.Date
			Group by i.CompanyId,i.UserId,i.date) sub
	where UserDay.CompanyId=sub.CompanyId and UserDay.UserId=sub.UserId and UserDay.date=sub.date

END