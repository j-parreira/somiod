/**
* @brief Somiod - Projeto de Integra��o de Sistemas
* @date 2024-12-18
* @authors Diogo Abeg�o, Jo�o Parreira, Marcelo Oliveira, Pedro Barbeiro
*/
ALTER TABLE [dbo].[notifications] DROP CONSTRAINT [FK_Notification_Parent];
ALTER TABLE [dbo].[records] DROP CONSTRAINT [FK_Record_Parent];
ALTER TABLE [dbo].[containers] DROP CONSTRAINT [FK_Container_Parent];

DROP TABLE IF EXISTS [dbo].[notifications];
DROP TABLE IF EXISTS [dbo].[records];
DROP TABLE IF EXISTS [dbo].[containers];
DROP TABLE IF EXISTS [dbo].[applications];
