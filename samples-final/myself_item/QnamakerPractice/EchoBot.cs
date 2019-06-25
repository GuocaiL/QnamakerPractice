// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using GetAnswer_Controller;
using Update_Controller;
using Model_MessageType;
using Model_UpdateMessageType;
using System;
using MyController_ControllerAdministrator;
using MyController_middleware_AdministratorSetings;
using Mycontroller_Telegram_SendHttp;
using Mymodel_Telegram_sendphotoresponse;
using Newtonsoft.Json;
using Mymodel_Telegram_photodownload;
using Mycontroller_Telegram_JudgeQr;

namespace AspNetCore_EchoBot_With_State
{   
    public class EchoBot : IBot
    {
        string s = null;
        string s2 = null;
        string str0 = "No good match found in KB.";
        ControllerAdministratorSetings abc = new ControllerAdministratorSetings();
        judgeqr b = new judgeqr();

        public async Task OnTurn(ITurnContext context)
        {  

            try
            {
                ControllerAdministrator controllerAdministrator = new ControllerAdministrator(context,
                  abc);
    
                switch (context.Activity.Type)
                {
                    // On "conversationUpdate"-type activities this bot will 
                    //send a greeting message to 
                    //users joining the conversation.
                    case ActivityTypes.ConversationUpdate:

                        if (null != context.Activity.MembersAdded)
                        {
                            
                            foreach (ChannelAccount a in context.Activity.MembersAdded)
                            {
                                if (controllerAdministrator.administrator.count++< 10)
                                {
                                    controllerAdministrator.administrator.storagecount
                                        [controllerAdministrator.administrator.count] = a.Name;
                                }
                                else
                                {
                                    foreach (string ab in controllerAdministrator.administrator.
                                        storagecount)
                                    {
                                        controllerAdministrator.administrator.message += ab;
                                    }
                                    // AddCount.show15minites();
                                    await context.SendActivity("welcome:" +
                                        controllerAdministrator.administrator.message);
                                    //new Welcome_View.WelcomBot();
                                    controllerAdministrator.administrator.count = 0;
                                    controllerAdministrator.administrator.message = null;
                                    Array.Clear(controllerAdministrator.administrator.storagecount, 0,
                                        controllerAdministrator.administrator.storagecount.Length);
                                    // AddCount.setnull();
                                }
                            }
                            
                            controllerAdministrator.totaladministrator[context.Activity.
                                Conversation.Id] = controllerAdministrator.administrator;

                            MyController_timer_GreetingTimer.Program.Addwelcome15Job(context.Activity.
                                Conversation.Id).GetAwaiter().GetResult();
                        }
                        break;

                    case ActivityTypes.Message:

                        if (context.Activity.Text == null)
                        {
                            SendHttp a = new SendHttp();
                            // string response="AgADBQAEqDEbptgBVN0ZE1AM9YGXsVHVMgAEORQyLqTyGdCaEQMAAQI";
                            
                            sendphotoresponse ser = JsonConvert.DeserializeObject<sendphotoresponse>(context.Activity.ChannelData.ToString());
                            var response = ser.message.photo[ser.message.photo.Length - 1].file_id;
                            photodownload download = JsonConvert.DeserializeObject<photodownload>(a.
                                getFile(response).ToString());
                            await context.SendActivity($"{a.getFile(response).ToString()}");
                            try
                            {
                                // var b = new judgeqr();
                                await context.SendActivity($"{download.result.file_path}");
                                if (b.CodeDecoder(download.result.file_path).ToString() != null)
                                {
                                    if (await a.kickChatMember(ser.message.chat.id, ser.message.from.id)=="true")
                                    {
                                        await context.SendActivity($"{ser.message.from.id}已被移出群聊");
                                        await context.SendActivity(@"{""chat_id"":" + "" + ser.message.chat.id + "," + @"""user_id"":" + "" + ser.message.from.id + "}");

                                    }
                                    else
                                    {
                                        await context.SendActivity($"{ser.message.from.id}没有被移出群聊");
                                        await context.SendActivity($"{await a.kickChatMember(ser.message.chat.id, ser.message.from.id)}");
                                    }
                                }
                            } catch (Exception e) { await context.SendActivity(e.ToString()); }
                            
                            
                        }
                        else
                        {
                            
                                if (context.Activity.Text.Length >= context.Activity.Recipient.Id.Length + 2)
                                {
                                    //representative bot name
                                    s = context.Activity.Text.Substring(0, context.Activity.Recipient.Id.Length + 1);
                                    //representative a massage that not include bot name
                                    s2 = context.Activity.Text.Remove(0, context.Activity.Recipient.Id.Length + 2);
                                }

                                string str1 = AnswerMessageType.getQue(s2);
                                string str2 = await GetAnswers.GetAnswerAsync(str1);
                                //await context.SendActivity($"@{context.Activity.Recipient.Id}::{s}:::");
                                //setAdminister if system has not administer
                                if (s == $"@{context.Activity.Recipient.Id}")
                                {
                                    if (controllerAdministrator.setadminandresponseyes())
                                    {
                                        await context.SendActivity($"Oh my god,you are my adminstor:" +
                                            $"{controllerAdministrator.administrator.name },I am very happy<''>,you can input quit" +
                                            $" to quit it,and you can add the knowledge base by the format:" +
                                            $" xxxxxx::xxxxxx and update greeting by the format: " +
                                            $"xx:xx(发送时间)||xxxxxxxx(问候语)||xxxx(多久一次，以秒计)");
                                        controllerAdministrator.administrator.judgeresponse = true;
                                        controllerAdministrator.totaladministrator[context.Activity.
                                            Conversation.Id] = controllerAdministrator.administrator;
                                    }
                                    else if (controllerAdministrator.quitadmin(context))
                                    {
                                        MyController_timer_GreetingTimer.Program.Deletewelcome15Job(context.Activity.
                                    Conversation.Id).GetAwaiter().GetResult();
                                        MyController_timer_GreetingTimer.Program.DeleteGreetingJob(context.
                                            Activity.Conversation.Id).GetAwaiter().GetResult();
                                        await context.SendActivity("quit successed!");
                                    }
                                    // Update message if administer send a fixed message.
                                    else if (UpdateMessageType.TryParseAddQA(s2,
                                        out string kb,
                                        out string new_kb) && (context.Activity.From.Id
                                        == controllerAdministrator.administrator.id))
                                    {
                                        Update.UpdateKB(kb, new_kb);

                                        // QnA didn't send the user an answer                       
                                        await context.SendActivity("add successed," +
                                            "publish successed for a while later!");

                                        //await dialogCtx.Begin("addTwoNumbers", dialogArgs);
                                    }
                                    // Update greeting message if administer send a fixed message.
                                    else if (controllerAdministrator.tryfindupdategreet(context))
                                    {
                                        MyController_timer_GreetingTimer.Program.DeleteGreetingJob(context.
                                            Activity.Conversation.Id).GetAwaiter().GetResult();
                                        MyController_timer_GreetingTimer.Program.AddGreetingJob(context.
                                            Activity.Conversation.Id).GetAwaiter().GetResult();
                                        await context.SendActivity("The greeting " +
                                            "massage has changed!");
                                        await context.SendActivity($"转换后的时间：" +
                                            $"{controllerAdministrator.administrator.settime.ToString()}");
                                    }
                                    else if (str0 != str2)
                                    {
                                        //return anwser
                                        await context.SendActivity($"{str2}");
                                    }
                                    else
                                    {
                                        await context.SendActivity($"Sorry, " +
                                            $"I couldn't find a good match in " +
                                            $"the KB.Please Ask the " +
                                            $"{controllerAdministrator.administrator.name} " +
                                            $"to add the knowledge " +
                                            $"base by the format xxxxxx::xxxxxx.");
                                    }
                                }
                            
                                                       
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                await context.SendActivity(e.ToString());
            }
               
        }

    }

}
