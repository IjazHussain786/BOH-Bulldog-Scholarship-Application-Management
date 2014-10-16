using BohFoundation.ApplicantsRepository;
using BohFoundation.AzureStorage;
using BohFoundation.Domain.Infrastructure;
using BohFoundation.MembershipProvider.Infrastructure;
using BohFoundation.MiddleTier;
using BohFoundation.ReferencesRepository.Infrastructure;
using BohFoundation.Utilities.Infrastructure;
using Ninject.Modules;

namespace BohFoundation.Infrastructure.DI
{
    public class BaseModule
    {
        public INinjectModule[] GetBaseModules()
        {
            INinjectModule[] modules =
            {
                new DomainModule(),
                new MembershipModule(),
                new UtilitiesModule(),
                new AzureStorageModule(), 
                new ApplicantRepositoryModule(), 
                new ReferencesRepositoryModule(),
                new MiddleTierModule(),
            };

            return modules;
        }
    }
}
