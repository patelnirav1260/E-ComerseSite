namespace Web.UI.Models.Dto
{
    public class Request
    {
        public string Url { get; set; }
        public ApiType Method { get; set; } = ApiType.GET;
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE,
    }

    public class APIUrl
    {
        public static string CouponBase { get; set; }

        public static string Auth_Base { get; set; }

        public static string Product_Base { get; set; }

        public static string ShopingCart_Base { get; set; }

        public const string CustmerRole = "Customer";

        public const string AdminRole = "Admin";
    }

    public class GetRole
    {

        public const string CustomerRole = "Customer";

        public const string AdminRole = "Admin";
    }
}
