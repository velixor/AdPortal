using AdPortalApi.Models;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace AdPortalApi.Mapping
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(
            IOptions<SieveOptions> options)
            : base(options)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            ConfigureAd(mapper);
            ConfigureUser(mapper);

            return mapper;
        }

        private static void ConfigureUser(SievePropertyMapper mapper)
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
        }

        private static void ConfigureAd(SievePropertyMapper mapper)
        {
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
        }
    }
}