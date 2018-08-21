using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace LdapConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<UserAdDTO> lstADUsers = new List<UserAdDTO>();

            var domainPath = string.Format("WinNT://{0},computer", Environment.MachineName);
            Console.WriteLine("Domain path = " + domainPath);

            //string domainPath = "LDAP://DC=xxxx,DC=com";

            DirectoryEntry searchRoot = new DirectoryEntry(domainPath);
            DirectorySearcher search = new DirectorySearcher(searchRoot);
            search.Filter = "(&(objectClass=user)(objectCategory=person))";
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("usergroup");
            search.PropertiesToLoad.Add("displayname");//first name
            SearchResult result;
            SearchResultCollection resultCol = search.FindAll();

            if (resultCol != null)
            {
                for (int counter = 0; counter < resultCol.Count; counter++)
                {
                    string UserNameEmailString = string.Empty;
                    result = resultCol[counter];
                    if (result.Properties.Contains("samaccountname") &&
                             result.Properties.Contains("mail") &&
                        result.Properties.Contains("displayname"))
                    {
                        UserAdDTO objSurveyUsers = new UserAdDTO();
                        objSurveyUsers.Email = (String)result.Properties["mail"][0] +
                          "^" + (String)result.Properties["displayname"][0];
                        objSurveyUsers.UserName = (String)result.Properties["samaccountname"][0];
                        objSurveyUsers.DisplayName = (String)result.Properties["displayname"][0];
                        lstADUsers.Add(objSurveyUsers);
                    }
                }
            }
        }
    }
}
