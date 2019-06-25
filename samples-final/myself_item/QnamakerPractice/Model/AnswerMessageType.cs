using System;

namespace Model_MessageType
{
    //The class a method that be used to get question that has a fixed format;
    public class AnswerMessageType
    {
        private static string questionPart1 = @"
              {
                   'question': '";
        private static string questiongPart2 = @"',
                   'top': 3
               }
                ";
        private static String question = null;
        public static string getQue(string text)
        {           
            question = questionPart1 + text  + questiongPart2;
            return question;
        }
    }
}
