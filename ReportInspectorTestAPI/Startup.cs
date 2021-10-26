using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportInspectorTestAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers()
				.AddNewtonsoftJson(options => {
					options.SerializerSettings.Converters.Add(new StringEnumConverter());
					options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				})
				.AddJsonOptions(options =>
					options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer("ReportInspectorJWT", options =>
			{
				options.Authority = $"https://{Configuration["Auth0:Domain"]}";
				options.Audience = $"https://{Configuration["Auth0:Audience"]}";
			});

			services.AddAuthorization(options =>
			{
				options.DefaultPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.AddAuthenticationSchemes("ReportInspectorJWT")
					.Build();
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReportInspectorTestAPI", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = "Bearer",
								Type = ReferenceType.SecurityScheme
							},
						},
						new List<string>()
					}
				});
			});
			services.AddSwaggerGenNewtonsoftSupport();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReportInspectorTestAPI v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
