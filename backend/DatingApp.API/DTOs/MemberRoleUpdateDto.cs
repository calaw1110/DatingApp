namespace DatingApp.API.DTOs
{
	public class MemberRoleUpdateDto
	{
        public string Username { get; set; }

		public List<string> Roles { get; set; }
    }
}
