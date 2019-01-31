/*
Script de déploiement pour BeerCoupons

Ce code a été généré par un outil.
La modification de ce fichier peut provoquer un comportement incorrect et sera perdue si
le code est régénéré.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "BeerCoupons"
:setvar DefaultFilePrefix "BeerCoupons"
:setvar DefaultDataPath "C:\Users\gcuyl\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB"
:setvar DefaultLogPath "C:\Users\gcuyl\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB"

GO
:on error exit
GO
/*
Détectez le mode SQLCMD et désactivez l'exécution du script si le mode SQLCMD n'est pas pris en charge.
Pour réactiver le script une fois le mode SQLCMD activé, exécutez ce qui suit :
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'Le mode SQLCMD doit être activé de manière à pouvoir exécuter ce script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
/*
La colonne [dbo].[Bars].[Desc] est en cours de suppression, des données risquent d'être perdues.
*/

IF EXISTS (select top 1 1 from [dbo].[Bars])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
/*
La colonne [dbo].[Cities].[Desc] est en cours de suppression, des données risquent d'être perdues.
*/

IF EXISTS (select top 1 1 from [dbo].[Cities])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
/*
La colonne [dbo].[Coupons].[Desc] est en cours de suppression, des données risquent d'être perdues.

La colonne [dbo].[Coupons].[Description] de la table [dbo].[Coupons] doit être ajoutée mais la colonne ne comporte pas de valeur par défaut et n'autorise pas les valeurs NULL. Si la table contient des données, le script ALTER ne fonctionnera pas. Pour éviter ce problème, vous devez ajouter une valeur par défaut à la colonne, la marquer comme autorisant les valeurs Null ou activer la génération de smart-defaults en tant qu'option de déploiement.
*/

IF EXISTS (select top 1 1 from [dbo].[Coupons])
    RAISERROR (N'Lignes détectées. Arrêt de la mise à jour du schéma en raison d''''un risque de perte de données.', 16, 127) WITH NOWAIT

GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[Coupons]...';


GO
ALTER TABLE [dbo].[Coupons] DROP CONSTRAINT [DF__Coupons__StartDa__3C69FB99];


GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[Bars]...';


GO
ALTER TABLE [dbo].[Bars] DROP CONSTRAINT [FK__Bars__FkCityId__3D5E1FD2];


GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[Coupons]...';


GO
ALTER TABLE [dbo].[Coupons] DROP CONSTRAINT [FK__Coupons__FkBarId__3E52440B];


GO
PRINT N'Suppression de contrainte sans nom sur [dbo].[Users]...';


GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK__Users__FkBarId__4222D4EF];


GO
PRINT N'Début de la régénération de la table [dbo].[Bars]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Bars] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50)  NOT NULL,
    [Address]  VARCHAR (100) NULL,
    [About]    VARCHAR (50)  NULL,
    [TAGS]     VARCHAR (200) NULL,
    [BarPic]   VARCHAR (100) NULL,
    [FkCityId] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Bars])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Bars] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Bars] ([Id], [Name], [Address], [TAGS], [BarPic], [FkCityId])
        SELECT   [Id],
                 [Name],
                 [Address],
                 [TAGS],
                 [BarPic],
                 [FkCityId]
        FROM     [dbo].[Bars]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Bars] OFF;
    END

DROP TABLE [dbo].[Bars];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Bars]', N'Bars';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Début de la régénération de la table [dbo].[Cities]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Cities] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [PostCode]  INT           NOT NULL,
    [Name]      VARCHAR (50)  NOT NULL,
    [About]     VARCHAR (150) NULL,
    [BannerPic] VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([PostCode] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Cities])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Cities] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Cities] ([Id], [PostCode], [Name], [BannerPic])
        SELECT   [Id],
                 [PostCode],
                 [Name],
                 [BannerPic]
        FROM     [dbo].[Cities]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Cities] OFF;
    END

DROP TABLE [dbo].[Cities];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Cities]', N'Cities';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Début de la régénération de la table [dbo].[Coupons]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Coupons] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    [BarCode]     VARCHAR (13) NOT NULL,
    [StartDate]   DATETIME     DEFAULT GETDATE() NOT NULL,
    [EndDate]     DATETIME     NOT NULL,
    [FkBarId]     INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Coupons])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Coupons] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Coupons] ([Id], [BarCode], [StartDate], [EndDate], [FkBarId])
        SELECT   [Id],
                 [BarCode],
                 [StartDate],
                 [EndDate],
                 [FkBarId]
        FROM     [dbo].[Coupons]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Coupons] OFF;
    END

DROP TABLE [dbo].[Coupons];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Coupons]', N'Coupons';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Début de la régénération de la table [dbo].[Users]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Users] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (20) NOT NULL,
    [Email]     VARCHAR (50) NOT NULL,
    [Password]  VARCHAR (50) NOT NULL,
    [FkBarId]   INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Users])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Users] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Users] ([Id], [FirstName], [Email], [Password], [FkBarId])
        SELECT   [Id],
                 [FirstName],
                 [Email],
                 [Password],
                 [FkBarId]
        FROM     [dbo].[Users]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Users] OFF;
    END

DROP TABLE [dbo].[Users];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Users]', N'Users';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Création de contrainte sans nom sur [dbo].[Bars]...';


GO
ALTER TABLE [dbo].[Bars] WITH NOCHECK
    ADD FOREIGN KEY ([FkCityId]) REFERENCES [dbo].[Cities] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[Coupons]...';


GO
ALTER TABLE [dbo].[Coupons] WITH NOCHECK
    ADD FOREIGN KEY ([FkBarId]) REFERENCES [dbo].[Bars] ([Id]);


GO
PRINT N'Création de contrainte sans nom sur [dbo].[Users]...';


GO
ALTER TABLE [dbo].[Users] WITH NOCHECK
    ADD FOREIGN KEY ([FkBarId]) REFERENCES [dbo].[Bars] ([Id]);


GO
PRINT N'Vérification de données existantes par rapport aux nouvelles contraintes';


GO
USE [$(DatabaseName)];


GO
CREATE TABLE [#__checkStatus] (
    id           INT            IDENTITY (1, 1) PRIMARY KEY CLUSTERED,
    [Schema]     NVARCHAR (256),
    [Table]      NVARCHAR (256),
    [Constraint] NVARCHAR (256)
);

SET NOCOUNT ON;

DECLARE tableconstraintnames CURSOR LOCAL FORWARD_ONLY
    FOR SELECT SCHEMA_NAME([schema_id]),
               OBJECT_NAME([parent_object_id]),
               [name],
               0
        FROM   [sys].[objects]
        WHERE  [parent_object_id] IN (OBJECT_ID(N'dbo.Bars'), OBJECT_ID(N'dbo.Coupons'), OBJECT_ID(N'dbo.Users'))
               AND [type] IN (N'F', N'C')
                   AND [object_id] IN (SELECT [object_id]
                                       FROM   [sys].[check_constraints]
                                       WHERE  [is_not_trusted] <> 0
                                              AND [is_disabled] = 0
                                       UNION
                                       SELECT [object_id]
                                       FROM   [sys].[foreign_keys]
                                       WHERE  [is_not_trusted] <> 0
                                              AND [is_disabled] = 0);

DECLARE @schemaname AS NVARCHAR (256);

DECLARE @tablename AS NVARCHAR (256);

DECLARE @checkname AS NVARCHAR (256);

DECLARE @is_not_trusted AS INT;

DECLARE @statement AS NVARCHAR (1024);

BEGIN TRY
    OPEN tableconstraintnames;
    FETCH tableconstraintnames INTO @schemaname, @tablename, @checkname, @is_not_trusted;
    WHILE @@fetch_status = 0
        BEGIN
            PRINT N'Vérification de la contrainte : ' + @checkname + N' [' + @schemaname + N'].[' + @tablename + N']';
            SET @statement = N'ALTER TABLE [' + @schemaname + N'].[' + @tablename + N'] WITH ' + CASE @is_not_trusted WHEN 0 THEN N'CHECK' ELSE N'NOCHECK' END + N' CHECK CONSTRAINT [' + @checkname + N']';
            BEGIN TRY
                EXECUTE [sp_executesql] @statement;
            END TRY
            BEGIN CATCH
                INSERT  [#__checkStatus] ([Schema], [Table], [Constraint])
                VALUES                  (@schemaname, @tablename, @checkname);
            END CATCH
            FETCH tableconstraintnames INTO @schemaname, @tablename, @checkname, @is_not_trusted;
        END
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH

IF CURSOR_STATUS(N'LOCAL', N'tableconstraintnames') >= 0
    CLOSE tableconstraintnames;

IF CURSOR_STATUS(N'LOCAL', N'tableconstraintnames') = -1
    DEALLOCATE tableconstraintnames;

SELECT N'Échec de vérification de la contrainte :' + [Schema] + N'.' + [Table] + N',' + [Constraint]
FROM   [#__checkStatus];

IF @@ROWCOUNT > 0
    BEGIN
        DROP TABLE [#__checkStatus];
        RAISERROR (N'Une erreur s''est produite lors de la vérification des contraintes', 16, 127);
    END

SET NOCOUNT OFF;

DROP TABLE [#__checkStatus];


GO
PRINT N'Mise à jour terminée.';


GO
