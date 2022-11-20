using Thabe.Bot;
using Thabe.Bot.Plugin.Command;
using Websocket.Client.Logging;

QQBot thabe = new("thabe.club:8080", "dathabe", "1930144367");
await thabe.LaunchAsync();

Log.Write("开始加载插件");
var path = @"C:\Users\datha\source\repos\thabe-library\Thabe.Bot.Plugin\Com\DaThabe\Commands.cs";
LocalCommandScriptPlugin cmd = new(path) { TypeName = "Commands" };
cmd.Register();
Log.Write("插件注册成功");

await MessageManager.SendFriendMessageAsync("2217568525", "Thabe bot launched");
Log.Write("Bot已启动");

while (Console.ReadLine()?.ToLower() == "exit");