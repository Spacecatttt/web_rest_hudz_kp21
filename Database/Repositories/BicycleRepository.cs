using web_rest_hudz_kp21.Models;

namespace web_rest_hudz_kp21.Database.Repositories
{
    public class BicycleRepository : GenericRepository<Bicycle>
    {
        public BicycleRepository(ApplicationContext context) : base(context)
        { }
    }
}