IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Pallets] (
    [Id] nvarchar(450) NOT NULL,
    [State] int NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Pallets] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Shelves] (
    [Id] nvarchar(450) NOT NULL,
    [PalletId] nvarchar(450) NULL,
    [Location] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Shelves] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Shelves_Pallets_PalletId] FOREIGN KEY ([PalletId]) REFERENCES [Pallets] ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Location', N'State') AND [object_id] = OBJECT_ID(N'[Pallets]'))
    SET IDENTITY_INSERT [Pallets] ON;
INSERT INTO [Pallets] ([Id], [Location], [State])
VALUES (N'P-0001', N'Warehouse A', 2),
(N'P-0002', N'Warehouse A', 2),
(N'P-0003', N'Warehouse A', 0),
(N'P-0004', N'Warehouse A', 1),
(N'P-0005', N'Warehouse A', 3);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Location', N'State') AND [object_id] = OBJECT_ID(N'[Pallets]'))
    SET IDENTITY_INSERT [Pallets] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Location', N'PalletId') AND [object_id] = OBJECT_ID(N'[Shelves]'))
    SET IDENTITY_INSERT [Shelves] ON;
INSERT INTO [Shelves] ([Id], [Location], [PalletId])
VALUES (N'S-0003', N'Warehouse A', NULL),
(N'S-0004', N'Warehouse A', NULL),
(N'S-0005', N'Warehouse A', NULL),
(N'S-0001', N'Warehouse A', N'P-0001'),
(N'S-0002', N'Warehouse A', N'P-0002');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Location', N'PalletId') AND [object_id] = OBJECT_ID(N'[Shelves]'))
    SET IDENTITY_INSERT [Shelves] OFF;
GO

CREATE UNIQUE INDEX [IX_Shelves_PalletId] ON [Shelves] ([PalletId]) WHERE [PalletId] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231126145831_Shelf_And_Pallete_Tables_Created', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Users] (
    [Id] nvarchar(450) NOT NULL,
    [UserType] int NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Passcode] nvarchar(max) NOT NULL,
    [ForkliftCertified] bit NOT NULL,
    [IncorrectPalletPlacements] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'FirstName', N'ForkliftCertified', N'IncorrectPalletPlacements', N'LastName', N'Passcode', N'UserType') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [FirstName], [ForkliftCertified], [IncorrectPalletPlacements], [LastName], [Passcode], [UserType])
VALUES (N'U-0001', N'Kacper', CAST(1 AS bit), 0, N'Wroblewski', N'245tbgt', 1),
(N'U-0002', N'Nikita', CAST(1 AS bit), 13, N'Fedans', N'245tbgt', 0),
(N'U-0003', N'Teodor', CAST(1 AS bit), 3, N'Donchev', N'245tbgt', 0),
(N'U-0004', N'Vincent', CAST(0 AS bit), 0, N'Arellano', N'245tbgt', 0),
(N'U-0005', N'Kyle', CAST(0 AS bit), 0, N'McQuillan', N'245tbgt', 0),
(N'U-0006', N'Siya', CAST(0 AS bit), 0, N'Salekar', N'245tbgt', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'FirstName', N'ForkliftCertified', N'IncorrectPalletPlacements', N'LastName', N'Passcode', N'UserType') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231130123101_Created_User_Class', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Forklifts] (
    [Id] nvarchar(450) NOT NULL,
    [LastUserId] nvarchar(450) NULL,
    [LastPalletId] nvarchar(450) NULL,
    CONSTRAINT [PK_Forklifts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Forklifts_Pallets_LastPalletId] FOREIGN KEY ([LastPalletId]) REFERENCES [Pallets] ([Id]),
    CONSTRAINT [FK_Forklifts_Users_LastUserId] FOREIGN KEY ([LastUserId]) REFERENCES [Users] ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'LastPalletId', N'LastUserId') AND [object_id] = OBJECT_ID(N'[Forklifts]'))
    SET IDENTITY_INSERT [Forklifts] ON;
INSERT INTO [Forklifts] ([Id], [LastPalletId], [LastUserId])
VALUES (N'F-0007', N'P-0003', N'U-0002'),
(N'F-0012', N'P-0003', N'U-0001'),
(N'F-0016', N'P-0005', N'U-0003'),
(N'F-0205', N'P-0001', N'U-0003');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'LastPalletId', N'LastUserId') AND [object_id] = OBJECT_ID(N'[Forklifts]'))
    SET IDENTITY_INSERT [Forklifts] OFF;
GO

CREATE INDEX [IX_Forklifts_LastPalletId] ON [Forklifts] ([LastPalletId]);
GO

CREATE INDEX [IX_Forklifts_LastUserId] ON [Forklifts] ([LastUserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231130141844_Created_Forklift_Class', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [PalletTrackingLog] (
    [Id] int NOT NULL IDENTITY,
    [DateTime] datetime2 NOT NULL,
    [Action] nvarchar(max) NOT NULL,
    [PalletId] nvarchar(450) NOT NULL,
    [PalletState] int NOT NULL,
    [PalletLocation] nvarchar(max) NOT NULL,
    [ForkliftId] nvarchar(450) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_PalletTrackingLog] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PalletTrackingLog_Forklifts_ForkliftId] FOREIGN KEY ([ForkliftId]) REFERENCES [Forklifts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PalletTrackingLog_Pallets_PalletId] FOREIGN KEY ([PalletId]) REFERENCES [Pallets] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PalletTrackingLog_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Action', N'DateTime', N'ForkliftId', N'PalletId', N'PalletLocation', N'PalletState', N'UserId') AND [object_id] = OBJECT_ID(N'[PalletTrackingLog]'))
    SET IDENTITY_INSERT [PalletTrackingLog] ON;
INSERT INTO [PalletTrackingLog] ([Id], [Action], [DateTime], [ForkliftId], [PalletId], [PalletLocation], [PalletState], [UserId])
VALUES (1, N'Forklift F-0012 placed pallet P-0001 on shelf S-0001 in Warehouse A by user U-0001', '2024-01-21T16:03:42.6388464Z', N'F-0012', N'P-0001', N'Warehouse A', 2, N'U-0001'),
(2, N'Forklift F-0007 placed pallet P-0002 on the floor in Warehouse B by user U-0002', '2024-01-21T16:03:42.6388467Z', N'F-0007', N'P-0002', N'Warehouse B', 0, N'U-0002');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Action', N'DateTime', N'ForkliftId', N'PalletId', N'PalletLocation', N'PalletState', N'UserId') AND [object_id] = OBJECT_ID(N'[PalletTrackingLog]'))
    SET IDENTITY_INSERT [PalletTrackingLog] OFF;
GO

CREATE INDEX [IX_PalletTrackingLog_ForkliftId] ON [PalletTrackingLog] ([ForkliftId]);
GO

CREATE INDEX [IX_PalletTrackingLog_PalletId] ON [PalletTrackingLog] ([PalletId]);
GO

CREATE INDEX [IX_PalletTrackingLog_UserId] ON [PalletTrackingLog] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240121160343_Created_Pallet_Tracking_Log', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-01-23T14:43:11.9827128Z'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-01-23T14:43:11.9827131Z'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240123144312_Add_DbSet_For_PalletTrackingLog', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-03-31T16:38:13.8385236Z'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-03-31T16:38:13.8385239Z'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ=='
WHERE [Id] = N'U-0001';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ=='
WHERE [Id] = N'U-0002';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ=='
WHERE [Id] = N'U-0003';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ=='
WHERE [Id] = N'U-0004';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ=='
WHERE [Id] = N'U-0005';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ=='
WHERE [Id] = N'U-0006';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240331163814_Sample users with hashed passwords', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Users] ADD [CorrectPalletPlacements] int NOT NULL DEFAULT 0;
GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-03-31T16:46:34.7852977Z'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-03-31T16:46:34.7852984Z'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [CorrectPalletPlacements] = 0, [Passcode] = N'FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA=='
WHERE [Id] = N'U-0001';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [CorrectPalletPlacements] = 0, [Passcode] = N'FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA=='
WHERE [Id] = N'U-0002';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [CorrectPalletPlacements] = 0, [Passcode] = N'FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA=='
WHERE [Id] = N'U-0003';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [CorrectPalletPlacements] = 0, [Passcode] = N'FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA=='
WHERE [Id] = N'U-0004';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [CorrectPalletPlacements] = 0, [Passcode] = N'FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA=='
WHERE [Id] = N'U-0005';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [CorrectPalletPlacements] = 0, [Passcode] = N'FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA=='
WHERE [Id] = N'U-0006';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240331164635_Add CorrectPlacements column to users', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-03-31T16:48:08.1590945Z'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-03-31T16:48:08.1590950Z'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [CorrectPalletPlacements] = 999, [Passcode] = N'bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw=='
WHERE [Id] = N'U-0001';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw=='
WHERE [Id] = N'U-0002';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [CorrectPalletPlacements] = 3, [Passcode] = N'bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw=='
WHERE [Id] = N'U-0003';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw=='
WHERE [Id] = N'U-0004';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw=='
WHERE [Id] = N'U-0005';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw=='
WHERE [Id] = N'U-0006';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240331164808_Add CorrectPlacements column to users v2', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Users] ADD [ImageFilePath] nvarchar(max) NOT NULL DEFAULT N'';
GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-03-31T17:08:49.6772704Z'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-03-31T17:08:49.6772709Z'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [ImageFilePath] = N'', [Passcode] = N'zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw=='
WHERE [Id] = N'U-0001';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [ImageFilePath] = N'', [Passcode] = N'zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw=='
WHERE [Id] = N'U-0002';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [ImageFilePath] = N'', [Passcode] = N'zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw=='
WHERE [Id] = N'U-0003';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [ImageFilePath] = N'', [Passcode] = N'zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw=='
WHERE [Id] = N'U-0004';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [ImageFilePath] = N'', [Passcode] = N'zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw=='
WHERE [Id] = N'U-0005';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [ImageFilePath] = N'', [Passcode] = N'zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw=='
WHERE [Id] = N'U-0006';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240331170850_Add image file path to users', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-04-01T16:03:33.9873391Z'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [PalletTrackingLog] SET [DateTime] = '2024-04-01T16:03:33.9873396Z'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ=='
WHERE [Id] = N'U-0001';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ=='
WHERE [Id] = N'U-0002';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ=='
WHERE [Id] = N'U-0003';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ=='
WHERE [Id] = N'U-0004';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ=='
WHERE [Id] = N'U-0005';
SELECT @@ROWCOUNT;

GO

UPDATE [Users] SET [Passcode] = N'DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ=='
WHERE [Id] = N'U-0006';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240401160334_test', N'8.0.0');
GO

COMMIT;
GO

