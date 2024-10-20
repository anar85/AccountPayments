namespace PaymentsApplication.Exceptions
{
	public class CustomException : Exception
	{
		public string Code { get; set; }
		public List<string>? Errors { get; set; }

		public CustomException(string code, string message) : base(message) => Code = code;
		public CustomException(string code, string message, List<string> errors) : base(message)
		{
			Code = code;
			Errors = errors;
		}
	}
}
