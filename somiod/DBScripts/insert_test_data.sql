INSERT INTO applications ([name], [creation_datetime]) 
VALUES 
    ('servers', GETDATE()), 
    ('computers', GETDATE()), 
    ('cables', GETDATE());

INSERT INTO containers ([name], [creation_datetime], [parent]) 
VALUES 
    ('server_mqtt', GETDATE(), 1),
    ('server_http', GETDATE(), 1),
    ('desktop', GETDATE(), 2),
    ('laptop', GETDATE(), 2),
    ('rj-45', GETDATE(), 3),
    ('hdmi', GETDATE(), 3);

INSERT INTO records ([name], [creation_datetime], [parent], [content]) 
VALUES 
    ('on1', GETDATE(), 1, 'server_mqtt is turned on'),
    ('off1', GETDATE(), 1, 'server_mqtt is turned off'),
    ('on2', GETDATE(), 2, 'server_http is turned on'),
    ('off2', GETDATE(), 2, 'server_http is turned off'),
    ('on3', GETDATE(), 3, 'desktop is turned on'),
    ('off3', GETDATE(), 3, 'desktop is turned off'),
    ('on4', GETDATE(), 4, 'laptop is turned on'),
    ('off4', GETDATE(), 4, 'laptop is turned off'),
    ('on5', GETDATE(), 5, 'rj-45 is turned on'),
    ('off5', GETDATE(), 5, 'rj-45 is turned off'),
    ('on6', GETDATE(), 6, 'hdmi is turned on'),
    ('off6', GETDATE(), 6, 'hdmi is turned off');

INSERT INTO notifications ([name], [creation_datetime], [parent], [event_type], [endpoint], [enabled]) 
VALUES 
    ('notification_1', GETDATE(), 1, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_2', GETDATE(), 2, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_3', GETDATE(), 3, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_4', GETDATE(), 4, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_5', GETDATE(), 5, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_6', GETDATE(), 6, '0', 'http://localhost:61958/api/message/', 1);