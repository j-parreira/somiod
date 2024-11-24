INSERT INTO Applications ([Name], [CreationDateTime]) 
VALUES 
    ('Lighting', GETDATE());

INSERT INTO Containers ([Name], [CreationDateTime], [Parent]) 
VALUES 
    ('light_bulb', GETDATE(), 1);

INSERT INTO Records ([Name], [Content], [CreationDateTime], [Parent]) 
VALUES 
    ('ON', 'Light is turned on', GETDATE(), 1);

INSERT INTO Notifications ([Name], [CreationDateTime], [Parent], [Event], [Endpoint]) 
VALUES 
    ('Notification 1', GETDATE(), 1, '1', 'http://127.0.0.1');