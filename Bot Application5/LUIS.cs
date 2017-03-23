using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net.Http;
using Microsoft.Bot.Connector;
using System.Runtime.Serialization;

namespace WellSample
{


    [LuisModel("LUIS id", "LUIS key")]
    [Serializable]
    public class SimpleLUISDialog : LuisDialog<object>
    {
        //public SimpleLUISDialog(Activity activity)
        //{
        //    this.activity = activity;
        //}
        //public const string Entity_location = "Location";
        //private Activity activity;

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"您好，我还年轻，目前只能提供中国地区天气查询功能";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("专业定制肩部")]
        public async Task ProShoulder(IDialogContext context, LuisResult result)
        {
            string message = $"您好，您需要定制肩部方案吗";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("回访问卷")]
        public async Task FeedbackQuestionaire(IDialogContext context, LuisResult result)
        {
            var activity = context.Activity as Activity;
            // //calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            // // return our reply to the user
            // await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            // context.Wait(MessageReceivedAsync);

            // return;

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            Activity replyToConversation = activity.CreateReply();
            replyToConversation.Recipient = activity.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();

            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://www.well-health.cn/file/image/recommend/20161216181155-9904.jpg"));

            List<CardAction> cardButtons = new List<CardAction>();

            CardAction plButton = new CardAction()
            {
                Value = "https://www.sojump.hk/jq/10278805.aspx",
                Type = "openUrl",
                Title = "开始问卷"
            };
            cardButtons.Add(plButton);

            HeroCard plCard = new HeroCard()
            {
                Title = "欢迎您反馈您的意见",
                Subtitle = "WELL健康用户反馈问卷",
                Images = cardImages,
                Buttons = cardButtons
            };

            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);

            var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);

            context.Wait(MessageReceived);
        }

        [LuisIntent("电话客服")]
        public async Task ChallengeDemonstration(IDialogContext context, LuisResult result)
        {
            var activity = context.Activity as Activity;
            // //calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            // // return our reply to the user
            // await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            // context.Wait(MessageReceivedAsync);

            // return;

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            Activity replyToConversation = activity.CreateReply();
            replyToConversation.Recipient = activity.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();

            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://a2.att.hudong.com/36/48/19300001318000131121484293425.jpg"));

            List<CardAction> cardButtons = new List<CardAction>();

            CardAction plButton = new CardAction()
            {
                Value = "tel:33333333333",
                Type = "call",
                Title = "拨打客服电话"
            };
            cardButtons.Add(plButton);

            ThumbnailCard plCard = new ThumbnailCard()
            {
                Subtitle = "如有需要，我们将为您提供电话客服服务",
                Images = cardImages,
                Buttons = cardButtons
            };

            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);

            var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);

            context.Wait(MessageReceived);
        }

        /*[LuisIntent("查询天气")]
        public async Task QueryWeather(IDialogContext context, LuisResult result)
        {
            string location = string.Empty;
            string replyString = "";

            if (TryToFindLocation(result, out location))
            {
                replyString = GetWeather(location);

                JObject WeatherResult = (JObject)JsonConvert.DeserializeObject(replyString);
                var weatherinfo = new
                {
                    城市 = WeatherResult["weatherinfo"]["city"].ToString(),
                    温度 = WeatherResult["weatherinfo"]["temp"].ToString(),
                    湿度 = WeatherResult["weatherinfo"]["SD"].ToString(),
                    风向 = WeatherResult["weatherinfo"]["WD"].ToString(),
                    风力 = WeatherResult["weatherinfo"]["WS"].ToString()
                };


                await context.PostAsync(weatherinfo.城市 + "的天气情况: 温度" + weatherinfo.温度 + "度;湿度" + weatherinfo.湿度 + ";风力" + weatherinfo.风力 + ";风向" + weatherinfo.风向);
            }
            else
            {

                await context.PostAsync("亲你要查询哪个地方的天气信息呢，快把城市的名字发给我吧");
            }
            context.Wait(MessageReceived);

        }

        private string GetWeather(string location)
        {
            string weathercode = "";
            XmlDocument citycode = new XmlDocument();
            citycode.Load("https://wqbot.blob.core.windows.net/botdemo/CityCode.xml");
            XmlNodeList xnList = citycode.SelectNodes("//province//city//county");
            foreach (XmlElement xnl in xnList)
            {
                if (xnl.GetAttribute("name").ToString() == location)
                    weathercode = xnl.GetAttribute("weatherCode").ToString();
            }
            HttpClient client = new HttpClient();
            string result = client.GetStringAsync("http://www.weather.com.cn/data/sk/" + weathercode + ".html").Result;
            return result;
        }
        private bool TryToFindLocation(LuisResult result, out String location)
        {
            location = "";
            EntityRecommendation title;
            if (result.TryFindEntity("地点", out title))
            {
                location = title.Entity;
            }
            else
            {
                location = "";
            }
            return !location.Equals("");
        }*/
    }
    }