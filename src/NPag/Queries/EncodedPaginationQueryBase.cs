using System.Text;
using Microsoft.IdentityModel.Tokens;
using NPag.Settings;

namespace NPag.Queries
{
    public abstract class EncodedPaginationQueryBase : IPaginationQuery
    {
        private string _where = "";
        private string _orderBy = "";
        
        public virtual string Where
        {
            get => _where;
            set => _where = Base64UrlEncoder.Decode(value);
        }

        public virtual string OrderBy
        {
            get => _orderBy;
            set => _orderBy = Base64UrlEncoder.Decode(value);
        }

        public virtual int Page { get; set; }
        public virtual int PageSize { get; set; } = PaginationSettings.DefaultPageSize;

        public EncodedPaginationQueryBase()
        {
        }
        
        private EncodedPaginationQueryBase(
            string where = null, 
            string orderBy = null, 
            int page = default,
            int pageSize = default)
        {
            _where = where;
            _orderBy = orderBy;
            
            Page = page;
            PageSize = pageSize;
        }

        public string ToUri()
        {
            var sb = new StringBuilder("?");

            if (!string.IsNullOrEmpty(_where))
                sb.Append($"{nameof(Where)}={Base64UrlEncoder.Encode(_where)}&");
            
            if (!string.IsNullOrEmpty(_orderBy))
                sb.Append($"{nameof(OrderBy)}={Base64UrlEncoder.Encode(_where)}&");
            
            if (Page != default(int))
                sb.Append($"{nameof(Page)}={Page}&");
            
            if (PageSize != default(int))
                sb.Append($"{nameof(PageSize)}={PageSize}&");

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}