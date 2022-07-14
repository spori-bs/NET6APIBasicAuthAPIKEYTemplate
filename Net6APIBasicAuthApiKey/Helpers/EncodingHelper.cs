namespace Net6APIBasicAuthApiKey.Helpers;

internal static class EncodingHelper
{
    /// <summary>
    /// Encode plaintext to Base64 string
    /// </summary>
    /// <param name="plainText">Plaintext to encode</param>
    /// <returns>Base64 encoded data</returns>
    internal static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    /// <summary>
    /// Decode base64 encoded string
    /// </summary>
    /// <param name="base64EncodedData">Encoded string</param>
    /// <returns>Decoded plaintext</returns>
    internal static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}