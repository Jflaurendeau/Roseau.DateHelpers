namespace Roseau.DateHelpers.DateArrayStrategies;

public class FirstDayOfEveryYearStrategy : DateArrayBaseStrategy
{
	protected override void CheckOutOfRangeException(DateOnly calculationDate, DateOnly lastDate)
	{
		calculationDate = calculationDate.FirstDayOfFollowingOrCoincidantYear();
		if (calculationDate > lastDate) throw new ArgumentException($"The first day of the year (if the {nameof(calculationDate)} is itself a first) or of the next year ({calculationDate}) must be before the {nameof(lastDate)} ({lastDate})",nameof(lastDate));
	}
	protected override DateOnly[] CalculateDateArray(DateOnly calculationDate, DateOnly lastDate)
	{
		calculationDate = calculationDate.FirstDayOfFollowingOrCoincidantYear();
		int numberOfDates = calculationDate.NumberOfCompleteYears(lastDate)+1;
		DateOnly[] paymentDatesArray = new DateOnly[numberOfDates];

		for (int i = 0; i < numberOfDates; i++)
		{
			paymentDatesArray[i] = calculationDate.AddYears(i);
		}

		return paymentDatesArray;
	}

}
