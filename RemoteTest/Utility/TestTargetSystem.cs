#region (C) Copyright 2023 Philips Medical Systems Nederland B.V.
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.
//
#endregion

namespace Utility
{
    public static class TestTargetSystem
    {
        public static string GetTargetIp()
        {
            var ip = NUnit.Framework.TestContext.Parameters.Get("TargetSystemIP");
            return !string.IsNullOrEmpty(ip) ? ip : "10.0.0.2"; //IP value which is in config file
        }
    }
}