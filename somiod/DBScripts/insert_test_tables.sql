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
    ('ON', 'Light is turned on', GETDATE(), 1),
    ('OFF', 'Light is turned off', GETDATE(), 1),
    ('ON', 'Parking light is turned on', GETDATE(), 2),
    ('OFF', 'Parking light is turned off', GETDATE(), 2),
    ('ON', 'Heater is turned on', GETDATE(), 3),
    ('OFF', 'Heater is turned off', GETDATE(), 3),
    ('ON', 'AC is turned on', GETDATE(), 4),
    ('OFF', 'AC is turned off', GETDATE(), 4),
    ('ON', 'Camera is turned on', GETDATE(), 5),
    ('OFF', 'Camera is turned off', GETDATE(), 5),
    ('ON', 'Alarm is turned on', GETDATE(), 6),
    ('OFF', 'Alarm is turned off', GETDATE(), 6);

INSERT INTO Notifications ([Name], [CreationDateTime], [Parent], [Event], [Endpoint]) 
VALUES 
    ('Notification 1', GETDATE(), 1, '1', 'http://127.0.0.1'),
    ('Notification 2', GETDATE(), 2, '2', 'mqtt://127.0.0.1'),
    ('Notification 3', GETDATE(), 3, '3', 'http://127.0.0.1'),
    ('Notification 4', GETDATE(), 4, '4', 'mqtt://127.0.0.1'),
    ('Notification 5', GETDATE(), 5, '5', 'http://127.0.0.1'),
    ('Notification 6', GETDATE(), 6, '6', 'mqtt://127.0.0.1');