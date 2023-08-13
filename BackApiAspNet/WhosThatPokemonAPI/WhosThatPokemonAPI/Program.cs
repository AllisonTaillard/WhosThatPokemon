using WhosThatPokemonAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services au conteneur de dependances (voir fichier DependencyInjectionExtension)
builder.InjectDependencies();

// Build
var app = builder.Build();

// Swagger pour le d�veloppement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// CORS
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();