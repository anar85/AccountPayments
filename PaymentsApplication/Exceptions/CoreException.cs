namespace PaymentsApplication.Exceptions
#nullable disable
{
	public class CoreException

	{
		public string Code { get; set; }
		public string Message { get; set; }
		public List<string> Errors { get; set; }

		public CoreException() { }

		public CoreException(string code, string message)
		{
			Code = code;
			Message = message;
		}
		public CoreException(string code, string message, List<string> errors)
		{
			Code = code;
			Message = message;
			Errors = errors;
		}
	}
}
