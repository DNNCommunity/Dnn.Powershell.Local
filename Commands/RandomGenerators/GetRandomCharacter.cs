using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.RandomGenerators
{
    [Cmdlet(VerbsCommon.Get, Nouns.RandomCharacter)]
    public class GetRandomCharacter : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int MinCharCode { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public int MaxCharCode { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(Common.Random.RandomCharacter(MinCharCode, MaxCharCode));
        }
    }
}
