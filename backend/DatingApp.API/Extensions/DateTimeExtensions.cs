namespace DatingApp.API.Extensions
{
	public static class DateTimeExtensions
	{
		public static int CalculateAge(this DateOnly DateOfBirth)
		{
			var today = DateOnly.FromDateTime(DateTime.UtcNow);

			var age = today.Year - DateOfBirth.Year;

			if (DateOfBirth > today.AddDays(-age)) age--;
			return age;
		}
	}
}