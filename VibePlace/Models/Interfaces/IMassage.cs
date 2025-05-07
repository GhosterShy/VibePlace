namespace VibePlace.Models.Interfaces
{
	public interface IMassage
	{
		public bool SendMessage(string to, string messageBody, string subject);
	}
}
