using Facebook.Unity;

namespace Script.Common.FB.FacebookEvent
{
    public class LogoutEvent: IFacebookEvent
    {
        public bool    IsSuccess { get; private set; }
        public IResult Result    { get; private set; }

        public LogoutEvent() {
            IsSuccess = true;
        }
    }
}