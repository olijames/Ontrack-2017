select contactid, ManagerID from contact where contactid='c0c5e069-3382-4c03-93b3-bd7647c08117'
update contact set ManagerID=contactid where ManagerID='00000000-0000-0000-0000-000000000000'
select * from Contact
select * from TaskLabour

select * from contact order by CreatedDate
select * from site
select * from JobContractor
select * from ContractorCustomer
select distinct c.contactid,c.FirstName,c.LastName,c.Address1,c.Address2,c.Address3,c.Address4,c.Email,c.Phone,c.CompanyName,c.ContactType,c.CreatedBy,c.CreatedDate,c.Active from Contact c join ContractorCustomer cc on cc.CustomerID=c.ContactID where cc.ContractorID='eca7b55c-3971-41da-8e84-a50da10dd233' and c.contactid NOT IN ( select contactid FROM contactSite WHERE siteID = 'd8e6d82b-3af3-4b6d-a658-7e26b87d1875') order by c.firstname, c.CompanyName
select * from contact where CompanyName='ligo' or CompanyName like 'electr%'

select * from EmployeeInfo
select * from task where TaskName='27test'
update task set TaskType=1 where taskname='27test'
select * from JobContractor
SELECT distinct c.ContactID,c.FirstName,c.LastName,c.Email,c.Phone,c.ContactType,c.Address1,c.Address2,c.Address3,c.Address4,c.CompanyName,c.createdby,c.createddate,c.active FROM JobContractor jc INNER JOIN Contact c ON jc.ContactID = c.ContactID WHERE jc.JobID = '8bb8fedc-d4fd-425a-a6b8-3d86f584b34f'  ORDER BY ContactType DESC, LastName, CompanyName

SELECT e.*,cc.ContactID FROM ContactCompany cc INNER JOIN EmployeeInfo e ON cc.ContactCompanyID = e.ContactCompanyID WHERE cc.ContactID = '53e58c1b-4d58-41f9-9849-fbb5b4f87833' AND cc.companyID = '51275e7d-aa64-445f-85d8-36f372b819b3'
select * from ContactCompany where ContactCompanyID='51275e7d-aa64-445f-85d8-36f372b819b3'
select * from SupplierInvoiceMaterial
select * from ContactCompany order by active
select * from job


"INSERT TaskLabour ([TaskLabourID],[TaskID],[LabourID],[Description],[InvoiceQuantity],[ContactID],[LabourDate],[StartMinute],[EndMinute],[TaskLabourCategoryID],[Chargeable],[LabourType],[LabourRate],[QuoteNumber],[InvoiceID],[InvoiceDescription],[CustomerID],[ContractorID],[CreatedBy],[CreatedDate],[Active]) 
VALUES ('53187660-cc29-466f-8772-12dbb9d39cc6','acedee2f-2c55-48a8-b103-000136883218','00000000-0000-0000-0000-000000000000','',0,'acedee2f-2c55-48a8-b103-000136883218','2017-03-06 21:18:21.162',0,0,'00000000-0000-0000-0000-000000000000',0,0,0,0,'00000000-0000-0000-0000-000000000000','','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','c9a7cebf-c61a-4386-947f-004eab5e3a82','2017-03-06 21:18:21.162',1)"
select * from labour

select * from Vehicle
select * from EmployeeInfo where EmployeeID='FC51F221-7979-4D6E-B0FD-54EBEEBEDA17'


select NEWID() as tempid, vehicle.vehicleid as vehicle, Material.MaterialName, Material.CostPrice, Material.SellPrice, Material.RRP, supplier.suppliername, material.uom, material.materialid, 
supplierinvoicematerial.qty, supplierinvoicematerial.qtyremainingtoassign, SupplierInvoice.CreatedBy, SupplierInvoice.CreatedDate,                     
SupplierInvoice.Active, supplier.supplierid, supplierinvoicematerial.oldsupplierinvoicematerialid, supplierinvoicematerial.createdby as Creator,                    
SupplierInvoiceMaterial.SupplierInvoiceMaterialID, supplierinvoicematerial.supplierinvoiceid, supplierinvoice.ContractorReference                    
from SupplierInvoiceMaterial, Material, Vehicle, SupplierInvoice, supplier                    
where                     SupplierInvoiceMaterial.VehicleID='7dcaba1a-7e8c-4a03-b286-18cb447a2960'                     
and Vehicle.VehicleID='7dcaba1a-7e8c-4a03-b286-18cb447a2960'
and SupplierInvoiceMaterial.QtyRemainingToAssign > 0                    
and SupplierInvoiceMaterial.vehicleid!='00000000-0000-0000-0000-000000000000'                    
and supplier.supplierid=supplierinvoice.supplierid 
and SupplierInvoiceMaterial.SupplierInvoiceID=SupplierInvoice.SupplierInvoiceID                    
and supplierinvoiceMaterial.contractorid='eca7b55c-3971-41da-8e84-a50da10dd233'
and Vehicle.VehicleOwner='eca7b55c-3971-41da-8e84-a50da10dd233'
and Vehicle.VehicleDriver='fc51f221-7979-4d6e-b0fd-54ebeebeda17'
and SupplierInvoiceMaterial.MaterialID=Material.MaterialID                    
and  (SupplierInvoiceMaterial.TaskMaterialID='00000000-0000-0000-0000-000000000000'  or SupplierInvoiceMaterial.TaskMaterialID!='00000000-0000-0000-0000-000000000000'
and SupplierInvoiceMaterial.OldSupplierInvoiceMaterialID='00000000-0000-0000-0000-000000000000')                    order by material.MaterialName                  


select * from Contact, ContractorCustomer where PendingSiteOwner=1 and contact.ContactID=ContractorCustomer.CustomerId order by Contact.CreatedDate

