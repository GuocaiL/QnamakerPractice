// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using Mycontroller_GetFileId;
using Mycontroller_JudgeQr;
using Mymodel_SeriObj_photodownload;
using Mymodel_SeriObj_seriobj;
using Newtonsoft.Json;

namespace AspNetCore_EchoBot_With_State
{
    public class EchoBot : IBot
    {
        /// <summary>
        /// Every Conversation turn for our EchoBot will call this method. In here
        /// the bot checks the Activty type to verify it's a message, bumps the 
        /// turn conversation 'Turn' count, and then echoes the users typing
        /// back to them. 
        /// </summary>
        /// <param name="context">Turn scoped context containing all the data needed
        /// for processing this conversation turn. </param>        
        public async Task OnTurn(ITurnContext context)
        {

            try
            {
                if (context.Activity.Type == ActivityTypes.Message)
                {
                    if (context.Activity.Text == null)
                    {
                        GetFileId a = new GetFileId();
                        // string response="AgADBQAEqDEbptgBVN0ZE1AM9YGXsVHVMgAEORQyLqTyGdCaEQMAAQI";
                        seriobj ser = JsonConvert.DeserializeObject<seriobj>(context.Activity.ChannelData.ToString());
                        var response = ser.message.photo[ser.message.photo.Length - 1].file_id;
                        //await context.SendActivity(@"{""file_id"":" + "\"" +"AgADBQAEqDEbptgBVN0ZE1AM9YGXsVHVMgAEORQyLqTyGdCaEQMAAQI"+ "\"}");
                        //await context.SendActivity(a.PostUrl(response).ToString());
                        //await context.SendActivity(response);
                        photodownload download = JsonConvert.DeserializeObject<photodownload>(a.
                            PostUrl(response).ToString());
                        judgeqr b = new judgeqr();
                        if (b.CodeDecoder(download.result.file_path).ToString() != null)
                        {
                            await context.SendActivity(b.CodeDecoder(download.result.file_path).ToString());
                        }
                    }
                    else
                    {
                        // Get the conversation state from the turn context
                        var state = context.GetConversationState<EchoState>();

                        // Bump the turn count. 
                        state.TurnCount++;


                        if (context.Activity.Text == "Hey! Welcome!")
                        {
                            await context.SendActivity(@"@test299_bot I received a message from a robot,I am a bot;");
                        }
                        await context.SendActivity(context.Activity.Text);
                        // Echo back to the user whatever they typed.
                    }

                }
            } catch (Exception e)
            {
                await context.SendActivity(e.ToString());
            }
            
        }
    }
}
