INSERT INTO Applications ([Name], [Creation_DateTime])
VALUES 
    ('Record', GETDATE()),
    ('Diario Leiria', GETDATE()),
    ('Maria', GETDATE());

INSERT INTO Containers ([Name], [Creation_DateTime], [Parent])
VALUES 
    ('Futebol', GETDATE(), 1), 
    ('Basquetebol', GETDATE(), 1), 
    ('Cultura', GETDATE(), 2), 
    ('Moda', GETDATE(), 3);

INSERT INTO Records ([Name], [Content], [Creation_DateTime], [Parent])
VALUES 
    ('Benfica', 'Sample content for Benfica', GETDATE(), 1), 
    ('Sporting', 'Sample content for Sporting', GETDATE(), 1), 
    ('Musica', 'Sample content for Musica', GETDATE(), 2), 
    ('Lisboa', 'Sample content for Lisboa', GETDATE(), 3);

INSERT INTO Notifications ([Name], [Creation_DateTime], [Parent], [Event], [Endpoint])
VALUES 
    ('Notification 1', GETDATE(), 1, '1', 'http://localhost:5000/api/somiod/Record/Futebol/Benfica/creation'),
    ('Notification 2', GETDATE(), 1, '1', 'http://localhost:5000/api/somiod/Record/Futebol/Sporting/creation'), 
    ('Notification 3', GETDATE(), 2, '2', 'http://localhost:5000/api/somiod/DiarioLeiria/Cultura/Musica/creation'),
    ('Notification 4', GETDATE(), 3, '2', 'http://localhost:5000/api/somiod/Maria/Moda/Lisboa/creation'),
    ('Notification 5', GETDATE(), 1, '2', 'http://localhost:5000/api/somiod/Record/Futebol/Sporting/deletion');