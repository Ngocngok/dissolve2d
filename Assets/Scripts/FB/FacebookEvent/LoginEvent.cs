using Facebook.Unity;

namespace Script.Common.FB.FacebookEvent
{
    public class LoginEvent : IFacebookEvent
    {
        public bool    IsSuccess { get; private set; }
        public IResult Result    { get; private set; }

        public LoginEvent(IResult result, bool isSuccess) {
            IsSuccess = isSuccess;
            Result = result;
        }
    }

    public class LoginFBEvent : IFacebookEvent
    {
        public bool IsSuccess { get; private set; }
        public IResult Result { get; private set; }

        public LoginFBEvent()
        {
            IsSuccess = true;
        }
    }
}