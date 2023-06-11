using System.Runtime.InteropServices;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DataAccess.Context
{
    public class MTSContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var stringUrl = @"Server=localhost;Initial Catalog=MtsDB; Database=MtsDB;Integrated Security=True;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true";
            //var stringUrl2 = @"Server=.\SQLEXPRESS;Initial Catalog=MtsDB; Database=MtsDB;Integrated Security=True;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true";

            //var stringUrl3 = @"Data Source=localhost,1433;Initial Catalog=MTSDb;User=sa;Password=Password.1;TrustServerCertificate=True";

            string jsonEnviroment = string.Empty;
            var homePath = Environment.GetEnvironmentVariable("HOME");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                jsonEnviroment = "C:\\mts\\connectionstring.json";
            else
                jsonEnviroment = $"{homePath}/mts/connectionstring.json";
            settingFile account = JsonConvert.DeserializeObject<settingFile>(File.ReadAllText(jsonEnviroment));
            optionsBuilder.UseSqlServer(account.DBConString);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserTicket> UserTickets { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<CompanySetting> CompanySettings { get; set; }

    }
    public class settingFile
    {
        public string DBConString { get; set; }
    }
}
