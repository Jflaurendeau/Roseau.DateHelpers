namespace Roseau.DateHelpers.UnitTests;

public static class AssertionExtensions
{
	public static void DoesNotThrow(this Assert _, Action action)
	{
		if (action is null)
			throw new ArgumentNullException(nameof(action));
		try
		{
			action();
		}
		catch (Exception)
		{

			Assert.Fail("This operation should not have thrown");
		}
	}
}
