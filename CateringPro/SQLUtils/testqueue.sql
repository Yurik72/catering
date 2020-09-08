--select * from AspNetUsers order by ChildNameSurname
declare @userid nvarchar(100)
declare @userid1 nvarchar(100)
declare @userid2 nvarchar(100)
declare @userid3 nvarchar(100)
declare @userid4 nvarchar(100)
declare @userid5 nvarchar(100)
declare @userid6 nvarchar(100)
Declare @dayDate Date

set @userid='f4243aa7-7e6c-490a-838d-9c23373e8fb4'
set @userid1='01bc233a-c58a-4f59-bb89-c58d4fd71983'
set @userid2='02e03f88-3930-4cd1-864c-c494719932b7'
set @userid3='03127de6-c34d-4651-b311-10028c18fc3f'
set @userid4='057aeb0f-2f99-472c-9386-a6bb7e06e69d'
set @userid5='06271708-4064-4a20-9cbc-c347914900ba'
set @userid6='074fdd74-e101-411e-968d-1bb34bf6c717'
set @dayDate='20200908'

IF(1=2)
BEGIN
update UserDayDish set IsDelivered=0 where Date=@dayDate and UserId=@userid
update UserDayDish set IsDelivered=0 where Date=@dayDate and UserId=@userid1
update UserDayDish set IsDelivered=0 where Date=@dayDate and UserId=@userid2
update UserDayDish set IsDelivered=0 where Date=@dayDate and UserId=@userid3
update UserDayDish set IsDelivered=0 where Date=@dayDate and UserId=@userid4
update UserDayDish set IsDelivered=0 where Date=@dayDate and UserId=@userid5
update UserDayDish set IsDelivered=0 where Date=@dayDate and UserId=@userid6
END


select id,CardTag from AspNetUsers  where id=@userid or id=@userid1 or id=@userid2 or id=@userid3 or id=@userid4 or id=@userid5 or id=@userid6
--select * from UserDayDish where Date=@dayDate and UserId=@userid4
--select * from UserDayDish where Date=@dayDate and UserId=@userid5
select * from UserDayDish where Date=@dayDate and (UserId=@userid or UserId=@userid1 or UserId=@userid2 or UserId=@userid3 or UserId=@userid4 or UserId=@userid5 or UserId=@userid6)  order by userid
--select * from dishcomplex where ComplexId=41
--select * from complex where id=41
--update UserDayDish set IsDelivered=0 where Date=@dayDate and UserId=@userid
select *  from deliveryqueues