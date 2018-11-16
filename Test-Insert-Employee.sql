INSERT INTO [dbo].[Employees]
           ([empLastName]
           ,[empFirstName]
           ,[empMiddleName]
           ,[empSalary]          
           ,[empTitle]          
           ,[UCTRegisteredOn]
		   ,[UCTStartDate])
     VALUES
           ('Mbiydzenyuy'
           ,'Arthur'
           ,'Kamsu'
           ,5000.12         
           ,'Senior .Net Engineer'       
           ,636776572410000000
		   ,636776572410000000)--Ticks for GMT / UTC Date Time 2018-11-12 22:07:21 + 0.0000000 second is: 636776572410000000
GO


SELECT * FROM Employees