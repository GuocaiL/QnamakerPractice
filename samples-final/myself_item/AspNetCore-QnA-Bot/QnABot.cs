// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using program2_;
using telegramData;

namespace AspNetCore_QnA_Bot
{
    public class QnABot : IBot

    {
        
        //.......
        // Recognizes if the message is a request to add 2 numbers, in the form: number + number, 
        // where number may have optionally have a decimal point.: 1 + 1, 123.99 + 45, 0.4+7. 
        // For the sake of simplicity it doesn't handle negative numbers or numbers like 1,000 that contain a comma.
        // If you need more robust number recognition, try System.Recognizers.Text
        public bool TryParseAddingTwoNumbers(string message, out string  first, out string second)
        {
            var succeeded = false;
            first = null;
            second = null;
            try
            {
                string[] strs = message.Split("-");
                string[] str1 = strs[0].Split("::");
                string[] str2 = strs[1].Split("::");
                if (str2[0] .Equals ("A"))
                {
                    succeeded = true;
                    first = str1[1];
                    second = str2[1];
                }
            }
            catch (Exception e)
            {
            }
            return succeeded;
           
        }
        //.......

        public async Task OnTurn(ITurnContext context)
        {
            /*
            if (context.Activity.Type == ActivityTypes.Message && !context.Responded)
            {
                await context.SendActivity("No QnA Maker answers were found. This example uses a QnA Maker Knowledge Base that focuses on smart light bulbs. To see QnA Maker in action, ask the bot questions like \"Why won't it turn on?\" or say something like \"I need help.\"");
            }
            */
            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {        
                if (admin_ .administor .judge )
                {
                    
                    admin_.administor.id = context.Activity.From.Id ;
                    admin_.administor.judge = false;
                    String ad = JsonConvert.SerializeObject(context.Activity.ChannelData );
                    ChannelDataTelegram obj = JsonConvert.DeserializeObject<ChannelDataTelegram>(ad);
                    admin_.administor.Name = obj.message.chat.first_name + obj.message.chat.last_name;
                    await context.SendActivity($"Oh my god,you are my adminstor:{admin_ .administor .Name },I am very happy<''>,you can input quit to quit it");
                }
                if (admin_ .administor .id ==context .Activity .From .Id &&context .Activity .Text =="quit")
                {
                    admin_.administor.judge = true;
                    await context.SendActivity("Quit successed");

                }
                if (!context.Responded)
                {
                    if (TryParseAddingTwoNumbers(context.Activity.Text, out string first, out string second)&&(context .Activity .From .Id ==admin_ .administor .id ))
                      
                    {
                        /* var dialogArgs = new System.Collections.Generic.Dictionary<string, object>
                         {
                             ["first"] = first,
                             ["second"] = second
                         };
                         */
                        
                        string kb = "1aac5ed8-8685-48ef-9171-122ad5516bdd";
                        string new_kb = @"
{
  'add': {
    'qnaList': [
      {
        'id': 1,
        'answer': '"+second+@"',
        'source': 'Custom Editorial',
        'questions': [
          '"+first+@"'
        ],
        'metadata': []
      }
    ],
    'urls': [
      'https://docs.microsoft.com/en-us/azure/cognitive-services/Emotion/FAQ'
    ]
  },
  'update' : {
    'name' : 'New KB Name'
  },
  'delete': {
    'ids': [
      0
    ]
  }
}
";
                        Progrm2.UpdateKB(kb, new_kb);
                        // QnA didn't send the user an answer                       
                        await context.SendActivity("add successed,publish successed for a while later!");                                          
                        //await dialogCtx.Begin("addTwoNumbers", dialogArgs);
                    }
                    else
                    {
                        
                        await context.SendActivity($"Sorry, I couldn't find a good match in the KB.Please Ask the { admin_ .administor .Name } to add the knowledge base by the format Q::xxxxxx-A::xxxxxx.");
                        // Echo back to the user whatever they typed.
                        //await context.SendActivity($"Turn: {state.TurnCount}. You said '{context.Activity.Text}'");
                    }
                    

                }
            }
        }
    }    
}
