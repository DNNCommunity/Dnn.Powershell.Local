using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.RandomGenerators
{
    [Cmdlet(VerbsCommon.Get, Nouns.RandomNumber)]
    public class GetRandomNumber : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int MinValue { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public int MaxValue { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(Common.Random.RandomNumber(MinValue, MaxValue));
        }
    }
}
