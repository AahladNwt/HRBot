using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace NewWave_Bot_Sample
{
    
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
                    System.Diagnostics.Debug.WriteLine($"This is the value of username/ID: {activity.From.Id} ");
                    await connector.Conversations.ReplyToActivityAsync(repl);            
                }
                else {
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