using SistemaEscolar.Api.Ioc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Host.UseSerilog((context, configuration) =>
{
    int _applicationNumber = Convert.ToInt32(context.Configuration["ApplicationConfigurationData:Identificador"]);
    string _applicationName = context.Configuration["ApplicationConfigurationData:Nome"];

    configuration.Enrich.FromLogContext()
                 .Enrich.WithMachineName()
                 .WriteTo.Console()
                /*
                Habilitar Log no ElasticSearch
                .WriteTo.Elasticsearch(
                   new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
                   {
                       BufferLogShippingInterval = TimeSpan.FromSeconds(5),
                       MinimumLogEventLevel = LogEventLevel.Verbose,
                       CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                       IndexFormat = $"applogs-{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-date-{DateTime.Now.ToString("yyyy-MM-dd")}-{Environment.MachineName.ToLower().Replace(".", "-")}",
                       AutoRegisterTemplate = true,
                       NumberOfShards = 2,
                       NumberOfReplicas = 2

                   })
                */
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddDependencias(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
