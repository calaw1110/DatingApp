using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Entities
{
	public class Connection
	{
		public Connection()
		{

		}
		public Connection(string connetionId, string username)
		{
			ConnetionId = connetionId;
			Username = username;
		}
		[Key]
		public string ConnetionId { get; set; }
		public string Username { get; set; }
	}
}
