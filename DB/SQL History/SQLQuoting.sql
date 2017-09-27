update taskmaterial set ContractorID='ECA7B55C-3971-41DA-8E84-A50DA10DD233' where contractorid is null
update tasklabour set ContractorID='ECA7B55C-3971-41DA-8E84-A50DA10DD233' where contractorid is null
update taskmaterial set CustomerID='ECA7B55C-3971-41DA-8E84-A50DA10DD233' where customerid is null
update tasklabour set CustomerID='ECA7B55C-3971-41DA-8E84-A50DA10DD233' where customerid is null

select * from Task where taskCustomerID='C9A7CEBF-C61A-4386-947F-004EAB5E3A82' order by jobid
select * from supplierinvoice where contractorreference like'1237%'
delete from SupplierInvoiceMaterial where SupplierInvoiceID='8EE547C0-CD9C-44C6-BB3A-3FC04C086186' and qty<0
select * from SupplierInvoiceMaterial where SupplierInvoiceID='C6F516E4-FB6B-4AC7-A385-CF7B640E3765' and qty<0
select * from SupplierInvoiceMaterial where SupplierInvoiceID='84BF3982-BEE1-4A47-8792-65568DDC75F9' and qty<0
select * from SupplierInvoiceMaterial where SupplierInvoiceID='AE967ECC-F11D-4711-862B-376E67F3727B' and qty<0
select * from SupplierInvoiceMaterial where SupplierInvoiceID='ACC989A9-03B6-492A-AACF-B4AE7C4D328C' and qty<0


update SupplierInvoice set Status=0 where SupplierInvoiceID='4203F633-C39B-4D80-8C0E-ACD42B9EC064'

update SupplierInvoice  set Status=0 where SupplierInvoiceID='8BE44C4D-9113-40E6-B0F5-F0F8D64E95A5'
update SupplierInvoice  set Status=0 where SupplierInvoiceID='84BF3982-BEE1-4A47-8792-65568DDC75F9'
update SupplierInvoice  set Status=0 where SupplierInvoiceID='AE967ECC-F11D-4711-862B-376E67F3727B'
update SupplierInvoice  set Status=0 where SupplierInvoiceID='ACC989A9-03B6-492A-AACF-B4AE7C4D328C'



select * from Contact where managerID='53E58C1B-4D58-41F9-9849-FBB5B4F87833'
--ECA7B55C-3971-41DA-8E84-A50DA10DD233


----purge
update tasklabour set InvoiceID='00000000-0000-0000-0000-000000000000', QuoteID='00000000-0000-0000-0000-000000000000'
update taskmaterial set InvoiceID='00000000-0000-0000-0000-000000000000', QuoteID='00000000-0000-0000-0000-000000000000'
delete from Invoice where invoiceid !='00000000-0000-0000-0000-000000000000'


--select * from contact where 

 --select distinct Taskmaterial.sellprice*taskmaterial.InvoiceQuantity as MaterialTotal, tasklabour.InvoiceQuantity*{2} as LabourTotal, task.*, job.name, job.jobnumberauto from Task,job, TaskMaterial, TaskLabour where task.taskcustomerID={1} and task.contractorid={0} and task.jobid=job.jobid and task.status!=2 and TaskMaterial.TaskID=task.taskid and TaskLabour.TaskID=task.taskid
 update TaskMaterial set InvoiceQuantity=2
 update TaskLabour set InvoiceQuantity=4

----add constants

----Getting quote table up to date etc.



--drop table JobQuote
--drop table taskquote
--create table Quote
--(
--QuoteID uniqueIdentifier primary key not null,
--CustomerID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', 
--ContractorID uniqueidentifier not null, 
--QuoteStatus int default 0 not null, 
--ExpiryDate date default '1900-01-01 00:00:00.000' not null,
--QuoteAcceptorID uniqueidentifier default '00000000-0000-0000-0000-000000000000' not null,
--Terms nvarchar(512),
--InvoiceDescription nvarchar(512),
--SubmissionDetailLevel int default 0 not null,
--CreatedBy uniqueIdentifier not null,
--CreatedDate date not null default '1900-01-01 00:00:00.000',
--Active bit default 0 not null,

--CONSTRAINT Quote_Creator
--    FOREIGN KEY (CreatedBy)
--    REFERENCES contact (contactid),

--CONSTRAINT Quote_Acceptor
--    FOREIGN KEY (QuoteAcceptorID)
--    REFERENCES contact (contactid),

--CONSTRAINT Quote_Customer   
--	FOREIGN KEY (CustomerID)
--    REFERENCES contact (contactid),

--CONSTRAINT Quote_Contractor   
--	FOREIGN KEY (ContractorID)
--    REFERENCES contact (contactid)

--)
--insert into site values ('00000000-0000-0000-0000-000000000000','1', '2', 'Name','lname','a1','a2','phone','email','00000000-0000-0000-0000-000000000000','2016-10-10',0,'00000000-0000-0000-0000-000000000000',0,'a3','a4','00000000-0000-0000-0000-000000000000',0)
--insert into job values ('00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'Name','00000000-0000-0000-0000-000000000000','2016-10-10',1,'','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','',0,0,0,'AccessTypeCustom','','','', 'Stock reqd',1,0,'2016-10-10', '00000000-0000-0000-0000-000000000000','incomplete reason','',0,1,'desc','2016-10-10','2016-10-10')



--create table Invoice
--(
--InvoiceID uniqueIdentifier primary key not null,
--CustomerID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', 
--ContractorID uniqueidentifier not null, 
--InvoiceStatus int default 0 not null, 
--DueDate date default '1900-01-01 00:00:00.000' not null,
--Terms nvarchar(512),
--InvoiceDescription nvarchar(512),
--SubmissionDetailLevel int default 0 not null,
--CreatedBy uniqueIdentifier not null,
--CreatedDate date not null default '1900-01-01 00:00:00.000',
--Active bit default 0 not null,
--TaskID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',

--CONSTRAINT Invoice_Creator
--    FOREIGN KEY (CreatedBy)
--    REFERENCES contact (contactid),

--CONSTRAINT Invoice_Customer   
--	FOREIGN KEY (CustomerID)
--    REFERENCES contact (contactid),

--CONSTRAINT Invoice_Contractor   
--	FOREIGN KEY (ContractorID)
--    REFERENCES contact (contactid),

--Constraint Invoice_Task
--	Foreign key (TaskID)
--	references task (TaskID)

--)



	

--alter table task
--add QuoteID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', InvoiceNumber int default 0 not null


--alter table tasklabour

--add QuoteID  uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', QuoteNumber int default 0,InvoiceID  uniqueidentifier not null default '00000000-0000-0000-0000-000000000000'

--alter table taskmaterial
--add QuoteID  uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', QuoteNumber int default 0,InvoiceID  uniqueidentifier not null default '00000000-0000-0000-0000-000000000000'


--insert into task values ('00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','Taskname',0,'Description', '00000000-0000-0000-0000-000000000000','2016-11-20',1, '00000000-0000-0000-0000-000000000000',0,'2016-11-20',-1,'2016-10-12',-1, 0, '00000000-0000-0000-0000-000000000000',0,0, '00000000-0000-0000-0000-000000000000','2016-10-16', '00000000-0000-0000-0000-000000000000',0,0,'00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',0)
--insert into invoice values ('00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',0,'2016-11-20','My Terms','My Description',0,'00000000-0000-0000-0000-000000000000','2016-10-16',1,'00000000-0000-0000-0000-000000000000')

select * from invoice

--ALTER TAble taskmaterial
--add CONSTRAINT Invoice_taskmaterial
--    FOREIGN KEY (InvoiceID)
--    REFERENCES Invoice (InvoiceID)
	
--ALTER TAble tasklabour
--add CONSTRAINT Invoice_tasklabour
--    FOREIGN KEY (InvoiceID)
--    REFERENCES Invoice (InvoiceID)

--alter table supplierinvoicematerial
--alter column qty decimal(18,2) not null

--alter table supplierinvoicematerial
--alter column qtyremainingtoassign decimal(18,2) not null


--EXEC sp_RENAME 'TaskLabour.quantity' , 'InvoiceQuantity', 'COLUMN'

--alter table tasklabour
--add InvoiceDescription nvarchar(1024)

--alter table tasklabour
--alter column description nvarchar(1024)




--alter table invoice
--add SentDate date default '1900-01-01'
--update invoice set sentdate='1900-01-01 00:00:00.000'
--update invoice set duedate='1900-01-01 00:00:00.000'




--alter table taskmaterial
--add InvoiceQuantity decimal(18,2) 








		--from TaskMaterial TM, contact c, TaskLabour TL where t.ContractorID='eca7b55c-3971-41da-8e84-a50da10dd233' and c.ContactID=tl.CustomerID and (tl.InvoiceID='00000000-0000-0000-0000-000000000000' or tm.InvoiceID='00000000-0000-0000-0000-000000000000') 
       --         and (tm.active=1 or tl.active=1)




	


--	alter table taskmaterial
--add  CustomerID uniqueidentifier, ContractorID uniqueidentifier

--alter table tasklabour
--add  CustomerID uniqueidentifier, ContractorID uniqueidentifier

--update TaskMaterial set customerid=task.TaskCustomerID from task, taskmaterial where taskmaterial.taskid=task.taskid
--update TaskLabour set customerid=task.TaskCustomerID from task, tasklabour where tasklabour.taskid=task.taskid
--update TaskMaterial set contractorid=task.ContractorID from task, taskmaterial where taskmaterial.taskid=task.taskid
--update TaskLabour set contractorid=task.ContractorID from task, tasklabour where tasklabour.taskid=task.taskid

--BELOW HERE NOT EXECUTED LIVE


alter table taskmaterial
add InvoiceDescription nvarchar(512)



--run on live with update
update task set status =1 where status=4 or status=5








