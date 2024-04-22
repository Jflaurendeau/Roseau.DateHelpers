using Roseau.DateHelpers;

namespace Roseau.DateHelpers.UnitTests;

[TestClass]
public class DateOnlyExtensionTests
{
    private static DateOnly[] Create2Dates()
    {
        DateOnly[] dateArray = new DateOnly[2];
        for (int i = 1; i <= dateArray.Length; i++)
            dateArray[i - 1] = new(2020 + i * i, 1 + i * i, 1 + i * i);
        return dateArray;
    }

    
    #region IsEqualOrBetweenDates Tests
    [TestMethod]
    public void IsEqualOrBetweenDates_IsLowerThanRange_ReturnsFalse()
    {
        // Arrange
        DateOnly[] dates = Create2Dates();
        DateOnly dateToTest = dates[0].AddDays(-1);
        bool isEqualOrBetweenDates;

        // Act
        isEqualOrBetweenDates = dateToTest.IsEqualOrBetweenDates(dates[0], dates[1]);

        // Assert
        Assert.IsFalse(isEqualOrBetweenDates);
    }
    [TestMethod]
    public void IsEqualOrBetweenDates_IsInsideRange_ReturnsTrue()
    {
        // Arrange
        DateOnly[] dates = Create2Dates();
        DateOnly dateToTest = dates[0].AddDays(1);
        bool isEqualOrBetweenDates;

        // Act
        isEqualOrBetweenDates = dateToTest.IsEqualOrBetweenDates(dates[0], dates[1]);

        // Assert
        Assert.IsTrue(isEqualOrBetweenDates);
    }
    [TestMethod]
    public void IsEqualOrBetweenDates_IsOnBorderOfRange_ReturnsTrue()
    {
        // Arrange
        DateOnly[] dates = Create2Dates();
        DateOnly dateLowerBorder = dates[0];
        DateOnly dateUpperBorder = dates[1];
        bool isEqualOrBetweenDatesLowerBorder;
        bool isEqualOrBetweenDatesUpperBorder;

        // Act
        isEqualOrBetweenDatesLowerBorder = dateLowerBorder.IsEqualOrBetweenDates(dates[0], dates[1]);
        isEqualOrBetweenDatesUpperBorder = dateUpperBorder.IsEqualOrBetweenDates(dates[0], dates[1]);

        // Assert
        Assert.IsTrue(isEqualOrBetweenDatesLowerBorder, "When the date equal the lower border, the method should have returned true");
        Assert.IsTrue(isEqualOrBetweenDatesUpperBorder, "When the date equal the upper border, the method should have returned true");
    }
    [TestMethod]
    public void IsEqualOrBetweenDates_IsHigherThanRange_ReturnsFalse()
    {
        // Arrange
        DateOnly[] dates = Create2Dates();
        DateOnly dateToTest = dates[1].AddDays(1);
        bool isEqualOrBetweenDates;

        // Act
        isEqualOrBetweenDates = dateToTest.IsEqualOrBetweenDates(dates[0], dates[1]);

        // Assert
        Assert.IsFalse(isEqualOrBetweenDates);
    }
    #endregion

    #region Age calculator functions
    #region AgeCalculator
    #region Decimal
    [TestMethod]
    public void AgeCalculatorDecimal_IsSymmetricByDate_ReturnsTrue()
    {
        // Arrange
        DateOnly[] dates = Create2Dates();
        bool isSymmetric;

        // Act
        isSymmetric = dates[0].AgeCalculator<decimal>(dates[1]) == dates[1].AgeCalculator<decimal>(dates[0]);

        // Assert
        Assert.IsTrue(isSymmetric, "The age is a measure of time. If it start at date1 or start at date 2, it should give the same value.");
    }
    [TestMethod]
    [DataRow(-6)]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(11)]
    public void AgeCalculatorDecimal_OnlyTheYearIsDifferent_NotOnLeapYear_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2019, 3, 3);
        DateOnly date2 = date1.AddYears(i);
        decimal age = (decimal)Math.Abs(i);
        decimal calculatedAge;

        // Act
        calculatedAge = date1.AgeCalculator<decimal>(date2);

        // Assert
        Assert.AreEqual(age, calculatedAge);
    }
    [TestMethod]
    [DataRow(-6)]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(11)]
    public void AgeCalculatorDecimal_OnlyTheYearIsDifferent_OnLeapYearButNot29February_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 3, 3);
        DateOnly date2 = date1.AddYears(i);
        decimal age = (decimal)Math.Abs(i);
        decimal calculatedAge;

        // Act
        calculatedAge = date1.AgeCalculator<decimal>(date2);

        // Assert
        Assert.AreEqual(age, calculatedAge);
    }
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(11)]
    [DataRow(13)]
    public void AgeCalculatorDecimal_FirstDateOn29FebruaryAndLastDateOn28February_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = date1.AddYears(i);
        decimal age = (decimal)Math.Abs(i);
        decimal differentOfOneDay;
        decimal calculatedAge;

        // Act
        differentOfOneDay = 0;
        calculatedAge = date1.AgeCalculator<decimal>(date2);

        // Assert
        Assert.AreEqual(age + differentOfOneDay, calculatedAge);
    }
    [TestMethod]
    [DataRow(-9)]
    [DataRow(-7)]
    [DataRow(-6)]
    [DataRow(-1)]
    public void AgeCalculatorDecimal_FirstDateOn28FebruaryAndSecondDateOn29February_ReturnsAgeWithAllDecimalsEqualToDifferenceOfOneDay(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = date1.AddYears(i);
        decimal age = (decimal)Math.Abs(i);
        decimal differentOfOneDay;
        decimal calculatedAge;

        // Act
        differentOfOneDay = (decimal)1 / 366;
        calculatedAge = date1.AgeCalculator<decimal>(date2);

        // Assert
        Assert.AreEqual(age + differentOfOneDay, calculatedAge);
    }
    [TestMethod]
    [DataRow(-4)]
    [DataRow(-12)]
    [DataRow(0)]
    [DataRow(4)]
    [DataRow(24)]
    [DataRow(32)]
    public void AgeCalculatorDecimal_OnlyLeapYearAndOnlyOn29February_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = date1.AddYears(i);
        decimal age = (decimal)Math.Abs(i);
        decimal calculatedAge;

        // Act
        calculatedAge = date1.AgeCalculator<decimal>(date2);

        // Assert
        Assert.AreEqual(age, calculatedAge);
    }
    [TestMethod]
    public void AgeCalculatorDecimal_BaseDateOnLeapYearAndOn28February_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 28);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDecimal_BaseDateOnLeapYearAndOn29February_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 29);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDecimal_BaseDateOnLeapYearAndOn1March_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDecimal_BaseDateNotOnLeapYearAndOn28February_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDecimal_BaseDateNotOnLeapYearAndOn1March_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDecimal_CalculationDateNotOnLeapYearAndOn1March_AgeOfLatestOfTwoBaseDates_ReturnsLowerAgeOrEqualIf29February()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            if (firstCalculationDate.Day == 29) ageDifference = true;
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDecimal_CalculationDateNotOnLeapYearAndOn28February_AgeOfLatestOfTwoBaseDates_ReturnsLowerAgeOrEqualIf29February()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 2, 28);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            if (firstCalculationDate.Day == 29) ageDifference = true;
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDecimal_CalculationDateOnLeapYearAndOn1March_AgeOfLatestOfTwoBaseDates_ReturnsLowerAgey()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDecimal_CalculationDateOnLeapYearAndOn29February_AgeOfLatestOfTwoBaseDates_ReturnsLowerAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 29);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDecimal_CalculationDateOnLeapYearAndOn28February_AgeOfLatestOfTwoBaseDates_ReturnsLowerAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 28);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<decimal>(secondCalculationDate) > dateOn28February.AgeCalculator<decimal>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    #endregion

    #region Double
    [TestMethod]
    public void AgeCalculatorDouble_IsSymmetricByDate_ReturnsTrue()
    {
        // Arrange
        DateOnly[] dates = Create2Dates();
        bool isSymmetric;

        // Act
        isSymmetric = dates[0].AgeCalculator<double>(dates[1]) == dates[1].AgeCalculator<double>(dates[0]);

        // Assert
        Assert.IsTrue(isSymmetric, "The age is a measure of time. If it start at date1 or start at date 2, it should give the same value.");
    }
    [TestMethod]
    [DataRow(-6)]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(11)]
    public void AgeCalculatorDouble_OnlyTheYearIsDifferent_NotOnLeapYear_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2019, 3, 3);
        DateOnly date2 = date1.AddYears(i);
        var age = (double)Math.Abs(i);

        // Act
        var calculatedAge = date1.AgeCalculator<double>(date2);

        // Assert
        Assert.AreEqual(age, calculatedAge);
    }
    [TestMethod]
    [DataRow(-6)]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(11)]
    public void AgeCalculatorDouble_OnlyTheYearIsDifferent_OnLeapYearButNot29February_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 3, 3);
        DateOnly date2 = date1.AddYears(i);
        var age = (double)Math.Abs(i);

        // Act
        var calculatedAge = date1.AgeCalculator<double>(date2);

        // Assert
        Assert.AreEqual(age, calculatedAge);
    }
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(11)]
    [DataRow(13)]
    public void AgeCalculatorDouble_FirstDateOn29FebruaryAndLastDateOn28February_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = date1.AddYears(i);
        var age = (double)Math.Abs(i);

        // Act
        var differentOfOneDay = 0.0;
        var calculatedAge = date1.AgeCalculator<double>(date2);

        // Assert
        Assert.AreEqual(age + differentOfOneDay, calculatedAge);
    }
    [TestMethod]
    [DataRow(-9)]
    [DataRow(-7)]
    [DataRow(-6)]
    [DataRow(-1)]
    public void AgeCalculatorDouble_FirstDateOn28FebruaryAndSecondDateOn29February_ReturnsAgeWithAllDecimalsEqualToDifferenceOfOneDay(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = date1.AddYears(i);
        var age = (double)Math.Abs(i);

        // Act
        var differentOfOneDay = (double)1 / 366;
        var calculatedAge = date1.AgeCalculator<double>(date2);

        // Assert
        Assert.AreEqual(age + differentOfOneDay, calculatedAge);
    }
    [TestMethod]
    [DataRow(-4)]
    [DataRow(-12)]
    [DataRow(0)]
    [DataRow(4)]
    [DataRow(24)]
    [DataRow(32)]
    public void AgeCalculatorDouble_OnlyLeapYearAndOnlyOn29February_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = date1.AddYears(i);
        var age = (double)Math.Abs(i);

        // Act
        var calculatedAge = date1.AgeCalculator<double>(date2);

        // Assert
        Assert.AreEqual(age, calculatedAge);
    }
    [TestMethod]
    public void AgeCalculatorDouble_BaseDateOnLeapYearAndOn28February_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 28);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDouble_BaseDateOnLeapYearAndOn29February_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 29);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDouble_BaseDateOnLeapYearAndOn1March_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDouble_BaseDateNotOnLeapYearAndOn28February_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDouble_BaseDateNotOnLeapYearAndOn1March_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDouble_CalculationDateNotOnLeapYearAndOn1March_AgeOfLatestOfTwoBaseDates_ReturnsLowerAgeOrEqualIf29February()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            if (firstCalculationDate.Day == 29) ageDifference = true;
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDouble_CalculationDateNotOnLeapYearAndOn28February_AgeOfLatestOfTwoBaseDates_ReturnsLowerAgeOrEqualIf29February()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 2, 28);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            if (firstCalculationDate.Day == 29) ageDifference = true;
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDouble_CalculationDateOnLeapYearAndOn1March_AgeOfLatestOfTwoBaseDates_ReturnsLowerAgey()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDouble_CalculationDateOnLeapYearAndOn29February_AgeOfLatestOfTwoBaseDates_ReturnsLowerAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 29);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorDouble_CalculationDateOnLeapYearAndOn28February_AgeOfLatestOfTwoBaseDates_ReturnsLowerAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 28);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<double>(secondCalculationDate) > dateOn28February.AgeCalculator<double>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    #endregion

    #region Float
    [TestMethod]
    public void AgeCalculatorFloat_IsSymmetricByDate_ReturnsTrue()
    {
        // Arrange
        DateOnly[] dates = Create2Dates();
        bool isSymmetric;

        // Act
        isSymmetric = dates[0].AgeCalculator<float>(dates[1]) == dates[1].AgeCalculator<float>(dates[0]);

        // Assert
        Assert.IsTrue(isSymmetric, "The age is a measure of time. If it start at date1 or start at date 2, it should give the same value.");
    }
    [TestMethod]
    [DataRow(-6)]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(11)]
    public void AgeCalculatorFloat_OnlyTheYearIsDifferent_NotOnLeapYear_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2019, 3, 3);
        DateOnly date2 = date1.AddYears(i);
        var age = (double)Math.Abs(i);

        // Act
        var calculatedAge = date1.AgeCalculator<float>(date2);

        // Assert
        Assert.AreEqual(age, calculatedAge);
    }
    [TestMethod]
    [DataRow(-6)]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(11)]
    public void AgeCalculatorFloat_OnlyTheYearIsDifferent_OnLeapYearButNot29February_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 3, 3);
        DateOnly date2 = date1.AddYears(i);
        var age = (double)Math.Abs(i);

        // Act
        var calculatedAge = date1.AgeCalculator<float>(date2);

        // Assert
        Assert.AreEqual(age, calculatedAge);
    }
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(11)]
    [DataRow(13)]
    public void AgeCalculatorFloat_FirstDateOn29FebruaryAndLastDateOn28February_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = date1.AddYears(i);
        var age = (float)Math.Abs(i);

        // Act
        var differentOfOneDay = 0f;
        var calculatedAge = date1.AgeCalculator<float>(date2);

        // Assert
        Assert.AreEqual(age + differentOfOneDay, calculatedAge);
    }
    [TestMethod]
    [DataRow(-9)]
    [DataRow(-7)]
    [DataRow(-6)]
    [DataRow(-1)]
    public void AgeCalculatorFloat_FirstDateOn28FebruaryAndSecondDateOn29February_ReturnsAgeWithAllDecimalsEqualToDifferenceOfOneDay(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = date1.AddYears(i);
        var age = (float)Math.Abs(i);

        // Act
        var differentOfOneDay = (float)1 / 366;
        var calculatedAge = date1.AgeCalculator<float>(date2);

        // Assert
        Assert.AreEqual(age + differentOfOneDay, calculatedAge);
    }
    [TestMethod]
    [DataRow(-4)]
    [DataRow(-12)]
    [DataRow(0)]
    [DataRow(4)]
    [DataRow(24)]
    [DataRow(32)]
    public void AgeCalculatorFloat_OnlyLeapYearAndOnlyOn29February_ReturnsAgeWithAllDecimalsEqualToZero(int i)
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = date1.AddYears(i);
        var age = (float)Math.Abs(i);

        // Act
        var calculatedAge = date1.AgeCalculator<float>(date2);

        // Assert
        Assert.AreEqual(age, calculatedAge);
    }
    [TestMethod]
    public void AgeCalculatorFloat_BaseDateOnLeapYearAndOn28February_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 28);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorFloat_BaseDateOnLeapYearAndOn29February_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 29);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorFloat_BaseDateOnLeapYearAndOn1March_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorFloat_BaseDateNotOnLeapYearAndOn28February_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorFloat_BaseDateNotOnLeapYearAndOn1March_AgeOfLatestOfTwoCalculationDates_ReturnsGreaterAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(1);
        }
    }
    [TestMethod]
    public void AgeCalculatorFloat_CalculationDateNotOnLeapYearAndOn1March_AgeOfLatestOfTwoBaseDates_ReturnsLowerAgeOrEqualIf29February()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            if (firstCalculationDate.Day == 29) ageDifference = true;
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorFloat_CalculationDateNotOnLeapYearAndOn28February_AgeOfLatestOfTwoBaseDates_ReturnsLowerAgeOrEqualIf29February()
    {
        // Arrange
        DateOnly dateOn28February = new(2021, 2, 28);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            if (firstCalculationDate.Day == 29) ageDifference = true;
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorFloat_CalculationDateOnLeapYearAndOn1March_AgeOfLatestOfTwoBaseDates_ReturnsLowerAgey()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 3, 1);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorFloat_CalculationDateOnLeapYearAndOn29February_AgeOfLatestOfTwoBaseDates_ReturnsLowerAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 29);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    [TestMethod]
    public void AgeCalculatorFloat_CalculationDateOnLeapYearAndOn28February_AgeOfLatestOfTwoBaseDates_ReturnsLowerAge()
    {
        // Arrange
        DateOnly dateOn28February = new(2020, 2, 28);
        DateOnly firstCalculationDate = dateOn28February.AddDays(-1);
        DateOnly secondCalculationDate = firstCalculationDate.AddDays(-1);
        bool ageDifference;

        for (int i = 1; i <= 366 * 5; i++)
        {
            // Act
            ageDifference = dateOn28February.AgeCalculator<float>(secondCalculationDate) > dateOn28February.AgeCalculator<float>(firstCalculationDate);
            // Assert
            Assert.IsTrue(ageDifference, $"Test fail when {nameof(firstCalculationDate)} is: {firstCalculationDate} and when {nameof(secondCalculationDate)} is: {secondCalculationDate}");
            firstCalculationDate = secondCalculationDate;
            secondCalculationDate = secondCalculationDate.AddDays(-1);
        }
    }
    #endregion

    #endregion

    #region AgeNearestBirthday
    [TestMethod]
    public void AgeNearestBirthday_AgeDecimalsNearerToCeiling_ReturnsFutureAge()
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = new(2020, 9, 29);
        int goodFutureAge = 1;
        int testFutureAge;
        bool isNearestAgeIsFutureAge;

        // Act
        testFutureAge = date1.AgeNearestBirthday(date2);
        isNearestAgeIsFutureAge = goodFutureAge == testFutureAge;

        // Assert
        Assert.IsTrue(isNearestAgeIsFutureAge);
    }
    [TestMethod]
    public void AgeNearestBirthday_AgeDecimalsNearerToFloor_ReturnsLastAge()
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = new(2020, 6, 29);
        int goodFutureAge = 0;
        int testFutureAge;
        bool isNearestAgeIsFutureAge;

        // Act
        testFutureAge = date1.AgeNearestBirthday(date2);
        isNearestAgeIsFutureAge = goodFutureAge == testFutureAge;

        // Assert
        Assert.IsTrue(isNearestAgeIsFutureAge);
    }
    #endregion

    #region AgeLastBirthday
    [TestMethod]
    public void AgeLastBirthday_OnLeapYearOn29FebruaryTo28FebruaryOfNextYear_ReturnsOne()
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = new(2021, 2, 28);
        int goodAge = 1;
        int testAge;

        // Act
        testAge = date1.AgeLastBirthday(date2);

        // Assert
        Assert.AreEqual(goodAge, testAge);
    }
    [TestMethod]
    public void AgeLastBirthday_OnLeapYearOn29FebruaryTo27FebruaryOfNextYear_ReturnsZero()
    {
        // Arrange
        DateOnly date1 = new(2020, 2, 29);
        DateOnly date2 = new(2021, 2, 27);
        int goodAge = 0;
        int testAge;

        // Act
        testAge = date1.AgeLastBirthday(date2);

        // Assert
        Assert.AreEqual(goodAge, testAge);
    }
    #endregion
    #endregion


    #region Difference between two dates
    #region TimeElapsedFromStartOfYear
    #region Decimal
    [TestMethod]
    public void TimeElapsedFromStartOfYearDecimal_OnLeapYear_ReturnsRatioOfNumberOfCompletedDaysInYearDividedBy366Days()
    {
        // Arrange
        DateOnly date = new(2020, 1, 1);
        decimal trueTimeElapsed;
        decimal testTimeElapsed;

        for (decimal i = 0m; i < 366; i++)
        {
            // Act
            testTimeElapsed = date.TimeElapsedFromStartOfYear<decimal>();
            trueTimeElapsed = i / 366;

            // Assert
            Assert.AreEqual(trueTimeElapsed, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the i-th day of the year: {i}");
            date = date.AddDays(1);
        }
        // Last Act
        testTimeElapsed = date.TimeElapsedFromStartOfYear<decimal>();

        // Last Assert
        Assert.AreEqual(0m, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the first day of new year");

    }
    [TestMethod]
    public void TimeElapsedFromStartOfYearDecimal_NotLeapYear_ReturnsRatioOfNumberOfCompletedDaysInYearDividedBy365Days()
    {
        // Arrange
        DateOnly date = new(2021, 1, 1);
        decimal trueTimeElapsed;
        decimal testTimeElapsed;

        for (decimal i = 0m; i < 365; i++)
        {
            // Act
            testTimeElapsed = date.TimeElapsedFromStartOfYear<decimal>();
            trueTimeElapsed = i / 365;

            // Assert
            Assert.AreEqual(trueTimeElapsed, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the i-th day of the year: {i}");
            date = date.AddDays(1);
        }
        // Last Act
        testTimeElapsed = date.TimeElapsedFromStartOfYear<decimal>();

        // Last Assert
        Assert.AreEqual(0m, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the first day of new year");

    }
    #endregion
    #region Double
    [TestMethod]
    public void TimeElapsedFromStartOfYearDouble_OnLeapYear_ReturnsRatioOfNumberOfCompletedDaysInYearDividedBy366Days()
    {
        // Arrange
        DateOnly date = new(2020, 1, 1);
        double trueTimeElapsed;
        double testTimeElapsed;

        for (double i = 0.0; i < 366; i++)
        {
            // Act
            testTimeElapsed = date.TimeElapsedFromStartOfYear<double>();
            trueTimeElapsed = i / 366;

            // Assert
            Assert.AreEqual(trueTimeElapsed, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the i-th day of the year: {i}");
            date = date.AddDays(1);
        }
        // Last Act
        testTimeElapsed = date.TimeElapsedFromStartOfYear<double>();

        // Last Assert
        Assert.AreEqual(0.0, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the first day of new year");

    }
    [TestMethod]
    public void TimeElapsedFromStartOfYearDouble_NotLeapYear_ReturnsRatioOfNumberOfCompletedDaysInYearDividedBy365Days()
    {
        // Arrange
        DateOnly date = new(2021, 1, 1);
        double trueTimeElapsed;
        double testTimeElapsed;

        for (double i = 0.0; i < 365; i++)
        {
            // Act
            testTimeElapsed = date.TimeElapsedFromStartOfYear<double>();
            trueTimeElapsed = i / 365;

            // Assert
            Assert.AreEqual(trueTimeElapsed, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the i-th day of the year: {i}");
            date = date.AddDays(1);
        }
        // Last Act
        testTimeElapsed = date.TimeElapsedFromStartOfYear<double>();

        // Last Assert
        Assert.AreEqual(0.0, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the first day of new year");

    }
    #endregion
    #region Float
    [TestMethod]
    public void TimeElapsedFromStartOfYearFloat_OnLeapYear_ReturnsRatioOfNumberOfCompletedDaysInYearDividedBy366Days()
    {
        // Arrange
        DateOnly date = new(2020, 1, 1);
        float trueTimeElapsed;
        float testTimeElapsed;

        for (float i = 0f; i < 366; i++)
        {
            // Act
            testTimeElapsed = date.TimeElapsedFromStartOfYear<float>();
            trueTimeElapsed = i / 366;

            // Assert
            Assert.AreEqual(trueTimeElapsed, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the i-th day of the year: {i}");
            date = date.AddDays(1);
        }
        // Last Act
        testTimeElapsed = date.TimeElapsedFromStartOfYear<float>();

        // Last Assert
        Assert.AreEqual(0f, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the first day of new year");

    }
    [TestMethod]
    public void TimeElapsedFromStartOfYearFloat_NotLeapYear_ReturnsRatioOfNumberOfCompletedDaysInYearDividedBy365Days()
    {
        // Arrange
        DateOnly date = new(2021, 1, 1);
        float trueTimeElapsed;
        float testTimeElapsed;

        for (float i = 0f; i < 365; i++)
        {
            // Act
            testTimeElapsed = date.TimeElapsedFromStartOfYear<float>();
            trueTimeElapsed = i / 365;

            // Assert
            Assert.AreEqual(trueTimeElapsed, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the i-th day of the year: {i}");
            date = date.AddDays(1);
        }
        // Last Act
        testTimeElapsed = date.TimeElapsedFromStartOfYear<float>();

        // Last Assert
        Assert.AreEqual(0f, testTimeElapsed, $"Test fail when {nameof(date)} is: {date} which is the first day of new year");

    }
    #endregion
    #endregion

    #region Difference of elapsed time
    [TestMethod]
    public void DifferenceOfTimeElapsedFromStartOfYear_FirstParameterIsOldestDate_ReturnsAgeBasedOnPartialYearsBeginningOnJanuaryFirst()
    {
        // Arrange
        DateOnly date = new(2020, 2, 13);
        DateOnly date2 = date.AddDays(450);
        decimal trueTimeElapsed;
        decimal testTimeElapsed;

        
        // Act
        testTimeElapsed = date.DifferenceOfTimeElapsedFromStartOfYear<decimal>(date2);
        trueTimeElapsed = date.FirstDayOfTheYear().AgeCalculator<decimal>(date2) - date.FirstDayOfTheYear().AgeCalculator<decimal>(date);
        
        // Last Assert
        Assert.AreEqual(trueTimeElapsed, testTimeElapsed, "Test fail when the first parameter is an older date than the second one");
    }
    [TestMethod]
    public void DifferenceOfTimeElapsedFromStartOfYear_SecondParameterIsOldestDate_ReturnsAgeBasedOnPartialYearsBeginningOnJanuaryFirst()
    {
        // Arrange
        DateOnly date = new(2020, 2, 13);
        DateOnly date2 = date.AddDays(450);
        decimal trueTimeElapsed;
        decimal testTimeElapsed;


        // Act
        testTimeElapsed = date2.DifferenceOfTimeElapsedFromStartOfYear<decimal>(date);
        trueTimeElapsed = date.FirstDayOfTheYear().AgeCalculator<decimal>(date2) - date.FirstDayOfTheYear().AgeCalculator<decimal>(date);

        // Last Assert
        Assert.AreEqual(trueTimeElapsed, testTimeElapsed, "Test fail when the first parameter is an older date than the second one");
    }
    #endregion
    #endregion


    #region NumberOfCompleteYears
    [TestMethod]
    public void NumberOfCompleteYears_EqualsAgeLastBirthday()
    {
        // Arrange
        DateOnly[] dates = Create2Dates();
        int trueNumberOfCompleteYears;
        int testNumberOfCompleteYears;

        // Act
        trueNumberOfCompleteYears = dates[0].AgeLastBirthday(dates[1]);
        testNumberOfCompleteYears = dates[0].NumberOfCompleteYears(dates[1]);

        // Assert
        Assert.AreEqual(trueNumberOfCompleteYears, testNumberOfCompleteYears);
    }
    #endregion

    #region NumberOfCompleteMonths
    [TestMethod]
    public void NumberOfCompleteMonths_MonthFirstDateIsLowerThanMonthSecondDate_DayOfFirstDateIsLowerThanDayOfSecondDate_ReturnsRightNumberOfFullMonths()
    {
        // Arrange
        DateOnly[] dates = new DateOnly[] 
        {
            new(2020, 2, 3),
            new(2026, 3, 4)
        };
        DateOnly temporaryDate;
        int numberOfCompleteYears;
        int completedMonthOfLastYear = 0;
        int trueNumberOfCompleteMonths;
        int testNumberOfCompleteMonths;

        // Act
        numberOfCompleteYears = dates[0].NumberOfCompleteYears(dates[1]);
        temporaryDate = dates[0].AddYears(numberOfCompleteYears);
        for (; completedMonthOfLastYear < 12; completedMonthOfLastYear++)
        {
            if (temporaryDate.AddMonths(completedMonthOfLastYear) >= dates[1]) break;
        }
        trueNumberOfCompleteMonths = completedMonthOfLastYear - 1 + 12 * numberOfCompleteYears;
        testNumberOfCompleteMonths = dates[0].NumberOfCompleteMonths(dates[1]);

        // Assert
        Assert.AreEqual(trueNumberOfCompleteMonths, testNumberOfCompleteMonths);
    }
    [TestMethod]
    public void NumberOfCompleteMonths_MonthFirstDateIsLowerThanMonthSecondDate_DayOfFirstDateIsGreaterThanDayOfSecondDate_ReturnsRightNumberOfFullMonths()
    {
        // Arrange
        DateOnly[] dates = new DateOnly[]
        {
            new(2020, 2, 5),
            new(2026, 3, 4)
        };
        DateOnly temporaryDate;
        int numberOfCompleteYears;
        int completedMonthOfLastYear = 0;
        int trueNumberOfCompleteMonths;
        int testNumberOfCompleteMonths;

        // Act
        numberOfCompleteYears = dates[0].NumberOfCompleteYears(dates[1]);
        temporaryDate = dates[0].AddYears(numberOfCompleteYears);
        for (; completedMonthOfLastYear < 12; completedMonthOfLastYear++)
        {
            if (temporaryDate.AddMonths(completedMonthOfLastYear) >= dates[1]) break;
        }
        trueNumberOfCompleteMonths = completedMonthOfLastYear - 1 + 12 * numberOfCompleteYears;
        testNumberOfCompleteMonths = dates[0].NumberOfCompleteMonths(dates[1]);

        // Assert
        Assert.AreEqual(trueNumberOfCompleteMonths, testNumberOfCompleteMonths);
    }
    [TestMethod]
    public void NumberOfCompleteMonths_MonthFirstDateIsGreaterThanMonthSecondDate_DayOfFirstDateIsLowerThanDayOfSecondDate_ReturnsRightNumberOfFullMonths()
    {
        // Arrange
        DateOnly[] dates = new DateOnly[]
        {
            new(2020, 4, 3),
            new(2026, 3, 4)
        };
        DateOnly temporaryDate;
        int numberOfCompleteYears;
        int completedMonthOfLastYear = 0;
        int trueNumberOfCompleteMonths;
        int testNumberOfCompleteMonths;

        // Act
        numberOfCompleteYears = dates[0].NumberOfCompleteYears(dates[1]);
        temporaryDate = dates[0].AddYears(numberOfCompleteYears);
        for (; completedMonthOfLastYear < 12; completedMonthOfLastYear++)
        {
            if (temporaryDate.AddMonths(completedMonthOfLastYear) >= dates[1]) break;
        }
        trueNumberOfCompleteMonths = completedMonthOfLastYear - 1 + 12 * numberOfCompleteYears;
        testNumberOfCompleteMonths = dates[0].NumberOfCompleteMonths(dates[1]);

        // Assert
        Assert.AreEqual(trueNumberOfCompleteMonths, testNumberOfCompleteMonths);
    }
    [TestMethod]
    public void NumberOfCompleteMonths_MonthFirstDateIsGreaterThanMonthSecondDate_DayOfFirstDateIsGreaterThanDayOfSecondDate_ReturnsRightNumberOfFullMonths()
    {
        // Arrange
        DateOnly[] dates = new DateOnly[]
        {
            new(2020, 4, 5),
            new(2026, 3, 4)
        };
        DateOnly temporaryDate;
        int numberOfCompleteYears;
        int completedMonthOfLastYear = 0;
        int trueNumberOfCompleteMonths;
        int testNumberOfCompleteMonths;

        // Act
        numberOfCompleteYears = dates[0].NumberOfCompleteYears(dates[1]);
        temporaryDate = dates[0].AddYears(numberOfCompleteYears);
        for (; completedMonthOfLastYear < 12; completedMonthOfLastYear++)
        {
            if (temporaryDate.AddMonths(completedMonthOfLastYear) >= dates[1]) break;
        }
        trueNumberOfCompleteMonths = completedMonthOfLastYear - 1 + 12 * numberOfCompleteYears;
        testNumberOfCompleteMonths = dates[0].NumberOfCompleteMonths(dates[1]);

        // Assert
        Assert.AreEqual(trueNumberOfCompleteMonths, testNumberOfCompleteMonths);
    }
    #endregion

    #region Determine new dates
    [TestMethod]
    public void FirstDayOfTheMonth_AreEquals()
    {
        // Arrange
        DateOnly date = new(2023, 4, 7);
        DateOnly date2 = new(2023, 4, 1);

        // Act
        var firstDayOfMonth = date.FirstDayOfTheMonth();

        // Assert
        Assert.AreEqual(date2, firstDayOfMonth);
    }
    [TestMethod]
    public void FirstDayOfFollowingMonth_AreEquals()
    {
        // Arrange
        DateOnly date = new(2023, 4, 7);

        // Act
        var expected = date.FirstDayOfTheMonth().AddMonths(1);
        var actual = date.FirstDayOfFollowingMonth();

        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void LastDayOfTheMonth_AreEquals()
    {
        // Arrange
        DateOnly date = new(2023, 4, 7);

        // Act
        var expected = date.FirstDayOfTheMonth().AddMonths(1).AddDays(-1);
        var actual = date.LastDayOfTheMonth();

        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void FirstDayOfFollowingOrCoincidantMonth_DateIsAFirstDayOfMonth_AreEquals()
    {
        // Arrange
        DateOnly date = new(2023, 4, 1);

        // Act
        var expected = date.FirstDayOfTheMonth().AddMonths(date.Day == 1 ? 0 : 1);
        var actual = date.FirstDayOfFollowingOrCoincidantMonth();

        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void FirstDayOfFollowingOrCoincidantMonth_DateIsNotFirstDayOfMonth_AreEquals()
    {
        // Arrange
        DateOnly date = new(2023, 4, 2);

        // Act
        var expected = date.FirstDayOfTheMonth().AddMonths(date.Day == 1 ? 0 : 1);
        var actual = date.FirstDayOfFollowingOrCoincidantMonth();

        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void FirstDayOfTheYear_AreEquals()
    {
        // Arrange
        DateOnly date = new(2023, 4, 2);

        // Act
        var expected = date.FirstDayOfTheMonth().AddMonths(1 - date.Month);
        var actual = date.FirstDayOfTheYear();

        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void FirstDayOfFollowingYear_AreEquals()
    {
        // Arrange
        DateOnly date = new(2023, 4, 2);

        // Act
        var expected = date.FirstDayOfTheMonth().AddMonths(13 - date.Month);
        var actual = date.FirstDayOfFollowingYear();

        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void FirstDayOfFollowingOrCoincidantYear_IsFirstDayOfTheYear_AreEquals()
    {
        // Arrange
        DateOnly date = new(2023, 1, 1);

        // Act
        var expected = date.FirstDayOfTheYear();
        var actual = date.FirstDayOfFollowingOrCoincidantYear();

        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void FirstDayOfFollowingOrCoincidantYear_IsNotFirstDayOfTheYear_AreEquals()
    {
        // Arrange
        DateOnly date = new(2023, 1, 2);

        // Act
        var expected = date.FirstDayOfTheYear().AddYears(1);
        var actual = date.FirstDayOfFollowingOrCoincidantYear();

        // Assert
        Assert.AreEqual(expected, actual);
    }

    #region Decimal
    [TestMethod]
    public void AddYearsDecimal_WhenAgeIsPositive_ReturnsTheLastDateCalculatedToGetAge()
    {
        // Arrange
        DateOnly[] dateOnlies = Create2Dates();
        DateOnly testDate;
        decimal age = dateOnlies[0].AgeCalculator<decimal>(dateOnlies[1]);

        // Act
        testDate = dateOnlies[0].AddYears(age);

        // Assert
        Assert.AreEqual(dateOnlies[1], testDate, $"Test fail when first and second dates {nameof(dateOnlies)} are: {dateOnlies[0]} and {dateOnlies[1]} which has an {nameof(age)} value of: {age}.");
    }
    [TestMethod]
    public void AddYearsDecimal_FirstDateNotOn29February_WhenAgeIsNegative_ReturnsTheFirstDateCalculatedToGetAge()
    {
        // Arrange
        DateOnly[] dateOnlies = Create2Dates();
        DateOnly testDate;
        decimal age = dateOnlies[0].AgeCalculator<decimal>(dateOnlies[1]);

        // Act
        testDate = dateOnlies[1].AddYears(-age);

        // Assert
        Assert.AreEqual(dateOnlies[0], testDate, $"Test fail when first and second dates {nameof(dateOnlies)} are: {dateOnlies[0]} and {dateOnlies[1]} which has an {nameof(age)} value of: {age}.");
    }
    [TestMethod]
    public void AddYearsDecimal_FirstDateOn29FebruaryAndLastDateNotOnLeapYear_WhenAgeIsNegative_ReturnsFebruary28CalculatedToGetAge()
    {
        // Arrange
        DateOnly[] dateOnlies =
        {
                new(2020, 2, 29),
                new(2025, 3, 3),
            };
        DateOnly testDate;
        decimal age = dateOnlies[0].AgeCalculator<decimal>(dateOnlies[1]);

        // Act
        testDate = dateOnlies[1].AddYears(-age);

        // Assert
        Assert.AreEqual(dateOnlies[0].AddDays(-1), testDate, $"Test fail when first and second dates {nameof(dateOnlies)} are: {dateOnlies[0]} and {dateOnlies[1]} which has an {nameof(age)} value of: {age}.");
    }
    #endregion
    #region Double
    [TestMethod]
    public void AddYearsDouble_WhenAgeIsPositive_ReturnsTheLastDateCalculatedToGetAge()
    {
        // Arrange
        DateOnly[] dateOnlies = Create2Dates();
        DateOnly testDate;
        var age = dateOnlies[0].AgeCalculator<double>(dateOnlies[1]);

        // Act
        testDate = dateOnlies[0].AddYears(age);

        // Assert
        Assert.AreEqual(dateOnlies[1], testDate, $"Test fail when first and second dates {nameof(dateOnlies)} are: {dateOnlies[0]} and {dateOnlies[1]} which has an {nameof(age)} value of: {age}.");
    }
    [TestMethod]
    public void AddYearsDouble_FirstDateNotOn29February_WhenAgeIsNegative_ReturnsTheFirstDateCalculatedToGetAge()
    {
        // Arrange
        DateOnly[] dateOnlies = Create2Dates();
        DateOnly testDate;
        var age = dateOnlies[0].AgeCalculator<double>(dateOnlies[1]);

        // Act
        testDate = dateOnlies[1].AddYears(-age);

        // Assert
        Assert.AreEqual(dateOnlies[0], testDate, $"Test fail when first and second dates {nameof(dateOnlies)} are: {dateOnlies[0]} and {dateOnlies[1]} which has an {nameof(age)} value of: {age}.");
    }
    [TestMethod]
    public void AddYearsDouble_FirstDateOn29FebruaryAndLastDateNotOnLeapYear_WhenAgeIsNegative_ReturnsFebruary28CalculatedToGetAge()
    {
        // Arrange
        DateOnly[] dateOnlies =
        {
                new(2020, 2, 29),
                new(2025, 3, 3),
            };
        DateOnly testDate;
        var age = dateOnlies[0].AgeCalculator<double>(dateOnlies[1]);

        // Act
        testDate = dateOnlies[1].AddYears(-age);

        // Assert
        Assert.AreEqual(dateOnlies[0].AddDays(-1), testDate, $"Test fail when first and second dates {nameof(dateOnlies)} are: {dateOnlies[0]} and {dateOnlies[1]} which has an {nameof(age)} value of: {age}.");
    }
    #endregion
    #region Float
    [TestMethod]
    public void AddYearsFloat_WhenAgeIsPositive_ReturnsTheLastDateCalculatedToGetAge()
    {
        // Arrange
        DateOnly[] dateOnlies = Create2Dates();
        DateOnly testDate;
        var age = dateOnlies[0].AgeCalculator<float>(dateOnlies[1]);

        // Act
        testDate = dateOnlies[0].AddYears(age);

        // Assert
        Assert.AreEqual(dateOnlies[1], testDate, $"Test fail when first and second dates {nameof(dateOnlies)} are: {dateOnlies[0]} and {dateOnlies[1]} which has an {nameof(age)} value of: {age}.");
    }
    [TestMethod]
    public void AddYearsFloat_FirstDateNotOn29February_WhenAgeIsNegative_ReturnsTheFirstDateCalculatedToGetAge()
    {
        // Arrange
        DateOnly[] dateOnlies = Create2Dates();
        DateOnly testDate;
        var age = dateOnlies[0].AgeCalculator<float>(dateOnlies[1]);

        // Act
        testDate = dateOnlies[1].AddYears(-age);

        // Assert
        Assert.AreEqual(dateOnlies[0], testDate, $"Test fail when first and second dates {nameof(dateOnlies)} are: {dateOnlies[0]} and {dateOnlies[1]} which has an {nameof(age)} value of: {age}.");
    }
    [TestMethod]
    public void AddYearsFloat_FirstDateOn29FebruaryAndLastDateNotOnLeapYear_WhenAgeIsNegative_ReturnsFebruary28CalculatedToGetAge()
    {
        // Arrange
        DateOnly[] dateOnlies =
        {
                new(2020, 2, 29),
                new(2025, 3, 3),
            };
        DateOnly testDate;
        var age = dateOnlies[0].AgeCalculator<float>(dateOnlies[1]);

        // Act
        testDate = dateOnlies[1].AddYears(-age);

        // Assert
        Assert.AreEqual(dateOnlies[0].AddDays(-1), testDate, $"Test fail when first and second dates {nameof(dateOnlies)} are: {dateOnlies[0]} and {dateOnlies[1]} which has an {nameof(age)} value of: {age}.");
    }
    #endregion
    #endregion
}