using System.Text;
using DocumentAccessApprovalSystem.API.Services;
using DocumentAccessApprovalSystem.Application.Interfaces;
using DocumentAccessApprovalSystem.Application.Services;
using DocumentAccessApprovalSystem.Domain.Entities;
using DocumentAccessApprovalSystem.Infrastructure.Data;
using DocumentAccessApprovalSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;   // <<–– add this

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=documentAccessApprovalSystem.db"));

builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IDecisionService, DecisionService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddHostedService<NotificationWorker>();
builder.Services.AddHostedService<EmailWorker>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("JWT secret key is missing from configuration.");

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key
        };
    });

builder.Services.AddAuthorization();

// --- UPDATED: configure Swagger to use JWT Bearer ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // 1. Define the Bearer security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.\n\n" +
                      "Enter your token in the text input below.\n\n" +
                      "Example: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // 2. Apply the scheme globally to all operations
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "http",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });
});
// --- end Swagger JWT config ---

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocumentAccessApprovalSystem.ApI V1");
        c.RoutePrefix = string.Empty;  // Launch swagger at app root
    });   // now shows the Authorize button
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Seed users & Document 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // seed Users incrementally 
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User {  Name = "Varun", Role = UserRole.User },
            new User {  Name = "Vidya", Role = UserRole.Approver }
        );     
    }
    //seed Documents incrementally 
    var documentTitles = new[] { "Employee Handbook", "Onboarding Checklist", "User Guide", "Data Privacy Policy", "Change Request Form", "Version Control Guidelines", "Security Policy", "Incident Report Template", "Data Privacy Policy" };
    
    foreach(var title in documentTitles)
    {
        if(!db.Documents.Any())
        {
            db.Documents.Add(new Document { Title = title });
        }
    }

    await db.SaveChangesAsync();

}

app.MapControllers();
app.Run();
