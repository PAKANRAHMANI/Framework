using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.FileUpload.Minio
{
    public sealed record MinioConfiguration
    {
        public string Endpoint { get; init; }
        public string AccessKey { get; init; }
        public string SecretKey { get; init; }
        public bool UseSSL { get; init; }
        public int TimeOut { get; init; }
    }
}
