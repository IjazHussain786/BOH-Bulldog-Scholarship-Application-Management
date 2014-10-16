using System;
using BohFoundation.Domain.Infrastructure;

namespace BohFoundation.TestHelpers
{
    public static class TestHelpersCommonFields
    {
        public static Guid GuidOne { get; set; }
        public static string DatabaseName { get; private set; }

        public static void InitializeFields()
        {
            GuidOne = Guid.NewGuid();
            GuidOneWithNoDashes = GuidOne.ToString().Replace("-", "");
            DatabaseName = IsLocalTest.LocalTest ? "LocalTest" : "CloudTest";
            AzureConnectionName = IsLocalTest.LocalTest ? "AzureStorageLocalTest" : "AzureStorageCloudTest";
        }

        public static string GuidOneWithNoDashes { get; set; }
        public static string AzureConnectionName { get; set; }

        public static string EmailDynamic() {
            return Guid.NewGuid() + "@email.com" ; }

        public static string GetRandomString()
        {
            return Guid.NewGuid().ToString();
        }

        public const string Email = "Email@email.com";
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public static string FullName { get { return FirstName + " " + LastName; } }
        public const string Password = "password";
        public const int GraduatingYear = 2013;
        public const string ExceptionMessage = "error";
        public const int Id = 123;
        public const string EssayTitle = "Are There Hats?";
        public const string EssayPrompt = "Explain: Are hats real?";

        public const string Paragraph =
            "Nullam quis risus eget urna mollis ornare vel eu leo. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam id dolor id nibh ultricies vehicula. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec ullamcorper nulla non metus auctor fringilla. Duis mollis, est non commodo luctus, nisi erat porttitor ligula, eget lacinia odio sem nec elit. Donec ullamcorper nulla non metus auctor fringilla. Maecenas sed diam eget risus varius blandit sit amet non magna. Donec id elit non mi porta gravida at eget metus. Duis mollis, est non commodo luctus, nisi erat porttitor ligula, eget lacinia odio sem nec elit.";
    }
}
