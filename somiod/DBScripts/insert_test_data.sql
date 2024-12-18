/**
* @brief Somiod - Projeto de Integração de Sistemas
* @date 2024-12-18
* @authors Diogo Abegão, João Parreira, Marcelo Oliveira, Pedro Barbeiro
*/
INSERT INTO applications ([name], [creation_datetime]) 
VALUES 
    ('http_servers', GETDATE()), 
    ('http_computers', GETDATE()), 
    ('http_sensors', GETDATE());

INSERT INTO containers ([name], [creation_datetime], [parent]) 
VALUES 
    ('http_server_mail', GETDATE(), 1),
    ('http_server_proxy', GETDATE(), 1),
    ('http_desktop', GETDATE(), 2),
    ('http_laptop', GETDATE(), 2),
    ('http_temperature_sensor', GETDATE(), 3),
    ('http_distance_sensor', GETDATE(), 3);

INSERT INTO records ([name], [creation_datetime], [parent], [content]) 
VALUES 
    ('on1', GETDATE(), 1, 'server_mail is turned on'),
    ('off1', GETDATE(), 1, 'server_mail is turned off'),
    ('on2', GETDATE(), 2, 'server_proxy is turned on'),
    ('off2', GETDATE(), 2, 'server_proxy is turned off'),
    ('on3', GETDATE(), 3, 'desktop is turned on'),
    ('off3', GETDATE(), 3, 'desktop is turned off'),
    ('on4', GETDATE(), 4, 'laptop is turned on'),
    ('off4', GETDATE(), 4, 'laptop is turned off'),
    ('on5', GETDATE(), 5, 'temperature_sensor is turned on'),
    ('off5', GETDATE(), 5, 'temperature_sensor is turned off'),
    ('on6', GETDATE(), 6, 'distance_sensor is turned on'),
    ('off6', GETDATE(), 6, 'distance_sensor is turned off');

INSERT INTO notifications ([name], [creation_datetime], [parent], [event_type], [endpoint], [enabled]) 
VALUES 
    ('notification_1', GETDATE(), 1, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_2', GETDATE(), 2, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_3', GETDATE(), 3, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_4', GETDATE(), 4, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_5', GETDATE(), 5, '0', 'http://localhost:61958/api/message/', 1),
    ('notification_6', GETDATE(), 6, '0', 'http://localhost:61958/api/message/', 1);