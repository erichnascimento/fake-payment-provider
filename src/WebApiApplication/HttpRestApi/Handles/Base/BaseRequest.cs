namespace WebApiApplication.HttpRestApi.Handles.Base;

public abstract record BaseRequest
{
    public abstract void Validate();
    public abstract string ToLoggableString();
}