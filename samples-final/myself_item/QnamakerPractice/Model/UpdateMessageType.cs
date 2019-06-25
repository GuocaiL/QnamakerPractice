
using System;

namespace Model_UpdateMessageType
{
    //The class a method that be used to get update massage that has a fixed format;
    public class UpdateMessageType
    {
       private  static string first = null;

       private  static string second = null;        
        
       private static string  new_kbPart1 = @"
{
  'add': {
    'qnaList': [
      {
        'id': 1,
        'answer': '";

        private static string new_kbPart2 = @"',
        'source': 'Custom Editorial',
        'questions': [
          '";

        private static string new_kbPart3 =@"'
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
        public static bool TryParseAddQA(string message, out string kb,out string NEW_KB)
        {
            var succeeded = false;
            NEW_KB = null;
            kb = "0586beac-69d9-4dc9-8f7f-756707fa1bee";
            try
            {           
                if (message.Contains("::"))
                {
                    string[] strs = message.Split("::");                  
                    first = strs[0];
                    second = strs[1];
                    succeeded = true;
                    
                }
                NEW_KB = new_kbPart1 + second + new_kbPart2 + first + new_kbPart3;
            }
            catch (Exception e) { }  
            return succeeded;
        }
    }
}
