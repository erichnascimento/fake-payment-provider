namespace WebApiApplication.HttpRestApi.Handles.Base;

public sealed record BadRequestResponse(string Message) : BaseResponse
{
    public override int StatusCode => 400;
    public override IResult AsResult() => Results.BadRequest(Message);
    public override string ToLoggableString() => Message;
}