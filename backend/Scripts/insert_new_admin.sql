-- Admin cusub: admin2@sfm.so / Password: SFM_Admin_New_2026!
-- SQL Server — ku ord database-ka SOMALIDB (ama magacaaga)

INSERT INTO [dbo].[Users] (
    [FullName],
    [Email],
    [PasswordHash],
    [Role],
    [IsVerified],
    [IsSuspended],
    [CreatedAt],
    [ProfileImageUrl]
)
VALUES (
    N'SFM Admin 2',
    N'admin2@sfm.so',
    N'$2a$11$sANG7pRiwfOwdUSZzE/El.w/roxyby64ofTNMvskCNpMWp9CotAli',
    2,
    1,
    0,
    SYSUTCDATETIME(),
    NULL
);

-- Hubi:
-- SELECT UserId, FullName, Email, Role FROM Users WHERE Email = N'admin2@sfm.so';
