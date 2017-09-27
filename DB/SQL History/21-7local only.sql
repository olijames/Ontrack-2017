
--below may need to be executed on live site

--user constants
insert into Supplier values
('baaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Deleted V items','00000000-0000-0000-0000-000000000000','2016-03-21 05:00:32.940',1)

insert into Supplier values
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Deleted TM items','00000000-0000-0000-0000-000000000000','2016-03-21 05:00:32.940',1)


insert into SupplierInvoice values ('00000000-0000-0000-0000-000000000001','aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa','Deleted from task','',
'00000000-0000-0000-0000-000000000000','2016-03-21 05:00:32.940','00000000-0000-0000-0000-000000000000','2016-03-21 05:00:32.940',1,0)

insert into SupplierInvoice values (
'00000000-0000-0000-0000-000000000000', 'CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC', '','','00000000-0000-0000-0000-000000000000','2016-02-19 00:00:00.000','00000000-0000-0000-0000-000000000000','2016-02-19 00:00:00.000',1,0)

insert into vehicle values ('00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','default','','2016-07-08 00:00:00.000','2016-07-08 00:00:00.000','2016-07-08 00:00:00.000','2016-07-08 00:00:00.000','00000000-0000-0000-0000-000000000000',1)


--prevent future errors
update material set SupplierProductCode = '' where SupplierProductCode IS NULL
update material set uom = '' where uom IS NULL
update material set rrp = 0 where rrp IS NULL
ALTER TABLE material ADD DEFAULT '' FOR SupplierProductCode;
ALTER TABLE material ADD DEFAULT '' FOR uom;
ALTER TABLE material ADD DEFAULT 0 FOR rrp;

--need default vehicle labeled bin (per entity?)


--foreignkey
alter table supplierinvoicematerial
ADD CONSTRAINT fk_vehicle
    FOREIGN KEY (vehicleid)
    REFERENCES vehicle (vehicleid);



--already executed on live 23/7
alter table contact 
add DefaultRegion uniqueidentifier default '00000000-0000-0000-0000-000000000000'
update contact set DefaultRegion = '00000013-0000-0000-0000-000000000000'
ALTER TABLE contact
ADD CONSTRAINT fk_region1
    FOREIGN KEY (Defaultregion)
    REFERENCES region (regionid);

