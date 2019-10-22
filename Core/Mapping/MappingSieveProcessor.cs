using Data.Models;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Core.Mapping
{
    public class MappingSieveProcessor : SieveProcessor
    {
        public MappingSieveProcessor(IOptions<SieveOptions> options)
            : base(options)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<User>(user => user.Id)
                .CanFilter();
            mapper.Property<User>(user => user.Name)
                .CanSort()
                .CanFilter();
            mapper.Property<User>(user => user.Ads.Count)
                .CanSort()
                .CanFilter()
                .HasName("adscount");

            mapper.Property<Ad>(ad => ad.Rating)
                .CanSort()
                .CanFilter();
            mapper.Property<Ad>(ad => ad.UserId)
                .CanSort()
                .CanFilter();
            mapper.Property<Ad>(ad => ad.CreationDate)
                .CanSort()
                .CanFilter()
                .HasName("created");
            mapper.Property<Ad>(ad => ad.Content)
                .CanFilter();

            return mapper;
        }
    }
}