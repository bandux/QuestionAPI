using Microsoft.Azure.Cosmos;
using QuestionApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton(s =>
{
    var configuration = s.GetRequiredService<IConfiguration>();
    return new CosmosClient(configuration["CosmosDb:Account"], configuration["CosmosDb:Key"]);
});
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
