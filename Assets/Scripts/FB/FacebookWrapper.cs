using System;
using UniRx;

public class FacebookWrapper
{
    private event Action<IFacebookEvent> eventStream = delegate { };

    public IObservable<T> EventStream<T>() where T : IFacebookEvent {
        return Observable.FromEvent<IFacebookEvent>(action => eventStream += action, action => eventStream -= action)
                         .OfType<IFacebookEvent, T>()
                         .SelectMany(@event => @event.IsSuccess
                             ? Observable.Return(@event)
                             : Observable.Throw<T>(new FacebookException(@event.Result)));
    }

    public void OnEventStream(IFacebookEvent @event) {
        eventStream.Invoke(@event);
    }
}