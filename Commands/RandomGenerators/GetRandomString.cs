using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.RandomGenerators
{
    [Cmdlet(VerbsCommon.Get, Nouns.RandomString)]
    public class GetRandomString : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int MinLength { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public int MaxLength { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(Common.Random.RandomString(MinLength, MaxLength));
        }
    }
}
