using MagniVerseBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Enable static files (images)
app.UseStaticFiles();

app.MapControllers();


// -------- Manual Setup --------

// folder to store manuals
var manualsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Manuals");

if (!Directory.Exists(manualsFolder))
{
    Directory.CreateDirectory(manualsFolder);
}

// manual file name
var manualFileName = "front-axle-manual.pdf";

var manualPath = Path.Combine(manualsFolder, manualFileName);

// public download URL (replace with your real link)
var manualUrl = "https://drive.google.com/file/d/1p6Ph0SWwRUb-LqPuNe55a4pN4FauKD_v/view?usp=sharing";

// download manual if not exists
if (!File.Exists(manualPath))
{
    Console.WriteLine("Downloading manual...");

    using var httpClient = new HttpClient();
    var fileBytes = await httpClient.GetByteArrayAsync(manualUrl);

    await File.WriteAllBytesAsync(manualPath, fileBytes);

    Console.WriteLine("Manual downloaded.");
}


// -------- Load manual into memory --------

var pdfService = new PDFService();

ManualRepository.LoadManual(manualPath, pdfService);


// -------- Start server --------

// required for Render hosting
app.Urls.Add("http://0.0.0.0:10000");

app.Run();