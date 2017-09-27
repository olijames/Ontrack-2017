ALTER TABLE [dbo].[Vehicle] DROP CONSTRAINT [FK_VehicleDriver_ContactID];

UPDATE
    v
SET
    v.vehicleDriver = e.employeeID
FROM
    employeeInfo e, contactcompany cc, vehicle v, contact c
WHERE 
	e.contactCompanyID = cc.contactCompanyID
	AND
	v.vehicleDriver = cc.contactID


INSERT  [dbo].[ContactCompany]
        ( [ContactCompanyID] ,
          [ContactID] ,
          [CompanyID] ,
          [CreatedBy] ,
          [CreatedDate] ,
          [Active] ,
          [Pending]
        )
VALUES  ( NEWID() ,
          '00000000-0000-0000-0000-000000000000' ,
          '00000000-0000-0000-0000-000000000000' ,
          '53e58c1b-4d58-41f9-9849-fbb5b4f87833' ,
          CAST('2017-02-21 16:04:33.833' AS DATETIME) ,
          1 ,
          0
        );

INSERT  [dbo].[EmployeeInfo]
        ( [EmployeeID] ,
          [ContactCompanyID] ,
          [FirstName] ,
          [LastName] ,
          [CompanyName] ,
          [Email] ,
          [Phone] ,
          [Address1] ,
          [Address2] ,
          [CreatedBy] ,
          [CreatedDate] ,
          [Active] ,
          [AccessFlags] ,
          [PayRate] ,
          [LabourRate] ,
          [Address3] ,
          [Address4]
        )
        SELECT  '00000000-0000-0000-0000-000000000000',
                contactCompanyID ,
                'default FirstName' ,
                'default LastName' ,
                '' ,
                'default@gmail.com' ,
                '1' ,
                '1' ,
                'Bexley' ,
                '53e58c1b-4d58-41f9-9849-fbb5b4f87833' ,
                CAST('2017-02-20 13:45:58.040' AS DATETIME) ,
                1 ,
                0 ,
                CAST(0.000 AS DECIMAL(13, 3)) ,
                CAST(0.000 AS DECIMAL(13, 3)) ,
                '' ,
                ''
        FROM    contactcompany
        WHERE   contactid = '00000000-0000-0000-0000-000000000000'


ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD CONSTRAINT [FK_VehicleDriver_EmployeeID] FOREIGN KEY([VehicleDriver])
REFERENCES [dbo].[EmployeeInfo] ([EmployeeID])
GO

alter table employeeinfo 
add DefaultVehicleID uniqueidentifier default '00000000-0000-0000-0000-000000000000' not null