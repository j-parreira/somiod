INSERT INTO Applications ([Name], [Creation_DateTime]) 
VALUES 
    ('Lighting', GETDATE());

INSERT INTO Containers ([Name], [Creation_DateTime], [Parent]) 
VALUES 
    ('light_bulb', GETDATE(), 1);

INSERT INTO Records ([Name], [Content], [Creation_DateTime], [Parent]) 
VALUES 
    ('ON', 'Light is turned on', GETDATE(), 1);

INSERT INTO Notifications ([Name], [Creation_DateTime], [Parent], [Event], [Endpoint]) 
VALUES 
    ('Notification 1', GETDATE(), 1, '1', 'http://127.0.0.1');