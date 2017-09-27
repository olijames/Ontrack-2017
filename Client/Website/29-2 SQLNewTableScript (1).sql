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


--executed 10:54pm 8/3 Jared
--alter table material
--add ContactID uniqueidentifier null;
--update material set ContactID ='ECA7B55C-3971-41DA-8E84-A50DA10DD233';
--alter table material alter column contactid uniqueidentifier not null;
--alter table material add RRP decimal null;
--INSERT INTO MaterialCategory 
--values('ccccdddd-dddd-dddd-dddd-ddddccccdddd', 'Corys electrical Ltd', '00000000-0000-0000-0000-000000000000',SYSDATETIME(),1,'ECA7B55C-3971-41DA-8E84-A50DA10DD233');



select * from Material where SupplierProductCode != '2'
select * from SupplierInvoice
update Material set SupplierProductCode = '2'
delete from material where uom='ea'
delete from SUPPLIERINVOICE

select * from containermaterial;

select * from task

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


