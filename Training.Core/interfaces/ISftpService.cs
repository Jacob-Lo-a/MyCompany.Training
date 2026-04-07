using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core.interfaces
{
    public interface ISftpService
    {
        Task UploadReportAsync(byte[] fileData, string remoteFileName);
        Task UploadFileAsync(IFormFile file);
    }
}