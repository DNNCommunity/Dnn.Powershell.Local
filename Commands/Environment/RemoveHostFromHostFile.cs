using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis"></para>
    /// <para type="description"></para>
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, Nouns.HostFromHostFile)]
    public class RemoveHostFromHostFile : DNNCmdLet
    {
        /// <summary>
        /// <para type="description"></para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string HostName { get; set; }

        protected override void ProcessRecord()
        {
            var hostFile = new HostsFile();
            hostFile.RemoveHost(HostName);
            hostFile.Save();
        }
    }
}
