


CREATE TABLE [Auths](
"Id"  integer Primary Key AutoIncrement not null,
"UserId" int,
"MenuType" int,
"ButtonType" int,
 unique(Id asc)
);
GO
Create index main.Auths_id on Auths (Id ASC);
GO

-- ----------------------------
-- Table structure for Users
-- Date: 2011-11-11
-- ----------------------------
CREATE TABLE [Users](
"Id"  integer Primary Key AutoIncrement not null,
"UserName"  nvarchar(32),
"Display"  nvarchar(32),
"Password"  nvarchar(32),
"Sex" int,
"IStatu"  bit,
"UserType"  int,
"LoginOn"  datetime,
"CreateOn"  datetime,
"UpdateOn"  datetime,
 unique(Id asc)
);
GO
Create index main.Users_id on Users (Id ASC);
GO

-- ----------------------------
-- Table structure for Admins
-- Date: 2011-11-11
-- ----------------------------
CREATE TABLE [Admins](
"Id"  integer Primary Key AutoIncrement not null,
"Name"  nvarchar(255),
"Value"  nvarchar(255),
"CreateOn"  datetime,
"UpdateOn"  datetime,
 unique(Id asc)
);
GO
Create index main.Admins_id on Admins (Id ASC);
GO