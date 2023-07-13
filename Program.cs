using Newtonsoft.Json;
using Supabase;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5173");
                      });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Supabase.Client>(_ => 
new Supabase.Client(
    //Insert SupabaseURL below
   "SupabaseURL",
   //Insert SupabaseAPIKey below
    "SupabaseAPIKey",
    new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true,
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/developers", async(
    Supabase.Client client) =>
    {
        var response = await client
            .From<Developer>()
            .Get();
        
        var developersString = response.Content;
        var developers = JsonConvert.DeserializeObject<List<DeveloperObject>>(developersString);

        return Results.Ok(developers);
    }
);

app.MapGet("/developers/{id}", async(
    int id, 
    Supabase.Client client) =>
{
    var response = await client
        .From<Developer>()
        .Where(dev => dev.Id == id)
        .Get();
    
    var developer = response.Models.FirstOrDefault();

    if (developer is null)
    {
        return Results.NotFound();
    }

    var developerResponse = new DeveloperResponse
    {
        Id = developer.Id,
        Name = developer.Name,
        Email = developer.Name,
        CreatedAt = developer.CreatedAt,
    };

    return Results.Ok(developerResponse);
});

app.MapPost("/developers", async(
    CreateDeveloperRequest request,
    Supabase.Client client) =>
{
    var developer = new Developer
    {
        Name = request.Name,
        Email = request.Email,
    };
    
    var response = await client.From<Developer>().Insert(developer);
    
    var newDeveloper = response.Models.First();

    return Results.Ok(newDeveloper.Id);
});

app.MapPut("/developers/{id}", async(
    int id,
    CreateDeveloperRequest request,
    Supabase.Client client) =>
{
    var responseOldDev = await client
    .From<Developer>()
    .Where(dev => dev.Id == id)
    .Get();
    
    var oldDeveloper = responseOldDev.Models.FirstOrDefault();

    if (oldDeveloper is null)
    {
        return Results.NotFound();
    }
    
    var update = await client
    .From<Developer>()
    .Where(dev => dev.Id == id)
    .Set(dev => dev.Name!, request.Name)
    .Set(dev => dev.Email!, request.Email)
    .Update();
    
    var response = await client
    .From<Developer>()
    .Where(dev => dev.Id == id)
    .Get();
    
    var updatedDeveloper = response.Models.First();

    return Results.Ok(updatedDeveloper.Id);
});

app.MapDelete("/developers/{id}", async(
    int id, 
    Supabase.Client client) =>
{
    await client
        .From<Developer>()
        .Where(dev => dev.Id == id)
        .Delete();
    
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.Run();
