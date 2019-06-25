using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Model_BotStateSetings_AdministratorSetings;
using Model_ChannelDataTelegram;
using MyController_middleware_AdministratorSetings;
using Newtonsoft.Json;

namespace MyController_ControllerAdministrator
{
    public class ControllerAdministrator
    {
        public string conversationid;       
        public Dictionary<string, AdministratorSetings> totaladministrator;
        public AdministratorSetings administrator;
        public int hours;
        public int minute;
        public int IntervalInSeconds;        

        public ControllerAdministrator(ITurnContext context,
            ControllerAdministratorSetings dictionarydministrator)
        {
            try
            {
                conversationid = context.Activity.Conversation.Id;
                totaladministrator = ControllerAdministratorSetings.Administrator;

                if (totaladministrator.ContainsKey(conversationid))
                {
                    administrator = totaladministrator[conversationid];
                    administrator.judgeresponse = true;
                    administrator.judgesetadministrator = true;

                }
                else if (context.Activity.From.Name == null)
                {
                    
                    administrator = new AdministratorSetings();
                    ChannelDataTelegram channeldata = JsonConvert.DeserializeObject<ChannelDataTelegram>
                        (context.Activity.ChannelData.ToString());
                    administrator.name = channeldata.message.from.first_name +
                        channeldata.message.from.last_name;
                    administrator.id = channeldata.message.from.id.ToString();
                    administrator.context = context;
                    administrator.judgesetadministrator = true;
                    administrator.judgeresponse = false;
                }
                else
                {
                    administrator = new AdministratorSetings();
                    administrator.name = context.Activity.From.Name;
                    administrator.id = context.Activity.From.Id;
                    administrator.context = context;
                    administrator.judgesetadministrator = true;
                    administrator.judgeresponse = false;
                }

                //string admin = JsonConvert.SerializeObject(administrator);
                totaladministrator.Add(conversationid, administrator);
            } catch (Exception e) { }
            
        }

        public bool setadminandresponseyes()
        {
            try
            {
                if (administrator.judgesetadministrator && !administrator.judgeresponse)
                {
                    administrator.judgeresponse = true;
                    //string admin = JsonConvert.SerializeObject(administrator);
                    totaladministrator[conversationid]= administrator;
                    return true;
                }                                    
            } catch (Exception e) { }
            return false;

        }

        public bool quitadmin(ITurnContext context)
        {
            try
            {
                if (administrator.id == context.Activity.From.Id &&
               context.Activity.Text == @"@test299_bot quit")
                {
                    totaladministrator.Remove(context.Activity.Conversation.Id);
                    return true;
                }                
                   
            } catch (Exception e) { }
            return false;

        }

        public bool tryfindupdategreet(ITurnContext context)
        {
            try
            {
                if (context.Activity.From.Id == administrator.id)
                {
                    if (context.Activity.Text.Contains("||"))
                    {
                        string[] strs = context.Activity.Text.Split("||");
                        if (strs[0].Length >= 13 && strs[0].Contains(":") && strs.Length == 3)
                        {
                            if (int.TryParse(strs[0].Remove(0, 13).Split(":")[0], out hours)
                             && int.TryParse(strs[0].Remove(0, 13).Split(":")[1], out minute))
                            {
                                administrator.hours = this.hours;
                                administrator.minute = this.minute;
                                if (int.TryParse(strs[2], out IntervalInSeconds))
                                {
                                    administrator.IntervalInSeconds = IntervalInSeconds;
                                    administrator.greeting = strs[1].Split("||")[0];
                                    // string admin = JsonConvert.SerializeObject(administrator);
                                    
                                    // totaladministrator.Add(conversationid, admin);
                                    DateTime a = DateTime.UtcNow;
                                    administrator.settime= new DateTimeOffset(a.Year, a.Month, a.Day, administrator.hours,
                                        administrator.minute, 0, new TimeSpan(+8, 0, 0));
                                    totaladministrator[conversationid] = administrator;
                                    return true;
                                }
                            }
                        }
                    }
                }
            } catch (Exception e) { }
            
            return false;     
        }
    }
}
