using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PO.Models
{
    public class RecordException
    {
        TETechuvaDBContext context = new TETechuvaDBContext();
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
}