using car_wash_backend.Models;

namespace car_wash_backend.Services;

public class BoxesInCarwashAccessor (CarWashContext db)
{
    public IQueryable<BoxesInCarwash> GetAll()
    {
        return db.BoxesInCarwashes;
    }

    public Box GetAvailableBox(Guid carwashId)
    {
        //потом использовать это:
        //return GetAll().FirstOrDefault(b => b.CarwashId == carwashId && b.Status == "available").Box;
        
        return GetAll().FirstOrDefault(b => b.CarwashId == carwashId).Box;
    }
}