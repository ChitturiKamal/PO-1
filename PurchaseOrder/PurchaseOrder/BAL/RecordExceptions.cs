using PurchaseOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrder.BAL
{
    public class RecordExceptions
    {//TETechuvaDBContext
        TETechuvaDBContext1 context = new TETechuvaDBContext1();
        public void RecordUnHandledException(Exception ex)
        {
            ApplicationErrorLog errorlog = new ApplicationErrorLog();
            if (ex.InnerException != null)
                errorlog.InnerException = ex.InnerException.ToString();
            if (ex.StackTrace != null)
                errorlog.Stacktrace = ex.StackTrace.ToString();
            if (ex.Source != null)
                errorlog.Source = ex.Source.ToString();
            if (ex.Message != null)
                errorlog.Error = ex.Message.ToString();
            errorlog.ExceptionDateTime = DateTime.Now;
            context.ApplicationErrorLogs.Add(errorlog);
            context.SaveChanges();
        }
    }
    public class JsonContent : HttpContent
    {
        private readonly MemoryStream _Stream = new MemoryStream();

        public JsonContent(object value)
        {

            Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var jw = new JsonTextWriter(new StreamWriter(_Stream));

            jw.Formatting = Formatting.None;

            var serializer = new JsonSerializer();

            serializer.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            serializer.Serialize(jw, value);

            jw.Flush();

            _Stream.Position = 0;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return _Stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _Stream.Length;
            return true;
        }

    }
}