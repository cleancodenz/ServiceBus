

namespace CustomWCFComponents.OldClaimsConstant
{
    public static  class OperationClaimTypes
    {
        public const string Create = "http://schemas.mywebsite.com/identity/claim/create";
        public const string Read = "http://schemas.mywebsite.com/identity/claim/read";
        public const string Update = "http://schemas.mywebsite.com/identity/claim/update";
        public const string Delete = "http://schemas.mywebsite.com/identity/claim/delete";

    }

    public static class IssuerClaimTypes
    {
        public const string Issuer = "http://schemas.mywebsite.com/issuer";
    }
}
