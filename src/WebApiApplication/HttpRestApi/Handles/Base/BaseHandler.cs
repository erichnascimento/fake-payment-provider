namespace WebApiApplication.HttpRestApi.Handles.Base;

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
            var badRequestResponse = new BadRequestResponse(ErrorMessage: e.Message);
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