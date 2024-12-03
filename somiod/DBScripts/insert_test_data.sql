﻿INSERT INTO Applications ([Name], [CreationDateTime]) 
VALUES 
    ('lighting', GETDATE()), 
    ('heating', GETDATE()), 
    ('security', GETDATE());

INSERT INTO Containers ([Name], [CreationDateTime], [Parent]) 
VALUES 
    ('light_bulb', GETDATE(), 1),
    ('parking_light', GETDATE(), 1),
    ('heater', GETDATE(), 2),
    ('ac', GETDATE(), 2),
    ('camera', GETDATE(), 3),
    ('alarm', GETDATE(), 3);

INSERT INTO Records ([Name], [CreationDateTime], [Parent], [Content]) 
VALUES 
    ('on1', GETDATE(), 1, 'Light is turned on'),
    ('off1', GETDATE(), 1, 'Light is turned off'),
    ('on2', GETDATE(), 2, 'Parking light is turned on'),
    ('off2', GETDATE(), 2, 'Parking light is turned off'),
    ('on3', GETDATE(), 3, 'Heater is turned on'),
    ('off3', GETDATE(), 3, 'Heater is turned off'),
    ('on4', GETDATE(), 4, 'AC is turned on'),
    ('off4', GETDATE(), 4, 'AC is turned off'),
    ('on5', GETDATE(), 5, 'Camera is turned on'),
    ('off5', GETDATE(), 5, 'Camera is turned off'),
    ('on6', GETDATE(), 6, 'Alarm is turned on'),
    ('off6', GETDATE(), 6, 'Alarm is turned off');

INSERT INTO Notifications ([Name], [CreationDateTime], [Parent], [Event], [Endpoint], [Enabled]) 
VALUES 
    ('notification_1', GETDATE(), 1, '1', '127.0.0.1', 1),
    ('notification_2', GETDATE(), 2, '2', '127.0.0.1', 1),
    ('notification_3', GETDATE(), 3, '3', '127.0.0.1', 0),
    ('notification_4', GETDATE(), 4, '4', '127.0.0.1', 0),
    ('notification_5', GETDATE(), 5, '5', '127.0.0.1', 1),
    ('notification_6', GETDATE(), 6, '6', '127.0.0.1', 1);