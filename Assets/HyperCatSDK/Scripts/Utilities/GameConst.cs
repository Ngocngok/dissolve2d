using System.Collections.Generic;

#if !PROTOTYPE
using UnityEngine.Purchasing;
#endif

public class GameConst
{
    public const string PACKAGE_NAME = "com.hypercat.milk";
    public const string NO_ADS_ID = PACKAGE_NAME + ".noads";

    public const string PUBLIC_KEY_APPSFLYER =
        "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5ey5IArolWyBplig8yepzwRCsmd2b43h9+WAcALzLeSfpNTGTFMWI0v/uKsjs7bhf6+zpXhdI1jfwqfbqOFT2e647kjYHEzCWMtRk8dPeJwE3cpyMQDc8dMl1XsGqw5b/sZcimTgTfPwg9gfQhj9G9ZEeEB1w6tOhK+acnMXwnDRQeesZ3CPZHuMC04UBL4FezmCtC9Vju8lOFmZiGtpWduM7r7nCsntOnaoWH6/EyThJtz9rHcDAi2vY9Wd1XVNERKtDtNBdNFfjCvmd1jThCXsKaN6zVVUAX8biX8y4HBnC4Daq1culFjtMacu237YBTfGQqkz9yOPQyaaNV2T9wIDAQAB";

    public const string APPFLYER_APP_KEY = "Mza5CYwx7pzKhdhcFcTHdm";

    //public const string IRON_SOURCE_APP_KEY = "f568afd9";

#if !PROTOTYPE
    public static Dictionary<string, ProductType> listIAP = new Dictionary<string, ProductType>() {{NO_ADS_ID, ProductType.NonConsumable}};
#endif

    public const string NetworkNotAvailble = "Network connection failed. Please try again later.";
}