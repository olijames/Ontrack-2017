CREATE TRIGGER tr_setDefaultCar ON vehicle
    AFTER INSERT, DELETE
AS
    BEGIN
        DECLARE @cnt INT;
        DECLARE @driverID VARCHAR(36);
        DECLARE @vehicleID VARCHAR(36);
    
		-- IF any values inserted to vehicle table
        IF EXISTS ( SELECT  *
                    FROM    inserted )
            BEGIN
                SELECT  @driverID = [vehicleDriver] ,
                        @vehicleID = [vehicleID]
                FROM    inserted;  

                SELECT  @cnt = COUNT(*)
                FROM    vehicle
                WHERE   vehicleDriver = @driverID;

                -- IF there is only one vehicle exists after insert, it will be the default car
                -- therefore update defaultvehicleID in employeeInfo table
                IF ( @cnt = 1 )
                    BEGIN
                        UPDATE  employeeInfo
                        SET     defaultVehicleID = @vehicleID
                        WHERE   employeeID = @driverID;
                    END;
            END;

		-- IF any values deleted from vehicle table
        IF EXISTS ( SELECT  *
                    FROM    deleted )
            BEGIN
                SELECT  @driverID = [vehicleDriver]
                FROM    deleted;

                SELECT  @cnt = COUNT(*)
                FROM    vehicle
                WHERE   vehicleDriver = @driverID;

				-- IF only one car exists after delete, set the vehicle to default car
                IF ( @cnt = 1 )
                    BEGIN
                        SELECT  @vehicleID = [vehicleID]
                        FROM    vehicle
                        WHERE   vehicleDriver = @driverID;
						 
                        UPDATE  employeeInfo
                        SET     defaultVehicleID = @vehicleID
                        WHERE   employeeID = @driverID;
                    END;

				-- IF no car exists after delete, update default car id in employeeInfo table to '00000000-0000-0000-0000-000000000000'
                IF ( @cnt = 0 )
                    BEGIN
                        UPDATE  employeeInfo
                        SET     defaultVehicleID = '00000000-0000-0000-0000-000000000000'
                        WHERE   employeeID = @driverID;
                    END;

            END;
    END;