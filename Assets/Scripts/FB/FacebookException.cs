using System;
using Facebook.Unity;

public class FacebookException : Exception
{
    public FacebookException(IResult error) : base(error.Error) { }
}