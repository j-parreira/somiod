ALTER TABLE [dbo].[notifications] DROP CONSTRAINT [FK_Notification_Parent];
ALTER TABLE [dbo].[records] DROP CONSTRAINT [FK_Record_Parent];
ALTER TABLE [dbo].[containers] DROP CONSTRAINT [FK_Container_Parent];

DROP TABLE IF EXISTS [dbo].[notifications];
DROP TABLE IF EXISTS [dbo].[records];
DROP TABLE IF EXISTS [dbo].[containers];
DROP TABLE IF EXISTS [dbo].[applications];
