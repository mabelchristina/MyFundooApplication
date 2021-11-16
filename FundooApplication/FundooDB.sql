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
		update UserInfo set password=@Currentpassword , @CurrentPassword=@Newpassword where Email=@Email;
		select * from UserInfo where Email = @Email;
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
'Jane@gmail.com','Jane','Janey'

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
@NotesId int,
@Title varchar(20),
@Description varchar(50),
@Reminder varchar(50)
)
As 
Begin try
update Note
set Title=@Title,
	Description=@Description,
	Reminder=@Reminder
where NotesId=@NotesId
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
    Select 1 as IsArchive
 End
 Else
 Begin
  Select 0 as IsNotArchive
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

exec spArchieve
1


--Procedure to check isPin in notes tables
Create procedure spPin
(
@NotesID int, @UserId int
)
As 
Begin try
if(exists(select * from Note where NotesId=@NotesID))
Begin
  Update Note
  Set pin = 1
  where NotesId = @NotesID
    Select 1 as IsPin
 End
 Else
 Begin
  Select 0 as IsNotPin
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
1,2
--Procedure to check isTrash in notes tables
Create procedure spTrash
(
@NotesID int, @UserId int
)
As 
Begin try
if(exists(select * from Note where NotesId=@NotesID))
Begin
  Update Note
  Set trash = 1
  where NotesId = @NotesID
    Select 1 as IsTrash
 End
 Else
 Begin
  Select 0 as IsNotTrash
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

exec spTrash
6,2


Select * from Note