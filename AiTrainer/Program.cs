using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestAiTrainer")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace AiTrainer
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = AiTrainerFactory.CreateAiTrainer(Games.Driving2D))
                game.Run();
        }
    }
}
