SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL Serializable
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AlertTriggerGroup]'
GO
CREATE TABLE [dbo].[AlertTriggerGroup]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[UserId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_AlertTriggerGroup] on [dbo].[AlertTriggerGroup]'
GO
ALTER TABLE [dbo].[AlertTriggerGroup] ADD CONSTRAINT [PK_AlertTriggerGroup] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AlertHistory]'
GO
CREATE TABLE [dbo].[AlertHistory]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[AlertHistoryGroupId] [int] NOT NULL,
[LogId] [int] NOT NULL,
[UserId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AlertTriggerGroupId] [int] NOT NULL,
[Timestamp] [datetime] NOT NULL,
[SiblingCount] [int] NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_AlertHistory] on [dbo].[AlertHistory]'
GO
ALTER TABLE [dbo].[AlertHistory] ADD CONSTRAINT [PK_AlertHistory] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Log]'
GO
CREATE TABLE [dbo].[Log]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[UserId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SeverityId] [int] NOT NULL,
[PublishingSystemId] [int] NOT NULL,
[Timestamp] [datetime] NOT NULL,
[Message] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Exception] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StackTrace] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_LOG] on [dbo].[Log]'
GO
ALTER TABLE [dbo].[Log] ADD CONSTRAINT [PK_LOG] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AspNetUsers]'
GO
CREATE TABLE [dbo].[AspNetUsers]
(
[Id] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailConfirmed] [bit] NOT NULL,
[PasswordHash] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SecurityStamp] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PhoneNumber] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PhoneNumberConfirmed] [bit] NOT NULL,
[TwoFactorEnabled] [bit] NOT NULL,
[LockoutEndDateUtc] [datetime] NULL,
[LockoutEnabled] [bit] NOT NULL,
[AccessFailedCount] [int] NOT NULL,
[UserName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_dbo.AspNetUsers] on [dbo].[AspNetUsers]'
GO
ALTER TABLE [dbo].[AspNetUsers] ADD CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating index [UserNameIndex] on [dbo].[AspNetUsers]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers] ([UserName])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AlertTriggerNode]'
GO
CREATE TABLE [dbo].[AlertTriggerNode]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[AlertTriggerNodeOperatorId] [int] NULL,
[AlertTriggerGroupId] [int] NOT NULL,
[DynamicNodeValue] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DynamicNodeDotNetDataType] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PositionInGroup] [int] NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_AlertTriggerNode] on [dbo].[AlertTriggerNode]'
GO
ALTER TABLE [dbo].[AlertTriggerNode] ADD CONSTRAINT [PK_AlertTriggerNode] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AlertTriggerNodeOperator]'
GO
CREATE TABLE [dbo].[AlertTriggerNodeOperator]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[AlertTriggerNodeTypeId] [int] NOT NULL,
[Value] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DotNetProperty] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_AlertTriggerNodeStaticOperator] on [dbo].[AlertTriggerNodeOperator]'
GO
ALTER TABLE [dbo].[AlertTriggerNodeOperator] ADD CONSTRAINT [PK_AlertTriggerNodeStaticOperator] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AlertTriggerNodeType]'
GO
CREATE TABLE [dbo].[AlertTriggerNodeType]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_AlertTriggerNodeType] on [dbo].[AlertTriggerNodeType]'
GO
ALTER TABLE [dbo].[AlertTriggerNodeType] ADD CONSTRAINT [PK_AlertTriggerNodeType] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AspNetUserClaims]'
GO
CREATE TABLE [dbo].[AspNetUserClaims]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[UserId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClaimType] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClaimValue] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_dbo.AspNetUserClaims] on [dbo].[AspNetUserClaims]'
GO
ALTER TABLE [dbo].[AspNetUserClaims] ADD CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating index [IX_UserId] on [dbo].[AspNetUserClaims]'
GO
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims] ([UserId])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AspNetUserLogins]'
GO
CREATE TABLE [dbo].[AspNetUserLogins]
(
[LoginProvider] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProviderKey] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UserId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_dbo.AspNetUserLogins] on [dbo].[AspNetUserLogins]'
GO
ALTER TABLE [dbo].[AspNetUserLogins] ADD CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED  ([LoginProvider], [ProviderKey], [UserId])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating index [IX_UserId] on [dbo].[AspNetUserLogins]'
GO
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins] ([UserId])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AspNetRoles]'
GO
CREATE TABLE [dbo].[AspNetRoles]
(
[Id] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_dbo.AspNetRoles] on [dbo].[AspNetRoles]'
GO
ALTER TABLE [dbo].[AspNetRoles] ADD CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating index [RoleNameIndex] on [dbo].[AspNetRoles]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles] ([Name])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AspNetUserRoles]'
GO
CREATE TABLE [dbo].[AspNetUserRoles]
(
[UserId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RoleId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_dbo.AspNetUserRoles] on [dbo].[AspNetUserRoles]'
GO
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED  ([UserId], [RoleId])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating index [IX_RoleId] on [dbo].[AspNetUserRoles]'
GO
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles] ([RoleId])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating index [IX_UserId] on [dbo].[AspNetUserRoles]'
GO
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles] ([UserId])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[PublishingSystem]'
GO
CREATE TABLE [dbo].[PublishingSystem]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[UserId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EnvironmentTypeId] [int] NOT NULL,
[PublishingSystemTypeId] [int] NOT NULL,
[Name] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_SYSTEM] on [dbo].[PublishingSystem]'
GO
ALTER TABLE [dbo].[PublishingSystem] ADD CONSTRAINT [PK_SYSTEM] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Subscription]'
GO
CREATE TABLE [dbo].[Subscription]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[AlertTriggerGroupId] [int] NOT NULL,
[AlertTypeId] [int] NOT NULL,
[UserId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsActive] [bit] NOT NULL CONSTRAINT [DF_Subscriber_PublishingSystem_IsActive] DEFAULT ((1))
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_SUBSCRIBER_SYSTEM] on [dbo].[Subscription]'
GO
ALTER TABLE [dbo].[Subscription] ADD CONSTRAINT [PK_SUBSCRIBER_SYSTEM] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Severity]'
GO
CREATE TABLE [dbo].[Severity]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_LEVEL] on [dbo].[Severity]'
GO
ALTER TABLE [dbo].[Severity] ADD CONSTRAINT [PK_LEVEL] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding constraints to [dbo].[Severity]'
GO
ALTER TABLE [dbo].[Severity] ADD CONSTRAINT [UQ__Level__A25C5AA7E341B49C] UNIQUE NONCLUSTERED  ([Code])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding constraints to [dbo].[Severity]'
GO
ALTER TABLE [dbo].[Severity] ADD CONSTRAINT [UQ__Level__737584F67B111FCC] UNIQUE NONCLUSTERED  ([Name])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[AlertType]'
GO
CREATE TABLE [dbo].[AlertType]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_ALERTTYPE] on [dbo].[AlertType]'
GO
ALTER TABLE [dbo].[AlertType] ADD CONSTRAINT [PK_ALERTTYPE] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding constraints to [dbo].[AlertType]'
GO
ALTER TABLE [dbo].[AlertType] ADD CONSTRAINT [UQ__AlertTyp__A25C5AA76568F6AB] UNIQUE NONCLUSTERED  ([Code])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding constraints to [dbo].[AlertType]'
GO
ALTER TABLE [dbo].[AlertType] ADD CONSTRAINT [UQ__AlertTyp__737584F6679F4D20] UNIQUE NONCLUSTERED  ([Name])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[EnvironmentType]'
GO
CREATE TABLE [dbo].[EnvironmentType]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_ENVIRONMENTTYPE] on [dbo].[EnvironmentType]'
GO
ALTER TABLE [dbo].[EnvironmentType] ADD CONSTRAINT [PK_ENVIRONMENTTYPE] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding constraints to [dbo].[EnvironmentType]'
GO
ALTER TABLE [dbo].[EnvironmentType] ADD CONSTRAINT [UQ__Environm__A25C5AA71302B9B6] UNIQUE NONCLUSTERED  ([Code])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding constraints to [dbo].[EnvironmentType]'
GO
ALTER TABLE [dbo].[EnvironmentType] ADD CONSTRAINT [UQ__Environm__737584F6ABF0CD93] UNIQUE NONCLUSTERED  ([Name])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[PublishingSystemType]'
GO
CREATE TABLE [dbo].[PublishingSystemType]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_SYSTEMTYPE] on [dbo].[PublishingSystemType]'
GO
ALTER TABLE [dbo].[PublishingSystemType] ADD CONSTRAINT [PK_SYSTEMTYPE] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding constraints to [dbo].[PublishingSystemType]'
GO
ALTER TABLE [dbo].[PublishingSystemType] ADD CONSTRAINT [UQ__SystemTy__A25C5AA79B1173E2] UNIQUE NONCLUSTERED  ([Code])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding constraints to [dbo].[PublishingSystemType]'
GO
ALTER TABLE [dbo].[PublishingSystemType] ADD CONSTRAINT [UQ__SystemTy__737584F6D0ED661B] UNIQUE NONCLUSTERED  ([Name])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[DashboardMetadata]'
GO
CREATE TABLE [dbo].[DashboardMetadata]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[MetadataItemName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MetadataItemValue] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MetadataItemDotNetType] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TimeAdded] [datetime] NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_DASHBOARDMETADATA] on [dbo].[DashboardMetadata]'
GO
ALTER TABLE [dbo].[DashboardMetadata] ADD CONSTRAINT [PK_DASHBOARDMETADATA] PRIMARY KEY CLUSTERED  ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding constraints to [dbo].[DashboardMetadata]'
GO
ALTER TABLE [dbo].[DashboardMetadata] ADD CONSTRAINT [UQ__Dashboar__3F2F1DE30AE96CD1] UNIQUE NONCLUSTERED  ([MetadataItemName])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[__MigrationHistory]'
GO
CREATE TABLE [dbo].[__MigrationHistory]
(
[MigrationId] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ContextKey] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Model] [varbinary] (max) NOT NULL,
[ProductVersion] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_dbo.__MigrationHistory] on [dbo].[__MigrationHistory]'
GO
ALTER TABLE [dbo].[__MigrationHistory] ADD CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED  ([MigrationId], [ContextKey])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[AlertHistory]'
GO
ALTER TABLE [dbo].[AlertHistory] ADD CONSTRAINT [FK_AlertHistory_Log] FOREIGN KEY ([LogId]) REFERENCES [dbo].[Log] ([Id])
GO
ALTER TABLE [dbo].[AlertHistory] ADD CONSTRAINT [FK_AlertHistory_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AlertHistory] ADD CONSTRAINT [FK_AlertHistory_AlertTriggerGroup] FOREIGN KEY ([AlertTriggerGroupId]) REFERENCES [dbo].[AlertTriggerGroup] ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[AlertTriggerNode]'
GO
ALTER TABLE [dbo].[AlertTriggerNode] ADD CONSTRAINT [FK_AlertTriggerNode_AlertTriggerGroup] FOREIGN KEY ([AlertTriggerGroupId]) REFERENCES [dbo].[AlertTriggerGroup] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertTriggerNode] ADD CONSTRAINT [FK_AlertTriggerNode_AlertTriggerNodeStaticOperator] FOREIGN KEY ([AlertTriggerNodeOperatorId]) REFERENCES [dbo].[AlertTriggerNodeOperator] ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Subscription]'
GO
ALTER TABLE [dbo].[Subscription] ADD CONSTRAINT [FK_Subscription_AlertTriggerGroup] FOREIGN KEY ([AlertTriggerGroupId]) REFERENCES [dbo].[AlertTriggerGroup] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subscription] ADD CONSTRAINT [Subscriber_System_fk1] FOREIGN KEY ([AlertTypeId]) REFERENCES [dbo].[AlertType] ([Id]) ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Subscription] ADD CONSTRAINT [FK_Subscription_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[AlertTriggerGroup]'
GO
ALTER TABLE [dbo].[AlertTriggerGroup] ADD CONSTRAINT [FK_AlertTriggerGroup_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[AlertTriggerNodeOperator]'
GO
ALTER TABLE [dbo].[AlertTriggerNodeOperator] ADD CONSTRAINT [FK_AlertTriggerNodeStaticOperator_AlertTriggerNodeStaticType] FOREIGN KEY ([AlertTriggerNodeTypeId]) REFERENCES [dbo].[AlertTriggerNodeType] ([Id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[AspNetUserRoles]'
GO
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[AspNetUserClaims]'
GO
ALTER TABLE [dbo].[AspNetUserClaims] ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[AspNetUserLogins]'
GO
ALTER TABLE [dbo].[AspNetUserLogins] ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Log]'
GO
ALTER TABLE [dbo].[Log] ADD CONSTRAINT [FK_Log_CLSUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Log_fk0] FOREIGN KEY ([SeverityId]) REFERENCES [dbo].[Severity] ([Id]) ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Log] ADD CONSTRAINT [Log_fk1] FOREIGN KEY ([PublishingSystemId]) REFERENCES [dbo].[PublishingSystem] ([Id]) ON UPDATE CASCADE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[PublishingSystem]'
GO
ALTER TABLE [dbo].[PublishingSystem] ADD CONSTRAINT [FK_PublishingSystem_CLSUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[PublishingSystem] ADD CONSTRAINT [System_fk0] FOREIGN KEY ([EnvironmentTypeId]) REFERENCES [dbo].[EnvironmentType] ([Id]) ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[PublishingSystem] ADD CONSTRAINT [System_fk1] FOREIGN KEY ([PublishingSystemTypeId]) REFERENCES [dbo].[PublishingSystemType] ([Id]) ON UPDATE CASCADE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
-- This statement writes to the SQL Server Log so SQL Monitor can show this deployment.
IF HAS_PERMS_BY_NAME(N'sys.xp_logevent', N'OBJECT', N'EXECUTE') = 1
BEGIN
    DECLARE @databaseName AS nvarchar(2048), @eventMessage AS nvarchar(2048)
    SET @databaseName = REPLACE(REPLACE(DB_NAME(), N'\', N'\\'), N'"', N'\"')
    SET @eventMessage = N'Redgate SQL Compare: { "deployment": { "description": "Redgate SQL Compare deployed to ' + @databaseName + N'", "database": "' + @databaseName + N'" }}'
    EXECUTE sys.xp_logevent 55000, @eventMessage
END
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'The database update failed'
END
GO