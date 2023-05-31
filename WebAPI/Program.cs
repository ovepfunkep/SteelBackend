using DataBase.Entities;
using static Application.Methods;
using Newtonsoft.Json;
using static Application.Extensions;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors();

//Users middleware
string middlewareUsersPath = "/api/users";

app.MapPost(middlewareUsersPath + "/{userString}", (string userString) =>
{
    try
    {
        User tempUser = JsonConvert.DeserializeObject<User>(userString)!;
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
        User user = JsonConvert.DeserializeObject<User>(userString)!;

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
            User user = JsonConvert.DeserializeObject<User>(userString)!;
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

app.MapGet(middlewareUsersPath, () =>
{
    try
    {
        return Results.Json(Users.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareUsersPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Users.Delete(id));
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

app.MapPost(middlewareActivitiesPath + "/{activityString}", (string activityString) =>
{
    try
    {
        Activity tempActivity = JsonConvert.DeserializeObject<Activity>(activityString)!;
        if (Activities.Get(tempActivity.Name) != null) throw new Exception("Activity already exists.");
        Activity newActivity = Activities.Add(tempActivity);
        return Results.Json(newActivity);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareActivitiesPath + "/{activityString}", (string activityString) =>
{
    try
    {
        Activity activity = JsonConvert.DeserializeObject<Activity>(activityString)!;

        return Results.Json(Activities.Update(activity));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareActivitiesPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Activities.Delete(id));
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

app.MapGet(middlewareTeachersPath + "/extended", () =>
{
    try
    {
        return Results.Json(Teachers.GetExtended(needActivities: true));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareTeachersPath + "/extended/{id}", (int id) =>
{
    try
    {
        return Results.Json(Teachers.GetExtended(new() { id })[0]);
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

app.MapGet(middlewareNewsPath, () =>
{
    try
    {
        return Results.Json(Application.Methods.News.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareNewsPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Application.Methods.News.Get(new List<int>() { id })[0]);
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

app.MapGet(middlewareTrainingsPath + "/users/{userId}", (int userId, bool isUpcoming) =>
{
    try
    {
        return Results.Json(Trainings.GetExtendedByUserId(userId, isUpcoming));
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
        Appointment appointment = JsonConvert.DeserializeObject<Appointment>(appointmentString)!;
        Appointment? dbAppointment = Appointments.Get(appointment.TrainingId, appointment.UserId);
        if (dbAppointment != null)
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

app.MapGet(middlewareAchievementsPath, () =>
{
    try
    {
        return Results.Json(Achievements.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Review middleware
string middlewareReviewsPath = "/api/reviews";

app.MapGet(middlewareReviewsPath + "/teacher/{id}", (int id) =>
{
    try
    {
        return Results.Json(Reviews.GetExtended(teacherId: id)[0]);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPost(middlewareReviewsPath + "/{reviewString}", (string reviewString) =>
{
    try
    {
        Review? review = JsonConvert.DeserializeObject<Review>(reviewString)!;

        if (Reviews.Get(review.TeacherId, review.UserId) != null)
        {
            Reviews.Delete(review.TeacherId, review.UserId);
            review = Reviews.Add(review);
        }
        else review = Reviews.Add(review);

        return Results.Json(review);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.Run();
