var pwd = args.Length > 0 ? args[0] : "SFM_Admin_New_2026!";
Console.WriteLine(BCrypt.Net.BCrypt.HashPassword(pwd));
