using DataBase.Entities;
using Newtonsoft.Json;
using static Application.Methods;

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

app.UseRouting();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors();

//Users middleware
string middlewareUsersPath = "/api/users";

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

app.MapGet(middlewareTeachersPath, () =>
{
    try
    {
        return Results.Json(Teachers.Get());
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
        return Results.Json(Teachers.GetExtended(new() { id }, needActivities: true)[0]);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareTeachersPath + "/extended/activity/{id}", (int id) =>
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

app.MapPost(middlewareTeachersPath + "/{teacherString}", (string teacherString) =>
{
    try
    {
        Teacher tempTeacher = JsonConvert.DeserializeObject<Teacher>(teacherString)!;
        if (Teachers.Get(tempTeacher.UserId) != null) throw new Exception("Teacher already exists.");
        Teacher newTeacher = Teachers.Add(tempTeacher);
        return Results.Json(newTeacher);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareTeachersPath + "/{teacherString}", (string teacherString) =>
{
    try
    {
        Teacher teacher = JsonConvert.DeserializeObject<Teacher>(teacherString)!;

        return Results.Json(Teachers.Update(teacher));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareTeachersPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Teachers.Delete(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});


//Activities teachers middleware
string middlewareActivitiesTeachersPath = "/api/activitiesTeachers";

app.MapGet(middlewareActivitiesTeachersPath, () =>
{
    try
    {
        return Results.Json(ActivitiesTeachers.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPost(middlewareActivitiesTeachersPath + "/{activityTeacherString}", (string activityTeacherString) =>
{
    try
    {
        ActivityTeacher activityTeacher = JsonConvert.DeserializeObject<ActivityTeacher>(activityTeacherString)!;
        ActivityTeacher? dbActivityTeacher = ActivitiesTeachers.Get(activityTeacher.ActivityId, activityTeacher.TeacherId);
        if (dbActivityTeacher != null) throw new Exception($"Activity already has {activityTeacher}!");

        return Results.Json(ActivitiesTeachers.Add(activityTeacher));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareActivitiesTeachersPath + "/{activityTeacherString}", (string activityTeacherString) =>
{
    try
    {
        ActivityTeacher activityTeacher = JsonConvert.DeserializeObject<ActivityTeacher>(activityTeacherString)!;

        return Results.Json(ActivitiesTeachers.Update(activityTeacher));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareActivitiesTeachersPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(ActivitiesTeachers.Delete(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//News middleware
string middlewareNewsPath = "/api/news";

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

app.MapPost(middlewareNewsPath + "/{newsString}", (string newsString) =>
{
    try
    {
        DataBase.Entities.News tempNews = JsonConvert.DeserializeObject<DataBase.Entities.News>(newsString)!;
        if (Application.Methods.News.Get(tempNews.Name) != null) throw new Exception("News already exists.");
        DataBase.Entities.News newNews = Application.Methods.News.Add(tempNews);
        return Results.Json(newNews);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareNewsPath + "/{newsString}", (string newsString) =>
{
    try
    {
        DataBase.Entities.News news = JsonConvert.DeserializeObject<DataBase.Entities.News>(newsString)!;

        return Results.Json(Application.Methods.News.Update(news));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareNewsPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Application.Methods.News.Delete(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Trainings middleware
string middlewareTrainingsPath = "/api/trainings";

app.MapGet(middlewareTrainingsPath, () =>
{
    try
    {
        return Results.Json(Trainings.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

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

app.MapPost(middlewareTrainingsPath + "/{trainingString}", (string trainingString) =>
{
    try
    {
        Training tempTraining = JsonConvert.DeserializeObject<Training>(trainingString)!;
        if (Trainings.Get(tempTraining.TeacherId, tempTraining.DateTimeStart) != null) throw new Exception("Training already exists.");
        Training newTraining = Trainings.Add(tempTraining);
        return Results.Json(newTraining);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareTrainingsPath + "/{trainingString}", (string trainingString) =>
{
    try
    {
        Training training = JsonConvert.DeserializeObject<Training>(trainingString)!;

        return Results.Json(Trainings.Update(training));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareTrainingsPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Trainings.Delete(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Appointments middleware
string middlewareAppointmentsPath = "/api/appointments";

app.MapGet(middlewareAppointmentsPath, () =>
{
    try
    {
        return Results.Json(Appointments.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareAppointmentsPath + "/{appointmentString}", (string appointmentString) =>
{
    try
    {
        var appointment = JsonConvert.DeserializeObject<Appointment>(appointmentString)!;
        return Results.Json(Appointments.Get(appointment.TrainingId, appointment.UserId));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareAppointmentsPath + "/extended/training/{trainingId}", (int trainingId) =>
{
    try
    {
        return Results.Json(Appointments.GetExtended(trainingId));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPost(middlewareAppointmentsPath + "/{appointmentString}", (string appointmentString) =>
{
    try
    {
        Appointment appointment = JsonConvert.DeserializeObject<Appointment>(appointmentString)!;
        Appointment? dbAppointment = Appointments.Get(appointment.TrainingId, appointment.UserId);
        if (dbAppointment != null) throw new Exception("User already signed up!");

        return Results.Json(Appointments.Add(appointment));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareAppointmentsPath + "/{appointmentString}", (string appointmentString) =>
{
    try
    {
        Appointment appointment = JsonConvert.DeserializeObject<Appointment>(appointmentString)!;

        return Results.Json(Appointments.Update(appointment));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareAppointmentsPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Appointments.Delete(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Attendees middleware
string middlewareАttendeesPath = "/api/attendees";

app.MapGet(middlewareАttendeesPath, () =>
{
    try
    {
        return Results.Json(Attendees.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPost(middlewareАttendeesPath + "/{attendString}", (string attendString) =>
{
    try
    {
        Attend attend = JsonConvert.DeserializeObject<Attend>(attendString)!;
        Attend? dbAttend = Attendees.Get(attend.TrainingId, attend.UserId);
        if (dbAttend != null) throw new Exception("User already beeing attended!");

        return Results.Json(Attendees.Add(attend));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareАttendeesPath + "/{attendString}", (string attendString) =>
{
    try
    {
        Attend attend = JsonConvert.DeserializeObject<Attend>(attendString)!;

        return Results.Json(Attendees.Update(attend));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareАttendeesPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Attendees.Delete(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Users achievements middleware
string middlewareUsersAchievementsPath = "/api/usersAchievements";

app.MapGet(middlewareUsersAchievementsPath, () =>
{
    try
    {
        return Results.Json(UsersAchievements.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPost(middlewareUsersAchievementsPath + "/{usersAchievementsString}", (string usersAchievementsString) =>
{
    try
    {
        UserAchievements usersAchievements = JsonConvert.DeserializeObject<UserAchievements>(usersAchievementsString)!;
        UserAchievements? dbUserAchievements = UsersAchievements.Get(usersAchievements.UserId, usersAchievements.AchievementId);
        if (dbUserAchievements != null) throw new Exception("User already has this achievement!");

        return Results.Json(UsersAchievements.Add(usersAchievements));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareUsersAchievementsPath + "/{usersAchievementsString}", (string usersAchievementsString) =>
{
    try
    {
        UserAchievements usersAchievements = JsonConvert.DeserializeObject<UserAchievements>(usersAchievementsString)!;

        return Results.Json(UsersAchievements.Update(usersAchievements));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareUsersAchievementsPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(UsersAchievements.Delete(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Achievements middleware
string middlewareAchievementsPath = "/api/achievements";

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

app.MapPost(middlewareAchievementsPath + "/{achievementString}", (string achievementString) =>
{
    try
    {
        Achievement achievement = JsonConvert.DeserializeObject<Achievement>(achievementString)!;
        Achievement? dbAchievement = Achievements.Get(achievement.Name);

        if (dbAchievement != null) throw new Exception("Achievement aready exists!");

        return Results.Json(Achievements.Add(achievement));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareAchievementsPath + "/{achievementString}", (string achievementString) =>
{
    try
    {
        Achievement achievement = JsonConvert.DeserializeObject<Achievement>(achievementString)!;

        return Results.Json(Achievements.Update(achievement));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareAchievementsPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Achievements.Delete(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

//Review middleware
string middlewareReviewsPath = "/api/reviews";

app.MapGet(middlewareReviewsPath, () =>
{
    try
    {
        return Results.Json(Reviews.Get());
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapGet(middlewareReviewsPath + "/teacher/{id}", (int id) =>
{
    try
    {
        return Results.Json(Reviews.GetExtended(teacherId: id));
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
        Review? dbReview = Reviews.Get(review.TeacherId, review.UserId);

        if (dbReview != null) Reviews.Delete(dbReview.Id);

        dbReview = Reviews.Add(review);

        return Results.Json(dbReview);
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapPut(middlewareReviewsPath + "/{reviewString}", (string reviewString) =>
{
    try
    {
        Review review = JsonConvert.DeserializeObject<Review>(reviewString)!;

        return Results.Json(Reviews.Update(review));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});

app.MapDelete(middlewareReviewsPath + "/{id}", (int id) =>
{
    try
    {
        return Results.Json(Reviews.Delete(id));
    }
    catch (Exception ex)
    {
        return Results.Problem(title: ex.Message);
    }
});


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapFallbackToFile("index.html");

    endpoints.Map("/", context =>
    {
        context.Response.Redirect("/tests");
        return Task.CompletedTask;
    });
});


app.Run();
