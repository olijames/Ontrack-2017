--executed local 'ontrack-20th may' 25/5 jared

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

Alter table fileupload
add 
FileSize bigint not null default 0,
CompanyID uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
constraint [fk_fileupload_company] foreign key (companyid) references contact (contactid)

select * from fileupload where CompanyID='ECA7B55C-3971-41DA-8E84-A50DA10DD233'