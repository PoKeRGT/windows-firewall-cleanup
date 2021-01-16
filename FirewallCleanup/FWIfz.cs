using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using NetFwTypeLib;

namespace FirewallCleanup
{
   internal class FWIfz
   {
      private readonly string pattern = @"\b[c-zC-Z]:\\";

      internal FWIfz()
      {
         uint counter = 0;
         Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
         INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
         INetFwRules Rules = fwPolicy2.Rules;
         IEnumerator rulesEnumerator = Rules.GetEnumerator();

         foreach (INetFwRule rule in Rules)
         {
            string exePath = rule.ApplicationName;
            if (!string.IsNullOrEmpty(exePath))
            {
               if (rule.Action == 0)
               {
                  if (!File.Exists(exePath))
                  {
                     if (Regex.IsMatch(exePath, pattern))
                     {
                        counter++;
                        Console.WriteLine(exePath);
                        fwPolicy2.Rules.Remove(rule.Name);
                     }
                  }
               }
            }
         }

         Console.WriteLine("-");

         if (counter > 0)
         {
            Console.WriteLine(counter + " rule(s) deleted.");
         }
         else
         {
            Console.WriteLine("Nothing found.");
         }

         Console.WriteLine("Press any key to exit...");
         Console.ReadKey();
         Environment.ExitCode = 0;
      }
   }
}
