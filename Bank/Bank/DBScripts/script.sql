
USE [bank]
GO

Create table tblUser(    
ID bigint IDENTITY(1,1) NOT NULL,    
LoginName varchar(20) NOT NULL,
Password varchar(20) NOT NULL,
AccountNumber bigint,
Balance decimal(18,2) default 0,
CreateDate DateTime NOT NULL
)

CREATE INDEX idx_tblUser_AccountNumber on tblUser (AccountNumber)




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spAddUser]     
(    
    @LoginName VARCHAR(20),     
    @Password VARCHAR(20)
)    
as     
Begin     
	DECLARE @num bigint;

    Insert into tblUser (LoginName,Password, CreateDate)     
    Values (@LoginName,@Password, GETUTCDATE())     

	SELECT @num = id FROM tblUser WHERE LoginName = @LoginName

	UPDATE tblUser SET AccountNumber = @num WHERE LoginName = @LoginName
End





SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spGetAllUsers
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ID, LoginName, Password, AccountNumber, Balance, CreateDate FROM tblUser
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE spValidateUserLogin  
(  
    @LoginName VARCHAR(20) ,  
    @Password VARCHAR(20)  
)  
AS  
BEGIN  
  
    DECLARE @authentication VARCHAR(10)='Failed'  
  
    IF EXISTS(SELECT 1 FROM tblUser WHERE LoginName=@LoginName AND Password=@Password)  
    BEGIN  
        SET @authentication='Success'  
    END  
      
    SELECT @authentication AS isAuthenticated  
  
END  




USE [bank]
GO

Create table tblTransaction(    
ID bigint IDENTITY(1,1) NOT NULL,    
AccountNumber bigint,
Type varchar(20),
Amount decimal(18,2) default 0,
Source bigint,
Destination bigint,
CreateDate DateTime NOT NULL
)

CREATE INDEX idx_tbTransaction_AccountNumber on tblTransaction (AccountNumber)






SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].spAddTransaction     
(    
    @AccountNumber bigint,     
	@Type varchar(20),
	@Amount decimal(18,2),
	@Source bigint,
	@Destination bigint
)    
as     
Begin     
	DECLARE @num bigint;

    Insert into tblTransaction (AccountNumber,Type,Amount,Source,Destination,CreateDate)     
    Values (@AccountNumber,@Type,@Amount,@Source,@Destination, GETUTCDATE())     

End





SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetAllTransactionsByAccountNumber]
(
	@AccountNumber bigint
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ID, AccountNumber,Type,Amount,Source,Destination,CreateDate FROM tblTransaction WITH (NOLOCK)
	WHERE AccountNumber = @AccountNUmber
	ORDER BY ID DESC
END
GO


















