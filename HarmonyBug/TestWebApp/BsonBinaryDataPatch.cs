using HarmonyLib;
using MongoDB.Bson;

[HarmonyPatch(typeof(BsonBinaryData), MethodType.Constructor, typeof(byte[]), typeof(BsonBinarySubType))]
public class BsonBinaryDataPatch
{
    public static readonly Lazy<Harmony> HarmonyInstance = new Lazy<Harmony>(() => new Harmony("tonic.Bson.Binary"), true);

    [HarmonyPrefix]
    static bool Prefix(ref byte[] bytes, BsonBinarySubType subType)
    {
        if (subType is BsonBinarySubType.UuidLegacy or BsonBinarySubType.UuidStandard && bytes.Length != 16)
        {
            var uuidBytes = new byte[16];
            Array.Copy(bytes, uuidBytes, Math.Min(16, bytes.Length));
            bytes = uuidBytes;
        }
        return true;
    }
}