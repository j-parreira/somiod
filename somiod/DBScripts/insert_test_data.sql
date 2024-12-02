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

INSERT INTO Records ([Name], [CreationDateTime], [Parent], [Content]) 
VALUES 
    ('ON1', GETDATE(), 1, 'Light is turned on'),
    ('OFF1', GETDATE(), 1, 'Light is turned off'),
    ('ON2', GETDATE(), 2, 'Parking light is turned on'),
    ('OFF2', GETDATE(), 2, 'Parking light is turned off'),
    ('ON3', GETDATE(), 3, 'Heater is turned on'),
    ('OFF3', GETDATE(), 3, 'Heater is turned off'),
    ('ON4', GETDATE(), 4, 'AC is turned on'),
    ('OFF4', GETDATE(), 4, 'AC is turned off'),
    ('ON5', GETDATE(), 5, 'Camera is turned on'),
    ('OFF5', GETDATE(), 5, 'Camera is turned off'),
    ('ON6', GETDATE(), 6, 'Alarm is turned on'),
    ('OFF6', GETDATE(), 6, 'Alarm is turned off');

INSERT INTO Notifications ([Name], [CreationDateTime], [Parent], [Event], [Endpoint], [Enabled]) 
VALUES 
    ('Notification_1', GETDATE(), 1, '1', 'http://127.0.0.1', 1),
    ('Notification_2', GETDATE(), 2, '2', 'mqtt://127.0.0.1', 1),
    ('Notification_3', GETDATE(), 3, '3', 'http://127.0.0.1', 0),
    ('Notification_4', GETDATE(), 4, '4', 'mqtt://127.0.0.1', 0),
    ('Notification_5', GETDATE(), 5, '5', 'http://127.0.0.1', 1),
    ('Notification_6', GETDATE(), 6, '6', 'mqtt://127.0.0.1', 1);