
declare @userid nvarchar(100)
set @userid='947d3d17-db3f-4d79-9ba2-f987ff61624e'



begin tran
delete  from userfinincomes where  id=@userid
delete  from userfinoutcomes where  id=@userid
delete  from userfinances where  id=@userid

delete  from userdaydish where  userid=@userid
delete  from userdaycomplex where  userid=@userid
delete  from userday where  userid=@userid

delete from CompanyUserCompanies where CompanyUserId=@userid
delete from aspnetusers where id=@userid
delete from aspnetusers where id=@userid

commit tran
