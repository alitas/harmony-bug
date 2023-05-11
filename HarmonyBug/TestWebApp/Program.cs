var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

BsonBinaryDataPatch.HarmonyInstance.Value.PatchAll();
app.MapGet("/", () => "Hello World!");

app.Run();