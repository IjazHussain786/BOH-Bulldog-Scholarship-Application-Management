using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using BrockAllen.MembershipReboot.WebHost;

namespace BohFoundation.MembershipProvider.UserManagement
{
    public class MembershipRebootConfig
    {
        public static MembershipRebootConfiguration<RelationalUserAccount> Create(bool requireAccountVerification)
        {

            var config = new MembershipRebootConfiguration<RelationalUserAccount> { RequireAccountVerification = requireAccountVerification, AllowAccountDeletion = true, EmailIsUsername = true};

            //config.AddEventHandler(new DebuggerEventHandler());

            var appinfo = new AspNetApplicationInformation("Your Website Address", "Signature of your emails",
                "UserAccount/Login",
                "UserAccount/ChangeEmail/Confirm/",
                "UserAccount/ChangeEmail/Cancel/",
                "UserAccount/ResetPassword/Confirm/");

            var emailFormatter = new EmailMessageFormatter<RelationalUserAccount>(appinfo);

            // uncomment if you want email notifications -- also update smtp settings in web.config

            config.AddEventHandler(new EmailAccountEventsHandler<RelationalUserAccount>(emailFormatter));

            //config.AddEventHandler(new TwilloSmsEventHandler(appinfo));
            // uncomment to ensure proper password complexity
            //config.ConfigurePasswordComplexity();

            return config;
        }
    }
}
