namespace Roseau.DateHelpers.DateArrayStrategies;

public class FirstDayOfEveryMonthStrategy : DateArrayBaseStrategy
{
	protected override void CheckOutOfRangeException(DateOnly calculationDate, DateOnly lastDate)
	{
		calculationDate = calculationDate.FirstDayOfFollowingOrCoincidantMonth();
		if (calculationDate > lastDate) throw new ArgumentException($"The first day of the month (if the {nameof(calculationDate)} is itself a first) or of the next month ({calculationDate}) must be before the {nameof(lastDate)} ({lastDate})", nameof(lastDate));
	}
	protected override DateOnly[] CalculateDateArray(DateOnly calculationDate, DateOnly lastDate)
	{
		calculationDate = calculationDate.FirstDayOfFollowingOrCoincidantMonth();
		int numberOfFullMonths = calculationDate.NumberOfCompleteMonths(lastDate);
		DateOnly[] paymentDatesArray = new DateOnly[numberOfFullMonths];

		for (int i = 0; i < numberOfFullMonths; i++)
		{
			paymentDatesArray[i] = calculationDate.AddMonths(i);
		}

		return paymentDatesArray;
	}

}
