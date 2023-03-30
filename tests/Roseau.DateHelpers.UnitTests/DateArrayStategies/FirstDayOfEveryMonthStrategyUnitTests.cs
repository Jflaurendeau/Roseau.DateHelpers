using Roseau.DateHelpers.DateArrayStrategies;

namespace Roseau.DateHelpers.UnitTests.DateArrayStategies;

[TestClass]
public class FirstDayOfEveryMonthStrategyUnitTests
{
	[TestMethod]
	public void CheckOutOfRangeException_LatestDateIsAfterFirstDate_AssertDoesNotThrow()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryMonthStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2022, 3, 1);
		// Act

		// Assert
		Assert.That.DoesNotThrow(() => strategy.GetDates(calculationDate, latestDate));
	}
	[TestMethod]
	public void CheckOutOfRangeException_LatestDateIsBeforeFirstDate_AssertThrow()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryMonthStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2022, 2, 28);
		// Act

		// Assert
		Assert.ThrowsException<ArgumentException>(() => strategy.GetDates(calculationDate, latestDate));
	}
	[TestMethod]
	public void CalculateDateArray_NumberMonthsBetween12And13_ReturnGoodDates()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryMonthStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2023, 2, 28);
		DateOnly[] expectedDates = new DateOnly[]
		{
			new(2022, 3, 1),
			new(2022, 4, 1),
			new(2022, 5, 1),
			new(2022, 6, 1),
			new(2022, 7, 1),
			new(2022, 8, 1),
			new(2022, 9, 1),
			new(2022, 10, 1),
			new(2022, 11, 1),
			new(2022, 12, 1),
			new(2023, 1, 1),
			new(2023, 2, 1),
		};
		// Act
		var dates = strategy.GetDates(calculationDate, latestDate);

		// Assert
		Assert.AreEqual(expectedDates.Length, dates.Length);
		for (int i = 0; i < expectedDates.Length; i++)
			Assert.AreEqual(expectedDates[i], dates[i]);

	}
	[TestMethod]
	public void CalculateDateArray_NumberMonthsBetween11And12_ReturnGoodDates()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryMonthStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2023, 2, 15);
		DateOnly[] expectedDates = new DateOnly[]
		{
			new(2022, 3, 1),
			new(2022, 4, 1),
			new(2022, 5, 1),
			new(2022, 6, 1),
			new(2022, 7, 1),
			new(2022, 8, 1),
			new(2022, 9, 1),
			new(2022, 10, 1),
			new(2022, 11, 1),
			new(2022, 12, 1),
			new(2023, 1, 1),
			new(2023, 2, 1),
		};
		// Act
		var dates = strategy.GetDates(calculationDate, latestDate);

		// Assert
		Assert.AreEqual(expectedDates.Length, dates.Length);
		for (int i = 0; i < expectedDates.Length; i++)
			Assert.AreEqual(expectedDates[i], dates[i]);

	}
	[TestMethod]
	public void CalculateDateArray_NumberMonthsExactly13_ReturnGoodDates()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryMonthStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2023, 3, 1);
		DateOnly[] expectedDates = new DateOnly[]
		{
			new(2022, 3, 1),
			new(2022, 4, 1),
			new(2022, 5, 1),
			new(2022, 6, 1),
			new(2022, 7, 1),
			new(2022, 8, 1),
			new(2022, 9, 1),
			new(2022, 10, 1),
			new(2022, 11, 1),
			new(2022, 12, 1),
			new(2023, 1, 1),
			new(2023, 2, 1),
			new(2023, 3, 1),
		};
		// Act
		var dates = strategy.GetDates(calculationDate, latestDate);

		// Assert
		Assert.AreEqual(expectedDates.Length, dates.Length);
		for (int i = 0; i < expectedDates.Length; i++)
			Assert.AreEqual(expectedDates[i], dates[i]);

	}
}
