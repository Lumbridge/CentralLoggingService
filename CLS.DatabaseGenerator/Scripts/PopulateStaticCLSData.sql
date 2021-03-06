SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL Serializable
GO
BEGIN TRANSACTION

PRINT(N'Drop constraints from [dbo].[AlertTriggerNodeOperator]')
ALTER TABLE [dbo].[AlertTriggerNodeOperator] NOCHECK CONSTRAINT [FK_AlertTriggerNodeStaticOperator_AlertTriggerNodeStaticType]

PRINT(N'Drop constraint FK_AlertTriggerNode_AlertTriggerNodeStaticOperator from [dbo].[AlertTriggerNode]')
ALTER TABLE [dbo].[AlertTriggerNode] NOCHECK CONSTRAINT [FK_AlertTriggerNode_AlertTriggerNodeStaticOperator]

PRINT(N'Drop constraint Log_fk0 from [dbo].[Log]')
ALTER TABLE [dbo].[Log] NOCHECK CONSTRAINT [Log_fk0]

PRINT(N'Drop constraint System_fk1 from [dbo].[PublishingSystem]')
ALTER TABLE [dbo].[PublishingSystem] NOCHECK CONSTRAINT [System_fk1]

PRINT(N'Drop constraint System_fk0 from [dbo].[PublishingSystem]')
ALTER TABLE [dbo].[PublishingSystem] NOCHECK CONSTRAINT [System_fk0]

PRINT(N'Drop constraint Subscriber_System_fk1 from [dbo].[Subscription]')
ALTER TABLE [dbo].[Subscription] NOCHECK CONSTRAINT [Subscriber_System_fk1]

PRINT(N'Add 4 rows to [dbo].[AlertTriggerNodeType]')
SET IDENTITY_INSERT [dbo].[AlertTriggerNodeType] ON
INSERT INTO [dbo].[AlertTriggerNodeType] ([Id], [Name]) VALUES (1, 'VariableName')
INSERT INTO [dbo].[AlertTriggerNodeType] ([Id], [Name]) VALUES (2, 'ComparisonOperator')
INSERT INTO [dbo].[AlertTriggerNodeType] ([Id], [Name]) VALUES (4, 'LogicalOperator')
INSERT INTO [dbo].[AlertTriggerNodeType] ([Id], [Name]) VALUES (5, 'DynamicVariable')
SET IDENTITY_INSERT [dbo].[AlertTriggerNodeType] OFF

PRINT(N'Add 3 rows to [dbo].[AlertType]')
SET IDENTITY_INSERT [dbo].[AlertType] ON
INSERT INTO [dbo].[AlertType] ([Id], [Name], [Code]) VALUES (1, 'Email', 'E')
INSERT INTO [dbo].[AlertType] ([Id], [Name], [Code]) VALUES (2, 'Text Alert', 'T')
INSERT INTO [dbo].[AlertType] ([Id], [Name], [Code]) VALUES (3, 'None', 'N')
SET IDENTITY_INSERT [dbo].[AlertType] OFF

PRINT(N'Add 4 rows to [dbo].[EnvironmentType]')
SET IDENTITY_INSERT [dbo].[EnvironmentType] ON
INSERT INTO [dbo].[EnvironmentType] ([Id], [Name], [Code]) VALUES (1, 'DEV', 'D')
INSERT INTO [dbo].[EnvironmentType] ([Id], [Name], [Code]) VALUES (2, 'SIT', 'S')
INSERT INTO [dbo].[EnvironmentType] ([Id], [Name], [Code]) VALUES (3, 'UAT', 'U')
INSERT INTO [dbo].[EnvironmentType] ([Id], [Name], [Code]) VALUES (4, 'LIVE', 'L')
SET IDENTITY_INSERT [dbo].[EnvironmentType] OFF

PRINT(N'Add 6 rows to [dbo].[PublishingSystemType]')
SET IDENTITY_INSERT [dbo].[PublishingSystemType] ON
INSERT INTO [dbo].[PublishingSystemType] ([Id], [Name], [Code]) VALUES (1, 'REST', 'R')
INSERT INTO [dbo].[PublishingSystemType] ([Id], [Name], [Code]) VALUES (2, 'SOAP', 'S')
INSERT INTO [dbo].[PublishingSystemType] ([Id], [Name], [Code]) VALUES (3, 'Website', 'W')
INSERT INTO [dbo].[PublishingSystemType] ([Id], [Name], [Code]) VALUES (4, 'WindowsService', 'V')
INSERT INTO [dbo].[PublishingSystemType] ([Id], [Name], [Code]) VALUES (1002, 'ConsoleApplication', 'C')
INSERT INTO [dbo].[PublishingSystemType] ([Id], [Name], [Code]) VALUES (1003, 'Other', 'O')
SET IDENTITY_INSERT [dbo].[PublishingSystemType] OFF

PRINT(N'Add 5 rows to [dbo].[Severity]')
SET IDENTITY_INSERT [dbo].[Severity] ON
INSERT INTO [dbo].[Severity] ([Id], [Name], [Code]) VALUES (1, 'Debug', 'D')
INSERT INTO [dbo].[Severity] ([Id], [Name], [Code]) VALUES (2, 'Info', 'I')
INSERT INTO [dbo].[Severity] ([Id], [Name], [Code]) VALUES (3, 'Warn', 'W')
INSERT INTO [dbo].[Severity] ([Id], [Name], [Code]) VALUES (4, 'Error', 'E')
INSERT INTO [dbo].[Severity] ([Id], [Name], [Code]) VALUES (5, 'Fatal', 'F')
SET IDENTITY_INSERT [dbo].[Severity] OFF

PRINT(N'Add 15 rows to [dbo].[AlertTriggerNodeOperator]')
SET IDENTITY_INSERT [dbo].[AlertTriggerNodeOperator] ON
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (1, 2, '==', 'equal to', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (2, 2, '!=', 'not equal to', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (3, 2, '>', 'greater than', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (4, 2, '>=', 'greater than or equal to', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (5, 2, '<', 'less than', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (6, 2, '<=', 'less than or equal to', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (7, 4, '&&', 'and', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (8, 4, '||', 'or', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (9, 1, 'TimeOfDay', 'the time of day a message was received', 'Timestamp.TimeOfDay')
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (10, 1, 'MessageSeverity', 'the severity of a message i.e. info, error, fatal', 'Severity.Name')
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (11, 1, 'PublishingSystemName', 'the source of the message', 'PublishingSystem.Name')
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (12, 1, 'EnvironmentType', 'the environment type of the source the message', 'PublishingSystem.EnvironmentType.Name')
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (14, 5, 'NumberOfMessages', 'the number of messages in a rolling time window i.e. 100 messages in 60 minutes would trigger an alert', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (16, 5, 'TimeWindowMinutes', 'the size of the rolling time window in minutes', NULL)
INSERT INTO [dbo].[AlertTriggerNodeOperator] ([Id], [AlertTriggerNodeTypeId], [Value], [Description], [DotNetProperty]) VALUES (17, 1, 'DayOfWeek', 'the day of the week that the message was received', 'TimeStamp.DayOfWeek.ToString()')
SET IDENTITY_INSERT [dbo].[AlertTriggerNodeOperator] OFF

PRINT(N'Add constraints to [dbo].[AlertTriggerNodeOperator]')
ALTER TABLE [dbo].[AlertTriggerNodeOperator] WITH CHECK CHECK CONSTRAINT [FK_AlertTriggerNodeStaticOperator_AlertTriggerNodeStaticType]
ALTER TABLE [dbo].[AlertTriggerNode] WITH CHECK CHECK CONSTRAINT [FK_AlertTriggerNode_AlertTriggerNodeStaticOperator]
ALTER TABLE [dbo].[Log] WITH CHECK CHECK CONSTRAINT [Log_fk0]
ALTER TABLE [dbo].[PublishingSystem] WITH CHECK CHECK CONSTRAINT [System_fk1]
ALTER TABLE [dbo].[PublishingSystem] WITH CHECK CHECK CONSTRAINT [System_fk0]
ALTER TABLE [dbo].[Subscription] WITH CHECK CHECK CONSTRAINT [Subscriber_System_fk1]
COMMIT TRANSACTION
GO