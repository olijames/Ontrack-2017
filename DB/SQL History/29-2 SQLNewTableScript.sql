select NEWID() as tempid, vehicle.vehicleid as vehicle, Material.MaterialName, Material.CostPrice, Material.SellPrice, Material.RRP, supplier.suppliername,
                    material.uom, material.materialid, supplierinvoicematerial.qty, supplierinvoicematerial.qtyremainingtoassign, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate, SupplierInvoice.Active, supplier.supplierid,
					supplierinvoicematerial.SupplierInvoiceMaterialID
                    from SupplierInvoiceMaterial, Material, Vehicle, SupplierInvoice, Supplier
                    where 
                    SupplierInvoiceMaterial.VehicleID='08F88AD1-7960-4F57-A899-94BF89792EBD' 
                    and supplier.supplierid=supplierinvoice.supplierid
					and SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID
                    and supplierinvoice.contractorid='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
					and Vehicle.VehicleOwner='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
					and Vehicle.VehicleDriver='53E58C1B-4D58-41F9-9849-FBB5B4F87833'
					and SupplierInvoiceMaterial.MaterialID=Material.MaterialID
                    order by material.MaterialName

					select * from Vehicle where VehicleDriver='53E58C1B-4D58-41F9-9849-FBB5B4F87833'




















exec sp_rename 'ContactCustomer','ContractorCustomer'
alter table ContractorCustomer drop column CustomerIDfromContactTable
alter table ContractorCustomer
add ContractorID uniqueidentifier null
alter table ContractorCustomer
add ContactIDCustomer uniqueidentifier null
update ContractorCustomer set ContactIDCustomer=contact.ContactID from customer,contact,ContractorCustomer where ContractorCustomer.customerid = customer.CustomerID and contact.ContactID=customer.ContactID
update ContractorCustomer set contractorid=ContactID

update site set CustomerAddress1 ='12 High St'  where SiteID='3F189C05-BBE0-4A3B-A4D8-A35A04C71F62'

select * from SupplierInvoiceMaterial
select * from task where TaskID='48C0380A-C3C2-454D-87A4-805A711A3A26'
select * from job where JobID='D135BC93-CB83-408D-A7BA-F1D00D17084B'

update task set status=0 where Description='Disconnect bedrooms 1,2 and 3'
select * from task where Description='Disconnect bedrooms 1,2 and 3'
select * from job where jobid='C2C10CF6-A722-4077-A11B-4D0BE614FDE3'
update job set jobtype=1 where jobid='C2C10CF6-A722-4077-A11B-4D0BE614FDE3'

"INSERT ContractorCustomer ([ContactCustomerID],[ContactID],[ContractorID],[CustomerID],[ContactIDCustomer],[CreatedBy],[CreatedDate],[Active])
 VALUES ('39375208-6c14-4163-afd9-25a96f193109','c9a7cebf-c61a-4386-947f-004eab5e3a82','c9a7cebf-c61a-4386-947f-004eab5e3a82','c9a7cebf-c61a-4386-947f-004eab5e3a82','c9a7cebf-c61a-4386-947f-004eab5e3a82','00000000-0000-0000-0000-000000000000','2016-05-08 17:58:47.471',1)"
 select * from Job


alter table ContractorCustomer add constraint uq_Contractorid_contactidcustomer unique(contactcustomerid,contractorid,contactidcustomer)

--To remove inactive sites from contactsite table
delete from ContactSite where SiteID in (select SiteID from Site where Active=0)



--POPULATE JOBCONTRACTOR TABLE 
alter table jobcontractor
drop constraint uq_jobcontractor
delete JobContractor
insert into JobContractor
select NEWID(), job.jobid, ContactSite.ContactID, '00000000-0000-0000-0000-000000000000', GETDATE(), 1, 0
 from site, job, ContactSite where site.siteid=job.SiteID and contactsite.siteid=site.siteid
  create table tempo(a uniqueidentifier, b uniqueidentifier)
 insert into tempo
SELECT distinct jobid, contactid FROM JOBCONTRACTOR
delete JobContractor 
insert into JobContractor
select NEWID(), tempo.a, tempo.b, '00000000-0000-0000-0000-000000000000', GETDATE(), 1, 0
 from tempo
 drop table tempo
ALTER TABLE jOBcONTRACTOR ADd constraint uq_jobcontractor UNIQUE (contactid,jobid)

--
alter table taskmaterial
add FromInvoice bit default 0
with values

--
update task set task.siteid=job.SiteID
from task, job where task.jobid=job.jobid

--Remove duplicates from contactsite table FAULTY!! SETS ALL TO ACTIVE - NEED TO FIX
 create table tempo(a uniqueidentifier, b uniqueidentifier)
 insert into tempo
SELECT distinct contactid, siteid FROM ContactSite
delete ContactSite
insert into ContactSite
select NEWID(), tempo.a, tempo.b, '00000000-0000-0000-0000-000000000000', GETDATE(), 1, 0
 from tempo
 drop table tempo
--Mandeep's fix to above
update ContactSite set Active=0
update ContactSite set Active=1 where ContactSite.SiteID in (select distinct Job.SiteID from Job where JobStatus=0)







--populate task.customerid
update task set TaskCustomerID=site.SiteOwnerID from site where task.siteid=site.SiteID
update task set task.TaskCustomerID=contactsite.ContactID from task,ContactSite, site where contactsite.siteid=site.siteid and contactsite.contactid != site.siteownerid 
and contactsite.contactid != 'ECA7B55C-3971-41DA-8E84-A50DA10DD233' and task.siteid=ContactSite.SiteID

--26/4
alter table material
alter column rrp decimal (18,2)

alter table vehicle
add CreatedDate date
alter table vehicle
add CreatedBy uniqueidentifier
alter table vehicle
add Active bit





































select * from Contact where CompanyName like '%cube%'

select * from contact where ContactID='F6A1BE56-2137-483A-95EA-18628AC595F2'
select * from contact where ContactID='3EFE2965-09B8-481A-A6D8-1C53B64E0853'
select * from contact where ContactID='9153C98E-A589-4047-B136-10269543D38A'
select * from customer where ContactID='C9A7CEBF-C61A-4386-947F-004EAB5E3A82'
select * from Customer where ContactID='8d35e543-036f-48c5-a57e-02dda9db0b3a'
update Contact set Active=1 where  ContactID='8d35e543-036f-48c5-a57e-02dda9db0b3a'
select * from ContactCustomer where ContractorID='8d35e543-036f-48c5-a57e-02dda9db0b3a'
update ContactCustomer set Active=0 where ContactCustomerID='9D6D6341-5855-493C-87DB-D1D738E54850'
select * from ContactCustomer where CustomerID='3755D2FF-0C9A-4102-805D-FAC7A1A4C704'
--execute this
delete from ContactCustomer where ContactCustomerID='9D6D6341-5855-493C-87DB-D1D738E54850'
select * from ContactCustomer where ContactID='C9A7CEBF-C61A-4386-947F-004EAB5E3A82'
select * from TaskMaterial

update task set TaskCustomerID=site.SiteOwnerID from site where task.siteid=site.SiteID
update task set task.TaskCustomerID=contactsite.ContactID from task,ContactSite, site where contactsite.siteid=site.siteid and contactsite.contactid != site.siteownerid 
and contactsite.contactid != 'ECA7B55C-3971-41DA-8E84-A50DA10DD233' and task.siteid=ContactSite.SiteID

--populate task.custid=siteownerid
--then
--and where contactsite.siteid=site.siteid and contactsite.contactid != site.siteownerid and contactsite.contactid != ecid




select * from Vehicle
select * from SupplierInvoiceMaterial where VehicleID!='00000000-0000-0000-0000-000000000000'


select * from site where address1 like '%wara%'
select * from task

update SupplierInvoiceMaterial set VehicleID='08F88AD1-7960-4F57-A899-94BF89792EBD' where SupplierInvoiceMaterialID='8BC4B05F-F0C8-40D2-98F7-65E5DF41F5D0'

select * from material order by CreatedDate

select * from contact where ContactID='4946D2B7-D1D2-4C35-946F-84162387083D'

alter table material
alter column rrp decimal (18,2)
select * from vehicle

select * from Contact where email like '%electr%'
----populate task.customerid with zero's SENT
--update task set TaskCustomerID='00000000-0000-0000-0000-000000000000'
--ALTER TABLE task add DEFAULT '00000000-0000-0000-0000-000000000000' FOR taskcustomerid;


 --contactcustomer has duplicates - need to remove cube had duplicate and a test had duplicates too
 --delete from ContactCustomer where ContactCustomerID='8D3E0E2B-58A2-4470-B41B-D6E484C2A0AF'
 --select * from Customer where CustomerID='CF7C7E8A-A223-4F86-A411-7F66A39C01CF'
 --delete from ContactCustomer where CustomerIDfromContactTable='64063368-C083-46A2-890E-807DBF008D13'
  
--REMOVE DUPLICATES FROM CONTACTSITE TABLE 20/4 JARED LOCAL
-- create table tempo(a uniqueidentifier, b uniqueidentifier)
-- insert into tempo
--SELECT distinct contactid, siteid FROM ContactSite
--delete ContactSite
--insert into ContactSite
--select NEWID(), tempo.a, tempo.b, '00000000-0000-0000-0000-000000000000', GETDATE(), 1, 0
-- from tempo
 --drop table tempo

--
--update task set task.siteid=job.SiteID
--from task, job where task.jobid=job.jobid



-------------------------------------------------------------------------------------------------------------------------aBOVE AFTER 19/4/16

--executed live and local 12/4 jared
--CREATE TABLE [dbo].[ToolBoxFile](
--	[ToolBoxFileID] [uniqueidentifier] NOT NULL,
--	[SiteID] [uniqueidentifier] NOT NULL,
--	[FileID] [uniqueidentifier] NOT NULL,
--	[CreatedBy] [uniqueidentifier] NOT NULL,
--	[Active] [bit] NOT NULL,
--	[CreatedDate] [datetime] NULL,
-- CONSTRAINT [PK_ToolBoxFile] PRIMARY KEY CLUSTERED 
--(
--	[ToolBoxFileID] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--) ON [PRIMARY]

--GO

--ALTER TABLE [dbo].[ToolBoxFile]  WITH CHECK ADD  CONSTRAINT [FK_ToolBoxFile_FileUpload] FOREIGN KEY([FileID])
--REFERENCES [dbo].[FileUpload] ([FileID])
--GO

--ALTER TABLE [dbo].[ToolBoxFile] CHECK CONSTRAINT [FK_ToolBoxFile_FileUpload]
--GO

--ALTER TABLE [dbo].[ToolBoxFile]  WITH CHECK ADD  CONSTRAINT [FK_ToolBoxFile_Site] FOREIGN KEY([SiteID])
--REFERENCES [dbo].[Site] ([SiteID])
--GO

--ALTER TABLE [dbo].[ToolBoxFile] CHECK CONSTRAINT [FK_ToolBoxFile_Site]
--GO





-----------------------------------------
--        ADD new tables
--1. Supplier
--2. SupplierInvoice
--3. SUpplierMaterial(Later changed to "SupplierInvoiceMaterial")
--4. ContainerMaterial
--5. Vehicle
--6. Container

--        ADD new columns
--1. Material Table: SupplierID, SupplierProductCode, UOM
--2. Site Table: SiteOwnerID
-----------------------------------------

--Executed 3.15pm 29/2 Jared
--CREATE TABLE [dbo].[Supplier](
--	[SupplierID] [uniqueidentifier] NOT NULL,
--	[SupplierName] [nvarchar](50) NOT NULL,
--	[CreatedBy] [uniqueidentifier] NOT NULL,
--	[CreatedDate] [datetime] NOT NULL,
--	[Active] [bit] NOT NULL,
--	primary key (SupplierID),
--	unique (SupplierName))

--executed 5:38pm 29/2 Jared
--Create table ontrack3.dbo.[SupplierInvoice](
--[SupplierInvoiceID] [uniqueidentifier] not null,
--[SupplierID] [uniqueidentifier] not null,
--[ContractorReference] [nvarchar] null,
--[SupplierReference] [nvarchar] not null,
--[ContractorID] [uniqueidentifier] not null,
--[InvoiceDate] [datetime] not null;
--[CreatedBy] [uniqueidentifier] NOT NULL,
--[CreatedDate] [datetime] NOT NULL,
--[Active] [bit] NOT NULL,
--Primary key (SupplierINvoiceID))

--executed 5:39pm 29/2 Jared
--Create table ontrack3.dbo.supplierMaterial(
--SupplierMaterialID uniqueidentifier not null,
--MaterialID uniqueidentifier not null,
--SupplierInvoiceID uniqueIdentifier not null,
--primary key (SupplierMaterialID))

--executed 5:40pm 29/2 Jared
--create table ontrack3.dbo.ContainerMaterial(
--ContainerMaterialID uniqueidentifier not null,
--MaterialID uniqueidentifier not null,
--ContainerID uniqueidentifier not null,
--Qty int not null,
--primary key (ContainerMaterialID))

--executed 5:41pm 29/2 Jared
--create table ontrack3.dbo.Vehicle(
--VehicleID uniqueidentifier not null,
--VehicleOwner uniqueidentifier not null,
--VehicleDriver uniqueidentifier not null,
--VehicleName nvarchar not null,
--VehicleRegistration nvarchar not null,
--WOFDueDate datetime null,
--RegoDueDate datetime null,
--InsuranceDueDate datetime null,
--Primary key (VehicleID))

--executed 5:42pm 29/2 Jared
--Create table ontrack3.dbo.Container(
--ContainerID uniqueidentifier not null,
--SiteID uniqueIdentifier not null,
--VehicleID uniqueIdentifier not null,
--ContainerName nvarchar not null,
--primary key (ContainerID))

--executed 5:43pm 29/2 Jared
--ALTER TABLE ontrack3.dbo.material ADD SupplierID uniqueidentifier NULL, SupplierProductCode nvarchar(20) NULL, UOM nvarchar(20) null;

--executed 5:44pm 29/2 Jared
--Alter table ontrack3.dbo.site Add SiteOwnerID uniqueidentifier null;



----------------------------------------------------
--    Mandeep has above code.
--              1. Add foreign keys
--				2. Rename SupplierMaterial to SupplierInvoiceMaterial
----------------------------------------------------



--executed 8:53am 2/3 Jared
--alter table ontrack3.dbo.supplierinvoice
--add constraint FK_SupplierInvoice_SupplierID 
--foreign key (SupplierID)
--references Supplier(SupplierID);

--executed 8:54am 2/3 Jared
--alter table ontrack3.dbo.supplierinvoice
--add constraint FK_SupplierInvoice_CreatedBy 
--foreign key (CreatedBy)
--references Contact(ContactID);

--executed 8:57am 2/3 Jared
--alter table ontrack3.dbo.supplierinvoice
--add constraint FK_SupplierInvoice_Contractor 
--foreign key (ContractorID)
--references Contact(ContactID);

--executed 9:53am 2/3 Jared
--use Ontrack3;
--go
--exec sp_rename supplierMaterial, SupplierInvoiceMaterial;
--go

--executed 9:56am 2/3 Jared
--alter table ontrack3.dbo.supplierinvoicematerial
--add constraint FK_SupplierInvoiceMaterial_SupplierInvoiceID
--foreign key (SupplierInvoiceID)
--references SupplierInvoice(SupplierInvoiceID);

--executed 10:11am 2/3 Jared
--alter table ontrack3.dbo.ContainerMaterial
--add constraint FK_ContainerMaterial_ContainerID
--foreign key (ContainerID)
--references Container(ContainerID);

--executed 10:16am 2/3 Jared
--alter table ontrack3.dbo.Vehicle
--add constraint FK_VehicleOwner_ContactID
--foreign key (VehicleOwner)
--references Contact(ContactID);

--executed 10:17am 2/3 Jared
--alter table ontrack3.dbo.Vehicle
--add constraint FK_VehicleDriver_ContactID
--foreign key (VehicleDriver)
--references Contact(ContactID);

--executed 10:26am 2/3 Jared
--alter table ontrack3.dbo.Container
--add constraint FK_SiteID
--foreign key (SiteID)
--references Site(SiteID);

--executed 10:27am 2/3 Jared
--alter table ontrack3.dbo.Container
--add constraint FK_VehicleID
--foreign key (VehicleID)
--references Vehicle(VehicleID);
--alter table ontrack3.dbo.SupplierInvoiceMaterial add QTY integer null;


---------------------------------------------------------------
--            Populate Material table with supplierID data
---------------------------------------------------------------

--executed 10:47am 2/3 Jared
--insert into Supplier (SupplierID, SupplierName, Active, CreatedBy, CreatedDate)
--values 
--('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 'Supplied by us', 1, '00000000-0000-0000-0000-000000000000', SYSDATETIME() );

--executed 10:52am 2/3 Jared
--update Material set SupplierID = 'AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA';



---------------------------------------------
--              Sorting out siteowners
-- putting customer info from site table into contact table where a CustomerEmail has been entered
------------------------------------------------

--executed 12:51pm 2/3 Jared
--alter table site add intCount int;
--alter table contact add intCount int;
--update contact set intCount = 0;

--executed 12:59pm 2/3 Jared    from site SET INTCOUNT VALUES TO BE FROM 1 TO ....
--declare @RN int
--set @RN = 0
--Update Site
--set intCount = @RN
--    , @RN = @RN + 1;

--executed 1:43pm 2/3 Jared      (approx 3/4 of records, MATCHED contact table against site.email)
--update site set site.SiteOwnerID = contact.ContactID 
--from contact
--where Contact.username = site.CustomerEmail

--executed Jared
--use Ontrack3;
--go
--exec sp_rename 'contact.intcount', 'iCount';
--go

--executed 12:23pm 2/3 Jared
--Update site set CustomerEmail = CustomerFirstName + '.' + CustomerLastName + '.' + CustomerAddress1 + '@NoEmail.com'
--where CustomerEmail = '?' or CustomerEmail= '';

--executed 11:09pm 2/3 Jared
--	update job set SiteID='FAE41EA9-A87E-4F1B-9169-BB625B20AF58' where siteid='D7DBD172-6173-47A0-8A14-49795BA51A89'

--executed 11:14 2/3 Jared
-- delete from ContactSite where siteid='D7DBD172-6173-47A0-8A14-49795BA51A89'
-- delete from site where siteid='D7DBD172-6173-47A0-8A14-49795BA51A89'
	


----EXECUTED  11:15PM 2/3 Jared			
--insert into Ontrack3.dbo.Contact
--    ( ContactID , Username, passwordhash, firstname, lastname, companyname, email, 
--	phone, address1, address2, createdby, createddate, active, contacttype, bankaccount, SubscriptionExpiryDate, SubscriptionPending, subscribed, 
--	managerid, CompanyKey, PendingUser,	CustomerExclude, DefaultQuoteRate, DefaultChargeUpRate, JobNumberAuto, Address3, Address4, iCount )
--select newid() , site.CustomerEmail, '',site.CustomerFirstName, site.CustomerLastName, '', site.CustomerEmail, 
--	site.CustomerPhone, site.CustomerAddress1, site.CustomerAddress2, '00000000-0000-0000-0000-000000000000' ,SYSDATETIME(), 1, 1, '', SYSDATETIME(), 0, 0, 
--	'00000000-0000-0000-0000-000000000000', '',0 ,0, 0, 998, 0, 'Christchurch', 'Canterbury', site.intCount 
--    from site where SiteOwnerID IS null and customeremail not like '%noemail.com%'

------EXECUTED  11:17PM 2/3 Jared			
--update site set site.SiteOwnerID = contact.ContactID
--from site, Contact
--where site.intCount=contact.iCount

----------------------------------------------
-- Remaining siteownerID's become site.contactID
----------------------------------------------
--------EXECUTED  11:25PM 2/3 Jared			
--update site set site.siteownerid = site.contactid where CustomerEmail like '%noemail.com%'
--------EXECUTED  11:27PM 2/3 Jared			
--update site set site.CustomerEmail = Contact.Email 
--from site, contact
--where site.ContactID=contact.ContactID and site.CustomerEmail like '%noemail.com%'

----------------------------------------------------
--          SORTING OUT JOB VISIBILITY 
--Make it so that site.siteOwner can see site, all jobs and tasks(but not edit any tasks that arent theirs(unless no data in them)
-- AND jobcontractor.contactid can see the site, but only the jobs that are in the jobcontractor table.
--if no data in tasks for a jobcontractor.contactid then they can be removed from job.
----------------------------------------------------

--executed 1:45pm 4/3 Jared
--alter table task add TaskCustomerID uniqueidentifier null;
--executed 1:49pm 4/3 Jared
--alter table task add SiteID uniqueidentifier null;

--executed 9:94am 7/3 Jared
--alter table SupplierInvoice 
--alter column ContractorReference nvarchar(30) null;
--alter table SupplierInvoice 
--alter column  supplierreference nvarchar(30) null;
--insert into Supplier
--values
--('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Corys Electrical', '00000000-0000-0000-0000-000000000000', GETDATE(), 1)


--executed 12:16pm 7/3 Jared
--alter table container
--drop constraint fk_siteID
--alter table container
--drop constraint fk_vehicleID
--alter table containermaterial
--drop constraint fk_containermaterial_containerid
--drop table container
--alter table containermaterial
--drop column containerid 
--alter table containermaterial
--add
--SupplierInvoiceMaterialID uniqueidentifier not null,
--SiteID uniqueidentifier not null,
--VehicleID uniqueidentifier not null,
--TheContainerIsASite bit not null;
--alter table ontrack3.dbo.Containermaterial
--add constraint FK_VehicleID
--foreign key (VehicleID)
--references Vehicle(VehicleID);
--alter table ontrack3.dbo.Containermaterial
--add constraint FK_SiteID
--foreign key (SiteID)
--references Site(SiteID);
--INSERT into Site
--values
--('AAAABBBB-AAAA-AAAA-AAAA-AAAABBBBCCCC',	'BlankForContainerMaterial',	'.',	'.',	'.',	'.',	'.',	'.',	'BLANK',	'00000000-0000-0000-0000-000000000000',	'2015-11-17 14:55:57.273',	1,	'00000000-0000-0000-0000-000000000000',	2,	NULL,	NULL,	'00000000-0000-0000-0000-000000000000',	200)


--executed 10:54am 8/3 Jared
--alter table material
--add ContactID uniqueidentifier null;
--update material set ContactID ='ECA7B55C-3971-41DA-8E84-A50DA10DD233';
--alter table material alter column contactid uniqueidentifier not null;
--alter table material add RRP decimal null;
--INSERT INTO MaterialCategory 
--values('ccccdddd-dddd-dddd-dddd-ddddccccdddd', 'Corys electrical Ltd', '00000000-0000-0000-0000-000000000000',SYSDATETIME(),1,'ECA7B55C-3971-41DA-8E84-A50DA10DD233');

-----------------------------
--Mandeep has above
-----------------------------



--executed 10:18am 9/3 Jared
--alter table supplierinvoicematerial
--add ContractorID uniqueidentifier not null;
--alter table ontrack3.dbo.supplierinvoicematerial
--add constraint FK_SupplierInvoicematerial_contactID 
--foreign key (ContractorID)
--references Contact(ContactID);
--alter table supplierinvoicematerial
--add Assigned bit not null;
--alter table supplierinvoicematerial
--add Createdby uniqueidentifier not null;
--alter table supplierinvoicematerial
--add Createddate datetime not null;
--alter table supplierinvoicematerial
--add Active int not null;
--alter table supplierinvoicematerial
--alter column qty decimal not null;
--sp_rename 'supplierinvoicematerial.suppliermaterialid', 'SupplierInvoiceMaterialID', 'COLUMN';
--alter table vehicle
--alter column vehiclename nvarchar(20) not null;
--alter table vehicle
--alter column vehicleregistration nvarchar(20) not null;
--insert into Vehicle
--values
--(NEWID(), 'ECA7B55C-3971-41DA-8E84-A50DA10DD233', '53E58C1B-4D58-41F9-9849-FBB5B4F87833', 'Ford Ranger', 'EKM819', SYSDATETIME(), SYSDATETIME(), SYSDATETIME())
--update site set SiteOwnerID='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
--where Address1 like '%eure%'

--executed 10/3 11:01am Jared
--alter table ContainerMaterial
--add taskmaterialID uniqueidentifier null;
--alter table ontrack3.dbo.containermaterial
--add constraint FK_ContainerMaterial_TaskMaterialID 
--foreign key (TaskMaterialID)
--references TaskMaterial(TaskMaterialID);
--alter table ContainerMaterial
--alter column SiteID uniqueidentifier null;
--alter table ContainerMaterial
--alter column VehicleID uniqueidentifier null;
--alter table ContainerMaterial
--drop column Thecontainerisasite
--sp_rename 'ContainerMaterial.taskmaterialid', 'TaskMaterialID', 'COLUMN';

--executed 11/3 12:01pm Jared
--alter table SupplierInvoice
--add TotalExGst decimal not null;
--alter table SupplierInvoice
--alter column TotalExGst decimal(10,2) null;
--alter table material
--alter column costprice decimal(10,2) null;
--alter table material
--alter column sellprice decimal(10,2) null;
--drop table ContainerMaterial
--alter table supplierinvoicematerial
--add TaskMaterialID uniqueidentifier null;
--alter table supplierinvoicematerial
--add SiteID uniqueidentifier null;
--alter table supplierinvoicematerial
--drop constraint FK_SupplierInvoiceMaterial_TaskMaterialID
--alter table supplierinvoicematerial
--add VehicleID uniqueidentifier null;
--alter table contactsite
--drop constraint UQ_ContactSite

--executed 2:20pm 16/3  Jared
--insert into ContactSite (ContactSiteID, ContactID, SiteID, CreatedBy, CreatedDate, Active)
--select NEWID(), Site.ContactID , site.SiteID, '00000000-0000-0000-0000-000000000000', SYSDATETIME(), '1'
--from site

--executed 2:21pm 16/3  Jared
--insert into ContactSite (ContactSiteID, ContactID, SiteID, CreatedBy, CreatedDate, Active)
--select NEWID(), Site.SiteOwnerID , site.siteID, '00000000-0000-0000-0000-000000000000', SYSDATETIME(), '1'
--from site

--executed 2:20pm 16/3  jared
--insert into contactsite (contactsiteid, contactid, siteid, createdby, createddate, active)
--select newid(), 'eca7b55c-3971-41da-8e84-a50da10dd233' , site.siteid, '00000000-0000-0000-0000-000000000000', sysdatetime(), '1'
--from site

--executed by Jared 5:09pm 16/3
--alter table contactsite
--add flag int null

--update contactsite set flag = 1 

--select ContactID, siteid, count(*) as blaha
--into #TempTable2
--from ContactSite
--group by ContactID, siteid

--insert into ContactSite
--(ContactSiteID, ContactID, SiteID, CreatedBy, CreatedDate, Active, flag)
--select newid(), #TempTable2.ContactID, #TempTable2.SiteID, '00000000-0000-0000-0000-000000000000', SYSDATETIME(), 1, 2 
--from #TempTable2

--update Sitenew  set Active=0
--from ContactSite Sitenew
--inner join ContactSite Siteold
--on SiteOld.siteid = SiteNew.siteid and SiteOld.ContactID = SiteNew.ContactID and siteold.Active=0

--delete  from ContactSite where flag != 2

-----------------------------
--Mandeep has above
---------------------------------


--executed 18/3 2:31pm Jared
--update site set Address4 = 'Canterbury' where Address2 != 'blenheim'
--update site set Address4 = 'Marlborough' where Address2 = 'blenheim'
--update site set address2 = '' where Address2='christchurch'
--update site set Address3 = 'Christchurch'
--update site set address3 = District.DistrictName from 
--Suburb, District, Site
--where Address2 = Suburb.SuburbName and Suburb.DistrictID =District.DistrictID
--update site set Address3 = 'Blenheim' where Address2='blenheim'


--Alter table contact add Searchable int;
--alter table contacttradecategory add TradeCategoryID uniqueidentifier
--alter table contacttradecategory drop constraint U_ContactIDTradeCategoryID
--alter table contacttradecategory add constraint U_ContactID_TradeCategoryID_SubTradeCategoryID unique (contactid,tradecategoryid,subtradecategoryid);
--alter table contacttradecategory drop constraint FK_ContactTradeCategory_SubTradeCategory;



-----------------------------
--Mandeep has above     18/3
---------------------------------

--NOT SURE WHY I DID THIS. DO NOT EXECUTE ON LIVE. executed local only 18/3 jared 4pm 
--alter table contactcustomer
--drop constraint fk_contactcustomer_customer
--alter table contactcustomer
--drop constraint uq_contactcustomer
--alter table contactcustomer 
--add ISCompany bit not null default 0 with values

--Below is for JOBCONTRACTOR, logic will be similar to sitecontact
--executed on LIVE DB by jared 4/4 3.34pm                                      LIVE



--alter table supplierinvoicematerial
--add OldSupplierInvoiceMaterialID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000'
--alter table jobcontractor
--add SiteID uniqueidentifier not null 
--alter table jobcontractor
--add constraint fk_JobContractor_Site
--foreign key (SiteID)
--References Site (SiteID)
--alter table jobcontractor
--drop constraint uq_jobcontractor



--EXECUTED LOCAL ONLY
--delete JobContractor
--insert into JobContractor
--select NEWID(), job.jobid, ContactSite.ContactID, '00000000-0000-0000-0000-000000000000', GETDATE(), 1, 0
-- from site, job, ContactSite where site.siteid=job.SiteID and contactsite.siteid=site.siteid
--  create table tempo(a uniqueidentifier, b uniqueidentifier)
-- insert into tempo
--SELECT distinct jobid, contactid FROM JOBCONTRACTOR
--delete JobContractor 
--insert into JobContractor
--select NEWID(), tempo.a, tempo.b, '00000000-0000-0000-0000-000000000000', GETDATE(), 1, 0
-- from tempo
-- drop table tempo
--ALTER TABLE jOBcONTRACTOR ADd constraint uq_jobcontractor UNIQUE (contactid,jobid)


--executed on LIVE and local jared 5/4/16                            LOCAL AND LIVE
--alter table supplierinvoicematerial
--add QtyRemainingToAssign decimal null

--executed on local 11/4 jared                                       LOCAL
--alter table contact
--add ISCompany bit not null default 0 with values
--update contact set ISCompany=1 where CompanyName != ''



---------------------------------------
--MERGE CONTACT WITH CUSTOMER.
---------------------------------------

--executed local and live jared 11/4
--alter table contactcustomer
--add ContractorID uniqueidentifier null
--alter table contactcustomer
--add CustomerIDfromContactTable uniqueidentifier null
---------------------------------------------------------------------
--mandeep you will need to run this before you make any tests
----------------------------------------------------------------------
--update ContactCustomer set CustomerIDfromContactTable=contact.ContactID from customer,contact,ContactCustomer where ContactCustomer.customerid = customer.CustomerID and contact.ContactID=customer.ContactID
--update contactcustomer set contractorid=ContactID
-----------------------------------------------------------------------

--executed live & local 12/4 jared
--alter table contactcustomer
--add Status int not null default 0
--alter table Jobcontractor
--add Status int not null default 0











--POPULATE JOBCONTRACTOR TABLE WITH JOBID ONLY.
--insert into JobContractor 
--(JobContractorID, JobID, ContactID, CreatedBy, CreatedDate, Active, Status)
--select distinct NEWID(), JobID, '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', getdate(),1,0
--from job, site where job.siteid=site.SiteID


--alter table taskmaterial
--add FromInvoice bit default 0
--with values




select * from task where Description like '%hp outdoor%'
select * from TaskMaterial where taskid='48C0380A-C3C2-454D-87A4-805A711A3A26'
select * from SupplierInvoiceMaterial

select * from ContactCustomer
update ContactCustomer set Status=4 where CustomerID like '%32%'

update customer set active=1 from ContactCustomer, customer where customer.CustomerID = ContactCustomer.CustomerID
select * from customer where active = 1

SELECT * from contact, customer where customer.ContactID=Contact.ContactID

select customer.CustomerID, contact.ContactID from Customer, contact where customer.ContactID=Contact.ContactID









select site.address1 from Site
select * from ContactCustomer

select * from JobContractor

select * from site, ContactSite, ContactCustomer where ContactCustomer.CustomerIDfromContactTable=contactsite.ContactID and site.siteid=contactsite.SiteID

select * from ContactSite
select * from Task
select * from job


select * from contact where ContactID='E19E731F-AEC8-47AB-9712-96F0BBA91B9E'
select * from site where CustomerFirstName like '%rangiora%'
update contact set address1='High St' where contactid='5661BC5A-A588-4CCC-A3A1-951DB3A38350'
 select * from Customer
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 select * from contact where ContactID='859a8bad-c879-47ad-8722-aaa96979e308'

 select * from ContactCustomer where CustomerID='5661BC5A-A588-4CCC-A3A1-951DB3A38350'
 select contact.firstname, contact.CompanyName from contact, ContactCustomer where ContactCustomer.CustomerID=contact.ContactID

 select * from site where Address1 like '%920%'  --5661BC5A-A588-4CCC-A3A1-951DB3A38350

select * from material where createdby='00000000-0000-0000-0000-000000000000' order by MaterialName


select * from ContactCustomer

select vehicle.VehicleName, contact.FirstName, contact.LastName from vehicle, Contact where VehicleDriver='53E58C1B-4D58-41F9-9849-FBB5B4F87833' and contact.ContactID='53E58C1B-4D58-41F9-9849-FBB5B4F87833'
select * from Vehicle
select VehicleDriver from Vehicle where driverId='53E58C1B-4D58-41F9-9849-FBB5B4F87833'
















select distinct vehicle.vehicleid as vehicle, Material.MaterialName, Material.CostPrice, Material.SellPrice, Material.RRP, SupplierInvoice.SupplierInvoiceID, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, 
                    SupplierInvoice.SupplierReference, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate, SupplierInvoice.Active, SupplierInvoice.ContractorReference, SupplierInvoice.TotalExGst
                    from SupplierInvoiceMaterial, Material, Vehicle, SupplierInvoice
                    where 
                    SupplierInvoiceMaterial.VehicleID='08F88AD1-7960-4F57-A899-94BF89792EBD' 
					and SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID
                    and supplierinvoice.contractorid='ECA7B55C-3971-41DA-8E84-A50DA10DD233' 
					and Vehicle.VehicleOwner='ECA7B55C-3971-41DA-8E84-A50DA10DD233' 
					and Vehicle.VehicleDriver='53E58C1B-4D58-41F9-9849-FBB5B4F87833'
					and SupplierInvoiceMaterial.MaterialID=Material.MaterialID
                    order by SupplierInvoice.ContractorReference




					select NEWID() as tempid, vehicle.vehicleid as vehicle, Material.MaterialName, Material.CostPrice, Material.SellPrice, Material.RRP, supplier.suppliername,
                    material.uom, material.materialid, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate, SupplierInvoice.Active
                    from SupplierInvoiceMaterial, Material, Vehicle, SupplierInvoice, Supplier
                    where 
					
                    SupplierInvoiceMaterial.VehicleID='08F88AD1-7960-4F57-A899-94BF89792EBD' 
                    and supplier.supplierid=supplierinvoice.supplierid
					and SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID
                    and supplierinvoice.contractorid='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
					and Vehicle.VehicleOwner='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
					and Vehicle.VehicleDriver='53E58C1B-4D58-41F9-9849-FBB5B4F87833'
					and SupplierInvoiceMaterial.MaterialID=Material.MaterialID
                    order by material.MaterialName
					


select * from SupplierInvoiceMaterial




update SupplierInvoiceMaterial set VehicleID='08F88AD1-7960-4F57-A899-94BF89792EBD' where VehicleID='74EC3DCC-0527-477F-B9D4-C3AF967A7B15'

select * from Material where MaterialID='3665e5ab-8ad0-4fc6-94ec-0b2dd226c66e'

select * from TaskMaterial where TaskID='48C0380A-C3C2-454D-87A4-805A711A3A26'

delete from SupplierInvoiceMaterial
delete from supplierinvoice
delete from taskmaterial where description like 'from wholesaler%'
delete from material where MaterialCategoryID='CCCCDDDD-DDDD-DDDD-DDDD-DDDDCCCCDDDD'

select * from material order by createddate

select * from SupplierInvoice



select * from site where Address1 like '%621%'
update site set siteownerid='01F64CD4-DF65-4A56-8285-D44CF49A52F3' where siteid='F86AB2D7-17EC-40F1-B057-D85EC7F0E187'
select * from contact where lastname like '%cham%'

--select * from site where Address1 like 

--F86AB2D7-17EC-40F1-B057-D85EC7F0E187 sandras site id.    contactid= 01F64CD4-DF65-4A56-8285-D44CF49A52F3


select * from job

--select * from SupplierInvoiceMaterial

select * from Task where TaskID='48C0380A-C3C2-454D-87A4-805A711A3A26'
select * from TaskMaterial where TaskID='48C0380A-C3C2-454D-87A4-805A711A3A26'

select * from Material

select * from Task where taskname like '%hp out%'

select * from Contact, Customer, ContactCustomer
where Customer.CustomerID=ContactCustomer.CustomerID and Customer.ContactID=Contact.ContactID and Customer.Address1 = contact.address1 and Customer.address2 = Contact.Address2 and customer.CompanyName =  Contact.CompanyName and Customer.Email = contact.email and Contact.FirstName = Customer.FirstName and Customer.LastName = Contact.LastName and Customer.Phone = Contact.Phone


select * from ContactCustomer



Begin transaction
update ContactCustomer set CustomerID=Contact.ContactID, ISCompany=1 from Contact, Customer, ContactCustomer
where Customer.CustomerID=ContactCustomer.CustomerID and Customer.ContactID=Contact.ContactID and Customer.Address1 = contact.address1 and Customer.address2 = Contact.Address2 and customer.CompanyName =  Contact.CompanyName and Customer.Email = contact.email and Contact.FirstName = Customer.FirstName and Customer.LastName = Contact.LastName and Customer.Phone = Contact.Phone
rollback




Begin transaction
update ContactCustomer set CustomerID=Contact.ContactID from Contact, Customer
where customer.ContactID=Contact.ContactID and Customer.Address1 = contact.address1 and Customer.address2 = Contact.Address2 and customer.CompanyName =  Contact.CompanyName and Customer.Email = contact.email and Contact.FirstName = Customer.FirstName and Customer.LastName = Contact.LastName and Customer.Phone = Contact.Phone
rollback

select * from ContactCompany
select * from ContactCustomer



select contact.ContactID, Customer.ContactID, ContactCustomer.CustomerID from Contact, Customer, ContactCustomer
where customer.ContactID=Contact.ContactID and Customer.Address1 = contact.address1 and Customer.address2 = Contact.Address2 and customer.CompanyName =  Contact.CompanyName and Customer.Email = contact.email and Contact.FirstName = Customer.FirstName and Customer.LastName = Contact.LastName and Customer.Phone = Contact.Phone
and ContactCustomer.ContactID=Contact.ContactID

select Contact.ContactID from contact,ContactCustomer where ContactCustomer.ContactID= Contact.ContactID


select Customer.Address1, contact.address1, Customer.address2, Contact.Address2, customer.CompanyName, Contact.CompanyName, Customer.Email, Contact.FirstName, Customer.FirstName, Customer.LastName, Contact.LastName, Customer.Phone, Contact.Phone from Customer, Contact 
where customer.ContactID=Contact.ContactID and Customer.Address1 = contact.address1 and Customer.address2 = Contact.Address2 and customer.CompanyName =  Contact.CompanyName and Customer.Email = contact.email and Contact.FirstName = Customer.FirstName and Customer.LastName = Contact.LastName and Customer.Phone = Contact.Phone

select * from Contact where ContactID='7E439A20-A9B3-4200-8C96-99F827F08FA5'














select Address1,Address2,Address3,Address4 from site where Address4 != 'Canterbury'

select * from suburb, District where District.DistrictName = 'blenheim' and Suburb.DistrictID=District.DistrictID

select * from suburb where DistrictID='FB4A09D2-28DB-42A5-BED0-D50282896B2E'






select * from ContactSite



update contactsite set flag = 1 



UPDATE ContactSite SET flag = 1 SELECT DISTINCT ContactSite.ContactID, ContactSite.SiteID FROM ContactSite


insert into contactsiteTemp (CSid)



select * from #temptable2

UPDATE ContactSite SET flag = 1 SELECT DISTINCT ContactSite.ContactID, ContactSite.SiteID FROM ContactSite

update contactsite set flag=0

select distinct contactid, SiteID, flag from Contactsite

select * from ContactCustomer, contactsite
where ContactCustomer.ContactID = contactsite.ContactID
--0002945966
--0002942933


--issues below
--1.suppliercode not having 0000 in front
--2. dupicate materials
--3. not displaying correct line items


--NEED TO CREATE QUERY WITH TASKID and QUANTITY KNOWN TO ADD MATERIALS TO TASKMATERIALS TABLE FROM SUPPLIERINVOICEMATERIAL TABLE.


--cd24b023-4b8a-4d99-be6a-0144bff28ccd  supplierinvoiceid
--CA122D89-F7F3-46C7-A525-346832E46076 vehicleid
--taskmaterialid 5ADFD9A2-F997-4811-9A9A-0074C9567CF2


select * from customer where Email like '%456%'

select distinct S1.SiteID, S1.ContactID, S2.ContactID
from ContactSite S1
inner join ContactSite S2
on s1.siteid = S2.siteid
where s1.ContactID='C9A7CEBF-C61A-4386-947F-004EAB5E3A82' and s2.ContactID='9A032F5C-F4E9-4AE8-B58B-017B9D8AF2C4'

select distinct S1.SiteID, S1.ContactID, S2.ContactID, s.Address1, s.Address2
from ContactSite S1
inner join ContactSite S2
on S1.SiteID = S2.SiteID
inner join Site s
on s1.siteid = s.siteid
where s1.ContactID='C9A7CEBF-C61A-4386-947F-004EAB5E3A82' and s2.ContactID='9A032F5C-F4E9-4AE8-B58B-017B9D8AF2C4'

select distinct s.SiteID from Site s, ContactSite cs, Contact c where cs.SiteID=s.SiteID and 
c.ContactID=cs.ContactID and cs.SiteID in (select SiteID from Site where SiteOwnerID='ECA7B55C-3971-41DA-8E84-A50DA10DD233' 
or ContactID='B813B3AE-28A7-4A23-BBE4-69E05C8B3B69')

select * from Site

--57C930AE-581E-43C5-9B31-07ED2C46A0E7
--3F54FEE0-8CE7-440F-8DB1-6D1DC5535E17
--E8F9DAC5-7BEA-4CD9-A8AC-859F08E867BE
--FCA5E11D-5733-4337-95BF-981EE611056C
--C6E171DD-3725-4C8E-B818-A9066941953D

select * from Contact where ContactID='ECA7B55C-3971-41DA-8E84-A50DA10DD233'


select * from TaskMaterial


update SupplierInvoiceMaterial set VehicleID='CA122D89-F7F3-46C7-A525-346832E46076' where SupplierInvoiceID='cd24b023-4b8a-4d99-be6a-0144bff28ccd'


select * from SupplierInvoiceMaterial where SupplierReference='0002942933'
select * from Material where SupplierProductCode  like '%966239%'
SELECT * from taskmaterial

select * from SupplierInvoiceMaterial
order by SupplierInvoiceID

DELETE from Material where SupplierProductCode != '2'
delete from SupplierInvoiceMaterial
delete from SUPPLIERINVOICE


--4CF05FF8-8003-4D1E-9F43-647862B8ADD4
--C9A7CEBF-C61A-4386-947F-004EAB5E3A82


update contactsite set siteid='4CF05FF8-8003-4D1E-9F43-647862B8ADD4' where contactid='C9A7CEBF-C61A-4386-947F-004EAB5E3A82'




--fix this one
select distinct S1.SiteID, S1.ContactID, S2.ContactID, s.Address1
from ContactSite S1
inner join ContactSite S2
on S1.SiteID = S2.SiteID
inner join Site s
on s1.siteid = s.siteid
where s1.ContactID='C9A7CEBF-C61A-4386-947F-004EAB5E3A82' and s2.ContactID='9A032F5C-F4E9-4AE8-B58B-017B9D8AF2C4'



select S.Address1
from ContactSite S1, Site S
inner join ContactSite S2
on S1.SiteID = S2.SiteID
where s1.ContactID='C9A7CEBF-C61A-4386-947F-004EAB5E3A82' and s2.ContactID='9A032F5C-F4E9-4AE8-B58B-017B9D8AF2C4' and S.SiteID=s1.SiteID



select * from contactsite where contactid='C9A7CEBF-C61A-4386-947F-004EAB5E3A82' or contactid='9A032F5C-F4E9-4AE8-B58B-017B9D8AF2C4'

select * from SupplierInvoiceMaterial
select * from contactsite
order by ContactID



select Distinct SupplierInvoice.SupplierInvoiceID, Supplier.SupplierName, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, 
SupplierInvoice.SupplierReference, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate, SupplierInvoice.Active, SupplierInvoice.ContractorReference, 
SupplierInvoice.TotalExGst
from Supplier, SupplierInvoiceMaterial, SupplierInvoice 
where SupplierInvoiceMaterial.Assigned='0' and SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID and                   
Supplier.SupplierID='BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB' and                   
supplierinvoice.contractorid='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
order by SupplierInvoice.ContractorReference




select * from SupplierInvoice

select distinct SupplierInvoice.SupplierInvoiceID, Supplier.SupplierName, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, 
                    SupplierInvoice.SupplierReference, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate, SupplierInvoice.Active, SupplierInvoice.ContractorReference, SupplierInvoice.TotalExGst
                    from Supplier, SupplierInvoiceMaterial, SupplierInvoice 
                    where 
                    SupplierInvoiceMaterial.Assigned=0 and 
                    SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID and
                    Supplier.SupplierID='BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB' and
                    supplierinvoice.contractorid='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
                    order by SupplierInvoice.ContractorReference

select * from site where SiteOwnerID='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
select * from contact where ContactID='53E58C1B-4D58-41F9-9849-FBB5B4F87833'






select distinct Supplier.SupplierName, SupplierInvoice.InvoiceDate, SupplierInvoice.ContractorReference, SupplierInvoice.SupplierReference
from Supplier, SupplierInvoiceMaterial, SupplierInvoice 
where 
SupplierInvoiceMaterial.Assigned=0 and 
SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID and
Supplier.SupplierID='BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB' and
supplierinvoice.contractorid='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
order by SupplierReference



select * from Material where SupplierProductCode != '2'
select * from Material where SupplierProductCode = '2' order by Description
select * from SupplierInvoice
update Material set SupplierProductCode = '2'
delete from material where uom='ea'
delete from SUPPLIERINVOICE

select * from Vehicle

select * from Contact where FirstName = 'jared'

select * from SupplierInvoice
SELECT * from SupplierInvoiceMaterial order by MaterialID

select * from containermaterial;

select * from Material

SELECT * from ContactSite where siteid='F8DC96DF-6F06-4ABD-8A78-216285181924'
select * from contact where ContactID like '4D172D77-693A-416C-BCBA-6DC9336918E8'
select * from site where SiteID='F8DC96DF-6F06-4ABD-8A78-216285181924'
select * from site


select * from job

select site.Address1, contact.CompanyName, Contact.FirstName, Contact.LastName from site, contact
where site.ContactID = contact.ContactID


update task set SiteID = job.SiteID where task.JobID = job.JobID
 





--------------------------------------------------------
--                     JOB OWNERSHIP
--- The job owner is the contact.contactid that you create the job for. Might also be siteowner





--------------------------------------------
--          Task needs CustomerID
--every task has a customer
--if you create a task for someone then you are the task.taskcustomerid
--if you create your own task then you choose the task.taskcustomerid from either the
--------------------------------------------



----------------------------------------------
---Queries to load your info
---assumning that contact.contactID for logged in person is 'ECA7B55C-3971-41DA-8E84-A50DA10DD233'
------------------------------------------------
select contact.CompanyName, contact.FirstName, Contact.LastName from Contact, Task 
where task.ContractorID='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
--and contact.ContactID=TASK.taskcustomerID

SELECT	 * FROM SupplierInvoice
DELETE from SupplierInvoice
select * from Material


