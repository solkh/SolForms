using SolForms.Services;
using SolForms.Extentions;
using SolForms.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.DocInclusionPredicate((docName, apiDesc) => true);
    options.OrderActionsBy(apiDesc => apiDesc.RelativePath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{    
    app.UseSwagger();
    app.UseSwaggerUI();    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();