using AccountPaymentsAPI.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PaymentsApplication;
using PaymentsApplication.Interfaces.AppServicesInterface;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.AccountServicesInterfaces;
using PaymentsApplication.Interfaces.FizikServicesInterfaces.CardServicesInterfaces;
using PaymentsApplication.Interfaces.OwnerServicesInterfaces;
using PaymentsApplication.Interfaces.Token;
using PaymentsApplication.Interfaces.Users;
using PaymentsApplication.Interfaces.YurikServicesİnterfaces;
using PaymentsApplication.Services.AppServices;
using PaymentsApplication.Services.FizikServices.AccountServices;
using PaymentsApplication.Services.FizikServices.CardServices;
using PaymentsApplication.Services.OwnerServices;
using PaymentsApplication.Services.Token;
using PaymentsApplication.Services.Users;
using PaymentsApplication.Services.YurikServices;
using PaymentsDataLayer.Contexts;
using PaymentsDataLayer.Interface.Fizik.Card;
using PaymentsDataLayer.Interface.FizikInterfaces.Accounts;
using PaymentsDataLayer.Interface.OperationInterfaces;
using PaymentsDataLayer.Interface.OwnerInterfaces;
using PaymentsDataLayer.Interface.Payments;
using PaymentsDataLayer.Interface.Users;
using PaymentsDataLayer.Interface.YurikInterfaces;
using PaymentsDataLayer.Repository.FizikRepositorys.AccountRepository;
using PaymentsDataLayer.Repository.FizikRepositorys.CardRepository;
using PaymentsDataLayer.Repository.Individual_Owner;
using PaymentsDataLayer.Repository.Operations;
using PaymentsDataLayer.Repository.Payments;
using PaymentsDataLayer.Repository.Users;
using PaymentsDataLayer.Repository.YurikRepository;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
	   .WriteTo.Console()
	   .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Payments API",
		Version = "V1"
	});
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter  your token in the text input below.\r\n\r\nExample: \"SampleToken123456789\"",
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
				 {
					 {
						   new OpenApiSecurityScheme
							 {
								 Reference = new OpenApiReference
								 {
									 Type = ReferenceType.SecurityScheme,
									 Id = "Bearer"
								 }
							 },
							 Array.Empty<string>()
					 }
				 });
});

var connectionString = builder.Configuration.GetConnectionString("DBConnection");
builder.Services.AddDbContext<Context>(options => options.UseOracle(connectionString));

builder.Services.AddTransient<Context>();
builder.Services.AddScoped<ExceptionHandler>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<IGetCardInfo, GetCardInfoDb>();

builder.Services.AddScoped<ISaveCardInfo, SaveCardInfoDb>();
builder.Services.AddScoped<ICardInfoService, GetCardInfoService>();
builder.Services.AddScoped<IPayAccounts, PayAccounts>();
builder.Services.AddScoped<IPayToCardNumberService, PayToCardNumberService>();
builder.Services.AddScoped<IAppServices, AppServices>();
builder.Services.AddScoped<IGetAccInfo, GetPhisicAccountInfoDb>();
builder.Services.AddScoped<IGetAccountInfoService, GetPhysicalPersonAccountInfoService>();
builder.Services.AddScoped<ISaveAccountInfo, SaveAccountInfoDb>();
builder.Services.AddScoped<IPayPhsicalAccountsService, PayPhsicalAccountsService>();
builder.Services.AddScoped<IGetLegalAccountInfoDb, GetLegalAccountInfoDb>();
builder.Services.AddScoped<IGetLegalAccountInfoService, GetLegalAccountInfoService>();
builder.Services.AddScoped<IPayLegalAccountsService, PayLegalAccountsService>();
builder.Services.AddScoped<IGetOwnerAccountInfoDb, GetOwnerAccountInfoDb>();
builder.Services.AddScoped<IGetOwnerAccountService, GetOwnerAccountService>();
builder.Services.AddScoped<IGetOperStatus, GetOperStatus>();

builder.Services.AddApplicationLayer(builder.Configuration);

var app = builder.Build();
app.UseSwagger(c => c.SerializeAsV2 = true);
app.UseSwaggerUI(c =>
{
	c.DocExpansion(DocExpansion.None);
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment from Terminals");
	c.RoutePrefix = string.Empty;

});
//for get ip adress
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.XForwardedFor |
	ForwardedHeaders.XForwardedProto
});
app.UseMiddleware<ExceptionHandler>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
