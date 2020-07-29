CREATE OR ALTER TRIGGER  [dbo].[UserDayGuard_Trigger]
   ON  [dbo].[UserDay]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
SET NOCOUNT ON;

IF Exists (Select * from Deleted where IsPaid=1)
BEGIN
    if @@trancount > 0
        rollback transaction;

    RAISERROR ('Attempt to delete userday , which is paid',16,1)
END


END