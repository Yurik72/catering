
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
CREATE OR ALTER PROCEDURE [dbo].[MakeOrderPayment]
	-- Add the parameters for the stored procedure here
	@DayDate Date,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @Updated table( 
    Operation nvarchar(30) NOT NULL,  
    UserId nvarchar(100) NOT NULL,  
    DayDate date NOT NULL
	);

WITH UserSummary(Total,cnt,UserId,ItemsCount)
AS
(
	Select Sum(Total) as Total,Count(*) as cnt,Sum(Quantity) as ItemsCount, UserId from UserDay 
	where Date=@DayDate AND CompanyId=@CompanyId and IsConfirmed=1 and IsPaid=0
	Group By UserId
)


--Select * from UserSummary


MERGE UserFinOutComes AS target
USING ( Select * from UserSummary) AS source (Total,cnt,ItemsCount,UserId)
ON (target.Id = source.UserId and target.DayDate=@DayDate and target.OutComeType=1 and target.CompanyId=@CompanyId) 
WHEN MATCHED
	 THEN UPDATE SET target.Amount = source.Total ,
					 target.ItemsCount=source.ItemsCount,
					 target.TransactionDate=getDate()
WHEN NOT MATCHED  
    THEN INSERT(Id,DayDate,OutComeType,Amount,ItemsCount,TransactionDate,Comments,CompanyId) 
	VALUES(source.UserId,@DayDate,1,source.Total,source.ItemsCount,getDate(),'auto pay',@CompanyId)
OUTPUT $action, inserted.id,inserted.DayDate into @Updated; 



Update UserDay
Set IsPaid=1
From UserDay,@Updated upd
Where UserDay.UserId=upd.UserId and UserDay.Date=@DayDate and UserDay.CompanyId=@CompanyId;



WITH UserSummaryPaid(UserId)
AS
(
	Select  UserId from UserDay 
	where Date=@DayDate AND CompanyId=@CompanyId and IsConfirmed=1 and IsPaid=1
	Group By UserId
)

Delete from UserFinOutComes
from UserFinOutComes ufo
LEFT JOIN UserSummaryPaid d on d.UserId=ufo.Id
where ufo.DayDate=@DayDate and ufo.CompanyId=@CompanyId and  d.UserId IS NULL

/*
MERGE UserFinOutComes AS target
USING ( Select * from UserSummaryPaid) AS source (UserId)
ON (target.Id = source.UserId and target.DayDate=@DayDate and target.OutComeType=1 ) 
WHEN NOT MATCHED BY SOURCE THEN DELETE;
*/

END