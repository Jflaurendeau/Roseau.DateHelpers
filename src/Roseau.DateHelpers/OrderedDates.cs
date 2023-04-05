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
	public ReadOnlyCollection<DateOnly> Dates => Array.AsReadOnly(dates);
	public ReadOnlySpan<DateOnly> AsSpan() => dates.AsSpan();
	public bool Contains(DateOnly date) => dates.Contains(date);
	public IEnumerator<DateOnly> GetEnumerator() => Dates.GetEnumerator();

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
