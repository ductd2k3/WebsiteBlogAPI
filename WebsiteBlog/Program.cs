
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebsiteBlog.Data;
using WebsiteBlog.Models;
using WebsiteBlog.Repository;

namespace WebsiteBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(op => 
            op.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

            builder.Services.AddCors(option => option.AddPolicy("cors1", build =>
            {
                build.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            }));

            builder.Services.AddScoped<IUserReposiroty, UserRepository>();
            builder.Services.AddScoped<ICommonRepository<Blog>, BlogRepository>();
            
            var secretKey = builder.Configuration["JWTSettings:SecretKey"];
            var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters 
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        ValidateIssuerSigningKey = true,// use a custom key?
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte), //Specifies the key used to sign the JWT

                        ClockSkew = TimeSpan.Zero
                    };
                });
            
            var app = builder.Build();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, "Images")),
                RequestPath = "/uploads"
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            

            app.UseCors("cors1");
            app.UseHttpsRedirection();

            

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
