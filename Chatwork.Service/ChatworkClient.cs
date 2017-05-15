using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Chatwork.Service
{
    public class ChatWorkAuthenticationHandler : DelegatingHandler
    {
        readonly string apiToken;

        public ChatWorkAuthenticationHandler(string apiToken)
            : this(apiToken, new System.Net.Http.HttpClientHandler())
        { }

        public ChatWorkAuthenticationHandler(string apiToken, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            this.apiToken = apiToken;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Add("X-ChatWorkToken", apiToken);
            return base.SendAsync(request, cancellationToken);
        }
    }

    public partial class ChatworkClient : IMe, IMy, IContact, IRoom
    {
        private static readonly string BaseUri = "https://api.chatwork.com/v2/";
        private static readonly int ContentBodyKeyValueLimit = 30000;
        
        readonly HttpClient httpClient;

        public long MaxResponseContentBufferSize
        {
            get
            {
                return httpClient.MaxResponseContentBufferSize;
            }
            set
            {
                httpClient.MaxResponseContentBufferSize = value;
            }
        }
 
        public TimeSpan Timeout
        {
            get
            {
                return httpClient.Timeout;
            }
            set
            {
                httpClient.Timeout = value;
            }
        }

        public int Limit { get; set; }
        public int RemainingLimit { get; set; }
        public DateTime ResetTime { get; set; }

        public IMe Me => this;
        public IMy My => this;
        public IContact Contract => this;
        public IRoom Room => this;

        public ChatworkClient(string apiToken)
        {
            httpClient = new HttpClient(new ChatWorkAuthenticationHandler(apiToken));
        }

        public ChatworkClient(string apiToken, HttpMessageHandler innerHandler)
        {
            httpClient = new HttpClient(new ChatWorkAuthenticationHandler(apiToken, innerHandler));
        }

        private Task<TResult> GetAsync<TResult>(string path, params KeyValuePair<string, object>[] parameters)
        {
            //TODO エスケープ処理
            var requestUri = new Uri(BaseUri + path + "?" + string.Join("&", parameters.Where(p => p.Value != null).Select(p => p.Key + "=" + ConvertToString(p.Value))));
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri
            };
            return SendAsync<TResult>(request);
        }

        private Task<TResult> SendAsync<TResult>(HttpMethod method, string path, params KeyValuePair<string, object>[] parameters)
        {
            var request = new HttpRequestMessage()
            {
                Method = method,
                Content = new FormUrlEncodedContent(parameters.Where(p => p.Value != null).Select(p => new KeyValuePair<string, string>(p.Key, ConvertToString(p.Value)))),
                RequestUri = new Uri(BaseUri + path)
            };
            return SendAsync<TResult>(request);
        }

        private async Task<TResult> SendAsync<TResult>(HttpRequestMessage request)
        {
            var res = await httpClient.SendAsync(request).ConfigureAwait(false);
            UpdateCallLimit(res);
            if (res.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResult>(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            throw new Exception($"Failed with code {res.StatusCode}. Message: {await res.Content.ReadAsStringAsync()}");
        }

        private async Task SendAsync(HttpMethod httpMethod, string path, params KeyValuePair<string, object>[] parameters)
        {
            var request = new HttpRequestMessage()
            {
                Method = httpMethod,
                Content = new FormUrlEncodedContent(parameters.Where(p => p.Value != null).Select(p => new KeyValuePair<string, string>(p.Key, ConvertToString(p.Value)))),
                RequestUri = new Uri(BaseUri + path)
            };
            var res = await httpClient.SendAsync(request).ConfigureAwait(false);
            UpdateCallLimit(res);
            if (res.IsSuccessStatusCode)
            {
                return;
            }
            throw new Exception($"Failed with code {res.StatusCode}. Message: {await res.Content.ReadAsStringAsync()}");
        }

        private string ConvertToString(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            var ints = value as IEnumerable<int>;
            if (ints != null)
            {
                return string.Join(",", ints);
            }
            var dt = value as DateTime?;
            if (dt.HasValue)
            {
                return dt.Value.ToUnixTime().ToString();
            }
            var flg = value as bool?;
            if (flg.HasValue)
            {
                return flg.Value ? "1" : "0";
            }
            return value.ToString();
        }

        private void UpdateCallLimit(HttpResponseMessage res)
        {
            Limit = int.Parse(res.Headers.GetValues("X-RateLimit-Limit").Single());
            RemainingLimit = int.Parse(res.Headers.GetValues("X-RateLimit-Remaining").Single());
            ResetTime = DateTimeExtensions.FromUnixTime(long.Parse(res.Headers.GetValues("X-RateLimit-Reset").Single()));
        }
    }

    public interface IMe
    {
        Task<MeModel> GetAsync();
    }

    public partial class ChatworkClient //IMe
    {
        Task<MeModel> IMe.GetAsync()
        {
            return GetAsync<MeModel>("/me");
        }
    }

    public interface IMy
    {
        Task<MyStatusModel> GetStatusAsync();

        Task<IList<MyTaskModel>> GetTasksAsync(int? assigned_by_account_id = null, string status = null);
    }

    public partial class ChatworkClient //IMy
    {
        Task<MyStatusModel> IMy.GetStatusAsync()
        {
            return GetAsync<MyStatusModel>("/my/status");
        }

        async Task<IList<MyTaskModel>> IMy.GetTasksAsync(int? assigned_by_account_id, string status)
        {
            var res = await GetAsync<IList<MyTaskModel>>("/my/tasks"
                , new KeyValuePair<string, object>("assigned_by_account_id", assigned_by_account_id)
                , new KeyValuePair<string, object>("status", status));
            return res ?? Enumerable.Empty<MyTaskModel>().ToList();
        }
    }

    public interface IContact
    {
        Task<IList<ContactModel>> GetAsync();
    }

    public partial class ChatworkClient // IContact
    {
        async Task<IList<ContactModel>> IContact.GetAsync()
        {
            var res = await GetAsync<IList<ContactModel>>("/contacts");
            return res ?? Enumerable.Empty<ContactModel>().ToList();
        }
    }

    public interface IRoom
    {
        Task<IList<RoomModel>> GetAsync();
        Task<CreatedRoomModel> CreateAsync(IEnumerable<int> members_admin_ids,
            string name,
            string description = null,
            string icon_preset = null,
            IEnumerable<int> members_member_ids = null,
            IEnumerable<int> members_readonly_ids = null);
        Task<RoomModel> GetRoomAsync(int room_id);
        Task<CreatedRoomModel> UpdateRoomAsync(int room_id,
            string name,
            string description = null,
            string icon_preset = null);
        Task LeaveRoomAsync(int room_id,
            string action_type);
        Task<IList<ContactModel>> GetRoomMembersAsync(int room_id);
        Task<UpdatedRoomMembersModel> UpdateRoomMembersAsync(int room_id,
            IEnumerable<int> members_admin_ids,
            IEnumerable<int> members_member_ids = null,
            IEnumerable<int> members_readonly_ids = null);
        Task<IList<MessageModel>> GetMessagesAsync(int room_id, bool force = false);
        Task<CreatedMessageModel> SendMessgesAsync(int room_id, string body);
        Task<MessageModel> GetMessageAsync(int room_id, long message_id);
        Task<IList<TaskModel>> GetTasksAsync(int room_id, int? account_id = null, int? assigned_by_account_id = null, string status = null);
        Task<CreatedTasksModel> CreateTasksAsync(int room_id,
            string body,
            IEnumerable<int> to_ids,
            DateTime? limit = null);
        Task<TaskModel> GetTaskInfoAsync(int room_id, int task_id);
        Task<IList<FileModel>> GetFilesAsync(int room_id);
        Task<FileModel> GetFilAsync(int room_id, int file_id, bool create_download_url = false);
    }

    public partial class ChatworkClient // IRoom
    {
        async Task<IList<RoomModel>> IRoom.GetAsync()
        {
            var res = await GetAsync<IList<RoomModel>>("/rooms");
            return res ?? Enumerable.Empty<RoomModel>().ToList();
        }

        Task<CreatedRoomModel> IRoom.CreateAsync(IEnumerable<int> members_admin_ids,
            string name,
            string description,
            string icon_preset,
            IEnumerable<int> members_member_ids,
            IEnumerable<int> members_readonly_ids)
        {
            return SendAsync<CreatedRoomModel>(
                HttpMethod.Post,
                "/rooms",
                new KeyValuePair<string, object>("members_admin_ids", members_admin_ids),
                new KeyValuePair<string, object>("name", name),
                new KeyValuePair<string, object>("icon_preset", icon_preset),
                new KeyValuePair<string, object>("members_member_ids", members_member_ids),
                new KeyValuePair<string, object>("members_readonly_ids", members_readonly_ids));
        }

        Task<RoomModel> IRoom.GetRoomAsync(int room_id)
        {
            return GetAsync<RoomModel>("/rooms/" + room_id);
        }

        Task<CreatedRoomModel> IRoom.UpdateRoomAsync(int room_id,
            string name,
            string description,
            string icon_preset)
        {
            return SendAsync<CreatedRoomModel>(
                HttpMethod.Put,
                "/rooms/" + room_id,
                new KeyValuePair<string, object>("description", description),
                new KeyValuePair<string, object>("name", name),
                new KeyValuePair<string, object>("icon_preset", icon_preset));
        }

        Task IRoom.LeaveRoomAsync(int room_id,
            string action_type)
        {
            return SendAsync(
                HttpMethod.Delete,
                "/rooms/" + room_id,
                new KeyValuePair<string, object>("action_type", action_type));
        }

        async Task<IList<ContactModel>> IRoom.GetRoomMembersAsync(int room_id)
        {
            var res = await GetAsync<IList<ContactModel>>("/rooms/" + room_id + "/members");
            return res ?? Enumerable.Empty<ContactModel>().ToList();
        }

        Task<UpdatedRoomMembersModel> IRoom.UpdateRoomMembersAsync(int room_id,
            IEnumerable<int> members_admin_ids,
            IEnumerable<int> members_member_ids,
            IEnumerable<int> members_readonly_ids)
        {
            return SendAsync<UpdatedRoomMembersModel>(HttpMethod.Put,
                "/rooms/" + room_id + "/members",
                new KeyValuePair<string, object>("members_admin_ids", members_admin_ids),
                new KeyValuePair<string, object>("members_member_ids", members_member_ids),
                new KeyValuePair<string, object>("members_readonly_ids", members_readonly_ids));
        }

        async Task<IList<MessageModel>> IRoom.GetMessagesAsync(int room_id, bool force)
        {
            var res = await GetAsync<IList<MessageModel>>("/rooms/" + room_id + "/messages",
                new KeyValuePair<string, object>("force", force));
            return res ?? Enumerable.Empty<MessageModel>().ToList();
        }

        Task<CreatedMessageModel> IRoom.SendMessgesAsync(int room_id, string body)
        {
            if (body.Length > ContentBodyKeyValueLimit)
            {
                body = body.Substring(0, ContentBodyKeyValueLimit) + "\r\n [メッセージが長すぎるため省略されました]";
            }
            return SendAsync<CreatedMessageModel>(HttpMethod.Post,
                "/rooms/" + room_id + "/messages",
                new KeyValuePair<string, object>("body", body));
        }

        Task<MessageModel> IRoom.GetMessageAsync(int room_id, long message_id)
        {
            return GetAsync<MessageModel>("/rooms/" + room_id + "/messages/" + message_id);
        }

        async Task<IList<TaskModel>> IRoom.GetTasksAsync(int room_id, int? account_id, int? assigned_by_account_id, string status)
        {
            var res = await GetAsync<IList<TaskModel>>("/rooms/" + room_id + "/tasks"
                , new KeyValuePair<string, object>("account_id", account_id)
                , new KeyValuePair<string, object>("assigned_by_account_id", assigned_by_account_id)
                , new KeyValuePair<string, object>("status", status));
            return res ?? Enumerable.Empty<TaskModel>().ToList();
        }

        Task<CreatedTasksModel> IRoom.CreateTasksAsync(int room_id,
            string body,
            IEnumerable<int> to_ids,
            DateTime? limit)
        {
            return SendAsync<CreatedTasksModel>(HttpMethod.Post
                , "/rooms/" + room_id + "/tasks"
                , new KeyValuePair<string, object>("body", body)
                , new KeyValuePair<string, object>("to_ids", to_ids)
                , new KeyValuePair<string, object>("limit", limit));
        }

        Task<TaskModel> IRoom.GetTaskInfoAsync(int room_id, int task_id)
        {
            return GetAsync<TaskModel>("/rooms/" + room_id + "/tasks/" + task_id);
        }

        async Task<IList<FileModel>> IRoom.GetFilesAsync(int room_id)
        {
            var res = await GetAsync<IList<FileModel>>("/rooms/" + room_id + "/files");
            return res ?? Enumerable.Empty<FileModel>().ToList();
        }

        Task<FileModel> IRoom.GetFilAsync(int room_id, int file_id, bool create_download_url)
        {
            return GetAsync<FileModel>("/rooms/" + room_id + "/files/" + file_id
                , new KeyValuePair<string, object>("create_download_url", create_download_url));
        }
    }
}
