using Facebook.Unity;

public interface IFacebookEvent
{
    bool    IsSuccess { get; }
    IResult Result    { get; }
}