namespace ShortLink.WebAPI.Extensions
{
    public static class AppExtensions
    {
        public static void ConfigureSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ShortLink API V1");
                });
            }
        }
    }
}