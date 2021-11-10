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
As
Begin try
INSERT INTO UserInfo (FirstName, LastName, City,Mobilenumber,Email,Password) VALUES(@Firstname,@Lastname,@City,@Mobilenumber,@Email,@Password)
end try
Begin catch
SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
END CATCH 

exec spRegister
'kiran', 'raj', 'blore','9638527410','kir@gmail.com','kir@123'


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
(@FirstName Nvarchar(50),
@Email Nvarchar(50))
As 
Begin try
select Password from UserInfo where @FirstName=FirstName and @Email=Email
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
'Jemmy','Jemmy@gmail.com'

--- Procedure to Reset password

alter procedure spUserResetPassword
(@Email varchar(50),
@CurrentPassword varchar(50),
@NewPassword varchar(50))
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
END CATCH e to reset password

 

exec spUserResetPassword
'Jane@gmail.com','Jane','Janey'


-------------------------------------------------------------------------------

CREATE TABLE Note
(
NotesId int primary key not null identity(1,1),  
Title Nvarchar(50),  
Description Nvarchar(50),  
Reminder Nvarchar(50),  
UserId int FOREIGN KEY REFERENCES UserInfo(UserId)
)



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