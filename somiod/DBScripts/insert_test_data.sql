INSERT INTO Applications ([Name], [CreationDateTime]) 
VALUES 
    ('Lighting', GETDATE()), 
    ('Heating', GETDATE()), 
    ('Security', GETDATE());

INSERT INTO Containers ([Name], [CreationDateTime], [Parent]) 
VALUES 
    ('light_bulb', GETDATE(), 1),
    ('parking_light', GETDATE(), 1),
    ('heater', GETDATE(), 2),
    ('AC', GETDATE(), 2),
    ('camera', GETDATE(), 3),
    ('alarm', GETDATE(), 3);

INSERT INTO Records ([Name], [Content], [CreationDateTime], [Parent]) 
VALUES 
    ('ON1', 'Light is turned on', GETDATE(), 1),
    ('OFF1', 'Light is turned off', GETDATE(), 1),
    ('ON2', 'Parking light is turned on', GETDATE(), 2),
    ('OFF2', 'Parking light is turned off', GETDATE(), 2),
    ('ON3', 'Heater is turned on', GETDATE(), 3),
    ('OFF3', 'Heater is turned off', GETDATE(), 3),
    ('ON4', 'AC is turned on', GETDATE(), 4),
    ('OFF4', 'AC is turned off', GETDATE(), 4),
    ('ON5', 'Camera is turned on', GETDATE(), 5),
    ('OFF5', 'Camera is turned off', GETDATE(), 5),
    ('ON6', 'Alarm is turned on', GETDATE(), 6),
    ('OFF6', 'Alarm is turned off', GETDATE(), 6);

INSERT INTO Notifications ([Name], [CreationDateTime], [Parent], [Event], [Endpoint], [Enabled]) 
VALUES 
    ('Notification_1', GETDATE(), 1, '1', 'http://127.0.0.1', 1),
    ('Notification_2', GETDATE(), 2, '2', 'mqtt://127.0.0.1', 1),
    ('Notification_3', GETDATE(), 3, '3', 'http://127.0.0.1', 0),
    ('Notification_4', GETDATE(), 4, '4', 'mqtt://127.0.0.1', 0),
    ('Notification_5', GETDATE(), 5, '5', 'http://127.0.0.1', 1),
    ('Notification_6', GETDATE(), 6, '6', 'mqtt://127.0.0.1', 1);