namespace AuthService.Domain.Enums
{
    public enum StatusCode
    {
        OK = 200,
        Unauthorized = 401,
        NotFound = 404,
        Conflict = 409,
        InternalServiceError = 500,
    }
}
