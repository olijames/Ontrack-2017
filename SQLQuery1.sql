select * from task where jobid='9B5BEB9A-E12F-4098-9DAF-A050776B5F60'
select * from site where siteid='A2CD37DF-69A1-4AD4-91A6-91170B463D5F'
select * from job where jobid='9B5BEB9A-E12F-4098-9DAF-A050776B5F60'

select * from Contact where ContactID='26D75EDF-F50B-4DE5-9273-63A11D30CB6C'
select * from ContractorCustomer where ContactCustomerID='483aed7d-d242-47a1-b745-1d4c104ae786'
select * from ContractorCustomer where ContactCustomerID='90F632B0-B1D6-41D5-864C-565D0AA4D579'
select * from contact where contactid='33194A21-08AE-4BB2-9B50-4DF715422B64'

--need to update task.taskcustomer where taskcustomer points to the contact table

select * from jobcontractor where JobID='2010dcda-71bb-4556-aca3-8f7d4d4df603', eca7b55c-3971-41da-8e84-a50da10dd233


Select distinct cc.ContactCustomerID,cc.FirstName,cc.LastName,cc.Email,cc.Phone,
                                        cc.CustomerType,cc.Address1,cc.Address2,cc.Address3,cc.Address4,cc.CompanyName,cc.createdby,cc.createddate,cc.active
                                        from
										(select distinct c.contactid from contact, JobContractor jc INNER JOIN Contact c ON jc.ContactID = C.ContactID 
                                            WHERE jc.JobID = '9B5BEB9A-E12F-4098-9DAF-A050776B5F60' AND c.Active = 1
										) a, ContractorCustomer cc where a.ContactID=cc.ContractorID and a.ContactID=cc.CustomerId
										ORDER BY CustomerType DESC, LastName, CompanyName



										select distinct c.contactid from contact, JobContractor jc INNER JOIN Contact c ON jc.ContactID = C.ContactID 
                                            WHERE jc.JobID = '9B5BEB9A-E12F-4098-9DAF-A050776B5F60' AND c.Active = 1
											
											
											a, ContractorCustomer cc 
											where a.ContactID=cc.ContractorID and a.ContactID=cc.CustomerId

