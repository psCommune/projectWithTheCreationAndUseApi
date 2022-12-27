using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SuperBank.DataTransfer
{
    public class QuestionDTO
    {
        // JsonPropertyName позволяет связать имя свойства JSON
        // с заданным свойством C#
        [JsonPropertyName("manager")]
        public int? Window { get; set; }

        [JsonPropertyName("code")]
        public string QuestionCode { get; set; }

        // свойство не участвует в десериализации/сериализации
        [JsonIgnore]
        public bool IsUpdated { get; set; }
    }
}
