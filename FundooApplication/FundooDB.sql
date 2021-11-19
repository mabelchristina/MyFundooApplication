create database FundooDB

USE FundooDB


CREATE TABLE UserInfo
(
UserId int primary key not null identity(1,1),  
FirstName Nvarchar(50),  
LastName Nvarchar(50),  
City Nvarchar(50), 
Mobilenumber varchar(10), 
Email Nvarchar(50),  
Password Nvarchar(50)
) 

ALTER TABLE UserInfo
ADD CONSTRAINT UC_Email UNIQUE (Email);

SELECT * FROM UserInfo

delete from UserInfo
where Email='niki@gmail.com'


INSERT INTO UserInfo (FirstName, LastName, City) VALUES('Jane', 'Althya', 'Dubai')
INSERT INTO UserInfo (FirstName, LastName, City) VALUES('Jemmy', 'Annie', 'Sarjah')
INSERT INTO UserInfo (FirstName, LastName, City) VALUES('Joy', 'Anna', 'Bangalore')
INSERT INTO UserInfo (FirstName, LastName, City) VALUES('Sharon', 'Lia', 'Bangalore')
INSERT INTO UserInfo (FirstName, LastName, City) VALUES('Joshua', 'Samuel', 'Mumbai')
INSERT INTO UserInfo (FirstName, LastName, City) VALUES('Jeremy', 'James', 'Chennai')
INSERT INTO UserInfo (FirstName, LastName, City) VALUES('James', 'John', 'Hyderabad')


UPDATE UserInfo SET Mobilenumber = REPLICATE(UserId, 10)
UPDATE UserInfo SET Email = FirstName+'@gmail.com'
UPDATE UserInfo SET Password = FirstName+REPLICATE(UserId,1)


---------------------------------   Stored Procedures    -------------------------------------------
-- Procedure to get all users data
Alter PROCEDURE spGetAllUsersInfo
As 
Begin try
select * from UserInfo
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH 

exec spGetAllUsersInfo


-- Procedure to Register particular user

alter PROCEDURE spRegister
(@Firstname Nvarchar(50),
@Lastname Nvarchar(50),
@City Nvarchar(50),
@Mobilenumber Nvarchar(50),
@Email Nvarchar(50),
 @Password Nvarchar(50))
as
begin
declare @res int
select @res = count(UserId) from UserInfo where Email = @Email
if (@res<>0)
begin
Raiserror('Email Id already registered with another user id.',16,1)
end
else
begin
Insert into UserInfo values (@FirstName, @LastName, @City, @Mobilenumber, @Email, @Password)
end
end
 

exec spRegister
'Terry', 'John', 'Kerala','7894561487','Terry@gmail.com','Terry@123'


-- Procedure to login to particular user
alter PROCEDURE spLogin
(@Email Nvarchar(50), @Password Nvarchar(50))
As
Begin try
select * from UserInfo where Email = @Email AND Password = @Password
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH 

exec spLogin
'Jane@gmail.com','Jane1'

--- Procedure for forgot password

alter procedure spUserForgotPassword
(@Email Nvarchar(50))
As 
Begin try
select Password from UserInfo where  @Email=Email
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH  

exec spUserForgotPassword
'Jemmy@gmail.com'

--- Procedure to Reset password

alter procedure spUserResetPassword
(
@Email varchar(50),
@CurrentPassword varchar(100),
@Newpassword varchar(100)
)
As 
Begin try
 if(Exists(Select *  from  UserInfo
     where Email = @Email
     and Password = @CurrentPassword))
 Begin
  Update UserInfo
  Set Password = @newpassword
  where Email = @Email
    Select 1 as IsPasswordChanged
 End
 Else
 Begin
  Select 0 as IsPasswordChanged
 End
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH 

 

exec spUserResetPassword
'Jane@gmail.com','Janey','Jane'

select * from UserInfo
-------------------------------------------------------------------------------

CREATE TABLE Note
(
NotesId int primary key not null identity(1,1),  
Title Nvarchar(50),  
Description Nvarchar(50),  
Reminder Nvarchar(50),  
UserId int FOREIGN KEY REFERENCES UserInfo(UserId)
)


alter table Note add color varchar(50),trash varchar(50),archive varchar(50),pin varchar(50)
alter table Note add CreatedDate Date, ModifiedDate Date

select * from Note
33
insert into Note (Title,Description,Reminder,UserId)values('To-Do','Shopping','Remind at 2',1)


----Procedure for getting all notes

alter PROCEDURE spGetAllNote
As
Begin try
Select * from Note
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spGetAllNote

----Procedure for adding a note

create PROCEDURE spAddUserNotes
(@Title Nvarchar(50),
@Description Nvarchar(50),
@Reminder Nvarchar(50),
@UserID Nvarchar(50))
As
Begin try
INSERT INTO Note(Title,Description,Reminder,UserId) VALUES(@Title,@Description,@Reminder,@UserId)
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spAddUserNotes
'Alarm','Nap','Remind at 4',1

-------Procedure for updating note

create procedure spUpdateNotes
(
@labelId int,
@labelName varchar(20),
@UserId varchar(50),
@NotesId varchar(50)
)
As 
Begin try
update NoteLabel
set labelName=@labelName,
	UserId=@UserId
	NotesId=@NotesId
where labelId=@labelId
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spUpdateNotes
1,'TODO','Shop','Alert at 5'


---Procedure to delete a note

Create procedure spDeleteNote
(
@Title varchar(20)
)
As 
Begin try
delete from Note where Title=@Title 
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH  

exec spDeleteNote
'alarm'

Select * from Note


--Procedure to check isArchive in notes tables

Alter procedure spArchieve
(
@NotesID int
)
As 
Begin try
if(exists(select * from Note where NotesId=@NotesID))
Begin
  Update Note
  Set archive = 1
  where NotesId = @NotesID
 end  
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH  

exec spArchieve
1


--Procedure to check isPin in notes tables
Alter procedure spPin
(
@NotesID int
)
As 
Begin try
if(exists(select * from Note where NotesId=@NotesID))
Begin
  Update Note
  Set pin = 1
  where NotesId = @NotesID

 End
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH  

exec spPin
1
--Procedure to check isTrash in notes tables
Alter procedure spTrash
(
@NotesID int, @UserId int
)
As 
Begin try
declare @count int
select @count =count (@NotesID) from Note where UserId=@UserId and NotesId=@NotesID
if(@count<>1)
begin
Raiserror('Note is not present',16,1)
end
else
begin
delete from Note where NotesId=@NotesID 
update Note set trash=trash^1 where UserId=@UserId and NotesId=@NotesID
/*select * from UserInfo where UserId=@UserId and @NotesID=@NotesID*/
end
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH  

exec spTrash
6,2


Select * from Note

--Procedure to check isTrash in notes tables
Alter procedure spColor
(
@NotesID int, @Color varchar(50)
)
As 
Begin try
  Update Note
  Set color = @color
  where NotesId = @NotesID
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH  

exec spColor
6,'#FFC0CB'



create table NoteLabel(
labelId int identity(1,1) primary key,
labelName varchar(50),
UserId int not null foreign key references UserInfo(UserId),
noteId int not null foreign key references Note(NotesID),
registeredDate datetime default GETDATE(),
modifiedDate datetime null
)


select * from NoteLabel


insert into NoteLabel (labelName,UserId,noteId)values('Books',1,1)
insert into NoteLabel (labelName,UserId,noteId,modifiedDate) values('Grocery',1,1,SYSDATETIME())


create procedure spGetAllLabel
As
Begin try
select * from NoteLabel 
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spGetAllLabel 



create procedure spAddLabel
(
@labelName varchar(50),
@userId int,
@noteId int 
)
As
Begin try
insert into NoteLabel(labelName,userId,noteId)values(@labelName,@userId,@noteId)
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spAddLabel 
'Shopping',2,1


Alter procedure spUpdateLabel
(
@labelName varchar(50),
@userId int,
@noteId int 
)
As
Begin try
Update  NoteLabel set labelName=@labelName
 where UserId= @userId and noteId =@noteId
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spUpdateLabel 
'Movie',2,1

select * from NoteLabel


alter procedure spDeleteLabel     
(      
   @labelId int    
)     
as       
begin try      

 Delete from NoteLabel Where
labelId =@labelId 

End try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spDeleteLabel
5