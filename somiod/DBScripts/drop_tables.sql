ALTER TABLE [dbo].[Notifications] DROP CONSTRAINT [FK_Notification_Parent];
ALTER TABLE [dbo].[Records] DROP CONSTRAINT [FK_Record_Parent];
ALTER TABLE [dbo].[Containers] DROP CONSTRAINT [FK_Container_Parent];

DROP TABLE IF EXISTS [dbo].[Notifications];
DROP TABLE IF EXISTS [dbo].[Records];
DROP TABLE IF EXISTS [dbo].[Containers];
DROP TABLE IF EXISTS [dbo].[Applications];
