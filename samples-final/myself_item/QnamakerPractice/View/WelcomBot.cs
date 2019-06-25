

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model_BotStateSetings_AdministratorSetings;
using MyController_middleware_AdministratorSetings;

namespace Welcome_View
{
    public class WelcomBot
    {
        private Dictionary<string, AdministratorSetings> copy;
        private AdministratorSetings admin=null;
        //private ITurnContext context;
        private string conversationId;

        public WelcomBot(string conversationId)
        {
            try
            {
                // copy = JsonConvert.DeserializeObject<ITurnContext>(context).GetConversationState<ControllerAdministratorSetings>().Administrator;
               // copy = Program.context.GetConversationState<ControllerAdministratorSetings>().Administrator;
                copy = ControllerAdministratorSetings.Administrator;
                this.conversationId = conversationId;
                if (copy.ContainsKey(conversationId))
                {
                    admin = copy[conversationId];
                }
            } catch (Exception e) { }
            
        }

        public async Task Resume1()
        {
            if (admin!=null)
            {
                await admin.context.SendActivity($"" +
                $"{admin.greeting}");
            }            
        }

        public async Task Resume2()
        {
            if (admin!= null)
            {
                if (admin.count<10)
                {
                    foreach (string ab in admin.storagecount)
                    {
                        admin.message+=ab;
                    }

                    // AddCount.show15minites();
                    if (admin.message != null)
                    {
                        await admin.context.SendActivity($"Wellcom:" +
                    $"{admin.message}");
                    }
                    
                    //new Welcome_View.WelcomBot();
                    admin.count = 0;
                    admin.message = null;
                    Array.Clear(admin.storagecount,0,admin.storagecount.Length);
                    //string str = JsonConvert.SerializeObject(admin);
                    copy[conversationId]=admin;

                }
            }    
        }
    }
    
}
