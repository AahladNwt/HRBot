using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace NewWave_Bot_Sample
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            if (activity.Type == ActivityTypes.Message)
            {   
                if (activity.Text == "help" || activity.Text == "Help")
                {
                    Activity repl = activity.CreateReply("In this HR Bot, you can ask frequently asked questions related to the HR Deparment." +
                        "\n\n" +                        
                        "Example: How many PTO does NewWave offer? " +
                        "\n\n" +                        
                        "Who do I contact for payroll queries?" +
                        "\n\n" +                        
                        "Who do I contact if I lose my desk key?");
                    //System.Diagnostics.Debug.WriteLine($"This is the value of username/ID: {activity.From.Id} ");
                    await connector.Conversations.ReplyToActivityAsync(repl);            
                }
                else if (activity.Text.ToLower() == "jamis login")
                {                 

                    Activity replyToConversation = activity.CreateReply();
                    replyToConversation.Recipient = activity.From;
                    replyToConversation.Type = "message";

                    replyToConversation.Attachments = new List<Attachment>();
                    List<CardAction> cardButtons = new List<CardAction>();
                    CardAction plButton = new CardAction()
                    {   //https://login.microsoftonline.com
                        //Value = $"{System.Configuration.ConfigurationManager.AppSettings["AppWebSite"]}/Home/Login?userid={HttpUtility.UrlEncode(activity.From.Id)}",
                        Value = $"{System.Configuration.ConfigurationManager.AppSettings["AppWebSite"]}",
                        Type = "signin",
                        Title = "JAMIS Login"
                    };
                    cardButtons.Add(plButton);
                    SigninCard plCard = new SigninCard("Please login to JAMIS", new List<CardAction>() { plButton });
                    Attachment plAttachment = plCard.ToAttachment();
                    replyToConversation.Attachments.Add(plAttachment);

                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
                }
                else if (activity.Text == "get mail")
                {
                    // Get access token from bot state
                    //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    StateClient stateClient = activity.GetStateClient();
                    BotState botState = new BotState(stateClient);
                    BotData botData = await botState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                    string token = botData.GetProperty<string>("AccessToken");

                    // Get recent 10 e-mail from Office 365
                    HttpClient cl = new HttpClient();
                    var acceptHeader =
                        new MediaTypeWithQualityHeaderValue("application/json");
                    cl.DefaultRequestHeaders.Accept.Add(acceptHeader);
                    cl.DefaultRequestHeaders.Authorization
                      = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage httpRes =
                      await cl.GetAsync("https://outlook.office365.com/api/v1.0/me/messages?$orderby=DateTimeSent%20desc&$top=10&$select=Subject,From");
                    if (httpRes.IsSuccessStatusCode)
                    {
                        var strRes = httpRes.Content.ReadAsStringAsync().Result;
                        JObject jRes = await httpRes.Content.ReadAsAsync<JObject>();
                        JArray jValue = (JArray)jRes["value"];
                        foreach (JObject jItem in jValue)
                        {
                            Activity reply = activity.CreateReply($"Subject={((JValue)jItem["Subject"]).Value}");
                            await connector.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                    else
                    {
                        Activity reply = activity.CreateReply("Failed to get e-mail.\n\nPlease type \"login\" before you get e-mail.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }                
                else
                {
                    await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {

            }

            return null;
        }
    }
}