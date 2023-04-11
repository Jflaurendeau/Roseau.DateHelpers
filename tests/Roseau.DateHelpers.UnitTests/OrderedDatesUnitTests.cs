using Moq;
using Roseau.DateHelpers.DateArrayStrategies;
using System;
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

	#region Constructors
	[TestMethod]
	[TestCategory("Constructors")]
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
	[TestCategory("Constructors")]
	public void OrderedDates_ArgumentExceptionWithNullStrategy_ThrowArgumentNullException()
	{
		// Arrange
		// Act

		// Assert
		Assert.ThrowsException<ArgumentNullException>(() => new OrderedDates(null!, new(), new()));
	}
	[TestMethod]
	[TestCategory("Constructors")]
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
	#endregion


	[TestMethod]
	[TestCategory("Count")]
	public void Count_ReturnLengthOfArray_IsTrue()
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
	[TestCategory("AsSpan")]
	public void AsSpan_ReturnSpanOverArray_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var dates = new OrderedDates(strategy, calculationDate, lastDate);
		var datesSpan = dates.AsSpan();
		var count = dates.Count;

		// Assert
		for (int i = 0; i < count; i++)
		{
			Assert.AreEqual(dates[i], datesSpan[i]);
		}
	}
	[TestMethod]
	[TestCategory("AsSpan")]
	public void AsSpan_ReturnSpanOverWithStart_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var dates = new OrderedDates(strategy, calculationDate, lastDate);
		var startIndex = 10;
		var datesSpan = dates.AsSpan(startIndex);
		var count = dates.Count-startIndex;

		// Assert
		for (int i = 0; i < count; i++)
		{
			Assert.AreEqual(dates[i+startIndex], datesSpan[i]);
		}
	}
	[TestMethod]
	[TestCategory("AsSpan")]
	public void AsSpan_ReturnSpanOverArrayWithStartAndLenght_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var dates = new OrderedDates(strategy, calculationDate, lastDate);
		var startIndex = 5;
		var length = 2;
		var datesSpan = dates.AsSpan(startIndex, length);
		var count = length;

		// Assert
		for (int i = 0; i < count; i++)
		{
			Assert.AreEqual(dates[i+startIndex], datesSpan[i]);
		}
	}
	[TestMethod]
	[TestCategory("AsMemory")]
	public void AsMemory_ReturnMemoryOverArray_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var dates = new OrderedDates(strategy, calculationDate, lastDate);
		var datesMemory = dates.AsMemory();
		var datesSpan = datesMemory.Span;
		var count = dates.Count;

		// Assert
		for (int i = 0; i < count; i++)
		{
			Assert.AreEqual(dates[i], datesSpan[i]);
		}
	}
	[TestMethod]
	[TestCategory("AsMemory")]
	public void AsMemory_ReturnMemoryOverArrayWithStart_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();


		// Act
		var dates = new OrderedDates(strategy, calculationDate, lastDate);
		var startIndex = 10;
		var datesMemory = dates.AsMemory(startIndex);
		var datesSpan = datesMemory.Span;
		var count = dates.Count - startIndex;

		// Assert
		for (int i = 0; i < count; i++)
		{
			Assert.AreEqual(dates[i+startIndex], datesSpan[i]);
		}
	}
	[TestMethod]
	[TestCategory("AsMemory")]
	public void AsMemory_ReturnMemoryOverArrayWithStartAndLength_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var dates = new OrderedDates(strategy, calculationDate, lastDate);
		var startIndex = 5;
		var length = 2;
		var datesMemory = dates.AsMemory(startIndex, length);
		var datesSpan = datesMemory.Span;
		var count = length;

		// Assert
		for (int i = 0; i < count; i++)
		{
			Assert.AreEqual(dates[i+startIndex], datesSpan[i]);
		}
	}
	[TestMethod]
	[TestCategory("Contains")]
	public void Contains_FeededByARealValue_IsTrue()
	{
		// Arrange
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var dates = new OrderedDates(strategy, new(2022, 2, 15), new(2023, 2, 28));

		// Assert
		Assert.IsTrue(dates.Contains(new(2022,3,1)));
	}
	[TestMethod]
	[TestCategory("Contains")]
	public void Contains_DoesNotContainsAValue_IsFalse()
	{
		// Arrange
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var dates = new OrderedDates(strategy, new(2022, 2, 15), new(2023, 2, 28));

		// Assert
		Assert.IsFalse(dates.Contains(new(2022, 3, 2)));
	}
	[TestMethod]
	[TestCategory("IndexOfOrPreviousElement")]
	public void IndexOfOrPreviousElement_ElementIsInArray_ReturnGoodIndex()
	{
		// Arrange
		var strategy = new FirstDayOfEveryMonthStrategy();
		var expectedDates = strategy.GetDates(new(2022, 2, 15), new(2023, 2, 28));
		var dates = new OrderedDates(expectedDates);

		// Act
		var date = dates[dates.IndexOfOrPreviousElement(expectedDates[0])];
		// Assert
		Assert.AreEqual(expectedDates[0], date);
	}
	[TestMethod]
	[TestCategory("IndexOfOrPreviousElement")]
	public void IndexOfOrPreviousElement_ElementIsBeforeFirstElementOfArray_ReturnGoodIndex()
	{
		// Arrange
		var strategy = new FirstDayOfEveryMonthStrategy();
		var expectedDates = strategy.GetDates(new(2022, 2, 15), new(2023, 2, 28));
		var dates = new OrderedDates(expectedDates);

		// Act
		var searchedDate = new DateOnly(2022, 2, 2);
		var expectedIndex = -1;
		var index = dates.IndexOfOrPreviousElement(searchedDate);
		
		// Assert
		Assert.AreEqual(expectedIndex, index);
	}
	[TestMethod]
	[TestCategory("IndexOfOrPreviousElement")]
	public void IndexOfOrPreviousElement_ElementIsAfterFirstElementOfArray_ReturnGoodIndex()
	{
		// Arrange
		var strategy = new FirstDayOfEveryMonthStrategy();
		var expectedDates = strategy.GetDates(new(2022, 2, 15), new(2023, 2, 28));
		var dates = new OrderedDates(expectedDates);

		// Act
		var searchedDate = new DateOnly(2022, 3, 2);
		var expectedDate = new DateOnly(2022, 3, 1);
		var index = dates.IndexOfOrPreviousElement(searchedDate);
		var date = dates[index];
		// Assert
		Assert.AreEqual(expectedDate, date);
	}
	[TestMethod]
	[TestCategory("IndexOfOrPreviousElement")]
	public void IndexOfOrPreviousElement_ElementIsAfterLasElementOfArray_ReturnGoodIndex()
	{
		// Arrange
		var strategy = new FirstDayOfEveryMonthStrategy();
		var expectedDates = strategy.GetDates(new(2022, 2, 15), new(2023, 2, 28));
		var dates = new OrderedDates(expectedDates);

		// Act
		var searchedDate = new DateOnly(2023, 3, 1);
		var expectedDate = new DateOnly(2023, 2, 1);
		var index = dates.IndexOfOrPreviousElement(searchedDate);
		var date = dates[index];
		// Assert
		Assert.AreEqual(expectedDate, date);
	}
	[TestMethod]
	[TestCategory("IsSorted")]
	public void IsSorted_GivenASortedArry_ReturnsTrue()
	{
		// Arrange

		// Act

		// Assert
		Assert.IsTrue(OrderedDates.IsSorted(dates));
	}
	[TestMethod]
	[TestCategory("IsSorted")]
	public void IsSorted_GivenAnUnsortedArry_ReturnsFalse()
	{
		// Arrange
		var temporaryDate = dates.ToArray();
		temporaryDate[^1] = temporaryDate[0];

		// Act

		// Assert
		Assert.IsFalse(OrderedDates.IsSorted(temporaryDate));
	}
	[TestMethod]
	[TestCategory("IsSorted")]
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
	[TestCategory("IsSorted")]
	public void IsSorted_ArgumentNull_ThrowArgumentNullException()
	{
		// Arrange
		// Act

		// Assert
		Assert.ThrowsException<ArgumentNullException>(() => OrderedDates.IsSorted(null!));
	}
	[TestMethod]
	[TestCategory("IsSorted")]
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
	[TestCategory("AsCollection")]
	public void AsCollection_ReturnCollectionOfArray_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var dates = new OrderedDates(strategy, calculationDate, lastDate);
		var datesCollection = dates.AsCollection();
		var count = dates.Count;

		// Assert
		for (int i = 0; i < count; i++)
		{
			Assert.AreEqual(dates[i], datesCollection[i]);
		}
	}
	[TestMethod]
	[TestCategory("Enumerator")]
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
	[TestCategory("Enumerator")]
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
	[TestMethod]
	[TestCategory("GetHashCode")]
	public void GetHashCode_SameArrayWithTwoOrderedDate_ReturnSameHashCode()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = new OrderedDates(strategy, calculationDate, lastDate);

		// Assert
		Assert.AreEqual(expectedOrderedDates.GetHashCode(), dates.GetHashCode());
		
	}
	[TestMethod]
	[TestCategory("GetHashCode")]
	public void GetHashCode_DifferentArrayWithTwoOrderedDate_ReturnDifferentHashCode()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		expectedDates[^1] = lastDate;
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = new OrderedDates(strategy, calculationDate, lastDate);

		// Assert
		Assert.AreNotEqual(expectedOrderedDates.GetHashCode(), dates.GetHashCode());
	}
	[TestMethod]
	[TestCategory("Equals")]
	public void Equals_DifferentTypes_IsFalse()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = new OrderedDates(strategy, calculationDate, lastDate);

		// Assert
		Assert.IsFalse(expectedOrderedDates.Equals(calculationDate));
	}
	[TestMethod]
	[TestCategory("Equals")]
	public void Equals_ComparisonAgainstNullDifferentObject_IsFalse()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();
		FirstDayOfEveryMonthStrategy nullStrategy = default!;

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = new OrderedDates(strategy, calculationDate, lastDate);

		// Assert
		Assert.IsFalse(expectedOrderedDates.Equals(nullStrategy));
	}
	[TestMethod]
	[TestCategory("Equals")]
	public void Equals_ComparisonAgainstNullOrderedDates_IsFalse()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();
		OrderedDates nullOrderedDates = default!;

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = new OrderedDates(strategy, calculationDate, lastDate);

		// Assert
		Assert.IsFalse(expectedOrderedDates.Equals(nullOrderedDates));
	}
	[TestMethod]
	[TestCategory("Equals")]
	public void Equals_ComparisonAgainstDifferentOrderedDates_IsFalse()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = new OrderedDates(strategy, calculationDate, lastDate);

		// Assert
		Assert.IsFalse(expectedOrderedDates.Equals(dates));
	}
	[TestMethod]
	[TestCategory("Equals")]
	public void Equals_ComparisonAgainstSameOrderedDates_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = expectedOrderedDates;

		// Assert
		Assert.IsTrue(expectedOrderedDates.Equals(dates));
	}
	[TestMethod]
	[TestCategory("Equals")]
	public void Equals_ComparisonAgainstSameOrderedDatesBoxedAsObject_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = expectedOrderedDates;

		// Assert
		Assert.IsTrue(expectedOrderedDates.Equals((object)dates));
	}
	[TestMethod]
	[TestCategory("Equals")]
	public void EqualOperator_ComparisonAgainstSameOrderedDates_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = expectedOrderedDates;

		// Assert
		Assert.IsTrue(expectedOrderedDates == dates);
	}
	[TestMethod]
	[TestCategory("Equals")]
	public void NotEqualOperator_ComparisonAgainstDifferentOrderedDates_IsTrue()
	{
		// Arrange
		var calculationDate = new DateOnly(2022, 2, 15);
		var lastDate = new DateOnly(2023, 2, 28);
		var strategy = new FirstDayOfEveryMonthStrategy();

		// Act
		var expectedDates = strategy.GetDates(calculationDate, lastDate);
		var expectedOrderedDates = new OrderedDates(expectedDates);
		var dates = new OrderedDates(expectedDates);

		// Assert
		Assert.IsTrue(expectedOrderedDates != dates);
	}

}
