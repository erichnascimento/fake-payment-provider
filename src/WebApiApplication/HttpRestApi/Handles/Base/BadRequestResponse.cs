using System.Text.Json.Serialization;

namespace WebApiApplication.HttpRestApi.Handles.Base;

[JsonSerializable(typeof(BadRequestResponse))]
public sealed record BadRequestResponse(
    string ErrorMessage,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? ErrorCode = null
) : BaseResponse
{
    [JsonIgnore] public override int StatusCode => 400;
    public override IResult AsResult() => Results.BadRequest(this);
    public override string ToLoggableString() => $"ErrorMessage={ErrorMessage}, ErrorCode={ErrorCode}";
}