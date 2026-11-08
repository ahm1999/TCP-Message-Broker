using MessageBroker.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessageBroker.Utils
{

    public record RequesParserDTO(List<string> Messages, List<string> ErrorMessages, string Queue  = "No Queue", RequestMethods Method = RequestMethods.Empty);

    public static class RequestParser
    {
        

        public static RequesParserDTO Handle (string message) {

            
            //await JsonSerializer.DeserializeAsync<RequesParserDTO>(message);

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };


            var response = JsonSerializer.Deserialize<RequesParserDTO>(message,options);


            if (response is null) {



                response = new RequesParserDTO(new List<string>(), new List<string>());
                response.ErrorMessages.Add("Failed to Desrialize the message");



                return response;
            }



           // if (response.Messages is null ) { response.ErrorMessages.Add("No messages added");   }
            if (response.Method == RequestMethods.Empty) { response.ErrorMessages.Add("No Method Specified");   }
            if (response.Queue == "No Queue") { response.ErrorMessages.Add("No Queue Specified");   }

            //if (response. == RequestMethods.Empty) { response.ErrorMessages.Add("No Method Specified");   }

            return response;

        }


        


    }



}










//public static RequesParserDTO HandleRequest(string message) {

//    RequestMethods requestMethod = RequestMethods.Empty;
//    List<string>? Messages = null;
//    string QueueName;


//    List<string> ErrorMessages = new();



//    message = message.Trim();

//    string[] parts = message.Split(",");

//    //string queuepart = parts[0];
//    //string methodpart = parts[1];
//    //string messagespart = parts[2];


//    foreach (var part in parts)
//    {

//        string title =part.Split(":")[0].Trim().ToLower();


//        switch (title)
//        {
//            case "queue":
//                QueueName = HandleQueuePart(part, ref ErrorMessages);
//                break;

//            case "method":
//                requestMethod = HandleMethodPart(part,ref ErrorMessages);
//                break;

//            case "messages":
//                Messages= HandleMessagesPart(part, ref ErrorMessages);
//                break;


//            default:

//                break;


//        }




//    }



//    // handle Q


//    //string QueueName = QueuePart.Split(":")[1].Trim().ToLower();
//    //string Method = MethodPart.Split(":")[1].Trim().ToLower();


//    // var reqTypes = new RequestTypes();



//    //parsing messeges 






//    return new RequesParserDTO(Messages, requestMethod, ErrorMessages);


//}

//private static List<string> HandleMessagesPart(string part, ref List<string> errorMessages)
//{
//    string Messages = part.Split(":")[1].Trim().ToLower();

//    Messages = Messages.Trim('[', ']').Trim();


//    List<string> MessagesList = Messages.Split(",").ToList();

//    return MessagesList; 
//}

//private static RequestMethods HandleMethodPart(string part, ref List<string> ErrorMessages)
//{

//    RequestMethods method = RequestMethods.Empty;

//    string  Method = part.Split(":")[1];



//    bool IsAcceptableMethod = RequestMethodsConsts.Consts.Any(c => c == Method);

//    if (!IsAcceptableMethod)
//    {
//        ErrorMessages.Add("No Acceptable Method provided");

//    }
//    else
//    {

//        bool IsParsed = Enum.TryParse<RequestMethods>(Method, true, out method);



//        if (!IsParsed) ErrorMessages.Add("Method could't be parsed");


//    }


//    return method;
//}

//private static string HandleQueuePart(string part, ref List<string> errorMessages)
//{


//    part.Split(":");

//}