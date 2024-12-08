INSERT INTO applications ([name], [creation_datetime]) 
VALUES 
    ('lighting', GETDATE()), 
    ('heating', GETDATE()), 
    ('security', GETDATE());

INSERT INTO containers ([name], [creation_datetime], [parent]) 
VALUES 
    ('light_bulb', GETDATE(), 1),
    ('parking_light', GETDATE(), 1),
    ('heater', GETDATE(), 2),
    ('ac', GETDATE(), 2),
    ('camera', GETDATE(), 3),
    ('alarm', GETDATE(), 3);

INSERT INTO records ([name], [creation_datetime], [parent], [content]) 
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

INSERT INTO notifications ([name], [creation_datetime], [parent], [event_type], [endpoint], [enabled]) 
VALUES 
    ('notification_1', GETDATE(), 1, '0', '127.0.0.1', 1),
    ('notification_2', GETDATE(), 2, '0', '127.0.0.1', 1),
    ('notification_3', GETDATE(), 3, '1', '127.0.0.1', 1),
    ('notification_4', GETDATE(), 4, '1', '127.0.0.1', 0),
    ('notification_5', GETDATE(), 5, '2', '127.0.0.1', 0),
    ('notification_6', GETDATE(), 6, '2', '127.0.0.1', 0);