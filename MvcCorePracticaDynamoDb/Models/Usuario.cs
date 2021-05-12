using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCorePracticaDynamoDb.Models
{
    [DynamoDBTable("usuarios")]
    public class Usuario
    {
        [DynamoDBProperty("idusuario")]
        public int IdUsuario { get; set; }
        [DynamoDBProperty("nombre")]
        public String Nombre { get; set; }
        [DynamoDBProperty("descripcion")]
        public String Descripcion { get; set; }
        [DynamoDBProperty("fechaalta")]
        public String FechaAlta { get; set; }
        [DynamoDBProperty("fotos")]
        public List<Foto> Fotos { get; set; }
    }
}
