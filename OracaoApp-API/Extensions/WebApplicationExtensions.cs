namespace OracaoApp.API.Extensions;

public static class WebApplicationExtensions
{
    private static WebApplication MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

        return app;
    }

    public static void UseCustomServices(this WebApplication app)
    {
        app.UseCors();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MigrateDb();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers().RequireAuthorization();
    }


}
