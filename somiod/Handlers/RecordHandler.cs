using somiod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Handlers
{
    public class RecordHandler
    {
        internal static void AddRecordToDatabase(string application, string container, Record record)
        {
            throw new NotImplementedException();
        }

        internal static void DeleteRecordFromDatabase(string application, string container, string record)
        {
            throw new NotImplementedException();
        }

        internal static Record FindRecordInDatabase(string application, string container, string record)
        {
            throw new NotImplementedException();
        }

        internal static List<Record> FindRecordsByApplication(string application)
        {
            throw new NotImplementedException();
        }

        internal static List<Record> FindRecordsByContainer(string application, string container)
        {
            throw new NotImplementedException();
        }

        internal static bool RecordExists(string application, string container, string record)
        {
            throw new NotImplementedException();
        }
    }
}