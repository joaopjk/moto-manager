namespace MotoManager.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a required field is invalid or missing.
    /// </summary>
    public class InvalidFieldException(string field) : ArgumentException(string.Format(Resource.FIELD_REQUIRED, field));
}
