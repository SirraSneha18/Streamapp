using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using Bogus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MinimalApiDemo;

public class Startup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
    }

    /// <summary>
    /// Application configuration (resolved from different sources)
    /// </summary>
    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">Dependency injection container for registering services in</param>
    public void ConfigureServices(IServiceCollection services)
    {
        var mvcBuilder = services.AddMvc(o =>
        {
            o.AllowEmptyInputInBodyModelBinding = true;
            o.EnableEndpointRouting = false;
        });

        mvcBuilder.AddJsonOptions(op =>
        {
            ApplyJsonOptions(op.JsonSerializerOptions);
            op.JsonSerializerOptions.WriteIndented = true;
        });

        // services.SetupVstSerializationSysText(mvcBuilder, !_env.IsDeployedToAzure());
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">Application builder</param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();

        var s = app.UseEndpoints(c =>
        {
            c.MapGet("/api/fluids-filters/pre-cache", async ([FromQuery] string prefixes) =>
            {
                async IAsyncEnumerable<Cat.Turbo.FluidsAndFilters.Data.Progress> StreamCountriesAsync()
                {
                    var fak = new ProgressFaker();
                    foreach (var country in fak.Generate(10))
                    {
                        country.Prefix = prefixes;

                        await Task.Delay(500);
                        yield return country;
                    }
                }

                return StreamCountriesAsync();
            });

            c.MapFallbackToFile("index.html");
        });
        
        
    }
    
    
    /// <summary>
    /// Create or Augment options
    /// </summary>
    /// <param name="options">options</param>
    /// <param name="convertorFilter">filter for convertors</param>
    /// <returns>JsonSerializerOptions</returns>
    public static JsonSerializerOptions ApplyJsonOptions(JsonSerializerOptions options, Predicate<JsonConverter> convertorFilter = null)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.MaxDepth = 128;

        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.NumberHandling = JsonNumberHandling.AllowReadingFromString;

        var converters = new List<JsonConverter>();

        var resolver = new DefaultJsonTypeInfoResolver
        {
            
        };

        options.TypeInfoResolver = resolver;

      

        if (convertorFilter != null)
        {
            converters = converters.Where(convertorFilter.Invoke).ToList();
        }

        foreach (var jsonConverter in converters)
        {
            options.Converters.Add(jsonConverter);
        }

        return options;
    }
}

public class ProgressFaker : Faker<Cat.Turbo.FluidsAndFilters.Data.Progress>
{
    public ProgressFaker()
    {
        UseSeed(42);
        RuleFor(x => x.Prefix, f => f.Lorem.Word());
        RuleFor(x => x.Value, f => f.Random.Decimal());
        RuleFor(x => x.Message, f => f.Lorem.Sentence());
    }
}


