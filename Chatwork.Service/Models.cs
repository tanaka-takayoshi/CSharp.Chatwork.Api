using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Chatwork.Service
{

    public class MeModel
    {
        public int account_id { get; set; }
        public int room_id { get; set; }
        public string name { get; set; }
        public string chatwork_id { get; set; }
        public int organization_id { get; set; }
        public string organization_name { get; set; }
        public string department { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string introduction { get; set; }
        public string mail { get; set; }
        public string tel_organization { get; set; }
        public string tel_extension { get; set; }
        public string tel_mobile { get; set; }
        public string skype { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string avatar_image_url { get; set; }
    }


    public class MyStatusModel
    {
        public int unread_room_num { get; set; }
        public int mention_room_num { get; set; }
        public int mytask_room_num { get; set; }
        public int unread_num { get; set; }
        public int mention_num { get; set; }
        public int mytask_num { get; set; }
    }


    public class MyTaskModel
    {
        public int task_id { get; set; }
        public RoomSummaryModel room { get; set; }
        public AccountModel assigned_by_account { get; set; }
        public int message_id { get; set; }
        public string body { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? limit_time { get; set; }
        public string status { get; set; }
    }

    public class RoomSummaryModel
    {
        public int room_id { get; set; }
        public string name { get; set; }
        public string icon_path { get; set; }
    }

    public class ContactModel
    {
        public int account_id { get; set; }
        public int room_id { get; set; }
        public string name { get; set; }
        public string chatwork_id { get; set; }
        public int organization_id { get; set; }
        public string organization_name { get; set; }
        public string department { get; set; }
        public string avatar_image_url { get; set; }
    }

    public class CreatedRoomModel
    {
        public int room_id { get; set; }
    }


    public class RoomModel
    {
        public int room_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string role { get; set; }
        public bool sticky { get; set; }
        public int unread_num { get; set; }
        public int mention_num { get; set; }
        public int mytask_num { get; set; }
        public int message_num { get; set; }
        public int file_num { get; set; }
        public int task_num { get; set; }
        public string icon_path { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? last_update_time { get; set; }
    }

    public class UpdatedRoomMembersModel
    {
        public int[] admin { get; set; }
        public int[] member { get; set; }
        public int[] _readonly { get; set; }
    }

    public class CreatedMessageModel
    {
        public int message_id { get; set; }
    }

    public class MessageModel
    {
        public int message_id { get; set; }
        public AccountModel account { get; set; }
        public string body { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? send_time { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? update_time { get; set; }
    }

    public class TaskModel
    {
        public int task_id { get; set; }
        public RoomModel room { get; set; }
        public AccountModel account { get; set; }
        public AccountModel assigned_by_account { get; set; }
        public int message_id { get; set; }
        public string body { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? limit_time { get; set; }
        public string status { get; set; }
    }

    public class AccountModel
    {
        public int account_id { get; set; }
        public string name { get; set; }
        public string avatar_image_url { get; set; }
    }
    
    public class CreatedTasksModel
    {
        public int[] task_ids { get; set; }
    }

    public class FileModel
    {
        public int file_id { get; set; }
        public AccountModel account { get; set; }
        public int message_id { get; set; }
        public string filename { get; set; }
        public int filesize { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? upload_time { get; set; }
        public string download_url { get; set; }
    }

}
