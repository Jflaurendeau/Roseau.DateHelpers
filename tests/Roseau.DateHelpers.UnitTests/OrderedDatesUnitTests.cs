using Moq;
using Roseau.DateHelpers.DateArrayStrategies;
using System.Collections;

namespace Roseau.DateHelpers.UnitTests;

[TestClass]
public class OrderedDatesUnitTests
{
	readonly DateOnly[] dates;
	public OrderedDatesUnitTests() 
	{ 
		dates = CreateDates();
	}
	private static DateOnly[] CreateDates()
	{
		DateOnly[] dateArray = new DateOnly[120];
		for (int i = 1; i <= dateArray.Length; i++)
			dateArray[i - 1] = new(2020 + i, 1, 1);
		return dateArray;
	}

	
	[TestMethod]
	public void OrderedDates_ArgumentExceptionWithDatesNotSortedInArray_ThrowArgumentException()
	{
		// Arrange
		var strategy = new FirstDayOfEveryMonthStrategy();
		var dates = strategy.GetDates(new(2022, 3, 5), new(2023, 3, 5));
		dates[^1] = dates[0];
		// Act

		// Assert
		Assert.ThrowsException<ArgumentException>(() => new OrderedDates(dates));
	}
	[TestMethod]
	public void OrderedDates_ArgumentExceptionWithNullStrategy_ThrowArgumentNullException()
	{
		// Arrange
		// Act

		// Assert
		Assert.ThrowsException<ArgumentNullException>(() => new OrderedDates(null!, new(), new()));
	}
	[TestMethod]
	public void OrderedDates_ArgumentExceptionWithStrategyNotSortedDates_ThrowArgumentException()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();
		var dates = strategy.GetDates(calculationDate, lastDate);
		dates[^1] = dates[0];

		var mockIStrategy = new Mock<IDateArrayStrategy>();
		mockIStrategy.Setup(m => m.GetDates(calculationDate, lastDate)).Returns(dates);
		// Act

		// Assert
		Assert.ThrowsException<ArgumentException>(() => new OrderedDates(mockIStrategy.Object, calculationDate, lastDate));
	}
	[TestMethod]
	public void OrderedDates_Count_ReturnGoodCount()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();
		
		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var dates = new OrderedDates(strategy, calculationDate, lastDate);

		// Assert
		Assert.AreEqual(expectedDates.Length, dates.Count);
	}
	[TestMethod]
	public void OrderedDates_ContainsReallyAValue_ReturnTrue()
	{
		// Arrange
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var dates = new OrderedDates(strategy, new(2022, 2, 15), new(2023, 2, 28));

		// Assert
		Assert.IsTrue(dates.Contains(new(2022,3,1)));
	}
	[TestMethod]
	public void OrderedDates_DoesNotContainsAValue_ReturnFalse()
	{
		// Arrange
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var dates = new OrderedDates(strategy, new(2022, 2, 15), new(2023, 2, 28));

		// Assert
		Assert.IsFalse(dates.Contains(new(2022, 3, 2)));
	}
	[TestMethod]
	public void IsSorted_GivenASortedArry_ReturnsTrue()
	{
		// Arrange

		// Act

		// Assert
		Assert.IsTrue(OrderedDates.IsSorted(dates));
	}
	[TestMethod]
	public void IsSorted_GivenAnUnsortedArry_ReturnsTrue()
	{
		// Arrange
		var temporaryDate = dates.ToArray();
		temporaryDate[^1] = temporaryDate[0];

		// Act

		// Assert
		Assert.IsFalse(OrderedDates.IsSorted(temporaryDate));
	}
	[TestMethod]
	public void IsSorted_ArrayOfLessThanTwoElements_ReturnsTrue()
	{
		// Arrange
		DateOnly[] dates0 = Array.Empty<DateOnly>();
		DateOnly[] dates1 = new DateOnly[1];

		// Act

		// Assert
		Assert.IsTrue(OrderedDates.IsSorted(dates0));
		Assert.IsTrue(OrderedDates.IsSorted(dates1));
	}
	[TestMethod]
	public void IsSorted_ArgumentNull_ThrowArgumentNullException()
	{
		// Arrange
		// Act

		// Assert
		Assert.ThrowsException<ArgumentNullException>(() => OrderedDates.IsSorted(null!));
	}
	[TestMethod]
	public void Indexer_IndexOfIndexerSameAsArray_ReturnsTrue()
	{
		// Arrange
		var orderedDates = new OrderedDates(dates);

		// Act

		// Assert
		for(int i = 0; i < dates.Length; i++)
		{
			Assert.AreEqual(dates[i], orderedDates[i]);
		}
	}
	[TestMethod]
	public void Enumerator_WorkWellInForEach_ReturnsTrue()
	{
		// Arrange
		var orderedDates = new OrderedDates(dates);
		int i = 0;
		// Act

		// Assert
		foreach (var item in orderedDates)
		{
			Assert.AreEqual(dates[i], item);
			i++;
		}
	}
	[TestMethod]
	public void Enumerator_GetIEnumerator_ReturnsTrue()
	{
		// Arrange
		var orderedDates = new OrderedDates(dates);
		IEnumerable enumerable = (IEnumerable)orderedDates;
		int i = 0;

		// Act

		// Assert
		using (var enumerator = (IEnumerator<DateOnly>)enumerable.GetEnumerator())
		{

			while (enumerator.MoveNext())
			{
				Assert.AreEqual(dates[i], enumerator.Current);
				i++;
			}
		}
		Assert.AreEqual(dates.Length, i);
	}

}
