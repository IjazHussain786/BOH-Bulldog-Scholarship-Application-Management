using BohFoundation.AdminsRepository.Repositories.Implementation;
using BohFoundation.AdminsRepository.Repositories.Interfaces;
using BohFoundation.ApplicantsRepository.Repositories.Implementations;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.AzureStorage.BlobStorageStreamProvider;
using BohFoundation.AzureStorage.BlobStorageStreamProvider.Interfaces;
using BohFoundation.AzureStorage.TableStorage.Implementations.Essay;
using BohFoundation.AzureStorage.TableStorage.Implementations.LettersOfRecommendation;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay;
using BohFoundation.AzureStorage.TableStorage.Interfaces.LettersOfRecommendation;
using BohFoundation.MembershipProvider.Repositories.Interfaces;
using BohFoundation.MembershipProvider.Repositories.Repos;
using BohFoundation.MembershipProvider.UserManagement;
using BohFoundation.PersonsRepository.Repositories.Implementations;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BohFoundation.ReferencesRepository.Repositories.Implementations;
using BohFoundation.ReferencesRepository.Repositories.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using Ninject.Modules;

namespace BohFoundation.Infrastructure.DI
{
    public class RepositoryModule : NinjectModule
    {
        private readonly string _dbConnection;
        private readonly string _azureConnectionString;
        private readonly bool _production;
        
        public RepositoryModule(string dbConnection, string azureConnectionString, bool production)
        {
            _dbConnection = dbConnection;
            _azureConnectionString = azureConnectionString;
            _production = production;
        }

        public override void Load()
        {
            Bind<IUserAccountRepository<RelationalUserAccount>>()
                .To<UserAccountRepository>()
                .WithConstructorArgument("nameOfConnection", _dbConnection);

            Bind<IMembershipRebootCustomQueries>()
                .To<MembershipRebootCustomQueries>()
                .WithConstructorArgument("nameOfConnection", _dbConnection);

            Bind<ICreatePersonRepository>()
                .To<CreatePersonRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IPersonalInformationRepository>()
                .To<PersonalInformationRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IApplicantsNotificationRepository>()
                .To<ApplicantsesNotificationRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IContactInformationRepository>()
                .To<ContactInformationRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<ITranscriptReferenceRepository>()
                .To<TranscriptReferenceRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IAcademicInformationRepository>()
                .To<AcademicInformationRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<ILowGradeInformationRepository>()
                .To<LowGradeInformationRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IEditEssayTopicRepository>()
                .To<EditEssayTopicRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IApplicantsEssayRepository>()
                .To<ApplicantsEssayRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IApplicantsReferencesRepository>()
                .To<ApplicantsReferencesRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IAnonymousLetterOfRecommendationRepository>()
                .To<AnonymousLetterOfRecommendationRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<ILetterOfRecommendationRowValueSqlRepository>()
                .To<AnonymousLetterOfRecommendationRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IFamilyInformationRepository>()
                .To<FamilyInformationRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IExtracurricularActivitiesRepository>()
                .To<ExtracurricularActivitiesRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IApplicantMetadataRepository>()
                .To<ApplicantMetadataRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IGetLetterOfRecommendationGuidRepository>()
                .To<GetLetterOfRecommendationGuidRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IChangeEmailRepository>()
                .To<ChangeEmailRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IGetListOfFinalizedApplicantsRepository>()
                .To<GetListOfFinalizedApplicantsRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IGetCompletedApplicationRepository>()
                .To<GetCompletedApplicationRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IRateApplicationRespository>()
                .To<RateApplicationRespository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IConfirmTranscriptRepository>()
                .To<ConfirmTranscriptRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IGetTopRatedApplicantsRepository>()
                .To<GetTopRatedApplicantsRepository>()
                .WithConstructorArgument("dbConnection", _dbConnection);

            Bind<IBlobHelper>()
                .To<BlobHelper>()
                .WithConstructorArgument("dbConnection", _azureConnectionString);

            Bind<IAzureEssayRepository>()
                .To<AzureEssayRepository>()
                .WithConstructorArgument("dbConnection", _azureConnectionString);

            Bind<IAzureLettersOfRecommendationRepository>()
                .To<AzureLettersOfRecommendationRepository>()
                .WithConstructorArgument("dbConnection", _azureConnectionString);


            var config = MembershipRebootConfig.Create(_production);

            Bind<MembershipRebootConfiguration<RelationalUserAccount>>().ToConstant(config);
        }
    }
}
