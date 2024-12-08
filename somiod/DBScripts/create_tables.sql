CREATE TABLE [dbo].[applications]
(
	[id] INT IDENTITY(1, 1) NOT NULL,
	[name] NVARCHAR (50) NOT NULL,
	[creation_datetime] SMALLDATETIME NOT NULL,
	CONSTRAINT [PK_Application_Id] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UC_Application_Name] UNIQUE NONCLUSTERED ([name] ASC),
)

CREATE TABLE [dbo].[containers]
(
	[id] INT IDENTITY(1, 1) NOT NULL,
	[name] NVARCHAR (50) NOT NULL,
	[creation_datetime] SMALLDATETIME NOT NULL,
	[parent] INT NOT NULL,
	CONSTRAINT [PK_Container_Id] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_Container_Parent] FOREIGN KEY (parent) REFERENCES applications(id) ON DELETE CASCADE,
    CONSTRAINT [UC_Container_Name] UNIQUE NONCLUSTERED ([name] ASC),
)

CREATE TABLE [dbo].[records]
(
	[id] INT IDENTITY(1, 1) NOT NULL,
	[name] NVARCHAR (100) NOT NULL,
	[creation_datetime] SMALLDATETIME NOT NULL,
	[parent] INT NOT NULL,
	[content] NVARCHAR (MAX) NOT NULL,
	CONSTRAINT [PK_Record_Id] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_Record_Parent] FOREIGN KEY (parent) REFERENCES containers(id) ON DELETE CASCADE,
	CONSTRAINT [UC_Record_Name] UNIQUE NONCLUSTERED ([name] ASC),
)

CREATE TABLE [dbo].[notifications]
(
	[id] INT IDENTITY(1, 1) NOT NULL,
	[name] NVARCHAR (50) NOT NULL,
	[creation_datetime] SMALLDATETIME NOT NULL,
	[parent] INT NOT NULL,
	[event_type] CHAR NOT NULL,
	[endpoint] NVARCHAR (150) NOT NULL,
	[enabled] BIT NOT NULL DEFAULT 1,
	CONSTRAINT [PK_Notification_Id] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_Notification_Parent] FOREIGN KEY (parent) REFERENCES containers(id) ON DELETE CASCADE,
	CONSTRAINT [UC_Notification_Name] UNIQUE NONCLUSTERED ([name] ASC),
	CONSTRAINT [CHK_Notification_Event] CHECK ([event_type] IN ('0', '1', '2')),
	CONSTRAINT [CHK_Notification_Enabled] CHECK ([enabled] IN (0, 1)),
)
