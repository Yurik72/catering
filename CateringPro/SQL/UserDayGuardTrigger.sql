--USE [CateringPro]
GO
/****** Object:  Trigger [dbo].[UserDayGuard_Trigger]    Script Date: 8/11/2020 11:01:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   TRIGGER  [dbo].[UserDayGuard_Trigger]
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