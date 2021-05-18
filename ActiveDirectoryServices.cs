using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace ADcontrol
{
    class ActiveDirectoryServices
    {
        public static DirectoryEntry CreateDirectoryEntry()
        {
            // create and return new LDAP connection with desired settings  
            DirectoryEntry ldapConnection = new DirectoryEntry();
            string adLogin = Environment.GetEnvironmentVariable("AD_LOGIN", EnvironmentVariableTarget.User);
            string adPassword = Environment.GetEnvironmentVariable("AD_PASS", EnvironmentVariableTarget.User);
            string adDomain = Environment.GetEnvironmentVariable("AD_DOMAIN", EnvironmentVariableTarget.User);
            string adOU = Environment.GetEnvironmentVariable("AD_OU", EnvironmentVariableTarget.User);


            String[] domainNameParts = adDomain.Split('.');
            StringBuilder domainNamePartsFormatted = new StringBuilder();
            foreach (string part in domainNameParts)
            {
                domainNamePartsFormatted.Append(String.Format("DC={0},", part));
            }
            domainNamePartsFormatted.Length--; // remove last comma sign

            ldapConnection.Username = String.Format("{0}@{1}", adLogin, adDomain);
            ldapConnection.Password = adPassword;
            ldapConnection.Path = String.Format("LDAP://OU={0},{1}", adOU, domainNamePartsFormatted.ToString());
            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;

            return ldapConnection;
        }

        public static List<string> GetADComputersNames(DirectoryEntry directoryEntry)
        {
            List<string> computerNames = new List<string>();

            using (DirectorySearcher adSearcher = new DirectorySearcher(directoryEntry))
            {
                adSearcher.Filter = ("(objectClass=computer)");

                // No size limit, reads all objects
                adSearcher.SizeLimit = 0;

                // Read data in pages of n objects. Make sure this value is below the limit configured in your AD domain (if there is a limit)
                adSearcher.PageSize = 100;

                // Let searcher know which properties are going to be used, and only load those
                adSearcher.PropertiesToLoad.Add("name");

                foreach (SearchResult adEntry in adSearcher.FindAll())
                {
                    // Note: Properties can contain multiple values.
                    if (adEntry.Properties["name"].Count > 0)
                    {
                        string computerName = (string)adEntry.Properties["name"][0];
                        computerNames.Add(computerName);
                    }
                }
            }
            return computerNames;
        }
    }
}
