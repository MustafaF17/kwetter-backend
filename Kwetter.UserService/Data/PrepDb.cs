using Kwetter.UserService.Model;

namespace Kwetter.UserService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DataContext>());
            }
        }

        private static async void SeedData(DataContext context)
        {
            //if (!context.Users.Any())
            //{
            //    Console.WriteLine("Seeding data");


            //    await context.Users.AddRangeAsync(

            //        new User() { Name = "Name1", Email = "random1@gmail.com", Password = "password", Username = "username1", Created = DateTime.Now },
            //        new User() { Name = "Name2", Email = "random2@gmail.com", Password = "password", Username = "username2", Created = DateTime.Now },
            //        new User() { Name = "Name3", Email = "random3@gmail.com", Password = "password", Username = "username3", Created = DateTime.Now }
            //        );

            //    await context.SaveChangesAsync();
            //}

            //else
            //{
            //    Console.WriteLine("There is already data, seeding failed");
            //}
        }

    }
}
