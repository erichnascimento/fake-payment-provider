namespace WebApiApplication.HttpRestApi.Handles.Base;

public sealed record NotFoundResponse : BaseResponse
{
    public override int StatusCode => 404;
    public override IResult AsResult() => Results.NotFound();
    public override string ToLoggableString() => "Not found";
}