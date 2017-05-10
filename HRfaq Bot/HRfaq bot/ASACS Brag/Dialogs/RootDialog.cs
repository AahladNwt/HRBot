using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

//Azure SQL Pass: newwaveHRbot*

/*<add key="BotId" value="nwthrbot" />
    <add key="MicrosoftAppId" value="9c552451-7a5a-47fc-9699-0b06848a7898" />
    <add key="MicrosoftAppPassword" value="xnNvGkddteAkdn44KQqdSjT" /> */

namespace NewWave_Bot_Sample.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;            
            string str = activity.Text;                        
            LuisJSON sLuis = await GetEntityFromLUIS(str);         

           
            System.Threading.Thread.Sleep(3000);
            await context.PostAsync($"HR Bot is currently under development by **Innov-Lab**");
            System.Threading.Thread.Sleep(2000);

            /*if(activity != null && activity.Text != null)
            {
                var replyMessage = context.MakeMessage();
                replyMessage.Attachments = new List<Attachment>();

                switch (activity.Text.ToLower())
                {
                    case "hero-card":
                        {
                            ShowHeroCard(replyMessage);
                            break;
                        }
                    default: { break; }
                }
                await context.PostAsync(replyMessage);
            }*/
            /*
            if (sLuis.intents[0].intent == "meals remain")
            {
                string val;
                DbOperationMR db = new DbOperationMR();
                db.db_connection();
                val = db.value;                
                await context.PostAsync($"The total number of Meals Remaining is {val} -- MySQL Connection");
               
            }
            else if (sLuis.intents[0].intent == "Bunk_Room")
            {
                string valrc, valbc;
                DbOperationBR db = new DbOperationBR();
                db.db_connection();
                valrc = db.Valrc;
                valbc = db.Valbc;
                await context.PostAsync($"The total number of rooms available is {valrc} and the number of bunks availabe is {valbc} -- MySQL Connection");

            }
            else if (sLuis.intents[0].intent == "soup_kit")
            {
                string valrc;
                DbOperationSK db = new DbOperationSK();
                db.db_connection();
                valrc = db.Valrc;                
                await context.PostAsync($"The total number of seats available is {valrc} -- MySQL Connection");

            }
            else if (sLuis.intents[0].intent == "donation")
            {
                string valrc;
                DbOperationDonat db = new DbOperationDonat();
                db.db_connectiondonat();
                valrc = db.value;
                await context.PostAsync($"{valrc} -- MySQL Connection");
            }
            else if (sLuis.intents[0].intent == "PTOLeave")
            {
                await context.PostAsync($"You are asking information about Paid Time Off Hours");
            }
            else if (sLuis.intents[0].intent =="None")
            {
                await context.PostAsync($"Sorry, I was not able to understand your query please rephrase");
            }*/

            switch (sLuis.intents[0].intent)
            {
                case "PTOLeave":
                    await context.PostAsync($"You are asking information about Paid Time Off Hours");
                    await context.PostAsync($"• Paid time off is available immediately (But not shown in payroll unitl 2nd pay period)" +
                        "\n\n" +
                        $"• 80 hours carryover max -per year" +
                        "\n\n" +
                        $"• PTO Calendar reflects the your current status");
                    break;
                case "NewHireReq":
                    await context.PostAsync($"You are asking information about the documents/forms required by HR Department");
                    await context.PostAsync($"You need to submit the following forms" +
                        "\n\n" +
                        $"• New Hire Payroll and Benefit Forms" +
                        "\n\n" +
                        $"• Employee Handbook and Acknowledgement Form" +
                        "\n\n" +
                        $"• Acceptable  USe Policy and Acknowwledgement Form" +
                        "\n\n" +
                        $"• Photo Release Form" +
                        "\n\n" +
                        $"• Email Signature Requirement" +
                        "\n\n" +
                        $"• Rule sof Behaviour Agreement Form" +
                        "\n\n" +
                        $"• Security Awarness and Other Training Requirements");
                    break;
                case "NewWaveBenf":
                    await context.PostAsync($"You are asking information on the benefits provided by NewWave");
                    await context.PostAsync($"The following are some of the benefits provided by NewWave" +
                        "\n\n" +
                        $"• Choice of different Health Plans" +
                        "\n\n" +
                        $"• Dentals Plans" +
                        "\n\n" +
                        $"• Vision Plans" +
                        "\n\n" +
                        $"• Group STD/LTD/Life ins plans" +
                        "\n\n" +
                        $"• Great 401k with corporatee matching" +
                        "\n\n" +
                        $"• Trainging Stipend & Tution Reimbursement");
                    break;
                case "ActivCont":
                    await context.PostAsync($"You are asking information about currently active contracts in the Company");
                    await context.PostAsync($"Our Company is currently working with CMS on these contracts:" +
                        "\n\n" +
                        $"1: CCW" +
                        "\n\n" +
                        $"2: EPPE" +
                        "\n\n" +
                        $"3: RMADA (IDIQ)" +
                        "\n\n" +
                        $"4: RDIS (IDIQ)" +
                        "\n\n" +
                        $"5: DM (IDIQ)" +
                        "\n\n" +
                        $"6: QNET Conference" +
                        "\n\n" +
                        $"7: PQPMI" +
                        "\n\n" +
                        $"8: RADV (O&M)" +
                        "\n\n" +
                        $"9: RADV (Infra)" +
                        "\n\n" +
                        $"10: T-MSIS" +
                        "\n\n" +
                        $"11: MACBIS PMO");
                    break;
                case "CoreValues":
                    await context.PostAsync($"You are asking information about core values of the Company");
                    await context.PostAsync($"There are the core values of out company:" +
                        "\n\n" +
                        $"1: **People First:** Our professionals are the backbone of our success; without them we cannot accomplish our mission. We value our employees’ individual strengths, and their entrepreneurial and charitable spirit." +
                        "\n\n" +
                        $"2: **Customer Always:** Our customers share our vision of providing quality healthcare services nationwide. We partner with our customers to help them achieve our shared ideals." +
                        "\n\n" +
                        $"3: **Integrity:** Our team values honesty, respect, and teamwork. We believe integrity is doing the right thing; even when no one is looking." +
                        "\n\n" +
                        $"4: **Quality:** Our partners rely on quality products and solutions for mission success. We provide efficient, measurable, and cost-effective results that exceed customer expectations." +
                        "\n\n" +
                        $"5: **Social Responsibilty:** Our personnel support and provide local community outreach. We embrace the charitable donation of our time, talent, and treasure as the bedrock principle upon which this company was founded."
                        );
                    break;
                case "Payroll_Ques":
                    await context.PostAsync($"All your concerns can be addressed by sending an email to *payroll@newwave.io*");
                    break;
                case "payday":
                    await context.PostAsync($"Our paydays fall on the **7th** and **22nd** of each month. However, if the 7th or 22nd falls on a holiday/eekend we get paid the business day before");
                    break;
                case "pto_incurmonth":
                    await context.PostAsync($"Eligible employees will incur a total of *10 hours* of PTO monthly. You will recieve *5 hours on the 7th* and *5 hours on the 22nd* payroll cycle which equates to *10 hours total* for the month");
                    break;
                case "holidaysch":
                    await context.PostAsync($"You can find the holiday/payroll calendar for 2017 on SharePoint. Click [here](https://newwavetechnologies.sharepoint.com/Lists/Announcements/DispForm.aspx?ID=4)");
                    break;
                case "None":
                    await context.PostAsync($"None"); ;
                    break;
            }

        }

        /*
        private static void ShowHeroCard(IMessageActivity replyMessage)
        {
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                Type = "openUrl",
                Title = "Wikipedia Page"
            };
            cardButtons.Add(plButton);
            HeroCard plCard = new HeroCard()
            {
                Title = "I'm a hero card",
                Subtitle = "Wikipedia Page",
                Images = cardImages,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);
        }*/

        public async Task MessageReceivedFBid(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            var number2 = int.Parse(numbers.Text);
            await context.PostAsync($"The FoodBank Id: {number2}");
            await context.PostAsync($"SQL database query condition");
        }

        public async Task MessageReceivedSHid(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            var number2 = int.Parse(numbers.Text);
            await context.PostAsync($"The Shelter Id: {number2}");
            await context.PostAsync($"SQL database query condition");
        }

        private static async Task<LuisJSON> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            LuisJSON Data = new LuisJSON();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/e6407f0e-5e84-4a27-9d75-09abff0b9b26?subscription-key=470c099f975a40c9b661a4c7e5c78b09&timezoneOffset=-300&verbose=true&spellCheck=true&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<LuisJSON>(JsonDataResponse);
                }
            }
            return Data;
        }
    }

}