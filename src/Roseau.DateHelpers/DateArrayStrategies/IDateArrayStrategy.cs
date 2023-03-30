namespace Roseau.DateHelpers.DateArrayStrategies;

public interface IDateArrayStrategy
{
    public DateOnly[] GetDates(DateOnly calculationDate, DateOnly lastDate);
}
