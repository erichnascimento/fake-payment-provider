using System.Text.Json.Serialization;

namespace WebApiApplication.HttpRestApi.Handles;

public abstract class BaseHandler<TRequest, TResponse>
    where TRequest : BaseRequest
    where TResponse : BaseResponse
{
    public async Task<IResult> Handle(TRequest request)
    {
        LogRequest(request);

        try
        {
            request.Validate();
        }
        catch (ArgumentException e)
        {
            var badRequestResponse = new BadRequestResponse(Message: e.Message);
            LogError(request, e, badRequestResponse.StatusCode);

            return badRequestResponse.AsResult();
        }
        // TODO: Handle others exception

        try
        {
            var response = await DoHandle(request);
            LogResponse(request, response);

            return response.AsResult();
        }
        catch (Exception e)
        {
            var internalServerErrorResponse = new InternalServerErrorResponse();
            LogError(request, e, internalServerErrorResponse.StatusCode);

            return internalServerErrorResponse.AsResult();
        }
    }
    
    protected abstract Task<TResponse> DoHandle(TRequest request);

    private static void LogResponse(TRequest request, TResponse response)
    {
        Console.WriteLine(
            $"Http Request: status={response.StatusCode} request={{{request.ToLoggableString()}}} response={{{response.ToLoggableString()}}}");
    }

    private static void LogError(TRequest request, Exception exception, int statusCode)
    {
        Console.WriteLine(
            $"Http Request: status={statusCode} request={{{request.ToLoggableString()}}} error={{{exception.Message}}}");
    }

    private static void LogRequest(TRequest request)
    {
        Console.WriteLine($"Http Request:            request={{{request.ToLoggableString()}}}");
    }
}

public abstract record BaseRequest
{
    public abstract void Validate();
    public abstract string ToLoggableString();
}

public abstract record BaseResponse
{
    public abstract IResult AsResult();
    public abstract string ToLoggableString();

    [JsonIgnore] public abstract int StatusCode { get; }
}

public abstract record CreatedResponse : BaseResponse
{
    protected abstract string Uri { get; }

    [JsonIgnore] public override int StatusCode => 201;
    public override IResult AsResult() => Results.Created(Uri, this);
}

public sealed record NotFoundResponse : BaseResponse
{
    public override int StatusCode => 404;
    public override IResult AsResult() => Results.NotFound();
    public override string ToLoggableString() => "Not found";
}

public abstract record OkResponse : BaseResponse
{
    public override int StatusCode => 200;
    public override IResult AsResult() => Results.Ok(this);
}

public sealed record BadRequestResponse(string Message) : BaseResponse
{
    public override int StatusCode => 400;
    public override IResult AsResult() => Results.BadRequest(Message);
    public override string ToLoggableString() => Message;
}

public sealed record InternalServerErrorResponse : BaseResponse
{
    public override int StatusCode => 500;
    public override IResult AsResult() => Results.StatusCode(StatusCode);
    public override string ToLoggableString() => "Internal server error";
}