using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Noodle.Helpers;
using Noodle.Models.Dictionary;
using RestSharp;

namespace Noodle.Core
{
    public class Define
    {
        const string IndentReponse = "  ";
        public void TextInputLoop()
        {
            //endless loop
            int i = -1;
            while (i == -1)
            {
                Console.WriteLine("Enter input:"); // Prompt
                string line = Console.ReadLine(); // Get string from user
                if (line == "exit") // Check string
                {
                    break;
                }

                //lookup word for spell check
                //http://dictionary-lookup.org/disgusting
                var client = new RestClient("http://dictionary-lookup.org/");
                var request = new RestRequest("{word}", Method.POST);
                request.AddUrlSegment("word", line); // replaces matching token in request.Resource

                // or automatically deserialize result
                // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
                RestResponse<DictionaryLookupModel.RootObject> response = (RestResponse<DictionaryLookupModel.RootObject>)client.Execute<DictionaryLookupModel.RootObject>(request);

                if (response.Data.meanings != null)
                {
                    //use other dictionary service
                    //https://glosbe.com/gapi/translate?from=eng&dest=eng&format=json&phrase=disgusting&pretty=true
                    //http://dictionary-lookup.org/disgusting
                    var client2 = new RestClient("https://glosbe.com/");
                    var request2 = new RestRequest("gapi/translate?from=eng&dest=eng&format=json&phrase={word}&pretty=true", Method.GET);
                    request2.AddUrlSegment("word", line); // replaces matching token in request.Resource

                    // or automatically deserialize result
                    // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
                    var response2 = (RestResponse<GlosbeResponseJsonModel.RootObject>)client2.Execute<GlosbeResponseJsonModel.RootObject>(request2);

                    if (response2.Data != null)
                    {
                        var responseList = new List<string>();
                        foreach (var tuc in response2.Data.tuc)
                        {
                            if (tuc.meanings != null)
                            {
                                foreach (var meaning in tuc.meanings)
                                {

                                    responseList.Add(meaning.text);
                                }
                            }
                        }

                        var listDeDupedResponses = responseList.Distinct().ToList();
                        foreach (var listDeDupedResponse in listDeDupedResponses)
                        {
                            Console.WriteLine(IndentReponse + HtmlHelper.StripHtml(System.Web.HttpUtility.HtmlDecode(listDeDupedResponse)));
                        }
                    }
                    else
                    {
                        //use meaning from 1st dictionary although it has embedded html tags 
                        // EXAMPLE: Console.Write(response.Data.meanings[0].content);
                        foreach (var meaning in response.Data.meanings)
                        {
                            Console.WriteLine(IndentReponse + HtmlHelper.StripHtml(System.Web.HttpUtility.HtmlDecode(meaning.content)));
                        }
                    }
                }
                else
                {
                    //SUGGESTIONS
                    //deserialize json object
                    var responseSuggestion = JsonConvert.DeserializeObject<DictionaryLookupSuggestionsModel.RootObject>(response.Content);

                    if (responseSuggestion.suggestions != null)
                    {
                        Console.WriteLine(IndentReponse + "Did you mean {0}?", string.Join(",", responseSuggestion.suggestions));
                    }
                    else
                    {
                        Console.WriteLine(IndentReponse + "Sorry, I did not understand your input '{0}'.", line);
                    }
                }

                //Console.Write("You typed "); // Report output
                //Console.Write(line.Length);
                //Console.WriteLine(" character(s)");
            }
        }
    }
}
