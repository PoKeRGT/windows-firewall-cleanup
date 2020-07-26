using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using NetFwTypeLib;

namespace FirewallCleanup
{
    class FWIfz
    {
        String pattern = @"\b[c-zC-Z]:\\";

        public FWIfz()
        {
            int counter = 0;
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
            INetFwRules Rules = fwPolicy2.Rules;
            IEnumerator rulesEnumerator = Rules.GetEnumerator();
            foreach(INetFwRule rule in Rules)
            {
                String exePath = rule.ApplicationName as string;
                if (exePath != null && exePath.Length > 0)
                {
                    if (rule.Action == 0)
                    {
                        if (!File.Exists(exePath))
                        {
                            if (Regex.IsMatch(exePath, pattern))
                            {
                                counter += 1;
                                Console.WriteLine(exePath);
                                fwPolicy2.Rules.Remove(rule.Name);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("-");
            Console.WriteLine(counter + " rules deleted.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
