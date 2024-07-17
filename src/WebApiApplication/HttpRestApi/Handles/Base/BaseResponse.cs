using System.Text.Json.Serialization;

namespace WebApiApplication.HttpRestApi.Handles.Base;

public abstract record BaseResponse
{
    public abstract IResult AsResult();
    public abstract string ToLoggableString();

    [JsonIgnore] public abstract int StatusCode { get; }
}