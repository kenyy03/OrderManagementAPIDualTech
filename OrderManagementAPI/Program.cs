using OrderManagementAPI.Common;
using OrderManagementAPI.Data;
using OrderManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddServices()
    .AddData(builder.Configuration);


builder.Services.AddAutoMapper(cfs => cfs.AddProfile(new MapProfileConfig()));

string policyName = "AllowAnyOrigin";
builder.Services.AddCors((opt) =>
{
    opt.AddPolicy(policyName, policy =>
    {
        policy.SetIsOriginAllowed((_) => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyName);

app.UseHealthChecks("/healthcheck");

app.MapControllers();

app.Run();

