using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SuperBank.DataTransfer
{
    internal class QuestionsLoader
    {
        private string uri;
        public QuestionsLoader(string questionsMethodUri)
        {
            uri = questionsMethodUri;
        }

        public List<QuestionDTO> Load()
        {
            using (var client = new HttpClient())
            {
                return client.GetFromJsonAsync<List<QuestionDTO>>(uri).Result;
            }
        }
    }
}
