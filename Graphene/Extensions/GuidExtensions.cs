
using Microsoft.AspNetCore.WebUtilities;

namespace Graphene.Extensions
{
    public static class GuidExtensions
    {
        public static string ToBase64 (this Guid guid) {
            return WebEncoders.Base64UrlEncode(guid.ToByteArray());
        }
    }
}