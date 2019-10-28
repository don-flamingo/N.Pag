using NPag.Settings;

namespace NPag
{
    public class NPagConfiguration
    {
        public NPagConfiguration SetDefaultPageSize(int defaultPageSize)
        {
            PaginationSettings.DefaultPageSize = defaultPageSize;
            return this;
        }
        
        public NPagConfiguration SetMaxPageSize(int maxPageSize)
        {
            PaginationSettings.MaxPageSize = maxPageSize;
            return this;
        }
        
        public static NPagConfiguration DoIt()
        {
            return new NPagConfiguration();
        }
    }
}