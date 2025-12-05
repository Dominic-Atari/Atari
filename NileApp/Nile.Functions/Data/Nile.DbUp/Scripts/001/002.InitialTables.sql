-- Initial Tables for Nile Social Media Application

-- Users Table
CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    FirstName NVARCHAR(100) NULL,
    LastName NVARCHAR(100) NULL,
    PhoneNumber NVARCHAR(50) NULL,
    Bio NVARCHAR(500) NULL,
    Location NVARCHAR(200) NULL,
    ProfileImageUrl NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedAt DATETIME2 NULL
);

-- Account Table
CREATE TABLE Schools (
                         SchoolId INT IDENTITY(1,1) PRIMARY KEY,
                         SchoolName NVARCHAR(100) NOT NULL,
                         SchoolAddress NVARCHAR(200) NOT NULL,
                         City NVARCHAR(100) NOT NULL,
                         State NVARCHAR(100) NOT NULL,
                         Country NVARCHAR(100) NOT NULL,
                         County NVARCHAR(100) NULL,
                         ZipCode NVARCHAR(50) NULL,
                         PhoneNumber NVARCHAR(50) NULL,
                         Email NVARCHAR(255) NULL,
                         Description NVARCHAR(500) NULL,
                         ImageUrl NVARCHAR(500) NULL,
                         CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                         UpdatedAt DATETIME2 NULL,
                         IsDeleted BIT NOT NULL DEFAULT 0,
                         DeletedAt DATETIME2 NULL,
                         UserId UNIQUEIDENTIFIER NOT NULL,
                         CONSTRAINT FK_Schools_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);


-- Passwords Table
CREATE TABLE Passwords (
    UserId UNIQUEIDENTIFIER NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    CONSTRAINT PK_Passwords PRIMARY KEY (UserId),
    CONSTRAINT FK_Passwords_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- EmailConfirmations Table
CREATE TABLE EmailConfirmations (
                                    ConfirmationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                                    UserId UNIQUEIDENTIFIER NOT NULL,
                                    ConfirmationToken NVARCHAR(MAX) NOT NULL,
                                    IsConfirmed BIT NOT NULL DEFAULT 0,
                                    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                                    ExpiresAt DATETIME2 NULL,
                                    CONSTRAINT FK_EmailConfirmations_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)

);

-- ResetPasswordTokens Table
CREATE TABLE ResetPasswordTokens (
                                     TokenId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                                     UserId UNIQUEIDENTIFIER NOT NULL,
                                     Token NVARCHAR(MAX) NOT NULL,
                                     ExpiresAt DATETIME2 NOT NULL,
                                     CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                                     CONSTRAINT FK_ResetPasswordTokens_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)

);

-- Role
CREATE TABLE Role(
    RoleId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RoleName NVARCHAR(20) NOT NULL
);

-- UserRole Table (Many-to-Many between Users and Roles)
CREATE TABLE UserRole (
    UserId UNIQUEIDENTIFIER NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT PK_UserRole PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRole_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_UserRole_Role FOREIGN KEY (RoleId) REFERENCES Role(RoleId)
);

-- Posts Table
CREATE TABLE Posts (
    PostId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(200) NULL,
    Content NVARCHAR(MAX) NULL,
    ImageUrl NVARCHAR(500) NULL,
    PostType NVARCHAR(2000) NOT NULL DEFAULT 'Text', -- Text, Image, Video, Recipe
    LikesCount INT NOT NULL DEFAULT 0,
    CommentsCount INT NOT NULL DEFAULT 0,
    SharesCount INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    CONSTRAINT FK_Posts_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Comments Table
CREATE TABLE Comments (
    CommentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PostId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    Content NVARCHAR(1000) NOT NULL,
    ParentCommentId UNIQUEIDENTIFIER NULL, -- For nested comments/replies
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    CONSTRAINT FK_Comments_Posts FOREIGN KEY (PostId) REFERENCES Posts(PostId),
    CONSTRAINT FK_Comments_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Comments_ParentComment FOREIGN KEY (ParentCommentId) REFERENCES Comments(CommentId)
);

-- Likes Table
CREATE TABLE Likes (
    LikeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PostId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Likes_Posts FOREIGN KEY (PostId) REFERENCES Posts(PostId),
    CONSTRAINT FK_Likes_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT UQ_Likes_PostUser UNIQUE (PostId, UserId)
);

-- UserRelationships Table (Followers/Following)
CREATE TABLE UserRelationships (
    RelationshipId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FollowerUserId UNIQUEIDENTIFIER NOT NULL,
    FollowingUserId UNIQUEIDENTIFIER NOT NULL,
    RelationshipType INT NOT NULL DEFAULT 0, -- Follow, Block, Friend
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_UserRelationships_Follower FOREIGN KEY (FollowerUserId) REFERENCES Users(UserId),
    CONSTRAINT FK_UserRelationships_Following FOREIGN KEY (FollowingUserId) REFERENCES Users(UserId),
    CONSTRAINT UQ_UserRelationships UNIQUE (FollowerUserId, FollowingUserId, RelationshipType)
);

-- Notifications Table
CREATE TABLE Notifications (
    NotificationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    ActorUserId UNIQUEIDENTIFIER NULL, -- User who triggered the notification
    NotificationType NVARCHAR(50) NOT NULL, -- Like, Comment, Follow, Mention
    EntityType NVARCHAR(50) NULL, -- Post, Comment, User
    EntityId UNIQUEIDENTIFIER NULL,
    Message NVARCHAR(500) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Notifications_Actor FOREIGN KEY (ActorUserId) REFERENCES Users(UserId)
);

-- Create Indexes for Performance
CREATE INDEX IX_Posts_UserId ON Posts(UserId);
CREATE INDEX IX_Posts_CreatedAt ON Posts(CreatedAt DESC);
CREATE INDEX IX_Comments_PostId ON Comments(PostId);
CREATE INDEX IX_Comments_UserId ON Comments(UserId);
CREATE INDEX IX_Likes_PostId ON Likes(PostId);
CREATE INDEX IX_Likes_UserId ON Likes(UserId);
CREATE INDEX IX_UserRelationships_Follower ON UserRelationships(FollowerUserId);
CREATE INDEX IX_UserRelationships_Following ON UserRelationships(FollowingUserId);
CREATE INDEX IX_Notifications_UserId ON Notifications(UserId);
CREATE INDEX IX_Notifications_IsRead ON Notifications(IsRead);

PRINT 'Initial tables created successfully.';
GO
