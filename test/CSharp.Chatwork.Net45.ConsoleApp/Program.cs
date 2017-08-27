﻿#if net45
using System.Configuration;
#else
using Microsoft.Extensions.Configuration;
using System.IO;
#endif
using System;
using System.Linq;

namespace Chatwork.Service.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
#if net45
            var token = ConfigurationManager.AppSettings["Token"];
#else
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("usersettings.json");
            var configuration = builder.Build();
            var token = configuration["Token"];
#endif
            
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Input auth key:");
                token = Console.ReadLine();
            }
            var client = new ChatworkClient(token);
            var me = client.Me.GetAsync().Result;
            Console.WriteLine("APIの制限回数:{0},残り:{1},リセット:{2}", client.Limit, client.RemainingLimit, client.ResetTime);

            Console.WriteLine("あなたの所属:{0},氏名:{1},ID:{2}", me.organization_name, me.name, me.account_id);

            var rooms = client.Room.GetAsync().Result;
            Console.WriteLine();
            Console.WriteLine("ルーム一覧");
            rooms.OrderBy(r => r.room_id).Select(r => string.Format("{0}:{1}", r.room_id, r.name)).ToList().ForEach(Console.WriteLine);
            Console.WriteLine();

            var mychat = rooms.First(r => r.type == "my");
            Console.WriteLine("マイチャットのIDは{0}です", mychat.room_id);
            Console.WriteLine();

            var users = client.Room.GetRoomMembersAsync(mychat.room_id).Result;
            users.Select(u => u.account_id + " " + u.name)
                .ToList()
                .ForEach(Console.WriteLine);

            var mystatus = client.My.GetStatusAsync().Result;
            Console.WriteLine("未読数:{0},未読TO数:{1},タスク数:{2}", mystatus.unread_num, mystatus.mention_num, mystatus.mytask_num);
            var mytasks = client.My.GetTasksAsync().Result;
            Console.WriteLine();
            Console.WriteLine("タスク一覧");
            mytasks.OrderBy(task => task.task_id).Select(task => string.Format("{0}:{1} 納期:{2}", task.task_id, task.body, task.limit_time)).ToList().ForEach(Console.WriteLine);
            Console.WriteLine();

            var contacs = client.Contract.GetAsync().Result;
            Console.WriteLine("あなたのコンタクトには{0}人登録されいます", contacs.Count);
                   
            var sentMessage = client.Room.SendMessgesAsync(mychat.room_id, "テスト投稿").Result;
            Console.WriteLine("マイチャットにテスト投稿しました");
            var message = client.Room.GetMessageAsync(mychat.room_id, sentMessage.message_id).Result;
            Console.WriteLine("投稿したメッセージ: {0}", message.body);

            var createdTaskId = client.Room.CreateTasksAsync(mychat.room_id, "テストタスク", new int[] { me.account_id },
                DateTime.Today.AddDays(1)).Result;
            var createdTask = client.Room.GetTaskInfoAsync(mychat.room_id, createdTaskId.task_ids.First()).Result;
            Console.WriteLine("作成したタスク: {0}", createdTask.body);

            var files = client.Room.GetFilesAsync(mychat.room_id).Result;
            var file = client.Room.GetFilAsync(mychat.room_id, files.First().file_id).Result;
            Console.WriteLine(file.account.account_id);
            var fileWithUrl = client.Room.GetFilAsync(mychat.room_id, files.First().file_id, true).Result;
            Console.WriteLine("ダウロードURL(30sec有効): " + fileWithUrl.download_url);

            var messages = client.Room.GetMessagesAsync(mychat.room_id).Result;
            Console.WriteLine(string.Join("\r\n", messages.Select(m => m.message_id + " " + m.body)));

            messages = client.Room.GetMessagesAsync(mychat.room_id, true).Result;
            Console.WriteLine(string.Join("\r\n", messages.Select(m => m.message_id + " " + m.body)));
            Console.WriteLine("何かキーを押して終了してください...");
            Console.ReadKey();
        }
    }
}
