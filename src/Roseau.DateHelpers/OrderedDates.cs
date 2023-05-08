using System.Collections.ObjectModel;
using System.Collections;
using Roseau.DateHelpers.DateArrayStrategies;

namespace Roseau.DateHelpers;

public sealed class OrderedDates : IEnumerable<DateOnly>
{
	readonly DateOnly[] dates;
	public OrderedDates(DateOnly[] dates)
	{
		if (!IsSorted(dates))
			throw new ArgumentException("The dates array must be ordered.", nameof(dates));
		this.dates = dates;
	}
	public OrderedDates(IDateArrayStrategy dateArrayStrategy, DateOnly calculationDate, DateOnly lastDate)
	{
		if (dateArrayStrategy is null) throw new ArgumentNullException(nameof(dateArrayStrategy));
		var datesFromStrategy = dateArrayStrategy.GetDates(calculationDate, lastDate);
		if (!IsSorted(datesFromStrategy)) throw new ArgumentException($"The dates array produced by the {nameof(dateArrayStrategy)} is not ordered.", nameof(dateArrayStrategy));
		this.dates = datesFromStrategy;
	}
	public int Count => dates.Length;
	public DateOnly this[int index] => dates[index];

	public ReadOnlySpan<DateOnly> AsSpan() => new(dates, 0, dates.Length);
	public ReadOnlySpan<DateOnly> AsSpan(int start) => new(dates, start, dates.Length - start);
	public ReadOnlySpan<DateOnly> AsSpan(int start, int length) => new(dates, start, length);
	public ReadOnlyMemory<DateOnly> AsMemory() => new(dates, 0, dates.Length);
	public ReadOnlyMemory<DateOnly> AsMemory(int start) => new(dates, start, dates.Length - start);
	public ReadOnlyMemory<DateOnly> AsMemory(int start, int length) => new(dates, start, length);

	public bool Contains(DateOnly date) => 0 <= BinarySearch(date);
	public int BinarySearch(DateOnly date) => Array.BinarySearch(dates, date);
	/// <summary>
	/// Get the index of the date, or if not found, the index of the element that comes before the feeded date.
	/// </summary>
	/// <param name="date">The date to search</param>
	/// <returns>If the feeded date is in the array, the array index associated with that date is returned. 
	/// If the date is earlier than the first date of the array, -1 is returned.
	/// Otherwise, the index of the nearest previous date is returned.</returns>
	public int IndexOfOrPreviousElement(DateOnly date)
	{
		int value = BinarySearch(date);
		if (value >= 0) return value;
		return ~value - 1;
	}

	public ReadOnlyCollection<DateOnly> AsCollection() => Array.AsReadOnly(dates);
	public IEnumerator<DateOnly> GetEnumerator() => AsCollection().GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	public static bool IsSorted(DateOnly[] dates)
	{
		if (dates is null)
			throw new ArgumentNullException(nameof(dates));
		int j = dates.Length - 1;
		if (j < 1) return true;
		int i = 1;
		DateOnly dateAti = dates[0];
		while (i <= j && dateAti <= (dateAti = dates[i])) i++;
		return i > j;
	}
}
