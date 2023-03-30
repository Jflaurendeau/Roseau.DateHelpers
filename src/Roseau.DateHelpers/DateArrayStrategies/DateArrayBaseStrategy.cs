namespace Roseau.DateHelpers.DateArrayStrategies;

abstract public class DateArrayBaseStrategy : IDateArrayStrategy
{
	public DateOnly[] GetDates(DateOnly calculationDate, DateOnly lastDate)
	{
		CheckOutOfRangeException(calculationDate, lastDate);
		return CalculateDateArray(calculationDate, lastDate);
	}
	abstract protected void CheckOutOfRangeException(DateOnly calculationDate, DateOnly lastDate);
	abstract protected DateOnly[] CalculateDateArray(DateOnly calculationDate, DateOnly lastDate);
}
