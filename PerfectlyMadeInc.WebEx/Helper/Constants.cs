using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfectlyMadeInc.WebEx.Helper
{
    public static class Constants
    {
        public static class WebexUrls
        {
            public const string BaseUrl = "https://webexapis.com/";
            /// <summary>
            /// get authorize from webex with clienId, clientSecret, and Scope as known as Roles
            /// </summary>
            public const string Authorize = "v1/authorize";
            public const string GetAccessToken = "v1/access_token";
            public const string GetListMeetingAttendee = "v1/meeting-invitees/list-meeting-invitees";
            public const string RegisterMeetingAttendee = "v1/meetings/register-a-meeting-registrant";
            public const string DelMeetingAttendee = "v1/meeting-invitees/delete-a-meeting-invitee";
            public const string ListMeeting = "v1/meetings";
            public const string GetEvents = "/v1/events";
            public const string Access_Token = "YjYwZTE1MjYtNzBiNy00OTVhLThlZGEtZjE0ZGZhMDc1YzA4YTMxMGU2YzAtOGZl_P0A1_0e0a02ae-ec0e-44fc-ae9f-899c501de15c";
        }

        public static class WebexConstants
        {
            public const string AccessToken = "access_token"; 
        }
    }
}
