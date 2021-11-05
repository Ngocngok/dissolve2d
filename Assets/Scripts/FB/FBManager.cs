using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Facebook.Unity;
using UnityEngine;
using UniRx;
using Script.Common.FB.FacebookEvent;
using SimpleJSON;

public class FBManager
{
    public Action SendInviteSuccessAction;
    public Action SendInviteCancelledAction;
    public Action<string> SendInviteFailAction;
    public Action<List<JSONNode>> AppRequestSuccessAction;
    public Action<string> AppRequestFailAction;
    public Action<string> GetAvatarLinkSuccessAction;
    public Action<string> GetAvatarLinkFailAction;
    public Action<List<JSONNode>> GetAllFriendsSuccessAction;
    public Action<string> GetAllFriendsFailAction;
    public Action<List<JSONNode>> GetInvitableFriendsSuccessAction;
    public Action<string> GetInvitableFriendsFailAction;
    public Action<List<JSONNode>> CheckUserLike;

    public Action<IShareResult> GetShareResult;
    public Action<int> GetCurrentNumberFriend;
    private event Action OnInitSuccess;
    private readonly FacebookWrapper _facebookWrapper = new FacebookWrapper();

    public IObservable<Unit> InitObservable()
    {
        return Observable.FromEvent(action => OnInitSuccess += action, action => OnInitSuccess -= action);
    }

    public IObservable<LoginEvent> LoginObservable()
    {
        return _facebookWrapper.EventStream<LoginEvent>();
    }

    public IObservable<Unit> LoginFBObservable()
    {
        return _facebookWrapper.EventStream<LoginFBEvent>().AsUnitObservable();
    }
    public void OnLoginFBObservable()
    {
        _facebookWrapper.OnEventStream(new LoginFBEvent());
    }

    public IObservable<Unit> LogoutObservable()
    {
        return _facebookWrapper.EventStream<LogoutEvent>()
            .AsUnitObservable();
    }

    private string _nextInvitableFriends
    {
        get { return PlayerPrefs.GetString("FBNextInvitableFriends", string.Empty); }
        set { PlayerPrefs.SetString("FBNextInvitableFriends", value); }
    }

    private readonly List<JSONNode> _listNode = new List<JSONNode>();

    public bool IsInit
    {
        get { return FB.IsInitialized; }
    }

    public bool IsLogin
    {
        get { return FB.IsInitialized && FB.IsLoggedIn; }
    }

    public string UserId
    {
        get { return IsLogin ? AccessToken.CurrentAccessToken.UserId : null; }
    }

    public void FirstInit()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() =>
            {
                FB.ActivateApp();
                //FirstLogin();
                if (OnInitSuccess != null)
                    OnInitSuccess();
            });
        }

        //		StartCoroutine (ReadFBInvite ());
    }

    public void FirstLogin()
    {
        if (!FB.IsLoggedIn)
            FB.LogInWithReadPermissions(new List<string>() {"public_profile", "email", "gaming_user_picture" }, HandleLogin);
        //else {
        //    if (LoginResultSuccessAction != null)
        //        LoginResultSuccessAction();
        //}
    }

    public IObservable<LoginEvent> Login()
    {
        if (!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(new List<string>() {"public_profile", "email" , "gaming_user_picture" }, HandleLogin);
            return LoginObservable();
        }

        return Observable.Empty<LoginEvent>();
    }

    public void Logout()
    {
        FB.LogOut();
        _facebookWrapper.OnEventStream(new LogoutEvent());
    }

    public void SendInvite(string dataToSend)
    {
        FB.AppRequest("Come to play with me.", null, null, null, 50, dataToSend, "", HandleSendInvite);
    }

    public void SendInvite(string dataToSend, IEnumerable<string> to)
    {
        FB.AppRequest("Come to play with me.", to, null, null, 50, dataToSend, "", HandleSendInvite);
    }

    public void Send50InviteWithHandle(string dataToSend, IEnumerable<string> to)
    {
        FB.AppRequest("Come to play with me.", to, null, null, 50, dataToSend, "", HandleSendInvite);
    }

    public void CheckAppLink()
    {
        FB.GetAppLink((appLink) =>
        {
            if (string.IsNullOrEmpty(appLink.Error) && !string.IsNullOrEmpty(appLink.Url))
            {
                GetRequestInvite(appLink.Url);
            }
            else
            {
                AppRequestFailAction("No app request.");
            }
        });
    }

    public void ShareFacebook()
    {
        FB.Mobile.ShareDialogMode = ShareDialogMode.FEED;
        FB.ShareLink(
            new Uri("https://go.onelink.me/y0dc/b2e792b4"),
            "Galaxy Attack : Space Shooter",
            "Lets have a great adventure.",
            new Uri("https://lh3.googleusercontent.com/M8YkorblDvBctd26w2-0w_46qwy6q18FLJEpr9TJO802SrcN9_wHOubhpTf0fr367vMz=w1920-h944"),
            callback: FeedCallback);
        
//        FB.FeedShare(
//            string.Empty,
//            new Uri("https://go.onelink.me/y0dc/b2e792b4"),
//            "Galaxy Attack : Space Shooter",
//            "Test caption",
//            "Lets have a great adventure.",
//            new Uri("http://i.imgur.com/zkYlB.jpg"),
//            string.Empty,
//            this.FeedCallback);
    }

    private void FeedCallback(IShareResult result)
    {
        GetShareResult(result);
    }

    public void CheckUserLikes()
    {
        FB.API("me/likes/852418654924076", HttpMethod.GET,
            ShareCallback);
    }

    private void ShareCallback(IGraphResult result)
    {
        if (string.IsNullOrEmpty(result.Error))
        {
            _listNode.Clear();
            var data = JSON.Parse(result.RawResult)["data"] as JSONArray;
            if (data != null)
                for (var i = 0; i < data.Count; i++)
                {
                    _listNode.Add(data[i]);
                }

        //    CheckUserLike(_listNode);
        }
        else
        {
            Debug.LogFormat("Apprequest Error:{0}", result.Error);
            if (GetAllFriendsFailAction != null)
                GetAllFriendsFailAction(result.Error);
        }
    }


    public class FBResult
    {
    }

    public void GetAllFriends()
    {
        var haveUserFriendsPerm = CheckPermisssion("user_friends");

        if (haveUserFriendsPerm)
        {
            FB.API("/me/friends?fields=id,name,picture.width(40).height(40)&limit=100", HttpMethod.GET,
                HandleAllFriendsRequest);
        }
        else
        {
            FB.LogInWithReadPermissions(new List<string>() {"public_profile", "email", "user_friends"}, (result) =>
            {
                var havePerm = CheckPermisssion("user_friends");
                if (havePerm)
                    FB.API("/me/friends?fields=id,name,picture.width(40).height(40)&limit=100", HttpMethod.GET,
                        HandleAllFriendsRequest);
                else
                {
                    if (GetAllFriendsFailAction != null)
                        GetAllFriendsFailAction("Dont have permission.");
                }
            });
        }
    }

    public void GetInvitableFriends()
    {
        FB.API(
            string.IsNullOrEmpty(_nextInvitableFriends)
                ? "/me/invitable_friends?fields=id,name,picture.width(40).height(40)&limit=50"
                : string.Format("/me/invitable_friends?fields=id,name,picture.width(40).height(40)&limit=50&after={0}",
                    _nextInvitableFriends),
            HttpMethod.GET,
            HandleFriendInvite);
    }

    public void GetAvatarLink()
    {
        FB.API("/me/?fields=picture.width(100).height(100),name", HttpMethod.GET, HandleAvatarQuery);
    }

    protected bool CheckPermisssion(string permToCheck)
    {
        foreach (var perm in AccessToken.CurrentAccessToken.Permissions)
        {
            if (string.Compare(perm, permToCheck, StringComparison.Ordinal) == 0)
            {
                return true;
            }
        }

        return false;
    }

    protected void GetRequestInvite(string url)
    {
        var requestparams = GetParams(url);
        string[] listId = null;
        foreach (var entry in requestparams)
        {
            if (String.Compare(entry.Key, "request_ids", StringComparison.Ordinal) == 0)
            {
                listId = entry.Value.Split(',');
                break;
            }
        }

        if (listId != null && listId.Length > 0)
        {
            FB.API("/me/apprequests?fields=created_time,data,from", HttpMethod.GET, HandleAppRequest);
        }
    }

    Dictionary<string, string> GetParams(string uri)
    {
        var matches = Regex.Matches(uri, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.None);
        return matches.Cast<Match>().ToDictionary(
            m => Uri.UnescapeDataString(m.Groups[2].Value),
            m => Uri.UnescapeDataString(m.Groups[3].Value)
        );
    }

    #region Handle Callback

    protected void HandleLogin(IResult result)
    {
        if (string.IsNullOrEmpty(result.Error))
        {
            if (!result.Cancelled)
            {
                _facebookWrapper.OnEventStream(new LoginEvent(result, true));

                GetAvatarLink();
                //var mainScreen = (ScreenMain)GUIManager.Instance.GetPanel(UI_PANEL.MainScreen);
                //if (mainScreen != null)
                //    mainScreen.btnFacebook.SetActive(false);
            }
            else
            {
                // Ẩn Loading
                //if (GUIManager.Instance.popupLoading != null)
                //    GUIManager.Instance.popupLoading.Close();
            }
        }
        else
        {
            Debug.LogFormat("Login Error:{0}", result.Error);
            _facebookWrapper.OnEventStream(new LoginEvent(result, false));
        }
    }

    protected void HandleAvatarQuery(IResult result)
    {
        if (string.IsNullOrEmpty(result.Error))
        {
            var data = JSON.Parse(result.RawResult);
            var avatar = data["picture"]["data"]["url"].Value;
            //DataManager.AvatarLink = avatar;
            //GameUtils.RaiseMessage(HomeMessages.DiplayNameUpdateMessage.Instance);
            if (GetAvatarLinkSuccessAction != null)
                GetAvatarLinkSuccessAction(avatar);
        }
        else
        {
            Debug.LogFormat("AvatarQuery  Error:{0}", result.Error);
            if (GetAvatarLinkFailAction != null)
                GetAvatarLinkFailAction(result.Error);
        }
    }

    protected void HandleAppRequest(IResult result)
    {
        if (string.IsNullOrEmpty(result.Error))
        {
            _listNode.Clear();
            var data = JSON.Parse(result.RawResult)["data"] as JSONArray;
            if (data != null)
                for (var i = 0; i < data.Count; i++)
                {
                    if (HanldeNode(data[i]))
                    {
                        _listNode.Add(data[i]);
                        FB.API(data[i]["id"], HttpMethod.DELETE);
                    }
                    else
                    {
                        FB.API(data[i]["id"], HttpMethod.DELETE);
                    }
                }

            if (AppRequestSuccessAction != null)
                AppRequestSuccessAction(_listNode);
        }
        else
        {
            Debug.LogFormat("Apprequest Error:{0}", result.Error);
            if (AppRequestFailAction != null)
                AppRequestFailAction(result.Error);
        }
    }

    protected void HandleSendInvite(IAppRequestResult result)
    {
        if (string.IsNullOrEmpty(result.Error))
        {
            _listNode.Clear();

            if (!result.Cancelled)
            {
                if (SendInviteSuccessAction != null)
                    SendInviteSuccessAction();
            }
            else
            {
                if (SendInviteCancelledAction != null)
                    SendInviteCancelledAction();
            }
        }
        else
        {
            Debug.LogFormat("SendInvite Error:{0}", result.Error);
            if (SendInviteFailAction != null)
                SendInviteFailAction(result.Error);
        }
    }

    protected bool HanldeNode(JSONNode node)
    {
        string data = node["data"];
        return !string.IsNullOrEmpty(data);
    }

    protected void HandleFriendInvite(IResult result)
    {
        if (string.IsNullOrEmpty(result.Error))
        {
            _listNode.Clear();
            var rootNode = JSON.Parse(result.RawResult);
            var data = rootNode["data"] as JSONArray;
            if (data != null)
                for (var i = 0; i < data.Count; i++)
                {
                    _listNode.Add(data[i]);
                }

            _nextInvitableFriends = !string.IsNullOrEmpty(rootNode["paging"]["next"].Value)
                ? rootNode["paging"]["cursors"]["after"].Value
                : string.Empty;

            if (GetInvitableFriendsSuccessAction != null)
                GetInvitableFriendsSuccessAction(_listNode);
        }
        else
        {
            _nextInvitableFriends = string.Empty;
            if (GetInvitableFriendsFailAction != null)
                GetInvitableFriendsFailAction(result.Error);
        }
    }


    protected void HandleAllFriendsRequest(IResult result)
    {
        if (string.IsNullOrEmpty(result.Error))
        {
            _listNode.Clear();
            var data = JSON.Parse(result.RawResult)["data"] as JSONArray;
            if (data != null)
                for (var i = 0; i < data.Count; i++)
                {
                    _listNode.Add(data[i]);
                }

            if (GetAllFriendsSuccessAction != null)
                GetAllFriendsSuccessAction(_listNode);
        }
        else
        {
            Debug.LogFormat("Apprequest Error:{0}", result.Error);
            if (GetAllFriendsFailAction != null)
                GetAllFriendsFailAction(result.Error);
        }
    }

    #endregion
}