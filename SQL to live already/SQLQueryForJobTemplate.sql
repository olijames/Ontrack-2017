
--delete from jobtemplatetask
--delete from task where status=7
--delete from jobtemplate

select * from JobTemplatetask

select * from Task where status=7

select * from taskmaterial






update job set StartDate='1900-01-01 00:00:00.000', EndDate='1900-01-01 00:00:00.000'


create table JobTemplate
(
JobTemplateID uniqueidentifier not null,
JobTemplateName NVarChar(128) not null,
ContractorID uniqueidentifier not null,
CreatedBy uniqueidentifier not null,
CreatedDate date not null,
Active bit default 1,
constraint pk_JobTemplate Primary key (JobTemplateID),
constraint fk_contact_JobTemplate foreign key (CompanyID) references contact(contactid)

)

create table JobTemplateTask
(
JobTemplateTaskID uniqueidentifier not null,
TemplateTaskID uniqueidentifier not null,
JobTemplateID uniqueidentifier not null,
StartDelay Decimal default 0,
Duration Decimal default 168,
CreatedBy uniqueidentifier not null,
CreatedDate date not null,
Active bit default 1,
constraint pk_JobTemplateTask Primary key (JobTemplateTaskID),
constraint fk_templatetask foreign key (TemplateTaskID) references Task(taskid),
constraint fk_jobtemplate foreign key (JobTemplateID) references JobTemplate(JobTemplateID)
)

--CREATE TABLE [dbo].[TaskFile] (
--    [TaskFileID]   UNIQUEIDENTIFIER NOT NULL,
--    [TaskID]       UNIQUEIDENTIFIER NOT NULL,
--    [FileID]      UNIQUEIDENTIFIER NOT NULL,
--    [CreatedBy]   UNIQUEIDENTIFIER NOT NULL,
--    [CreatedDate] DATETIME         NOT NULL,
--    [Active]      BIT              NOT NULL,
--    PRIMARY KEY CLUSTERED ([TaskFileID] ASC),
--    CONSTRAINT [FK_TaskFile_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Contact] ([ContactID]),
--    CONSTRAINT [FK_TaskFile_File] FOREIGN KEY ([FileID]) REFERENCES [dbo].[FileUpload] ([FileID]),
--    CONSTRAINT [FK_TaskFile_Job] FOREIGN KEY ([TaskID]) REFERENCES [dbo].[Task] ([TaskID])
--);

insert into site 
(
 [SiteID],            
    [Address1],
    [Address2],       
    [CustomerFirstName],
    [CustomerLastName], 
    [CustomerAddress1], 
    [CustomerAddress2], 
    [CustomerPhone],    
    [CustomerEmail],    
    [CreatedBy],        
    [CreatedDate],      
    [Active],           
    [ContactID],        
    [VisibilityStatus], 
    [Address3],         
    [Address4],         
    [SiteOwnerID],      
    [intCount] 

)
values
(
 '99999999-9999-9999-9999-999999999999',            
    'Address1',
    'Address2',       
    'CustomerFirstName',
    'CustomerLastName', 
    'CustomerAddress1', 
    'CustomerAddress2', 
    'CustomerPhone',    
    'CustomerEmail',    
    '00000000-0000-0000-0000-000000000000',        
    GETDATE(),      
    0,           
    '00000000-0000-0000-0000-000000000000',        
    1, 
    'Address3',         
    'Address4',         
    '00000000-0000-0000-0000-000000000000',      
    0 
	
)

insert into Job 
(
 [JobID],                
    [SiteID] ,             
    [JobOwner] ,           
    [Name]   ,             
    [CreatedBy]   ,        
    [CreatedDate],          
    [Active]  ,             
    [JobNumber] ,          
    [ProjectManagerID] ,    
    [InvoiceTo],            
    [ProjectManagerText],   
    [JobStatus]  ,          
    [JobType],              
    [AccessType] ,          
    [AccessTypeCustom]  ,    
    [PoweredItems]  ,   
    [AlarmCode]  ,        
    [SiteNotes] ,        
    [StockRequired]    ,   
    [NoPoweredItems],        
    [QuotedAmount],      
    [CompletedDate],        
    [CompletedBy] ,         
    [IncompleteTasksReason],
    [ProjectManagerPhone],  
    [InvoiceToType]   ,      
    [JobNumberAuto] ,        
    [Description]          


)
values
(

 '99999999-9999-9999-9999-999999999999',                
    '99999999-9999-9999-9999-999999999999',             
    '00000000-0000-0000-0000-000000000000',           
    'Name'  ,             
    '00000000-0000-0000-0000-000000000000'   ,        
    getdate(),          
    0  ,             
    'JobNumber' ,          
    '00000000-0000-0000-0000-000000000000',    
    '00000000-0000-0000-0000-000000000000',            
    'ProjectManagerText',   
    0  ,          
    0,              
    0 ,          
    'AccessTypeCustom'  ,    
    'PoweredItems'  ,   
    'AlarmCode'  ,        
    'SiteNotes' ,        
    'StockRequired'    ,   
    0,        
    100.99,      
    getdate(),        
    '00000000-0000-0000-0000-000000000000',         
    'IncompleteTasksReason',
    'ProjectManagerPhone',  
    1   ,      
    1 ,        
    'Description'

)

--insert into JobTemplate
--(
--JobTemplateID ,
--JobTemplateName ,
--ContractorID,
--CreatedBy,
--CreatedDate,
--Active
--)
--values
--(
--'00000000-0000-0000-0000-000000000000',
--'None',
--'00000000-0000-0000-0000-000000000000',
--'00000000-0000-0000-0000-000000000000',
--GETDATE(),
--1
--)
