namespace WebApiApplication.HttpRestApi.Handles.Base;

public abstract record OkResponse : BaseResponse
{
    public override int StatusCode => 200;
    public override IResult AsResult() => Results.Ok(this);
}