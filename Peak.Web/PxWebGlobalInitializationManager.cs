using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Configuration;
using Peak.Localization;
using Peak.Scheduling;
using Peak.Session;
using Peak.Web.Filters;
using Peak.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.AspNetCore.Mvc;
using Peak.Logging;
using Microsoft.Extensions.Caching.Memory;
using Peak.Web.Caching;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Peak.Web {
  public static class PxWebGlobalInitializationManager {

    /// <summary>
    /// Servis collectiona mvc, distributed cache, session, antiforgery ve Peak'e özel servisler eklenmektedir.
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureServices(IServiceCollection services) {

      var sp = services.BuildServiceProvider();
      var objectPoolProvider = sp.GetService<ObjectPoolProvider>();

      services.AddMvc(options =>
      {
        options.Filters.Add(typeof(PxExceptionFilter));
        options.Filters.Add(new PxActionFilter());
        options.UseHtmlEncodeModelBinding();
        options.UseHtmlEncodeJsonInputFormatter(PxLoggerFactory.Instance.CreateLogger<MvcOptions>(), objectPoolProvider);
      });
      services.Configure<FormOptions>(x =>
      {
        x.ValueLengthLimit = int.MaxValue;
        x.MultipartBodyLengthLimit = int.MaxValue;
      });
      services.AddMemoryCache();
      services.AddSession();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddSingleton<IPxSessionStore, PxSessionStore>();
      services.AddSingleton<IPxClientInfoHelper, PxClientInfoHelper>();
      services.AddAntiforgery();
      services.AddSingleton<PxRazorViewToStringRenderer>();
    }

    /// <summary>
    /// ConfigureServices'de eklenen servisleri configure eder.
    /// Error sayfası için Error Controller'daki Index actionı kullanılmaktadır.
    /// Error StatusCode'lar için "/Error/Status/{0}" yapısı kullanılmaktadır.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="loggerFactory"></param>
    public static void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }
      else {
        app.UseExceptionHandler("/Error");
      }
      if (env.IsDevelopment()) {
        loggerFactory.AddConsole()
                     .AddDebug();
      }
      app.UseStatusCodePagesWithReExecute("/Error/Status/{0}");
      //PxWebMemoryCacheManager.Instance.MemoryCache = app.ApplicationServices.GetService<IMemoryCache>();
      PxRazorViewToStringRenderer.Configure(app.ApplicationServices.GetRequiredService<PxRazorViewToStringRenderer>());
      PxHttpContext.Configure(app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
      /* Session kullanımı için gerekli */
      app.UseSession(new SessionOptions()
      {
        CookieHttpOnly = true,       
        IdleTimeout = TimeSpan.FromMinutes(PxConfigurationManager.PxConfig.Session.DefaultTimeoutDuration)
      });
      /* Biz session üzerinde kullanıcı IP bilgisini veriyoruz. Load Balancer'lar arkasında bulunan bir sunucuda client IP bilgisini alabilmek için
			 Forwarded Header'lara bakmak gerekiyor */
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });
      if (PxConfigurationManager.PxConfig.UseCompression) {
        app.UseCompression();
      }
      IPxSessionStore iPxContextStore = app.ApplicationServices.GetService<IPxSessionStore>();
      IPxClientInfoHelper iPxClientInfoHelper = app.ApplicationServices.GetService<IPxClientInfoHelper>();
      PxSession.Initilize(iPxContextStore, iPxClientInfoHelper);

      app.UseStaticFiles(new StaticFileOptions
      {
        OnPrepareResponse = context =>
        {
          // Cache static file for 1 year
          if (!string.IsNullOrEmpty(context.Context.Request.Query["v"])) {
            context.Context.Response.Headers.Add("cache-control", new[] { "public,max-age=31536000" });
            context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddDays(1).ToString("R") });
          }
        }
      });

      CultureInfo culture = new CultureInfo("tr-TR");
      culture.NumberFormat.NumberDecimalSeparator = ".";
      culture.NumberFormat.NumberGroupSeparator = ",";
      culture.NumberFormat.CurrencyDecimalSeparator = ".";
      culture.NumberFormat.CurrencyGroupSeparator = ",";


      CultureInfo.DefaultThreadCurrentCulture = culture;
      CultureInfo.DefaultThreadCurrentUICulture = culture;
      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;

      app.UseRequestLocalization(new RequestLocalizationOptions
      {
        DefaultRequestCulture = new RequestCulture(culture),
        SupportedCultures = new List<CultureInfo>
        {
           culture
        },
        SupportedUICultures = new List<CultureInfo>
        {
            culture
        }
      });

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });

      PxLocalizationManager.Initialize();
      if (PxConfigurationManager.PxConfig.Schedule.Enabled) {
        PxTaskScheduler.Instance.Start();
      }
    }
  }
}
