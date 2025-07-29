using System;
using Utility;

public class RemoteBatchExecutionTest
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting remote batch execution test...");

        var tester = new RemoteBatchExecutionTest();
        tester.TestAllVariants();

        Console.WriteLine("All tests finished.");
    }

    public void TestAllVariants()
    {
        var helper = new RemoteAccessToolHelper();
        string remotePathRaw = @"I:\demo_script.bat";
        string remotePathQuoted = $"\"{remotePathRaw}\"";
        int waitTime = 60;

       // TestVariant(helper, "Raw path, no args", remotePathRaw, waitTime, "");
        // TestVariant(helper, "Quoted path, no args", remotePathQuoted, waitTime, "");

        // NEW: Test running the batch file via cmd.exe
        Console.WriteLine("Testing batch file execution via cmd.exe...");
        bool resultCmd = helper.StartProcessAndWait("cmd.exe", waitTime, "/c " + remotePathQuoted);
        Console.WriteLine("Batch file execution via cmd.exe result: " + resultCmd);
    }

    private void TestVariant(RemoteAccessToolHelper helper, string variant, string path, int wait, string args)
    {
        try
        {
            Console.WriteLine($"Testing: {variant}");
            bool result = helper.StartProcessAndWait(path, wait, args);
            Console.WriteLine($"Result for {variant}: {(result ? "Success" : "Failure")}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception for {variant}: {ex.Message}\n");
        }
    }
}