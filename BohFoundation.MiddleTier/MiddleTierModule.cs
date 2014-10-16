using BohFoundation.MiddleTier.ApplicantsOrchestration.Implementations;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Implementations.Helpers;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces.Helpers;
using BohFoundation.MiddleTier.ReferencesOrchestration.Implementations;
using BohFoundation.MiddleTier.ReferencesOrchestration.Implementations.Helpers;
using BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces;
using BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces.Helpers.BohFoundation.ReferencesRepository.OrchestrationLayer.Interfaces.Helper;
using Ninject.Modules;

namespace BohFoundation.MiddleTier
{
    public class MiddleTierModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicantReferenceOrchestration>().To<ApplicantReferenceOrchestration>();
            Bind<ICreateEmailBodyForApplicantReferenceRequest>().To<CreateEmailBodyForApplicantReferenceRequest>();
            Bind<IFinalizeApplicationOrchestration>().To<FinalizeApplicationOrchestration>();

            Bind<IGenerateLetterOfRecommendationKey>().To<GenerateLetterOfRecommendationKey>();
            Bind<IAnonymousLetterOfRecommendationOrchestration>().To<AnonymousLetterOfRecommendationOrchestration>();
        }
    }
}
