namespace WebApiApplication.HttpRestApi.Handles.Base;

public sealed record InternalServerErrorResponse : BaseResponse
{
    public override int StatusCode => 500;
    public override IResult AsResult() => Results.StatusCode(StatusCode);
    public override string ToLoggableString() => "Internal server error";
}