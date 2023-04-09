using System.Collections.ObjectModel;
using System.Collections;
using Roseau.DateHelpers.DateArrayStrategies;

namespace Roseau.DateHelpers;

public sealed class OrderedDates : IEnumerable<DateOnly>, IEquatable<OrderedDates?>
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

	public ReadOnlySpan<DateOnly> AsSpan() => AsSpan(0);
	public ReadOnlySpan<DateOnly> AsSpan(int start) => AsSpan(start, dates.Length);
	public ReadOnlySpan<DateOnly> AsSpan(int start, int length) => new(dates, start, length);
	public ReadOnlyMemory<DateOnly> AsMemory() => AsMemory(0);
	public ReadOnlyMemory<DateOnly> AsMemory(int start) => AsMemory(start, dates.Length);
	public ReadOnlyMemory<DateOnly> AsMemory(int start, int length) => new(dates, start, length);

	public bool Contains(DateOnly date) => 0 <= BinarySearch(date);
	public int BinarySearch(DateOnly date) => Array.BinarySearch(dates, date);

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
	
	public override int GetHashCode() => ((IStructuralEquatable)dates).GetHashCode(EqualityComparer<DateOnly>.Default);
	public override bool Equals(object? obj)
	{
		if (obj is null)
			return false;
		if (obj.GetType() != this.GetType())
			return false;
		return Equals(obj as OrderedDates);
	}
	public bool Equals(OrderedDates? other)
	{
		return other is not null &&
			   ReferenceEquals(this, other);
	}
	public static bool operator ==(OrderedDates? left, OrderedDates? right)
	{
		return EqualityComparer<OrderedDates>.Default.Equals(left, right);
	}
	public static bool operator !=(OrderedDates? left, OrderedDates? right)
	{
		return !(left == right);
	}
}
