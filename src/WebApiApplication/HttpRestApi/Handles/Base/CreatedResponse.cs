using System.Text.Json.Serialization;

namespace WebApiApplication.HttpRestApi.Handles.Base;

public abstract record CreatedResponse : BaseResponse
{
    protected abstract string Uri { get; }

    [JsonIgnore] public override int StatusCode => 201;
    public override IResult AsResult() => Results.Created(Uri, this);
}