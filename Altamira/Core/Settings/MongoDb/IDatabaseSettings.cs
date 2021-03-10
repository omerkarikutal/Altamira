using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Settings.MongoDb
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
