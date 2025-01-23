Use [HIA3_ST] -- Change this to your database

GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Apps]') AND type in (N'U'))
BEGIN
    CREATE TABLE Apps (
        appid BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        appcode VARCHAR(500) NOT NULL,
        appname VARCHAR(500) NOT NULL,
        createdby BIGINT NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LocalUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE LocalUsers (
        localuserid BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        password VARCHAR(500) NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Logs]') AND type in (N'U'))
BEGIN
    CREATE TABLE Logs (
        logid BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        date DATETIME NOT NULL,
        logevent VARCHAR(500) NOT NULL,
        userid BIGINT NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
    CREATE TABLE Roles (
        roleid BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        rolecode VARCHAR(500) NOT NULL,
        rolename VARCHAR(500) NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE Users (
        userid BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        username VARCHAR(500) NOT NULL,
        firstname VARCHAR(500) NOT NULL,
        lastname VARCHAR(500) NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppUserRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE AppUserRoles (
        appuserroleid BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        appid BIGINT,
        roleid BIGINT,
        userid BIGINT
    );
END

ALTER TABLE AppUserRoles ADD CONSTRAINT Apps_appid_fk FOREIGN KEY (appid) REFERENCES Apps (appid);
ALTER TABLE AppUserRoles ADD CONSTRAINT Roles_roleid_fk FOREIGN KEY (roleid) REFERENCES Roles (roleid);
ALTER TABLE AppUserRoles ADD CONSTRAINT Users_userid_fk FOREIGN KEY (userid) REFERENCES Users (userid);

ALTER TABLE Apps ADD CONSTRAINT Apps_createdby_fk FOREIGN KEY (createdby) REFERENCES Users (userid);
ALTER TABLE LocalUsers ADD CONSTRAINT LocalUsers_localuserid_fk FOREIGN KEY (localuserid) REFERENCES Users (userid);
ALTER TABLE Logs ADD CONSTRAINT Logs_userid_fk FOREIGN KEY (userid) REFERENCES Users (userid);

GO

GO

INSERT INTO Users (username, firstname, lastname)
VALUES
('jdoe', 'John', 'Doe'),
('asmith', 'Alice', 'Smith'),
('bwilliams', 'Bob', 'Williams'),
('cjohnson', 'Carol', 'Johnson');

INSERT INTO Apps (appcode, appname, createdby)
VALUES
('APP001', 'Application One', 1),
('APP002', 'Application Two', 2),
('APP003', 'Application Three', 3);

INSERT INTO Roles (rolecode, rolename)
VALUES
('ADMIN', 'Administrator'),
('APPADMIN', 'App Administrator'),
('USER', 'User'),
('ST', 'Student'),
('TE', 'Teacher');

INSERT INTO LocalUsers (password)
VALUES
('password123'),
('securepassword'),
('mypassword');

INSERT INTO Logs (date, logevent, userid)
VALUES
(GETDATE(), 'User logged in', 1),
(GETDATE(), 'User updated profile', 2),
(GETDATE(), 'User logged out', 3);

GO