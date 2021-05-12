using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MvcCorePracticaDynamoDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCorePracticaDynamoDb.Services
{
    public class ServiceDynamoDB
    {
        public DynamoDBContext context;

        public ServiceDynamoDB()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            this.context = new DynamoDBContext(client);
        }

        public async Task CreateUsuario(Usuario u)
        {
            await this.context.SaveAsync<Usuario>(u);
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var tabla = this.context.GetTargetTable<Usuario>();
            var scanoptions = new ScanOperationConfig();
            var result = tabla.Scan(scanoptions);
            List<Document> data = await result.GetNextSetAsync();
            return this.context.FromDocuments<Usuario>(data).ToList();
        }

        public async Task<Usuario> GetUsuario(int idusuario)
        {
            return await this.context.LoadAsync<Usuario>(idusuario);
        }

        //public async Task Edit(Usuario u)
        //{
        //    await this.context.SaveAsync<Usuario>(u);
        //}

        //public async Task Update(Usuario usuario)
        //{
        //    if (usuario.Fotos.Count > 0 || usuario.Fotos != null)
        //    {
        //        usuario.Fotos = usuario.Fotos;
        //    }
        //    await this.context.SaveAsync<Usuario>(usuario);
        //}

        public async Task DeleteUsuario(int idusuario)
        {
            await this.context.DeleteAsync<Usuario>(idusuario);
        }
    }
}
