alter table contractorcustomer drop column contactid
alter table contractorcustomer drop constraint UQ_ContactCustomer
alter table contractorcustomer drop constraint FK_ContactCustomer_Contact
alter table contractorcustomer add constraint FK_ContractorID foreign key (contractorid) references contact(contactid)
alter table contractorcustomer drop column customerid
---Confirm with Jared before deleting---
alter table site drop column intcount