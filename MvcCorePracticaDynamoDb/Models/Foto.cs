using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCorePracticaDynamoDb.Models
{
    public class Foto
    {
        [DynamoDBProperty("titulo")]
        public String Titulo { get; set; }
        [DynamoDBProperty("imagen")]
        public String Imagen { get; set; }
    }
}
