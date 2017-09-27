--executed local and live 23/5/17
alter table invoice
drop constraint invoice_customer
update Tasklabour set InvoiceQuantity=EndMinute / cast(60 as decimal(9,2)) where InvoiceQuantity=0 
update TaskMaterial set InvoiceQuantity=Quantity where InvoiceQuantity=0 
ALTER TABLE jobcontractor   
ADD CONSTRAINT uniqueJNAandContact UNIQUE (JobNumberAuto, ContactID);  
update Contact set JobNumberAuto=1600 where ContactID='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
 
 --junk below
 select taskid, JobID, QuoteID, InvoiceNumber, ContractorID, ParentTaskID, TaskName, TaskType, Description, CreatedBy, CreatedDate, Active, TaskOwner      , Status, StartDate, StartMinute, EndDate, EndMinute, Appointment, AmendedTaskID, LMVisibility, InvoiceToType, AmendedBy      , AmendedDate, TradeCategoryID, TaskNumber, TaskInvoiceStatus, TaskCustomerID, SiteID from task where TaskCustomerID = '4fb86767-b494-42c5-9f9f-42403f0278da' and status!=2 ORDER BY CreatedDate

 select * from ContactCompany where CompanyID='15CB8BCE-D0D8-45A0-8C37-BEE173EF5258'
 select * from Contact where ContactID='7A3AF9B9-7655-48EB-8B5A-2889D11BDB92'
 --Belair
 --cid 15CB8BCE-D0D8-45A0-8C37-BEE173EF5258
 --ccid D901F60C-9A09-4B11-89D6-FE7FEFDB29A7
 select * from contact where lastName like 'zhao%'
 select * from ContractorCustomer where  customerid='15CB8BCE-D0D8-45A0-8C37-BEE173EF5258' and  contractorid='15CB8BCE-D0D8-45A0-8C37-BEE173EF5258'

 
 --metro
 --cid 4C2E0BB9-2CE3-46D8-8697-FD8791A2CCCC
 --ccid F4F76DB0-F9A7-4856-9C75-65DC94E56F08
 select * from contact where companyname like 'metro%'
 select * from ContractorCustomer where  customerid='4C2E0BB9-2CE3-46D8-8697-FD8791A2CCCC' and  contractorid='4C2E0BB9-2CE3-46D8-8697-FD8791A2CCCC'



 select * from Contact where ContactID='CEF15CB0-9BF4-4C75-93FE-28A90F1FDEA9'
 select * from ContactTradeCategory
 select * from ContractorCustomer

 exec sp_columns  contact
 select * from Contact where Contactid='53E58C1B-4D58-41F9-9849-FBB5B4F87833'
 














 select * from job where JobNumberAuto=1312
 select * from task where JobID='FE436322-214E-4B28-A9E8-4FFC9190AD3E'
 select * from TaskMaterial where TaskID='697C6A5C-D817-4655-8F7D-4542DC945209'


 select * from ContractorCustomer where  customerid='7103733B-A16B-4414-BA2E-27A468C84F6D' and  contractorid='7103733B-A16B-4414-BA2E-27A468C84F6D'
 select * from ContactCompany
 select * from contact where lastname='kelly'
 select * from contact where contactid='7103733B-A16B-4414-BA2E-27A468C84F6D'
 select * from EmployeeInfo
 select * from ContactSite


 select * from contractorcustomer, Task, Site where 
 ContactCustomerID ='5f58b302-4ce5-42cb-9d99-0ab7600cd568'  and TaskCustomerID ='5f58b302-4ce5-42cb-9d99-0ab7600cd568' and task.siteid=site.siteid or
 ContactCustomerID ='90f632b0-b1d6-41d5-864c-565d0aa4d579' and TaskCustomerID ='90f632b0-b1d6-41d5-864c-565d0aa4d579' and task.siteid=site.siteid
 order by ContactCustomerID
 or ContactCustomerID ='f5fedeea-cc11-40f4-86d2-87e75c01b617'
 or ContactCustomerID ='fb50bd09-be57-4c18-830c-ba88c1fbd34c'
 
 --
 select * from Contact where ContactID='B1B80954-77AE-4457-85AB-78AA564085DE'
 select * from TaskMaterial where taskid='058A4C9B-C149-4EC1-A88B-F27ABAB35E19' order by createddate desc
 select * from task where taskname like 'trail%'
  
  SELECT  SUM(Total) Total, ContactID, ContactType, FirstName, LastName, CompanyName, Email, 
  Address1, Address2, Address3, Address4, 
  Phone, CreatedBy, CreatedDate, Active                     
  FROM(
  SELECT t.TaskCustomerID,tl.CustomerID, tl.InvoiceQuantity*65 Total, c.ContactID, c.ContactType, c.FirstName, c.LastName, c.CompanyName, c.Email,  
         c.Address1, c.Address2, c.Address3, c.Address4, c.Phone, c.CreatedBy, c.CreatedDate, c.Active                           
  FROM TaskLabour tl, Contact c, Task t, Job j                          
  WHERE 
  tl.ContractorID = 'eca7b55c-3971-41da-8e84-a50da10dd233' AND 
  t.JobID = j.JobID AND 
  tl.TaskID = t.TaskID AND 
  t.taskCustomerID = c.ContactID AND 
  t.Status != 2  AND 
  tl.InvoiceID = '00000000-0000-0000-0000-000000000000' AND 
  tl.Active = 1                        
  UNION ALL                         
  SELECT t.TaskCustomerID, tm.CustomerID, tm.InvoiceQuantity* tm.SellPrice Total, c.ContactID, c.ContactType, c.FirstName, c.LastName, c.CompanyName, 
  c.Email, c.Address1, c.Address2, c.Address3, c.Address4, c.Phone,  c.CreatedBy, c.CreatedDate, c.Active                           
  FROM TaskMaterial tm, Contact c, Task t,Job j                          
  WHERE 
  t.ContractorID = 'eca7b55c-3971-41da-8e84-a50da10dd233' AND 
  t.JobID = j.JobID AND 
  tm.TaskID = t.TaskID AND 
  t.TaskCustomerID = c.ContactID AND 
  t.Status != 2                            AND 
  tm.InvoiceID = '00000000-0000-0000-0000-000000000000' AND 
  tm.Active = 1) t                       
  GROUP BY t.taskCustomerID, ContactID, ContactType, FirstName, LastName, CompanyName, Email, Address1, Address2, Address3, Address4, Phone, 
  CreatedBy, CreatedDate, Active                       
  ORDER BY FirstName, CompanyName




 select * from Contact where contactid='ECA7B55C-3971-41DA-8E84-A50DA10DD233'
 select * from contractorcustomer where contractorid='e60f7a91-84a5-48b3-9a32-d675b21715d0' and customerid='e60f7a91-84a5-48b3-9a32-d675b21715d0'

INSERT ContractorCustomer 
([ContactCustomerID],[ContractorID],[CustomerID],[FirstName],[LastName],[Phone],
[Address1],[Address2],[Address3],[Address4],
[CompanyName],[Linked],[Deleted],[CustomerType],[PendingSiteOwner],[Email],[CreatorContractorCustomer],[CreatedBy],[CreatedDate],[Active]) 
VALUES ('5c102def-5f0d-4cda-93dd-a3e18bc84e80','e60f7a91-84a5-48b3-9a32-d675b21715d0','e60f7a91-84a5-48b3-9a32-d675b21715d0','1','1','1',
'1','1','Christchurch','Canterbury',
'',4,0,1,0,'','5c102def-5f0d-4cda-93dd-a3e18bc84e80','53e58c1b-4d58-41f9-9849-fbb5b4f87833','2017-06-06 20:37:29.261',1)


 declare @i int  = 0
 update JobContractor
set JobNumberAuto  = @i , @i = @i - 1
where contactid!='ECA7B55C-3971-41DA-8E84-A50DA10DD233'


 select * from jobcontractor where contactid='eca7b55c-3971-41da-8e84-a50da10dd233' order by JobNumberAuto desc
 update jobcontractor set JobNumberAuto=1508 where JobContractorID='1F21CC63-AC28-497F-84B9-93F9823CF104'

select * from JobContractor where contactid='ECA7B55C-3971-41DA-8E84-A50DA10DD233' order by CreatedDate desc
select * from job order by CreatedDate desc

SELECT * FROM JobContractor WHERE JobNumberAuto=1343 and contactid='eca7b55c-3971-41da-8e84-a50da10dd233'

 
INSERT Invoice 
([InvoiceID],[ContractorID],[CustomerID],[DueDate],[InvoiceStatus],
[Terms],[SubmissionDetailLevel],[InvoiceDescription],[TaskID],[SentDate],[CreatedBy],
[CreatedDate],[Active]) 
VALUES 
('24093fc5-ef12-4174-8496-ee26c3556c89','eca7b55c-3971-41da-8e84-a50da10dd233','520aee1b-e42d-4a9a-9697-47814483dddf','2017-05-22 00:00:00.000',0,
'My Terms',0,'My Description','f2562664-d38d-4891-8eb0-4f65491cf4a9','1900-01-01 00:00:00.000','53e58c1b-4d58-41f9-9849-fbb5b4f87833',
'2017-05-22 19:37:46.141',1)

select * from contact where contactid='520aee1b-e42d-4a9a-9697-47814483dddf'

alter table task
add foreign key (taskcustomerid) references contact(contactid) 

SELECT  *
FROM    task
WHERE   TaskCustomerID NOT IN
        (
        SELECT  contactid
        FROM    Contact
        )
select * from JobContractor where jobid='CFC98542-3466-4544-BD73-C7B8FF286F51' --1420
select * from JobContractor where jobid='2169BB2C-C506-46C6-8377-8256DBDCB0EA' --1422
select * from JobContractor where jobid='1D48965E-A964-4D23-9832-4BB43C1478FB' --1421

SELECT  *
FROM    site
WHERE   SiteOwnerID NOT IN
        (
        SELECT  ContactCustomerID
        FROM    ContractorCustomer
        )
		order by CreatedDate

INSERT ContractorCustomer 
([ContactCustomerID],[ContractorID],[CustomerID],[FirstName],[LastName],[Phone],
[Address1],[Address2],[Address3],[Address4],
[CompanyName],[Linked],[Deleted],
[CustomerType],[PendingSiteOwner],[Email],[CreatorContractorCustomer],[CreatedBy],[CreatedDate],[Active]) 
VALUES ('d9e4c403-42ed-439e-a0cd-620422757faf','754a2bb5-7fec-47d5-bfa5-d83a9abd9e35','754a2bb5-7fec-47d5-bfa5-d83a9abd9e35','Richard','Pidcock','033492129',
'Unit 3/25 Lunns Rd','Middleton','Christchurch City','Canterbury',
'Canterbury Metal Solutions',4,0,0,0,'','d9e4c403-42ed-439e-a0cd-620422757faf','53e58c1b-4d58-41f9-9849-fbb5b4f87833','2017-05-22 20:29:57.854',1)

select * from ContractorCustomer where ContactCustomerID='52b3bf55-65c3-44d1-bf83-d3e34187ae06'
select * from contact where contactid='4fb86767-b494-42c5-9f9f-42403f0278da'
 select * from ContractorCustomer where ContactCustomerID='520aee1b-e42d-4a9a-9697-47814483dddf'
 select * from Invoice
 sp_help invoice
 INSERT Invoice 
 ([InvoiceID],[ContractorID],[CustomerID],[DueDate],[InvoiceStatus],[Terms],
 [SubmissionDetailLevel],[InvoiceDescription],[TaskID],[SentDate],[CreatedBy],[CreatedDate],[Active]) 
 VALUES 
 ('39315ffd-05c6-45f9-88ba-30c5a4d6b6d4','eca7b55c-3971-41da-8e84-a50da10dd233','520aee1b-e42d-4a9a-9697-47814483dddf','2017-05-23 00:00:00.000',0,'My Terms',
 0,'My Description','f2562664-d38d-4891-8eb0-4f65491cf4a9','1900-01-01 00:00:00.000','53e58c1b-4d58-41f9-9849-fbb5b4f87833','2017-05-23 21:48:22.417',1)
 
 
 select * from taskmaterial
 
 