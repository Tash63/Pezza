namespace DataAcess.Filters
{
  public static class SideFilter
    {
        public static IQueryable<Side> FilterByName(this IQueryable<Side> query,string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return query;
            }
            return query.Where(x => x.Name == name);
        }

        public static IQueryable<Side> FilterByInStock(this IQueryable<Side> query,bool? instock)
        {
            if(!instock.HasValue)
            {
                return query;
            }
            return query.Where(x=>x.InStock==instock);
        }
    }
}
