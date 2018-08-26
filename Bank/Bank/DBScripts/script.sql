

Create table tblUser(    
ID bigint IDENTITY(1,1) NOT NULL,    
LoginName varchar(20) NOT NULL,
Password varchar(20) NOT NULL,
AccountNumber bigint,
Balance decimal default 0,
CreateDate DateTime NOT NULL
)

CREATE INDEX idx_tblUser_AccountNumber on tblUser (AccountNumber)




USE [bank]
GO
/****** Object:  StoredProcedure [dbo].[spAddUser]    Script Date: 26 Aug 2018 4:15:29 PM ******/
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
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
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
