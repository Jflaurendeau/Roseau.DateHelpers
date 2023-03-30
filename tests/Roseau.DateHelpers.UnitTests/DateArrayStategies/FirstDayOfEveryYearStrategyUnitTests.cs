using Roseau.DateHelpers.DateArrayStrategies;

namespace Roseau.DateHelpers.UnitTests.DateArrayStategies;

[TestClass]
public class FirstDayOfEveryYearStrategyUnitTests
{
	[TestMethod]
	public void CheckOutOfRangeException_LatestDateIsAfterFirstDate_AssertDoesNotThrow()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryYearStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2023, 3, 1);
		// Act

		// Assert
		_ = strategy.GetDates(calculationDate, latestDate);
	}
	[TestMethod]
	public void CheckOutOfRangeException_LatestDateIsBeforeFirstDate_AssertThrow()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryYearStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2022, 2, 28);
		// Act

		// Assert
		Assert.ThrowsException<ArgumentException>(() => strategy.GetDates(calculationDate, latestDate));
	}
	[TestMethod]
	public void CalculateDateArray_NumberYearsBetween12And13_ReturnGoodDates()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryYearStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2034, 2, 28);
		DateOnly[] expectedDates = new DateOnly[]
		{
			new(2023, 1, 1),
			new(2024, 1, 1),
			new(2025, 1, 1),
			new(2026, 1, 1),
			new(2027, 1, 1),
			new(2028, 1, 1),
			new(2029, 1, 1),
			new(2030, 1, 1),
			new(2031, 1, 1),
			new(2032, 1, 1),
			new(2033, 1, 1),
			new(2034, 1, 1),
		};
		// Act
		var dates = strategy.GetDates(calculationDate, latestDate);

		// Assert
		Assert.AreEqual(expectedDates.Length, dates.Length);
		for (int i = 0; i < expectedDates.Length; i++)
			Assert.AreEqual(expectedDates[i], dates[i]);

	}
	[TestMethod]
	public void CalculateDateArray_NumberYearsBetween11And12_ReturnGoodDates()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryYearStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2033, 2, 1);
		DateOnly[] expectedDates = new DateOnly[]
		{
			new(2023, 1, 1),
			new(2024, 1, 1),
			new(2025, 1, 1),
			new(2026, 1, 1),
			new(2027, 1, 1),
			new(2028, 1, 1),
			new(2029, 1, 1),
			new(2030, 1, 1),
			new(2031, 1, 1),
			new(2032, 1, 1),
			new(2033, 1, 1),
		};
		// Act
		var dates = strategy.GetDates(calculationDate, latestDate);

		// Assert
		Assert.AreEqual(expectedDates.Length, dates.Length);
		for (int i = 0; i < expectedDates.Length; i++)
			Assert.AreEqual(expectedDates[i], dates[i]);

	}
	[TestMethod]
	public void CalculateDateArray_NumberYearsExactly13_ReturnGoodDates()
	{
		// Arrange
		IDateArrayStrategy strategy = new FirstDayOfEveryYearStrategy();
		DateOnly calculationDate = new(2022, 2, 15);
		DateOnly latestDate = new(2035, 1, 1);
		DateOnly[] expectedDates = new DateOnly[]
		{
			new(2023, 1, 1),
			new(2024, 1, 1),
			new(2025, 1, 1),
			new(2026, 1, 1),
			new(2027, 1, 1),
			new(2028, 1, 1),
			new(2029, 1, 1),
			new(2030, 1, 1),
			new(2031, 1, 1),
			new(2032, 1, 1),
			new(2033, 1, 1),
			new(2034, 1, 1),
			new(2035, 1, 1),
		};
		// Act
		var dates = strategy.GetDates(calculationDate, latestDate);

		// Assert
		Assert.AreEqual(expectedDates.Length, dates.Length);
		for (int i = 0; i < expectedDates.Length; i++)
			Assert.AreEqual(expectedDates[i], dates[i]);

	}
}
