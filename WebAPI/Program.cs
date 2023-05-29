using DataBase.Entities;
using static Application.Methods;
using Newtonsoft.Json;
using static Application.Extensions;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

//Users middleware
string middlewareUsersPath = "/api/users";

app.MapPost(middlewareUsersPath + "/{userString}", (string userString) =>
{
    try
    {
        User tempUser = JsonConvert.DeserializeObject<User>(userString);
        if (Users.Get(phone: tempUser.Phone) != null) throw new Exception("User already exists.");
        User newUser = Users.Add(tempUser);
        return Results.Json(newUser);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareUsersPath + "/{userString}", (string userString) =>
{
    try
    {
        User user = JsonConvert.DeserializeObject<User>(userString);

        return Results.Json(Users.Update(user));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareUsersPath + "/{userString}", (string userString) =>
{
    try
    {
        User? dbUser;

        try 
        { 
            User user = JsonConvert.DeserializeObject<User>(userString);
            dbUser = Users.Get(user.Id, user.Phone, user.Password);
        }
        catch
        {
            int userId = Convert.ToInt32(userString);
            dbUser = Users.Get(userId);
        }

        if (dbUser == null) throw new Exception("User not exists.");

        return Results.Json(dbUser);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Activities middleware
string middlewareActivitiesPath = "/api/activities";

app.MapGet(middlewareActivitiesPath, () =>
{
    try
    {
        return Results.Json(Activities.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareActivitiesPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Activities.Get(new List<int>() { id })[0]);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareActivitiesPath + "/main", () =>
{
    try
    {
        return Results.Json(Activities.Get(new List<int>() { 1 }));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Teachers middleware
string middlewareTeachersPath = "/api/teachers";

app.MapGet(middlewareTeachersPath + "/main", () =>
{
    try
    {
        return Results.Json(Teachers.GetExtended(new List<int>() { 1 }, true));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareTeachersPath + "/activity/{id}", (int id) =>
{
    try
    {
        return Results.Json(Teachers.GetExtendedByActivity(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//News middleware
string middlewareNewsPath = "/api/news";

app.MapGet(middlewareNewsPath + "/main", () =>
{
    try
    {
        return Results.Json(Application.Methods.News.Get(new List<int>() { 1 }));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Trainings middleware
string middlewareTrainingsPath = "/api/trainings";

app.MapGet(middlewareTrainingsPath + "/monthly/{date}", (DateTime date) =>
{
    try
    {
        DateTime start = new(date.Year, date.Month, 1, 0, 0, 0);
        DateTime end = start.AddMonths(1).AddSeconds(-1);
        return Results.Json(Trainings.GetExtended(start, end));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareTrainingsPath + "/users/{userId}", (int userId) =>
{
    try
    {
        return Results.Json(Trainings.GetUpcomingExtendedByUserId(userId));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Appointments middleware
string middlewareAppointmentsPath = "/api/appointments";

app.MapPost(middlewareAppointmentsPath + "/{appointmentString}", (string appointmentString) =>
{
    try
    {
        Appointment appointment = JsonConvert.DeserializeObject<Appointment>(appointmentString);
        if (Appointments.Get(appointment.TrainingId, appointment.UserId))
            throw new Exception("User already signed up!");

        return Results.Json(Appointments.Add(appointment.TrainingId, appointment.UserId));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Achievements middleware
string middlewareAchievementsPath = "/api/achievements";

app.MapGet(middlewareAchievementsPath + "/users/{userId}", (int userId) =>
{
    try
    {
        return Results.Json(Achievements.GetByUserId(userId));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.Run();
