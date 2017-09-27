--this file has been executed to live db 17.5.17

--need to have the siteowner fk with contractorcustomer instead of contact
--1. insert all the siteowners as customers of EC into the contractorcustomer table
insert into ContractorCustomer (ContactCustomerID,CreatedBy,CreatedDate,Active,ContractorID,Status,CustomerId,FirstName,LastName,CompanyName,Phone,Address1,Address2,Address3,Address4, Linked,Deleted,CustomerType)
select distinct newid(),'00000000-0000-0000-0000-000000000000', getdate(), 1, 'ECA7B55C-3971-41DA-8E84-A50DA10DD233', 0, site.SiteOwnerID, site.CustomerFirstName, site.CustomerLastName, '', site.CustomerPhone, site.CustomerAddress1, site.CustomerAddress2, site.Address3, site.Address4, '0','0','1'
from (select site.*,
             row_number() over (partition by siteownerid order by siteownerid) as seqnum
      from site
     ) site
where seqnum = 1 and
site.SiteOwnerID not in (
select distinct ContractorCustomer.CustomerId 
from ContractorCustomer)
--2. remove FK from site, copy across contractorcustomerids to the site.siteowner column, reinstate new fk_contractorcustomer_siteowner
update site set site.SiteOwnerID=ContractorCustomer.ContactCustomerID from site,ContractorCustomer where ContractorCustomer.CustomerID=site.SiteOwnerID
--3 add fk for site
alter table site
add constraint FK_SiteOwner_ContractorCustomer
foreign key (siteownerid) references contractorcustomer(contactcustomerid)
--4 add columns email and pendingsiteowner to contractorcustomer
alter table contractorcustomer
add PendingSiteOwner bit default 0,
email nvarchar(50) default ''

update ContractorCustomer set deleted = 0 where deleted !=1 or Deleted IS NULL
update cc set cc.firstname=c.firstname, cc.LastName=c.LastName, cc.CompanyName=c.CompanyName from contractorcustomer cc, contact c where cc.customerid=c.contactid

--5 admin stuff. probably tony doesnt have
alter table ContactCompany 
add Settings integer default 0
update ContactCompany set settings=0


alter table ContractorCustomer 
add CreatorContractorCustomer uniqueidentifier
update ContractorCustomer set CreatorContractorCustomer = ContactCustomerID
alter table contractorcustomer
alter column creatorcontractorcustomer uniqueidentifier not null
update ContractorCustomer set CustomerType =0 where ContractorID like 'eca%' and CustomerId like 'eca%'
alter table jobcontractor 
add JobNumberAuto bigint
update JobContractor set jobcontractor.JobNumberAuto = job.JobNumberAuto from job, JobContractor where job.jobid=JobContractor.JobID


--add contractor items
create table MyContractorsTradeCategory(
ContractorCustomerTradeCategoryID uniqueidentifier not null primary key,
ContractorCustomerID uniqueidentifier not null foreign key references contractorcustomer(contactcustomerid),
TradeCategoryID uniqueidentifier not null foreign key references tradecategory(tradecategoryID),
CreatedDate date not null,
createdby uniqueidentifier not null,
Active bit not null)




--junk below
select * from contact where companyName like 'cube%'
select * from job where name like 'cabin%'
update ContactCompany set settings =1420 where companyid='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
select * from contact where email like 'nick@%'

INSERT Invoice ([InvoiceID],[ContractorID],[CustomerID],[DueDate],[InvoiceStatus],[Terms],
[SubmissionDetailLevel],[InvoiceDescription],[TaskID],[SentDate],[CreatedBy],[CreatedDate],[Active]) 

VALUES ('c84d8cd0-0715-4586-930e-ef68e432b950','eca7b55c-3971-41da-8e84-a50da10dd233','52b3bf55-65c3-44d1-bf83-d3e34187ae06','2017-05-17 00:00:00.000',0,'My Terms',
0,'My Description','8d0e0eba-b612-4059-a05b-2ad6c42eb3c7','1900-01-01 00:00:00.000','53e58c1b-4d58-41f9-9849-fbb5b4f87833','2017-05-17 17:58:28.881',1)

select * from invoice