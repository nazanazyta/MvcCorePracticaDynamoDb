using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCorePracticaDynamoDb.Services
{
    public class ServiceS3
    {
        private String bucket;
        private IAmazonS3 client;
        private String url;

        public ServiceS3(IAmazonS3 client, IConfiguration configuration)
        {
            this.client = client;
            this.bucket = configuration["AWSS3:bucketname"];
            this.url = configuration["AWSS3:urlbucket"];
        }

        public async Task<bool> UploadFile(Stream stream, String filename)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,
                Key = filename,
                BucketName = this.bucket
            };
            PutObjectResponse response = await this.client.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        //public async Task<List<String>> GetFileNames()
        //{
        //    ListVersionsResponse response = await this.client.ListVersionsAsync(this.bucket);
        //    return response.Versions.Select(x => x.Key).ToList();
        //}

        public async Task<bool> DeleteFile(String filename)
        {
            DeleteObjectResponse response = await this.client.DeleteObjectAsync(this.bucket, filename);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                return true;
            return false;
        }

        public async Task<Stream> GetFile(String filename)
        {
            GetObjectResponse response = await this.client.GetObjectAsync(this.bucket, filename);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return response.ResponseStream;
            return null;
        }

        public String GetUrlFile(String filename)
        {
            return this.url + filename;
        }
    }
}
