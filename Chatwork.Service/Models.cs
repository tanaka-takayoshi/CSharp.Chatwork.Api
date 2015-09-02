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
        [JsonProperty("account_id")]
        public int AccountId { get; set; }
        [JsonProperty("room_id")]
        public int RoomId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("chatwork_id")]
        public string ChatworkId { get; set; }
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }
        [JsonProperty("organization_name")]
        public string OrganizationName { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("introduction")]
        public string Introduction { get; set; }
        [JsonProperty("mail")]
        public string Mail { get; set; }
        [JsonProperty("tel_organization")]
        public string TelOrganization { get; set; }
        [JsonProperty("tel_extension")]
        public string TelExtension { get; set; }
        [JsonProperty("tel_mobile")]
        public string TelMobile { get; set; }
        [JsonProperty("skype")]
        public string Skype { get; set; }
        [JsonProperty("facebook")]
        public string Facebook { get; set; }
        [JsonProperty("twitter")]
        public string Twitter { get; set; }
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }
    }


    public class MyStatusModel
    {
        [JsonProperty("unread_room_num")]
        public int UnreadRoomNum { get; set; }
        [JsonProperty("mention_room_num")]
        public int MentionRoomNum { get; set; }
        [JsonProperty("mytask_room_num")]
        public int MytaskRoomNum { get; set; }
        [JsonProperty("unread_num")]
        public int UnreadNum { get; set; }
        [JsonProperty("mention_num")]
        public int MentionNum { get; set; }
        [JsonProperty("mytask_num")]
        public int MytaskNum { get; set; }
    }


    public class MyTaskModel
    {
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        [JsonProperty("room")]
        public RoomSummaryModel Room { get; set; }
        [JsonProperty("assigned_by_account")]
        public AccountModel AssignedByAccount { get; set; }
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty("limit_time")]
        public DateTime? LimitTime { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class RoomSummaryModel
    {
        [JsonProperty("room_id")]
        public int RoomId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("icon_path")]
        public string IconPath { get; set; }
    }

    public class ContactModel
    {
        [JsonProperty("account_id")]
        public int AccountId { get; set; }
        [JsonProperty("room_id")]
        public int RoomId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("chatwork_id")]
        public string ChatworkId { get; set; }
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }
        [JsonProperty("organization_name")]
        public string OrganizationName { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }
    }

    public class CreatedRoomModel
    {
        [JsonProperty("room_id")]
        public int RoomId { get; set; }
    }


    public class RoomModel
    {
        [JsonProperty("room_id")]
        public int RoomId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("sticky")]
        public bool Sticky { get; set; }
        [JsonProperty("unread_num")]
        public int UnreadNum { get; set; }
        [JsonProperty("mention_num")]
        public int MentionNum { get; set; }
        [JsonProperty("mytask_num")]
        public int MytaskNum { get; set; }
        [JsonProperty("message_num")]
        public int MessageNum { get; set; }
        [JsonProperty("file_num")]
        public int FileNum { get; set; }
        [JsonProperty("task_num")]
        public int TaskNum { get; set; }
        [JsonProperty("icon_path")]
        public string IconPath { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty("last_update_time")]
        public DateTime? LastUpdateTime { get; set; }
    }

    public class UpdatedRoomMembersModel
    {
        [JsonProperty("admin")]
        public int[] Admin { get; set; }
        [JsonProperty("member")]
        public int[] Member { get; set; }
        [JsonProperty("_readonly")]
        public int[] Readonly { get; set; }
    }

    public class CreatedMessageModel
    {
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
    }

    public class MessageModel
    {
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        [JsonProperty("account")]
        public AccountModel Account { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty("send_time")]
        public DateTime SendTime { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty("update_time")]
        public DateTime? UpdateTime { get; set; }
    }

    public class TaskModel
    {
        [JsonProperty("task_id")]
        public int TaskId { get; set; }
        [JsonProperty("room")]
        public RoomModel Room { get; set; }
        [JsonProperty("account")]
        public AccountModel Account { get; set; }
        [JsonProperty("assigned_by_account")]
        public AccountModel AssignedByAccount { get; set; }
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty("limit_time")]
        public DateTime? LimitTime { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class AccountModel
    {
        [JsonProperty("account_id")]
        public int AccountId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }
    }
    
    public class CreatedTasksModel
    {
        [JsonProperty("task_ids")]
        public int[] TaskIds { get; set; }
    }

    public class FileModel
    {
        [JsonProperty("file_id")]
        public int FileId { get; set; }
        [JsonProperty("account")]
        public AccountModel Account { get; set; }
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        [JsonProperty("filename")]
        public string Filename { get; set; }
        [JsonProperty("filesize")]
        public int Filesize { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty("upload_time")]
        public DateTime UploadTime { get; set; }
        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }
    }

}
