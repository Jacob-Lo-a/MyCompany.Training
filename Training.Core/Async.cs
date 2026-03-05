using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core
{
    public class Async
    {
        public async Task<string> DownloadDataAsync(int id, CancellationToken cancellationToken)
        {

            await Task.Delay(2000, cancellationToken);

            return $"Data {id} downloaded at {DateTime.Now}";
        }
    }
}
