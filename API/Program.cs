using Microsoft.OpenApi.Models;
using Clothespin2.Data;
using Clothespin2.Common;
using Clothespin2.Common.Clothes.Items;
using Clothespin2.Common.Tracking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("V2", new OpenApiInfo {
        Version = "V2", Title = "Clothespin API",
        Description = "Clothespin API for the UI course",
        //TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact {
            Name = "Chopo",
            Url = new Uri("https://twitter.com/igtampe"),
            Email = "igtampe@gmail.com",
        },
        License = new OpenApiLicense {
            Name = "CC0",
            //Url = new Uri("https://example.com/license") 
        }
    });
    options.IncludeXmlComments("./API.xml");
});

builder.Services.AddDbContext<ClothespinContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/V2/swagger.json", "Clothespin API"));
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();