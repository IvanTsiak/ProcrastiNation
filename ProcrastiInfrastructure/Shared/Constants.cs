using Npgsql.PostgresTypes;

namespace ProcrastiInfrastructure.Shared
{
    public static class Constants
    {
        public static class Limits
        {
            public const int TopComents = 5;
            public const int TopUsers = 3;
            public const int TopActivities = 6;

            public const int LatestLogs = 4;
            public const int LogsInHome = 100;
            public const int LogsImportMax = 50;
            public const int ModerateTimeAmount = 720;

            public const int NotifReplyTextLen = 30;
            public const int NotificationLimit = 30;

            public const int CheckBanFreqMinutes = 2;
        }

        public static class LogTypes
        {
            public const string WinText = "Win";
            public const string LossText = "Loss";
        }
        public static class Achievements
        {
            public const string PROcrastinator = "PROcrastinator";
            public const string ProProCrastinator = "PROPROCRASTINATOR";

            public const string Survivor = "SURVIVOR";
            public const int SurvivorTimeSeconds = 300;

            public const string ProductivityIllusion = "PRODUCTIVITY_ILLUSION";
            public const string Coward = "COWARD";
            public const string DataSmuggler = "DATA_SMUGGLER";
            public const string RickRoll = "NGGYU";
            public const string RickRollAchivementName = "Rick Astley";
            public const string Intelectual = "INTELLECTUAL";
        }

        public static class Unknown
        {
            public const string UnkUserFunny = "Хрін зна хто";
            public const string UnkUser = "Анонім";
            public const string UnkActivity = "Невідома активність";
            public const string UnkCategory = "Невідома категорія";
            public const string UnkTitle = "Без титулу";
        }

        public static class Paths
        {
            public const string DefaultProfilePicture = "/images/avatars/default-avatar.png";

            public const string GoldMedal = "/images/medals/gold.png";
            public const string SilverMedal = "/images/medals/silver.png";
            public const string BronzeMedal = "/images/medals/bronze.png";

            public const string AvatarFolder = "/images/avatars/";

            public const string AvatarFolderName = "avatars";

            public const string Banned = "/account/banned";
            public const string Logout = "/account/logout";    
        }
    }
}
