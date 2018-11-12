CREATE SEQUENCE employee_code_seq
  AS BIGINT
  START WITH 1
  INCREMENT BY 1
  MINVALUE 1
  NO MAXVALUE
  NO CYCLE
  NO CACHE;

GO
CREATE TABLE [dbo].[Employees]
(
	[empId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newid(), 
    [empLastName] VARCHAR(50) NOT NULL, 
    [empFirstName] VARCHAR(50) NULL, 
    [empMiddleName] VARCHAR(50) NULL, 
    [empSalary] DECIMAL(6, 2) NOT NULL, 
    [empManager] UNIQUEIDENTIFIER NULL, 
    [empTitle] VARCHAR(50) NOT NULL, 
    [empCode] CHAR(8) NOT NULL, 
    [registeredOnUTC] BIGINT NOT NULL, 
    CONSTRAINT [FK_Employees_Manager] FOREIGN KEY ([empManager]) REFERENCES [Employees]([empId]), 
    CONSTRAINT [CK_Employees_empCode] CHECK (empSalary > 0)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is the primary key for identifying an employee',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Employees',
    @level2type = N'COLUMN',
    @level2name = N'empId'
GO

CREATE INDEX [IX_Employees_LastName] ON [dbo].[Employees] ([empLastName])

GO

CREATE INDEX [IX_Employees_FirstName] ON [dbo].[Employees] ([empFirstName])

GO

CREATE TRIGGER [dbo].[Trigger_Employee_Inserted_GenerateMatricule]
    ON [dbo].[Employees]
    INSTEAD OF INSERT
    AS
    BEGIN
		SET NOCOUNT ON;
		declare @codeEmp  char(8)
		set @codeEmp  = CONVERT(char(8), NEXT VALUE FOR employee_code_seq)
		INSERT INTO Employees(empLastName,empFirstName,empMiddleName,empSalary,empManager,empTitle,empCode)
		VALUES(inserted.empLastName,inserted.empFirstName,inserted.empMiddleName,inserted.empSalary,inserted.empManager,inserted.empTitle,@codeEmp)

    END
GO


CREATE UNIQUE INDEX [IX_Employees_empCode] ON [dbo].[Employees] ([empCode])
