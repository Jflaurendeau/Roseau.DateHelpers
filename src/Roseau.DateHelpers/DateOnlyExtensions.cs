using System.Numerics;

namespace Roseau.DateHelpers;

#region Delegate
public delegate int AgeDiscreteDelegate(DateOnly date, DateOnly calculationDate);
#endregion

/// <summary>
/// Extention methods to the DateOnly struct.
/// </summary>
public static class DateOnlyExtensions
{
    /// <summary>
    /// Determine if the date is between or equal to two dates.
    /// </summary>
    /// <param name="dateToTest">The instances of the date</param>
    /// <param name="firstDate">First date of the range</param>
    /// <param name="secondDate">Second date of the range</param>
    /// <returns>Return if it is or not in the range</returns>
    static public bool IsEqualOrBetweenDates(this DateOnly dateToTest, DateOnly firstDate, DateOnly secondDate)
    {
        DateOnly date1 = firstDate.FirstDate(secondDate);
        DateOnly date2 = firstDate.LastDate(secondDate);
        return (date1 <= dateToTest && dateToTest <= date2);
    }

    #region Age calculator function
    /// <summary>
    /// Calculate the exact age of the last date compared to the first date of the two
    /// </summary>
    /// <param name="date1">Date 1</param>
    /// <param name="date2">Date 2</param>
    /// <returns>The exact age</returns>
    static public TSelf AgeCalculator<TSelf>(this DateOnly date1, DateOnly date2)
        where TSelf : IFloatingPoint<TSelf>
    {
        DateOnly firstDate = date1.FirstDate(date2);
        DateOnly secondDate = date1.LastDate(date2);

        int numberOfYears = secondDate.Year - firstDate.Year;
        firstDate = firstDate.AddYears(numberOfYears);

        int numberDaysBeforeNearestBirthDay = secondDate.DayOfYear - firstDate.DayOfYear;
        int numberDaysLastYear = numberDaysBeforeNearestBirthDay > 0 ? firstDate.AddYears(1).DayNumber - firstDate.DayNumber : firstDate.DayNumber - firstDate.AddYears(-1).DayNumber;
        return TSelf.CreateSaturating(numberOfYears) + TSelf.CreateSaturating(numberDaysBeforeNearestBirthDay) / TSelf.CreateSaturating(numberDaysLastYear);
    }
    /// <summary>
    /// Age at the nearest birthday.
    /// </summary>
    /// <param name="date1">Date of birth</param>
    /// <param name="calculationDate">Date to compare</param>
    /// <returns>The round age at the calculation date.</returns>
    static public int AgeNearestBirthday(this DateOnly date1, DateOnly calculationDate)
    {
        return (int)Math.Round(AgeCalculator<float>(date1, calculationDate));
    }
    /// <summary>
    /// Age at the last birthday.
    /// </summary>
    /// <param name="date1">Date of birth</param>
    /// <param name="calculationDate">Date to compare</param>
    /// <returns>The last full integer age at the calculation date.</returns>
    static public int AgeLastBirthday(this DateOnly date1, DateOnly calculationDate)
    {
        return (int)AgeCalculator<float>(date1, calculationDate);
    }
    #endregion 

    #region Difference between two dates
    /// <summary>
    /// Determine the time elapsed from the stard of the year.
    /// </summary>
    /// <param name="date">The instance of DateTime</param>
    /// <returns>The partial year elapsed from the start of the year.</returns>
    static public TSelf TimeElapsedFromStartOfYear<TSelf>(this DateOnly date)
        where TSelf : IFloatingPoint<TSelf>
    {
        return AgeCalculator<TSelf>(new DateOnly(date.Year, 1, 1), date);
    }
    /// <summary>
    /// Difference of time elapsed from the start of the year between two dates.
    /// </summary>
    /// <param name="date1">Date 1</param>
    /// <param name="date2">Date 2</param>
    /// <returns>The partial year elapsed from the start of the year.</returns>
    static public TSelf DifferenceOfTimeElapsedFromStartOfYear<TSelf>(this DateOnly date1, DateOnly date2)
        where TSelf : IFloatingPoint<TSelf>
    {
        if (date1 > date2) return AgeCalculator<TSelf>(new DateOnly(date2.Year, 1, 1), date1) - date2.TimeElapsedFromStartOfYear<TSelf>();

        return AgeCalculator<TSelf>(new DateOnly(date1.Year, 1, 1), date2) - date1.TimeElapsedFromStartOfYear<TSelf>();
    }

    static public int NumberOfCompleteYears(this DateOnly firstDate, DateOnly secondDate)
    {
        return AgeLastBirthday(firstDate, secondDate);
    }
    static public int NumberOfCompleteMonths(this DateOnly firstDate, DateOnly secondDate)
    {
        int numberOfFullYears = AgeLastBirthday(firstDate, secondDate);
        int lastYearMonthDif = secondDate.Month - firstDate.Month;
        lastYearMonthDif = lastYearMonthDif <= 0 ? (lastYearMonthDif + 12) % 12 : lastYearMonthDif;
        lastYearMonthDif = secondDate.Day < firstDate.Day ? (lastYearMonthDif + 11) % 12 : lastYearMonthDif;

        return lastYearMonthDif + numberOfFullYears * 12;
    }
    #endregion

    #region Determine new dates
    /// <summary>
    /// Extension method that determine the first date between two dates
    /// </summary>
    /// <param name="date1">Date 1</param>
    /// <param name="date2">Date 2</param>
    /// <returns>The first date</returns>
    public static DateOnly FirstDate(this DateOnly date1, DateOnly date2)
    {
        return date1 > date2 ? date2 : date1;
    }
    /// <summary>
    /// Extension method that determine the last date between two dates
    /// </summary>
    /// <param name="date1">Date 1</param>
    /// <param name="date2">Date 2</param>
    /// <returns>The last date</returns>
    public static DateOnly LastDate(this DateOnly date1, DateOnly date2)
    {
        return date1 > date2 ? date1 : date2;
    }
    /// <summary>
    /// Determine the first day of the month
    /// </summary>
    /// <param name="date">The instance of DateTime</param>
    /// <returns>The first day of the month</returns>
    static public DateOnly FirstDayOfTheMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }
    /// <summary>
    /// Determine the first day of the following month
    /// </summary>
    /// <param name="date">The instance of DateTime</param>
    /// <returns>The first day of the following month</returns>
    static public DateOnly FirstDayOfFollowingMonth(this DateOnly date)
    {
        return FirstDayOfTheMonth(date).AddMonths(1);
    }
    /// <summary>
    /// Determine the last day of the month
    /// </summary>
    /// <param name="date">The instance of DateTime</param>
    /// <returns>The last day of the month</returns>
    static public DateOnly LastDayOfTheMonth(this DateOnly date)
    {
        return FirstDayOfFollowingMonth(date).AddDays(-1);
    }
    /// <summary>
    /// Determine the first day of the next month or coincidant month
    /// </summary>
    /// <param name="date">The instance of DateTime</param>
    /// <returns>The first day of the folowing month of the coincidant month</returns>
    static public DateOnly FirstDayOfFollowingOrCoincidantMonth(this DateOnly date)
    {
        if (date.Day == 1) return date;
        return FirstDayOfFollowingMonth(date);
    }
    /// <summary>
    /// Determine the first day of the year
    /// </summary>
    /// <param name="date">The instance of DateOnly</param>
    /// <returns>The first day of the year</returns>
    static public DateOnly FirstDayOfTheYear(this DateOnly date)
    {
        return new DateOnly(date.Year, 1, 1);
    }
    /// <summary>
    /// Determine the first day of the following year
    /// </summary>
    /// <param name="date">The instance of DateOnly</param>
    /// <returns>The first day of the following year</returns>
    static public DateOnly FirstDayOfFollowingYear(this DateOnly date)
    {
        return FirstDayOfTheYear(date).AddYears(1);
    }
    /// <summary>
    /// Determine the first day of the next year or coincidant if this first of this year
    /// </summary>
    /// <param name="date">The instance of DateOnly</param>
    /// <returns>The first day of the folowing year of the coincidant year</returns>
    static public DateOnly FirstDayOfFollowingOrCoincidantYear(this DateOnly date)
    {
        if (date.DayOfYear == 1) return date;
        return FirstDayOfFollowingYear(date);
    }
    /// <summary>
    /// Add de number of years to a date
    /// </summary>
    /// <param name="date">Basis date</param>
    /// <param name="numberOfYears">Number of years</param>
    /// <returns>The date resulted from the addition of years</returns>
    static public DateOnly AddYears<TSelf>(this DateOnly date, TSelf numberOfYears)
        where TSelf : IFloatingPoint<TSelf>
    {
        return numberOfYears switch
        {
            float years => date.AddYearsFloat(years),
            double years => date.AddYearsDouble(years),
            decimal years => date.AddYearsDecimal(years),
            _ => date.AddYearsFloatingPoint(numberOfYears),
        };
    }
    static private DateOnly AddYearsDecimal(this DateOnly date, decimal numberOfYears)
    {
        decimal test365 = (numberOfYears - (int)numberOfYears) * 365m;
        decimal test366 = (numberOfYears - (int)numberOfYears) * 366m;
        int numberOfDaysLastYear = Math.Abs(Math.Round(test365) - test365) < Math.Abs(Math.Round(test366) - test366) ? (int)Math.Round(test365) : (int)Math.Round(test366);
        if (numberOfYears < 0m)
            return date.AddDays(numberOfDaysLastYear).AddYears((int)numberOfYears);
        return date.AddYears((int)numberOfYears).AddDays(numberOfDaysLastYear);
    }
    static private DateOnly AddYearsDouble(this DateOnly date, double numberOfYears)
    {
        var test365 = (numberOfYears - (int)numberOfYears) * 365d;
        var test366 = (numberOfYears - (int)numberOfYears) * 366d;
        int numberOfDaysLastYear = Math.Abs(Math.Round(test365) - test365) < Math.Abs(Math.Round(test366) - test366) ? (int)Math.Round(test365) : (int)Math.Round(test366);
        if (numberOfYears < 0d)
            return date.AddDays(numberOfDaysLastYear).AddYears((int)numberOfYears);
        return date.AddYears((int)numberOfYears).AddDays(numberOfDaysLastYear);
    }
    static private DateOnly AddYearsFloat(this DateOnly date, float numberOfYears)
    {
        float test365 = (numberOfYears - (int)numberOfYears) * 365f;
        float test366 = (numberOfYears - (int)numberOfYears) * 366f;
        int numberOfDaysLastYear = Math.Abs(Math.Round(test365) - test365) < Math.Abs(Math.Round(test366) - test366) ? (int)Math.Round(test365) : (int)Math.Round(test366);
        if (numberOfYears < 0f)
            return date.AddDays(numberOfDaysLastYear).AddYears((int)numberOfYears);
        return date.AddYears((int)numberOfYears).AddDays(numberOfDaysLastYear);
    }
    static private DateOnly AddYearsFloatingPoint<TSelf>(this DateOnly date, TSelf numberOfYears)
        where TSelf : IFloatingPoint<TSelf>
    {
        var truncatedNumberOfYears = TSelf.Truncate(numberOfYears);
        var test365 = (numberOfYears - truncatedNumberOfYears) * TSelf.CreateSaturating(365);
        var test366 = (numberOfYears - truncatedNumberOfYears) * TSelf.CreateSaturating(366);
        int numberOfDaysLastYear = TSelf.Abs(TSelf.Round(test365) - test365) < TSelf.Abs(TSelf.Round(test366) - test366) ? int.CreateSaturating(TSelf.Round(test365)) : int.CreateSaturating(TSelf.Round(test366));
        if (numberOfYears < TSelf.CreateSaturating(0))
            return date.AddDays(numberOfDaysLastYear).AddYears(int.CreateSaturating(numberOfYears));
        return date.AddYears(int.CreateSaturating(numberOfYears)).AddDays(numberOfDaysLastYear);
    }
    #endregion
}