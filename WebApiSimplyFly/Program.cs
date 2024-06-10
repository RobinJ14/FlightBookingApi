using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;
using WebApiSimplyFly.Repositories;
using WebApiSimplyFly.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<newContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });



    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
});

//Json serielization
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;

});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])),
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 });
//cors policy
builder.Services.AddCors(Options =>
{
    Options.AddPolicy("AngularPolicy", opts =>
    {
        opts.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    });
});


#region RepositoryInjection 
builder.Services.AddScoped<IRepository<Admin, int>, AdminRepository>();
builder.Services.AddScoped<IRepository<Airport,int>, AirportRepository>();
builder.Services.AddScoped<IRepository<Booking,int>, BookingRepository>();
builder.Services.AddScoped<IRepository<Customer,int>, CustomerRepository>();
builder.Services.AddScoped<IRepository<FlightOwner,int>, FlightOwnerRepository>();
builder.Services.AddScoped<IRepository<Flight,int>, FlightRepository>();
builder.Services.AddScoped<IRepository<History, int>, HistoryRepository>();
builder.Services.AddScoped<IRepository<PassengerBooking,int>, PassengerBookingRepository>();
builder.Services.AddScoped<IRepository<Passenger,int>, PassengerRepository>();
builder.Services.AddScoped<IRepository<PaymentDetails,int>, PaymentDetailsRepository>();
builder.Services.AddScoped<IRepository<Payment,int>, PaymentRepository>();
builder.Services.AddScoped<IRepository<Refund, int>, RefundRepository>();
builder.Services.AddScoped<IRepository<WebApiSimplyFly.Models.Route,int>, RouteRepository>();
builder.Services.AddScoped<IRepository<Schedule,int>, ScheduleRepository>();
builder.Services.AddScoped<IRepository<Seat,int>, SeatRepository>();
builder.Services.AddScoped<IRepository<User,string>, UserRepository>();
builder.Services.AddScoped<IPassengerBookingRepository, PassengerBookingRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();

#endregion

#region Service Injection

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IFlightOwnerService, FlightOwnerService>();
builder.Services.AddScoped<IFlightSearchService, FlightSearchService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IuserService, UserService>();




#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AngularPolicy");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
