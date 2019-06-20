using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Adds a host to the hosts file on this computer</para>
    /// <para type="description">Add a host to the hosts file potentially overwriting
    /// a previous reference to the specified host</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.HostToHostFile)]
    public class AddHostToHostFile : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">Host to add</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string HostName { get; set; }

        /// <summary>
        /// <para type="description">IP address of this host</para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string IpAddress { get; set; }

        /// <summary>
        /// <para type="description">Comment to add for this entry to the hosts file</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Comment { get; set; } = "";

        protected override void ProcessRecord()
        {
            var hostFile = new HostsFile();
            hostFile.AddHost(HostName, IpAddress, Comment);
            hostFile.Save();
        }
    }
}
