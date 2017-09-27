--purge
--update tasklabour set InvoiceID='00000000-0000-0000-0000-000000000000', QuoteID='00000000-0000-0000-0000-000000000000'
--update taskmaterial set InvoiceID='00000000-0000-0000-0000-000000000000', QuoteID='00000000-0000-0000-0000-000000000000'
--delete from Invoice where invoiceid !='00000000-0000-0000-0000-000000000000'

--add constants

--Getting quote table up to date etc.



drop table JobQuote
drop table taskquote
create table Quote
(
QuoteID uniqueIdentifier primary key not null,
CustomerID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', 
ContractorID uniqueidentifier not null, 
QuoteStatus int default 0 not null, 
ExpiryDate date default '1900-01-01 00:00:00.000' not null,
QuoteAcceptorID uniqueidentifier default '00000000-0000-0000-0000-000000000000' not null,
Terms nvarchar(512),
InvoiceDescription nvarchar(512),
SubmissionDetailLevel int default 0 not null,
CreatedBy uniqueIdentifier not null,
CreatedDate date not null default '1900-01-01 00:00:00.000',
Active bit default 0 not null,

CONSTRAINT Quote_Creator
    FOREIGN KEY (CreatedBy)
    REFERENCES contact (contactid),

CONSTRAINT Quote_Acceptor
    FOREIGN KEY (QuoteAcceptorID)
    REFERENCES contact (contactid),

CONSTRAINT Quote_Customer   
	FOREIGN KEY (CustomerID)
    REFERENCES contact (contactid),

CONSTRAINT Quote_Contractor   
	FOREIGN KEY (ContractorID)
    REFERENCES contact (contactid)

)
insert into site values ('00000000-0000-0000-0000-000000000000','1', '2', 'Name','lname','a1','a2','phone','email','00000000-0000-0000-0000-000000000000','2016-10-10',0,'00000000-0000-0000-0000-000000000000',0,'a3','a4','00000000-0000-0000-0000-000000000000',0)
insert into job values ('00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'Name','00000000-0000-0000-0000-000000000000','2016-10-10',1,'','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','',0,0,0,'AccessTypeCustom','','','', 'Stock reqd',1,0,'2016-10-10', '00000000-0000-0000-0000-000000000000','incomplete reason','',0,1,'desc','2016-10-10','2016-10-10')



create table Invoice
(
InvoiceID uniqueIdentifier primary key not null,
CustomerID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', 
ContractorID uniqueidentifier not null, 
InvoiceStatus int default 0 not null, 
DueDate date default '1900-01-01 00:00:00.000' not null,
Terms nvarchar(512),
InvoiceDescription nvarchar(512),
SubmissionDetailLevel int default 0 not null,
CreatedBy uniqueIdentifier not null,
CreatedDate date not null default '1900-01-01 00:00:00.000',
Active bit default 0 not null,
TaskID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',

CONSTRAINT Invoice_Creator
    FOREIGN KEY (CreatedBy)
    REFERENCES contact (contactid),

CONSTRAINT Invoice_Customer   
	FOREIGN KEY (CustomerID)
    REFERENCES contact (contactid),

CONSTRAINT Invoice_Contractor   
	FOREIGN KEY (ContractorID)
    REFERENCES contact (contactid),

Constraint Invoice_Task
	Foreign key (TaskID)
	references task (TaskID)

)



	

alter table task
add QuoteID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', InvoiceNumber int default 0 not null


alter table tasklabour

add QuoteID  uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', QuoteNumber int default 0,InvoiceID  uniqueidentifier not null default '00000000-0000-0000-0000-000000000000'

alter table taskmaterial
add QuoteID  uniqueidentifier not null default '00000000-0000-0000-0000-000000000000', QuoteNumber int default 0,InvoiceID  uniqueidentifier not null default '00000000-0000-0000-0000-000000000000'


insert into task values ('00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000','Taskname',0,'Description', '00000000-0000-0000-0000-000000000000','2016-11-20',1, '00000000-0000-0000-0000-000000000000',0,'2016-11-20',-1,'2016-10-12',-1, 0, '00000000-0000-0000-0000-000000000000',0,0, '00000000-0000-0000-0000-000000000000','2016-10-16', '00000000-0000-0000-0000-000000000000',0,0,'00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',0)
insert into invoice values ('00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',0,'2016-11-20','My Terms','My Description',0,'00000000-0000-0000-0000-000000000000','2016-10-16',1,'00000000-0000-0000-0000-000000000000')



ALTER TAble taskmaterial
add CONSTRAINT Invoice_taskmaterial
    FOREIGN KEY (InvoiceID)
    REFERENCES Invoice (InvoiceID)
	
ALTER TAble tasklabour
add CONSTRAINT Invoice_tasklabour
    FOREIGN KEY (InvoiceID)
    REFERENCES Invoice (InvoiceID)

alter table supplierinvoicematerial
alter column qty decimal(18,2) not null

alter table supplierinvoicematerial
alter column qtyremainingtoassign decimal(18,2) not null


EXEC sp_RENAME 'TaskLabour.quantity' , 'InvoiceQuantity', 'COLUMN'

alter table tasklabour
add InvoiceDescription nvarchar(1024)

alter table tasklabour
alter column description nvarchar(1024)


select * from tasklabour

select * from invoice
select * from task





select * from site where address1 like '%batt%'





