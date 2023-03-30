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
		Assert.ThrowsException<ArgumentNullException>(() => OrderedDates.IsSorted(null));
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
	
}
