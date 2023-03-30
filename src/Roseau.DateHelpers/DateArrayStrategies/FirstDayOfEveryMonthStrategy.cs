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
		int numberDates = calculationDate.NumberOfCompleteMonths(lastDate)+1;
		DateOnly[] paymentDatesArray = new DateOnly[numberDates];

		for (int i = 0; i < numberDates; i++)
		{
			paymentDatesArray[i] = calculationDate.AddMonths(i);
		}

		return paymentDatesArray;
	}

}
