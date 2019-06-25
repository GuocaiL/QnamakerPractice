using System;
using Microsoft.Bot.Builder;

namespace Model_BotStateSetings_AdministratorSetings
{
    public class AdministratorSetings
    {
        //AddCount
        public string[] storagecount { get; set; } = new string[10];
        public int count {get;set;}=0;
        public string message {get;set;}=null;       

        //Administrator
        public bool judgesetadministrator {get;set;}=false;
        public bool judgeresponse {get;set;} = false;
        public string id {get;set;} = null;
        public string name {get;set;} = null;

        //GetTimeAndJudge
        public string greeting {get;set;} ="8:00,早上好！";
        public DateTimeOffset settime {get;set;}
        public int IntervalInSeconds {get;set;} = 86400;                
        public int hours {get;set;} = 8;
        public int minute {get;set;} = 00;
        public int seconds {get;set;} = 00;

        public ITurnContext context {get;set;}=null;
    }
}
