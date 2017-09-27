alter table contractorcustomer drop column contactid
alter table contractorcustomer drop constraint UQ_ContactCustomer
alter table contractorcustomer drop constraint FK_ContactCustomer_Contact
alter table contractorcustomer add constraint FK_ContractorID foreign key (contractorid) references contact(contactid)
alter table contractorcustomer drop column customerid
---Confirm with Jared before deleting---
alter table site drop column intcount

----10th June,2016 Contractor Customer table Mandeep--------------
alter table contractorcustomer add FirstName nvarchar(128)
  
  alter table contractorcustomer add LastName nvarchar(128)
 
  alter table contractorcustomer add CompanyName nvarchar(128)

   
  alter table contractorcustomer add Phone nvarchar(128)

    alter table contractorcustomer add Address1 nvarchar(128)
	  alter table contractorcustomer add Address2 nvarchar(128)
	    alter table contractorcustomer add Address3 nvarchar(128)
		  alter table contractorcustomer add Address4 nvarchar(128)

 update ContractorCustomer 
set ContractorCustomer.FirstName=c.FirstName,ContractorCustomer.LastName=c.LastName,
ContractorCustomer.CompanyName=c.CompanyName,ContractorCustomer.Phone=c.Phone,
ContractorCustomer.Address1=c.Address1,ContractorCustomer.Address2=c.Address2,
ContractorCustomer.Address3=c.Address3,ContractorCustomer.Address4=c.Address4
from ContractorCustomer cc
inner join Contact c
on cc.ContactIDCustomer=c.ContactID
sp_rename 'contractorcustomer.contactidcustomer', 'CustomerId','column';
alter table contractorcustomer add Linked int default 0
  alter table contractorcustomer add foreign key (customerid) references contact(contactid)
    create nonclustered index ix_contractorcustomer_customerid on contractorcustomer(customerid)
	alter table contractorcustomer drop constraint uq_Contractorid_contactidcustomer
	WITH cc AS(
   SELECT [contractorid],customerid,
       RN = ROW_NUMBER()OVER(PARTITION BY contractorid, customerid ORDER BY contractorid)
   FROM ContractorCustomer
)
delete  FROM cc WHERE RN > 1

alter table contractorcustomer add constraint uq_contractorid_customerid unique(contractorid,customerid)
update ContractorCustomer set Linked=0 where Linked is null


  select * from ContractorCustomer where ContractorID=customerid

  delete from ContractorCustomer where ContractorID=customerid

    alter table [ContactTradeCategory] add DefaultTradeCategory bit default 0


	--Linkedcustomers stored procedure- create query and execute on live 


	  WITH cc AS(
   SELECT *,
       RN = ROW_NUMBER()OVER(PARTITION BY jobid, contactid ORDER by jobid) 
   FROM jobcontractor
)
delete FROM cc WHERE RN > 1

alter table jobcontractor add constraint uq_jobid_contactid unique(jobid,contactid)
----------------10th June,2016 Jared--------
	  alter table supplierinvoicematerial
alter column active bit

  alter table taskmaterial
add FromVehicle bit default 0 not null


----------------3rd August,2016----------------
alter table contractorcustomer add Deleted bit 

-----------------21st August,2016---------------
-----------------Not sure-----------------------
  select * from Contact where Address3='Christchurch'
  update Contact set Address3='Christchurch City' where Address3='Christchurch'


  -----------task file------------
  CREATE TABLE [dbo].[TaskFile] (
    [TaskFileID]   UNIQUEIDENTIFIER NOT NULL,
    [TaskID]       UNIQUEIDENTIFIER NOT NULL,
    [FileID]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate] DATETIME         NOT NULL,
    [Active]      BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([TaskFileID] ASC),
    CONSTRAINT [FK_taskFile_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Contact] ([ContactID]),
    CONSTRAINT [FK_taskFile_File] FOREIGN KEY ([FileID]) REFERENCES [dbo].[FileUpload] ([FileID]),
    CONSTRAINT [FK_taskFile_Job] FOREIGN KEY ([TaskID]) REFERENCES [dbo].[Task] ([TaskID])
);


select * from fileupload where CompanyID='ECA7B55C-3971-41DA-8E84-A50DA10DD233'

---------------12th September,2016-----------------
--Dropping constraint to allow individuals(same contact id-same company id) relationship 
ALTER TABLE [dbo].[ContactCompany] DROP CONSTRAINT [UQ_ContactCompany]

--9th Oct, to identify the type of customer.
alter table ContractorCustomer add CustomerType int 
select * from ContractorCustomer where CompanyName=''
update ContractorCustomer set customertype=0 where CompanyName!=''
update ContractorCustomer set customertype=1 where CompanyName!=''